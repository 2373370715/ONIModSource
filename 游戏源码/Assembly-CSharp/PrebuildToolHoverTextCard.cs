﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001B36 RID: 6966
public class PrebuildToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06009225 RID: 37413 RVA: 0x00385E0C File Offset: 0x0038400C
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		if (!this.errorMessage.IsNullOrWhiteSpace())
		{
			bool flag = true;
			foreach (string text in this.errorMessage.Split('\n', StringSplitOptions.None))
			{
				if (!flag)
				{
					hoverTextDrawer.NewLine(26);
				}
				hoverTextDrawer.DrawText(text.ToUpper(), this.HoverTextStyleSettings[flag ? 0 : 1]);
				flag = false;
			}
		}
		hoverTextDrawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			hoverTextDrawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("icon_mouse_right"), 20);
		}
		hoverTextDrawer.DrawText(this.backStr, this.Styles_Instruction.Standard);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04006E8F RID: 28303
	public string errorMessage;

	// Token: 0x04006E90 RID: 28304
	public BuildingDef currentDef;
}
