﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SandboxDestroyerTool : BrushTool
{
	public static void DestroyInstance()
	{
		SandboxDestroyerTool.instance = null;
	}

		private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxDestroyerTool.instance = this;
		this.affectFoundation = true;
	}

	protected override string GetDragSound()
	{
		return "SandboxTool_Delete_Add";
	}

	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

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

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Delete", false));
	}

	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

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

	public static SandboxDestroyerTool instance;

	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}
