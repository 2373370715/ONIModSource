﻿using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C09 RID: 7177
public class BuildMenu : KScreen
{
	// Token: 0x0600951F RID: 38175 RVA: 0x001011EE File Offset: 0x000FF3EE
	public override float GetSortKey()
	{
		return 6f;
	}

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06009520 RID: 38176 RVA: 0x001011F5 File Offset: 0x000FF3F5
	// (set) Token: 0x06009521 RID: 38177 RVA: 0x001011FC File Offset: 0x000FF3FC
	public static BuildMenu Instance { get; private set; }

	// Token: 0x06009522 RID: 38178 RVA: 0x00101204 File Offset: 0x000FF404
	public static void DestroyInstance()
	{
		BuildMenu.Instance = null;
	}

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x06009523 RID: 38179 RVA: 0x0010120C File Offset: 0x000FF40C
	public BuildingDef SelectedBuildingDef
	{
		get
		{
			return this.selectedBuilding;
		}
	}

	// Token: 0x06009524 RID: 38180 RVA: 0x00101214 File Offset: 0x000FF414
	private static HashedString CacheHashString(string str)
	{
		return HashCache.Get().Add(str);
	}

	// Token: 0x06009525 RID: 38181 RVA: 0x00101221 File Offset: 0x000FF421
	public static bool UseHotkeyBuildMenu()
	{
		return KPlayerPrefs.GetInt("ENABLE_HOTKEY_BUILD_MENU") != 0;
	}

