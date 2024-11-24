using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KleiInventoryScreen : KModalScreen
{
	private enum PermitPrintabilityState
	{
		Printable,
		AlreadyOwned,
		TooExpensive,
		NotForSale,
		NotForSaleYet,
		UserOffline
	}

	private enum MultiToggleState
	{
		Default,
		Selected,
		NonInteractable
	}

	[Header("Header")]
	[SerializeField]
	private KButton closeButton;

	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	[SerializeField]
	private GameObject categoryRowPrefab;

	private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	[SerializeField]
	private RectTransform galleryGridContent;

	[SerializeField]
	private GameObject gridItemPrefab;

	[SerializeField]
	private GameObject subcategoryPrefab;

	[SerializeField]
	private GameObject itemDummyPrefab;

	[Header("GalleryFilters")]
	[SerializeField]
	private KInputTextField searchField;

	[SerializeField]
	private KButton clearSearchButton;

	[SerializeField]
	private MultiToggle doublesOnlyToggle;

	public const int FILTER_SHOW_ALL = 0;

	public const int FILTER_SHOW_OWNED_ONLY = 1;

	public const int FILTER_SHOW_DOUBLES_ONLY = 2;

	private int showFilterState;

	[Header("BarterSection")]
	[SerializeField]
	private Image barterPanelBG;

	[SerializeField]
	private KButton barterBuyButton;

	[SerializeField]
	private KButton barterSellButton;

	[SerializeField]
	private GameObject barterConfirmationScreenPrefab;

	[SerializeField]
	private GameObject filamentWalletSection;

	[SerializeField]
	private GameObject barterOfflineLabel;

	private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();

	private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

	private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

	private GridLayouter galleryGridLayouter;

	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	[SerializeField]
	private KScrollRect selectionDetailsScrollRect;

	[SerializeField]
	private RectTransform selectionDetailsScrollRectScrollBarContainer;

	[SerializeField]
	private LocText selectionNameLabel;

	[SerializeField]
	private LocText selectionDescriptionLabel;

	[SerializeField]
	private LocText selectionFacadeForLabel;

	[SerializeField]
	private LocText selectionCollectionLabel;

	[SerializeField]
	private LocText selectionRarityDetailsLabel;

	[SerializeField]
	private LocText selectionOwnedCount;

	private bool IS_ONLINE;

	private bool initConfigComplete;

	private PermitResource SelectedPermit { get; set; }

	private string SelectedCategoryId { get; set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		closeButton.onClick += delegate
		{
			Show(show: false);
		};
		base.ConsumeMouseScroll = true;
		galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = new List<GridLayoutGroup>()
		};
		galleryGridLayouter.overrideParentForSizeReference = galleryGridContent;
		InventoryOrganization.Initialize();
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
		{
			Show(show: false);
		}
		base.OnKeyDown(e);
	}

	public override float GetSortKey()
	{
		return 20f;
	}

	protected override void OnActivate()
	{
		OnShow(show: true);
	}

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			InitConfig();
			ToggleDoublesOnly(0);
			ClearSearch();
		}
	}

	private void ToggleDoublesOnly(int newState)
	{
		showFilterState = newState;
		doublesOnlyToggle.ChangeState(showFilterState);
		doublesOnlyToggle.GetComponentInChildren<LocText>().text = showFilterState + "+";
		string simpleTooltip = "";
		switch (showFilterState)
		{
		case 0:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_ALL_ITEMS;
			break;
		case 1:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_OWNED_ONLY;
			break;
		case 2:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_DOUBLES_ONLY;
			break;
		}
		ToolTip component = doublesOnlyToggle.GetComponent<ToolTip>();
		component.SetSimpleTooltip(simpleTooltip);
		component.refreshWhileHovering = true;
		component.forceRefresh = true;
		RefreshGallery();
	}

	private void InitConfig()
	{
		if (!initConfigComplete)
		{
			initConfigComplete = true;
			galleryGridLayouter.RequestGridResize();
			categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
			PopulateCategories();
			PopulateGallery();
			SelectCategory("BUILDINGS");
			searchField.onValueChanged.RemoveAllListeners();
			searchField.onValueChanged.AddListener(delegate
			{
				RefreshGallery();
			});
			clearSearchButton.ClearOnClick();
			clearSearchButton.onClick += ClearSearch;
			MultiToggle multiToggle = doublesOnlyToggle;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
			{
				int newState = (showFilterState + 1) % 3;
				ToggleDoublesOnly(newState);
			});
		}
	}

	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		ToggleDoublesOnly(0);
		ClearSearch();
		if (!initConfigComplete)
		{
			InitConfig();
		}
		RefreshUI();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			RefreshUI();
		});
	}

	private void ClearSearch()
	{
		searchField.text = "";
		searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
		RefreshGallery();
	}

	private void Update()
	{
		galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	private void RefreshUI()
	{
		IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
		RefreshCategories();
		RefreshGallery();
		if (SelectedCategoryId.IsNullOrWhiteSpace())
		{
			SelectCategory("BUILDINGS");
		}
		RefreshDetails();
		RefreshBarterPanel();
	}

	private GameObject GetAvailableGridButton()
	{
		if (recycledGalleryGridButtons.Count == 0)
		{
			return Util.KInstantiateUI(gridItemPrefab, galleryGridContent.gameObject, force_active: true);
		}
		GameObject result = recycledGalleryGridButtons[0];
		recycledGalleryGridButtons.RemoveAt(0);
		return result;
	}

	private void RecycleGalleryGridButton(GameObject button)
	{
		button.GetComponent<MultiToggle>().onClick = null;
		recycledGalleryGridButtons.Add(button);
	}

	public void PopulateCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> categoryToggle in categoryToggles)
		{
			UnityEngine.Object.Destroy(categoryToggle.Value.gameObject);
		}
		categoryToggles.Clear();
		foreach (KeyValuePair<string, List<string>> item in InventoryOrganization.categoryIdToSubcategoryIdsMap)
		{
			Util.Deconstruct(item, out var key, out var _);
			string categoryId = key;
			GameObject obj = Util.KInstantiateUI(categoryRowPrefab, categoryListContent.gameObject, force_active: true);
			HierarchyReferences component = obj.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
			component.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
			MultiToggle component2 = obj.GetComponent<MultiToggle>();
			component2.onEnter = (System.Action)Delegate.Combine(component2.onEnter, new System.Action(OnMouseOverToggle));
			component2.onClick = delegate
			{
				SelectCategory(categoryId);
			};
			categoryToggles.Add(categoryId, component2);
			SetCatogoryClickUISound(categoryId, component2);
		}
	}

	public void PopulateGallery()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> galleryGridButton in galleryGridButtons)
		{
			RecycleGalleryGridButton(galleryGridButton.Value.gameObject);
		}
		galleryGridButtons.Clear();
		galleryGridLayouter.ImmediateSizeGridToScreenResolution();
		foreach (PermitResource resource in Db.Get().Permits.resources)
		{
			if (!resource.Id.StartsWith("visonly_"))
			{
				AddItemToGallery(resource);
			}
		}
		subcategories.Sort((KleiInventoryUISubcategory a, KleiInventoryUISubcategory b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[b.subcategoryID].sortKey));
		foreach (KleiInventoryUISubcategory subcategory in subcategories)
		{
			subcategory.gameObject.transform.SetAsLastSibling();
		}
		CollectSubcategoryGridLayouts();
		CloseSubcategory("UNCATEGORIZED");
	}

	private void CloseSubcategory(string subcategoryID)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryID);
		if (kleiInventoryUISubcategory != null)
		{
			kleiInventoryUISubcategory.ToggleOpen(open: false);
		}
	}

	private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryId);
		if (kleiInventoryUISubcategory == null)
		{
			kleiInventoryUISubcategory = Util.KInstantiateUI(subcategoryPrefab, galleryGridContent.gameObject, force_active: true).GetComponent<KleiInventoryUISubcategory>();
			kleiInventoryUISubcategory.subcategoryID = subcategoryId;
			subcategories.Add(kleiInventoryUISubcategory);
			kleiInventoryUISubcategory.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
		}
		itemButton.transform.SetParent(kleiInventoryUISubcategory.gridLayout.transform);
	}

	private void CollectSubcategoryGridLayouts()
	{
		galleryGridLayouter.OnSizeGridComplete = null;
		foreach (KleiInventoryUISubcategory subcategory in subcategories)
		{
			galleryGridLayouter.targetGridLayouts.Add(subcategory.gridLayout);
			GridLayouter gridLayouter = galleryGridLayouter;
			gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete, new System.Action(subcategory.RefreshDisplay));
		}
		galleryGridLayouter.RequestGridResize();
	}

	private void AddItemToGallery(PermitResource permit)
	{
		if (!galleryGridButtons.ContainsKey(permit))
		{
			PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
			GameObject availableGridButton = GetAvailableGridButton();
			AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
			HierarchyReferences component = availableGridButton.GetComponent<HierarchyReferences>();
			Image reference = component.GetReference<Image>("Icon");
			LocText reference2 = component.GetReference<LocText>("OwnedCountLabel");
			Image reference3 = component.GetReference<Image>("IsUnownedOverlay");
			Image reference4 = component.GetReference<Image>("DlcBanner");
			MultiToggle component2 = availableGridButton.GetComponent<MultiToggle>();
			reference.sprite = permitPresentationInfo.sprite;
			if (permit.IsOwnableOnServer())
			{
				int ownedCount = PermitItems.GetOwnedCount(permit);
				reference2.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
				reference2.gameObject.SetActive(ownedCount > 0);
				reference3.gameObject.SetActive(ownedCount <= 0);
			}
			else
			{
				reference2.gameObject.SetActive(value: false);
				reference3.gameObject.SetActive(value: false);
			}
			string dlcIdFrom = permit.GetDlcIdFrom();
			if (DlcManager.IsDlcId(dlcIdFrom))
			{
				reference4.gameObject.SetActive(value: true);
				reference4.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
			}
			else
			{
				reference4.gameObject.SetActive(value: false);
			}
			component2.onEnter = (System.Action)Delegate.Combine(component2.onEnter, new System.Action(OnMouseOverToggle));
			component2.onClick = delegate
			{
				SelectItem(permit);
			};
			galleryGridButtons.Add(permit, component2);
			SetItemClickUISound(permit, component2);
			KleiItemsUI.ConfigureTooltipOn(availableGridButton, KleiItemsUI.GetTooltipStringFor(permit));
		}
	}

	public void SelectCategory(string categoryId)
	{
		if (!InventoryOrganization.categoryIdToIsEmptyMap[categoryId])
		{
			SelectedCategoryId = categoryId;
			galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
			RefreshCategories();
			SelectDefaultCategoryItem();
		}
	}

	private void SelectDefaultCategoryItem()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> galleryGridButton in galleryGridButtons)
		{
			if (InventoryOrganization.categoryIdToSubcategoryIdsMap[SelectedCategoryId].Contains(InventoryOrganization.GetPermitSubcategory(galleryGridButton.Key)))
			{
				SelectItem(galleryGridButton.Key);
				return;
			}
		}
		SelectItem(null);
	}

	public void SelectItem(PermitResource permit)
	{
		SelectedPermit = permit;
		RefreshGallery();
		RefreshDetails();
		RefreshBarterPanel();
	}

	private void RefreshGallery()
	{
		string value = searchField.text.ToUpper();
		foreach (KeyValuePair<PermitResource, MultiToggle> galleryGridButton in galleryGridButtons)
		{
			Util.Deconstruct(galleryGridButton, out var key, out var value2);
			PermitResource permitResource = key;
			MultiToggle multiToggle = value2;
			string permitSubcategory = InventoryOrganization.GetPermitSubcategory(permitResource);
			bool flag = (permitSubcategory == "UNCATEGORIZED" || InventoryOrganization.categoryIdToSubcategoryIdsMap[SelectedCategoryId].Contains(permitSubcategory)) && (permitResource.Name.ToUpper().Contains(value) || permitResource.Id.ToUpper().Contains(value) || permitResource.Description.ToUpper().Contains(value));
			multiToggle.ChangeState((permitResource == SelectedPermit) ? 1 : 0);
			HierarchyReferences component = multiToggle.gameObject.GetComponent<HierarchyReferences>();
			LocText reference = component.GetReference<LocText>("OwnedCountLabel");
			Image reference2 = component.GetReference<Image>("IsUnownedOverlay");
			if (permitResource.IsOwnableOnServer())
			{
				int ownedCount = PermitItems.GetOwnedCount(permitResource);
				reference.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
				reference.gameObject.SetActive(ownedCount > 0);
				reference2.gameObject.SetActive(ownedCount <= 0);
				if (showFilterState == 2 && ownedCount < 2)
				{
					flag = false;
				}
				else if (showFilterState == 1 && ownedCount == 0)
				{
					flag = false;
				}
			}
			else if (!permitResource.IsUnlocked())
			{
				reference.gameObject.SetActive(value: false);
				reference2.gameObject.SetActive(value: true);
				if (showFilterState != 0)
				{
					flag = false;
				}
			}
			else
			{
				reference.gameObject.SetActive(value: false);
				reference2.gameObject.SetActive(value: false);
				if (showFilterState == 2)
				{
					flag = false;
				}
			}
			if (multiToggle.gameObject.activeSelf != flag)
			{
				multiToggle.gameObject.SetActive(flag);
			}
		}
		foreach (KleiInventoryUISubcategory subcategory in subcategories)
		{
			subcategory.RefreshDisplay();
		}
	}

	private void RefreshCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> categoryToggle in categoryToggles)
		{
			categoryToggle.Value.ChangeState((categoryToggle.Key == SelectedCategoryId) ? 1 : 0);
			if (InventoryOrganization.categoryIdToIsEmptyMap[categoryToggle.Key])
			{
				categoryToggle.Value.ChangeState(2);
			}
			else
			{
				categoryToggle.Value.ChangeState((categoryToggle.Key == SelectedCategoryId) ? 1 : 0);
			}
		}
	}

	private void RefreshDetails()
	{
		PermitResource selectedPermit = SelectedPermit;
		PermitPresentationInfo permitPresentationInfo = selectedPermit.GetPermitPresentationInfo();
		permitVis.ConfigureWith(selectedPermit);
		selectionDetailsScrollRect.rectTransform().anchorMin = new Vector2(0f, 0f);
		selectionDetailsScrollRect.rectTransform().anchorMax = new Vector2(1f, 1f);
		selectionDetailsScrollRect.rectTransform().sizeDelta = new Vector2(-24f, 0f);
		selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
		selectionDetailsScrollRect.content.rectTransform().sizeDelta = new Vector2(0f, selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);
		selectionDetailsScrollRectScrollBarContainer.anchorMin = new Vector2(1f, 0f);
		selectionDetailsScrollRectScrollBarContainer.anchorMax = new Vector2(1f, 1f);
		selectionDetailsScrollRectScrollBarContainer.sizeDelta = new Vector2(24f, 0f);
		selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
		selectionHeaderLabel.SetText(selectedPermit.Name);
		selectionNameLabel.SetText(selectedPermit.Name);
		selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
		selectionDescriptionLabel.SetText(selectedPermit.Description);
		selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(permitPresentationInfo.facadeFor));
		selectionFacadeForLabel.SetText(permitPresentationInfo.facadeFor);
		string dlcIdFrom = selectedPermit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			selectionRarityDetailsLabel.gameObject.SetActive(value: false);
			selectionOwnedCount.gameObject.SetActive(value: false);
			selectionCollectionLabel.gameObject.SetActive(value: true);
			if (selectedPermit.Rarity == PermitRarity.UniversalLocked)
			{
				selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
			}
			else
			{
				selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
			}
			return;
		}
		selectionCollectionLabel.gameObject.SetActive(value: false);
		string text = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", selectedPermit.Rarity.GetLocStringName());
		selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
		selectionRarityDetailsLabel.SetText(text);
		selectionOwnedCount.gameObject.SetActive(value: true);
		if (selectedPermit.IsOwnableOnServer())
		{
			int ownedCount = PermitItems.GetOwnedCount(selectedPermit);
			if (ownedCount > 0)
			{
				selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}", ownedCount.ToString()));
			}
			else
			{
				selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			}
		}
		else
		{
			selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
		}
	}

	private PermitPrintabilityState GetPermitPrintabilityState(PermitResource permit)
	{
		if (!IS_ONLINE)
		{
			return PermitPrintabilityState.UserOffline;
		}
		PermitItems.TryGetBarterPrice(SelectedPermit.Id, out var buy_price, out var _);
		if (buy_price == 0L)
		{
			if (permit.Rarity == PermitRarity.Universal || permit.Rarity == PermitRarity.UniversalLocked || permit.Rarity == PermitRarity.Loyalty || permit.Rarity == PermitRarity.Unknown)
			{
				return PermitPrintabilityState.NotForSale;
			}
			return PermitPrintabilityState.NotForSaleYet;
		}
		if (PermitItems.GetOwnedCount(permit) > 0)
		{
			return PermitPrintabilityState.AlreadyOwned;
		}
		if (KleiItems.GetFilamentAmount() < buy_price)
		{
			return PermitPrintabilityState.TooExpensive;
		}
		return PermitPrintabilityState.Printable;
	}

	private void RefreshBarterPanel()
	{
		barterBuyButton.ClearOnClick();
		barterSellButton.ClearOnClick();
		barterBuyButton.isInteractable = IS_ONLINE;
		barterSellButton.isInteractable = IS_ONLINE;
		HierarchyReferences component = barterBuyButton.GetComponent<HierarchyReferences>();
		HierarchyReferences component2 = barterSellButton.GetComponent<HierarchyReferences>();
		new Color(1f, 59f / 85f, 59f / 85f);
		Color color = new Color(0.6f, 81f / 85f, 0.5019608f);
		LocText reference = component.GetReference<LocText>("CostLabel");
		LocText reference2 = component2.GetReference<LocText>("CostLabel");
		barterPanelBG.color = (IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F"));
		filamentWalletSection.gameObject.SetActive(IS_ONLINE);
		barterOfflineLabel.gameObject.SetActive(!IS_ONLINE);
		ulong filamentAmount = KleiItems.GetFilamentAmount();
		filamentWalletSection.GetComponent<ToolTip>().SetSimpleTooltip((filamentAmount > 1) ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_PLURAL_TOOLTIP, filamentAmount) : string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP, filamentAmount));
		PermitPrintabilityState permitPrintabilityState = GetPermitPrintabilityState(SelectedPermit);
		if (IS_ONLINE)
		{
			PermitItems.TryGetBarterPrice(SelectedPermit.Id, out var buy_price, out var sell_price);
			filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
			switch (permitPrintabilityState)
			{
			case PermitPrintabilityState.NotForSale:
				barterBuyButton.isInteractable = false;
				barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);
				reference.SetText("");
				break;
			case PermitPrintabilityState.NotForSaleYet:
				barterBuyButton.isInteractable = false;
				barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_BETA);
				reference.SetText("");
				break;
			case PermitPrintabilityState.AlreadyOwned:
				barterBuyButton.isInteractable = false;
				barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_ALREADY_OWNED);
				reference.SetText("-" + buy_price);
				break;
			case PermitPrintabilityState.TooExpensive:
				barterBuyButton.isInteractable = false;
				barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);
				reference.SetText("-" + buy_price);
				break;
			case PermitPrintabilityState.Printable:
				barterBuyButton.isInteractable = true;
				barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE, buy_price.ToString()));
				reference.SetText("-" + buy_price);
				barterBuyButton.onClick += delegate
				{
					GameObject obj2 = Util.KInstantiateUI(barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject);
					obj2.rectTransform().sizeDelta = Vector2.zero;
					obj2.GetComponent<BarterConfirmationScreen>().Present(SelectedPermit, isPurchase: true);
				};
				break;
			}
			if (sell_price == 0L)
			{
				barterSellButton.isInteractable = false;
				barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);
				reference2.SetText("");
				reference2.color = Color.white;
				return;
			}
			bool flag = PermitItems.GetOwnedCount(SelectedPermit) > 0;
			barterSellButton.isInteractable = flag;
			barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE, sell_price.ToString()) : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);
			if (flag)
			{
				reference2.color = color;
				reference2.SetText("+" + sell_price);
			}
			else
			{
				reference2.color = Color.white;
				reference2.SetText("+" + sell_price);
			}
			barterSellButton.onClick += delegate
			{
				GameObject obj = Util.KInstantiateUI(barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject);
				obj.rectTransform().sizeDelta = Vector2.zero;
				obj.GetComponent<BarterConfirmationScreen>().Present(SelectedPermit, isPurchase: false);
			};
		}
		else
		{
			component.GetReference<LocText>("CostLabel").SetText("");
			reference2.SetText("");
			reference2.color = Color.white;
			barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
		}
	}

	private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle)
	{
		if (!categoryToggles.ContainsKey(categoryID))
		{
			toggle.states[1].on_click_override_sound_path = "";
			toggle.states[0].on_click_override_sound_path = "";
		}
		else
		{
			toggle.states[1].on_click_override_sound_path = "General_Category_Click";
			toggle.states[0].on_click_override_sound_path = "General_Category_Click";
		}
	}

	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		string facadeItemSoundName = GetFacadeItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	public static string GetFacadeItemSoundName(PermitResource permit)
	{
		if (permit == null)
		{
			return "HUD";
		}
		switch (permit.Category)
		{
		case PermitCategory.DupeTops:
			return "tops";
		case PermitCategory.DupeBottoms:
			return "bottoms";
		case PermitCategory.DupeGloves:
			return "gloves";
		case PermitCategory.DupeShoes:
			return "shoes";
		case PermitCategory.DupeHats:
			return "hats";
		case PermitCategory.AtmoSuitBelt:
			return "belt";
		case PermitCategory.AtmoSuitHelmet:
			return "atmosuit_helmet";
		case PermitCategory.AtmoSuitBody:
			return "tops";
		case PermitCategory.AtmoSuitShoes:
			return "shoes";
		case PermitCategory.AtmoSuitGloves:
			return "gloves";
		default:
			if (permit.Category == PermitCategory.Building)
			{
				BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
				if (buildingDef2 == null)
				{
					return "HUD";
				}
				switch (buildingDef2.PrefabID)
				{
				case "ExteriorWall":
					return "wall";
				case "FlowerVase":
				case "FlowerVaseWall":
				case "FlowerVaseHanging":
				case "FlowerVaseHangingFancy":
					return "flowervase";
				case "Bed":
					return "bed";
				case "LuxuryBed":
					return permit.Id switch
					{
						"LuxuryBed_boat" => "elegantbed_boat", 
						"LuxuryBed_bouncy" => "elegantbed_bouncy", 
						_ => "elegantbed", 
					};
				case "CeilingLight":
					return "ceilingLight";
				case "EggCracker":
					return "eggcracker";
				case "RockCrusher":
					return "rockrefinery";
				case "StorageLocker":
					return "storagelocker";
				case "PlanterBox":
					return "planterbox";
				case "GasReservoir":
					return "gasstorage";
				case "WaterCooler":
					return "watercooler";
				case "MassageTable":
					return "massagetable";
				case "FlushToilet":
					return "flushtoilate";
				}
			}
			if (permit.Category == PermitCategory.Artwork)
			{
				BuildingDef buildingDef3 = KleiPermitVisUtil.GetBuildingDef(permit);
				if (buildingDef3 == null)
				{
					return "HUD";
				}
				_ = (ArtableStage)permit;
				if (Has<Sculpture>(buildingDef3))
				{
					return buildingDef3.PrefabID switch
					{
						"IceSculpture" => "icesculpture", 
						"WoodSculpture" => "woodsculpture", 
						_ => "sculpture", 
					};
				}
				if (Has<Painting>(buildingDef3))
				{
					return "painting";
				}
			}
			if (permit.Category == PermitCategory.JoyResponse && permit is BalloonArtistFacadeResource)
			{
				return "balloon";
			}
			return "HUD";
		}
		static bool Has<T>(BuildingDef buildingDef) where T : Component
		{
			return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
		}
	}

	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));
	}
}
