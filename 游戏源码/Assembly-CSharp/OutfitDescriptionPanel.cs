using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E78 RID: 7800
public class OutfitDescriptionPanel : KMonoBehaviour
{
	// Token: 0x0600A383 RID: 41859 RVA: 0x0010A0BD File Offset: 0x001082BD
	public void Refresh(PermitResource permitResource, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		if (permitResource != null)
		{
			this.Refresh(permitResource.Name, new string[]
			{
				permitResource.Id
			}, outfitType, personality);
			return;
		}
		this.Refresh(UI.OUTFIT_NAME.NONE, OutfitDescriptionPanel.NO_ITEMS, outfitType, personality);
	}

	// Token: 0x0600A384 RID: 41860 RVA: 0x003E1C08 File Offset: 0x003DFE08
	public void Refresh(Option<ClothingOutfitTarget> outfit, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		if (outfit.IsSome())
		{
			this.Refresh(outfit.Unwrap().ReadName(), outfit.Unwrap().ReadItems(), outfitType, personality);
			if (personality.IsNone() && outfit.IsSome())
			{
				ClothingOutfitTarget.Implementation impl = outfit.Unwrap().impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					string dlcIdFrom = databaseAuthoredTemplate.resource.GetDlcIdFrom();
					if (DlcManager.IsDlcId(dlcIdFrom))
					{
						this.collectionLabel.text = UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom));
						this.collectionLabel.gameObject.SetActive(true);
						this.collectionLabel.transform.SetAsLastSibling();
						return;
					}
				}
			}
		}
		else
		{
			this.Refresh(KleiItemsUI.GetNoneOutfitName(outfitType), OutfitDescriptionPanel.NO_ITEMS, outfitType, personality);
		}
	}

	// Token: 0x0600A385 RID: 41861 RVA: 0x0010A0F7 File Offset: 0x001082F7
	public void Refresh(OutfitDesignerScreen_OutfitState outfitState, Option<Personality> personality)
	{
		this.Refresh(outfitState.name, outfitState.GetItems(), outfitState.outfitType, personality);
	}

	// Token: 0x0600A386 RID: 41862 RVA: 0x003E1CE4 File Offset: 0x003DFEE4
	public void Refresh(string outfitName, string[] outfitItemIds, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		this.ClearItemDescRows();
		using (DictionaryPool<PermitCategory, Option<PermitResource>, OutfitDescriptionPanel>.PooledDictionary pooledDictionary = PoolsFor<OutfitDescriptionPanel>.AllocateDict<PermitCategory, Option<PermitResource>>())
		{
			using (ListPool<PermitResource, OutfitDescriptionPanel>.PooledList pooledList = PoolsFor<OutfitDescriptionPanel>.AllocateList<PermitResource>())
			{
				switch (outfitType)
				{
				case ClothingOutfitUtility.OutfitType.Clothing:
					this.outfitNameLabel.SetText(outfitName);
					this.outfitDescriptionLabel.gameObject.SetActive(false);
					foreach (PermitCategory key in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING)
					{
						pooledDictionary.Add(key, Option.None);
					}
					break;
				case ClothingOutfitUtility.OutfitType.JoyResponse:
					if (outfitItemIds != null && outfitItemIds.Length != 0)
					{
						if (Db.Get().Permits.BalloonArtistFacades.TryGet(outfitItemIds[0]) != null)
						{
							this.outfitDescriptionLabel.gameObject.SetActive(true);
							string text = DUPLICANTS.TRAITS.BALLOONARTIST.NAME;
							this.outfitNameLabel.SetText(text);
							this.outfitDescriptionLabel.SetText(outfitName);
						}
					}
					else
					{
						this.outfitNameLabel.SetText(outfitName);
						this.outfitDescriptionLabel.gameObject.SetActive(false);
					}
					pooledDictionary.Add(PermitCategory.JoyResponse, Option.None);
					break;
				case ClothingOutfitUtility.OutfitType.AtmoSuit:
					this.outfitNameLabel.SetText(outfitName);
					this.outfitDescriptionLabel.gameObject.SetActive(false);
					foreach (PermitCategory key2 in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS)
					{
						pooledDictionary.Add(key2, Option.None);
					}
					break;
				}
				foreach (string id in outfitItemIds)
				{
					PermitResource permitResource = Db.Get().Permits.Get(id);
					Option<PermitResource> option;
					if (pooledDictionary.TryGetValue(permitResource.Category, out option) && !option.HasValue)
					{
						pooledDictionary[permitResource.Category] = permitResource;
					}
					else
					{
						pooledList.Add(permitResource);
					}
				}
				foreach (KeyValuePair<PermitCategory, Option<PermitResource>> keyValuePair in pooledDictionary)
				{
					PermitCategory permitCategory;
					Option<PermitResource> option2;
					keyValuePair.Deconstruct(out permitCategory, out option2);
					PermitCategory category = permitCategory;
					Option<PermitResource> option3 = option2;
					if (option3.HasValue)
					{
						this.AddItemDescRow(option3.Value);
					}
					else
					{
						this.AddItemDescRow(KleiItemsUI.GetNoneClothingItemIcon(category, personality), KleiItemsUI.GetNoneClothingItemStrings(category).Item1, null, 1f);
					}
				}
				foreach (PermitResource permitResource2 in pooledList)
				{
					ClothingItemResource permit = (ClothingItemResource)permitResource2;
					this.AddItemDescRow(permit);
				}
			}
		}
		bool flag = ClothingOutfitTarget.DoesContainLockedItems(outfitItemIds);
		this.usesUnownedItemsLabel.transform.SetAsLastSibling();
		if (!flag)
		{
			this.usesUnownedItemsLabel.gameObject.SetActive(false);
		}
		else
		{
			this.usesUnownedItemsLabel.SetText(KleiItemsUI.WrapWithColor(UI.OUTFIT_DESCRIPTION.CONTAINS_NON_OWNED_ITEMS, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			this.usesUnownedItemsLabel.gameObject.SetActive(true);
		}
		this.collectionLabel.gameObject.SetActive(false);
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.Refresh(outfitName, outfitItemIds, outfitType, personality);
		});
	}

	// Token: 0x0600A387 RID: 41863 RVA: 0x003E20C4 File Offset: 0x003E02C4
	private void ClearItemDescRows()
	{
		for (int i = 0; i < this.itemDescriptionRows.Count; i++)
		{
			UnityEngine.Object.Destroy(this.itemDescriptionRows[i]);
		}
		this.itemDescriptionRows.Clear();
	}

	// Token: 0x0600A388 RID: 41864 RVA: 0x003E2104 File Offset: 0x003E0304
	private void AddItemDescRow(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		bool flag = permit.IsUnlocked();
		string tooltip = flag ? null : UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE;
		this.AddItemDescRow(permitPresentationInfo.sprite, permit.Name, tooltip, flag ? 1f : 0.7f);
	}

	// Token: 0x0600A389 RID: 41865 RVA: 0x003E2154 File Offset: 0x003E0354
	private void AddItemDescRow(Sprite icon, string text, string tooltip = null, float alpha = 1f)
	{
		GameObject gameObject = Util.KInstantiateUI(this.itemDescriptionRowPrefab, this.itemDescriptionContainer, true);
		this.itemDescriptionRows.Add(gameObject);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = icon;
		component.GetReference<LocText>("Label").SetText(text);
		gameObject.AddOrGet<CanvasGroup>().alpha = alpha;
		gameObject.AddOrGet<NonDrawingGraphic>();
		if (tooltip != null)
		{
			gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(tooltip);
			return;
		}
		gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
	}

	// Token: 0x04007FB5 RID: 32693
	[SerializeField]
	public LocText outfitNameLabel;

	// Token: 0x04007FB6 RID: 32694
	[SerializeField]
	public LocText outfitDescriptionLabel;

	// Token: 0x04007FB7 RID: 32695
	[SerializeField]
	private GameObject itemDescriptionRowPrefab;

	// Token: 0x04007FB8 RID: 32696
	[SerializeField]
	private GameObject itemDescriptionContainer;

	// Token: 0x04007FB9 RID: 32697
	[SerializeField]
	private LocText collectionLabel;

	// Token: 0x04007FBA RID: 32698
	[SerializeField]
	private LocText usesUnownedItemsLabel;

	// Token: 0x04007FBB RID: 32699
	private List<GameObject> itemDescriptionRows = new List<GameObject>();

	// Token: 0x04007FBC RID: 32700
	public static readonly string[] NO_ITEMS = new string[0];
}
