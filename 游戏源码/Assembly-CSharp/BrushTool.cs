using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001419 RID: 5145
public class BrushTool : InterfaceTool
{
	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06006A0A RID: 27146 RVA: 0x000E599D File Offset: 0x000E3B9D
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x06006A0B RID: 27147 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void PlaySound()
	{
	}

	// Token: 0x06006A0C RID: 27148 RVA: 0x000E59A5 File Offset: 0x000E3BA5
	protected virtual void clearVisitedCells()
	{
		this.visitedCells.Clear();
	}

	// Token: 0x06006A0D RID: 27149 RVA: 0x000E59B2 File Offset: 0x000E3BB2
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
	}

	// Token: 0x06006A0E RID: 27150 RVA: 0x002DD498 File Offset: 0x002DB698
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006A0F RID: 27151 RVA: 0x002DD500 File Offset: 0x002DB700
	public virtual void SetBrushSize(int radius)
	{
		if (radius == this.brushRadius)
		{
			return;
		}
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x06006A10 RID: 27152 RVA: 0x000E59C1 File Offset: 0x000E3BC1
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006A11 RID: 27153 RVA: 0x002DD5A4 File Offset: 0x002DB7A4
	protected override void OnPrefabInit()
	{
		Game.Instance.Subscribe(1634669191, new Action<object>(this.OnTutorialOpened));
		base.OnPrefabInit();
		if (this.visualizer != null)
		{
			this.visualizer = global::Util.KInstantiate(this.visualizer, null, null);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer = global::Util.KInstantiate(this.areaVisualizer, null, null);
			this.areaVisualizer.SetActive(false);
			this.areaVisualizer.GetComponent<RectTransform>().SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x06006A12 RID: 27154 RVA: 0x000E59E4 File Offset: 0x000E3BE4
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x06006A13 RID: 27155 RVA: 0x000E59ED File Offset: 0x000E3BED
	protected override void OnCmpDisable()
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(false);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06006A14 RID: 27156 RVA: 0x000E5A23 File Offset: 0x000E3C23
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		this.dragging = true;
		this.downPos = cursor_pos;
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(true, null);
		}
		this.Paint();
	}

	// Token: 0x06006A15 RID: 27157 RVA: 0x002DD658 File Offset: 0x002DB858
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		BrushTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis == BrushTool.DragAxis.Horizontal)
		{
			cursor_pos.y = this.downPos.y;
			this.dragAxis = BrushTool.DragAxis.None;
			return;
		}
		if (dragAxis != BrushTool.DragAxis.Vertical)
		{
			return;
		}
		cursor_pos.x = this.downPos.x;
		this.dragAxis = BrushTool.DragAxis.None;
	}

	// Token: 0x06006A16 RID: 27158 RVA: 0x000E5A63 File Offset: 0x000E3C63
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x06006A17 RID: 27159 RVA: 0x000E5A6A File Offset: 0x000E3C6A
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x06006A18 RID: 27160 RVA: 0x000E5A71 File Offset: 0x000E3C71
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06006A19 RID: 27161 RVA: 0x00228B58 File Offset: 0x00226D58
	private static int GetGridDistance(int cell, int center_cell)
	{
		Vector2I u = Grid.CellToXY(cell);
		Vector2I v = Grid.CellToXY(center_cell);
		Vector2I vector2I = u - v;
		return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
	}

	// Token: 0x06006A1A RID: 27162 RVA: 0x002DD6E0 File Offset: 0x002DB8E0
	private void Paint()
	{
		int count = this.visitedCells.Count;
		foreach (int num in this.cellsInRadius)
		{
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId && (!Grid.Foundation[num] || this.affectFoundation))
			{
				this.OnPaintCell(num, Grid.GetCellDistance(this.currentCell, num));
			}
		}
		if (this.lastCell != this.currentCell)
		{
			this.PlayDragSound();
		}
		if (count < this.visitedCells.Count)
		{
			this.PlaySound();
		}
	}

	// Token: 0x06006A1B RID: 27163 RVA: 0x002DD7A4 File Offset: 0x002DB9A4
	protected virtual void PlayDragSound()
	{
		string dragSound = this.GetDragSound();
		if (!string.IsNullOrEmpty(dragSound))
		{
			string sound = GlobalAssets.GetSound(dragSound, false);
			if (sound != null)
			{
				Vector3 pos = Grid.CellToPos(this.currentCell);
				pos.z = 0f;
				int cellDistance = Grid.GetCellDistance(Grid.PosToCell(this.downPos), this.currentCell);
				EventInstance instance = SoundEvent.BeginOneShot(sound, pos, 1f, false);
				instance.setParameterByName("tileCount", (float)cellDistance, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06006A1C RID: 27164 RVA: 0x002DD824 File Offset: 0x002DBA24
	public override void OnMouseMove(Vector3 cursorPos)
	{
		int num = Grid.PosToCell(cursorPos);
		this.currentCell = num;
		base.OnMouseMove(cursorPos);
		this.cellsInRadius.Clear();
		foreach (Vector2 vector in this.brushOffsets)
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y));
			if (Grid.IsValidCell(num2) && (int)Grid.WorldIdx[num2] == ClusterManager.Instance.activeWorldId)
			{
				this.cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y)));
			}
		}
		if (!this.dragging)
		{
			return;
		}
		this.Paint();
		this.lastCell = this.currentCell;
	}

	// Token: 0x06006A1D RID: 27165 RVA: 0x000E5A78 File Offset: 0x000E3C78
	protected virtual void OnPaintCell(int cell, int distFromOrigin)
	{
		if (!this.visitedCells.Contains(cell))
		{
			this.visitedCells.Add(cell);
		}
	}

	// Token: 0x06006A1E RID: 27166 RVA: 0x000E5A94 File Offset: 0x000E3C94
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.None;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06006A1F RID: 27167 RVA: 0x000E5AC7 File Offset: 0x000E3CC7
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.Invalid;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06006A20 RID: 27168 RVA: 0x002DD914 File Offset: 0x002DBB14
	private void HandlePriortyKeysDown(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 > action || action > global::Action.Plan10 || !e.TryConsume(action))
		{
			return;
		}
		int num = action - global::Action.Plan1 + 1;
		if (num <= 9)
		{
			ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, num), true);
			return;
		}
		ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
	}

	// Token: 0x06006A21 RID: 27169 RVA: 0x002DD978 File Offset: 0x002DBB78
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x06006A22 RID: 27170 RVA: 0x000E5AFA File Offset: 0x000E3CFA
	public override void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
		base.OnFocus(focus);
	}

	// Token: 0x06006A23 RID: 27171 RVA: 0x000E59E4 File Offset: 0x000E3BE4
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x06006A24 RID: 27172 RVA: 0x000E5B24 File Offset: 0x000E3D24
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x06006A25 RID: 27173 RVA: 0x000E5B36 File Offset: 0x000E3D36
	public override void LateUpdate()
	{
		base.LateUpdate();
	}

	// Token: 0x04005010 RID: 20496
	[SerializeField]
	private Texture2D brushCursor;

	// Token: 0x04005011 RID: 20497
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04005012 RID: 20498
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04005013 RID: 20499
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04005014 RID: 20500
	protected Vector3 placementPivot;

	// Token: 0x04005015 RID: 20501
	protected bool interceptNumberKeysForPriority;

	// Token: 0x04005016 RID: 20502
	protected List<Vector2> brushOffsets = new List<Vector2>();

	// Token: 0x04005017 RID: 20503
	protected bool affectFoundation;

	// Token: 0x04005018 RID: 20504
	private bool dragging;

	// Token: 0x04005019 RID: 20505
	protected int brushRadius = -1;

	// Token: 0x0400501A RID: 20506
	private BrushTool.DragAxis dragAxis = BrushTool.DragAxis.Invalid;

	// Token: 0x0400501B RID: 20507
	protected Vector3 downPos;

	// Token: 0x0400501C RID: 20508
	protected int currentCell;

	// Token: 0x0400501D RID: 20509
	protected int lastCell;

	// Token: 0x0400501E RID: 20510
	protected List<int> visitedCells = new List<int>();

	// Token: 0x0400501F RID: 20511
	protected HashSet<int> cellsInRadius = new HashSet<int>();

	// Token: 0x0200141A RID: 5146
	private enum DragAxis
	{
		// Token: 0x04005021 RID: 20513
		Invalid = -1,
		// Token: 0x04005022 RID: 20514
		None,
		// Token: 0x04005023 RID: 20515
		Horizontal,
		// Token: 0x04005024 RID: 20516
		Vertical
	}
}
