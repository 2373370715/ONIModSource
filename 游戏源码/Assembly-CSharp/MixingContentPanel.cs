using System;
using System.Collections.Generic;
using System.Linq;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class MixingContentPanel : CustomGameSettingsPanelBase
{
	[SerializeField]
	private GameObject prefabMixingSection;

	[SerializeField]
	private GameObject prefabSettingCycle;

	[SerializeField]
	private GameObject prefabSettingDlcContent;

	[SerializeField]
	private GameObject contentPanel;

	private static Dictionary<string, string> dlcSettingIdToLastSetLevelId = new Dictionary<string, string>();

	private Dictionary<string, bool> settingIdToIsInteractableRecord = new Dictionary<string, bool>();

	private System.Action onRefresh;

	private System.Action onDestroy;

	public override void Init()
	{
		prefabMixingSection.SetActive(value: false);
		prefabMixingSection.transform.Find("Title").Find("ImageError").gameObject.SetActive(value: false);
		prefabMixingSection.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(value: false);
		prefabSettingDlcContent.SetActive(value: false);
		prefabSettingCycle.SetActive(value: false);
		GameObject gameObject = CreateSection(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_DLC_HEADER);
		GameObject gameObject2 = CreateSection(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_WORLDMIXING_HEADER);
		GameObject gameObject3 = CreateSection(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_SUBWORLDMIXING_HEADER);
		GameObject parent = gameObject.transform.Find("Content").Find("Grid").gameObject;
		GameObject parent2 = gameObject2.transform.Find("Content").Find("Grid").gameObject;
		GameObject parent3 = gameObject3.transform.Find("Content").Find("Grid").gameObject;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<string, SettingConfig> mixingSetting in CustomGameSettings.Instance.MixingSettings)
		{
			if (!mixingSetting.Value.ShowInUI())
			{
				continue;
			}
			if (mixingSetting.Value is DlcMixingSettingConfig dlcMixingSettingConfig)
			{
				if (!dlcSettingIdToLastSetLevelId.TryGetValue(dlcMixingSettingConfig.id, out var value))
				{
					value = CustomGameSettings.Instance.GetCurrentMixingSettingLevel(dlcMixingSettingConfig).id;
					dlcSettingIdToLastSetLevelId[dlcMixingSettingConfig.id] = value;
				}
				CustomGameSettings.Instance.SetMixingSetting(dlcMixingSettingConfig, value);
				AddDLCMixingWidget(prefabSettingDlcContent, parent, mixingSetting.Key, dlcMixingSettingConfig);
				flag = true;
			}
			if (mixingSetting.Value is WorldMixingSettingConfig config2)
			{
				AddWorldMixingWidget(prefabSettingCycle, parent2, mixingSetting.Key, config2);
				flag2 = true;
			}
			if (mixingSetting.Value is SubworldMixingSettingConfig config3)
			{
				AddWorldMixingWidget(prefabSettingCycle, parent3, mixingSetting.Key, config3);
				flag3 = true;
			}
		}
		gameObject.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag);
		gameObject2.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag2);
		gameObject3.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag3);
		if (!DlcManager.IsExpansion1Active())
		{
			gameObject2.gameObject.SetActive(value: false);
		}
		gameObject.transform.Find("Title").GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_MIXING);
		ToolTip worldMaxToolTip = gameObject2.transform.Find("Title").GetComponent<ToolTip>();
		Image worldMaxErrorIcon = gameObject2.transform.Find("Title").Find("ImageError").GetComponent<Image>();
		ToolTip subworldMaxToolTip = gameObject3.transform.Find("Title").GetComponent<ToolTip>();
		Image subworldMaxErrorIcon = gameObject3.transform.Find("Title").Find("ImageError").GetComponent<Image>();
		onRefresh = (System.Action)Delegate.Combine(onRefresh, (System.Action)delegate
		{
			bool flag4 = false;
			int currentNumOfGuaranteedWorldMixings = GetCurrentNumOfGuaranteedWorldMixings();
			int maxNumOfGuaranteedWorldMixings = GetMaxNumOfGuaranteedWorldMixings();
			if (currentNumOfGuaranteedWorldMixings > maxNumOfGuaranteedWorldMixings)
			{
				worldMaxToolTip.SetSimpleTooltip(string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_TOO_MANY_GUARENTEED_ASTEROID_MIXINGS, currentNumOfGuaranteedWorldMixings, maxNumOfGuaranteedWorldMixings));
				worldMaxErrorIcon.gameObject.SetActive(value: true);
				flag4 = true;
			}
			else
			{
				worldMaxToolTip.SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_ASTEROID_MIXING);
				worldMaxErrorIcon.gameObject.SetActive(value: false);
			}
			int currentNumOfGuaranteedSubworldMixings = GetCurrentNumOfGuaranteedSubworldMixings();
			int maxNumOfGuaranteedSubworldMixings = GetMaxNumOfGuaranteedSubworldMixings();
			if (currentNumOfGuaranteedSubworldMixings > maxNumOfGuaranteedSubworldMixings)
			{
				subworldMaxToolTip.SetSimpleTooltip(string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_TOO_MANY_GUARENTEED_BIOME_MIXINGS, currentNumOfGuaranteedSubworldMixings, maxNumOfGuaranteedSubworldMixings));
				subworldMaxErrorIcon.gameObject.SetActive(value: true);
				flag4 = true;
			}
			else
			{
				subworldMaxToolTip.SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_BIOME_MIXING);
				subworldMaxErrorIcon.gameObject.SetActive(value: false);
			}
			GameObject gameObject4 = base.transform.parent.Find("Map").Find("MenuTabs").Find("Mixing Tab")
				.Find("ImageError")
				.gameObject;
			ToolTip component = base.transform.parent.Find("Map").Find("MenuTabs").Find("Mixing Tab")
				.GetComponent<ToolTip>();
			GameObject gameObject5 = base.transform.parent.Find("Buttons").Find("LaunchButton").gameObject;
			if (flag4)
			{
				gameObject5.GetComponent<KButton>().isInteractable = false;
				gameObject5.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_CANNOT_START);
				gameObject4.SetActive(value: true);
				component.SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_CANNOT_START);
			}
			else
			{
				gameObject5.GetComponent<KButton>().isInteractable = true;
				gameObject5.GetComponent<ToolTip>().ClearMultiStringTooltip();
				gameObject4.SetActive(value: false);
				component.ClearMultiStringTooltip();
			}
		});
		CustomGameSettings.Instance.OnQualitySettingChanged += OnSettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged += OnSettingChanged;
		CustomGameSettings.Instance.OnMixingSettingChanged += OnSettingChanged;
		onDestroy = (System.Action)Delegate.Combine(onDestroy, (System.Action)delegate
		{
			CustomGameSettings.Instance.OnQualitySettingChanged -= OnSettingChanged;
			CustomGameSettings.Instance.OnStorySettingChanged -= OnSettingChanged;
			CustomGameSettings.Instance.OnMixingSettingChanged -= OnSettingChanged;
		});
		Refresh();
		void OnSettingChanged(SettingConfig config, SettingLevel level)
		{
			onRefresh();
		}
	}

	public override void Uninit()
	{
		if (onDestroy != null)
		{
			onDestroy();
		}
	}

	private GameObject CreateSection(string name)
	{
		GameObject gameObject = Util.KInstantiateUI(prefabMixingSection, contentPanel);
		gameObject.SetActive(value: true);
		gameObject.transform.Find("Title").Find("Title Text").GetComponent<LocText>()
			.SetText(name);
		MultiToggle toggle = gameObject.transform.Find("Title").GetComponent<MultiToggle>();
		MultiToggle multiToggle = toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			int num = ((toggle.CurrentState == 0) ? 1 : 0);
			toggle.ChangeState(num);
			gameObject.transform.Find("Content").gameObject.SetActive(num == 0);
		});
		return gameObject;
	}

	private void AddDLCMixingWidget(GameObject prefab, GameObject parent, string name, DlcMixingSettingConfig config)
	{
		CustomGameSettingWidget widget = Util.KInstantiateUI<CustomGameSettingWidget>(prefab, parent);
		widget.gameObject.name = name;
		widget.gameObject.SetActive(value: true);
		LocText component = widget.transform.Find("Label").GetComponent<LocText>();
		ToolTip component2 = widget.transform.Find("Label").GetComponent<ToolTip>();
		Image component3 = widget.transform.Find("DLC Image").GetComponent<Image>();
		ToolTip component4 = widget.transform.Find("DLC Image").GetComponent<ToolTip>();
		MultiToggle toggle = widget.transform.Find("Checkbox").GetComponent<MultiToggle>();
		ToolTip toggleToolTip = widget.transform.Find("Checkbox").GetComponent<ToolTip>();
		GameObject overlayDisabled = widget.transform.Find("Checkbox").Find("OverlayDisabled").gameObject;
		bool isInteractable = true;
		component.text = config.label;
		string toolTip = (component4.toolTip = config.tooltip);
		component2.toolTip = toolTip;
		string dlcId = config.id;
		Sprite sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcId));
		if (sprite == null)
		{
			sprite = Assets.GetSprite("unknown");
		}
		component3.sprite = sprite;
		MultiToggle multiToggle = toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(OnClick));
		widget.onRefresh += OnRefresh;
		CustomGameSettings.Instance.OnQualitySettingChanged += OnQualitySettingChanged;
		bool didCleanup = false;
		widget.onDestroy += Cleanup;
		onDestroy = (System.Action)Delegate.Combine(onDestroy, new System.Action(Cleanup));
		OnRefresh();
		AddWidget(widget);
		void Cleanup()
		{
			if (!didCleanup)
			{
				didCleanup = true;
				MultiToggle multiToggle2 = toggle;
				multiToggle2.onClick = (System.Action)Delegate.Remove(multiToggle2.onClick, new System.Action(OnClick));
				widget.onRefresh -= OnRefresh;
				CustomGameSettings.Instance.OnQualitySettingChanged -= OnQualitySettingChanged;
			}
		}
		void OnClick()
		{
			if (isInteractable)
			{
				CustomGameSettings.Instance.ToggleMixingSettingLevel(config);
				dlcSettingIdToLastSetLevelId[config.id] = CustomGameSettings.Instance.GetCurrentMixingSettingLevel(config).id;
				widget.Notify();
			}
		}
		void OnQualitySettingChanged(SettingConfig config, SettingLevel level)
		{
			if (config == CustomGameSettingConfigs.ClusterLayout)
			{
				OnRefresh();
			}
		}
		void OnRefresh()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
			if (Array.IndexOf(SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id).requiredDlcIds, dlcId) == -1)
			{
				SettingLevel currentMixingSettingLevel = CustomGameSettings.Instance.GetCurrentMixingSettingLevel(config);
				toggle.ChangeState(config.IsOnLevel(currentMixingSettingLevel.id) ? 1 : 0);
				toggleToolTip.toolTip = currentMixingSettingLevel.tooltip;
				overlayDisabled.SetActive(value: false);
				isInteractable = true;
			}
			else
			{
				SettingLevel on_level = config.on_level;
				toggle.ChangeState(config.IsOnLevel(on_level.id) ? 1 : 0);
				toggleToolTip.toolTip = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED;
				overlayDisabled.SetActive(value: true);
				isInteractable = false;
			}
			settingIdToIsInteractableRecord[config.id] = isInteractable;
		}
	}

	private void AddWorldMixingWidget(GameObject prefab, GameObject parent, string name, MixingSettingConfig config)
	{
		CustomGameSettingWidget widget = Util.KInstantiateUI<CustomGameSettingWidget>(prefab, parent);
		widget.gameObject.name = name;
		widget.gameObject.SetActive(value: true);
		LocText component = widget.transform.Find("Label").GetComponent<LocText>();
		ToolTip component2 = widget.transform.Find("Label").GetComponent<ToolTip>();
		Image component3 = widget.transform.Find("Icon").GetComponent<Image>();
		ToolTip component4 = widget.transform.Find("Icon").GetComponent<ToolTip>();
		LocText valueLabel = widget.transform.Find("Cycler").Find("Box").Find("Value Label")
			.GetComponent<LocText>();
		ToolTip valueToolTip = widget.transform.Find("Cycler").Find("Box").Find("Value Label")
			.GetComponent<ToolTip>();
		KButton cycleLeft = widget.transform.Find("Cycler").Find("Arrow_Left").GetComponent<KButton>();
		KButton cycleRight = widget.transform.Find("Cycler").Find("Arrow_Right").GetComponent<KButton>();
		GameObject overlayDisabled = widget.transform.Find("Cycler").Find("OverlayDisabled").gameObject;
		Image component5 = widget.transform.Find("Banner").GetComponent<Image>();
		bool isInteractable = true;
		component.text = config.label;
		string text = config.tooltip;
		if (config.isModded)
		{
			text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_MODDED_SETTING;
		}
		else if (DlcManager.IsDlcId(config.dlcIdFrom))
		{
			text = text + "\n\n" + string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_CONTENT, DlcManager.GetDlcTitle(config.dlcIdFrom));
		}
		string toolTip = (component4.toolTip = text);
		component2.toolTip = toolTip;
		if (config.isModded)
		{
			component5.gameObject.SetActive(value: false);
		}
		else if (DlcManager.IsDlcId(config.dlcIdFrom))
		{
			component5.color = DlcManager.GetDlcBannerColor(config.dlcIdFrom);
			component5.gameObject.SetActive(value: true);
		}
		else
		{
			component5.gameObject.SetActive(value: false);
		}
		component3.sprite = config.icon;
		cycleLeft.onClick += OnClickLeft;
		cycleRight.onClick += OnClickRight;
		widget.onRefresh += OnRefresh;
		CustomGameSettings.Instance.OnQualitySettingChanged += OnQualitySettingChanged;
		bool didCleanup = false;
		widget.onDestroy += Cleanup;
		onDestroy = (System.Action)Delegate.Combine(onDestroy, new System.Action(Cleanup));
		OnRefresh();
		AddWidget(widget);
		void Cleanup()
		{
			if (!didCleanup)
			{
				didCleanup = true;
				cycleLeft.onClick -= OnClickLeft;
				cycleRight.onClick -= OnClickRight;
				widget.onRefresh -= OnRefresh;
				CustomGameSettings.Instance.OnQualitySettingChanged -= OnQualitySettingChanged;
			}
		}
		static bool IsDlcMixedIn(string dlcId)
		{
			if (CustomGameSettings.Instance.MixingSettings.TryGetValue(dlcId, out var value))
			{
				DlcMixingSettingConfig dlcMixingSettingConfig = (DlcMixingSettingConfig)value;
				return CustomGameSettings.Instance.GetCurrentMixingSettingLevel(dlcId) == dlcMixingSettingConfig.on_level;
			}
			if (dlcId == "EXPANSION1_ID")
			{
				return DlcManager.IsExpansion1Active();
			}
			if (dlcId == "")
			{
				return true;
			}
			return false;
		}
		void OnClickLeft()
		{
			if (isInteractable)
			{
				CustomGameSettings.Instance.CycleMixingSettingLevel(config, -1);
				widget.Notify();
			}
		}
		void OnClickRight()
		{
			if (isInteractable)
			{
				CustomGameSettings.Instance.CycleMixingSettingLevel(config, 1);
				widget.Notify();
			}
		}
		void OnQualitySettingChanged(SettingConfig config, SettingLevel level)
		{
			if (config == CustomGameSettingConfigs.ClusterLayout)
			{
				OnRefresh();
			}
		}
		void OnRefresh()
		{
			string text3 = null;
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
			ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
			bool flag = true;
			if (config.forbiddenClusterTags != null)
			{
				foreach (string forbiddenClusterTag in config.forbiddenClusterTags)
				{
					if (clusterData.clusterTags.Contains(forbiddenClusterTag))
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				text3 = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED;
			}
			else
			{
				bool flag2 = true;
				if (config.required_content != null)
				{
					string[] required_content = config.required_content;
					for (int i = 0; i < required_content.Length; i++)
					{
						if (!IsDlcMixedIn(required_content[i]))
						{
							flag2 = false;
							break;
						}
					}
				}
				if (!flag2)
				{
					text3 = string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_LOCKED_REQUIRE_DLC_NOT_ENABLED, string.Join("\n", config.required_content.Select((string dlcId) => "    • " + DlcManager.GetDlcTitle(dlcId))));
				}
			}
			if (text3 == null)
			{
				SettingLevel currentMixingSettingLevel = CustomGameSettings.Instance.GetCurrentMixingSettingLevel(config);
				valueLabel.text = currentMixingSettingLevel.label;
				valueToolTip.toolTip = currentMixingSettingLevel.tooltip;
				cycleLeft.isInteractable = !config.IsFirstLevel(currentMixingSettingLevel.id);
				cycleRight.isInteractable = !config.IsLastLevel(currentMixingSettingLevel.id);
				overlayDisabled.SetActive(value: false);
				isInteractable = true;
			}
			else
			{
				SettingLevel settingLevel = config.levels[0];
				valueLabel.text = settingLevel.label;
				valueToolTip.toolTip = text3;
				cycleLeft.isInteractable = false;
				cycleRight.isInteractable = false;
				overlayDisabled.SetActive(value: true);
				isInteractable = false;
			}
			settingIdToIsInteractableRecord[config.id] = isInteractable;
		}
	}

	public override void Refresh()
	{
		base.Refresh();
		RectTransform component = contentPanel.GetComponent<RectTransform>();
		component.offsetMin = new Vector2(0f, component.offsetMin.y);
		component.offsetMax = new Vector2(0f, component.offsetMax.y);
		if (onRefresh != null)
		{
			onRefresh();
		}
	}

	public int GetMaxNumOfGuaranteedWorldMixings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		int num = 0;
		foreach (WorldPlacement worldPlacement in clusterData.worldPlacements)
		{
			if (worldPlacement.IsMixingPlacement())
			{
				num++;
			}
		}
		return num;
	}

	public int GetCurrentNumOfGuaranteedWorldMixings()
	{
		int num = 0;
		foreach (KeyValuePair<string, SettingConfig> mixingSetting in CustomGameSettings.Instance.MixingSettings)
		{
			if (mixingSetting.Value.ShowInUI() && (!settingIdToIsInteractableRecord.TryGetValue(mixingSetting.Value.id, out var value) || value) && mixingSetting.Value is WorldMixingSettingConfig setting && CustomGameSettings.Instance.GetCurrentMixingSettingLevel(setting).id == "GuranteeMixing")
			{
				num++;
			}
		}
		return num;
	}

	public int GetMaxNumOfGuaranteedSubworldMixings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		int num = 0;
		foreach (WorldPlacement worldPlacement in clusterData.worldPlacements)
		{
			num += SettingsCache.worlds.GetWorldData(worldPlacement.world).subworldMixingRules.Count;
		}
		return num;
	}

	public int GetCurrentNumOfGuaranteedSubworldMixings()
	{
		int num = 0;
		foreach (KeyValuePair<string, SettingConfig> mixingSetting in CustomGameSettings.Instance.MixingSettings)
		{
			if (mixingSetting.Value.ShowInUI() && (!settingIdToIsInteractableRecord.TryGetValue(mixingSetting.Value.id, out var value) || value) && mixingSetting.Value is SubworldMixingSettingConfig setting && CustomGameSettings.Instance.GetCurrentMixingSettingLevel(setting).id == "GuranteeMixing")
			{
				num++;
			}
		}
		return num;
	}
}
