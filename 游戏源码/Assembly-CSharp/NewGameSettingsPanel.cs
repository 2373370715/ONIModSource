using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : CustomGameSettingsPanelBase
{
	[SerializeField]
	private Transform content;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private KButton background;

	[Header("Prefab UI Refs")]
	[SerializeField]
	private GameObject prefab_cycle_setting;

	[SerializeField]
	private GameObject prefab_slider_setting;

	[SerializeField]
	private GameObject prefab_checkbox_setting;

	[SerializeField]
	private GameObject prefab_seed_input_setting;

	private CustomGameSettings settings;

	public void SetCloseAction(System.Action onClose)
	{
		if (closeButton != null)
		{
			closeButton.onClick += onClose;
		}
		if (background != null)
		{
			background.onClick += onClose;
		}
	}

	public override void Init()
	{
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		settings = CustomGameSettings.Instance;
		widgets = new List<CustomGameSettingWidget>();
		foreach (KeyValuePair<string, SettingConfig> qualitySetting in settings.QualitySettings)
		{
			if (qualitySetting.Value.ShowInUI())
			{
				if (qualitySetting.Value is ListSettingConfig config)
				{
					CustomGameSettingListWidget customGameSettingListWidget = Util.KInstantiateUI<CustomGameSettingListWidget>(prefab_cycle_setting, content.gameObject);
					customGameSettingListWidget.Initialize(config, CustomGameSettings.Instance.GetCurrentQualitySetting, CustomGameSettings.Instance.CycleQualitySettingLevel);
					customGameSettingListWidget.gameObject.SetActive(value: true);
					AddWidget(customGameSettingListWidget);
				}
				else if (qualitySetting.Value is ToggleSettingConfig config2)
				{
					CustomGameSettingToggleWidget customGameSettingToggleWidget = Util.KInstantiateUI<CustomGameSettingToggleWidget>(prefab_checkbox_setting, content.gameObject);
					customGameSettingToggleWidget.Initialize(config2, CustomGameSettings.Instance.GetCurrentQualitySetting, CustomGameSettings.Instance.ToggleQualitySettingLevel);
					customGameSettingToggleWidget.gameObject.SetActive(value: true);
					AddWidget(customGameSettingToggleWidget);
				}
				else if (qualitySetting.Value is SeedSettingConfig config3)
				{
					CustomGameSettingSeed customGameSettingSeed = Util.KInstantiateUI<CustomGameSettingSeed>(prefab_seed_input_setting, content.gameObject);
					customGameSettingSeed.Initialize(config3);
					customGameSettingSeed.gameObject.SetActive(value: true);
					AddWidget(customGameSettingSeed);
				}
			}
		}
		Refresh();
	}

	public void ConsumeSettingsCode(string code)
	{
		settings.ParseAndApplySettingsCode(code);
	}

	public void ConsumeStoryTraitsCode(string code)
	{
		settings.ParseAndApplyStoryTraitSettingsCode(code);
	}

	public void ConsumeMixingSettingsCode(string code)
	{
		settings.ParseAndApplyMixingSettingsCode(code);
	}

	public void SetSetting(SettingConfig setting, string level, bool notify = true)
	{
		settings.SetQualitySetting(setting, level, notify);
	}

	public string GetSetting(SettingConfig setting)
	{
		return settings.GetCurrentQualitySetting(setting).id;
	}

	public string GetSetting(string setting)
	{
		return settings.GetCurrentQualitySetting(setting).id;
	}

	public void Cancel()
	{
	}
}
