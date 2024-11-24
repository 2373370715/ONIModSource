using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001C6B RID: 7275
public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
	// Token: 0x060097AD RID: 38829 RVA: 0x003ACB00 File Offset: 0x003AAD00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backButton.onClick += this.BackClicked;
		this.customizeButton.onClick += this.CustomizeClicked;
		this.launchButton.onClick += this.LaunchClicked;
		this.shuffleButton.onClick += this.ShuffleClicked;
		this.storyTraitShuffleButton.onClick += this.StoryTraitShuffleClicked;
		this.storyTraitShuffleButton.gameObject.SetActive(Db.Get().Stories.Count > 5);
		this.destinationMapPanel.OnAsteroidClicked += this.OnAsteroidClicked;
		KInputTextField kinputTextField = this.coordinate;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.CoordinateEditStarted));
		this.coordinate.onEndEdit.AddListener(new UnityAction<string>(this.CoordinateEditFinished));
		if (this.locationIcons != null)
		{
			bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
			this.locationIcons.gameObject.SetActive(cloudSavesAvailable);
		}
		this.random = new KRandom();
	}

	// Token: 0x060097AE RID: 38830 RVA: 0x003ACC34 File Offset: 0x003AAE34
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshCloudSavePref();
		this.RefreshCloudLocalIcon();
		this.newGameSettingsPanel.Init();
		this.newGameSettingsPanel.SetCloseAction(new System.Action(this.CustomizeClose));
		this.destinationMapPanel.Init();
		this.mixingPanel.Init();
		this.ShuffleClicked();
		this.RefreshMenuTabs();
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			int target = i;
			this.menuTabs[i].onClick = delegate()
			{
				this.selectedMenuTabIdx = target;
				this.RefreshMenuTabs();
			};
		}
		this.ResizeLayout();
		this.storyContentPanel.Init();
		this.storyContentPanel.SelectRandomStories(5, 5, true);
		this.storyContentPanel.SelectDefault();
		this.RefreshStoryLabel();
		this.RefreshRowsAndDescriptions();
		CustomGameSettings.Instance.OnQualitySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnMixingSettingChanged += this.QualitySettingChanged;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x060097AF RID: 38831 RVA: 0x003ACD64 File Offset: 0x003AAF64
	private void ResizeLayout()
	{
		Vector2 sizeDelta = this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta;
		this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta = new Vector2(sizeDelta.x, (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76));
		Vector2 sizeDelta2 = this.worldsScrollPanel.rectTransform().sizeDelta;
		Vector2 anchoredPosition = this.worldsScrollPanel.rectTransform().anchoredPosition;
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.worldsScrollPanel.rectTransform().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 88f);
		}
		float num = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 436 : 524);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		num = Mathf.Min(num, this.destinationInfoPanel.sizeDelta.y - (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76) - 22f);
		this.worldsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.storyScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.mixingScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.gameSettingsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
	}

	// Token: 0x060097B0 RID: 38832 RVA: 0x003ACECC File Offset: 0x003AB0CC
	protected override void OnCleanUp()
	{
		CustomGameSettings.Instance.OnQualitySettingChanged -= this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged -= this.QualitySettingChanged;
		this.newGameSettingsPanel.Uninit();
		this.destinationMapPanel.Uninit();
		this.mixingPanel.Uninit();
		this.storyContentPanel.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x060097B1 RID: 38833 RVA: 0x003ACF38 File Offset: 0x003AB138
	private void RefreshCloudLocalIcon()
	{
		if (this.locationIcons == null)
		{
			return;
		}
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		HierarchyReferences component = this.locationIcons.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
		KButton component3 = component.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
		KButton component4 = component.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
		ToolTip component5 = component3.GetComponent<ToolTip>();
		ToolTip component6 = component4.GetComponent<ToolTip>();
		component5.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		component6.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_LOCAL, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		component2.text = (flag ? UI.FRONTEND.LOADSCREEN.CLOUD_SAVE : UI.FRONTEND.LOADSCREEN.LOCAL_SAVE);
		component3.gameObject.SetActive(flag);
		component3.ClearOnClick();
		if (flag)
		{
			component3.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Disabled");
				this.RefreshCloudLocalIcon();
			};
		}
		component4.gameObject.SetActive(!flag);
		component4.ClearOnClick();
		if (!flag)
		{
			component4.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Enabled");
				this.RefreshCloudLocalIcon();
			};
		}
	}

	// Token: 0x060097B2 RID: 38834 RVA: 0x003AD06C File Offset: 0x003AB26C
	private void RefreshCloudSavePref()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavesDefaultPref = SaveLoader.GetCloudSavesDefaultPref();
		CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, cloudSavesDefaultPref);
	}

	// Token: 0x060097B3 RID: 38835 RVA: 0x00102931 File Offset: 0x00100B31
	private void BackClicked()
	{
		this.newGameSettingsPanel.Cancel();
		base.NavigateBackward();
	}

	// Token: 0x060097B4 RID: 38836 RVA: 0x00102944 File Offset: 0x00100B44
	private void CustomizeClicked()
	{
		this.newGameSettingsPanel.Refresh();
		this.customSettings.SetActive(true);
	}

	// Token: 0x060097B5 RID: 38837 RVA: 0x0010295D File Offset: 0x00100B5D
	private void CustomizeClose()
	{
		this.customSettings.SetActive(false);
	}

	// Token: 0x060097B6 RID: 38838 RVA: 0x0010296B File Offset: 0x00100B6B
	private void LaunchClicked()
	{
		CustomGameSettings.Instance.RemoveInvalidMixingSettings();
		base.NavigateForward();
	}

	// Token: 0x060097B7 RID: 38839 RVA: 0x003AD098 File Offset: 0x003AB298
	private void RefreshMenuTabs()
	{
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			this.menuTabs[i].ChangeState((i == this.selectedMenuTabIdx) ? 1 : 0);
			LocText componentInChildren = this.menuTabs[i].GetComponentInChildren<LocText>();
			HierarchyReferences component = this.menuTabs[i].GetComponent<HierarchyReferences>();
			if (componentInChildren != null)
			{
				componentInChildren.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
			}
			if (component != null)
			{
				Image reference = component.GetReference<Image>("Icon");
				if (reference != null)
				{
					reference.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
				}
			}
		}
		this.destinationInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 1);
		this.storyInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 2);
		this.mixingSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 3);
		this.gameSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 4);
		int num = this.selectedMenuTabIdx;
		if (num != 1)
		{
			if (num == 2)
			{
				this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Story);
			}
		}
		else
		{
			this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Asteroid);
		}
		this.destinationDetailsHeader.SetAsFirstSibling();
	}

	// Token: 0x060097B8 RID: 38840 RVA: 0x003AD1F8 File Offset: 0x003AB3F8
	private void ShuffleClicked()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		int num = this.random.Next();
		if (currentClusterLayout != null && currentClusterLayout.fixedCoordinate != -1)
		{
			num = currentClusterLayout.fixedCoordinate;
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, num.ToString(), true);
	}

	// Token: 0x060097B9 RID: 38841 RVA: 0x0010297D File Offset: 0x00100B7D
	private void StoryTraitShuffleClicked()
	{
		this.storyContentPanel.SelectRandomStories(5, 5, false);
	}

	// Token: 0x060097BA RID: 38842 RVA: 0x003AD248 File Offset: 0x003AB448
	private void CoordinateChanged(string text)
	{
		string[] array = CustomGameSettings.ParseSettingCoordinate(text);
		if (array.Length < 4 || array.Length > 6)
		{
			return;
		}
		int num;
		if (!int.TryParse(array[2], out num))
		{
			return;
		}
		ClusterLayout clusterLayout = null;
		foreach (string name in SettingsCache.GetClusterNames())
		{
			ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(name);
			if (clusterData.coordinatePrefix == array[1])
			{
				clusterLayout = clusterData;
			}
		}
		if (clusterLayout != null)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, clusterLayout.filePath, true);
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, array[2], true);
		this.newGameSettingsPanel.ConsumeSettingsCode(array[3]);
		string code = (array.Length >= 5) ? array[4] : "0";
		this.newGameSettingsPanel.ConsumeStoryTraitsCode(code);
		string code2 = (array.Length >= 6) ? array[5] : "0";
		this.newGameSettingsPanel.ConsumeMixingSettingsCode(code2);
	}

	// Token: 0x060097BB RID: 38843 RVA: 0x0010298D File Offset: 0x00100B8D
	private void CoordinateEditStarted()
	{
		this.isEditingCoordinate = true;
	}

	// Token: 0x060097BC RID: 38844 RVA: 0x00102996 File Offset: 0x00100B96
	private void CoordinateEditFinished(string text)
	{
		this.CoordinateChanged(text);
		this.isEditingCoordinate = false;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x060097BD RID: 38845 RVA: 0x003AD350 File Offset: 0x003AB550
	private void QualitySettingChanged(SettingConfig config, SettingLevel level)
	{
		if (config == CustomGameSettingConfigs.SaveToCloud)
		{
			this.RefreshCloudLocalIcon();
		}
		if (!this.destinationDetailsHeader.IsNullOrDestroyed())
		{
			if (!this.isEditingCoordinate && !this.coordinate.IsNullOrDestroyed())
			{
				this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
			}
			this.RefreshRowsAndDescriptions();
		}
	}

	// Token: 0x060097BE RID: 38846 RVA: 0x003AD3A8 File Offset: 0x003AB5A8
	public void RefreshRowsAndDescriptions()
	{
		string setting = this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);
		int seed;
		int.TryParse(this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.WorldgenSeed), out seed);
		int fixedCoordinate = CustomGameSettings.Instance.GetCurrentClusterLayout().fixedCoordinate;
		if (fixedCoordinate != -1)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, fixedCoordinate.ToString(), false);
			seed = fixedCoordinate;
			this.shuffleButton.isInteractable = false;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP_DISABLED);
		}
		else
		{
			this.coordinate.interactable = true;
			this.shuffleButton.isInteractable = true;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP);
		}
		ColonyDestinationAsteroidBeltData cluster;
		try
		{
			cluster = this.destinationMapPanel.SelectCluster(setting, seed);
		}
		catch
		{
			string defaultAsteroid = this.destinationMapPanel.GetDefaultAsteroid();
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, defaultAsteroid, true);
			cluster = this.destinationMapPanel.SelectCluster(defaultAsteroid, seed);
		}
		if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
		{
			this.destinationProperties.EnableClusterLocationLabels(true);
			this.destinationProperties.RefreshAsteroidLines(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			this.destinationProperties.EnableClusterDetails(true);
			this.destinationProperties.SetClusterDetailLabels(cluster);
			this.selectedLocationProperties.headerLabel.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.SELECTED_CLUSTER_TRAITS_HEADER);
			this.destinationProperties.clusterDetailsButton.onClick = delegate()
			{
				this.destinationProperties.SelectWholeClusterDetails(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			};
		}
		else
		{
			this.destinationProperties.EnableClusterDetails(false);
			this.destinationProperties.EnableClusterLocationLabels(false);
			this.destinationProperties.SetParameterDescriptors(cluster.GetParamDescriptors());
			this.selectedLocationProperties.SetTraitDescriptors(cluster.GetTraitDescriptors(), this.storyContentPanel.GetActiveStories(), true);
		}
		this.RefreshStoryLabel();
	}

	// Token: 0x060097BF RID: 38847 RVA: 0x001029BB File Offset: 0x00100BBB
	public void RefreshStoryLabel()
	{
		this.storyTraitsDestinationDetailsLabel.SetText(this.storyContentPanel.GetTraitsString(false));
		this.storyTraitsDestinationDetailsLabel.GetComponent<ToolTip>().SetSimpleTooltip(this.storyContentPanel.GetTraitsString(true));
	}

	// Token: 0x060097C0 RID: 38848 RVA: 0x001029F0 File Offset: 0x00100BF0
	private void OnAsteroidClicked(ColonyDestinationAsteroidBeltData cluster)
	{
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, cluster.beltPath, true);
		this.ShuffleClicked();
	}

	// Token: 0x060097C1 RID: 38849 RVA: 0x003AD5B8 File Offset: 0x003AB7B8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isEditingCoordinate)
		{
			return;
		}
		if (!e.Consumed && e.TryConsume(global::Action.PanLeft))
		{
			this.destinationMapPanel.ScrollLeft();
		}
		else if (!e.Consumed && e.TryConsume(global::Action.PanRight))
		{
			this.destinationMapPanel.ScrollRight();
		}
		else if (this.customSettings.activeSelf && !e.Consumed && (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)))
		{
			this.CustomizeClose();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x040075A8 RID: 30120
	[SerializeField]
	private GameObject destinationMap;

	// Token: 0x040075A9 RID: 30121
	[SerializeField]
	private GameObject customSettings;

	// Token: 0x040075AA RID: 30122
	[Header("Menu")]
	[SerializeField]
	private MultiToggle[] menuTabs;

	// Token: 0x040075AB RID: 30123
	private int selectedMenuTabIdx = 1;

	// Token: 0x040075AC RID: 30124
	[Header("Buttons")]
	[SerializeField]
	private KButton backButton;

	// Token: 0x040075AD RID: 30125
	[SerializeField]
	private KButton customizeButton;

	// Token: 0x040075AE RID: 30126
	[SerializeField]
	private KButton launchButton;

	// Token: 0x040075AF RID: 30127
	[SerializeField]
	private KButton shuffleButton;

	// Token: 0x040075B0 RID: 30128
	[SerializeField]
	private KButton storyTraitShuffleButton;

	// Token: 0x040075B1 RID: 30129
	[Header("Scroll Panels")]
	[SerializeField]
	private RectTransform worldsScrollPanel;

	// Token: 0x040075B2 RID: 30130
	[SerializeField]
	private RectTransform storyScrollPanel;

	// Token: 0x040075B3 RID: 30131
	[SerializeField]
	private RectTransform mixingScrollPanel;

	// Token: 0x040075B4 RID: 30132
	[SerializeField]
	private RectTransform gameSettingsScrollPanel;

	// Token: 0x040075B5 RID: 30133
	[Header("Panels")]
	[SerializeField]
	private RectTransform destinationDetailsHeader;

	// Token: 0x040075B6 RID: 30134
	[SerializeField]
	private RectTransform destinationInfoPanel;

	// Token: 0x040075B7 RID: 30135
	[SerializeField]
	private RectTransform storyInfoPanel;

	// Token: 0x040075B8 RID: 30136
	[SerializeField]
	private RectTransform mixingSettingsPanel;

	// Token: 0x040075B9 RID: 30137
	[SerializeField]
	private RectTransform gameSettingsPanel;

	// Token: 0x040075BA RID: 30138
	[Header("References")]
	[SerializeField]
	private RectTransform destinationDetailsParent_Asteroid;

	// Token: 0x040075BB RID: 30139
	[SerializeField]
	private RectTransform destinationDetailsParent_Story;

	// Token: 0x040075BC RID: 30140
	[SerializeField]
	private LocText storyTraitsDestinationDetailsLabel;

	// Token: 0x040075BD RID: 30141
	[SerializeField]
	private HierarchyReferences locationIcons;

	// Token: 0x040075BE RID: 30142
	[SerializeField]
	private KInputTextField coordinate;

	// Token: 0x040075BF RID: 30143
	[SerializeField]
	private StoryContentPanel storyContentPanel;

	// Token: 0x040075C0 RID: 30144
	[SerializeField]
	private AsteroidDescriptorPanel destinationProperties;

	// Token: 0x040075C1 RID: 30145
	[SerializeField]
	private AsteroidDescriptorPanel selectedLocationProperties;

	// Token: 0x040075C2 RID: 30146
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_CLUSTER = 164;

	// Token: 0x040075C3 RID: 30147
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_BASE = 76;

	// Token: 0x040075C4 RID: 30148
	private const int WORLDS_SCROLL_PANEL_HEIGHT_CLUSTER = 436;

	// Token: 0x040075C5 RID: 30149
	private const int WORLDS_SCROLL_PANEL_HEIGHT_BASE = 524;

	// Token: 0x040075C6 RID: 30150
	[SerializeField]
	private NewGameSettingsPanel newGameSettingsPanel;

	// Token: 0x040075C7 RID: 30151
	[MyCmpReq]
	private DestinationSelectPanel destinationMapPanel;

	// Token: 0x040075C8 RID: 30152
	[SerializeField]
	private MixingContentPanel mixingPanel;

	// Token: 0x040075C9 RID: 30153
	private KRandom random;

	// Token: 0x040075CA RID: 30154
	private bool isEditingCoordinate;
}
