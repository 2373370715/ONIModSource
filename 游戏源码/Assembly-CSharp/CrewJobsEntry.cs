using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D8D RID: 7565
public class CrewJobsEntry : CrewListEntry
{
	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x06009E23 RID: 40483 RVA: 0x00106FCE File Offset: 0x001051CE
	// (set) Token: 0x06009E24 RID: 40484 RVA: 0x00106FD6 File Offset: 0x001051D6
	public ChoreConsumer consumer { get; private set; }

	// Token: 0x06009E25 RID: 40485 RVA: 0x003CA030 File Offset: 0x003C8230
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.consumer = _identity.GetComponent<ChoreConsumer>();
		ChoreConsumer consumer = this.consumer;
		consumer.choreRulesChanged = (System.Action)Delegate.Combine(consumer.choreRulesChanged, new System.Action(this.Dirty));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.CreateChoreButton(chore_group);
		}
		this.CreateAllTaskButton();
		this.dirty = true;
	}

	// Token: 0x06009E26 RID: 40486 RVA: 0x003CA0D4 File Offset: 0x003C82D4
	private void CreateChoreButton(ChoreGroup chore_group)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButton, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = chore_group.Id;
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = chore_group.Name;
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.choreGroup = chore_group;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		priorityButton.tooltip.OnToolTip = (() => this.OnPriorityButtonTooltip(priorityButton));
		priorityButton.button.onClick.AddListener(delegate()
		{
			this.OnPriorityPress(chore_group);
		});
		this.PriorityButtons.Add(priorityButton);
	}

	// Token: 0x06009E27 RID: 40487 RVA: 0x003CA250 File Offset: 0x003C8450
	private void CreateAllTaskButton()
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButtonAllTasks, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = "AllTasks";
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = "";
		Button b = gameObject.GetComponent<Button>();
		b.onClick.AddListener(delegate()
		{
			this.ToggleTasksAll(b);
		});
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		this.AllTasksButton = priorityButton;
	}

	// Token: 0x06009E28 RID: 40488 RVA: 0x003CA360 File Offset: 0x003C8560
	private void ToggleTasksAll(Button button)
	{
		bool flag = this.rowToggleState != CrewJobsScreen.everyoneToggleState.on;
		string name = "HUD_Click_Deselect";
		if (flag)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.consumer.SetPermittedByUser(chore_group, flag);
		}
	}

	// Token: 0x06009E29 RID: 40489 RVA: 0x003CA3EC File Offset: 0x003C85EC
	private void OnPriorityPress(ChoreGroup chore_group)
	{
		bool flag = this.consumer.IsPermittedByUser(chore_group);
		string name = "HUD_Click";
		if (flag)
		{
			name = "HUD_Click_Deselect";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		this.consumer.SetPermittedByUser(chore_group, !this.consumer.IsPermittedByUser(chore_group));
	}

	// Token: 0x06009E2A RID: 40490 RVA: 0x003CA440 File Offset: 0x003C8640
	private void Refresh(object data = null)
	{
		if (this.identity == null)
		{
			this.dirty = false;
			return;
		}
		if (this.dirty)
		{
			Attributes attributes = this.identity.GetAttributes();
			foreach (CrewJobsEntry.PriorityButton priorityButton in this.PriorityButtons)
			{
				bool flag = this.consumer.IsPermittedByUser(priorityButton.choreGroup);
				if (priorityButton.ToggleIcon.activeSelf != flag)
				{
					priorityButton.ToggleIcon.SetActive(flag);
				}
				float t = Mathf.Min(attributes.Get(priorityButton.choreGroup.attribute).GetTotalValue() / 10f, 1f);
				Color baseBorderColor = priorityButton.baseBorderColor;
				baseBorderColor.r = Mathf.Lerp(priorityButton.baseBorderColor.r, 0.72156864f, t);
				baseBorderColor.g = Mathf.Lerp(priorityButton.baseBorderColor.g, 0.44313726f, t);
				baseBorderColor.b = Mathf.Lerp(priorityButton.baseBorderColor.b, 0.5803922f, t);
				if (priorityButton.border.color != baseBorderColor)
				{
					priorityButton.border.color = baseBorderColor;
				}
				Color color = priorityButton.baseBackgroundColor;
				color.a = Mathf.Lerp(0f, 1f, t);
				bool flag2 = this.consumer.IsPermittedByTraits(priorityButton.choreGroup);
				if (!flag2)
				{
					color = Color.clear;
					priorityButton.border.color = Color.clear;
					priorityButton.ToggleIcon.SetActive(false);
				}
				priorityButton.button.interactable = flag2;
				if (priorityButton.background.color != color)
				{
					priorityButton.background.color = color;
				}
			}
			int num = 0;
			int num2 = 0;
			foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
			{
				if (this.consumer.IsPermittedByTraits(chore_group))
				{
					num2++;
					if (this.consumer.IsPermittedByUser(chore_group))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.off;
			}
			else if (num < num2)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.mixed;
			}
			else
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.on;
			}
			ImageToggleState component = this.AllTasksButton.ToggleIcon.GetComponent<ImageToggleState>();
			switch (this.rowToggleState)
			{
			case CrewJobsScreen.everyoneToggleState.off:
				component.SetDisabled();
				break;
			case CrewJobsScreen.everyoneToggleState.mixed:
				component.SetInactive();
				break;
			case CrewJobsScreen.everyoneToggleState.on:
				component.SetActive();
				break;
			}
			this.dirty = false;
		}
	}

	// Token: 0x06009E2B RID: 40491 RVA: 0x003CA730 File Offset: 0x003C8930
	private string OnPriorityButtonTooltip(CrewJobsEntry.PriorityButton b)
	{
		b.tooltip.ClearMultiStringTooltip();
		if (this.identity != null)
		{
			Attributes attributes = this.identity.GetAttributes();
			if (attributes != null)
			{
				if (!this.consumer.IsPermittedByTraits(b.choreGroup))
				{
					string newString = string.Format(UI.TOOLTIPS.JOBSSCREEN_CANNOTPERFORMTASK, this.consumer.GetComponent<MinionIdentity>().GetProperName());
					b.tooltip.AddMultiStringTooltip(newString, this.TooltipTextStyle_AbilityNegativeModifier);
					return "";
				}
				b.tooltip.AddMultiStringTooltip(UI.TOOLTIPS.JOBSSCREEN_RELEVANT_ATTRIBUTES, this.TooltipTextStyle_Ability);
				Klei.AI.Attribute attribute = b.choreGroup.attribute;
				AttributeInstance attributeInstance = attributes.Get(attribute);
				float totalValue = attributeInstance.GetTotalValue();
				TextStyleSetting styleSetting = this.TooltipTextStyle_Ability;
				if (totalValue > 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityPositiveModifier;
				}
				else if (totalValue < 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityNegativeModifier;
				}
				b.tooltip.AddMultiStringTooltip(attribute.Name + " " + attributeInstance.GetTotalValue().ToString(), styleSetting);
			}
		}
		return "";
	}

	// Token: 0x06009E2C RID: 40492 RVA: 0x00106FDF File Offset: 0x001051DF
	private void LateUpdate()
	{
		this.Refresh(null);
	}

	// Token: 0x06009E2D RID: 40493 RVA: 0x00106FE8 File Offset: 0x001051E8
	private void OnLevelUp(object data)
	{
		this.Dirty();
	}

	// Token: 0x06009E2E RID: 40494 RVA: 0x00106FF0 File Offset: 0x001051F0
	private void Dirty()
	{
		this.dirty = true;
		CrewJobsScreen.Instance.Dirty(null);
	}

	// Token: 0x06009E2F RID: 40495 RVA: 0x00107004 File Offset: 0x00105204
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.consumer != null)
		{
			ChoreConsumer consumer = this.consumer;
			consumer.choreRulesChanged = (System.Action)Delegate.Remove(consumer.choreRulesChanged, new System.Action(this.Dirty));
		}
	}

	// Token: 0x04007BED RID: 31725
	public GameObject Prefab_JobPriorityButton;

	// Token: 0x04007BEE RID: 31726
	public GameObject Prefab_JobPriorityButtonAllTasks;

	// Token: 0x04007BEF RID: 31727
	private List<CrewJobsEntry.PriorityButton> PriorityButtons = new List<CrewJobsEntry.PriorityButton>();

	// Token: 0x04007BF0 RID: 31728
	private CrewJobsEntry.PriorityButton AllTasksButton;

	// Token: 0x04007BF1 RID: 31729
	public TextStyleSetting TooltipTextStyle_Title;

	// Token: 0x04007BF2 RID: 31730
	public TextStyleSetting TooltipTextStyle_Ability;

	// Token: 0x04007BF3 RID: 31731
	public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;

	// Token: 0x04007BF4 RID: 31732
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04007BF5 RID: 31733
	private bool dirty;

	// Token: 0x04007BF7 RID: 31735
	private CrewJobsScreen.everyoneToggleState rowToggleState;

	// Token: 0x02001D8E RID: 7566
	[Serializable]
	public struct PriorityButton
	{
		// Token: 0x04007BF8 RID: 31736
		public Button button;

		// Token: 0x04007BF9 RID: 31737
		public GameObject ToggleIcon;

		// Token: 0x04007BFA RID: 31738
		public ChoreGroup choreGroup;

		// Token: 0x04007BFB RID: 31739
		public ToolTip tooltip;

		// Token: 0x04007BFC RID: 31740
		public Image border;

		// Token: 0x04007BFD RID: 31741
		public Image background;

		// Token: 0x04007BFE RID: 31742
		public Color baseBorderColor;

		// Token: 0x04007BFF RID: 31743
		public Color baseBackgroundColor;
	}
}
