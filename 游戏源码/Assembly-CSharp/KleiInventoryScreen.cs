using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D38 RID: 7480
public class KleiInventoryScreen : KModalScreen
{
	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x06009C23 RID: 39971 RVA: 0x001058F7 File Offset: 0x00103AF7
	// (set) Token: 0x06009C24 RID: 39972 RVA: 0x001058FF File Offset: 0x00103AFF
	private PermitResource SelectedPermit { get; set; }

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x06009C25 RID: 39973 RVA: 0x00105908 File Offset: 0x00103B08
	// (set) Token: 0x06009C26 RID: 39974 RVA: 0x00105910 File Offset: 0x00103B10
	private string SelectedCategoryId { get; set; }

	// Token: 0x06009C27 RID: 39975 RVA: 0x003C2050 File Offset: 0x003C0250
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		base.ConsumeMouseScroll = true;
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = new List<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		InventoryOrganization.Initialize();
	}

	// Token: 0x06009C28 RID: 39976 RVA: 0x00105919 File Offset: 0x00103B19
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009C29 RID: 39977 RVA: 0x000CAB45 File Offset: 0x000C8D45
	public override float GetSortKey()
	{
		return 20f;
	}

	// Token: 0x06009C2A RID: 39978 RVA: 0x00105768 File Offset: 0x00103968
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06009C2B RID: 39979 RVA: 0x0010593B File Offset: 0x00103B3B
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.InitConfig();
			this.ToggleDoublesOnly(0);
			this.ClearSearch();
		}
	}

	// Token: 0x06009C2C RID: 39980 RVA: 0x003C20C4 File Offset: 0x003C02C4
	private void ToggleDoublesOnly(int newState)
	{
		this.showFilterState = newState;
		this.doublesOnlyToggle.ChangeState(this.showFilterState);
		this.doublesOnlyToggle.GetComponentInChildren<LocText>().text = this.showFilterState.ToString() + "+";
		string simpleTooltip = "";
		switch (this.showFilterState)
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
		ToolTip component = this.doublesOnlyToggle.GetComponent<ToolTip>();
		component.SetSimpleTooltip(simpleTooltip);
		component.refreshWhileHovering = true;
		component.forceRefresh = true;
		this.RefreshGallery();
	}

	// Token: 0x06009C2D RID: 39981 RVA: 0x003C217C File Offset: 0x003C037C
	private void InitConfig()
	{
		if (this.initConfigComplete)
		{
			return;
		}
		this.initConfigComplete = true;
		this.galleryGridLayouter.RequestGridResize();
		this.categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		this.PopulateCategories();
		this.PopulateGallery();
		this.SelectCategory("BUILDINGS");
		this.searchField.onValueChanged.RemoveAllListeners();
		this.searchField.onValueChanged.AddListener(delegate(string value)
		{
			this.RefreshGallery();
		});
		this.clearSearchButton.ClearOnClick();
		this.clearSearchButton.onClick += this.ClearSearch;
		MultiToggle multiToggle = this.doublesOnlyToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			int newState = (this.showFilterState + 1) % 3;
			this.ToggleDoublesOnly(newState);
		}));
	}

	// Token: 0x06009C2E RID: 39982 RVA: 0x0010595A File Offset: 0x00103B5A
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.ToggleDoublesOnly(0);
		this.ClearSearch();
		if (!this.initConfigComplete)
		{
			this.InitConfig();
		}
		this.RefreshUI();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshUI();
		});
	}

	// Token: 0x06009C2F RID: 39983 RVA: 0x0010599A File Offset: 0x00103B9A
	private void ClearSearch()
	{
		this.searchField.text = "";
		this.searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
		this.RefreshGallery();
	}

	// Token: 0x06009C30 RID: 39984 RVA: 0x001059D1 File Offset: 0x00103BD1
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06009C31 RID: 39985 RVA: 0x003C2250 File Offset: 0x003C0450
	private void RefreshUI()
	{
		this.IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
		this.RefreshCategories();
		this.RefreshGallery();
		if (this.SelectedCategoryId.IsNullOrWhiteSpace())
		{
			this.SelectCategory("BUILDINGS");
		}
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x06009C32 RID: 39986 RVA: 0x001059DE File Offset: 0x00103BDE
	private GameObject GetAvailableGridButton()
	{
		if (this.recycledGalleryGridButtons.Count == 0)
		{
			return Util.KInstantiateUI(this.gridItemPrefab, this.galleryGridContent.gameObject, true);
		}
		GameObject result = this.recycledGalleryGridButtons[0];
		this.recycledGalleryGridButtons.RemoveAt(0);
		return result;
	}

	// Token: 0x06009C33 RID: 39987 RVA: 0x00105A1D File Offset: 0x00103C1D
	private void RecycleGalleryGridButton(GameObject button)
	{
		button.GetComponent<MultiToggle>().onClick = null;
		this.recycledGalleryGridButtons.Add(button);
	}

	// Token: 0x06009C34 RID: 39988 RVA: 0x003C22A0 File Offset: 0x003C04A0
	public void PopulateCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
		}
		this.categoryToggles.Clear();
		foreach (KeyValuePair<string, List<string>> keyValuePair2 in InventoryOrganization.categoryIdToSubcategoryIdsMap)
		{
			string categoryId2;
			List<string> list;
			keyValuePair2.Deconstruct(out categoryId2, out list);
			string categoryId = categoryId2;
			GameObject gameObject = Util.KInstantiateUI(this.categoryRowPrefab, this.categoryListContent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
			component.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
			MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = component2;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
			component2.onClick = delegate()
			{
				this.SelectCategory(categoryId);
			};
			this.categoryToggles.Add(categoryId, component2);
			this.SetCatogoryClickUISound(categoryId, component2);
		}
	}

	// Token: 0x06009C35 RID: 39989 RVA: 0x003C2420 File Offset: 0x003C0620
	public void PopulateGallery()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			this.RecycleGalleryGridButton(keyValuePair.Value.gameObject);
		}
		this.galleryGridButtons.Clear();
		this.galleryGridLayouter.ImmediateSizeGridToScreenResolution();
		foreach (PermitResource permitResource in Db.Get().Permits.resources)
		{
			if (!permitResource.Id.StartsWith("visonly_"))
			{
				this.AddItemToGallery(permitResource);
			}
		}
		this.subcategories.Sort((KleiInventoryUISubcategory a, KleiInventoryUISubcategory b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[b.subcategoryID].sortKey));
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.gameObject.transform.SetAsLastSibling();
		}
		this.CollectSubcategoryGridLayouts();
		this.CloseSubcategory("UNCATEGORIZED");
	}

	// Token: 0x06009C36 RID: 39990 RVA: 0x003C2578 File Offset: 0x003C0778
	private void CloseSubcategory(string subcategoryID)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryID);
		if (kleiInventoryUISubcategory != null)
		{
			kleiInventoryUISubcategory.ToggleOpen(false);
		}
	}

	// Token: 0x06009C37 RID: 39991 RVA: 0x003C25BC File Offset: 0x003C07BC
	private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryId);
		if (kleiInventoryUISubcategory == null)
		{
			kleiInventoryUISubcategory = Util.KInstantiateUI(this.subcategoryPrefab, this.galleryGridContent.gameObject, true).GetComponent<KleiInventoryUISubcategory>();
			kleiInventoryUISubcategory.subcategoryID = subcategoryId;
			this.subcategories.Add(kleiInventoryUISubcategory);
			kleiInventoryUISubcategory.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
		}
		itemButton.transform.SetParent(kleiInventoryUISubcategory.gridLayout.transform);
	}

	// Token: 0x06009C38 RID: 39992 RVA: 0x003C2668 File Offset: 0x003C0868
	private void CollectSubcategoryGridLayouts()
	{
		this.galleryGridLayouter.OnSizeGridComplete = null;
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			this.galleryGridLayouter.targetGridLayouts.Add(kleiInventoryUISubcategory.gridLayout);
			GridLayouter gridLayouter = this.galleryGridLayouter;
			gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete, new System.Action(kleiInventoryUISubcategory.RefreshDisplay));
		}
		this.galleryGridLayouter.RequestGridResize();
	}

	// Token: 0x06009C39 RID: 39993 RVA: 0x003C2708 File Offset: 0x003C0908
	private void AddItemToGallery(PermitResource permit)
	{
		if (this.galleryGridButtons.ContainsKey(permit))
		{
			return;
		}
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		GameObject availableGridButton = this.GetAvailableGridButton();
		this.AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
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
			reference2.gameObject.SetActive(false);
			reference3.gameObject.SetActive(false);
		}
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			reference4.gameObject.SetActive(true);
			reference4.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
		}
		else
		{
			reference4.gameObject.SetActive(false);
		}
		MultiToggle multiToggle = component2;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
		component2.onClick = delegate()
		{
			this.SelectItem(permit);
		};
		this.galleryGridButtons.Add(permit, component2);
		this.SetItemClickUISound(permit, component2);
		KleiItemsUI.ConfigureTooltipOn(availableGridButton, KleiItemsUI.GetTooltipStringFor(permit));
	}

	// Token: 0x06009C3A RID: 39994 RVA: 0x00105A37 File Offset: 0x00103C37
	public void SelectCategory(string categoryId)
	{
		if (InventoryOrganization.categoryIdToIsEmptyMap[categoryId])
		{
			return;
		}
		this.SelectedCategoryId = categoryId;
		this.galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
		this.RefreshCategories();
		this.SelectDefaultCategoryItem();
	}

	// Token: 0x06009C3B RID: 39995 RVA: 0x003C28D4 File Offset: 0x003C0AD4
	private void SelectDefaultCategoryItem()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			if (InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(InventoryOrganization.GetPermitSubcategory(keyValuePair.Key)))
			{
				this.SelectItem(keyValuePair.Key);
				return;
			}
		}
		this.SelectItem(null);
	}

	// Token: 0x06009C3C RID: 39996 RVA: 0x00105A6B File Offset: 0x00103C6B
	public void SelectItem(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x06009C3D RID: 39997 RVA: 0x003C295C File Offset: 0x003C0B5C
	private void RefreshGallery()
	{
		string value = this.searchField.text.ToUpper();
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			PermitResource permitResource;
			MultiToggle multiToggle;
			keyValuePair.Deconstruct(out permitResource, out multiToggle);
			PermitResource permitResource2 = permitResource;
			MultiToggle multiToggle2 = multiToggle;
			string permitSubcategory = InventoryOrganization.GetPermitSubcategory(permitResource2);
			bool flag = permitSubcategory == "UNCATEGORIZED" || InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(permitSubcategory);
			flag = (flag && (permitResource2.Name.ToUpper().Contains(value) || permitResource2.Id.ToUpper().Contains(value) || permitResource2.Description.ToUpper().Contains(value)));
			multiToggle2.ChangeState((permitResource2 == this.SelectedPermit) ? 1 : 0);
			HierarchyReferences component = multiToggle2.gameObject.GetComponent<HierarchyReferences>();
			LocText reference = component.GetReference<LocText>("OwnedCountLabel");
			Image reference2 = component.GetReference<Image>("IsUnownedOverlay");
			if (permitResource2.IsOwnableOnServer())
			{
				int ownedCount = PermitItems.GetOwnedCount(permitResource2);
				reference.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
				reference.gameObject.SetActive(ownedCount > 0);
				reference2.gameObject.SetActive(ownedCount <= 0);
				if (this.showFilterState == 2 && ownedCount < 2)
				{
					flag = false;
				}
				else if (this.showFilterState == 1 && ownedCount == 0)
				{
					flag = false;
				}
			}
			else if (!permitResource2.IsUnlocked())
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(true);
				if (this.showFilterState != 0)
				{
					flag = false;
				}
			}
			else
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(false);
				if (this.showFilterState == 2)
				{
					flag = false;
				}
			}
			if (multiToggle2.gameObject.activeSelf != flag)
			{
				multiToggle2.gameObject.SetActive(flag);
			}
		}
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.RefreshDisplay();
		}
	}

	// Token: 0x06009C3E RID: 39998 RVA: 0x003C2BC0 File Offset: 0x003C0DC0
	private void RefreshCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			if (InventoryOrganization.categoryIdToIsEmptyMap[keyValuePair.Key])
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			}
		}
	}

	// Token: 0x06009C3F RID: 39999 RVA: 0x003C2C78 File Offset: 0x003C0E78
	private void RefreshDetails()
	{
		PermitResource selectedPermit = this.SelectedPermit;
		PermitPresentationInfo permitPresentationInfo = selectedPermit.GetPermitPresentationInfo();
		this.permitVis.ConfigureWith(selectedPermit);
		this.selectionDetailsScrollRect.rectTransform().anchorMin = new Vector2(0f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRect.rectTransform().sizeDelta = new Vector2(-24f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
		this.selectionDetailsScrollRect.content.rectTransform().sizeDelta = new Vector2(0f, this.selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMin = new Vector2(1f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRectScrollBarContainer.sizeDelta = new Vector2(24f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
		this.selectionHeaderLabel.SetText(selectedPermit.Name);
		this.selectionNameLabel.SetText(selectedPermit.Name);
		this.selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
		this.selectionDescriptionLabel.SetText(selectedPermit.Description);
		this.selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(permitPresentationInfo.facadeFor));
		this.selectionFacadeForLabel.SetText(permitPresentationInfo.facadeFor);
		string dlcIdFrom = selectedPermit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.selectionRarityDetailsLabel.gameObject.SetActive(false);
			this.selectionOwnedCount.gameObject.SetActive(false);
			this.selectionCollectionLabel.gameObject.SetActive(true);
			if (selectedPermit.Rarity == PermitRarity.UniversalLocked)
			{
				this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
				return;
			}
			this.selectionCollectionLabel.SetText(UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
			return;
		}
		else
		{
			this.selectionCollectionLabel.gameObject.SetActive(false);
			string text = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", selectedPermit.Rarity.GetLocStringName());
			this.selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
			this.selectionRarityDetailsLabel.SetText(text);
			this.selectionOwnedCount.gameObject.SetActive(true);
			if (!selectedPermit.IsOwnableOnServer())
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
				return;
			}
			int ownedCount = PermitItems.GetOwnedCount(selectedPermit);
			if (ownedCount > 0)
			{
				this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}", ownedCount.ToString()));
				return;
			}
			this.selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			return;
		}
	}

	// Token: 0x06009C40 RID: 40000 RVA: 0x003C2F88 File Offset: 0x003C1188
	private KleiInventoryScreen.PermitPrintabilityState GetPermitPrintabilityState(PermitResource permit)
	{
		if (!this.IS_ONLINE)
		{
			return KleiInventoryScreen.PermitPrintabilityState.UserOffline;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		if (num == 0UL)
		{
			if (permit.Rarity == PermitRarity.Universal || permit.Rarity == PermitRarity.UniversalLocked || permit.Rarity == PermitRarity.Loyalty || permit.Rarity == PermitRarity.Unknown)
			{
				return KleiInventoryScreen.PermitPrintabilityState.NotForSale;
			}
			return KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet;
		}
		else
		{
			if (PermitItems.GetOwnedCount(permit) > 0)
			{
				return KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned;
			}
			if (KleiItems.GetFilamentAmount() < num)
			{
				return KleiInventoryScreen.PermitPrintabilityState.TooExpensive;
			}
			return KleiInventoryScreen.PermitPrintabilityState.Printable;
		}
	}

	// Token: 0x06009C41 RID: 40001 RVA: 0x003C2FFC File Offset: 0x003C11FC
	private void RefreshBarterPanel()
	{
		this.barterBuyButton.ClearOnClick();
		this.barterSellButton.ClearOnClick();
		this.barterBuyButton.isInteractable = this.IS_ONLINE;
		this.barterSellButton.isInteractable = this.IS_ONLINE;
		HierarchyReferences component = this.barterBuyButton.GetComponent<HierarchyReferences>();
		HierarchyReferences component2 = this.barterSellButton.GetComponent<HierarchyReferences>();
		new Color(1f, 0.69411767f, 0.69411767f);
		Color color = new Color(0.6f, 0.9529412f, 0.5019608f);
		LocText reference = component.GetReference<LocText>("CostLabel");
		LocText reference2 = component2.GetReference<LocText>("CostLabel");
		this.barterPanelBG.color = (this.IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F"));
		this.filamentWalletSection.gameObject.SetActive(this.IS_ONLINE);
		this.barterOfflineLabel.gameObject.SetActive(!this.IS_ONLINE);
		ulong filamentAmount = KleiItems.GetFilamentAmount();
		this.filamentWalletSection.GetComponent<ToolTip>().SetSimpleTooltip((filamentAmount > 1UL) ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_PLURAL_TOOLTIP, filamentAmount) : string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP, filamentAmount));
		KleiInventoryScreen.PermitPrintabilityState permitPrintabilityState = this.GetPermitPrintabilityState(this.SelectedPermit);
		if (!this.IS_ONLINE)
		{
			component.GetReference<LocText>("CostLabel").SetText("");
			reference2.SetText("");
			reference2.color = Color.white;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		this.filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
		switch (permitPrintabilityState)
		{
		case KleiInventoryScreen.PermitPrintabilityState.Printable:
			this.barterBuyButton.isInteractable = true;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE, num.ToString()));
			reference.SetText("-" + num.ToString());
			this.barterBuyButton.onClick += delegate()
			{
				GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
				gameObject.rectTransform().sizeDelta = Vector2.zero;
				gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, true);
			};
			break;
		case KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_ALREADY_OWNED);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.TooExpensive:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);
			reference.SetText("-" + num.ToString());
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSale:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);
			reference.SetText("");
			break;
		case KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet:
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_BETA);
			reference.SetText("");
			break;
		}
		if (num2 == 0UL)
		{
			this.barterSellButton.isInteractable = false;
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);
			reference2.SetText("");
			reference2.color = Color.white;
			return;
		}
		bool flag = PermitItems.GetOwnedCount(this.SelectedPermit) > 0;
		this.barterSellButton.isInteractable = flag;
		this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE, num2.ToString()) : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);
		if (flag)
		{
			reference2.color = color;
			reference2.SetText("+" + num2.ToString());
		}
		else
		{
			reference2.color = Color.white;
			reference2.SetText("+" + num2.ToString());
		}
		this.barterSellButton.onClick += delegate()
		{
			GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, false);
		};
	}

	// Token: 0x06009C42 RID: 40002 RVA: 0x003C3454 File Offset: 0x003C1654
	private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle)
	{
		if (!this.categoryToggles.ContainsKey(categoryID))
		{
			toggle.states[1].on_click_override_sound_path = "";
			toggle.states[0].on_click_override_sound_path = "";
			return;
		}
		toggle.states[1].on_click_override_sound_path = "General_Category_Click";
		toggle.states[0].on_click_override_sound_path = "General_Category_Click";
	}

	// Token: 0x06009C43 RID: 40003 RVA: 0x003C34C8 File Offset: 0x003C16C8
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		string facadeItemSoundName = KleiInventoryScreen.GetFacadeItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06009C44 RID: 40004 RVA: 0x003C35B0 File Offset: 0x003C17B0
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
		if (permit.Category == PermitCategory.Building)
		{
			BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef == null)
			{
				return "HUD";
			}
			string prefabID = buildingDef.PrefabID;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(prefabID);
			if (num <= 2076384603U)
			{
				if (num <= 1633134164U)
				{
					if (num <= 595816591U)
					{
						if (num != 228062815U)
						{
							if (num != 595816591U)
							{
								goto IL_38D;
							}
							if (!(prefabID == "FlowerVase"))
							{
								goto IL_38D;
							}
						}
						else
						{
							if (!(prefabID == "LuxuryBed"))
							{
								goto IL_38D;
							}
							string id = permit.Id;
							if (id == "LuxuryBed_boat")
							{
								return "elegantbed_boat";
							}
							if (!(id == "LuxuryBed_bouncy"))
							{
								return "elegantbed";
							}
							return "elegantbed_bouncy";
						}
					}
					else if (num != 1607642960U)
					{
						if (num != 1633134164U)
						{
							goto IL_38D;
						}
						if (!(prefabID == "CeilingLight"))
						{
							goto IL_38D;
						}
						return "ceilingLight";
					}
					else
					{
						if (!(prefabID == "FlushToilet"))
						{
							goto IL_38D;
						}
						return "flushtoilate";
					}
				}
				else if (num <= 1943253450U)
				{
					if (num != 1734850496U)
					{
						if (num != 1943253450U)
						{
							goto IL_38D;
						}
						if (!(prefabID == "WaterCooler"))
						{
							goto IL_38D;
						}
						return "watercooler";
					}
					else
					{
						if (!(prefabID == "RockCrusher"))
						{
							goto IL_38D;
						}
						return "rockrefinery";
					}
				}
				else if (num != 2028863301U)
				{
					if (num != 2076384603U)
					{
						goto IL_38D;
					}
					if (!(prefabID == "GasReservoir"))
					{
						goto IL_38D;
					}
					return "gasstorage";
				}
				else if (!(prefabID == "FlowerVaseHanging"))
				{
					goto IL_38D;
				}
			}
			else if (num <= 3048425356U)
			{
				if (num <= 2722382738U)
				{
					if (num != 2402859370U)
					{
						if (num != 2722382738U)
						{
							goto IL_38D;
						}
						if (!(prefabID == "PlanterBox"))
						{
							goto IL_38D;
						}
						return "planterbox";
					}
					else
					{
						if (!(prefabID == "StorageLocker"))
						{
							goto IL_38D;
						}
						return "storagelocker";
					}
				}
				else if (num != 2899744071U)
				{
					if (num != 3048425356U)
					{
						goto IL_38D;
					}
					if (!(prefabID == "Bed"))
					{
						goto IL_38D;
					}
					return "bed";
				}
				else
				{
					if (!(prefabID == "ExteriorWall"))
					{
						goto IL_38D;
					}
					return "wall";
				}
			}
			else if (num <= 3534553076U)
			{
				if (num != 3132083755U)
				{
					if (num != 3534553076U)
					{
						goto IL_38D;
					}
					if (!(prefabID == "MassageTable"))
					{
						goto IL_38D;
					}
					return "massagetable";
				}
				else if (!(prefabID == "FlowerVaseWall"))
				{
					goto IL_38D;
				}
			}
			else if (num != 3903452895U)
			{
				if (num != 3958671086U)
				{
					goto IL_38D;
				}
				if (!(prefabID == "FlowerVaseHangingFancy"))
				{
					goto IL_38D;
				}
			}
			else
			{
				if (!(prefabID == "EggCracker"))
				{
					goto IL_38D;
				}
				return "eggcracker";
			}
			return "flowervase";
		}
		IL_38D:
		if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2 == null)
			{
				return "HUD";
			}
			ArtableStage artableStage = (ArtableStage)permit;
			if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|76_0<Sculpture>(buildingDef2))
			{
				string prefabID = buildingDef2.PrefabID;
				if (prefabID == "IceSculpture")
				{
					return "icesculpture";
				}
				if (!(prefabID == "WoodSculpture"))
				{
					return "sculpture";
				}
				return "woodsculpture";
			}
			else if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|76_0<Painting>(buildingDef2))
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

	// Token: 0x06009C45 RID: 40005 RVA: 0x001051E4 File Offset: 0x001033E4
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06009C4D RID: 40013 RVA: 0x00105B3C File Offset: 0x00103D3C
	[CompilerGenerated]
	internal static bool <GetFacadeItemSoundName>g__Has|76_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04007A55 RID: 31317
	[Header("Header")]
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007A56 RID: 31318
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04007A57 RID: 31319
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04007A58 RID: 31320
	private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

	// Token: 0x04007A59 RID: 31321
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04007A5A RID: 31322
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04007A5B RID: 31323
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04007A5C RID: 31324
	[SerializeField]
	private GameObject subcategoryPrefab;

	// Token: 0x04007A5D RID: 31325
	[SerializeField]
	private GameObject itemDummyPrefab;

	// Token: 0x04007A5E RID: 31326
	[Header("GalleryFilters")]
	[SerializeField]
	private KInputTextField searchField;

	// Token: 0x04007A5F RID: 31327
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04007A60 RID: 31328
	[SerializeField]
	private MultiToggle doublesOnlyToggle;

	// Token: 0x04007A61 RID: 31329
	public const int FILTER_SHOW_ALL = 0;

	// Token: 0x04007A62 RID: 31330
	public const int FILTER_SHOW_OWNED_ONLY = 1;

	// Token: 0x04007A63 RID: 31331
	public const int FILTER_SHOW_DOUBLES_ONLY = 2;

	// Token: 0x04007A64 RID: 31332
	private int showFilterState;

	// Token: 0x04007A65 RID: 31333
	[Header("BarterSection")]
	[SerializeField]
	private Image barterPanelBG;

	// Token: 0x04007A66 RID: 31334
	[SerializeField]
	private KButton barterBuyButton;

	// Token: 0x04007A67 RID: 31335
	[SerializeField]
	private KButton barterSellButton;

	// Token: 0x04007A68 RID: 31336
	[SerializeField]
	private GameObject barterConfirmationScreenPrefab;

	// Token: 0x04007A69 RID: 31337
	[SerializeField]
	private GameObject filamentWalletSection;

	// Token: 0x04007A6A RID: 31338
	[SerializeField]
	private GameObject barterOfflineLabel;

	// Token: 0x04007A6B RID: 31339
	private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();

	// Token: 0x04007A6C RID: 31340
	private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

	// Token: 0x04007A6D RID: 31341
	private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

	// Token: 0x04007A6E RID: 31342
	private GridLayouter galleryGridLayouter;

	// Token: 0x04007A6F RID: 31343
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04007A70 RID: 31344
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04007A71 RID: 31345
	[SerializeField]
	private KScrollRect selectionDetailsScrollRect;

	// Token: 0x04007A72 RID: 31346
	[SerializeField]
	private RectTransform selectionDetailsScrollRectScrollBarContainer;

	// Token: 0x04007A73 RID: 31347
	[SerializeField]
	private LocText selectionNameLabel;

	// Token: 0x04007A74 RID: 31348
	[SerializeField]
	private LocText selectionDescriptionLabel;

	// Token: 0x04007A75 RID: 31349
	[SerializeField]
	private LocText selectionFacadeForLabel;

	// Token: 0x04007A76 RID: 31350
	[SerializeField]
	private LocText selectionCollectionLabel;

	// Token: 0x04007A77 RID: 31351
	[SerializeField]
	private LocText selectionRarityDetailsLabel;

	// Token: 0x04007A78 RID: 31352
	[SerializeField]
	private LocText selectionOwnedCount;

	// Token: 0x04007A7A RID: 31354
	private bool IS_ONLINE;

	// Token: 0x04007A7B RID: 31355
	private bool initConfigComplete;

	// Token: 0x02001D39 RID: 7481
	private enum PermitPrintabilityState
	{
		// Token: 0x04007A7E RID: 31358
		Printable,
		// Token: 0x04007A7F RID: 31359
		AlreadyOwned,
		// Token: 0x04007A80 RID: 31360
		TooExpensive,
		// Token: 0x04007A81 RID: 31361
		NotForSale,
		// Token: 0x04007A82 RID: 31362
		NotForSaleYet,
		// Token: 0x04007A83 RID: 31363
		UserOffline
	}

	// Token: 0x02001D3A RID: 7482
	private enum MultiToggleState
	{
		// Token: 0x04007A85 RID: 31365
		Default,
		// Token: 0x04007A86 RID: 31366
		Selected,
		// Token: 0x04007A87 RID: 31367
		NonInteractable
	}
}
