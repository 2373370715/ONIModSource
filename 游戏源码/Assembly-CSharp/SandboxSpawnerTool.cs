using System.Collections.Generic;
using UnityEngine;

public class SandboxSpawnerTool : InterfaceTool
{
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	private int currentCell;

	private string soundPath = GlobalAssets.GetSound("SandboxTool_Spawner");

	[SerializeField]
	private GameObject fxPrefab;

	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		colors.Add(new ToolMenu.CellColorData(currentCell, radiusIndicatorColor));
	}

	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		currentCell = Grid.PosToCell(cursorPos);
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		Place(Grid.PosToCell(cursor_pos));
	}

	private void Place(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
		GameObject prefab = Assets.GetPrefab(stringSetting);
		if (stringSetting == MinionConfig.ID)
		{
			SpawnMinion();
		}
		else if (prefab.GetComponent<Building>() != null)
		{
			BuildingDef def = prefab.GetComponent<Building>().Def;
			def.Build(cell, Orientation.Neutral, null, def.DefaultElements(), 298.15f);
		}
		else
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			Grid.SceneLayer sceneLayer = ((component == null) ? Grid.SceneLayer.Creatures : component.sceneLayer);
			GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(currentCell, sceneLayer), sceneLayer);
			if (gameObject.GetComponent<Pickupable>() != null && !gameObject.HasTag(GameTags.Creature))
			{
				gameObject.transform.position += Vector3.up * (Grid.CellSizeInMeters / 3f);
			}
			gameObject.SetActive(value: true);
		}
		GameUtil.KInstantiate(fxPrefab, Grid.CellToPosCCC(currentCell, Grid.SceneLayer.FXFront), Grid.SceneLayer.FXFront).GetComponent<KAnimControllerBase>().Play("placer");
		KFMOD.PlayUISound(soundPath);
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.entitySelector.row.SetActive(value: true);
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: false);
	}

	private void SpawnMinion()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID));
		gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(currentCell, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(value: true);
		new MinionStartingStats(is_starter_minion: false).Apply(gameObject);
		gameObject.GetMyWorld().SetDupeVisited();
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			List<ObjectLayer> list = new List<ObjectLayer>();
			list.Add(ObjectLayer.Pickupables);
			list.Add(ObjectLayer.Plants);
			list.Add(ObjectLayer.Minion);
			list.Add(ObjectLayer.Building);
			if (Grid.IsValidCell(cell))
			{
				foreach (ObjectLayer item in list)
				{
					GameObject gameObject = Grid.Objects[cell, (int)item];
					if ((bool)gameObject)
					{
						SandboxToolParameterMenu.instance.settings.SetStringSetting("SandboxTools.SelectedEntity", gameObject.PrefabID().ToString());
						break;
					}
				}
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}
}
