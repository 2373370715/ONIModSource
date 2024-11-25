using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KleiInventoryScreen : KModalScreen {
    public const int FILTER_SHOW_ALL          = 0;
    public const int FILTER_SHOW_OWNED_ONLY   = 1;
    public const int FILTER_SHOW_DOUBLES_ONLY = 2;

    [SerializeField]
    private KButton barterBuyButton;

    [SerializeField]
    private GameObject barterConfirmationScreenPrefab;

    [SerializeField]
    private GameObject barterOfflineLabel;

    [Header("BarterSection"), SerializeField]
    private Image barterPanelBG;

    [SerializeField]
    private KButton barterSellButton;

    [Header("CategoryColumn"), SerializeField]
    private RectTransform categoryListContent;

    [SerializeField]
    private GameObject categoryRowPrefab;

    private readonly Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

    [SerializeField]
    private KButton clearSearchButton;

    [Header("Header"), SerializeField]
    private KButton closeButton;

    [SerializeField]
    private MultiToggle doublesOnlyToggle;

    [SerializeField]
    private GameObject filamentWalletSection;

    private readonly Dictionary<PermitResource, MultiToggle> galleryGridButtons
        = new Dictionary<PermitResource, MultiToggle>();

    [SerializeField]
    private RectTransform galleryGridContent;

    private GridLayouter galleryGridLayouter;

    [Header("ItemGalleryColumn"), SerializeField]
    private LocText galleryHeaderLabel;

    [SerializeField]
    private GameObject gridItemPrefab;

    private bool initConfigComplete;
    private bool IS_ONLINE;

    [SerializeField]
    private GameObject itemDummyPrefab;

    [SerializeField]
    private KleiPermitDioramaVis permitVis;

    private readonly List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

    [Header("GalleryFilters"), SerializeField]
    private KInputTextField searchField;

    [SerializeField]
    private LocText selectionCollectionLabel;

    [SerializeField]
    private LocText selectionDescriptionLabel;

    [SerializeField]
    private KScrollRect selectionDetailsScrollRect;

    [SerializeField]
    private RectTransform selectionDetailsScrollRectScrollBarContainer;

    [SerializeField]
    private LocText selectionFacadeForLabel;

    [Header("SelectionDetailsColumn"), SerializeField]
    private LocText selectionHeaderLabel;

    [SerializeField]
    private LocText selectionNameLabel;

    [SerializeField]
    private LocText selectionOwnedCount;

    [SerializeField]
    private LocText selectionRarityDetailsLabel;

    private          int                              showFilterState;
    private readonly List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

    [SerializeField]
    private GameObject subcategoryPrefab;

    private PermitResource SelectedPermit     { get; set; }
    private string         SelectedCategoryId { get; set; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        closeButton.onClick += delegate { Show(false); };
        ConsumeMouseScroll  =  true;
        galleryGridLayouter = new GridLayouter {
            minCellSize = 64f, maxCellSize = 96f, targetGridLayouts = new List<GridLayoutGroup>()
        };

        galleryGridLayouter.overrideParentForSizeReference = galleryGridContent;
        InventoryOrganization.Initialize();
    }

    public override void OnKeyDown(KButtonEvent e) {
        if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight)) Show(false);
        base.OnKeyDown(e);
    }

    public override    float GetSortKey() { return 20f; }
    protected override void  OnActivate() { OnShow(true); }

    protected override void OnShow(bool show) {
        base.OnShow(show);
        if (show) {
            InitConfig();
            ToggleDoublesOnly(0);
            ClearSearch();
        }
    }

    private void ToggleDoublesOnly(int newState) {
        showFilterState = newState;
        doublesOnlyToggle.ChangeState(showFilterState);
        doublesOnlyToggle.GetComponentInChildren<LocText>().text = showFilterState + "+";
        var simpleTooltip = "";
        switch (showFilterState) {
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

        var component = doublesOnlyToggle.GetComponent<ToolTip>();
        component.SetSimpleTooltip(simpleTooltip);
        component.refreshWhileHovering = true;
        component.forceRefresh         = true;
        RefreshGallery();
    }

    private void InitConfig() {
        if (initConfigComplete) return;

        initConfigComplete = true;
        galleryGridLayouter.RequestGridResize();
        categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        PopulateCategories();
        PopulateGallery();
        SelectCategory("BUILDINGS");
        searchField.onValueChanged.RemoveAllListeners();
        searchField.onValueChanged.AddListener(delegate { RefreshGallery(); });
        clearSearchButton.ClearOnClick();
        clearSearchButton.onClick += ClearSearch;
        var multiToggle = doublesOnlyToggle;
        multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick,
                                                              new System.Action(delegate {
                                                                                    var newState
                                                                                        = (showFilterState + 1) % 3;

                                                                                    ToggleDoublesOnly(newState);
                                                                                }));
    }

    protected override void OnCmpEnable() {
        base.OnCmpEnable();
        ToggleDoublesOnly(0);
        ClearSearch();
        if (!initConfigComplete) InitConfig();
        RefreshUI();
        KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate { RefreshUI(); });
    }

    private void ClearSearch() {
        searchField.text                                             = "";
        searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
        RefreshGallery();
    }

    private void Update() { galleryGridLayouter.CheckIfShouldResizeGrid(); }

    private void RefreshUI() {
        IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
        RefreshCategories();
        RefreshGallery();
        if (SelectedCategoryId.IsNullOrWhiteSpace()) SelectCategory("BUILDINGS");
        RefreshDetails();
        RefreshBarterPanel();
    }

    private GameObject GetAvailableGridButton() {
        if (recycledGalleryGridButtons.Count == 0)
            return Util.KInstantiateUI(gridItemPrefab, galleryGridContent.gameObject, true);

        var result = recycledGalleryGridButtons[0];
        recycledGalleryGridButtons.RemoveAt(0);
        return result;
    }

    private void RecycleGalleryGridButton(GameObject button) {
        button.GetComponent<MultiToggle>().onClick = null;
        recycledGalleryGridButtons.Add(button);
    }

    public void PopulateCategories() {
        foreach (var keyValuePair in categoryToggles) Destroy(keyValuePair.Value.gameObject);
        categoryToggles.Clear();
        foreach (var keyValuePair2 in InventoryOrganization.categoryIdToSubcategoryIdsMap) {
            string       categoryId2;
            List<string> list;
            keyValuePair2.Deconstruct(out categoryId2, out list);
            var categoryId = categoryId2;
            var gameObject = Util.KInstantiateUI(categoryRowPrefab, categoryListContent.gameObject, true);
            var component  = gameObject.GetComponent<HierarchyReferences>();
            component.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
            component.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
            var component2  = gameObject.GetComponent<MultiToggle>();
            var multiToggle = component2;
            multiToggle.onEnter
                = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));

            component2.onClick = delegate { SelectCategory(categoryId); };
            categoryToggles.Add(categoryId, component2);
            SetCatogoryClickUISound(categoryId, component2);
        }
    }

    public void PopulateGallery() {
        foreach (var keyValuePair in galleryGridButtons) RecycleGalleryGridButton(keyValuePair.Value.gameObject);
        galleryGridButtons.Clear();
        galleryGridLayouter.ImmediateSizeGridToScreenResolution();
        foreach (var permitResource in Db.Get().Permits.resources)
            if (!permitResource.Id.StartsWith("visonly_"))
                AddItemToGallery(permitResource);

        subcategories.Sort((a, b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID]
                                                          .sortKey.CompareTo(InventoryOrganization
                                                                             .subcategoryIdToPresentationDataMap
                                                                                 [b.subcategoryID].sortKey));

        foreach (var kleiInventoryUISubcategory in subcategories)
            kleiInventoryUISubcategory.gameObject.transform.SetAsLastSibling();

        CollectSubcategoryGridLayouts();
        CloseSubcategory("UNCATEGORIZED");
    }

    private void CloseSubcategory(string subcategoryID) {
        var kleiInventoryUISubcategory = subcategories.Find(match => match.subcategoryID == subcategoryID);
        if (kleiInventoryUISubcategory != null) kleiInventoryUISubcategory.ToggleOpen(false);
    }

    private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId) {
        var kleiInventoryUISubcategory = subcategories.Find(match => match.subcategoryID == subcategoryId);
        if (kleiInventoryUISubcategory == null) {
            kleiInventoryUISubcategory = Util.KInstantiateUI(subcategoryPrefab, galleryGridContent.gameObject, true)
                                             .GetComponent<KleiInventoryUISubcategory>();

            kleiInventoryUISubcategory.subcategoryID = subcategoryId;
            subcategories.Add(kleiInventoryUISubcategory);
            kleiInventoryUISubcategory.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId),
                                                   InventoryOrganization
                                                       .subcategoryIdToPresentationDataMap[subcategoryId].icon);
        }

        itemButton.transform.SetParent(kleiInventoryUISubcategory.gridLayout.transform);
    }

    private void CollectSubcategoryGridLayouts() {
        galleryGridLayouter.OnSizeGridComplete = null;
        foreach (var kleiInventoryUISubcategory in subcategories) {
            galleryGridLayouter.targetGridLayouts.Add(kleiInventoryUISubcategory.gridLayout);
            var gridLayouter = galleryGridLayouter;
            gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete,
                                                                              new
                                                                                  System.
                                                                                  Action(kleiInventoryUISubcategory
                                                                                      .RefreshDisplay));
        }

        galleryGridLayouter.RequestGridResize();
    }

    private void AddItemToGallery(PermitResource permit) {
        if (galleryGridButtons.ContainsKey(permit)) return;

        var permitPresentationInfo = permit.GetPermitPresentationInfo();
        var availableGridButton    = GetAvailableGridButton();
        AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
        var component  = availableGridButton.GetComponent<HierarchyReferences>();
        var reference  = component.GetReference<Image>("Icon");
        var reference2 = component.GetReference<LocText>("OwnedCountLabel");
        var reference3 = component.GetReference<Image>("IsUnownedOverlay");
        var reference4 = component.GetReference<Image>("DlcBanner");
        var component2 = availableGridButton.GetComponent<MultiToggle>();
        reference.sprite = permitPresentationInfo.sprite;
        if (permit.IsOwnableOnServer()) {
            var ownedCount = PermitItems.GetOwnedCount(permit);
            reference2.text
                = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());

            reference2.gameObject.SetActive(ownedCount > 0);
            reference3.gameObject.SetActive(ownedCount <= 0);
        } else {
            reference2.gameObject.SetActive(false);
            reference3.gameObject.SetActive(false);
        }

        var dlcIdFrom = permit.GetDlcIdFrom();
        if (DlcManager.IsDlcId(dlcIdFrom)) {
            reference4.gameObject.SetActive(true);
            reference4.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
        } else
            reference4.gameObject.SetActive(false);

        var multiToggle = component2;
        multiToggle.onEnter
            = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));

        component2.onClick = delegate { SelectItem(permit); };
        galleryGridButtons.Add(permit, component2);
        SetItemClickUISound(permit, component2);
        KleiItemsUI.ConfigureTooltipOn(availableGridButton, KleiItemsUI.GetTooltipStringFor(permit));
    }

    public void SelectCategory(string categoryId) {
        if (InventoryOrganization.categoryIdToIsEmptyMap[categoryId]) return;

        SelectedCategoryId = categoryId;
        galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
        RefreshCategories();
        SelectDefaultCategoryItem();
    }

    private void SelectDefaultCategoryItem() {
        foreach (var keyValuePair in galleryGridButtons)
            if (InventoryOrganization.categoryIdToSubcategoryIdsMap[SelectedCategoryId]
                                     .Contains(InventoryOrganization.GetPermitSubcategory(keyValuePair.Key))) {
                SelectItem(keyValuePair.Key);
                return;
            }

        SelectItem(null);
    }

    public void SelectItem(PermitResource permit) {
        SelectedPermit = permit;
        RefreshGallery();
        RefreshDetails();
        RefreshBarterPanel();
    }

    private void RefreshGallery() {
        var value = searchField.text.ToUpper();
        foreach (var keyValuePair in galleryGridButtons) {
            PermitResource permitResource;
            MultiToggle    multiToggle;
            keyValuePair.Deconstruct(out permitResource, out multiToggle);
            var permitResource2   = permitResource;
            var multiToggle2      = multiToggle;
            var permitSubcategory = InventoryOrganization.GetPermitSubcategory(permitResource2);
            var flag = permitSubcategory == "UNCATEGORIZED" ||
                       InventoryOrganization.categoryIdToSubcategoryIdsMap[SelectedCategoryId]
                                            .Contains(permitSubcategory);

            flag = flag &&
                   (permitResource2.Name.ToUpper().Contains(value) ||
                    permitResource2.Id.ToUpper().Contains(value)   ||
                    permitResource2.Description.ToUpper().Contains(value));

            multiToggle2.ChangeState(permitResource2 == SelectedPermit ? 1 : 0);
            var component  = multiToggle2.gameObject.GetComponent<HierarchyReferences>();
            var reference  = component.GetReference<LocText>("OwnedCountLabel");
            var reference2 = component.GetReference<Image>("IsUnownedOverlay");
            if (permitResource2.IsOwnableOnServer()) {
                var ownedCount = PermitItems.GetOwnedCount(permitResource2);
                reference.text
                    = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}",
                     ownedCount.ToString());

                reference.gameObject.SetActive(ownedCount  > 0);
                reference2.gameObject.SetActive(ownedCount <= 0);
                if (showFilterState == 2 && ownedCount < 2)
                    flag                                               = false;
                else if (showFilterState == 1 && ownedCount == 0) flag = false;
            } else if (!permitResource2.IsUnlocked()) {
                reference.gameObject.SetActive(false);
                reference2.gameObject.SetActive(true);
                if (showFilterState != 0) flag = false;
            } else {
                reference.gameObject.SetActive(false);
                reference2.gameObject.SetActive(false);
                if (showFilterState == 2) flag = false;
            }

            if (multiToggle2.gameObject.activeSelf != flag) multiToggle2.gameObject.SetActive(flag);
        }

        foreach (var kleiInventoryUISubcategory in subcategories) kleiInventoryUISubcategory.RefreshDisplay();
    }

    private void RefreshCategories() {
        foreach (var keyValuePair in categoryToggles) {
            keyValuePair.Value.ChangeState(keyValuePair.Key == SelectedCategoryId ? 1 : 0);
            if (InventoryOrganization.categoryIdToIsEmptyMap[keyValuePair.Key])
                keyValuePair.Value.ChangeState(2);
            else
                keyValuePair.Value.ChangeState(keyValuePair.Key == SelectedCategoryId ? 1 : 0);
        }
    }

    private void RefreshDetails() {
        var selectedPermit         = SelectedPermit;
        var permitPresentationInfo = selectedPermit.GetPermitPresentationInfo();
        permitVis.ConfigureWith(selectedPermit);
        selectionDetailsScrollRect.rectTransform().anchorMin        = new Vector2(0f,   0f);
        selectionDetailsScrollRect.rectTransform().anchorMax        = new Vector2(1f,   1f);
        selectionDetailsScrollRect.rectTransform().sizeDelta        = new Vector2(-24f, 0f);
        selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
        selectionDetailsScrollRect.content.rectTransform().sizeDelta
            = new Vector2(0f, selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);

        selectionDetailsScrollRectScrollBarContainer.anchorMin        = new Vector2(1f,  0f);
        selectionDetailsScrollRectScrollBarContainer.anchorMax        = new Vector2(1f,  1f);
        selectionDetailsScrollRectScrollBarContainer.sizeDelta        = new Vector2(24f, 0f);
        selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
        selectionHeaderLabel.SetText(selectedPermit.Name);
        selectionNameLabel.SetText(selectedPermit.Name);
        selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
        selectionDescriptionLabel.SetText(selectedPermit.Description);
        selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(permitPresentationInfo.facadeFor));
        selectionFacadeForLabel.SetText(permitPresentationInfo.facadeFor);
        var dlcIdFrom = selectedPermit.GetDlcIdFrom();
        if (DlcManager.IsDlcId(dlcIdFrom)) {
            selectionRarityDetailsLabel.gameObject.SetActive(false);
            selectionOwnedCount.gameObject.SetActive(false);
            selectionCollectionLabel.gameObject.SetActive(true);
            if (selectedPermit.Rarity == PermitRarity.UniversalLocked) {
                selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}",
                                                  DlcManager.GetDlcTitle(dlcIdFrom)));

                return;
            }

            selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}",
                                              DlcManager.GetDlcTitle(dlcIdFrom)));

            return;
        }

        selectionCollectionLabel.gameObject.SetActive(false);
        var text = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}",
                                                                        selectedPermit.Rarity.GetLocStringName());

        selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
        selectionRarityDetailsLabel.SetText(text);
        selectionOwnedCount.gameObject.SetActive(true);
        if (!selectedPermit.IsOwnableOnServer()) {
            selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
            return;
        }

        var ownedCount = PermitItems.GetOwnedCount(selectedPermit);
        if (ownedCount > 0) {
            selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}",
                                         ownedCount.ToString()));

            return;
        }

        selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE,
                                                              KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
    }

    private PermitPrintabilityState GetPermitPrintabilityState(PermitResource permit) {
        if (!IS_ONLINE) return PermitPrintabilityState.UserOffline;

        ulong num;
        ulong num2;
        PermitItems.TryGetBarterPrice(SelectedPermit.Id, out num, out num2);
        if (num == 0UL) {
            if (permit.Rarity == PermitRarity.Universal       ||
                permit.Rarity == PermitRarity.UniversalLocked ||
                permit.Rarity == PermitRarity.Loyalty         ||
                permit.Rarity == PermitRarity.Unknown)
                return PermitPrintabilityState.NotForSale;

            return PermitPrintabilityState.NotForSaleYet;
        }

        if (PermitItems.GetOwnedCount(permit) > 0) return PermitPrintabilityState.AlreadyOwned;

        if (KleiItems.GetFilamentAmount() < num) return PermitPrintabilityState.TooExpensive;

        return PermitPrintabilityState.Printable;
    }

    private void RefreshBarterPanel() {
        barterBuyButton.ClearOnClick();
        barterSellButton.ClearOnClick();
        barterBuyButton.isInteractable  = IS_ONLINE;
        barterSellButton.isInteractable = IS_ONLINE;
        var component  = barterBuyButton.GetComponent<HierarchyReferences>();
        var component2 = barterSellButton.GetComponent<HierarchyReferences>();
        new Color(1f, 0.69411767f, 0.69411767f);
        var color      = new Color(0.6f, 0.9529412f, 0.5019608f);
        var reference  = component.GetReference<LocText>("CostLabel");
        var reference2 = component2.GetReference<LocText>("CostLabel");
        barterPanelBG.color = IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F");
        filamentWalletSection.gameObject.SetActive(IS_ONLINE);
        barterOfflineLabel.gameObject.SetActive(!IS_ONLINE);
        var filamentAmount = KleiItems.GetFilamentAmount();
        filamentWalletSection.GetComponent<ToolTip>()
                             .SetSimpleTooltip(filamentAmount > 1UL
                                                   ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING
                                                                     .WALLET_PLURAL_TOOLTIP,
                                                                   filamentAmount)
                                                   : string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP,
                                                                   filamentAmount));

        var permitPrintabilityState = GetPermitPrintabilityState(SelectedPermit);
        if (!IS_ONLINE) {
            component.GetReference<LocText>("CostLabel").SetText("");
            reference2.SetText("");
            reference2.color = Color.white;
            barterBuyButton.GetComponent<ToolTip>()
                           .SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);

            barterSellButton.GetComponent<ToolTip>()
                            .SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);

            return;
        }

        ulong num;
        ulong num2;
        PermitItems.TryGetBarterPrice(SelectedPermit.Id, out num, out num2);
        filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
        switch (permitPrintabilityState) {
            case PermitPrintabilityState.Printable:
                barterBuyButton.isInteractable = true;
                barterBuyButton.GetComponent<ToolTip>()
                               .SetSimpleTooltip(string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE,
                                                               num.ToString()));

                reference.SetText("-" + num);
                barterBuyButton.onClick += delegate {
                                               var gameObject
                                                   = Util.KInstantiateUI(barterConfirmationScreenPrefab,
                                                                         LockerNavigator.Instance.gameObject);

                                               gameObject.rectTransform().sizeDelta = Vector2.zero;
                                               gameObject.GetComponent<BarterConfirmationScreen>()
                                                         .Present(SelectedPermit, true);
                                           };

                break;
            case PermitPrintabilityState.AlreadyOwned:
                barterBuyButton.isInteractable = false;
                barterBuyButton.GetComponent<ToolTip>()
                               .SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_ALREADY_OWNED);

                reference.SetText("-" + num);
                break;
            case PermitPrintabilityState.TooExpensive:
                barterBuyButton.isInteractable = false;
                barterBuyButton.GetComponent<ToolTip>()
                               .SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);

                reference.SetText("-" + num);
                break;
            case PermitPrintabilityState.NotForSale:
                barterBuyButton.isInteractable = false;
                barterBuyButton.GetComponent<ToolTip>()
                               .SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);

                reference.SetText("");
                break;
            case PermitPrintabilityState.NotForSaleYet:
                barterBuyButton.isInteractable = false;
                barterBuyButton.GetComponent<ToolTip>()
                               .SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_BETA);

                reference.SetText("");
                break;
        }

        if (num2 == 0UL) {
            barterSellButton.isInteractable = false;
            barterSellButton.GetComponent<ToolTip>()
                            .SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);

            reference2.SetText("");
            reference2.color = Color.white;
            return;
        }

        var flag = PermitItems.GetOwnedCount(SelectedPermit) > 0;
        barterSellButton.isInteractable = flag;
        barterSellButton.GetComponent<ToolTip>()
                        .SetSimpleTooltip(flag
                                              ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE,
                                                              num2.ToString())
                                              : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);

        if (flag) {
            reference2.color = color;
            reference2.SetText("+" + num2);
        } else {
            reference2.color = Color.white;
            reference2.SetText("+" + num2);
        }

        barterSellButton.onClick += delegate {
                                        var gameObject
                                            = Util.KInstantiateUI(barterConfirmationScreenPrefab,
                                                                  LockerNavigator.Instance.gameObject);

                                        gameObject.rectTransform().sizeDelta = Vector2.zero;
                                        gameObject.GetComponent<BarterConfirmationScreen>()
                                                  .Present(SelectedPermit, false);
                                    };
    }

    private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle) {
        if (!categoryToggles.ContainsKey(categoryID)) {
            toggle.states[1].on_click_override_sound_path = "";
            toggle.states[0].on_click_override_sound_path = "";
            return;
        }

        toggle.states[1].on_click_override_sound_path = "General_Category_Click";
        toggle.states[0].on_click_override_sound_path = "General_Category_Click";
    }

    private void SetItemClickUISound(PermitResource permit, MultiToggle toggle) {
        var facadeItemSoundName = GetFacadeItemSoundName(permit);
        toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
        toggle.states[1].sound_parameter_name         = "Unlocked";
        toggle.states[1].sound_parameter_value        = permit.IsUnlocked() ? 1f : 0f;
        toggle.states[1].has_sound_parameter          = true;
        toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
        toggle.states[0].sound_parameter_name         = "Unlocked";
        toggle.states[0].sound_parameter_value        = permit.IsUnlocked() ? 1f : 0f;
        toggle.states[0].has_sound_parameter          = true;
    }

    public static string GetFacadeItemSoundName(PermitResource permit) {
        if (permit == null) return "HUD";

        switch (permit.Category) {
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
            case PermitCategory.AtmoSuitHelmet:
                return "atmosuit_helmet";
            case PermitCategory.AtmoSuitBody:
                return "tops";
            case PermitCategory.AtmoSuitGloves:
                return "gloves";
            case PermitCategory.AtmoSuitBelt:
                return "belt";
            case PermitCategory.AtmoSuitShoes:
                return "shoes";
        }

        if (permit.Category == PermitCategory.Building) {
            var buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
            if (buildingDef == null) return "HUD";

            var  prefabID = buildingDef.PrefabID;
            uint num      = <PrivateImplementationDetails >.ComputeStringHash(prefabID);
            if (num <= 2076384603U) {
                if (num <= 1633134164U) {
                    if (num <= 595816591U) {
                        if (num != 228062815U) {
                            if (num != 595816591U) goto IL_38D;

                            if (!(prefabID == "FlowerVase")) goto IL_38D;
                        } else {
                            if (!(prefabID == "LuxuryBed")) goto IL_38D;

                            var id = permit.Id;
                            if (id == "LuxuryBed_boat") return "elegantbed_boat";

                            if (!(id == "LuxuryBed_bouncy")) return "elegantbed";

                            return "elegantbed_bouncy";
                        }
                    } else if (num != 1607642960U) {
                        if (num != 1633134164U) goto IL_38D;

                        if (!(prefabID == "CeilingLight")) goto IL_38D;

                        return "ceilingLight";
                    } else {
                        if (!(prefabID == "FlushToilet")) goto IL_38D;

                        return "flushtoilate";
                    }
                } else if (num <= 1943253450U) {
                    if (num != 1734850496U) {
                        if (num != 1943253450U) goto IL_38D;

                        if (!(prefabID == "WaterCooler")) goto IL_38D;

                        return "watercooler";
                    }

                    if (!(prefabID == "RockCrusher")) goto IL_38D;

                    return "rockrefinery";
                } else if (num != 2028863301U) {
                    if (num != 2076384603U) goto IL_38D;

                    if (!(prefabID == "GasReservoir")) goto IL_38D;

                    return "gasstorage";
                } else if (!(prefabID == "FlowerVaseHanging")) goto IL_38D;
            } else if (num <= 3048425356U) {
                if (num <= 2722382738U) {
                    if (num != 2402859370U) {
                        if (num != 2722382738U) goto IL_38D;

                        if (!(prefabID == "PlanterBox")) goto IL_38D;

                        return "planterbox";
                    }

                    if (!(prefabID == "StorageLocker")) goto IL_38D;

                    return "storagelocker";
                }

                if (num != 2899744071U) {
                    if (num != 3048425356U) goto IL_38D;

                    if (!(prefabID == "Bed")) goto IL_38D;

                    return "bed";
                }

                if (!(prefabID == "ExteriorWall")) goto IL_38D;

                return "wall";
            } else if (num <= 3534553076U) {
                if (num != 3132083755U) {
                    if (num != 3534553076U) goto IL_38D;

                    if (!(prefabID == "MassageTable")) goto IL_38D;

                    return "massagetable";
                }

                if (!(prefabID == "FlowerVaseWall")) goto IL_38D;
            } else if (num != 3903452895U) {
                if (num != 3958671086U) goto IL_38D;

                if (!(prefabID == "FlowerVaseHangingFancy")) goto IL_38D;
            } else {
                if (!(prefabID == "EggCracker")) goto IL_38D;

                return "eggcracker";
            }

            return "flowervase";
        }

        IL_38D:
        if (permit.Category == PermitCategory.Artwork) {
            var buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
            if (buildingDef2 == null) return "HUD";

            var artableStage = (ArtableStage)permit;
            if (KleiInventoryScreen.<(GetFacadeItemSoundName > g__Has) | (76_0 < Sculpture > buildingDef2))
            {
                var prefabID = buildingDef2.PrefabID;
                if (prefabID == "IceSculpture") return "icesculpture";

                if (!(prefabID == "WoodSculpture")) return "sculpture";

                return "woodsculpture";
            }

            else if (KleiInventoryScreen.<(GetFacadeItemSoundName > g__Has) | (76_0 < Painting > buildingDef2))
            {
                return "painting";
            }
        }

        if (permit.Category == PermitCategory.JoyResponse && permit is BalloonArtistFacadeResource) return "balloon";

        return "HUD";
    }

    private void OnMouseOverToggle() { KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover")); }

    [CompilerGenerated]
    internal static bool<GetFacadeItemSoundName>g__Has|76_0<T>
    private (BuildingDef buildingDef) where T:Component
    {
        return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
    }

    private enum PermitPrintabilityState {
        Printable,
        AlreadyOwned,
        TooExpensive,
        NotForSale,
        NotForSaleYet,
        UserOffline
    }

    private enum MultiToggleState {
        Default,
        Selected,
        NonInteractable
    }
}