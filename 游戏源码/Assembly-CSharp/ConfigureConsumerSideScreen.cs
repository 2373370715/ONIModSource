using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class ConfigureConsumerSideScreen : SideScreenContent
{
	[SerializeField]
	private RectTransform consumptionSettingToggleContainer;

	[SerializeField]
	private GameObject consumptionSettingTogglePrefab;

	[SerializeField]
	private RectTransform settingRequirementRowsContainer;

	[SerializeField]
	private RectTransform settingEffectRowsContainer;

	[SerializeField]
	private LocText selectedOptionNameLabel;

	[SerializeField]
	private GameObject settingDescriptorPrefab;

	private IConfigurableConsumer targetProducer;

	private IConfigurableConsumerOption[] settings;

	private LocText descriptor;

	private List<HierarchyReferences> settingToggles = new List<HierarchyReferences>();

	private List<GameObject> requirementRows = new List<GameObject>();

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IConfigurableConsumer>() != null;
	}

	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		targetProducer = target.GetComponent<IConfigurableConsumer>();
		if (settings == null)
		{
			settings = targetProducer.GetSettingOptions();
		}
		PopulateOptions();
	}

	private void ClearOldOptions()
	{
		if (descriptor != null)
		{
			descriptor.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < settingToggles.Count; i++)
		{
			settingToggles[i].gameObject.SetActive(value: false);
		}
	}

	private void PopulateOptions()
	{
		ClearOldOptions();
		for (int i = settingToggles.Count; i < settings.Length; i++)
		{
			HierarchyReferences hierarchyReferences = null;
			IConfigurableConsumerOption setting = settings[i];
			hierarchyReferences = Util.KInstantiateUI(consumptionSettingTogglePrefab, consumptionSettingToggleContainer.gameObject, force_active: true).GetComponent<HierarchyReferences>();
			settingToggles.Add(hierarchyReferences);
			hierarchyReferences.GetReference<LocText>("Label").text = setting.GetName();
			hierarchyReferences.GetReference<Image>("Image").sprite = setting.GetIcon();
			MultiToggle reference = hierarchyReferences.GetReference<MultiToggle>("Toggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, (System.Action)delegate
			{
				SelectOption(setting);
			});
		}
		RefreshToggles();
		RefreshDetails();
	}

	private void SelectOption(IConfigurableConsumerOption option)
	{
		targetProducer.SetSelectedOption(option);
		RefreshToggles();
		RefreshDetails();
	}

	private void RefreshToggles()
	{
		for (int i = 0; i < settingToggles.Count; i++)
		{
			MultiToggle reference = settingToggles[i].GetReference<MultiToggle>("Toggle");
			reference.ChangeState((settings[i] == targetProducer.GetSelectedOption()) ? 1 : 0);
			reference.gameObject.SetActive(value: true);
		}
	}

	private void RefreshDetails()
	{
		if (descriptor == null)
		{
			GameObject gameObject = Util.KInstantiateUI(settingDescriptorPrefab, settingEffectRowsContainer.gameObject, force_active: true);
			descriptor = gameObject.GetComponent<LocText>();
		}
		IConfigurableConsumerOption selectedOption = targetProducer.GetSelectedOption();
		if (selectedOption != null)
		{
			descriptor.text = selectedOption.GetDetailedDescription();
			selectedOptionNameLabel.text = "<b>" + selectedOption.GetName() + "</b>";
			descriptor.gameObject.SetActive(value: true);
		}
		else
		{
			selectedOptionNameLabel.text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPESELECTED;
		}
	}

	public override int GetSideScreenSortOrder()
	{
		return 1;
	}
}
