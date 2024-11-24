using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200143C RID: 5180
public class SandboxClearFloorTool : BrushTool
{
	// Token: 0x06006B60 RID: 27488 RVA: 0x000E689B File Offset: 0x000E4A9B
	public static void DestroyInstance()
	{
		SandboxClearFloorTool.instance = null;
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06006B61 RID: 27489 RVA: 0x000E67C3 File Offset: 0x000E49C3
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006B62 RID: 27490 RVA: 0x000E68A3 File Offset: 0x000E4AA3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxClearFloorTool.instance = this;
	}

	// Token: 0x06006B63 RID: 27491 RVA: 0x000CA99D File Offset: 0x000C8B9D
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006B64 RID: 27492 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B65 RID: 27493 RVA: 0x002E27A4 File Offset: 0x002E09A4
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x06006B66 RID: 27494 RVA: 0x000E68B1 File Offset: 0x000E4AB1
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006B67 RID: 27495 RVA: 0x002DD498 File Offset: 0x002DB698
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006B68 RID: 27496 RVA: 0x000E68CA File Offset: 0x000E4ACA
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006B69 RID: 27497 RVA: 0x000E6802 File Offset: 0x000E4A02
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06006B6A RID: 27498 RVA: 0x002E2808 File Offset: 0x002E0A08
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		bool flag = false;
		using (List<Pickupable>.Enumerator enumerator = Components.Pickupables.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Pickupable pickup = enumerator.Current;
				if (!(pickup.storage != null) && Grid.PosToCell(pickup) == cell && Components.LiveMinionIdentities.Items.Find((MinionIdentity match) => match.gameObject == pickup.gameObject) == null)
				{
					if (!flag)
					{
						KFMOD.PlayOneShot(this.soundPath, pickup.gameObject.transform.GetPosition(), 1f);
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickup.transform, 1.5f, false);
						flag = true;
					}
					Util.KDestroyGameObject(pickup.gameObject);
				}
			}
		}
	}

	// Token: 0x040050C3 RID: 20675
	public static SandboxClearFloorTool instance;

	// Token: 0x040050C4 RID: 20676
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
