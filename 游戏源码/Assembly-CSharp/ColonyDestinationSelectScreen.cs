using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
	[SerializeField]
	private GameObject destinationMap;

	[SerializeField]
	private GameObject customSettings;

	[Header("Menu")]
	[SerializeField]
	private MultiToggle[] menuTabs;

	private int selectedMenuTabIdx = 1;

	[Header("Buttons")]
	[SerializeField]
	private KButton backButton;

	[SerializeField]
	private KButton customizeButton;

	[SerializeField]
	private KButton launchButton;

	[SerializeField]
	private KButton shuffleButton;

	[SerializeField]
	private KButton storyTraitShuffleButton;

	[Header("Scroll Panels")]
	[SerializeField]
	private RectTransform worldsScrollPanel;

	[SerializeField]
	private RectTransform storyScrollPanel;

	[SerializeField]
	private RectTransform mixingScrollPanel;

	[SerializeField]
	private RectTransform gameSettingsScrollPanel;

	[Header("Panels")]
	[SerializeField]
	private RectTransform destinationDetailsHeader;

	[SerializeField]
	private RectTransform destinationInfoPanel;

	[SerializeField]
	private RectTransform storyInfoPanel;

	[SerializeField]
	private RectTransform mixingSettingsPanel;

	[SerializeField]
	private RectTransform gameSettingsPanel;

	[Header("References")]
	[SerializeField]
	private RectTransform destinationDetailsParent_Asteroid;

	[SerializeField]
	private RectTransform destinationDetailsParent_Story;

	[SerializeField]
	private LocText storyTraitsDestinationDetailsLabel;

	[SerializeField]
	private HierarchyReferences locationIcons;

	[SerializeField]
	private KInputTextField coordinate;

	[SerializeField]
	private StoryContentPanel storyContentPanel;

	[SerializeField]
	private AsteroidDescriptorPanel destinationProperties;

	[SerializeField]
	private AsteroidDescriptorPanel selectedLocationProperties;

	private const int DESTINATION_HEADER_BUTTON_HEIGHT_CLUSTER = 164;

	private const int DESTINATION_HEADER_BUTTON_HEIGHT_BASE = 76;

	private const int WORLDS_SCROLL_PANEL_HEIGHT_CLUSTER = 436;

	private const int WORLDS_SCROLL_PANEL_HEIGHT_BASE = 524;

	[SerializeField]
	private NewGameSettingsPanel newGameSettingsPanel;

	[MyCmpReq]
	private DestinationSelectPanel destinationMapPanel;

	[SerializeField]
	private MixingContentPanel mixingPanel;

	private KRandom random;

	private bool isEditingCoordinate;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		backButton.onClick += BackClicked;
		customizeButton.onClick += CustomizeClicked;
		launchButton.onClick += LaunchClicked;
		shuffleButton.onClick += ShuffleClicked;
		storyTraitShuffleButton.onClick += StoryTraitShuffleClicked;
		storyTraitShuffleButton.gameObject.SetActive(Db.Get().Stories.Count > 5);
		destinationMapPanel.OnAsteroidClicked += OnAsteroidClicked;
		KInputTextField kInputTextField = coordinate;
		kInputTextField.onFocus = (System.Action)Delegate.Combine(kInputTextField.onFocus, new System.Action(CoordinateEditStarted));
		coordinate.onEndEdit.AddListener(CoordinateEditFinished);
		if (locationIcons != null)
		{
			bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
			locationIcons.gameObject.SetActive(cloudSavesAvailable);
		}
		random = new KRandom();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		RefreshCloudSavePref();
		RefreshCloudLocalIcon();
		newGameSettingsPanel.Init();
		newGameSettingsPanel.SetCloseAction(CustomizeClose);
		destinationMapPanel.Init();
		mixingPanel.Init();
		ShuffleClicked();
		RefreshMenuTabs();
		for (int i = 0; i < menuTabs.Length; i++)
		{
			int target = i;
			menuTabs[i].onClick = delegate
			{
				selectedMenuTabIdx = target;
				RefreshMenuTabs();
			};
		}
		ResizeLayout();
		storyContentPanel.Init();
		storyContentPanel.SelectRandomStories(5, 5, useBias: true);
		storyContentPanel.SelectDefault();
		RefreshStoryLabel();
		RefreshRowsAndDescriptions();
		CustomGameSettings.Instance.OnQualitySettingChanged += QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged += QualitySettingChanged;
		CustomGameSettings.Instance.OnMixingSettingChanged += QualitySettingChanged;
	}

	private void ResizeLayout()
	{
		Vector2 sizeDelta = destinationProperties.clusterDetailsButton.rectTransform().sizeDelta;
		destinationProperties.clusterDetailsButton.rectTransform().sizeDelta = new Vector2(sizeDelta.x, DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76);
		Vector2 sizeDelta2 = worldsScrollPanel.rectTransform().sizeDelta;
		Vector2 anchoredPosition = worldsScrollPanel.rectTransform().anchoredPosition;
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			worldsScrollPanel.rectTransform().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 88f);
		}
		float a = (DlcManager.FeatureClusterSpaceEnabled() ? 436 : 524);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		a = Mathf.Min(a, destinationInfoPanel.sizeDelta.y - (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76) - 22f);
		worldsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, a);
		storyScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, a);
		mixingScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, a);
		gameSettingsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, a);
	}

	protected override void OnCleanUp()
	{
		CustomGameSettings.Instance.OnQualitySettingChanged -= QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged -= QualitySettingChanged;
		newGameSettingsPanel.Uninit();
		destinationMapPanel.Uninit();
		mixingPanel.Uninit();
		storyContentPanel.Cleanup();
		base.OnCleanUp();
	}

	private void RefreshCloudLocalIcon()
	{
		if (locationIcons == null || !SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		HierarchyReferences component = locationIcons.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
		KButton component3 = component.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
		KButton component4 = component.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
		ToolTip component5 = component3.GetComponent<ToolTip>();
		ToolTip component6 = component4.GetComponent<ToolTip>();
		component5.toolTip = $"{UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP}\n{UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA}";
		component6.toolTip = $"{UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_LOCAL}\n{UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA}";
		bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		component2.text = (flag ? UI.FRONTEND.LOADSCREEN.CLOUD_SAVE : UI.FRONTEND.LOADSCREEN.LOCAL_SAVE);
		component3.gameObject.SetActive(flag);
		component3.ClearOnClick();
		if (flag)
		{
			component3.onClick += delegate
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Disabled");
				RefreshCloudLocalIcon();
			};
		}
		component4.gameObject.SetActive(!flag);
		component4.ClearOnClick();
		if (!flag)
		{
			component4.onClick += delegate
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Enabled");
				RefreshCloudLocalIcon();
			};
		}
	}

	private void RefreshCloudSavePref()
	{
		if (SaveLoader.GetCloudSavesAvailable())
		{
			string cloudSavesDefaultPref = SaveLoader.GetCloudSavesDefaultPref();
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, cloudSavesDefaultPref);
		}
	}

	private void BackClicked()
	{
		newGameSettingsPanel.Cancel();
		NavigateBackward();
	}

	private void CustomizeClicked()
	{
		newGameSettingsPanel.Refresh();
		customSettings.SetActive(value: true);
	}

	private void CustomizeClose()
	{
		customSettings.SetActive(value: false);
	}

	private void LaunchClicked()
	{
		CustomGameSettings.Instance.RemoveInvalidMixingSettings();
		NavigateForward();
	}

	private void RefreshMenuTabs()
	{
		for (int i = 0; i < menuTabs.Length; i++)
		{
			menuTabs[i].ChangeState((i == selectedMenuTabIdx) ? 1 : 0);
			LocText componentInChildren = menuTabs[i].GetComponentInChildren<LocText>();
			HierarchyReferences component = menuTabs[i].GetComponent<HierarchyReferences>();
			if (componentInChildren != null)
			{
				componentInChildren.color = ((i == selectedMenuTabIdx) ? Color.white : Color.grey);
			}
			if (component != null)
			{
				Image reference = component.GetReference<Image>("Icon");
				if (reference != null)
				{
					reference.color = ((i == selectedMenuTabIdx) ? Color.white : Color.grey);
				}
			}
		}
		destinationInfoPanel.gameObject.SetActive(selectedMenuTabIdx == 1);
		storyInfoPanel.gameObject.SetActive(selectedMenuTabIdx == 2);
		mixingSettingsPanel.gameObject.SetActive(selectedMenuTabIdx == 3);
		gameSettingsPanel.gameObject.SetActive(selectedMenuTabIdx == 4);
		switch (selectedMenuTabIdx)
		{
		case 1:
			destinationDetailsHeader.SetParent(destinationDetailsParent_Asteroid);
			break;
		case 2:
			destinationDetailsHeader.SetParent(destinationDetailsParent_Story);
			break;
		}
		destinationDetailsHeader.SetAsFirstSibling();
	}

	private void ShuffleClicked()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		int num = random.Next();
		if (currentClusterLayout != null && currentClusterLayout.fixedCoordinate != -1)
		{
			num = currentClusterLayout.fixedCoordinate;
		}
		newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, num.ToString());
	}

	private void StoryTraitShuffleClicked()
	{
		storyContentPanel.SelectRandomStories();
	}

	private void CoordinateChanged(string text)
	{
		string[] array = CustomGameSettings.ParseSettingCoordinate(text);
		if (array.Length < 4 || array.Length > 6 || !int.TryParse(array[2], out var _))
		{
			return;
		}
		ClusterLayout clusterLayout = null;
		foreach (string clusterName in SettingsCache.GetClusterNames())
		{
			ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterName);
			if (clusterData.coordinatePrefix == array[1])
			{
				clusterLayout = clusterData;
			}
		}
		if (clusterLayout != null)
		{
			newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, clusterLayout.filePath);
		}
		newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, array[2]);
		newGameSettingsPanel.ConsumeSettingsCode(array[3]);
		string code = ((array.Length >= 5) ? array[4] : "0");
		newGameSettingsPanel.ConsumeStoryTraitsCode(code);
		string code2 = ((array.Length >= 6) ? array[5] : "0");
		newGameSettingsPanel.ConsumeMixingSettingsCode(code2);
	}

	private void CoordinateEditStarted()
	{
		isEditingCoordinate = true;
	}

	private void CoordinateEditFinished(string text)
	{
		CoordinateChanged(text);
		isEditingCoordinate = false;
		coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	private void QualitySettingChanged(SettingConfig config, SettingLevel level)
	{
		if (config == CustomGameSettingConfigs.SaveToCloud)
		{
			RefreshCloudLocalIcon();
		}
		if (!destinationDetailsHeader.IsNullOrDestroyed())
		{
			if (!isEditingCoordinate && !coordinate.IsNullOrDestroyed())
			{
				coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
			}
			RefreshRowsAndDescriptions();
		}
	}

	public void RefreshRowsAndDescriptions()
	{
		string setting = newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);
		int.TryParse(newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.WorldgenSeed), out var result);
		int fixedCoordinate = CustomGameSettings.Instance.GetCurrentClusterLayout().fixedCoordinate;
		if (fixedCoordinate != -1)
		{
			newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, fixedCoordinate.ToString(), notify: false);
			result = fixedCoordinate;
			shuffleButton.isInteractable = false;
			shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP_DISABLED);
		}
		else
		{
			coordinate.interactable = true;
			shuffleButton.isInteractable = true;
			shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP);
		}
		ColonyDestinationAsteroidBeltData cluster;
		try
		{
			cluster = destinationMapPanel.SelectCluster(setting, result);
		}
		catch
		{
			string defaultAsteroid = destinationMapPanel.GetDefaultAsteroid();
			newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, defaultAsteroid);
			cluster = destinationMapPanel.SelectCluster(defaultAsteroid, result);
		}
		if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
		{
			destinationProperties.EnableClusterLocationLabels(enable: true);
			destinationProperties.RefreshAsteroidLines(cluster, selectedLocationProperties, storyContentPanel.GetActiveStories());
			destinationProperties.EnableClusterDetails(setActive: true);
			destinationProperties.SetClusterDetailLabels(cluster);
			selectedLocationProperties.headerLabel.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.SELECTED_CLUSTER_TRAITS_HEADER);
			destinationProperties.clusterDetailsButton.onClick = delegate
			{
				destinationProperties.SelectWholeClusterDetails(cluster, selectedLocationProperties, storyContentPanel.GetActiveStories());
			};
		}
		else
		{
			destinationProperties.EnableClusterDetails(setActive: false);
			destinationProperties.EnableClusterLocationLabels(enable: false);
			destinationProperties.SetParameterDescriptors(cluster.GetParamDescriptors());
			selectedLocationProperties.SetTraitDescriptors(cluster.GetTraitDescriptors(), storyContentPanel.GetActiveStories());
		}
		RefreshStoryLabel();
	}

	public void RefreshStoryLabel()
	{
		storyTraitsDestinationDetailsLabel.SetText(storyContentPanel.GetTraitsString());
		storyTraitsDestinationDetailsLabel.GetComponent<ToolTip>().SetSimpleTooltip(storyContentPanel.GetTraitsString(tooltip: true));
	}

	private void OnAsteroidClicked(ColonyDestinationAsteroidBeltData cluster)
	{
		newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, cluster.beltPath);
		ShuffleClicked();
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (!isEditingCoordinate)
		{
			if (!e.Consumed && e.TryConsume(Action.PanLeft))
			{
				destinationMapPanel.ScrollLeft();
			}
			else if (!e.Consumed && e.TryConsume(Action.PanRight))
			{
				destinationMapPanel.ScrollRight();
			}
			else if (customSettings.activeSelf && !e.Consumed && (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight)))
			{
				CustomizeClose();
			}
			base.OnKeyDown(e);
		}
	}
}
