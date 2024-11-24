using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001441 RID: 5185
public class SandboxFOWTool : BrushTool
{
	// Token: 0x06006B87 RID: 27527 RVA: 0x000E6A29 File Offset: 0x000E4C29
	public static void DestroyInstance()
	{
		SandboxFOWTool.instance = null;
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06006B88 RID: 27528 RVA: 0x000E67C3 File Offset: 0x000E49C3
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006B89 RID: 27529 RVA: 0x000E6A31 File Offset: 0x000E4C31
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFOWTool.instance = this;
	}

	// Token: 0x06006B8A RID: 27530 RVA: 0x000CA99D File Offset: 0x000C8B9D
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006B8B RID: 27531 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B8C RID: 27532 RVA: 0x000E698E File Offset: 0x000E4B8E
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06006B8D RID: 27533 RVA: 0x000E6A3F File Offset: 0x000E4C3F
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x06006B8E RID: 27534 RVA: 0x002E2DCC File Offset: 0x002E0FCC
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

	// Token: 0x06006B8F RID: 27535 RVA: 0x000E68CA File Offset: 0x000E4ACA
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006B90 RID: 27536 RVA: 0x000E6A64 File Offset: 0x000E4C64
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		Grid.Reveal(cell, byte.MaxValue, true);
	}

	// Token: 0x06006B91 RID: 27537 RVA: 0x002E2E84 File Offset: 0x002E1084
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		int intSetting = this.settings.GetIntSetting("SandboxTools.BrushSize");
		this.ev = KFMOD.CreateInstance(GlobalAssets.GetSound("SandboxTool_Reveal", false));
		this.ev.setParameterByName("BrushSize", (float)intSetting, false);
		this.ev.start();
	}

	// Token: 0x06006B92 RID: 27538 RVA: 0x000E6A7A File Offset: 0x000E4C7A
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.ev.stop(STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x040050CD RID: 20685
	public static SandboxFOWTool instance;

	// Token: 0x040050CE RID: 20686
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040050CF RID: 20687
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x040050D0 RID: 20688
	private EventInstance ev;
}
