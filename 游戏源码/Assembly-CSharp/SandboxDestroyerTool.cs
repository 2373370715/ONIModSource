using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200143F RID: 5183
public class SandboxDestroyerTool : BrushTool
{
	// Token: 0x06006B79 RID: 27513 RVA: 0x000E696A File Offset: 0x000E4B6A
	public static void DestroyInstance()
	{
		SandboxDestroyerTool.instance = null;
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06006B7A RID: 27514 RVA: 0x000E67C3 File Offset: 0x000E49C3
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006B7B RID: 27515 RVA: 0x000E6972 File Offset: 0x000E4B72
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxDestroyerTool.instance = this;
		this.affectFoundation = true;
	}

	// Token: 0x06006B7C RID: 27516 RVA: 0x000E6987 File Offset: 0x000E4B87
	protected override string GetDragSound()
	{
		return "SandboxTool_Delete_Add";
	}

	// Token: 0x06006B7D RID: 27517 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B7E RID: 27518 RVA: 0x000E698E File Offset: 0x000E4B8E
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06006B7F RID: 27519 RVA: 0x000E68B1 File Offset: 0x000E4AB1
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006B80 RID: 27520 RVA: 0x002E2A14 File Offset: 0x002E0C14
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006B81 RID: 27521 RVA: 0x000E69C5 File Offset: 0x000E4BC5
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Delete", false));
	}

	// Token: 0x06006B82 RID: 27522 RVA: 0x000E68CA File Offset: 0x000E4ACA
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006B83 RID: 27523 RVA: 0x002E2ACC File Offset: 0x002E0CCC
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SandBoxTool, 0f, 0f, byte.MaxValue, 0, index);
		HashSetPool<GameObject, SandboxDestroyerTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxDestroyerTool>.Allocate();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			if (Grid.PosToCell(pickupable) == cell)
			{
				pooledHashSet.Add(pickupable.gameObject);
			}
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			if (Grid.PosToCell(buildingComplete) == cell)
			{
				pooledHashSet.Add(buildingComplete.gameObject);
			}
		}
		if (Grid.Objects[cell, 1] != null)
		{
			pooledHashSet.Add(Grid.Objects[cell, 1]);
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.PosToCell(crop) == cell)
			{
				pooledHashSet.Add(crop.gameObject);
			}
		}
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell)
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (Comet comet in Components.Meteors.GetItems((int)Grid.WorldIdx[cell]))
		{
			if (!comet.IsNullOrDestroyed() && Grid.PosToCell(comet) == cell)
			{
				pooledHashSet.Add(comet.gameObject);
			}
		}
		foreach (GameObject original in pooledHashSet)
		{
			Util.KDestroyGameObject(original);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x040050C8 RID: 20680
	public static SandboxDestroyerTool instance;

	// Token: 0x040050C9 RID: 20681
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040050CA RID: 20682
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}
