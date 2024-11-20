using System;
using System.Collections.Generic;
using UnityEngine;

public class ClusterMapSelectToolHoverTextCard : HoverTextConfiguration
{
	public override void ConfigureHoverScreen()
	{
		base.ConfigureHoverScreen();
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.m_iconWarning = instance.GetSprite("iconWarning");
		this.m_iconDash = instance.GetSprite("dash");
		this.m_iconHighlighted = instance.GetSprite("dash_arrow");
	}

	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
	{
		if (this.m_iconWarning == null)
		{
			this.ConfigureHoverScreen();
		}
		HoverTextDrawer hoverTextDrawer = HoverTextScreen.Instance.BeginDrawing();
		foreach (KSelectable kselectable in hoverObjects)
		{
			hoverTextDrawer.BeginShadowBar(ClusterMapSelectTool.Instance.GetSelected() == kselectable);
			string unitFormattedName = GameUtil.GetUnitFormattedName(kselectable.gameObject, true);
			hoverTextDrawer.DrawText(unitFormattedName, this.Styles_Title.Standard);
			foreach (StatusItemGroup.Entry entry in kselectable.GetStatusItemGroup())
			{
				if (entry.category != null && entry.category.Id == "Main")
				{
					TextStyleSetting style = this.IsStatusItemWarning(entry) ? this.Styles_Warning.Standard : this.Styles_BodyText.Standard;
					Sprite icon = (entry.item.sprite != null) ? entry.item.sprite.sprite : this.m_iconWarning;
					Color color = this.IsStatusItemWarning(entry) ? this.Styles_Warning.Standard.textColor : this.Styles_BodyText.Standard.textColor;
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawIcon(icon, color, 18, 2);
					hoverTextDrawer.DrawText(entry.GetName(), style);
				}
			}
			foreach (StatusItemGroup.Entry entry2 in kselectable.GetStatusItemGroup())
			{
				if (entry2.category == null || entry2.category.Id != "Main")
				{
					TextStyleSetting style2 = this.IsStatusItemWarning(entry2) ? this.Styles_Warning.Standard : this.Styles_BodyText.Standard;
					Sprite icon2 = (entry2.item.sprite != null) ? entry2.item.sprite.sprite : this.m_iconWarning;
					Color color2 = this.IsStatusItemWarning(entry2) ? this.Styles_Warning.Standard.textColor : this.Styles_BodyText.Standard.textColor;
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawIcon(icon2, color2, 18, 2);
					hoverTextDrawer.DrawText(entry2.GetName(), style2);
				}
			}
			hoverTextDrawer.EndShadowBar();
		}
		hoverTextDrawer.EndDrawing();
	}

	private bool IsStatusItemWarning(StatusItemGroup.Entry item)
	{
		return item.item.notificationType == NotificationType.Bad || item.item.notificationType == NotificationType.BadMinor || item.item.notificationType == NotificationType.DuplicantThreatening;
	}

	private Sprite m_iconWarning;

	private Sprite m_iconDash;

	private Sprite m_iconHighlighted;
}
