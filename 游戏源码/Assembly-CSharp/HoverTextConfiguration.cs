using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001ACA RID: 6858
[AddComponentMenu("KMonoBehaviour/scripts/HoverTextConfiguration")]
public class HoverTextConfiguration : KMonoBehaviour
{
	// Token: 0x06008FA9 RID: 36777 RVA: 0x000FDE30 File Offset: 0x000FC030
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ConfigureHoverScreen();
	}

	// Token: 0x06008FAA RID: 36778 RVA: 0x000FDE3E File Offset: 0x000FC03E
	protected virtual void ConfigureTitle(HoverTextScreen screen)
	{
		if (string.IsNullOrEmpty(this.ToolName))
		{
			this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
		}
	}

	// Token: 0x06008FAB RID: 36779 RVA: 0x000FDE68 File Offset: 0x000FC068
	protected void DrawTitle(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		drawer.DrawText(this.ToolName, this.ToolTitleTextStyle);
	}

	// Token: 0x06008FAC RID: 36780 RVA: 0x00377914 File Offset: 0x00375B14
	protected void DrawInstructions(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		TextStyleSetting standard = this.Styles_Instruction.Standard;
		drawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseLeft, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_left"), 20);
		}
		drawer.DrawText(this.ActionName, standard);
		drawer.AddIndent(8);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_right"), 20);
		}
		drawer.DrawText(this.backStr, standard);
	}

	// Token: 0x06008FAD RID: 36781 RVA: 0x003779B8 File Offset: 0x00375BB8
	public virtual void ConfigureHoverScreen()
	{
		if (!string.IsNullOrEmpty(this.ActionStringKey))
		{
			this.ActionName = Strings.Get(this.ActionStringKey);
		}
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.ConfigureTitle(instance);
		this.backStr = UI.TOOLS.GENERIC.BACK.ToString().ToUpper();
	}

	// Token: 0x06008FAE RID: 36782 RVA: 0x00377A0C File Offset: 0x00375C0C
	public virtual void UpdateHoverElements(List<KSelectable> hover_objects)
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
		this.DrawTitle(instance, hoverTextDrawer);
		this.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04006C64 RID: 27748
	public TextStyleSetting[] HoverTextStyleSettings;

	// Token: 0x04006C65 RID: 27749
	public string ToolNameStringKey = "";

	// Token: 0x04006C66 RID: 27750
	public string ActionStringKey = "";

	// Token: 0x04006C67 RID: 27751
	[HideInInspector]
	public string ActionName = "";

	// Token: 0x04006C68 RID: 27752
	[HideInInspector]
	public string ToolName;

	// Token: 0x04006C69 RID: 27753
	protected string backStr;

	// Token: 0x04006C6A RID: 27754
	public TextStyleSetting ToolTitleTextStyle;

	// Token: 0x04006C6B RID: 27755
	public HoverTextConfiguration.TextStylePair Styles_Title;

	// Token: 0x04006C6C RID: 27756
	public HoverTextConfiguration.TextStylePair Styles_BodyText;

	// Token: 0x04006C6D RID: 27757
	public HoverTextConfiguration.TextStylePair Styles_Instruction;

	// Token: 0x04006C6E RID: 27758
	public HoverTextConfiguration.TextStylePair Styles_Warning;

	// Token: 0x04006C6F RID: 27759
	public HoverTextConfiguration.ValuePropertyTextStyles Styles_Values;

	// Token: 0x02001ACB RID: 6859
	[Serializable]
	public struct TextStylePair
	{
		// Token: 0x04006C70 RID: 27760
		public TextStyleSetting Standard;

		// Token: 0x04006C71 RID: 27761
		public TextStyleSetting Selected;
	}

	// Token: 0x02001ACC RID: 6860
	[Serializable]
	public struct ValuePropertyTextStyles
	{
		// Token: 0x04006C72 RID: 27762
		public HoverTextConfiguration.TextStylePair Property;

		// Token: 0x04006C73 RID: 27763
		public HoverTextConfiguration.TextStylePair Property_Decimal;

		// Token: 0x04006C74 RID: 27764
		public HoverTextConfiguration.TextStylePair Property_Unit;
	}
}
