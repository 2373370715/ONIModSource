using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001EA4 RID: 7844
public class PlanScreen : KIconToggleMenu
{
	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x0600A492 RID: 42130 RVA: 0x0010AC9C File Offset: 0x00108E9C
	// (set) Token: 0x0600A493 RID: 42131 RVA: 0x0010ACA3 File Offset: 0x00108EA3
	public static PlanScreen Instance { get; private set; }

	// Token: 0x0600A494 RID: 42132 RVA: 0x0010ACAB File Offset: 0x00108EAB
	public static void DestroyInstance()
	{
		PlanScreen.Instance = null;
	}

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x0600A495 RID: 42133 RVA: 0x0010ACB3 File Offset: 0x00108EB3
	public static Dictionary<HashedString, string> IconNameMap
	{
		get
		{
			return PlanScreen.iconNameMap;
		}
	}

	// Token: 0x0600A496 RID: 42134 RVA: 0x00101214 File Offset: 0x000FF414
	private static HashedString CacheHashedString(string str)
	{
		return HashCache.Get().Add(str);
	}

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x0600A497 RID: 42135 RVA: 0x0010ACBA File Offset: 0x00108EBA
	// (set) Token: 0x0600A498 RID: 42136 RVA: 0x0010ACC2 File Offset: 0x00108EC2
	public ProductInfoScreen ProductInfoScreen { get; private set; }

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x0600A499 RID: 42137 RVA: 0x0010ACCB File Offset: 0x00108ECB
	public KIconToggleMenu.ToggleInfo ActiveCategoryToggleInfo
	{
		get
		{
			return this.activeCategoryInfo;
		}
	}

	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x0600A49A RID: 42138 RVA: 0x0010ACD3 File Offset: 0x00108ED3
	// (set) Token: 0x0600A49B RID: 42139 RVA: 0x0010ACDB File Offset: 0x00108EDB
	public GameObject SelectedBuildingGameObject { get; private set; }

	// Token: 0x0600A49C RID: 42140 RVA: 0x000D4D6D File Offset: 0x000D2F6D
	public override float GetSortKey()
	{
		return 2f;
	}

	// Token: 0x0600A49D RID: 42141 RVA: 0x0010ACE4 File Offset: 0x00108EE4
	public PlanScreen.RequirementsState GetBuildableState(BuildingDef def)
	{
		if (def == null)
		{
			return PlanScreen.RequirementsState.Materials;
		}
		return this._buildableStatesByID[def.PrefabID];
	}

	// Token: 0x0600A49E RID: 42142 RVA: 0x003E73B4 File Offset: 0x003E55B4
	private bool IsDefResearched(BuildingDef def)
	{
		bool result = false;
		if (!this._researchedDefs.TryGetValue(def, out result))
		{
			result = this.UpdateDefResearched(def);
		}
		return result;
	}

	// Token: 0x0600A49F RID: 42143 RVA: 0x003E73DC File Offset: 0x003E55DC
	private bool UpdateDefResearched(BuildingDef def)
	{
		return this._researchedDefs[def] = Db.Get().TechItems.IsTechItemComplete(def.PrefabID);
	}