	// Token: 0x06009526 RID: 38182 RVA: 0x00398DF0 File Offset: 0x00396FF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.initTime = KTime.Instance.UnscaledGameTime;
		bool flag = BuildMenu.UseHotkeyBuildMenu();
		if (flag)
		{
			BuildMenu.Instance = this;
			this.productInfoScreen = global::Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, base.gameObject, true);
			this.productInfoScreen.rectTransform().pivot = new Vector2(0f, 0f);
			this.productInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
			this.productInfoScreen.Show(false);
			this.buildingsScreen = global::Util.KInstantiateUI<BuildMenuBuildingsScreen>(this.buildingsMenuPrefab.gameObject, base.gameObject, true);
			BuildMenuBuildingsScreen buildMenuBuildingsScreen = this.buildingsScreen;
			buildMenuBuildingsScreen.onBuildingSelected = (Action<BuildingDef>)Delegate.Combine(buildMenuBuildingsScreen.onBuildingSelected, new Action<BuildingDef>(this.OnBuildingSelected));
			this.buildingsScreen.Show(false);
			Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
			Game.Instance.Subscribe(-1190690038, new Action<object>(this.OnBuildToolDeactivated));
			this.Initialize();
			this.rectTransform().anchoredPosition = Vector2.zero;
			return;
		}
		base.gameObject.SetActive(flag);
	}

	// Token: 0x06009527 RID: 38183 RVA: 0x00398F34 File Offset: 0x00397134
	private void Initialize()
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			BuildMenuCategoriesScreen value = keyValuePair.Value;
			value.Close();
			UnityEngine.Object.DestroyImmediate(value.gameObject);
		}
		this.submenuStack.Clear();
		this.tagCategoryMap = new Dictionary<Tag, HashedString>();
		this.tagOrderMap = new Dictionary<Tag, int>();
		this.categorizedBuildingMap = new Dictionary<HashedString, List<BuildingDef>>();
		this.categorizedCategoryMap = new Dictionary<HashedString, List<HashedString>>();
		int num = 0;
		BuildMenu.DisplayInfo orderedBuildings = BuildMenu.OrderedBuildings;
		this.PopulateCategorizedMaps(orderedBuildings.category, 0, orderedBuildings.data, this.tagCategoryMap, this.tagOrderMap, ref num, this.categorizedBuildingMap, this.categorizedCategoryMap);
		BuildMenuCategoriesScreen buildMenuCategoriesScreen = this.submenus[BuildMenu.ROOT_HASHSTR];
		buildMenuCategoriesScreen.Show(true);
		buildMenuCategoriesScreen.modalKeyInputBehaviour = false;
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair2 in this.submenus)
		{
			HashedString key = keyValuePair2.Key;
			List<HashedString> list;
			if (!(key == BuildMenu.ROOT_HASHSTR) && this.categorizedCategoryMap.TryGetValue(key, out list))
			{
				Image component = keyValuePair2.Value.GetComponent<Image>();
				if (component != null)
				{
					component.enabled = (list.Count > 0);
				}
			}
		}
		this.PositionMenus();
	}

	// Token: 0x06009528 RID: 38184 RVA: 0x003990B4 File Offset: 0x003972B4
	[ContextMenu("PositionMenus")]
	private void PositionMenus()
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			HashedString key = keyValuePair.Key;
			BuildMenuCategoriesScreen value = keyValuePair.Value;
			LayoutGroup component = value.GetComponent<LayoutGroup>();
			Vector2 anchoredPosition;
			BuildMenu.PadInfo padInfo;
			if (key == BuildMenu.ROOT_HASHSTR)
			{
				anchoredPosition = this.rootMenuOffset;
				padInfo = this.rootMenuPadding;
				value.GetComponent<Image>().enabled = false;
			}
			else
			{
				anchoredPosition = this.nestedMenuOffset;
				padInfo = this.nestedMenuPadding;
			}
			value.rectTransform().anchoredPosition = anchoredPosition;
			component.padding.left = padInfo.left;
			component.padding.right = padInfo.right;
			component.padding.top = padInfo.top;
			component.padding.bottom = padInfo.bottom;
		}
		this.buildingsScreen.rectTransform().anchoredPosition = this.buildingsMenuOffset;
	}

	// Token: 0x06009529 RID: 38185 RVA: 0x003991C4 File Offset: 0x003973C4
	public void Refresh()
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			keyValuePair.Value.UpdateBuildableStates(true);
		}
	}

	// Token: 0x0600952A RID: 38186 RVA: 0x00101230 File Offset: 0x000FF430
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x0600952B RID: 38187 RVA: 0x00101254 File Offset: 0x000FF454
	protected override void OnCmpDisable()
	{
		Game.Instance.Unsubscribe(-107300940, new Action<object>(this.OnResearchComplete));
		base.OnCmpDisable();
	}

	// Token: 0x0600952C RID: 38188 RVA: 0x00399220 File Offset: 0x00397420
	private BuildMenuCategoriesScreen CreateCategorySubMenu(HashedString category, int depth, object data, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map, Dictionary<Tag, HashedString> tag_category_map, BuildMenuBuildingsScreen buildings_screen)
	{
		BuildMenuCategoriesScreen buildMenuCategoriesScreen = global::Util.KInstantiateUI<BuildMenuCategoriesScreen>(this.categoriesMenuPrefab.gameObject, base.gameObject, true);
		buildMenuCategoriesScreen.Show(false);
		buildMenuCategoriesScreen.Configure(category, depth, data, this.categorizedBuildingMap, this.categorizedCategoryMap, this.buildingsScreen);
		buildMenuCategoriesScreen.onCategoryClicked = (Action<HashedString, int>)Delegate.Combine(buildMenuCategoriesScreen.onCategoryClicked, new Action<HashedString, int>(this.OnCategoryClicked));
		buildMenuCategoriesScreen.name = "BuildMenu_" + category.ToString();
		return buildMenuCategoriesScreen;
	}

	// Token: 0x0600952D RID: 38189 RVA: 0x003992A8 File Offset: 0x003974A8
	private void PopulateCategorizedMaps(HashedString category, int depth, object data, Dictionary<Tag, HashedString> category_map, Dictionary<Tag, int> order_map, ref int building_index, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map)
	{
		Type type = data.GetType();
		if (type == typeof(BuildMenu.DisplayInfo))
		{
			BuildMenu.DisplayInfo displayInfo = (BuildMenu.DisplayInfo)data;
			List<HashedString> list;
			if (!categorized_category_map.TryGetValue(category, out list))
			{
				list = new List<HashedString>();
				categorized_category_map[category] = list;
			}
			list.Add(displayInfo.category);
			this.PopulateCategorizedMaps(displayInfo.category, depth + 1, displayInfo.data, category_map, order_map, ref building_index, categorized_building_map, categorized_category_map);
		}
		else
		{
			if (typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(type))
			{
				IEnumerable<BuildMenu.DisplayInfo> enumerable = (IList<BuildMenu.DisplayInfo>)data;
				List<HashedString> list2;
				if (!categorized_category_map.TryGetValue(category, out list2))
				{
					list2 = new List<HashedString>();
					categorized_category_map[category] = list2;
				}
				using (IEnumerator<BuildMenu.DisplayInfo> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BuildMenu.DisplayInfo displayInfo2 = enumerator.Current;
						list2.Add(displayInfo2.category);
						this.PopulateCategorizedMaps(displayInfo2.category, depth + 1, displayInfo2.data, category_map, order_map, ref building_index, categorized_building_map, categorized_category_map);
					}
					goto IL_195;
				}
			}
			foreach (BuildMenu.BuildingInfo buildingInfo in ((IList<BuildMenu.BuildingInfo>)data))
			{
				Tag key = new Tag(buildingInfo.id);
				category_map[key] = category;
				order_map[key] = building_index;
				building_index++;
				List<BuildingDef> list3;
				if (!categorized_building_map.TryGetValue(category, out list3))
				{
					list3 = new List<BuildingDef>();
					categorized_building_map[category] = list3;
				}
				BuildingDef buildingDef = Assets.GetBuildingDef(buildingInfo.id);
				buildingDef.HotKey = buildingInfo.hotkey;
				list3.Add(buildingDef);
			}
		}
		IL_195:
		this.submenus[category] = this.CreateCategorySubMenu(category, depth, data, this.categorizedBuildingMap, this.categorizedCategoryMap, this.tagCategoryMap, this.buildingsScreen);
	}

	// Token: 0x0600952E RID: 38190 RVA: 0x00399494 File Offset: 0x00397694
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.mouseOver && base.ConsumeMouseScroll && !e.TryConsume(global::Action.ZoomIn))
		{
			e.TryConsume(global::Action.ZoomOut);
		}
		if (!e.Consumed && this.selectedCategory.IsValid && e.TryConsume(global::Action.Escape))
		{
			this.OnUIClear(null);
			return;
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600952F RID: 38191 RVA: 0x00399500 File Offset: 0x00397700
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.selectedCategory.IsValid && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.OnUIClear(null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06009530 RID: 38192 RVA: 0x00399550 File Offset: 0x00397750
	private void OnUIClear(object data)
	{
		SelectTool.Instance.Activate();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		SelectTool.Instance.Select(null, true);
		this.productInfoScreen.materialSelectionPanel.PriorityScreen.ResetPriority();
		this.CloseMenus();
	}

	// Token: 0x06009531 RID: 38193 RVA: 0x00101277 File Offset: 0x000FF477
	private void OnBuildToolDeactivated(object data)
	{
		if (this.updating)
		{
			this.deactivateToolQueued = true;
			return;
		}
		this.CloseMenus();
		this.productInfoScreen.materialSelectionPanel.PriorityScreen.ResetPriority();
	}

	// Token: 0x06009532 RID: 38194 RVA: 0x003995A0 File Offset: 0x003977A0
	private void CloseMenus()
	{
		this.productInfoScreen.Close();
		while (this.submenuStack.Count > 0)
		{
			this.submenuStack.Pop().Close();
			this.productInfoScreen.Close();
		}
		this.selectedCategory = HashedString.Invalid;
		this.submenus[BuildMenu.ROOT_HASHSTR].ClearSelection();
	}

	// Token: 0x06009533 RID: 38195 RVA: 0x001012A4 File Offset: 0x000FF4A4
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.timeSinceNotificationPing < 8f)
		{
			this.timeSinceNotificationPing += Time.unscaledDeltaTime;
		}
		if (this.timeSinceNotificationPing >= 0.5f)
		{
			this.notificationPingCount = 0;
		}
	}

	// Token: 0x06009534 RID: 38196 RVA: 0x00399604 File Offset: 0x00397804
	public void PlayNewBuildingSounds()
	{
		if (KTime.Instance.UnscaledGameTime - this.initTime > 1.5f)
		{
			if (BuildMenu.Instance.timeSinceNotificationPing >= 8f)
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
				instance.setParameterByName("playCount", (float)BuildMenu.Instance.notificationPingCount, false);
				SoundEvent.EndOneShot(instance);
			}
		}
		this.timeSinceNotificationPing = 0f;
		this.notificationPingCount++;
	}

	// Token: 0x06009535 RID: 38197 RVA: 0x003996D4 File Offset: 0x003978D4
	public PlanScreen.RequirementsState BuildableState(BuildingDef def)
	{
		PlanScreen.RequirementsState result = PlanScreen.RequirementsState.Complete;
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)
		{
			if (!Db.Get().TechItems.IsTechItemComplete(def.PrefabID))
			{
				result = PlanScreen.RequirementsState.Tech;
			}
			else if (!ProductInfoScreen.MaterialsMet(def.CraftRecipe))
			{
				result = PlanScreen.RequirementsState.Materials;
			}
		}
		return result;
	}

	// Token: 0x06009536 RID: 38198 RVA: 0x001012E0 File Offset: 0x000FF4E0
	private void CloseProductInfoScreen()
	{
		this.productInfoScreen.ClearProduct(true);
		this.productInfoScreen.Show(false);
	}

	// Token: 0x06009537 RID: 38199 RVA: 0x00399724 File Offset: 0x00397924
	private void Update()
	{
		if (this.deactivateToolQueued)
		{
			this.deactivateToolQueued = false;
			this.OnBuildToolDeactivated(null);
		}
		this.elapsedTime += Time.unscaledDeltaTime;
		if (this.elapsedTime <= this.updateInterval)
		{
			return;
		}
		this.elapsedTime = 0f;
		this.updating = true;
		foreach (KIconToggleMenu kiconToggleMenu in this.submenuStack)
		{
			if (kiconToggleMenu is BuildMenuCategoriesScreen)
			{
				(kiconToggleMenu as BuildMenuCategoriesScreen).UpdateBuildableStates(false);
			}
		}
		this.submenus[BuildMenu.ROOT_HASHSTR].UpdateBuildableStates(false);
		this.updating = false;
	}

	// Token: 0x06009538 RID: 38200 RVA: 0x003997EC File Offset: 0x003979EC
	private void OnRecipeElementsFullySelected()
	{
		if (this.selectedBuilding == null)
		{
			global::Debug.Log("No def!");
		}
		if (this.selectedBuilding.isKAnimTile && this.selectedBuilding.isUtility)
		{
			IList<Tag> getSelectedElementAsList = this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
			((this.selectedBuilding.BuildingComplete.GetComponent<Wire>() != null) ? WireBuildTool.Instance : UtilityBuildTool.Instance).Activate(this.selectedBuilding, getSelectedElementAsList);
			return;
		}
		BuildTool.Instance.Activate(this.selectedBuilding, this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList);
	}

	// Token: 0x06009539 RID: 38201 RVA: 0x00399890 File Offset: 0x00397A90
	private void OnBuildingSelected(BuildingDef def)
	{
		if (this.selecting)
		{
			return;
		}
		this.selecting = true;
		this.selectedBuilding = def;
		this.buildingsScreen.SetHasFocus(false);
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			keyValuePair.Value.SetHasFocus(false);
		}
		ToolMenu.Instance.ClearSelection();
		if (def != null)
		{
			Vector2 anchoredPosition = this.productInfoScreen.rectTransform().anchoredPosition;
			RectTransform rectTransform = this.buildingsScreen.rectTransform();
			anchoredPosition.y = rectTransform.anchoredPosition.y;
			anchoredPosition.x = rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x + 10f;
			this.productInfoScreen.rectTransform().anchoredPosition = anchoredPosition;
			this.productInfoScreen.ClearProduct(false);
			this.productInfoScreen.Show(true);
			this.productInfoScreen.ConfigureScreen(def);
		}
		else
		{
			this.productInfoScreen.Close();
		}
		this.selecting = false;
	}

	// Token: 0x0600953A RID: 38202 RVA: 0x003999C0 File Offset: 0x00397BC0
	private void OnCategoryClicked(HashedString new_category, int depth)
	{
		while (this.submenuStack.Count > depth)
		{
			KIconToggleMenu kiconToggleMenu = this.submenuStack.Pop();
			kiconToggleMenu.ClearSelection();
			kiconToggleMenu.Close();
		}
		this.productInfoScreen.Close();
		if (new_category != this.selectedCategory && new_category.IsValid)
		{
			foreach (KIconToggleMenu kiconToggleMenu2 in this.submenuStack)
			{
				if (kiconToggleMenu2 is BuildMenuCategoriesScreen)
				{
					(kiconToggleMenu2 as BuildMenuCategoriesScreen).SetHasFocus(false);
				}
			}
			this.selectedCategory = new_category;
			BuildMenuCategoriesScreen buildMenuCategoriesScreen;
			this.submenus.TryGetValue(new_category, out buildMenuCategoriesScreen);
			if (buildMenuCategoriesScreen != null)
			{
				buildMenuCategoriesScreen.Show(true);
				buildMenuCategoriesScreen.SetHasFocus(true);
				this.submenuStack.Push(buildMenuCategoriesScreen);
			}
		}
		else
		{
			this.selectedCategory = HashedString.Invalid;
		}
		foreach (KIconToggleMenu kiconToggleMenu3 in this.submenuStack)
		{
			if (kiconToggleMenu3 is BuildMenuCategoriesScreen)
			{
				(kiconToggleMenu3 as BuildMenuCategoriesScreen).UpdateBuildableStates(true);
			}
		}
		this.submenus[BuildMenu.ROOT_HASHSTR].UpdateBuildableStates(true);
	}

	// Token: 0x0600953B RID: 38203 RVA: 0x001012FA File Offset: 0x000FF4FA
	public void RefreshProductInfoScreen(BuildingDef def)
	{
		if (this.productInfoScreen.currentDef == def)
		{
			this.productInfoScreen.ClearProduct(false);
			this.productInfoScreen.Show(true);
			this.productInfoScreen.ConfigureScreen(def);
		}
	}

	// Token: 0x0600953C RID: 38204 RVA: 0x00399B18 File Offset: 0x00397D18
	private HashedString GetParentCategory(HashedString desired_category)
	{
		foreach (KeyValuePair<HashedString, List<HashedString>> keyValuePair in this.categorizedCategoryMap)
		{
			using (List<HashedString>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == desired_category)
					{
						return keyValuePair.Key;
					}
				}
			}
		}
		return HashedString.Invalid;
	}

	// Token: 0x0600953D RID: 38205 RVA: 0x00399BB8 File Offset: 0x00397DB8
	private void AddParentCategories(HashedString child_category, ICollection<HashedString> categories)
	{
		for (;;)
		{
			HashedString parentCategory = this.GetParentCategory(child_category);
			if (parentCategory == HashedString.Invalid)
			{
				break;
			}
			categories.Add(parentCategory);
			child_category = parentCategory;
		}
	}

	// Token: 0x0600953E RID: 38206 RVA: 0x00399BE8 File Offset: 0x00397DE8
	private void OnResearchComplete(object data)
	{
		HashSet<HashedString> hashSet = new HashSet<HashedString>();
		if (data is Tech)
		{
			Tech tech = (Tech)data;
			using (List<TechItem>.Enumerator enumerator = tech.unlockedItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TechItem techItem = enumerator.Current;
					BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
					if (buildingDef == null)
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							string.Format("Tech '{0}' unlocked building '{1}' but no such building exists", tech.Name, techItem.Id)
						});
					}
					else
					{
						HashedString hashedString = this.tagCategoryMap[buildingDef.Tag];
						hashSet.Add(hashedString);
						this.AddParentCategories(hashedString, hashSet);
					}
				}
				goto IL_E0;
			}
		}
		if (data is BuildingDef)
		{
			BuildingDef buildingDef2 = data as BuildingDef;
			HashedString hashedString2 = this.tagCategoryMap[buildingDef2.Tag];
			hashSet.Add(hashedString2);
			this.AddParentCategories(hashedString2, hashSet);
		}
		IL_E0:
		this.UpdateNotifications(hashSet, BuildMenu.OrderedBuildings);
	}

	// Token: 0x0600953F RID: 38207 RVA: 0x00399CF8 File Offset: 0x00397EF8
	private void UpdateNotifications(ICollection<HashedString> updated_categories, object data)
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			keyValuePair.Value.UpdateNotifications(updated_categories);
		}
	}

	// Token: 0x06009540 RID: 38208 RVA: 0x00101333 File Offset: 0x000FF533
	public PrioritySetting GetBuildingPriority()
	{
		return this.productInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
	}

	// Token: 0x040073BC RID: 29628
	public const string ENABLE_HOTKEY_BUILD_MENU_KEY = "ENABLE_HOTKEY_BUILD_MENU";

	// Token: 0x040073BD RID: 29629
	[SerializeField]
	private BuildMenuCategoriesScreen categoriesMenuPrefab;

	// Token: 0x040073BE RID: 29630
	[SerializeField]
	private BuildMenuBuildingsScreen buildingsMenuPrefab;

	// Token: 0x040073BF RID: 29631
	[SerializeField]
	private GameObject productInfoScreenPrefab;

	// Token: 0x040073C0 RID: 29632
	private ProductInfoScreen productInfoScreen;

	// Token: 0x040073C1 RID: 29633
	private BuildMenuBuildingsScreen buildingsScreen;

	// Token: 0x040073C2 RID: 29634
	private BuildingDef selectedBuilding;

	// Token: 0x040073C3 RID: 29635
	private HashedString selectedCategory;

	// Token: 0x040073C4 RID: 29636
	private static readonly HashedString ROOT_HASHSTR = new HashedString("ROOT");

	// Token: 0x040073C5 RID: 29637
	private Dictionary<HashedString, BuildMenuCategoriesScreen> submenus = new Dictionary<HashedString, BuildMenuCategoriesScreen>();

	// Token: 0x040073C6 RID: 29638
	private Stack<KIconToggleMenu> submenuStack = new Stack<KIconToggleMenu>();

	// Token: 0x040073C7 RID: 29639
	private bool selecting;

	// Token: 0x040073C8 RID: 29640
	private bool updating;

	// Token: 0x040073C9 RID: 29641
	private bool deactivateToolQueued;

	// Token: 0x040073CA RID: 29642
	[SerializeField]
	private Vector2 rootMenuOffset = Vector2.zero;

	// Token: 0x040073CB RID: 29643
	[SerializeField]
	private BuildMenu.PadInfo rootMenuPadding;

	// Token: 0x040073CC RID: 29644
	[SerializeField]
	private Vector2 nestedMenuOffset = Vector2.zero;

	// Token: 0x040073CD RID: 29645
	[SerializeField]
	private BuildMenu.PadInfo nestedMenuPadding;

	// Token: 0x040073CE RID: 29646
	[SerializeField]
	private Vector2 buildingsMenuOffset = Vector2.zero;

	// Token: 0x040073CF RID: 29647
	public static BuildMenu.DisplayInfo OrderedBuildings = new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("ROOT"), "icon_category_base", global::Action.NumActions, KKeyCode.None, new List<BuildMenu.DisplayInfo>
	{
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Base"), "icon_category_base", global::Action.Plan1, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Tiles"), "icon_category_base", global::Action.BuildCategoryTiles, KKeyCode.T, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Tile", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SnowTile", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("WoodTile", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("GasPermeableMembrane", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("MeshTile", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("InsulationTile", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("PlasticTile", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("MetalTile", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("GlassTile", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("BunkerTile", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("CarpetTile", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("ExteriorWall", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("ExobaseHeadquarters", global::Action.BuildMenuKeyH)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ladders"), "icon_category_base", global::Action.BuildCategoryLadders, KKeyCode.A, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Ladder", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LadderFast", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("FirePole", global::Action.BuildMenuKeyF)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Doors"), "icon_category_base", global::Action.BuildCategoryDoors, KKeyCode.D, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Door", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("ManualPressureDoor", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("PressureDoor", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("BunkerDoor", global::Action.BuildMenuKeyB)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Storage"), "icon_category_base", global::Action.BuildCategoryStorage, KKeyCode.S, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("StorageLocker", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("RationBox", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("Refrigerator", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("StorageLockerSmart", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LiquidReservoir", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("GasReservoir", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("ObjectDispenser", global::Action.BuildMenuKeyO)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Research"), "icon_category_misc", global::Action.BuildCategoryResearch, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("ResearchCenter", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("AdvancedResearchCenter", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("CosmicResearchCenter", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("DLC1CosmicResearchCenter", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("NuclearResearchCenter", global::Action.BuildMenuKeyN),
				new BuildMenu.BuildingInfo("Telescope", global::Action.BuildMenuKeyT)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Food And Agriculture"), "icon_category_food", global::Action.Plan2, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Farming"), "icon_category_food", global::Action.BuildCategoryFarming, KKeyCode.F, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("PlanterBox", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("FarmTile", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("HydroponicFarm", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("Compost", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("FertilizerMaker", global::Action.BuildMenuKeyR)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Cooking"), "icon_category_food", global::Action.BuildCategoryCooking, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("MicrobeMusher", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("CookingStation", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("Deepfryer", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("SpiceGrinder", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("GourmetCookingStation", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("EggCracker", global::Action.BuildMenuKeyE)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ranching"), "icon_category_food", global::Action.BuildCategoryRanching, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("CreatureDeliveryPoint", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("FishDeliveryPoint", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("CreatureFeeder", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("FishFeeder", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("RanchStation", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("ShearingStation", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("EggIncubator", global::Action.BuildMenuKeyI),
				new BuildMenu.BuildingInfo("CreatureGroundTrap", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("WaterTrap", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("CreatureAirTrap", global::Action.BuildMenuKeyL)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Health And Happiness"), "icon_category_medical", global::Action.Plan3, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Medical"), "icon_category_medical", global::Action.BuildCategoryMedical, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Apothecary", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("DoctorStation", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("AdvancedDoctorStation", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("MedicalCot", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("MassageTable", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("Grave", global::Action.BuildMenuKeyR)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Hygiene"), "icon_category_medical", global::Action.BuildCategoryHygiene, KKeyCode.E, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Outhouse", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("FlushToilet", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo(ShowerConfig.ID, global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("WashBasin", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("WashSink", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("HandSanitizer", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("DecontaminationShower", global::Action.BuildMenuKeyD)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Furniture"), "icon_category_furniture", global::Action.BuildCategoryFurniture, KKeyCode.F, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Bed", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("LuxuryBed", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo(LadderBedConfig.ID, global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("DiningTable", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("FloorLamp", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("CeilingLight", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SunLamp", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("MercuryCeilingLight", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("RadiationLight", global::Action.BuildMenuKeyR)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Decor"), "icon_category_furniture", global::Action.BuildCategoryDecor, KKeyCode.D, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("FlowerVase", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("Canvas", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("CanvasWide", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("CanvasTall", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("Sculpture", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("IceSculpture", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("ItemPedestal", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("CrownMoulding", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("CornerMoulding", global::Action.BuildMenuKeyN)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Recreation"), "icon_category_medical", global::Action.BuildCategoryRecreation, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("WaterCooler", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("ArcadeMachine", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("Phonobox", global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("EspressoMachine", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("HotTub", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("MechanicalSurfboard", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("Sauna", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("BeachChair", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("Juicer", global::Action.BuildMenuKeyJ),
				new BuildMenu.BuildingInfo("SodaFountain", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("VerticalWindTunnel", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("ParkSign", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("Telephone", global::Action.BuildMenuKeyT)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Infrastructure"), "icon_category_utilities", global::Action.Plan4, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Wires"), "icon_category_electrical", global::Action.BuildCategoryWires, KKeyCode.W, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Wire", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("WireBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("HighWattageWire", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("WireBridgeHighWattage", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("WireRefined", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("WireRefinedBridge", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("WireRefinedHighWattage", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("WireRefinedBridgeHighWattage", global::Action.BuildMenuKeyA)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Generators"), "icon_category_electrical", global::Action.BuildCategoryGenerators, KKeyCode.G, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("ManualGenerator", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("Generator", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("WoodGasGenerator", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("NuclearReactor", global::Action.BuildMenuKeyN),
				new BuildMenu.BuildingInfo("HydrogenGenerator", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("MethaneGenerator", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("PetroleumGenerator", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("SteamTurbine", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SteamTurbine2", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SolarPanel", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("DevGenerator", global::Action.BuildMenuKeyX)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("PowerControl"), "icon_category_electrical", global::Action.BuildCategoryPowerControl, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Battery", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("BatteryMedium", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("BatterySmart", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("PowerTransformerSmall", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("PowerTransformer", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo(SwitchConfig.ID, global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo(TemperatureControlledSwitchConfig.ID, global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo(PressureSwitchLiquidConfig.ID, global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo(PressureSwitchGasConfig.ID, global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo(LogicPowerRelayConfig.ID, global::Action.BuildMenuKeyX)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Pipes"), "icon_category_plumbing", global::Action.BuildCategoryPipes, KKeyCode.E, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("LiquidConduit", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("LiquidConduitBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("InsulatedLiquidConduit", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("LiquidConduitRadiant", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("GasConduit", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("GasConduitBridge", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("InsulatedGasConduit", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("GasConduitRadiant", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("ContactConductivePipeBridge", global::Action.BuildMenuKeyA)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Plumbing Structures"), "icon_category_plumbing", global::Action.BuildCategoryPlumbingStructures, KKeyCode.B, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("IceKettle", global::Action.BuildMenuKeyI),
				new BuildMenu.BuildingInfo("LiquidPumpingStation", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("BottleEmptier", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("LiquidPump", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("LiquidMiniPump", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("LiquidValve", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LiquidLogicValve", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("LiquidVent", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo("LiquidFilter", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("LiquidConduitPreferentialFlow", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("LiquidConduitOverflow", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LiquidLimitValve", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortLiquid", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortLiquidUnloader", global::Action.BuildMenuKeyU)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ventilation Structures"), "icon_category_ventilation", global::Action.BuildCategoryVentilationStructures, KKeyCode.V, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("GasPump", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("GasMiniPump", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("GasValve", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("GasLogicValve", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("GasVent", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo("GasVentHighPressure", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("GasFilter", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("GasBottler", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("BottleEmptierGas", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("GasConduitPreferentialFlow", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("GasConduitOverflow", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("GasLimitValve", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortGas", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortGasUnloader", global::Action.BuildMenuKeyU)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Industrial"), "icon_category_refinery", global::Action.Plan5, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Oxygen"), "icon_category_oxygen", global::Action.BuildCategoryOxygen, KKeyCode.X, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("MineralDeoxidizer", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("SublimationStation", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("Oxysconce", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("AlgaeHabitat", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("AirFilter", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("CO2Scrubber", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("Electrolyzer", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("RustDeoxidizer", global::Action.BuildMenuKeyF)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Utilities"), "icon_category_utilities", global::Action.BuildCategoryUtilities, KKeyCode.T, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Campfire", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("DevHeater", global::Action.BuildMenuKeyH),
				new BuildMenu.BuildingInfo("SpaceHeater", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("LiquidHeater", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("IceCooledFan", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("IceMachine", global::Action.BuildMenuKeyI),
				new BuildMenu.BuildingInfo("AirConditioner", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LiquidConditioner", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("OreScrubber", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("ThermalBlock", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("HighEnergyParticleRedirector", global::Action.BuildMenuKeyP)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Refining"), "icon_category_refinery", global::Action.BuildCategoryRefining, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("WaterPurifier", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("AlgaeDistillery", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("EthanolDistillery", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("RockCrusher", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("SludgePress", global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("Kiln", global::Action.BuildMenuKeyZ),
				new BuildMenu.BuildingInfo("OilWellCap", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("OilRefinery", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("Polymerizer", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("MetalRefinery", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("GlassForge", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("OxyliteRefinery", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("SupermaterialRefinery", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("UraniumCentrifuge", global::Action.BuildMenuKeyU)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Equipment"), "icon_category_misc", global::Action.BuildCategoryEquipment, KKeyCode.S, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("RoleStation", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("FarmStation", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("PowerControlStation", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("AstronautTrainingCenter", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("ResetSkillsStation", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("CraftingTable", global::Action.BuildMenuKeyZ),
				new BuildMenu.BuildingInfo("OxygenMaskMarker", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("OxygenMaskLocker", global::Action.BuildMenuKeyY),
				new BuildMenu.BuildingInfo("ClothingFabricator", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SuitFabricator", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("SuitMarker", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("SuitLocker", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("JetSuitMarker", global::Action.BuildMenuKeyJ),
				new BuildMenu.BuildingInfo("JetSuitLocker", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("LeadSuitMarker", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("LeadSuitLocker", global::Action.BuildMenuKeyD)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Rocketry"), "icon_category_rocketry", global::Action.BuildCategoryRocketry, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Gantry", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortBridge", global::Action.BuildMenuKeyU),
				new BuildMenu.BuildingInfo("KeroseneEngine", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("SolidBooster", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("SteamEngine", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("LiquidFuelTank", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("CargoBay", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("GasCargoBay", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("LiquidCargoBay", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("SpecialCargoBay", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("SpecialCargoBayCluster", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("CommandModule", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("TouristModule", global::Action.BuildMenuKeyY),
				new BuildMenu.BuildingInfo("ResearchModule", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("HydrogenEngine", global::Action.BuildMenuKeyH),
				new BuildMenu.BuildingInfo("RailGun", global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("LandingBeacon", global::Action.BuildMenuKeyL)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Logistics"), "icon_category_ventilation", global::Action.Plan6, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("TravelTubes"), "icon_category_ventilation", global::Action.BuildCategoryTravelTubes, KKeyCode.T, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("TravelTube", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("TravelTubeEntrance", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("TravelTubeWallBridge", global::Action.BuildMenuKeyB)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Conveyance"), "icon_category_ventilation", global::Action.BuildCategoryConveyance, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("SolidTransferArm", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("SolidConduit", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("SolidConduitInbox", global::Action.BuildMenuKeyI),
				new BuildMenu.BuildingInfo("SolidConduitOutbox", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("SolidVent", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo("SolidLogicValve", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("SolidLimitValve", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("SolidConduitBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("AutoMiner", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortSolid", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortSolidUnloader", global::Action.BuildMenuKeyU)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicWiring"), "icon_category_automation", global::Action.BuildCategoryLogicWiring, KKeyCode.W, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("LogicWire", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("LogicWireBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("LogicRibbon", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LogicRibbonBridge", global::Action.BuildMenuKeyV)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicGates"), "icon_category_automation", global::Action.BuildCategoryLogicGates, KKeyCode.G, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("LogicGateAND", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LogicGateOR", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LogicGateXOR", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("LogicGateNOT", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("LogicGateBUFFER", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("LogicGateFILTER", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo(LogicMemoryConfig.ID, global::Action.BuildMenuKeyV)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicSwitches"), "icon_category_automation", global::Action.BuildCategoryLogicSwitches, KKeyCode.S, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo(LogicSwitchConfig.ID, global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo(LogicPressureSensorGasConfig.ID, global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo(LogicPressureSensorLiquidConfig.ID, global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo(LogicTemperatureSensorConfig.ID, global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo(LogicTimeOfDaySensorConfig.ID, global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo(LogicTimerSensorConfig.ID, global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo(LogicCritterCountSensorConfig.ID, global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo(LogicDiseaseSensorConfig.ID, global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo(LogicElementSensorGasConfig.ID, global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo(LogicWattageSensorConfig.ID, global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("FloorSwitch", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("Checkpoint", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo(CometDetectorConfig.ID, global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LogicDuplicantSensor", global::Action.BuildMenuKeyF)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("ConduitSensors"), "icon_category_automation", global::Action.BuildCategoryLogicConduits, KKeyCode.X, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo(LiquidConduitTemperatureSensorConfig.ID, global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo(LiquidConduitDiseaseSensorConfig.ID, global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo(LiquidConduitElementSensorConfig.ID, global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo(GasConduitTemperatureSensorConfig.ID, global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo(GasConduitDiseaseSensorConfig.ID, global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo(GasConduitElementSensorConfig.ID, global::Action.BuildMenuKeyS)
			})
		})
	});

	// Token: 0x040073D0 RID: 29648
	private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;

	// Token: 0x040073D1 RID: 29649
	private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;

	// Token: 0x040073D2 RID: 29650
	private Dictionary<Tag, HashedString> tagCategoryMap;

	// Token: 0x040073D3 RID: 29651
	private Dictionary<Tag, int> tagOrderMap;

	// Token: 0x040073D4 RID: 29652
	private const float NotificationPingExpire = 0.5f;

	// Token: 0x040073D5 RID: 29653
	private const float SpecialNotificationEmbellishDelay = 8f;

	// Token: 0x040073D6 RID: 29654
	private float timeSinceNotificationPing;

	// Token: 0x040073D7 RID: 29655
	private int notificationPingCount;

	// Token: 0x040073D8 RID: 29656
	private float initTime;

	// Token: 0x040073D9 RID: 29657
	private float updateInterval = 1f;

	// Token: 0x040073DA RID: 29658
	private float elapsedTime;

	// Token: 0x02001C0A RID: 7178
	[Serializable]
	private struct PadInfo
	{
		// Token: 0x040073DB RID: 29659
		public int left;

		// Token: 0x040073DC RID: 29660
		public int right;

		// Token: 0x040073DD RID: 29661
		public int top;

		// Token: 0x040073DE RID: 29662
		public int bottom;
	}

	// Token: 0x02001C0B RID: 7179
	public struct BuildingInfo
	{
		// Token: 0x06009543 RID: 38211 RVA: 0x0010134A File Offset: 0x000FF54A
		public BuildingInfo(string id, global::Action hotkey)
		{
			this.id = id;
			this.hotkey = hotkey;
		}

		// Token: 0x040073DF RID: 29663
		public string id;

		// Token: 0x040073E0 RID: 29664
		public global::Action hotkey;
	}

	// Token: 0x02001C0C RID: 7180
	public struct DisplayInfo
	{
		// Token: 0x06009544 RID: 38212 RVA: 0x0010135A File Offset: 0x000FF55A
		public DisplayInfo(HashedString category, string icon_name, global::Action hotkey, KKeyCode key_code, object data)
		{
			this.category = category;
			this.iconName = icon_name;
			this.hotkey = hotkey;
			this.keyCode = key_code;
			this.data = data;
		}

		// Token: 0x06009545 RID: 38213 RVA: 0x0039B628 File Offset: 0x00399828
		public BuildMenu.DisplayInfo GetInfo(HashedString category)
		{
			BuildMenu.DisplayInfo displayInfo = default(BuildMenu.DisplayInfo);
			if (this.data != null && typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(this.data.GetType()))
			{
				foreach (BuildMenu.DisplayInfo displayInfo2 in ((IList<BuildMenu.DisplayInfo>)this.data))
				{
					displayInfo = displayInfo2.GetInfo(category);
					if (displayInfo.category == category)
					{
						break;
					}
					if (displayInfo2.category == category)
					{
						displayInfo = displayInfo2;
						break;
					}
				}
			}
			return displayInfo;
		}

		// Token: 0x040073E1 RID: 29665
		public HashedString category;

		// Token: 0x040073E2 RID: 29666
		public string iconName;

		// Token: 0x040073E3 RID: 29667
		public global::Action hotkey;

		// Token: 0x040073E4 RID: 29668
		public KKeyCode keyCode;

		// Token: 0x040073E5 RID: 29669
		public object data;
	}
}
