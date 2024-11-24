using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001447 RID: 5191
public class SandboxSpawnerTool : InterfaceTool
{
	// Token: 0x06006BBA RID: 27578 RVA: 0x000E6CD8 File Offset: 0x000E4ED8
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
	}

	// Token: 0x06006BBB RID: 27579 RVA: 0x000E6CFA File Offset: 0x000E4EFA
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.currentCell = Grid.PosToCell(cursorPos);
	}

	// Token: 0x06006BBC RID: 27580 RVA: 0x000E6D0F File Offset: 0x000E4F0F
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		this.Place(Grid.PosToCell(cursor_pos));
	}

	// Token: 0x06006BBD RID: 27581 RVA: 0x002E37F8 File Offset: 0x002E19F8
	private void Place(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
		GameObject prefab = Assets.GetPrefab(stringSetting);
		if (prefab.HasTag(GameTags.BaseMinion))
		{
			this.SpawnMinion(stringSetting);
		}
		else if (prefab.GetComponent<Building>() != null)
		{
			BuildingDef def = prefab.GetComponent<Building>().Def;
			def.Build(cell, Orientation.Neutral, null, def.DefaultElements(), 298.15f, true, -1f);
		}
		else
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			Grid.SceneLayer sceneLayer = (component == null) ? Grid.SceneLayer.Creatures : component.sceneLayer;
			GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(this.currentCell, sceneLayer), sceneLayer, null, 0);
			if (gameObject.GetComponent<Pickupable>() != null && !gameObject.HasTag(GameTags.Creature))
			{
				gameObject.transform.position += Vector3.up * (Grid.CellSizeInMeters / 3f);
			}
			gameObject.SetActive(true);
		}
		GameUtil.KInstantiate(this.fxPrefab, Grid.CellToPosCCC(this.currentCell, Grid.SceneLayer.FXFront), Grid.SceneLayer.FXFront, null, 0).GetComponent<KAnimControllerBase>().Play("placer", KAnim.PlayMode.Once, 1f, 0f);
		KFMOD.PlayUISound(this.soundPath);
	}

	// Token: 0x06006BBE RID: 27582 RVA: 0x000E6D1D File Offset: 0x000E4F1D
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.entitySelector.row.SetActive(true);
	}

	// Token: 0x06006BBF RID: 27583 RVA: 0x000E6D54 File Offset: 0x000E4F54
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006BC0 RID: 27584 RVA: 0x002E3950 File Offset: 0x002E1B50
	private void SpawnMinion(string prefabID)
	{
		GameObject prefab = Assets.GetPrefab(prefabID);
		Tag model = prefabID;
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(this.currentCell, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		new MinionStartingStats(model, false, null, null, false).Apply(gameObject);
		gameObject.GetMyWorld().SetDupeVisited();
	}

	// Token: 0x06006BC1 RID: 27585 RVA: 0x002E39CC File Offset: 0x002E1BCC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			List<ObjectLayer> list = new List<ObjectLayer>();
			list.Add(ObjectLayer.Pickupables);
			list.Add(ObjectLayer.Plants);
			list.Add(ObjectLayer.Minion);
			list.Add(ObjectLayer.Building);
			if (Grid.IsValidCell(cell))
			{
				foreach (ObjectLayer layer in list)
				{
					GameObject gameObject = Grid.Objects[cell, (int)layer];
					if (gameObject)
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

	// Token: 0x040050E0 RID: 20704
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x040050E1 RID: 20705
	private int currentCell;

	// Token: 0x040050E2 RID: 20706
	private string soundPath = GlobalAssets.GetSound("SandboxTool_Spawner", false);

	// Token: 0x040050E3 RID: 20707
	[SerializeField]
	private GameObject fxPrefab;
}
