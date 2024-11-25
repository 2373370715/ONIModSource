using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class OutfitDescriptionPanel : KMonoBehaviour {
    public static readonly string[] NO_ITEMS = new string[0];

    [SerializeField]
    private LocText collectionLabel;

    [SerializeField]
    private GameObject itemDescriptionContainer;

    [SerializeField]
    private GameObject itemDescriptionRowPrefab;

    private readonly List<GameObject> itemDescriptionRows = new List<GameObject>();

    [SerializeField]
    public LocText outfitDescriptionLabel;

    [SerializeField]
    public LocText outfitNameLabel;

    [SerializeField]
    private LocText usesUnownedItemsLabel;

    public void Refresh(PermitResource                   permitResource,
                        ClothingOutfitUtility.OutfitType outfitType,
                        Option<Personality>              personality) {
        if (permitResource != null) {
            Refresh(permitResource.Name, new[] { permitResource.Id }, outfitType, personality);
            return;
        }

        Refresh(UI.OUTFIT_NAME.NONE, NO_ITEMS, outfitType, personality);
    }

    public void Refresh(Option<ClothingOutfitTarget>     outfit,
                        ClothingOutfitUtility.OutfitType outfitType,
                        Option<Personality>              personality) {
        if (outfit.IsSome()) {
            Refresh(outfit.Unwrap().ReadName(), outfit.Unwrap().ReadItems(), outfitType, personality);
            if (personality.IsNone() && outfit.IsSome()) {
                var impl = outfit.Unwrap().impl;
                if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate) {
                    var databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
                    var dlcIdFrom                = databaseAuthoredTemplate.resource.GetDlcIdFrom();
                    if (DlcManager.IsDlcId(dlcIdFrom)) {
                        collectionLabel.text
                            = UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}",
                                                                          DlcManager.GetDlcTitle(dlcIdFrom));

                        collectionLabel.gameObject.SetActive(true);
                        collectionLabel.transform.SetAsLastSibling();
                    }
                }
            }
        } else
            Refresh(KleiItemsUI.GetNoneOutfitName(outfitType), NO_ITEMS, outfitType, personality);
    }

    public void Refresh(OutfitDesignerScreen_OutfitState outfitState, Option<Personality> personality) {
        Refresh(outfitState.name, outfitState.GetItems(), outfitState.outfitType, personality);
    }

    public void Refresh(string                           outfitName,
                        string[]                         outfitItemIds,
                        ClothingOutfitUtility.OutfitType outfitType,
                        Option<Personality>              personality) {
        ClearItemDescRows();
        using (var pooledDictionary
               = PoolsFor<OutfitDescriptionPanel>.AllocateDict<PermitCategory, Option<PermitResource>>()) {
            using (var pooledList = PoolsFor<OutfitDescriptionPanel>.AllocateList<PermitResource>()) {
                switch (outfitType) {
                    case ClothingOutfitUtility.OutfitType.Clothing:
                        outfitNameLabel.SetText(outfitName);
                        outfitDescriptionLabel.gameObject.SetActive(false);
                        foreach (var key in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING)
                            pooledDictionary.Add(key, Option.None);

                        break;
                    case ClothingOutfitUtility.OutfitType.JoyResponse:
                        if (outfitItemIds != null && outfitItemIds.Length != 0) {
                            if (Db.Get().Permits.BalloonArtistFacades.TryGet(outfitItemIds[0]) != null) {
                                outfitDescriptionLabel.gameObject.SetActive(true);
                                string text = DUPLICANTS.TRAITS.BALLOONARTIST.NAME;
                                outfitNameLabel.SetText(text);
                                outfitDescriptionLabel.SetText(outfitName);
                            }
                        } else {
                            outfitNameLabel.SetText(outfitName);
                            outfitDescriptionLabel.gameObject.SetActive(false);
                        }

                        pooledDictionary.Add(PermitCategory.JoyResponse, Option.None);
                        break;
                    case ClothingOutfitUtility.OutfitType.AtmoSuit:
                        outfitNameLabel.SetText(outfitName);
                        outfitDescriptionLabel.gameObject.SetActive(false);
                        foreach (var key2 in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS)
                            pooledDictionary.Add(key2, Option.None);

                        break;
                }

                foreach (var id in outfitItemIds) {
                    var                    permitResource = Db.Get().Permits.Get(id);
                    Option<PermitResource> option;
                    if (pooledDictionary.TryGetValue(permitResource.Category, out option) && !option.HasValue)
                        pooledDictionary[permitResource.Category] = permitResource;
                    else
                        pooledList.Add(permitResource);
                }

                foreach (var keyValuePair in pooledDictionary) {
                    PermitCategory         permitCategory;
                    Option<PermitResource> option2;
                    keyValuePair.Deconstruct(out permitCategory, out option2);
                    var category = permitCategory;
                    var option3  = option2;
                    if (option3.HasValue)
                        AddItemDescRow(option3.Value);
                    else
                        AddItemDescRow(KleiItemsUI.GetNoneClothingItemIcon(category, personality),
                                       KleiItemsUI.GetNoneClothingItemStrings(category).Item1);
                }

                foreach (var permitResource2 in pooledList) {
                    var permit = (ClothingItemResource)permitResource2;
                    AddItemDescRow(permit);
                }
            }
        }

        var flag = ClothingOutfitTarget.DoesContainLockedItems(outfitItemIds);
        usesUnownedItemsLabel.transform.SetAsLastSibling();
        if (!flag)
            usesUnownedItemsLabel.gameObject.SetActive(false);
        else {
            usesUnownedItemsLabel.SetText(KleiItemsUI.WrapWithColor(UI.OUTFIT_DESCRIPTION.CONTAINS_NON_OWNED_ITEMS,
                                                                    KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));

            usesUnownedItemsLabel.gameObject.SetActive(true);
        }

        collectionLabel.gameObject.SetActive(false);
        KleiItemsStatusRefresher.AddOrGetListener(this)
                                .OnRefreshUI(delegate { Refresh(outfitName, outfitItemIds, outfitType, personality); });
    }

    private void ClearItemDescRows() {
        for (var i = 0; i < itemDescriptionRows.Count; i++) Destroy(itemDescriptionRows[i]);
        itemDescriptionRows.Clear();
    }

    private void AddItemDescRow(PermitResource permit) {
        var    permitPresentationInfo = permit.GetPermitPresentationInfo();
        var    flag                   = permit.IsUnlocked();
        string tooltip                = flag ? null : UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE;
        AddItemDescRow(permitPresentationInfo.sprite, permit.Name, tooltip, flag ? 1f : 0.7f);
    }

    private void AddItemDescRow(Sprite icon, string text, string tooltip = null, float alpha = 1f) {
        var gameObject = Util.KInstantiateUI(itemDescriptionRowPrefab, itemDescriptionContainer, true);
        itemDescriptionRows.Add(gameObject);
        var component = gameObject.GetComponent<HierarchyReferences>();
        component.GetReference<Image>("Icon").sprite = icon;
        component.GetReference<LocText>("Label").SetText(text);
        gameObject.AddOrGet<CanvasGroup>().alpha = alpha;
        gameObject.AddOrGet<NonDrawingGraphic>();
        if (tooltip != null) {
            gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(tooltip);
            return;
        }

        gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
    }
}