using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001E4F RID: 7759
[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : CustomGameSettingsPanelBase
{
	// Token: 0x0600A295 RID: 41621 RVA: 0x00109753 File Offset: 0x00107953
	public void SetCloseAction(System.Action onClose)
	{
		if (this.closeButton != null)
		{
			this.closeButton.onClick += onClose;
		}
		if (this.background != null)
		{
			this.background.onClick += onClose;
		}
	}

	// Token: 0x0600A296 RID: 41622 RVA: 0x003DE328 File Offset: 0x003DC528
	public override void Init()
	{
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		this.settings = CustomGameSettings.Instance;
		this.widgets = new List<CustomGameSettingWidget>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.settings.QualitySettings)
		{
			if (keyValuePair.Value.ShowInUI())
			{
				ListSettingConfig listSettingConfig = keyValuePair.Value as ListSettingConfig;
				if (listSettingConfig != null)
				{
					CustomGameSettingListWidget customGameSettingListWidget = Util.KInstantiateUI<CustomGameSettingListWidget>(this.prefab_cycle_setting, this.content.gameObject, false);
					customGameSettingListWidget.Initialize(listSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ListSettingConfig, int, SettingLevel>(CustomGameSettings.Instance.CycleQualitySettingLevel));
					customGameSettingListWidget.gameObject.SetActive(true);
					base.AddWidget(customGameSettingListWidget);
				}
				else
				{
					ToggleSettingConfig toggleSettingConfig = keyValuePair.Value as ToggleSettingConfig;
					if (toggleSettingConfig != null)
					{
						CustomGameSettingToggleWidget customGameSettingToggleWidget = Util.KInstantiateUI<CustomGameSettingToggleWidget>(this.prefab_checkbox_setting, this.content.gameObject, false);
						customGameSettingToggleWidget.Initialize(toggleSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ToggleSettingConfig, SettingLevel>(CustomGameSettings.Instance.ToggleQualitySettingLevel));
						customGameSettingToggleWidget.gameObject.SetActive(true);
						base.AddWidget(customGameSettingToggleWidget);
					}
					else
					{
						SeedSettingConfig seedSettingConfig = keyValuePair.Value as SeedSettingConfig;
						if (seedSettingConfig != null)
						{
							CustomGameSettingSeed customGameSettingSeed = Util.KInstantiateUI<CustomGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject, false);
							customGameSettingSeed.Initialize(seedSettingConfig);
							customGameSettingSeed.gameObject.SetActive(true);
							base.AddWidget(customGameSettingSeed);
						}
					}
				}
			}
		}
		this.Refresh();
	}

	// Token: 0x0600A297 RID: 41623 RVA: 0x00109789 File Offset: 0x00107989
	public void ConsumeSettingsCode(string code)
	{
		this.settings.ParseAndApplySettingsCode(code);
	}

	// Token: 0x0600A298 RID: 41624 RVA: 0x00109797 File Offset: 0x00107997
	public void ConsumeStoryTraitsCode(string code)
	{
		this.settings.ParseAndApplyStoryTraitSettingsCode(code);
	}

	// Token: 0x0600A299 RID: 41625 RVA: 0x001097A5 File Offset: 0x001079A5
	public void ConsumeMixingSettingsCode(string code)
	{
		this.settings.ParseAndApplyMixingSettingsCode(code);
	}

	// Token: 0x0600A29A RID: 41626 RVA: 0x001097B3 File Offset: 0x001079B3
	public void SetSetting(SettingConfig setting, string level, bool notify = true)
	{
		this.settings.SetQualitySetting(setting, level, notify);
	}

	// Token: 0x0600A29B RID: 41627 RVA: 0x001097C3 File Offset: 0x001079C3
	public string GetSetting(SettingConfig setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x0600A29C RID: 41628 RVA: 0x001097D6 File Offset: 0x001079D6
	public string GetSetting(string setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x0600A29D RID: 41629 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void Cancel()
	{
	}

	// Token: 0x04007EDF RID: 32479
	[SerializeField]
	private Transform content;

	// Token: 0x04007EE0 RID: 32480
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007EE1 RID: 32481
	[SerializeField]
	private KButton background;

	// Token: 0x04007EE2 RID: 32482
	[Header("Prefab UI Refs")]
	[SerializeField]
	private GameObject prefab_cycle_setting;

	// Token: 0x04007EE3 RID: 32483
	[SerializeField]
	private GameObject prefab_slider_setting;

	// Token: 0x04007EE4 RID: 32484
	[SerializeField]
	private GameObject prefab_checkbox_setting;

	// Token: 0x04007EE5 RID: 32485
	[SerializeField]
	private GameObject prefab_seed_input_setting;

	// Token: 0x04007EE6 RID: 32486
	private CustomGameSettings settings;
}
