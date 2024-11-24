using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02001EB3 RID: 7859
public class PriorityScreen : KScreen
{
	// Token: 0x0600A508 RID: 42248 RVA: 0x003EA7B0 File Offset: 0x003E89B0
	public void InstantiateButtons(Action<PrioritySetting> on_click, bool playSelectionSound = true)
	{
		this.onClick = on_click;
		for (int i = 1; i <= 9; i++)
		{
			int num = i;
			PriorityButton priorityButton = global::Util.KInstantiateUI<PriorityButton>(this.buttonPrefab_basic.gameObject, this.buttonPrefab_basic.transform.parent.gameObject, false);
			this.buttons_basic.Add(priorityButton);
			priorityButton.playSelectionSound = playSelectionSound;
			priorityButton.onClick = this.onClick;
			priorityButton.text.text = num.ToString();
			priorityButton.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, num);
			priorityButton.tooltip.SetSimpleTooltip(string.Format(UI.PRIORITYSCREEN.BASIC, num));
		}
		this.buttonPrefab_basic.gameObject.SetActive(false);
		this.button_emergency.playSelectionSound = playSelectionSound;
		this.button_emergency.onClick = this.onClick;
		this.button_emergency.priority = new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1);
		this.button_emergency.tooltip.SetSimpleTooltip(UI.PRIORITYSCREEN.TOP_PRIORITY);
		this.button_toggleHigh.gameObject.SetActive(false);
		this.PriorityMenuContainer.SetActive(true);
		this.button_priorityMenu.gameObject.SetActive(true);
		this.button_priorityMenu.onClick += this.PriorityButtonClicked;
		this.button_priorityMenu.GetComponent<ToolTip>().SetSimpleTooltip(UI.PRIORITYSCREEN.OPEN_JOBS_SCREEN);
		this.diagram.SetActive(false);
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x0600A509 RID: 42249 RVA: 0x0010B0FE File Offset: 0x001092FE
	private void OnClick(PrioritySetting priority)
	{
		if (this.onClick != null)
		{
			this.onClick(priority);
		}
	}

	// Token: 0x0600A50A RID: 42250 RVA: 0x0010B114 File Offset: 0x00109314
	public void ShowDiagram(bool show)
	{
		this.diagram.SetActive(show);
	}

	// Token: 0x0600A50B RID: 42251 RVA: 0x0010B122 File Offset: 0x00109322
	public void ResetPriority()
	{
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x0600A50C RID: 42252 RVA: 0x0010B132 File Offset: 0x00109332
	public void PriorityButtonClicked()
	{
		ManagementMenu.Instance.TogglePriorities();
	}

	// Token: 0x0600A50D RID: 42253 RVA: 0x003EA934 File Offset: 0x003E8B34
	private void RefreshButton(PriorityButton b, PrioritySetting priority, bool play_sound)
	{
		if (b.priority == priority)
		{
			b.toggle.Select();
			b.toggle.isOn = true;
			if (play_sound)
			{
				b.toggle.soundPlayer.Play(0);
				return;
			}
		}
		else
		{
			b.toggle.isOn = false;
		}
	}

	// Token: 0x0600A50E RID: 42254 RVA: 0x003EA988 File Offset: 0x003E8B88
	public void SetScreenPriority(PrioritySetting priority, bool play_sound = false)
	{
		if (this.lastSelectedPriority == priority)
		{
			return;
		}
		this.lastSelectedPriority = priority;
		if (priority.priority_class == PriorityScreen.PriorityClass.high)
		{
			this.button_toggleHigh.isOn = true;
		}
		else if (priority.priority_class == PriorityScreen.PriorityClass.basic)
		{
			this.button_toggleHigh.isOn = false;
		}
		for (int i = 0; i < this.buttons_basic.Count; i++)
		{
			this.buttons_basic[i].priority = new PrioritySetting(this.button_toggleHigh.isOn ? PriorityScreen.PriorityClass.high : PriorityScreen.PriorityClass.basic, i + 1);
			this.buttons_basic[i].tooltip.SetSimpleTooltip(string.Format(this.button_toggleHigh.isOn ? UI.PRIORITYSCREEN.HIGH : UI.PRIORITYSCREEN.BASIC, i + 1));
			this.RefreshButton(this.buttons_basic[i], this.lastSelectedPriority, play_sound);
		}
		this.RefreshButton(this.button_emergency, this.lastSelectedPriority, play_sound);
	}

	// Token: 0x0600A50F RID: 42255 RVA: 0x0010B13E File Offset: 0x0010933E
	public PrioritySetting GetLastSelectedPriority()
	{
		return this.lastSelectedPriority;
	}

	// Token: 0x0600A510 RID: 42256 RVA: 0x003EAA8C File Offset: 0x003E8C8C
	public static void PlayPriorityConfirmSound(PrioritySetting priority)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Priority_Tool_Confirm", false), Vector3.zero, 1f);
		if (instance.isValid())
		{
			float num = 0f;
			if (priority.priority_class >= PriorityScreen.PriorityClass.high)
			{
				num += 10f;
			}
			if (priority.priority_class >= PriorityScreen.PriorityClass.topPriority)
			{
				num += 0f;
			}
			num += (float)priority.priority_value;
			instance.setParameterByName("priority", num, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x04008131 RID: 33073
	[SerializeField]
	protected PriorityButton buttonPrefab_basic;

	// Token: 0x04008132 RID: 33074
	[SerializeField]
	protected GameObject EmergencyContainer;

	// Token: 0x04008133 RID: 33075
	[SerializeField]
	protected PriorityButton button_emergency;

	// Token: 0x04008134 RID: 33076
	[SerializeField]
	protected GameObject PriorityMenuContainer;

	// Token: 0x04008135 RID: 33077
	[SerializeField]
	protected KButton button_priorityMenu;

	// Token: 0x04008136 RID: 33078
	[SerializeField]
	protected KToggle button_toggleHigh;

	// Token: 0x04008137 RID: 33079
	[SerializeField]
	protected GameObject diagram;

	// Token: 0x04008138 RID: 33080
	protected List<PriorityButton> buttons_basic = new List<PriorityButton>();

	// Token: 0x04008139 RID: 33081
	protected List<PriorityButton> buttons_emergency = new List<PriorityButton>();

	// Token: 0x0400813A RID: 33082
	private PrioritySetting priority;

	// Token: 0x0400813B RID: 33083
	private PrioritySetting lastSelectedPriority = new PrioritySetting(PriorityScreen.PriorityClass.basic, -1);

	// Token: 0x0400813C RID: 33084
	private Action<PrioritySetting> onClick;

	// Token: 0x02001EB4 RID: 7860
	public enum PriorityClass
	{
		// Token: 0x0400813E RID: 33086
		idle = -1,
		// Token: 0x0400813F RID: 33087
		basic,
		// Token: 0x04008140 RID: 33088
		high,
		// Token: 0x04008141 RID: 33089
		personalNeeds,
		// Token: 0x04008142 RID: 33090
		topPriority,
		// Token: 0x04008143 RID: 33091
		compulsory
	}
}
