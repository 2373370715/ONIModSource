using System.Collections.Generic;
using UnityEngine;

public class SandboxCritterTool : BrushTool
{
	public static SandboxCritterTool instance;

	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor");

	public static void DestroyInstance()
	{
		instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		instance = this;
	}

	protected override string GetDragSound()
	{
		return "";
	}

	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(6f);
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: false);
	}

	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int item in cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(item, radiusIndicatorColor));
		}
	}

	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click"));
	}

	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		HashSetPool<GameObject, SandboxCritterTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxCritterTool>.Allocate();
		foreach (Health item in Components.Health.Items)
		{
			if (Grid.PosToCell(item) == cell && item.GetComponent<KPrefabID>().HasTag(GameTags.Creature))
			{
				pooledHashSet.Add(item.gameObject);
			}
		}
		foreach (GameObject item2 in pooledHashSet)
		{
			KFMOD.PlayOneShot(soundPath, item2.gameObject.transform.GetPosition());
			Util.KDestroyGameObject(item2);
		}
		pooledHashSet.Recycle();
	}
}
