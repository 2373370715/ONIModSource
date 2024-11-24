using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200143E RID: 5182
public class SandboxCritterTool : BrushTool
{
	// Token: 0x06006B6E RID: 27502 RVA: 0x000E6904 File Offset: 0x000E4B04
	public static void DestroyInstance()
	{
		SandboxCritterTool.instance = null;
	}

	// Token: 0x06006B6F RID: 27503 RVA: 0x000E690C File Offset: 0x000E4B0C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxCritterTool.instance = this;
	}

	// Token: 0x06006B70 RID: 27504 RVA: 0x000CA99D File Offset: 0x000C8B9D
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006B71 RID: 27505 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B72 RID: 27506 RVA: 0x000E691A File Offset: 0x000E4B1A
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(6f, true);
	}

	// Token: 0x06006B73 RID: 27507 RVA: 0x000E68B1 File Offset: 0x000E4AB1
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006B74 RID: 27508 RVA: 0x002DD498 File Offset: 0x002DB698
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006B75 RID: 27509 RVA: 0x000E68CA File Offset: 0x000E4ACA
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006B76 RID: 27510 RVA: 0x000E6802 File Offset: 0x000E4A02
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06006B77 RID: 27511 RVA: 0x002E2928 File Offset: 0x002E0B28
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		HashSetPool<GameObject, SandboxCritterTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxCritterTool>.Allocate();
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell && health.GetComponent<KPrefabID>().HasTag(GameTags.Creature))
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (GameObject gameObject in pooledHashSet)
		{
			KFMOD.PlayOneShot(this.soundPath, gameObject.gameObject.transform.GetPosition(), 1f);
			Util.KDestroyGameObject(gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x040050C6 RID: 20678
	public static SandboxCritterTool instance;

	// Token: 0x040050C7 RID: 20679
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
