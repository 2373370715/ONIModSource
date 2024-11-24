using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class DetailsScreen : KTabMenu
{
	[Serializable]
	private struct Screens
	{
		public string name;

		public string displayName;

		public string tooltip;

		public Sprite icon;

		public TargetPanel screen;

		public int displayOrderPriority;

		public bool hideWhenDead;

		public HashedString focusInViewMode;

		[HideInInspector]
		public int tabIdx;
	}

	[Serializable]
	public class SideScreenRef
	{
		public string name;

		public SideScreenContent screenPrefab;

		public Vector2 offset;

		[HideInInspector]
		public SideScreenContent screenInstance;
	}

	public static DetailsScreen Instance;

	[SerializeField]
	private KButton CodexEntryButton;

	[SerializeField]
	private KButton PinResourceButton;

	[Header("Panels")]
	public Transform UserMenuPanel;

	[Header("Name Editing (disabled)")]
	[SerializeField]
	private KButton CloseButton;

	[Header("Tabs")]
	[SerializeField]
	private DetailTabHeader tabHeader;

	[SerializeField]
	private EditableTitleBar TabTitle;

	[SerializeField]
	private Screens[] screens;

	[SerializeField]
	private GameObject tabHeaderContainer;

	[Header("Side Screen Tabs")]
	[SerializeField]
	private GameObject sidescreenTabHeader;

	[SerializeField]
	private MultiToggle sidescreenConfigTab;

	[SerializeField]
	private MultiToggle sidescreenMaterialTab;

	[SerializeField]
	private MultiToggle sidescreenSkinTab;

	private const string sidescreenConfigID = "sidescreen_config";

	private const string sidescreenMaterialID = "sidescreen_material";

	private const string sidescreenSkinID = "sidescreen_skin";

	[Header("Side Screens")]
	[SerializeField]
	private GameObject sideScreenConfigContentBody;

	[SerializeField]
	private GameObject sideScreenMaterialContentBody;

	[SerializeField]
	private GameObject sideScreenSkinContentBody;

	[SerializeField]
	private GameObject sideScreen;

	[SerializeField]
	private GameObject sideScreenTitle;

	[SerializeField]
	private LocText sideScreenTitleLabel;

	[SerializeField]
	private List<SideScreenRef> sideScreens;

	[SerializeField]
	private GameObject noConfigSideScreen;

	[SerializeField]
	private LayoutElement tabBodyLayoutElement;

	[Header("Secondary Side Screens")]
	[SerializeField]
	private GameObject sideScreen2ContentBody;

	[SerializeField]
	private GameObject sideScreen2;

	[SerializeField]
	private LocText sideScreen2Title;

	private KScreen activeSideScreen2;

	private Tag previousTargetID = null;

	private bool HasActivated;

	private string selectedSidescreenTabID = "sidescreen_config";

	private SideScreenContent currentSideScreen;

	private Dictionary<KScreen, KScreen> instantiatedSecondarySideScreens = new Dictionary<KScreen, KScreen>();

	private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>(delegate(DetailsScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	private List<KeyValuePair<GameObject, int>> sortedSideScreens = new List<KeyValuePair<GameObject, int>>();

	private int setRocketTitleHandle = -1;

	public GameObject target { get; private set; }

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SortScreenOrder();
		base.ConsumeMouseScroll = true;
		Debug.Assert(Instance == null);
		Instance = this;
		DeactivateSideContent();
		Show(show: false);
		Subscribe(Game.Instance.gameObject, -1503271301, OnSelectObject);
		tabHeader.Init();
	}

	private void OnSelectObject(object data)
	{
		if (data == null)
		{
			previouslyActiveTab = -1;
			SelectSideScreenTab("sidescreen_config", considerPreviousMinLayoutHeight: false);
			return;
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		if (component == null || previousTargetID != component.PrefabID())
		{
			SelectSideScreenTab("sidescreen_config", considerPreviousMinLayoutHeight: false);
		}
		else
		{
			SelectSideScreenTab(selectedSidescreenTabID);
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		CodexEntryButton.onClick += CodexEntryButton_OnClick;
		PinResourceButton.onClick += PinResourceButton_OnClick;
		CloseButton.onClick += DeselectAndClose;
		TabTitle.OnNameChanged += OnNameChanged;
		TabTitle.OnStartedEditing += OnStartedEditing;
		MultiToggle multiToggle = sidescreenConfigTab;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			SelectSideScreenTab("sidescreen_config");
		});
		MultiToggle multiToggle2 = sidescreenMaterialTab;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, (System.Action)delegate
		{
			SelectSideScreenTab("sidescreen_material");
		});
		MultiToggle multiToggle3 = sidescreenSkinTab;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, (System.Action)delegate
		{
			SelectSideScreenTab("sidescreen_skin");
		});
		sideScreen2.SetActive(value: false);
		Subscribe(-1514841199, OnRefreshDataDelegate);
	}

	private void OnStartedEditing()
	{
		base.isEditing = true;
		KScreenManager.Instance.RefreshStack();
	}

	private void OnNameChanged(string newName)
	{
		base.isEditing = false;
		if (!string.IsNullOrEmpty(newName))
		{
			MinionIdentity component = target.GetComponent<MinionIdentity>();
			UserNameable component2 = target.GetComponent<UserNameable>();
			ClustercraftExteriorDoor component3 = target.GetComponent<ClustercraftExteriorDoor>();
			CommandModule component4 = target.GetComponent<CommandModule>();
			if (component != null)
			{
				component.SetName(newName);
			}
			else if (component4 != null)
			{
				SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component4.GetComponent<LaunchConditionManager>()).SetRocketName(newName);
			}
			else if (component3 != null)
			{
				component3.GetTargetWorld().GetComponent<UserNameable>().SetName(newName);
			}
			else if (component2 != null)
			{
				component2.SetName(newName);
			}
			TabTitle.UpdateRenameTooltip(target);
		}
	}

	protected override void OnDeactivate()
	{
		if (target != null && setRocketTitleHandle != -1)
		{
			target.Unsubscribe(setRocketTitleHandle);
		}
		setRocketTitleHandle = -1;
		DeactivateSideContent();
		base.OnDeactivate();
	}

	protected override void OnShow(bool show)
	{
		if (!show)
		{
			DeactivateSideContent();
		}
		else
		{
			MaskSideContent(hide: false);
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
		}
		base.OnShow(show);
	}

	protected override void OnCmpDisable()
	{
		DeactivateSideContent();
		base.OnCmpDisable();
	}

	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.isEditing && target != null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
		{
			DeselectAndClose();
		}
	}

	private static Component GetComponent(GameObject go, string name)
	{
		Component component = null;
		Type type = Type.GetType(name);
		if (type != null)
		{
			return go.GetComponent(type);
		}
		return go.GetComponent(name);
	}

	private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
	{
		if (excluded_tags == null || excluded_tags.Length == 0)
		{
			return false;
		}
		bool result = false;
		KPrefabID component = go.GetComponent<KPrefabID>();
		foreach (Tag tag in excluded_tags)
		{
			if (component.PrefabTag == tag)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private string CodexEntryButton_GetCodexId()
	{
		string text = "";
		Debug.Assert(target != null, "Details Screen has no target");
		KSelectable component = target.GetComponent<KSelectable>();
		DebugUtil.AssertArgs(component != null, "Details Screen target is not a KSelectable", target);
		CellSelectionObject component2 = component.GetComponent<CellSelectionObject>();
		BuildingUnderConstruction component3 = component.GetComponent<BuildingUnderConstruction>();
		CreatureBrain component4 = component.GetComponent<CreatureBrain>();
		PlantableSeed component5 = component.GetComponent<PlantableSeed>();
		BudUprootedMonitor component6 = component.GetComponent<BudUprootedMonitor>();
		if (component2 != null)
		{
			text = CodexCache.FormatLinkID(component2.element.id.ToString());
		}
		else if (component3 != null)
		{
			text = CodexCache.FormatLinkID(component3.Def.PrefabID);
		}
		else if (component4 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("BABY", "");
		}
		else if (component5 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("SEED", "");
		}
		else if (component6 != null)
		{
			if (component6.parentObject.Get() != null)
			{
				text = CodexCache.FormatLinkID(component6.parentObject.Get().PrefabID().ToString());
			}
			else if (component6.GetComponent<TreeBud>() != null)
			{
				text = CodexCache.FormatLinkID(component6.GetComponent<TreeBud>().buddingTrunk.Get().PrefabID().ToString());
			}
		}
		else
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
		}
		if (CodexCache.entries.ContainsKey(text) || CodexCache.FindSubEntry(text) != null)
		{
			return text;
		}
		return "";
	}

	private void CodexEntryButton_Refresh()
	{
		string text = CodexEntryButton_GetCodexId();
		CodexEntryButton.isInteractable = text != "";
		CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip(CodexEntryButton.isInteractable ? UI.TOOLTIPS.OPEN_CODEX_ENTRY : UI.TOOLTIPS.NO_CODEX_ENTRY);
	}

	public void CodexEntryButton_OnClick()
	{
		string text = CodexEntryButton_GetCodexId();
		if (text != "")
		{
			ManagementMenu.Instance.OpenCodexToEntry(text);
		}
	}

	private bool PinResourceButton_TryGetResourceTagAndProperName(out Tag targetTag, out string targetProperName)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		if (component != null && ShouldUse(component.PrefabTag))
		{
			targetTag = component.PrefabTag;
			targetProperName = component.GetProperName();
			return true;
		}
		CellSelectionObject component2 = target.GetComponent<CellSelectionObject>();
		if (component2 != null && ShouldUse(component2.element.tag))
		{
			targetTag = component2.element.tag;
			targetProperName = component2.GetProperName();
			return true;
		}
		targetTag = null;
		targetProperName = null;
		return false;
		static bool ShouldUse(Tag targetTag)
		{
			foreach (Tag materialCategory in GameTags.MaterialCategories)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(materialCategory).Contains(targetTag))
				{
					return true;
				}
			}
			foreach (Tag calorieCategory in GameTags.CalorieCategories)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(calorieCategory).Contains(targetTag))
				{
					return true;
				}
			}
			foreach (Tag unitCategory in GameTags.UnitCategories)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(unitCategory).Contains(targetTag))
				{
					return true;
				}
			}
			return false;
		}
	}

	private void PinResourceButton_Refresh()
	{
		if (PinResourceButton_TryGetResourceTagAndProperName(out var targetTag, out var targetProperName))
		{
			ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag);
			if (!AllResourcesScreen.Instance.units.TryGetValue(targetTag, out var value))
			{
				value = GameUtil.MeasureUnit.quantity;
			}
			string arg = value switch
			{
				GameUtil.MeasureUnit.mass => GameUtil.GetFormattedMass(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(targetTag, includeRelatedWorlds: false)), 
				GameUtil.MeasureUnit.quantity => GameUtil.GetFormattedUnits(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(targetTag, includeRelatedWorlds: false)), 
				GameUtil.MeasureUnit.kcal => GameUtil.GetFormattedCalories(RationTracker.Get().CountRationsByFoodType(targetTag.Name, ClusterManager.Instance.activeWorld.worldInventory)), 
				_ => "", 
			};
			PinResourceButton.gameObject.SetActive(value: true);
			PinResourceButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.TOOLTIPS.OPEN_RESOURCE_INFO, arg, targetProperName));
		}
		else
		{
			PinResourceButton.gameObject.SetActive(value: false);
		}
	}

	public void PinResourceButton_OnClick()
	{
		if (PinResourceButton_TryGetResourceTagAndProperName(out var _, out var targetProperName))
		{
			AllResourcesScreen.Instance.SetFilter(UI.StripLinkFormatting(targetProperName));
			AllResourcesScreen.Instance.Show();
		}
	}

	public void OnRefreshData(object obj)
	{
		RefreshTitle();
		for (int i = 0; i < tabs.Count; i++)
		{
			if (tabs[i].gameObject.activeInHierarchy)
			{
				tabs[i].Trigger(-1514841199, obj);
			}
		}
	}

	public void Refresh(GameObject go)
	{
		if (screens == null)
		{
			return;
		}
		if (target != go)
		{
			if (setRocketTitleHandle != -1)
			{
				target.Unsubscribe(setRocketTitleHandle);
				setRocketTitleHandle = -1;
			}
			if (target != null && target.GetComponent<KPrefabID>() != null)
			{
				previousTargetID = target.GetComponent<KPrefabID>().PrefabID();
			}
		}
		target = go;
		sortedSideScreens.Clear();
		CellSelectionObject component = target.GetComponent<CellSelectionObject>();
		if ((bool)component)
		{
			component.OnObjectSelected(null);
		}
		UpdateTitle();
		tabHeader.RefreshTabDisplayForTarget(target);
		if (sideScreens != null && sideScreens.Count > 0)
		{
			bool flag = false;
			foreach (SideScreenRef sideScreen in sideScreens)
			{
				if (!sideScreen.screenPrefab.IsValidForTarget(target))
				{
					if (sideScreen.screenInstance != null && sideScreen.screenInstance.gameObject.activeSelf)
					{
						sideScreen.screenInstance.gameObject.SetActive(value: false);
					}
					continue;
				}
				flag = true;
				if (sideScreen.screenInstance == null)
				{
					sideScreen.screenInstance = Util.KInstantiateUI<SideScreenContent>(sideScreen.screenPrefab.gameObject, sideScreenConfigContentBody);
				}
				if (!this.sideScreen.activeSelf)
				{
					this.sideScreen.SetActive(value: true);
				}
				sideScreen.screenInstance.SetTarget(target);
				sideScreen.screenInstance.Show();
				int sideScreenSortOrder = sideScreen.screenInstance.GetSideScreenSortOrder();
				sortedSideScreens.Add(new KeyValuePair<GameObject, int>(sideScreen.screenInstance.gameObject, sideScreenSortOrder));
				if (currentSideScreen == null || !currentSideScreen.gameObject.activeSelf || sideScreenSortOrder > sortedSideScreens.Find((KeyValuePair<GameObject, int> match) => match.Key == currentSideScreen.gameObject).Value)
				{
					currentSideScreen = sideScreen.screenInstance;
				}
				RefreshTitle();
			}
			if (!flag)
			{
				if (target.GetComponent<MinionIdentity>() == null && (target.GetComponent<Reconstructable>() == null || !target.GetComponent<Reconstructable>().AllowReconstruct) && target.GetComponent<BuildingFacade>() == null)
				{
					noConfigSideScreen.SetActive(value: false);
					this.sideScreen.SetActive(value: false);
				}
				else
				{
					noConfigSideScreen.SetActive(value: true);
					sideScreenTitleLabel.SetText(UI.UISIDESCREENS.NOCONFIG.TITLE);
					this.sideScreen.SetActive(value: true);
				}
			}
			else
			{
				noConfigSideScreen.SetActive(value: false);
			}
		}
		sortedSideScreens.Sort((KeyValuePair<GameObject, int> x, KeyValuePair<GameObject, int> y) => (x.Value <= y.Value) ? 1 : (-1));
		for (int i = 0; i < sortedSideScreens.Count; i++)
		{
			sortedSideScreens[i].Key.transform.SetSiblingIndex(i);
		}
	}

	public void RefreshTitle()
	{
		if ((bool)currentSideScreen)
		{
			sideScreenTitleLabel.SetText(currentSideScreen.GetTitle());
		}
	}

	private void SelectSideScreenTab(string tabID, bool considerPreviousMinLayoutHeight = true)
	{
		if (selectedSidescreenTabID == "sidescreen_config" && considerPreviousMinLayoutHeight)
		{
			tabBodyLayoutElement.minHeight = tabBodyLayoutElement.rectTransform().sizeDelta.y;
		}
		else if (tabID == "sidescreen_config")
		{
			tabBodyLayoutElement.minHeight = 0f;
		}
		selectedSidescreenTabID = tabID;
		RefreshSideScreenTabs();
	}

	private void RefreshSideScreenTabs()
	{
		switch (selectedSidescreenTabID)
		{
		case "sidescreen_config":
			sidescreenConfigTab.ChangeState(1);
			sidescreenMaterialTab.ChangeState(0);
			sidescreenSkinTab.ChangeState(0);
			sideScreenConfigContentBody.SetActive(value: true);
			sideScreenMaterialContentBody.SetActive(value: false);
			sideScreenSkinContentBody.SetActive(value: false);
			sideScreenTitle.SetActive(value: true);
			break;
		case "sidescreen_material":
			sidescreenConfigTab.ChangeState(0);
			sidescreenMaterialTab.ChangeState(1);
			sidescreenSkinTab.ChangeState(0);
			sideScreenConfigContentBody.SetActive(value: false);
			sideScreenMaterialContentBody.SetActive(value: true);
			sideScreenSkinContentBody.SetActive(value: false);
			sideScreenTitle.SetActive(value: false);
			sideScreenMaterialContentBody.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(target);
			break;
		case "sidescreen_skin":
		{
			CosmeticsPanel reference = sideScreenSkinContentBody.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
			reference.SetTarget(target);
			sidescreenConfigTab.ChangeState(0);
			sidescreenMaterialTab.ChangeState(0);
			sidescreenSkinTab.ChangeState(1);
			sideScreenConfigContentBody.SetActive(value: false);
			sideScreenMaterialContentBody.SetActive(value: false);
			sideScreenSkinContentBody.SetActive(value: true);
			sideScreenTitle.SetActive(value: false);
			reference.Refresh();
			break;
		}
		}
		int num = 1;
		if (!target.IsNullOrDestroyed())
		{
			if (target.GetComponent<Reconstructable>() == null || !target.GetComponent<Reconstructable>().AllowReconstruct)
			{
				sidescreenMaterialTab.gameObject.SetActive(value: false);
			}
			else
			{
				sidescreenMaterialTab.gameObject.SetActive(value: true);
				num++;
			}
			MinionIdentity component = target.GetComponent<MinionIdentity>();
			if (component != null)
			{
				HierarchyReferences component2 = sidescreenConfigTab.GetComponent<HierarchyReferences>();
				component2.GetReference<LocText>("label").SetText(UI.UISIDESCREENS.MINIONTODOSIDESCREEN.NAME);
				component2.GetReference<Image>("icon").sprite = Assets.GetSprite("icon_display_screen_errands");
				sidescreenConfigTab.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP);
			}
			else
			{
				HierarchyReferences component3 = sidescreenConfigTab.GetComponent<HierarchyReferences>();
				component3.GetReference<LocText>("label").SetText(UI.DETAILTABS.CONFIGURATION.NAME);
				component3.GetReference<Image>("icon").sprite = Assets.GetSprite("icon_display_screen_config");
				sidescreenConfigTab.GetComponent<ToolTip>().SetSimpleTooltip(UI.DETAILTABS.CONFIGURATION.TOOLTIP);
			}
			if (component == null && target.GetComponent<BuildingFacade>() == null)
			{
				sidescreenSkinTab.gameObject.SetActive(value: false);
			}
			else
			{
				sidescreenSkinTab.gameObject.SetActive(value: true);
				num++;
			}
		}
		sidescreenTabHeader.SetActive(num > 1);
	}

	public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
	{
		ClearSecondarySideScreen();
		if (instantiatedSecondarySideScreens.ContainsKey(secondaryPrefab))
		{
			activeSideScreen2 = instantiatedSecondarySideScreens[secondaryPrefab];
			activeSideScreen2.gameObject.SetActive(value: true);
		}
		else
		{
			activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, sideScreen2ContentBody);
			activeSideScreen2.Activate();
			instantiatedSecondarySideScreens.Add(secondaryPrefab, activeSideScreen2);
		}
		sideScreen2Title.text = title;
		sideScreen2.SetActive(value: true);
		return activeSideScreen2;
	}

	public void ClearSecondarySideScreen()
	{
		if (activeSideScreen2 != null)
		{
			activeSideScreen2.gameObject.SetActive(value: false);
			activeSideScreen2 = null;
		}
		sideScreen2.SetActive(value: false);
	}

	public void DeactivateSideContent()
	{
		if (SideDetailsScreen.Instance != null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
		{
			SideDetailsScreen.Instance.Show(show: false);
		}
		if (sideScreens != null && sideScreens.Count > 0)
		{
			sideScreens.ForEach(delegate(SideScreenRef scn)
			{
				if (scn.screenInstance != null)
				{
					scn.screenInstance.ClearTarget();
					scn.screenInstance.Show(show: false);
				}
			});
		}
		sideScreenMaterialContentBody.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(null);
		sideScreenSkinContentBody.GetComponentInChildren<CosmeticsPanel>().SetTarget(null);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
		sideScreen.SetActive(value: false);
	}

	public void MaskSideContent(bool hide)
	{
		if (hide)
		{
			sideScreen.transform.localScale = Vector3.zero;
		}
		else
		{
			sideScreen.transform.localScale = Vector3.one;
		}
	}

	public void DeselectAndClose()
	{
		if (base.gameObject.activeInHierarchy)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back"));
		}
		if (GetActiveTab() != null)
		{
			GetActiveTab().SetTarget(null);
		}
		SelectTool.Instance.Select(null);
		ClusterMapSelectTool.Instance.Select(null);
		if (!(target == null))
		{
			target = null;
			DeactivateSideContent();
			Show(show: false);
		}
	}

	private void SortScreenOrder()
	{
		Array.Sort(screens, (Screens x, Screens y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority));
	}

	public void UpdatePortrait(GameObject target)
	{
		KSelectable component = target.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		TabTitle.portrait.ClearPortrait();
		Building component2 = component.GetComponent<Building>();
		if ((bool)component2)
		{
			Sprite sprite = null;
			sprite = component2.Def.GetUISprite();
			if (sprite != null)
			{
				TabTitle.portrait.SetPortrait(sprite);
				return;
			}
		}
		if ((bool)target.GetComponent<MinionIdentity>())
		{
			TabTitle.SetPortrait(component.gameObject);
			return;
		}
		Edible component3 = target.GetComponent<Edible>();
		if (component3 != null)
		{
			Sprite uISpriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0]);
			TabTitle.portrait.SetPortrait(uISpriteFromMultiObjectAnim);
			return;
		}
		PrimaryElement component4 = target.GetComponent<PrimaryElement>();
		if (component4 != null)
		{
			TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim));
			return;
		}
		CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
		if (component5 != null)
		{
			string animName = (component5.element.IsSolid ? "ui" : component5.element.substance.name);
			Sprite uISpriteFromMultiObjectAnim2 = Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName);
			TabTitle.portrait.SetPortrait(uISpriteFromMultiObjectAnim2);
		}
	}

	public bool CompareTargetWith(GameObject compare)
	{
		return target == compare;
	}

	public void UpdateTitle()
	{
		CodexEntryButton_Refresh();
		PinResourceButton_Refresh();
		TabTitle.SetTitle(target.GetProperName());
		if (TabTitle != null)
		{
			TabTitle.SetTitle(target.GetProperName());
			MinionIdentity minionIdentity = null;
			UserNameable userNameable = null;
			ClustercraftExteriorDoor clustercraftExteriorDoor = null;
			CommandModule commandModule = null;
			if (target != null)
			{
				minionIdentity = target.gameObject.GetComponent<MinionIdentity>();
				userNameable = target.gameObject.GetComponent<UserNameable>();
				clustercraftExteriorDoor = target.gameObject.GetComponent<ClustercraftExteriorDoor>();
				commandModule = target.gameObject.GetComponent<CommandModule>();
			}
			if (minionIdentity != null)
			{
				TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle());
				TabTitle.SetUserEditable(editable: true);
			}
			else if (userNameable != null)
			{
				TabTitle.SetSubText("");
				TabTitle.SetUserEditable(editable: true);
			}
			else if (commandModule != null)
			{
				TrySetRocketTitle(commandModule);
			}
			else if (clustercraftExteriorDoor != null)
			{
				TrySetRocketTitle(clustercraftExteriorDoor);
			}
			else
			{
				TabTitle.SetSubText("");
				TabTitle.SetUserEditable(editable: false);
			}
			TabTitle.UpdateRenameTooltip(target);
		}
	}

	private void TrySetRocketTitle(ClustercraftExteriorDoor clusterCraftDoor)
	{
		if (clusterCraftDoor.HasTargetWorld())
		{
			WorldContainer targetWorld = clusterCraftDoor.GetTargetWorld();
			TabTitle.SetTitle(targetWorld.GetComponent<ClusterGridEntity>().Name);
			TabTitle.SetUserEditable(editable: true);
			TabTitle.SetSubText(target.GetProperName());
			setRocketTitleHandle = -1;
		}
		else if (setRocketTitleHandle == -1)
		{
			setRocketTitleHandle = target.Subscribe(-71801987, delegate
			{
				OnRefreshData(null);
				target.Unsubscribe(setRocketTitleHandle);
				setRocketTitleHandle = -1;
			});
		}
	}

	private void TrySetRocketTitle(CommandModule commandModule)
	{
		if (commandModule != null)
		{
			TabTitle.SetTitle(SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(commandModule.GetComponent<LaunchConditionManager>()).GetRocketName());
			TabTitle.SetUserEditable(editable: true);
		}
		TabTitle.SetSubText(target.GetProperName());
	}

	public TargetPanel GetActiveTab()
	{
		return tabHeader.ActivePanel;
	}
}
