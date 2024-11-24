﻿using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class OutfitDescriptionPanel : KMonoBehaviour
{
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

	public void Refresh(OutfitDesignerScreen_OutfitState outfitState, Option<Personality> personality)
	{
		this.Refresh(outfitState.name, outfitState.GetItems(), outfitState.outfitType, personality);
	}

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
				foreach (KeyValuePair<PermitCategory, Option<PermitResource>> self in pooledDictionary)
				{
					PermitCategory permitCategory;
					Option<PermitResource> option2;
					self.Deconstruct(out permitCategory, out option2);
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

	private void ClearItemDescRows()
	{
		for (int i = 0; i < this.itemDescriptionRows.Count; i++)
		{
			UnityEngine.Object.Destroy(this.itemDescriptionRows[i]);
		}
		this.itemDescriptionRows.Clear();
	}

	private void AddItemDescRow(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		bool flag = permit.IsUnlocked();
		string tooltip = flag ? null : UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE;
		this.AddItemDescRow(permitPresentationInfo.sprite, permit.Name, tooltip, flag ? 1f : 0.7f);
	}

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

	[SerializeField]
	public LocText outfitNameLabel;

	[SerializeField]
	public LocText outfitDescriptionLabel;

	[SerializeField]
	private GameObject itemDescriptionRowPrefab;

	[SerializeField]
	private GameObject itemDescriptionContainer;

	[SerializeField]
	private LocText collectionLabel;

	[SerializeField]
	private LocText usesUnownedItemsLabel;

	private List<GameObject> itemDescriptionRows = new List<GameObject>();

	public static readonly string[] NO_ITEMS = new string[0];
}
