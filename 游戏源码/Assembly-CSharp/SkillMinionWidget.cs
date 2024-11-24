﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001FFF RID: 8191
[AddComponentMenu("KMonoBehaviour/scripts/SkillMinionWidget")]
public class SkillMinionWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x0600ADF6 RID: 44534 RVA: 0x00111395 File Offset: 0x0010F595
	// (set) Token: 0x0600ADF7 RID: 44535 RVA: 0x0011139D File Offset: 0x0010F59D
	public IAssignableIdentity assignableIdentity { get; private set; }

	// Token: 0x0600ADF8 RID: 44536 RVA: 0x004158E4 File Offset: 0x00413AE4
	public void SetMinon(IAssignableIdentity identity)
	{
		this.assignableIdentity = identity;
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		base.GetComponent<NotificationHighlightTarget>().targetKey = identity.GetSoleOwner().gameObject.GetInstanceID().ToString();
	}

	// Token: 0x0600ADF9 RID: 44537 RVA: 0x001113A6 File Offset: 0x0010F5A6
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHover(true);
		this.soundPlayer.Play(1);
	}

	// Token: 0x0600ADFA RID: 44538 RVA: 0x001113BB File Offset: 0x0010F5BB
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHover(false);
	}

	// Token: 0x0600ADFB RID: 44539 RVA: 0x001113C4 File Offset: 0x0010F5C4
	private void ToggleHover(bool on)
	{
		if (this.skillsScreen.CurrentlySelectedMinion != this.assignableIdentity)
		{
			this.SetColor(on ? this.hover_color : this.unselected_color);
		}
	}

	// Token: 0x0600ADFC RID: 44540 RVA: 0x001113F0 File Offset: 0x0010F5F0
	private void SetColor(Color color)
	{
		this.background.color = color;
		if (this.assignableIdentity != null && this.assignableIdentity as StoredMinionIdentity != null)
		{
			base.GetComponent<CanvasGroup>().alpha = 0.6f;
		}
	}

	// Token: 0x0600ADFD RID: 44541 RVA: 0x00111429 File Offset: 0x0010F629
	public void OnPointerClick(PointerEventData eventData)
	{
		this.skillsScreen.CurrentlySelectedMinion = this.assignableIdentity;
		base.GetComponent<NotificationHighlightTarget>().View();
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
	}

	// Token: 0x0600ADFE RID: 44542 RVA: 0x00415930 File Offset: 0x00413B30
	public void Refresh()
	{
		if (this.assignableIdentity.IsNullOrDestroyed())
		{
			return;
		}
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		this.hatDropDown.gameObject.SetActive(true);
		string hat;
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			int availableSkillpoints = component.AvailableSkillpoints;
			int totalSkillPointsGained = component.TotalSkillPointsGained;
			this.masteryPoints.text = ((availableSkillpoints > 0) ? GameUtil.ApplyBoldString(GameUtil.ColourizeString(new Color(0.5f, 1f, 0.5f, 1f), availableSkillpoints.ToString())) : "0");
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component);
			this.morale.text = string.Format("{0}/{1}", attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue());
			this.RefreshToolTip(component);
			List<IListableOption> list = new List<IListableOption>();
			foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
			{
				if (keyValuePair.Value)
				{
					list.Add(new SkillListable(keyValuePair.Key));
				}
			}
			this.hatDropDown.Initialize(list, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, minionIdentity);
			hat = (string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat);
		}
		else
		{
			ToolTip component2 = base.GetComponent<ToolTip>();
			component2.ClearMultiStringTooltip();
			component2.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), storedMinionIdentity.GetProperName()), null);
			hat = (string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat);
			this.masteryPoints.text = UI.TABLESCREENS.NA;
			this.morale.text = UI.TABLESCREENS.NA;
		}
		bool flag = this.skillsScreen.CurrentlySelectedMinion == this.assignableIdentity;
		if (this.skillsScreen.CurrentlySelectedMinion != null && this.assignableIdentity != null)
		{
			flag = (flag || this.skillsScreen.CurrentlySelectedMinion.GetSoleOwner() == this.assignableIdentity.GetSoleOwner());
		}
		this.SetColor(flag ? this.selected_color : this.unselected_color);
		HierarchyReferences component3 = base.GetComponent<HierarchyReferences>();
		this.RefreshHat(hat);
		component3.GetReference("openButton").gameObject.SetActive(minionIdentity != null);
	}

	// Token: 0x0600ADFF RID: 44543 RVA: 0x00415C14 File Offset: 0x00413E14
	private void RefreshToolTip(MinionResume resume)
	{
		if (resume != null)
		{
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(resume);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(resume);
			ToolTip component = base.GetComponent<ToolTip>();
			component.ClearMultiStringTooltip();
			component.AddMultiStringTooltip(this.assignableIdentity.GetProperName() + "\n\n", this.TooltipTextStyle_Header);
			component.AddMultiStringTooltip(string.Format(UI.SKILLS_SCREEN.CURRENT_MORALE, attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue()), null);
			component.AddMultiStringTooltip("\n" + UI.DETAILTABS.STATS.NAME + "\n\n", this.TooltipTextStyle_Header);
			foreach (AttributeInstance attributeInstance3 in resume.GetAttributes())
			{
				if (attributeInstance3.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
				{
					string text = UIConstants.ColorPrefixWhite;
					if (attributeInstance3.GetTotalValue() > 0f)
					{
						text = UIConstants.ColorPrefixGreen;
					}
					else if (attributeInstance3.GetTotalValue() < 0f)
					{
						text = UIConstants.ColorPrefixRed;
					}
					component.AddMultiStringTooltip(string.Concat(new string[]
					{
						"    • ",
						attributeInstance3.Name,
						": ",
						text,
						attributeInstance3.GetTotalValue().ToString(),
						UIConstants.ColorSuffix
					}), null);
				}
			}
		}
	}

	// Token: 0x0600AE00 RID: 44544 RVA: 0x00111457 File Offset: 0x0010F657
	public void RefreshHat(string hat)
	{
		base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite(string.IsNullOrEmpty(hat) ? "hat_role_none" : hat);
	}

	// Token: 0x0600AE01 RID: 44545 RVA: 0x00415DA8 File Offset: 0x00413FA8
	private void OnHatDropEntryClick(IListableOption skill, object data)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity == null)
		{
			return;
		}
		MinionResume component = minionIdentity.GetComponent<MinionResume>();
		if (skill != null)
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((skill as SkillListable).skillHat);
			if (component != null)
			{
				string skillHat = (skill as SkillListable).skillHat;
				component.SetHats(component.CurrentHat, skillHat);
				if (component.OwnsHat(skillHat))
				{
					new PutOnHatChore(component, Db.Get().ChoreTypes.SwitchHat);
				}
			}
		}
		else
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite("hat_role_none");
			if (component != null)
			{
				component.SetHats(component.CurrentHat, null);
				component.ApplyTargetHat();
			}
		}
		this.skillsScreen.RefreshAll();
	}

	// Token: 0x0600AE02 RID: 44546 RVA: 0x00415EA0 File Offset: 0x004140A0
	private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillListable skillListable = entry.entryData as SkillListable;
			entry.image.sprite = Assets.GetSprite(skillListable.skillHat);
		}
	}

	// Token: 0x0600AE03 RID: 44547 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	private int hatDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x0400889F RID: 34975
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x040088A0 RID: 34976
	[SerializeField]
	private CrewPortrait portrait;

	// Token: 0x040088A1 RID: 34977
	[SerializeField]
	private LocText masteryPoints;

	// Token: 0x040088A2 RID: 34978
	[SerializeField]
	private LocText morale;

	// Token: 0x040088A3 RID: 34979
	[SerializeField]
	private Image background;

	// Token: 0x040088A4 RID: 34980
	[SerializeField]
	private Image hat_background;

	// Token: 0x040088A5 RID: 34981
	[SerializeField]
	private Color selected_color;

	// Token: 0x040088A6 RID: 34982
	[SerializeField]
	private Color unselected_color;

	// Token: 0x040088A7 RID: 34983
	[SerializeField]
	private Color hover_color;

	// Token: 0x040088A8 RID: 34984
	[SerializeField]
	private DropDown hatDropDown;

	// Token: 0x040088A9 RID: 34985
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x040088AA RID: 34986
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x040088AB RID: 34987
	public ButtonSoundPlayer soundPlayer;
}
