using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F57 RID: 8023
public class ConfigureConsumerSideScreen : SideScreenContent
{
	// Token: 0x0600A95C RID: 43356 RVA: 0x0010E071 File Offset: 0x0010C271
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IConfigurableConsumer>() != null;
	}

	// Token: 0x0600A95D RID: 43357 RVA: 0x0010E07C File Offset: 0x0010C27C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetProducer = target.GetComponent<IConfigurableConsumer>();
		if (this.settings == null)
		{
			this.settings = this.targetProducer.GetSettingOptions();
		}
		this.PopulateOptions();
	}

	// Token: 0x0600A95E RID: 43358 RVA: 0x004014E8 File Offset: 0x003FF6E8
	private void ClearOldOptions()
	{
		if (this.descriptor != null)
		{
			this.descriptor.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			this.settingToggles[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x0600A95F RID: 43359 RVA: 0x00401544 File Offset: 0x003FF744
	private void PopulateOptions()
	{
		this.ClearOldOptions();
		for (int i = this.settingToggles.Count; i < this.settings.Length; i++)
		{
			IConfigurableConsumerOption setting = this.settings[i];
			HierarchyReferences component = Util.KInstantiateUI(this.consumptionSettingTogglePrefab, this.consumptionSettingToggleContainer.gameObject, true).GetComponent<HierarchyReferences>();
			this.settingToggles.Add(component);
			component.GetReference<LocText>("Label").text = setting.GetName();
			component.GetReference<Image>("Image").sprite = setting.GetIcon();
			MultiToggle reference = component.GetReference<MultiToggle>("Toggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				this.SelectOption(setting);
			}));
		}
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x0600A960 RID: 43360 RVA: 0x0010E0B0 File Offset: 0x0010C2B0
	private void SelectOption(IConfigurableConsumerOption option)
	{
		this.targetProducer.SetSelectedOption(option);
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x0600A961 RID: 43361 RVA: 0x00401630 File Offset: 0x003FF830
	private void RefreshToggles()
	{
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			MultiToggle reference = this.settingToggles[i].GetReference<MultiToggle>("Toggle");
			reference.ChangeState((this.settings[i] == this.targetProducer.GetSelectedOption()) ? 1 : 0);
			reference.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600A962 RID: 43362 RVA: 0x00401694 File Offset: 0x003FF894
	private void RefreshDetails()
	{
		if (this.descriptor == null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.settingDescriptorPrefab, this.settingEffectRowsContainer.gameObject, true);
			this.descriptor = gameObject.GetComponent<LocText>();
		}
		IConfigurableConsumerOption selectedOption = this.targetProducer.GetSelectedOption();
		if (selectedOption != null)
		{
			this.descriptor.text = selectedOption.GetDetailedDescription();
			this.selectedOptionNameLabel.text = "<b>" + selectedOption.GetName() + "</b>";
			this.descriptor.gameObject.SetActive(true);
			return;
		}
		this.selectedOptionNameLabel.text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPESELECTED;
	}

	// Token: 0x0600A963 RID: 43363 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x04008539 RID: 34105
	[SerializeField]
	private RectTransform consumptionSettingToggleContainer;

	// Token: 0x0400853A RID: 34106
	[SerializeField]
	private GameObject consumptionSettingTogglePrefab;

	// Token: 0x0400853B RID: 34107
	[SerializeField]
	private RectTransform settingRequirementRowsContainer;

	// Token: 0x0400853C RID: 34108
	[SerializeField]
	private RectTransform settingEffectRowsContainer;

	// Token: 0x0400853D RID: 34109
	[SerializeField]
	private LocText selectedOptionNameLabel;

	// Token: 0x0400853E RID: 34110
	[SerializeField]
	private GameObject settingDescriptorPrefab;

	// Token: 0x0400853F RID: 34111
	private IConfigurableConsumer targetProducer;

	// Token: 0x04008540 RID: 34112
	private IConfigurableConsumerOption[] settings;

	// Token: 0x04008541 RID: 34113
	private LocText descriptor;

	// Token: 0x04008542 RID: 34114
	private List<HierarchyReferences> settingToggles = new List<HierarchyReferences>();

	// Token: 0x04008543 RID: 34115
	private List<GameObject> requirementRows = new List<GameObject>();
}