	// Token: 0x0600A4A0 RID: 42144 RVA: 0x003E7410 File Offset: 0x003E5610
	protected override void OnPrefabInit()
	{
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.OnPrefabInit();
			PlanScreen.Instance = this;
			this.ProductInfoScreen = global::Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.recipeInfoScreenParent, false);
			this.ProductInfoScreen.rectTransform().pivot = new Vector2(0f, 0f);
			this.ProductInfoScreen.rectTransform().SetLocalPosition(new Vector3(326f, 0f, 0f));
			this.ProductInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
			KInputManager.InputChange.AddListener(new UnityAction(this.RefreshToolTip));
			this.planScreenScrollRect = base.transform.parent.GetComponentInParent<KScrollRect>();
			Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
			Game.Instance.Subscribe(1174281782, new Action<object>(this.OnActiveToolChanged));
			Game.Instance.Subscribe(1557339983, new Action<object>(this.ForceUpdateAllCategoryToggles));
		}
		this.buildingGroupsRoot.gameObject.SetActive(false);
	}

	// Token: 0x0600A4A1 RID: 42145 RVA: 0x003E7548 File Offset: 0x003E5748
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.useSubCategoryLayout = (KPlayerPrefs.GetInt("usePlanScreenListView") == 1);
		this.initTime = KTime.Instance.UnscaledGameTime;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			this._buildableStatesByID.Add(buildingDef.PrefabID, PlanScreen.RequirementsState.Materials);
		}
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.onSelect += this.OnClickCategory;
			this.Refresh();
			foreach (KToggle ktoggle in this.toggles)
			{
				ktoggle.group = base.GetComponent<ToggleGroup>();
			}
			this.RefreshBuildableStates(true);
			Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
		}
		this.copyBuildingButton.GetComponent<MultiToggle>().onClick = delegate()
		{
			this.OnClickCopyBuilding();
		};
		this.RefreshCopyBuildingButton(null);
		Game.Instance.Subscribe(-1503271301, new Action<object>(this.RefreshCopyBuildingButton));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.CloseRecipe(false);
		});
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.PointerEnter));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.PointerExit));
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
		this.RefreshScale(null);
		this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
		this.BuildButtonList();
		this.gridViewButton.onClick += this.OnClickGridView;
		this.listViewButton.onClick += this.OnClickListView;
	}

	// Token: 0x0600A4A2 RID: 42146 RVA: 0x003E778C File Offset: 0x003E598C
	private void RefreshScale(object data = null)
	{
		base.GetComponent<GridLayoutGroup>().cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(54f, 50f) : new Vector2(45f, 45f));
		this.toggles.ForEach(delegate(KToggle to)
		{
			to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
		});
		LayoutElement component = this.copyBuildingButton.GetComponent<LayoutElement>();
		component.minWidth = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		component.minHeight = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		base.gameObject.rectTransform().anchoredPosition = new Vector2(0f, (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? -68 : -74));
		this.adjacentPinnedButtons.GetComponent<HorizontalLayoutGroup>().padding.bottom = (ScreenResolutionMonitor.UsingGamepadUIMode() ? 14 : 6);
		Vector2 sizeDelta = this.buildingGroupsRoot.rectTransform().sizeDelta;
		Vector2 vector = ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(320f, sizeDelta.y) : new Vector2(264f, sizeDelta.y);
		this.buildingGroupsRoot.rectTransform().sizeDelta = vector;
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (this.useSubCategoryLayout)
			{
				componentInChildren.constraintCount = 1;
				componentInChildren.cellSize = new Vector2(vector.x - 24f, 36f);
			}
			else
			{
				componentInChildren.constraintCount = 3;
				componentInChildren.cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize : PlanScreen.standarduildingButtonSize);
			}
		}
		this.ProductInfoScreen.rectTransform().anchoredPosition = new Vector2(vector.x + 8f, this.ProductInfoScreen.rectTransform().anchoredPosition.y);
	}

	// Token: 0x0600A4A3 RID: 42147 RVA: 0x0010AD02 File Offset: 0x00108F02
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600A4A4 RID: 42148 RVA: 0x0010AD20 File Offset: 0x00108F20
	protected override void OnCleanUp()
	{
		if (Game.Instance != null)
		{
			Game.Instance.Unsubscribe(this.refreshScaleHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600A4A5 RID: 42149 RVA: 0x003E7994 File Offset: 0x003E5B94
	private void OnClickCopyBuilding()
	{
		if (!this.LastSelectedBuilding.IsNullOrDestroyed() && this.LastSelectedBuilding.gameObject.activeInHierarchy && (!this.lastSelectedBuilding.Def.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.LastSelectedBuilding);
			return;
		}
		if (this.lastSelectedBuildingDef != null && (!this.lastSelectedBuildingDef.DebugOnly || DebugHandler.InstantBuildMode))
		{
			PlanScreen.Instance.CopyBuildingOrder(this.lastSelectedBuildingDef, this.LastSelectedBuildingFacade);
		}
	}

	// Token: 0x0600A4A6 RID: 42150 RVA: 0x0010AD45 File Offset: 0x00108F45
	private void OnClickListView()
	{
		this.useSubCategoryLayout = true;
		this.ForceRefreshAllBuildingToggles();
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 1);
	}

	// Token: 0x0600A4A7 RID: 42151 RVA: 0x0010AD73 File Offset: 0x00108F73
	private void OnClickGridView()
	{
		this.useSubCategoryLayout = false;
		this.ForceRefreshAllBuildingToggles();
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 0);
	}

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x0600A4A8 RID: 42152 RVA: 0x0010ADA1 File Offset: 0x00108FA1
	// (set) Token: 0x0600A4A9 RID: 42153 RVA: 0x003E7A24 File Offset: 0x003E5C24
	private Building LastSelectedBuilding
	{
		get
		{
			return this.lastSelectedBuilding;
		}
		set
		{
			this.lastSelectedBuilding = value;
			if (this.lastSelectedBuilding != null)
			{
				this.lastSelectedBuildingDef = this.lastSelectedBuilding.Def;
				if (this.lastSelectedBuilding.gameObject.activeInHierarchy)
				{
					this.LastSelectedBuildingFacade = this.lastSelectedBuilding.GetComponent<BuildingFacade>().CurrentFacade;
				}
			}
		}
	}

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x0600A4AA RID: 42154 RVA: 0x0010ADA9 File Offset: 0x00108FA9
	// (set) Token: 0x0600A4AB RID: 42155 RVA: 0x0010ADB1 File Offset: 0x00108FB1
	public string LastSelectedBuildingFacade
	{
		get
		{
			return this.lastSelectedBuildingFacade;
		}
		set
		{
			this.lastSelectedBuildingFacade = value;
		}
	}

	// Token: 0x0600A4AC RID: 42156 RVA: 0x003E7A80 File Offset: 0x003E5C80
	public void RefreshCopyBuildingButton(object data = null)
	{
		this.adjacentPinnedButtons.rectTransform().anchoredPosition = new Vector2(Mathf.Min(base.gameObject.rectTransform().sizeDelta.x, base.transform.parent.rectTransform().rect.width), 0f);
		MultiToggle component = this.copyBuildingButton.GetComponent<MultiToggle>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Building component2 = SelectTool.Instance.selected.GetComponent<Building>();
			if (component2 != null && component2.Def.ShouldShowInBuildMenu() && component2.Def.IsAvailable())
			{
				this.LastSelectedBuilding = component2;
			}
		}
		if (this.lastSelectedBuildingDef != null)
		{
			component.gameObject.SetActive(PlanScreen.Instance.gameObject.activeInHierarchy);
			Sprite sprite = this.lastSelectedBuildingDef.GetUISprite("ui", false);
			if (this.LastSelectedBuildingFacade != null && this.LastSelectedBuildingFacade != "DEFAULT_FACADE" && Db.Get().Permits.BuildingFacades.TryGet(this.LastSelectedBuildingFacade) != null)
			{
				sprite = Def.GetFacadeUISprite(this.LastSelectedBuildingFacade);
			}
			component.transform.Find("FG").GetComponent<Image>().sprite = sprite;
			component.transform.Find("FG").GetComponent<Image>().color = Color.white;
			component.ChangeState(1);
			return;
		}
		component.gameObject.SetActive(false);
		component.ChangeState(0);
	}

	// Token: 0x0600A4AD RID: 42157 RVA: 0x003E7C18 File Offset: 0x003E5E18
	public void RefreshToolTip()
	{
		for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
		{
			PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
			if (SaveLoader.Instance.IsDLCActiveForCurrentSave(planInfo.RequiredDlcId))
			{
				global::Action action = (i < 14) ? (global::Action.Plan1 + i) : global::Action.NumActions;
				string str = HashCache.Get().Get(planInfo.category).ToUpper();
				this.toggleInfo[i].tooltip = GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action);
			}
		}
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
	}

	// Token: 0x0600A4AE RID: 42158 RVA: 0x003E7CD4 File Offset: 0x003E5ED4
	public void Refresh()
	{
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (this.tagCategoryMap == null)
		{
			int num = 0;
			this.tagCategoryMap = new Dictionary<Tag, HashedString>();
			this.tagOrderMap = new Dictionary<Tag, int>();
			if (TUNING.BUILDINGS.PLANORDER.Count > 15)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Insufficient keys to cover root plan menu",
					"Max of 14 keys supported but TUNING.BUILDINGS.PLANORDER has " + TUNING.BUILDINGS.PLANORDER.Count.ToString()
				});
			}
			this.toggleEntries.Clear();
			for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
			{
				PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
				if (SaveLoader.Instance.IsDLCActiveForCurrentSave(planInfo.RequiredDlcId))
				{
					global::Action action = (i < 15) ? (global::Action.Plan1 + i) : global::Action.NumActions;
					string icon = PlanScreen.iconNameMap[planInfo.category];
					string str = HashCache.Get().Get(planInfo.category).ToUpper();
					KIconToggleMenu.ToggleInfo toggleInfo = new KIconToggleMenu.ToggleInfo(UI.StripLinkFormatting(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".NAME")), icon, planInfo.category, action, GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action), "");
					list.Add(toggleInfo);
					PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, this.tagCategoryMap, this.tagOrderMap, ref num);
					List<BuildingDef> list2 = new List<BuildingDef>();
					foreach (BuildingDef buildingDef in Assets.BuildingDefs)
					{
						HashedString x;
						if (buildingDef.IsAvailable() && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out x) && !(x != planInfo.category))
						{
							list2.Add(buildingDef);
						}
					}
					this.toggleEntries.Add(new PlanScreen.ToggleEntry(toggleInfo, planInfo.category, list2, planInfo.hideIfNotResearched));
				}
			}
			base.Setup(list);
			this.toggleBouncers.Clear();
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
				to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
				this.toggleBouncers.Add(to, to.GetComponent<Bouncer>());
			});
			for (int j = 0; j < this.toggleEntries.Count; j++)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[j];
				toggleEntry.CollectToggleImages();
				this.toggleEntries[j] = toggleEntry;
			}
			this.ForceUpdateAllCategoryToggles(null);
		}
	}

	// Token: 0x0600A4AF RID: 42159 RVA: 0x0010ADBA File Offset: 0x00108FBA
	private void ForceUpdateAllCategoryToggles(object data = null)
	{
		this.forceUpdateAllCategoryToggles = true;
	}

	// Token: 0x0600A4B0 RID: 42160 RVA: 0x0010ADC3 File Offset: 0x00108FC3
	public void ForceRefreshAllBuildingToggles()
	{
		this.forceRefreshAllBuildings = true;
	}

	// Token: 0x0600A4B1 RID: 42161 RVA: 0x003E7F68 File Offset: 0x003E6168
	public void CopyBuildingOrder(BuildingDef buildingDef, string facadeID)
	{
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				if (buildingDef.PrefabID == keyValuePair.Key)
				{
					this.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
					this.OnSelectBuilding(this.activeCategoryBuildingToggles[buildingDef].gameObject, buildingDef, facadeID);
					this.ProductInfoScreen.ToggleExpandedInfo(true);
					break;
				}
			}
		}
	}

	// Token: 0x0600A4B2 RID: 42162 RVA: 0x003E8048 File Offset: 0x003E6248
	public void CopyBuildingOrder(Building building)
	{
		this.CopyBuildingOrder(building.Def, building.GetComponent<BuildingFacade>().CurrentFacade);
		if (this.ProductInfoScreen.materialSelectionPanel == null)
		{
			DebugUtil.DevLogError(building.Def.name + " def likely needs to be marked def.ShowInBuildMenu = false");
			return;
		}
		this.ProductInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);
		Rotatable component = building.GetComponent<Rotatable>();
		if (component != null)
		{
			BuildTool.Instance.SetToolOrientation(component.GetOrientation());
		}
	}

	// Token: 0x0600A4B3 RID: 42163 RVA: 0x003E80CC File Offset: 0x003E62CC
	private static void PopulateOrderInfo(HashedString category, object data, Dictionary<Tag, HashedString> category_map, Dictionary<Tag, int> order_map, ref int building_index)
	{
		if (data.GetType() == typeof(PlanScreen.PlanInfo))
		{
			PlanScreen.PlanInfo planInfo = (PlanScreen.PlanInfo)data;
			PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, category_map, order_map, ref building_index);
			return;
		}
		foreach (KeyValuePair<string, string> keyValuePair in ((List<KeyValuePair<string, string>>)data))
		{
			Tag key = new Tag(keyValuePair.Key);
			category_map[key] = category;
			order_map[key] = building_index;
			building_index++;
		}
	}

	// Token: 0x0600A4B4 RID: 42164 RVA: 0x0010ADCC File Offset: 0x00108FCC
	protected override void OnCmpEnable()
	{
		this.Refresh();
		this.RefreshCopyBuildingButton(null);
	}

	// Token: 0x0600A4B5 RID: 42165 RVA: 0x0010ADDB File Offset: 0x00108FDB
	protected override void OnCmpDisable()
	{
		this.ClearButtons();
	}

	// Token: 0x0600A4B6 RID: 42166 RVA: 0x003E8174 File Offset: 0x003E6374
	private void ClearButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
		}
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair2 in this.allBuildingToggles)
		{
			keyValuePair2.Value.gameObject.SetActive(false);
		}
		this.activeCategoryBuildingToggles.Clear();
		this.copyBuildingButton.gameObject.SetActive(false);
		this.copyBuildingButton.GetComponent<MultiToggle>().ChangeState(0);
	}

	// Token: 0x0600A4B7 RID: 42167 RVA: 0x003E823C File Offset: 0x003E643C
	public void OnSelectBuilding(GameObject button_go, BuildingDef def, string facadeID = null)
	{
		if (button_go == null)
		{
			global::Debug.Log("Button gameObject is null", base.gameObject);
			return;
		}
		if (button_go == this.SelectedBuildingGameObject)
		{
			this.CloseRecipe(true);
			return;
		}
		this.ignoreToolChangeMessages++;
		PlanBuildingToggle planBuildingToggle = null;
		if (this.currentlySelectedToggle != null)
		{
			planBuildingToggle = this.currentlySelectedToggle.GetComponent<PlanBuildingToggle>();
		}
		this.SelectedBuildingGameObject = button_go;
		this.currentlySelectedToggle = button_go.GetComponent<KToggle>();
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		HashedString category = this.tagCategoryMap[def.Tag];
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
		{
			toggleEntry.pendingResearchAttentions.Remove(def.Tag);
			button_go.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			if (toggleEntry.pendingResearchAttentions.Count == 0)
			{
				toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			}
		}
		this.ProductInfoScreen.ClearProduct(false);
		if (planBuildingToggle != null)
		{
			planBuildingToggle.Refresh();
		}
		ToolMenu.Instance.ClearSelection();
		PrebuildTool.Instance.Activate(def, this.GetTooltipForBuildable(def));
		this.LastSelectedBuilding = def.BuildingComplete.GetComponent<Building>();
		this.RefreshCopyBuildingButton(null);
		this.ProductInfoScreen.Show(true);
		this.ProductInfoScreen.ConfigureScreen(def, facadeID);
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x0600A4B8 RID: 42168 RVA: 0x003E83B0 File Offset: 0x003E65B0
	private void RefreshBuildableStates(bool force_update)
	{
		if (Assets.BuildingDefs == null || Assets.BuildingDefs.Count == 0)
		{
			return;
		}
		if (this.timeSinceNotificationPing < this.specialNotificationEmbellishDelay)
		{
			this.timeSinceNotificationPing += Time.unscaledDeltaTime;
		}
		if (this.timeSinceNotificationPing >= this.notificationPingExpire)
		{
			this.notificationPingCount = 0;
		}
		int num = 10;
		if (force_update)
		{
			num = Assets.BuildingDefs.Count;
			this.buildable_state_update_idx = 0;
		}
		ListPool<HashedString, PlanScreen>.PooledList pooledList = ListPool<HashedString, PlanScreen>.Allocate();
		for (int i = 0; i < num; i++)
		{
			this.buildable_state_update_idx = (this.buildable_state_update_idx + 1) % Assets.BuildingDefs.Count;
			BuildingDef buildingDef = Assets.BuildingDefs[this.buildable_state_update_idx];
			PlanScreen.RequirementsState buildableStateForDef = this.GetBuildableStateForDef(buildingDef);
			HashedString hashedString;
			if (this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && this._buildableStatesByID[buildingDef.PrefabID] != buildableStateForDef)
			{
				this._buildableStatesByID[buildingDef.PrefabID] = buildableStateForDef;
				if (this.ProductInfoScreen.currentDef == buildingDef)
				{
					this.ignoreToolChangeMessages++;
					this.ProductInfoScreen.ClearProduct(false);
					this.ProductInfoScreen.Show(true);
					this.ProductInfoScreen.ConfigureScreen(buildingDef);
					this.ignoreToolChangeMessages--;
				}
				if (buildableStateForDef == PlanScreen.RequirementsState.Complete)
				{
					foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
					{
						if ((HashedString)toggleInfo.userData == hashedString)
						{
							Bouncer bouncer = this.toggleBouncers[toggleInfo.toggle];
							if (bouncer != null && !bouncer.IsBouncing() && !pooledList.Contains(hashedString))
							{
								pooledList.Add(hashedString);
								bouncer.Bounce();
								if (KTime.Instance.UnscaledGameTime - this.initTime > 1.5f)
								{
									if (this.timeSinceNotificationPing >= this.specialNotificationEmbellishDelay)
									{
										string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
										if (sound != null)
										{
											SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
										}
									}
									string sound2 = GlobalAssets.GetSound("NewBuildable", false);
									if (sound2 != null)
									{
										EventInstance instance = SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
										instance.setParameterByName("playCount", (float)this.notificationPingCount, false);
										SoundEvent.EndOneShot(instance);
									}
								}
								this.timeSinceNotificationPing = 0f;
								this.notificationPingCount++;
							}
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A4B9 RID: 42169 RVA: 0x003E8680 File Offset: 0x003E6880
	private PlanScreen.RequirementsState GetBuildableStateForDef(BuildingDef def)
	{
		if (!def.IsAvailable())
		{
			return PlanScreen.RequirementsState.Invalid;
		}
		PlanScreen.RequirementsState result = PlanScreen.RequirementsState.Complete;
		KPrefabID component = def.BuildingComplete.GetComponent<KPrefabID>();
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !this.IsDefResearched(def))
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (component.HasTag(GameTags.Telepad) && ClusterUtil.ActiveWorldHasPrinter())
		{
			result = PlanScreen.RequirementsState.TelepadBuilt;
		}
		else if (component.HasTag(GameTags.RocketInteriorBuilding) && !ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorOnly;
		}
		else if (component.HasTag(GameTags.NotRocketInteriorBuilding) && ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorForbidden;
		}
		else if (component.HasTag(GameTags.UniquePerWorld) && BuildingInventory.Instance.BuildingCountForWorld_BAD_PERF(def.Tag, ClusterManager.Instance.activeWorldId) > 0)
		{
			result = PlanScreen.RequirementsState.UniquePerWorld;
		}
		else if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !ProductInfoScreen.MaterialsMet(def.CraftRecipe))
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		return result;
	}

	// Token: 0x0600A4BA RID: 42170 RVA: 0x003E8764 File Offset: 0x003E6964
	private void SetCategoryButtonState()
	{
		this.nextCategoryToUpdateIDX = (this.nextCategoryToUpdateIDX + 1) % this.toggleEntries.Count;
		for (int i = 0; i < this.toggleEntries.Count; i++)
		{
			if (this.forceUpdateAllCategoryToggles || i == this.nextCategoryToUpdateIDX)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[i];
				KIconToggleMenu.ToggleInfo toggleInfo = toggleEntry.toggleInfo;
				toggleInfo.toggle.ActivateFlourish(this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData);
				bool flag = false;
				bool flag2 = true;
				if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
				{
					flag = true;
					flag2 = false;
				}
				else
				{
					foreach (BuildingDef def in toggleEntry.buildingDefs)
					{
						if (this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete)
						{
							flag = true;
							flag2 = false;
							break;
						}
					}
					if (flag2 && toggleEntry.AreAnyRequiredTechItemsAvailable())
					{
						flag2 = false;
					}
				}
				this.CategoryInteractive[toggleInfo] = !flag2;
				GameObject gameObject = toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject;
				if (!flag)
				{
					if (flag2 && toggleEntry.hideIfNotResearched)
					{
						toggleInfo.toggle.gameObject.SetActive(false);
					}
					else if (flag2)
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(true);
					}
					else
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(false);
					}
					ImageToggleState.State state = (this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state);
					}
				}
				else
				{
					toggleInfo.toggle.gameObject.SetActive(true);
					gameObject.gameObject.SetActive(false);
					ImageToggleState.State state2 = (this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state2);
					}
				}
			}
		}
		this.RefreshCopyBuildingButton(null);
		this.forceUpdateAllCategoryToggles = false;
	}

	// Token: 0x0600A4BB RID: 42171 RVA: 0x003E89D0 File Offset: 0x003E6BD0
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || type == typeof(PrebuildTool))
			{
				activeTool.DeactivateTool(null);
				PlayerController.Instance.ActivateTool(SelectTool.Instance);
			}
		}
	}

	// Token: 0x0600A4BC RID: 42172 RVA: 0x003E8A44 File Offset: 0x003E6C44
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
		this.SelectedBuildingGameObject = null;
	}

	// Token: 0x0600A4BD RID: 42173 RVA: 0x003E8ACC File Offset: 0x003E6CCC
	public void SoftCloseRecipe()
	{
		this.ignoreToolChangeMessages++;
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		this.currentlySelectedToggle = null;
		this.SelectedBuildingGameObject = null;
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x0600A4BE RID: 42174 RVA: 0x003E8B50 File Offset: 0x003E6D50
	public void CloseCategoryPanel(bool playSound = true)
	{
		this.activeCategoryInfo = null;
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
		{
			this.ClearButtons();
			this.buildingGroupsRoot.gameObject.SetActive(false);
			this.ForceUpdateAllCategoryToggles(null);
		});
		this.PlanCategoryLabel.text = "";
		this.ForceUpdateAllCategoryToggles(null);
	}

	// Token: 0x0600A4BF RID: 42175 RVA: 0x003E8BAC File Offset: 0x003E6DAC
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.CloseRecipe(false);
		if (!this.CategoryInteractive.ContainsKey(toggle_info) || !this.CategoryInteractive[toggle_info])
		{
			this.CloseCategoryPanel(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		if (this.activeCategoryInfo == toggle_info)
		{
			this.CloseCategoryPanel(true);
		}
		else
		{
			this.OpenCategoryPanel(toggle_info, true);
		}
		this.ConfigurePanelSize(null);
		this.SetScrollPoint(0f);
	}

	// Token: 0x0600A4C0 RID: 42176 RVA: 0x003E8C20 File Offset: 0x003E6E20
	private void OpenCategoryPanel(KIconToggleMenu.ToggleInfo toggle_info, bool play_sound = true)
	{
		HashedString hashedString = (HashedString)toggle_info.userData;
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen.Instance.ClearSearch();
		}
		this.ClearButtons();
		this.buildingGroupsRoot.gameObject.SetActive(true);
		this.activeCategoryInfo = toggle_info;
		if (play_sound)
		{
			UISounds.PlaySound(UISounds.Sound.ClickObject);
		}
		this.BuildButtonList();
		this.ForceRefreshAllBuildingToggles();
		this.UpdateBuildingButtonList(this.activeCategoryInfo);
		this.RefreshCategoryPanelTitle();
		this.ForceUpdateAllCategoryToggles(null);
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Expand(null);
	}

	// Token: 0x0600A4C1 RID: 42177 RVA: 0x003E8CB0 File Offset: 0x003E6EB0
	public void RefreshCategoryPanelTitle()
	{
		if (this.activeCategoryInfo != null)
		{
			this.PlanCategoryLabel.text = this.activeCategoryInfo.text.ToUpper();
		}
		if (!BuildingGroupScreen.SearchIsEmpty)
		{
			this.PlanCategoryLabel.text = UI.BUILDMENU.SEARCH_RESULTS_HEADER;
		}
	}

	// Token: 0x0600A4C2 RID: 42178 RVA: 0x003E8CFC File Offset: 0x003E6EFC
	public void OpenCategoryByName(string category)
	{
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry))
		{
			this.OpenCategoryPanel(toggleEntry.toggleInfo, false);
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x0600A4C3 RID: 42179 RVA: 0x003E8D30 File Offset: 0x003E6F30
	private void UpdateBuildingButtonList(KIconToggleMenu.ToggleInfo toggle_info)
	{
		KToggle toggle = toggle_info.toggle;
		if (toggle == null)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				if (toggleInfo.userData == toggle_info.userData)
				{
					toggle = toggleInfo.toggle;
					break;
				}
			}
		}
		if (toggle != null && this.allBuildingToggles.Count != 0)
		{
			if (this.forceRefreshAllBuildings)
			{
				for (int i = 0; i < this.allBuildingToggles.Count; i++)
				{
					PlanBuildingToggle value = this.allBuildingToggles.ElementAt(i).Value;
					this.categoryPanelSizeNeedsRefresh = (value.Refresh() || this.categoryPanelSizeNeedsRefresh);
					this.forceRefreshAllBuildings = false;
					value.SwitchViewMode(this.useSubCategoryLayout);
				}
			}
			else
			{
				for (int j = 0; j < this.maxToggleRefreshPerFrame; j++)
				{
					if (this.building_button_refresh_idx >= this.allBuildingToggles.Count)
					{
						this.building_button_refresh_idx = 0;
					}
					PlanBuildingToggle value2 = this.allBuildingToggles.ElementAt(this.building_button_refresh_idx).Value;
					this.categoryPanelSizeNeedsRefresh = (value2.Refresh() || this.categoryPanelSizeNeedsRefresh);
					value2.SwitchViewMode(this.useSubCategoryLayout);
					this.building_button_refresh_idx++;
				}
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (!(componentInChildren == null))
			{
				int num = 0;
				for (int k = 0; k < componentInChildren.transform.childCount; k++)
				{
					if (componentInChildren.transform.GetChild(k).gameObject.activeSelf)
					{
						num++;
					}
				}
				if (keyValuePair.Value.gameObject.activeSelf != num > 0)
				{
					keyValuePair.Value.gameObject.SetActive(num > 0);
				}
			}
		}
		if (this.categoryPanelSizeNeedsRefresh && this.building_button_refresh_idx >= this.activeCategoryBuildingToggles.Count)
		{
			this.categoryPanelSizeNeedsRefresh = false;
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x0600A4C4 RID: 42180 RVA: 0x0010ADE3 File Offset: 0x00108FE3
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.RefreshBuildableStates(false);
		this.SetCategoryButtonState();
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
	}

	// Token: 0x0600A4C5 RID: 42181 RVA: 0x003E8F8C File Offset: 0x003E718C
	private void BuildButtonList()
	{
		this.activeCategoryBuildingToggles.Clear();
		Dictionary<string, HashedString> dictionary = new Dictionary<string, HashedString>();
		Dictionary<string, List<BuildingDef>> dictionary2 = new Dictionary<string, List<BuildingDef>>();
		if (!dictionary2.ContainsKey("default"))
		{
			dictionary2.Add("default", new List<BuildingDef>());
		}
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && buildingDef.IsValidDLC())
				{
					dictionary.Add(buildingDef.PrefabID, planInfo.category);
					if (!dictionary2.ContainsKey(keyValuePair.Value))
					{
						dictionary2.Add(keyValuePair.Value, new List<BuildingDef>());
					}
					dictionary2[keyValuePair.Value].Add(buildingDef);
				}
			}
		}
		if (!this.allSubCategoryObjects.ContainsKey("default"))
		{
			this.allSubCategoryObjects.Add("default", global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true));
		}
		GameObject gameObject = this.allSubCategoryObjects["default"].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
		foreach (KeyValuePair<string, List<BuildingDef>> keyValuePair2 in dictionary2)
		{
			if (!this.allSubCategoryObjects.ContainsKey(keyValuePair2.Key))
			{
				this.allSubCategoryObjects.Add(keyValuePair2.Key, global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true));
			}
			if (keyValuePair2.Key == "default")
			{
				this.allSubCategoryObjects[keyValuePair2.Key].SetActive(this.useSubCategoryLayout);
			}
			HierarchyReferences component = this.allSubCategoryObjects[keyValuePair2.Key].GetComponent<HierarchyReferences>();
			GameObject parent;
			if (this.useSubCategoryLayout)
			{
				component.GetReference<RectTransform>("Header").gameObject.SetActive(true);
				parent = this.allSubCategoryObjects[keyValuePair2.Key].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
				StringEntry entry;
				if (Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + keyValuePair2.Key.ToUpper() + ".BUILDMENUTITLE", out entry))
				{
					component.GetReference<LocText>("HeaderLabel").SetText(entry);
				}
			}
			else
			{
				component.GetReference<RectTransform>("Header").gameObject.SetActive(false);
				parent = gameObject;
			}
			foreach (BuildingDef buildingDef2 in keyValuePair2.Value)
			{
				HashedString hashedString = dictionary[buildingDef2.PrefabID];
				GameObject gameObject2 = this.CreateButton(buildingDef2, parent, hashedString);
				PlanScreen.ToggleEntry toggleEntry = null;
				this.GetToggleEntryForCategory(hashedString, out toggleEntry);
				if (toggleEntry != null && toggleEntry.pendingResearchAttentions.Contains(buildingDef2.PrefabID))
				{
					gameObject2.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
				}
			}
		}
		this.RefreshScale(null);
	}

	// Token: 0x0600A4C6 RID: 42182 RVA: 0x003E9354 File Offset: 0x003E7554
	public void ConfigurePanelSize(object data = null)
	{
		if (this.useSubCategoryLayout)
		{
			this.buildGrid_bg_rowHeight = 48f;
		}
		else
		{
			this.buildGrid_bg_rowHeight = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize.y : PlanScreen.standarduildingButtonSize.y);
		}
		GridLayoutGroup reference = this.subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid");
		this.buildGrid_bg_rowHeight += reference.spacing.y;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.GroupsTransform.childCount; i++)
		{
			int num3 = 0;
			HierarchyReferences component = this.GroupsTransform.GetChild(i).GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				GridLayoutGroup reference2 = component.GetReference<GridLayoutGroup>("Grid");
				if (!(reference2 == null))
				{
					bool flag = false;
					for (int j = 0; j < reference2.transform.childCount; j++)
					{
						if (reference2.transform.GetChild(j).gameObject.activeSelf)
						{
							flag = true;
							num3++;
						}
					}
					if (flag)
					{
						num2 += 24;
					}
					num += num3 / reference2.constraintCount;
					if (num3 % reference2.constraintCount != 0)
					{
						num++;
					}
				}
			}
		}
		num2 = Math.Min(72, num2);
		this.noResultMessage.SetActive(num == 0);
		int num4 = num;
		int num5 = Math.Max(1, Screen.height / (int)this.buildGrid_bg_rowHeight - 3);
		num5 = Math.Min(num5, this.useSubCategoryLayout ? 12 : 6);
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num4 = Mathf.Min(num5, this.useSubCategoryLayout ? 8 : 4);
		}
		this.BuildingGroupContentsRect.GetComponent<ScrollRect>().verticalScrollbar.gameObject.SetActive(num4 >= num5 - 1);
		float num6 = this.buildGrid_bg_borderHeight + (float)num2 + 36f + (float)Mathf.Clamp(num4, 0, num5) * this.buildGrid_bg_rowHeight;
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num6 = Mathf.Max(num6, this.buildingGroupsRoot.sizeDelta.y);
		}
		this.buildingGroupsRoot.sizeDelta = new Vector2(this.buildGrid_bg_width, num6);
		this.RefreshScale(null);
	}

	// Token: 0x0600A4C7 RID: 42183 RVA: 0x0010AE0D File Offset: 0x0010900D
	private void SetScrollPoint(float targetY)
	{
		this.BuildingGroupContentsRect.anchoredPosition = new Vector2(this.BuildingGroupContentsRect.anchoredPosition.x, targetY);
	}

	// Token: 0x0600A4C8 RID: 42184 RVA: 0x003E9584 File Offset: 0x003E7784
	private GameObject CreateButton(BuildingDef def, GameObject parent, HashedString plan_category)
	{
		GameObject gameObject;
		PlanBuildingToggle planBuildingToggle;
		if (this.allBuildingToggles.ContainsKey(def.PrefabID))
		{
			gameObject = this.allBuildingToggles[def.PrefabID].gameObject;
			planBuildingToggle = this.allBuildingToggles[def.PrefabID];
			planBuildingToggle.Refresh();
		}
		else
		{
			gameObject = global::Util.KInstantiateUI(this.planButtonPrefab, parent, false);
			gameObject.name = UI.StripLinkFormatting(def.name) + " Group:" + plan_category.ToString();
			planBuildingToggle = gameObject.GetComponentInChildren<PlanBuildingToggle>();
			planBuildingToggle.Config(def, this, plan_category);
			planBuildingToggle.soundPlayer.Enabled = false;
			planBuildingToggle.SwitchViewMode(this.useSubCategoryLayout);
			this.allBuildingToggles.Add(def.PrefabID, planBuildingToggle);
		}
		if (gameObject.transform.parent != parent)
		{
			gameObject.transform.SetParent(parent.transform);
		}
		this.activeCategoryBuildingToggles.Add(def, planBuildingToggle);
		return gameObject;
	}

	// Token: 0x0600A4C9 RID: 42185 RVA: 0x0010AE30 File Offset: 0x00109030
	public static bool TechRequirementsMet(TechItem techItem)
	{
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x0600A4CA RID: 42186 RVA: 0x0010AE50 File Offset: 0x00109050
	private static bool TechRequirementsUpcoming(TechItem techItem)
	{
		return PlanScreen.TechRequirementsMet(techItem);
	}

	// Token: 0x0600A4CB RID: 42187 RVA: 0x003E967C File Offset: 0x003E787C
	private bool GetToggleEntryForCategory(HashedString category, out PlanScreen.ToggleEntry toggleEntry)
	{
		toggleEntry = null;
		foreach (PlanScreen.ToggleEntry toggleEntry2 in this.toggleEntries)
		{
			if (toggleEntry2.planCategory == category)
			{
				toggleEntry = toggleEntry2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600A4CC RID: 42188 RVA: 0x0010AE58 File Offset: 0x00109058
	public bool IsDefBuildable(BuildingDef def)
	{
		return this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete;
	}

	// Token: 0x0600A4CD RID: 42189 RVA: 0x003E96E4 File Offset: 0x003E78E4
	public string GetTooltipForBuildable(BuildingDef def)
	{
		PlanScreen.RequirementsState buildableState = this.GetBuildableState(def);
		return PlanScreen.GetTooltipForRequirementsState(def, buildableState);
	}

	// Token: 0x0600A4CE RID: 42190 RVA: 0x003E9700 File Offset: 0x003E7900
	public static string GetTooltipForRequirementsState(BuildingDef def, PlanScreen.RequirementsState state)
	{
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		string text = null;
		if (Game.Instance.SandboxModeActive)
		{
			text = UIConstants.ColorPrefixYellow + UI.SANDBOXTOOLS.SETTINGS.INSTANT_BUILD.NAME + UIConstants.ColorSuffix;
		}
		else if (DebugHandler.InstantBuildMode)
		{
			text = UIConstants.ColorPrefixYellow + UI.DEBUG_TOOLS.DEBUG_ACTIVE + UIConstants.ColorSuffix;
		}
		else
		{
			switch (state)
			{
			case PlanScreen.RequirementsState.Tech:
				text = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
				break;
			case PlanScreen.RequirementsState.Materials:
				text = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
				foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
				{
					string str = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					text = text + "\n" + str;
				}
				break;
			case PlanScreen.RequirementsState.TelepadBuilt:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.UniquePerWorld:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.RocketInteriorOnly:
				text = UI.PRODUCTINFO_ROCKET_INTERIOR;
				break;
			case PlanScreen.RequirementsState.RocketInteriorForbidden:
				text = UI.PRODUCTINFO_ROCKET_NOT_INTERIOR;
				break;
			}
		}
		return text;
	}

	// Token: 0x0600A4CF RID: 42191 RVA: 0x0010AE64 File Offset: 0x00109064
	private void PointerEnter(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = true;
	}

	// Token: 0x0600A4D0 RID: 42192 RVA: 0x0010AE72 File Offset: 0x00109072
	private void PointerExit(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = false;
	}

	// Token: 0x0600A4D1 RID: 42193 RVA: 0x003E988C File Offset: 0x003E7A8C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyDown(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.IsAction(global::Action.CopyBuilding) && e.TryConsume(global::Action.CopyBuilding))
		{
			this.OnClickCopyBuilding();
		}
		if (this.toggles == null)
		{
			return;
		}
		if (!e.Consumed && this.activeCategoryInfo != null && e.TryConsume(global::Action.Escape))
		{
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			this.ClearSelection();
			return;
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600A4D2 RID: 42194 RVA: 0x003E9954 File Offset: 0x003E7B54
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyUp(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.Consumed)
		{
			return;
		}
		if (this.SelectedBuildingGameObject != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.CloseRecipe(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		else if (this.activeCategoryInfo != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.OnUIClear(null);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x0600A4D3 RID: 42195 RVA: 0x003E9A14 File Offset: 0x003E7C14
	private void OnRecipeElementsFullySelected()
	{
		BuildingDef buildingDef = null;
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair in this.allBuildingToggles)
		{
			if (keyValuePair.Value == this.currentlySelectedToggle)
			{
				buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				break;
			}
		}
		DebugUtil.DevAssert(buildingDef, "def is null", null);
		if (buildingDef)
		{
			if (buildingDef.isKAnimTile && buildingDef.isUtility)
			{
				IList<Tag> getSelectedElementAsList = this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
				((buildingDef.BuildingComplete.GetComponent<Wire>() != null) ? WireBuildTool.Instance : UtilityBuildTool.Instance).Activate(buildingDef, getSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
				return;
			}
			BuildTool.Instance.Activate(buildingDef, this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
		}
	}

	// Token: 0x0600A4D4 RID: 42196 RVA: 0x003E9B24 File Offset: 0x003E7D24
	public void OnResearchComplete(object tech)
	{
		if (tech is Tech)
		{
			using (List<TechItem>.Enumerator enumerator = ((Tech)tech).unlockedItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TechItem techItem = enumerator.Current;
					BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
					this.AddResearchedBuildingCategory(buildingDef);
				}
				return;
			}
		}
		if (tech is BuildingDef)
		{
			BuildingDef def = tech as BuildingDef;
			this.AddResearchedBuildingCategory(def);
		}
	}

	// Token: 0x0600A4D5 RID: 42197 RVA: 0x003E9BA4 File Offset: 0x003E7DA4
	private void AddResearchedBuildingCategory(BuildingDef def)
	{
		if (def != null)
		{
			this.UpdateDefResearched(def);
			if (this.tagCategoryMap.ContainsKey(def.Tag))
			{
				HashedString category = this.tagCategoryMap[def.Tag];
				PlanScreen.ToggleEntry toggleEntry;
				if (this.GetToggleEntryForCategory(category, out toggleEntry))
				{
					toggleEntry.pendingResearchAttentions.Add(def.Tag);
					toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
					toggleEntry.Refresh();
				}
			}
		}
	}

	// Token: 0x0600A4D6 RID: 42198 RVA: 0x003E9C20 File Offset: 0x003E7E20
	private void OnUIClear(object data)
	{
		if (this.activeCategoryInfo != null)
		{
			this.selected = -1;
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x0600A4D7 RID: 42199 RVA: 0x003E9C70 File Offset: 0x003E7E70
	private void OnActiveToolChanged(object data)
	{
		if (data == null)
		{
			return;
		}
		if (this.ignoreToolChangeMessages > 0)
		{
			return;
		}
		Type type = data.GetType();
		if (!typeof(BuildTool).IsAssignableFrom(type) && !typeof(PrebuildTool).IsAssignableFrom(type) && !typeof(BaseUtilityBuildTool).IsAssignableFrom(type))
		{
			this.CloseRecipe(false);
			this.CloseCategoryPanel(false);
		}
	}

	// Token: 0x0600A4D8 RID: 42200 RVA: 0x0010AE80 File Offset: 0x00109080
	public PrioritySetting GetBuildingPriority()
	{
		return this.ProductInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
	}

	// Token: 0x0400809E RID: 32926
	[SerializeField]
	private GameObject planButtonPrefab;

	// Token: 0x0400809F RID: 32927
	[SerializeField]
	private GameObject recipeInfoScreenParent;

	// Token: 0x040080A0 RID: 32928
	[SerializeField]
	private GameObject productInfoScreenPrefab;

	// Token: 0x040080A1 RID: 32929
	[SerializeField]
	private GameObject copyBuildingButton;

	// Token: 0x040080A2 RID: 32930
	[SerializeField]
	private KButton gridViewButton;

	// Token: 0x040080A3 RID: 32931
	[SerializeField]
	private KButton listViewButton;

	// Token: 0x040080A4 RID: 32932
	private bool useSubCategoryLayout;

	// Token: 0x040080A5 RID: 32933
	private int refreshScaleHandle = -1;

	// Token: 0x040080A6 RID: 32934
	[SerializeField]
	private GameObject adjacentPinnedButtons;

	// Token: 0x040080A7 RID: 32935
	private static Dictionary<HashedString, string> iconNameMap = new Dictionary<HashedString, string>
	{
		{
			PlanScreen.CacheHashedString("Base"),
			"icon_category_base"
		},
		{
			PlanScreen.CacheHashedString("Oxygen"),
			"icon_category_oxygen"
		},
		{
			PlanScreen.CacheHashedString("Power"),
			"icon_category_electrical"
		},
		{
			PlanScreen.CacheHashedString("Food"),
			"icon_category_food"
		},
		{
			PlanScreen.CacheHashedString("Plumbing"),
			"icon_category_plumbing"
		},
		{
			PlanScreen.CacheHashedString("HVAC"),
			"icon_category_ventilation"
		},
		{
			PlanScreen.CacheHashedString("Refining"),
			"icon_category_refinery"
		},
		{
			PlanScreen.CacheHashedString("Medical"),
			"icon_category_medical"
		},
		{
			PlanScreen.CacheHashedString("Furniture"),
			"icon_category_furniture"
		},
		{
			PlanScreen.CacheHashedString("Equipment"),
			"icon_category_misc"
		},
		{
			PlanScreen.CacheHashedString("Utilities"),
			"icon_category_utilities"
		},
		{
			PlanScreen.CacheHashedString("Automation"),
			"icon_category_automation"
		},
		{
			PlanScreen.CacheHashedString("Conveyance"),
			"icon_category_shipping"
		},
		{
			PlanScreen.CacheHashedString("Rocketry"),
			"icon_category_rocketry"
		},
		{
			PlanScreen.CacheHashedString("HEP"),
			"icon_category_radiation"
		}
	};

	// Token: 0x040080A8 RID: 32936
	private Dictionary<KIconToggleMenu.ToggleInfo, bool> CategoryInteractive = new Dictionary<KIconToggleMenu.ToggleInfo, bool>();

	// Token: 0x040080AA RID: 32938
	[SerializeField]
	public PlanScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x040080AB RID: 32939
	public PlanScreen.BuildingNameTextSetting buildingNameTextSettings;

	// Token: 0x040080AC RID: 32940
	private KIconToggleMenu.ToggleInfo activeCategoryInfo;

	// Token: 0x040080AD RID: 32941
	public Dictionary<BuildingDef, PlanBuildingToggle> activeCategoryBuildingToggles = new Dictionary<BuildingDef, PlanBuildingToggle>();

	// Token: 0x040080AE RID: 32942
	private float timeSinceNotificationPing;

	// Token: 0x040080AF RID: 32943
	private float notificationPingExpire = 0.5f;

	// Token: 0x040080B0 RID: 32944
	private float specialNotificationEmbellishDelay = 8f;

	// Token: 0x040080B1 RID: 32945
	private int notificationPingCount;

	// Token: 0x040080B2 RID: 32946
	private Dictionary<KToggle, Bouncer> toggleBouncers = new Dictionary<KToggle, Bouncer>();

	// Token: 0x040080B3 RID: 32947
	public const string DEFAULT_SUBCATEGORY_KEY = "default";

	// Token: 0x040080B4 RID: 32948
	private Dictionary<string, GameObject> allSubCategoryObjects = new Dictionary<string, GameObject>();

	// Token: 0x040080B5 RID: 32949
	private Dictionary<string, PlanBuildingToggle> allBuildingToggles = new Dictionary<string, PlanBuildingToggle>();

	// Token: 0x040080B6 RID: 32950
	private static Vector2 bigBuildingButtonSize = new Vector2(98f, 123f);

	// Token: 0x040080B7 RID: 32951
	private static Vector2 standarduildingButtonSize = PlanScreen.bigBuildingButtonSize * 0.8f;

	// Token: 0x040080B8 RID: 32952
	public static int fontSizeBigMode = 16;

	// Token: 0x040080B9 RID: 32953
	public static int fontSizeStandardMode = 14;

	// Token: 0x040080BB RID: 32955
	[SerializeField]
	private GameObject subgroupPrefab;

	// Token: 0x040080BC RID: 32956
	public Transform GroupsTransform;

	// Token: 0x040080BD RID: 32957
	public Sprite Overlay_NeedTech;

	// Token: 0x040080BE RID: 32958
	public RectTransform buildingGroupsRoot;

	// Token: 0x040080BF RID: 32959
	public RectTransform BuildButtonBGPanel;

	// Token: 0x040080C0 RID: 32960
	public RectTransform BuildingGroupContentsRect;

	// Token: 0x040080C1 RID: 32961
	public Sprite defaultBuildingIconSprite;

	// Token: 0x040080C2 RID: 32962
	private KScrollRect planScreenScrollRect;

	// Token: 0x040080C3 RID: 32963
	public Material defaultUIMaterial;

	// Token: 0x040080C4 RID: 32964
	public Material desaturatedUIMaterial;

	// Token: 0x040080C5 RID: 32965
	public LocText PlanCategoryLabel;

	// Token: 0x040080C6 RID: 32966
	public GameObject noResultMessage;

	// Token: 0x040080C7 RID: 32967
	private int nextCategoryToUpdateIDX = -1;

	// Token: 0x040080C8 RID: 32968
	private bool forceUpdateAllCategoryToggles;

	// Token: 0x040080C9 RID: 32969
	private bool forceRefreshAllBuildings = true;

	// Token: 0x040080CA RID: 32970
	private List<PlanScreen.ToggleEntry> toggleEntries = new List<PlanScreen.ToggleEntry>();

	// Token: 0x040080CB RID: 32971
	private int ignoreToolChangeMessages;

	// Token: 0x040080CC RID: 32972
	private Dictionary<string, PlanScreen.RequirementsState> _buildableStatesByID = new Dictionary<string, PlanScreen.RequirementsState>();

	// Token: 0x040080CD RID: 32973
	private Dictionary<Def, bool> _researchedDefs = new Dictionary<Def, bool>();

	// Token: 0x040080CE RID: 32974
	[SerializeField]
	private TextStyleSetting[] CategoryLabelTextStyles;

	// Token: 0x040080CF RID: 32975
	private float initTime;

	// Token: 0x040080D0 RID: 32976
	private Dictionary<Tag, HashedString> tagCategoryMap;

	// Token: 0x040080D1 RID: 32977
	private Dictionary<Tag, int> tagOrderMap;

	// Token: 0x040080D2 RID: 32978
	private BuildingDef lastSelectedBuildingDef;

	// Token: 0x040080D3 RID: 32979
	private Building lastSelectedBuilding;

	// Token: 0x040080D4 RID: 32980
	private string lastSelectedBuildingFacade = "DEFAULT_FACADE";

	// Token: 0x040080D5 RID: 32981
	private int buildable_state_update_idx;

	// Token: 0x040080D6 RID: 32982
	private int building_button_refresh_idx;

	// Token: 0x040080D7 RID: 32983
	private int maxToggleRefreshPerFrame = 10;

	// Token: 0x040080D8 RID: 32984
	private bool categoryPanelSizeNeedsRefresh;

	// Token: 0x040080D9 RID: 32985
	private float buildGrid_bg_width = 320f;

	// Token: 0x040080DA RID: 32986
	private float buildGrid_bg_borderHeight = 48f;

	// Token: 0x040080DB RID: 32987
	private const float BUILDGRID_SEARCHBAR_HEIGHT = 36f;

	// Token: 0x040080DC RID: 32988
	private const int SUBCATEGORY_HEADER_HEIGHT = 24;

	// Token: 0x040080DD RID: 32989
	private float buildGrid_bg_rowHeight;

	// Token: 0x02001EA5 RID: 7845
	public struct PlanInfo
	{
		// Token: 0x0600A4DF RID: 42207 RVA: 0x003EA028 File Offset: 0x003E8228
		public PlanInfo(HashedString category, bool hideIfNotResearched, List<string> listData, string RequiredDlcId = "")
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (string key in listData)
			{
				list.Add(new KeyValuePair<string, string>(key, TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(key) ? TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[key] : "uncategorized"));
			}
			this.category = category;
			this.hideIfNotResearched = hideIfNotResearched;
			this.data = listData;
			this.buildingAndSubcategoryData = list;
			this.RequiredDlcId = RequiredDlcId;
		}

		// Token: 0x040080DE RID: 32990
		public HashedString category;

		// Token: 0x040080DF RID: 32991
		public bool hideIfNotResearched;

		// Token: 0x040080E0 RID: 32992
		[Obsolete("Modders: Use ModUtil.AddBuildingToPlanScreen")]
		public List<string> data;

		// Token: 0x040080E1 RID: 32993
		public List<KeyValuePair<string, string>> buildingAndSubcategoryData;

		// Token: 0x040080E2 RID: 32994
		public string RequiredDlcId;
	}

	// Token: 0x02001EA6 RID: 7846
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x040080E3 RID: 32995
		public TextStyleSetting BuildButtonName;

		// Token: 0x040080E4 RID: 32996
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x040080E5 RID: 32997
		public TextStyleSetting MaterialRequirement;

		// Token: 0x040080E6 RID: 32998
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02001EA7 RID: 7847
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x040080E7 RID: 32999
		public TextStyleSetting ActiveSelected;

		// Token: 0x040080E8 RID: 33000
		public TextStyleSetting ActiveDeselected;

		// Token: 0x040080E9 RID: 33001
		public TextStyleSetting InactiveSelected;

		// Token: 0x040080EA RID: 33002
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02001EA8 RID: 7848
	private class ToggleEntry
	{
		// Token: 0x0600A4E0 RID: 42208 RVA: 0x003EA0C4 File Offset: 0x003E82C4
		public ToggleEntry(KIconToggleMenu.ToggleInfo toggle_info, HashedString plan_category, List<BuildingDef> building_defs, bool hideIfNotResearched)
		{
			this.toggleInfo = toggle_info;
			this.planCategory = plan_category;
			building_defs.RemoveAll((BuildingDef def) => !def.IsValidDLC());
			this.buildingDefs = building_defs;
			this.hideIfNotResearched = hideIfNotResearched;
			this.pendingResearchAttentions = new List<Tag>();
			this.requiredTechItems = new List<TechItem>();
			this.toggleImages = null;
			foreach (BuildingDef buildingDef in building_defs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(buildingDef.PrefabID);
				if (techItem == null)
				{
					this.requiredTechItems.Clear();
					break;
				}
				if (!this.requiredTechItems.Contains(techItem))
				{
					this.requiredTechItems.Add(techItem);
				}
			}
			this._areAnyRequiredTechItemsAvailable = false;
			this.Refresh();
		}

		// Token: 0x0600A4E1 RID: 42209 RVA: 0x0010AEC8 File Offset: 0x001090C8
		public bool AreAnyRequiredTechItemsAvailable()
		{
			return this._areAnyRequiredTechItemsAvailable;
		}

		// Token: 0x0600A4E2 RID: 42210 RVA: 0x003EA1C0 File Offset: 0x003E83C0
		public void Refresh()
		{
			if (this._areAnyRequiredTechItemsAvailable)
			{
				return;
			}
			if (this.requiredTechItems.Count == 0)
			{
				this._areAnyRequiredTechItemsAvailable = true;
				return;
			}
			using (List<TechItem>.Enumerator enumerator = this.requiredTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (PlanScreen.TechRequirementsUpcoming(enumerator.Current))
					{
						this._areAnyRequiredTechItemsAvailable = true;
						break;
					}
				}
			}
		}

		// Token: 0x0600A4E3 RID: 42211 RVA: 0x0010AED0 File Offset: 0x001090D0
		public void CollectToggleImages()
		{
			this.toggleImages = this.toggleInfo.toggle.gameObject.GetComponents<ImageToggleState>();
		}

		// Token: 0x040080EB RID: 33003
		public KIconToggleMenu.ToggleInfo toggleInfo;

		// Token: 0x040080EC RID: 33004
		public HashedString planCategory;

		// Token: 0x040080ED RID: 33005
		public List<BuildingDef> buildingDefs;

		// Token: 0x040080EE RID: 33006
		public List<Tag> pendingResearchAttentions;

		// Token: 0x040080EF RID: 33007
		private List<TechItem> requiredTechItems;

		// Token: 0x040080F0 RID: 33008
		public ImageToggleState[] toggleImages;

		// Token: 0x040080F1 RID: 33009
		public bool hideIfNotResearched;

		// Token: 0x040080F2 RID: 33010
		private bool _areAnyRequiredTechItemsAvailable;
	}

	// Token: 0x02001EAA RID: 7850
	public enum RequirementsState
	{
		// Token: 0x040080F6 RID: 33014
		Invalid,
		// Token: 0x040080F7 RID: 33015
		Tech,
		// Token: 0x040080F8 RID: 33016
		Materials,
		// Token: 0x040080F9 RID: 33017
		Complete,
		// Token: 0x040080FA RID: 33018
		TelepadBuilt,
		// Token: 0x040080FB RID: 33019
		UniquePerWorld,
		// Token: 0x040080FC RID: 33020
		RocketInteriorOnly,
		// Token: 0x040080FD RID: 33021
		RocketInteriorForbidden
	}
}
