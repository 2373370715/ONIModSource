using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200141D RID: 5149
public class CancelTool : FilteredDragTool
{
	// Token: 0x06006A47 RID: 27207 RVA: 0x000E5CBD File Offset: 0x000E3EBD
	public static void DestroyInstance()
	{
		CancelTool.Instance = null;
	}

	// Token: 0x06006A48 RID: 27208 RVA: 0x000E5CC5 File Offset: 0x000E3EC5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CancelTool.Instance = this;
	}

	// Token: 0x06006A49 RID: 27209 RVA: 0x000E5CD3 File Offset: 0x000E3ED3
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		base.GetDefaultFilters(filters);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CLEANANDCLEAR, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.DIGPLACER, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06006A4A RID: 27210 RVA: 0x000E5CF4 File Offset: 0x000E3EF4
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06006A4B RID: 27211 RVA: 0x000E5CFB File Offset: 0x000E3EFB
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06006A4C RID: 27212 RVA: 0x002DE728 File Offset: 0x002DC928
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(2127324410, null);
				}
			}
		}
	}

	// Token: 0x06006A4D RID: 27213 RVA: 0x002DE778 File Offset: 0x002DC978
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, false);
	}

	// Token: 0x04005035 RID: 20533
	public static CancelTool Instance;
}
