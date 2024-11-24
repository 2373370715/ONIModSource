using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E34 RID: 7732
public class MixingContentPanel : CustomGameSettingsPanelBase
{
	// Token: 0x0600A1FF RID: 41471 RVA: 0x003DAB30 File Offset: 0x003D8D30
	public override void Init()
	{
		MixingContentPanel.<>c__DisplayClass8_0 CS$<>8__locals1 = new MixingContentPanel.<>c__DisplayClass8_0();
		CS$<>8__locals1.<>4__this = this;
		this.prefabMixingSection.SetActive(false);
		this.prefabMixingSection.transform.Find("Title").Find("ImageError").gameObject.SetActive(false);
		this.prefabMixingSection.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(false);
		this.prefabSettingDlcContent.SetActive(false);
		this.prefabSettingCycle.SetActive(false);
		GameObject gameObject = this.CreateSection(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_DLC_HEADER);
		GameObject gameObject2 = this.CreateSection(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_WORLDMIXING_HEADER);
		GameObject gameObject3 = this.CreateSection(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_SUBWORLDMIXING_HEADER);
		GameObject gameObject4 = gameObject.transform.Find("Content").Find("Grid").gameObject;
		GameObject gameObject5 = gameObject2.transform.Find("Content").Find("Grid").gameObject;
		GameObject gameObject6 = gameObject3.transform.Find("Content").Find("Grid").gameObject;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in CustomGameSettings.Instance.MixingSettings)
		{
			if (keyValuePair.Value.ShowInUI())
			{
				DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
				if (dlcMixingSettingConfig != null)
				{
					string id;
					if (!MixingContentPanel.dlcSettingIdToLastSetLevelId.TryGetValue(dlcMixingSettingConfig.id, out id))
					{
						id = CustomGameSettings.Instance.GetCurrentMixingSettingLevel(dlcMixingSettingConfig).id;
						MixingContentPanel.dlcSettingIdToLastSetLevelId[dlcMixingSettingConfig.id] = id;
					}
					CustomGameSettings.Instance.SetMixingSetting(dlcMixingSettingConfig, id);
					this.AddDLCMixingWidget(this.prefabSettingDlcContent, gameObject4, keyValuePair.Key, dlcMixingSettingConfig);
					flag = true;
				}
				WorldMixingSettingConfig worldMixingSettingConfig = keyValuePair.Value as WorldMixingSettingConfig;
				if (worldMixingSettingConfig != null)
				{
					this.AddWorldMixingWidget(this.prefabSettingCycle, gameObject5, keyValuePair.Key, worldMixingSettingConfig);
					flag2 = true;
				}
				SubworldMixingSettingConfig subworldMixingSettingConfig = keyValuePair.Value as SubworldMixingSettingConfig;
				if (subworldMixingSettingConfig != null)
				{
					this.AddWorldMixingWidget(this.prefabSettingCycle, gameObject6, keyValuePair.Key, subworldMixingSettingConfig);
					flag3 = true;
				}
			}
		}
		gameObject.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag);
		gameObject2.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag2);
		gameObject3.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag3);
		if (!DlcManager.IsExpansion1Active())
		{
			gameObject2.gameObject.SetActive(false);
		}
		gameObject.transform.Find("Title").GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_MIXING);
		CS$<>8__locals1.worldMaxToolTip = gameObject2.transform.Find("Title").GetComponent<ToolTip>();
		CS$<>8__locals1.worldMaxErrorIcon = gameObject2.transform.Find("Title").Find("ImageError").GetComponent<Image>();
		CS$<>8__locals1.subworldMaxToolTip = gameObject3.transform.Find("Title").GetComponent<ToolTip>();
		CS$<>8__locals1.subworldMaxErrorIcon = gameObject3.transform.Find("Title").Find("ImageError").GetComponent<Image>();
		this.onRefresh = (System.Action)Delegate.Combine(this.onRefresh, new System.Action(delegate()
		{
			bool flag4 = false;
			int currentNumOfGuaranteedWorldMixings = CS$<>8__locals1.<>4__this.GetCurrentNumOfGuaranteedWorldMixings();
			int maxNumOfGuaranteedWorldMixings = CS$<>8__locals1.<>4__this.GetMaxNumOfGuaranteedWorldMixings();
			if (currentNumOfGuaranteedWorldMixings > maxNumOfGuaranteedWorldMixings)
			{
				CS$<>8__locals1.worldMaxToolTip.SetSimpleTooltip(string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_TOO_MANY_GUARENTEED_ASTEROID_MIXINGS, currentNumOfGuaranteedWorldMixings, maxNumOfGuaranteedWorldMixings));
				CS$<>8__locals1.worldMaxErrorIcon.gameObject.SetActive(true);
				flag4 = true;
			}
			else
			{
				CS$<>8__locals1.worldMaxToolTip.SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_ASTEROID_MIXING);
				CS$<>8__locals1.worldMaxErrorIcon.gameObject.SetActive(false);
			}
			int currentNumOfGuaranteedSubworldMixings = CS$<>8__locals1.<>4__this.GetCurrentNumOfGuaranteedSubworldMixings();
			int maxNumOfGuaranteedSubworldMixings = CS$<>8__locals1.<>4__this.GetMaxNumOfGuaranteedSubworldMixings();
			if (currentNumOfGuaranteedSubworldMixings > maxNumOfGuaranteedSubworldMixings)
			{
				CS$<>8__locals1.subworldMaxToolTip.SetSimpleTooltip(string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_TOO_MANY_GUARENTEED_BIOME_MIXINGS, currentNumOfGuaranteedSubworldMixings, maxNumOfGuaranteedSubworldMixings));
				CS$<>8__locals1.subworldMaxErrorIcon.gameObject.SetActive(true);
				flag4 = true;
			}
			else
			{
				CS$<>8__locals1.subworldMaxToolTip.SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_BIOME_MIXING);
				CS$<>8__locals1.subworldMaxErrorIcon.gameObject.SetActive(false);
			}
			GameObject gameObject7 = CS$<>8__locals1.<>4__this.transform.parent.Find("Map").Find("MenuTabs").Find("Mixing Tab").Find("ImageError").gameObject;
			ToolTip component = CS$<>8__locals1.<>4__this.transform.parent.Find("Map").Find("MenuTabs").Find("Mixing Tab").GetComponent<ToolTip>();
			GameObject gameObject8 = CS$<>8__locals1.<>4__this.transform.parent.Find("Buttons").Find("LaunchButton").gameObject;
			if (flag4)
			{
				gameObject8.GetComponent<KButton>().isInteractable = false;
				gameObject8.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_CANNOT_START);
				gameObject7.SetActive(true);
				component.SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_CANNOT_START);
				return;
			}
			gameObject8.GetComponent<KButton>().isInteractable = true;
			gameObject8.GetComponent<ToolTip>().ClearMultiStringTooltip();
			gameObject7.SetActive(false);
			component.ClearMultiStringTooltip();
		}));
		CustomGameSettings.Instance.OnQualitySettingChanged += CS$<>8__locals1.<Init>g__OnSettingChanged|1;
		CustomGameSettings.Instance.OnStorySettingChanged += CS$<>8__locals1.<Init>g__OnSettingChanged|1;
		CustomGameSettings.Instance.OnMixingSettingChanged += CS$<>8__locals1.<Init>g__OnSettingChanged|1;
		this.onDestroy = (System.Action)Delegate.Combine(this.onDestroy, new System.Action(delegate()
		{
			CustomGameSettings.Instance.OnQualitySettingChanged -= base.<Init>g__OnSettingChanged|1;
			CustomGameSettings.Instance.OnStorySettingChanged -= base.<Init>g__OnSettingChanged|1;
			CustomGameSettings.Instance.OnMixingSettingChanged -= base.<Init>g__OnSettingChanged|1;
		}));
		this.Refresh();
	}

	// Token: 0x0600A200 RID: 41472 RVA: 0x001092A9 File Offset: 0x001074A9
	public override void Uninit()
	{
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
	}

	// Token: 0x0600A201 RID: 41473 RVA: 0x003DAF48 File Offset: 0x003D9148
	private GameObject CreateSection(string name)
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.prefabMixingSection, this.contentPanel, false);
		gameObject.SetActive(true);
		gameObject.transform.Find("Title").Find("Title Text").GetComponent<LocText>().SetText(name);
		MultiToggle toggle = gameObject.transform.Find("Title").GetComponent<MultiToggle>();
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			int num = (toggle.CurrentState == 0) ? 1 : 0;
			toggle.ChangeState(num);
			gameObject.transform.Find("Content").gameObject.SetActive(num == 0);
		}));
		return gameObject;
	}

	// Token: 0x0600A202 RID: 41474 RVA: 0x003DAFF8 File Offset: 0x003D91F8
	private void AddDLCMixingWidget(GameObject prefab, GameObject parent, string name, DlcMixingSettingConfig config)
	{
		MixingContentPanel.<>c__DisplayClass11_0 CS$<>8__locals1 = new MixingContentPanel.<>c__DisplayClass11_0();
		CS$<>8__locals1.config = config;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.widget = global::Util.KInstantiateUI<CustomGameSettingWidget>(prefab, parent, false);
		CS$<>8__locals1.widget.gameObject.name = name;
		CS$<>8__locals1.widget.gameObject.SetActive(true);
		LocText component = CS$<>8__locals1.widget.transform.Find("Label").GetComponent<LocText>();
		ToolTip component2 = CS$<>8__locals1.widget.transform.Find("Label").GetComponent<ToolTip>();
		Image component3 = CS$<>8__locals1.widget.transform.Find("DLC Image").GetComponent<Image>();
		ToolTip component4 = CS$<>8__locals1.widget.transform.Find("DLC Image").GetComponent<ToolTip>();
		CS$<>8__locals1.toggle = CS$<>8__locals1.widget.transform.Find("Checkbox").GetComponent<MultiToggle>();
		CS$<>8__locals1.toggleToolTip = CS$<>8__locals1.widget.transform.Find("Checkbox").GetComponent<ToolTip>();
		CS$<>8__locals1.overlayDisabled = CS$<>8__locals1.widget.transform.Find("Checkbox").Find("OverlayDisabled").gameObject;
		CS$<>8__locals1.isInteractable = true;
		component.text = CS$<>8__locals1.config.label;
		component2.toolTip = (component4.toolTip = CS$<>8__locals1.config.tooltip);
		CS$<>8__locals1.dlcId = CS$<>8__locals1.config.id;
		Sprite sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(CS$<>8__locals1.dlcId));
		if (sprite == null)
		{
			sprite = Assets.GetSprite("unknown");
		}
		component3.sprite = sprite;
		MultiToggle toggle = CS$<>8__locals1.toggle;
		toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(CS$<>8__locals1.<AddDLCMixingWidget>g__OnClick|0));
		CS$<>8__locals1.widget.onRefresh += CS$<>8__locals1.<AddDLCMixingWidget>g__OnRefresh|1;
		CustomGameSettings.Instance.OnQualitySettingChanged += CS$<>8__locals1.<AddDLCMixingWidget>g__OnQualitySettingChanged|2;
		CS$<>8__locals1.didCleanup = false;
		CS$<>8__locals1.widget.onDestroy += CS$<>8__locals1.<AddDLCMixingWidget>g__Cleanup|3;
		this.onDestroy = (System.Action)Delegate.Combine(this.onDestroy, new System.Action(CS$<>8__locals1.<AddDLCMixingWidget>g__Cleanup|3));
		CS$<>8__locals1.<AddDLCMixingWidget>g__OnRefresh|1();
		base.AddWidget(CS$<>8__locals1.widget);
	}

	// Token: 0x0600A203 RID: 41475 RVA: 0x003DB244 File Offset: 0x003D9444
	private void AddWorldMixingWidget(GameObject prefab, GameObject parent, string name, MixingSettingConfig config)
	{
		MixingContentPanel.<>c__DisplayClass12_0 CS$<>8__locals1 = new MixingContentPanel.<>c__DisplayClass12_0();
		CS$<>8__locals1.config = config;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.widget = global::Util.KInstantiateUI<CustomGameSettingWidget>(prefab, parent, false);
		CS$<>8__locals1.widget.gameObject.name = name;
		CS$<>8__locals1.widget.gameObject.SetActive(true);
		TMP_Text component = CS$<>8__locals1.widget.transform.Find("Label").GetComponent<LocText>();
		ToolTip component2 = CS$<>8__locals1.widget.transform.Find("Label").GetComponent<ToolTip>();
		Image component3 = CS$<>8__locals1.widget.transform.Find("Icon").GetComponent<Image>();
		ToolTip component4 = CS$<>8__locals1.widget.transform.Find("Icon").GetComponent<ToolTip>();
		CS$<>8__locals1.valueLabel = CS$<>8__locals1.widget.transform.Find("Cycler").Find("Box").Find("Value Label").GetComponent<LocText>();
		CS$<>8__locals1.valueToolTip = CS$<>8__locals1.widget.transform.Find("Cycler").Find("Box").Find("Value Label").GetComponent<ToolTip>();
		CS$<>8__locals1.cycleLeft = CS$<>8__locals1.widget.transform.Find("Cycler").Find("Arrow_Left").GetComponent<KButton>();
		CS$<>8__locals1.cycleRight = CS$<>8__locals1.widget.transform.Find("Cycler").Find("Arrow_Right").GetComponent<KButton>();
		CS$<>8__locals1.overlayDisabled = CS$<>8__locals1.widget.transform.Find("Cycler").Find("OverlayDisabled").gameObject;
		Image component5 = CS$<>8__locals1.widget.transform.Find("Banner").GetComponent<Image>();
		CS$<>8__locals1.isInteractable = true;
		component.text = CS$<>8__locals1.config.label;
		string text = CS$<>8__locals1.config.tooltip;
		if (CS$<>8__locals1.config.isModded)
		{
			text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_MODDED_SETTING;
		}
		else if (DlcManager.IsDlcId(CS$<>8__locals1.config.dlcIdFrom))
		{
			text = text + "\n\n" + string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_CONTENT, DlcManager.GetDlcTitle(CS$<>8__locals1.config.dlcIdFrom));
		}
		component2.toolTip = (component4.toolTip = text);
		if (CS$<>8__locals1.config.isModded)
		{
			component5.gameObject.SetActive(false);
		}
		else if (DlcManager.IsDlcId(CS$<>8__locals1.config.dlcIdFrom))
		{
			component5.color = DlcManager.GetDlcBannerColor(CS$<>8__locals1.config.dlcIdFrom);
			component5.gameObject.SetActive(true);
		}
		else
		{
			component5.gameObject.SetActive(false);
		}
		component3.sprite = CS$<>8__locals1.config.icon;
		CS$<>8__locals1.cycleLeft.onClick += CS$<>8__locals1.<AddWorldMixingWidget>g__OnClickLeft|0;
		CS$<>8__locals1.cycleRight.onClick += CS$<>8__locals1.<AddWorldMixingWidget>g__OnClickRight|1;
		CS$<>8__locals1.widget.onRefresh += CS$<>8__locals1.<AddWorldMixingWidget>g__OnRefresh|3;
		CustomGameSettings.Instance.OnQualitySettingChanged += CS$<>8__locals1.<AddWorldMixingWidget>g__OnQualitySettingChanged|4;
		CS$<>8__locals1.didCleanup = false;
		CS$<>8__locals1.widget.onDestroy += CS$<>8__locals1.<AddWorldMixingWidget>g__Cleanup|5;
		this.onDestroy = (System.Action)Delegate.Combine(this.onDestroy, new System.Action(CS$<>8__locals1.<AddWorldMixingWidget>g__Cleanup|5));
		CS$<>8__locals1.<AddWorldMixingWidget>g__OnRefresh|3();
		base.AddWidget(CS$<>8__locals1.widget);
	}

	// Token: 0x0600A204 RID: 41476 RVA: 0x003DB5C0 File Offset: 0x003D97C0
	public override void Refresh()
	{
		base.Refresh();
		RectTransform component = this.contentPanel.GetComponent<RectTransform>();
		component.offsetMin = new Vector2(0f, component.offsetMin.y);
		component.offsetMax = new Vector2(0f, component.offsetMax.y);
		if (this.onRefresh != null)
		{
			this.onRefresh();
		}
	}

	// Token: 0x0600A205 RID: 41477 RVA: 0x003DB628 File Offset: 0x003D9828
	public int GetMaxNumOfGuaranteedWorldMixings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		int num = 0;
		using (List<WorldPlacement>.Enumerator enumerator = clusterData.worldPlacements.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsMixingPlacement())
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x0600A206 RID: 41478 RVA: 0x003DB6A0 File Offset: 0x003D98A0
	public int GetCurrentNumOfGuaranteedWorldMixings()
	{
		int num = 0;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in CustomGameSettings.Instance.MixingSettings)
		{
			bool flag;
			if (keyValuePair.Value.ShowInUI() && (!this.settingIdToIsInteractableRecord.TryGetValue(keyValuePair.Value.id, out flag) || flag))
			{
				WorldMixingSettingConfig worldMixingSettingConfig = keyValuePair.Value as WorldMixingSettingConfig;
				if (worldMixingSettingConfig != null && CustomGameSettings.Instance.GetCurrentMixingSettingLevel(worldMixingSettingConfig).id == "GuranteeMixing")
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x0600A207 RID: 41479 RVA: 0x003DB754 File Offset: 0x003D9954
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

	// Token: 0x0600A208 RID: 41480 RVA: 0x003DB7E0 File Offset: 0x003D99E0
	public int GetCurrentNumOfGuaranteedSubworldMixings()
	{
		int num = 0;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in CustomGameSettings.Instance.MixingSettings)
		{
			bool flag;
			if (keyValuePair.Value.ShowInUI() && (!this.settingIdToIsInteractableRecord.TryGetValue(keyValuePair.Value.id, out flag) || flag))
			{
				SubworldMixingSettingConfig subworldMixingSettingConfig = keyValuePair.Value as SubworldMixingSettingConfig;
				if (subworldMixingSettingConfig != null && CustomGameSettings.Instance.GetCurrentMixingSettingLevel(subworldMixingSettingConfig).id == "GuranteeMixing")
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x0600A20B RID: 41483 RVA: 0x003DB894 File Offset: 0x003D9A94
	[CompilerGenerated]
	internal static bool <AddWorldMixingWidget>g__IsDlcMixedIn|12_2(string dlcId)
	{
		SettingConfig settingConfig;
		if (CustomGameSettings.Instance.MixingSettings.TryGetValue(dlcId, out settingConfig))
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = (DlcMixingSettingConfig)settingConfig;
			return CustomGameSettings.Instance.GetCurrentMixingSettingLevel(dlcId) == dlcMixingSettingConfig.on_level;
		}
		if (dlcId == "EXPANSION1_ID")
		{
			return DlcManager.IsExpansion1Active();
		}
		return dlcId == "";
	}

	// Token: 0x04007E50 RID: 32336
	[SerializeField]
	private GameObject prefabMixingSection;

	// Token: 0x04007E51 RID: 32337
	[SerializeField]
	private GameObject prefabSettingCycle;

	// Token: 0x04007E52 RID: 32338
	[SerializeField]
	private GameObject prefabSettingDlcContent;

	// Token: 0x04007E53 RID: 32339
	[SerializeField]
	private GameObject contentPanel;

	// Token: 0x04007E54 RID: 32340
	private static Dictionary<string, string> dlcSettingIdToLastSetLevelId = new Dictionary<string, string>();

	// Token: 0x04007E55 RID: 32341
	private Dictionary<string, bool> settingIdToIsInteractableRecord = new Dictionary<string, bool>();

	// Token: 0x04007E56 RID: 32342
	private System.Action onRefresh;

	// Token: 0x04007E57 RID: 32343
	private System.Action onDestroy;
}
