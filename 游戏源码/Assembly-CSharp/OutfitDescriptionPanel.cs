using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class OutfitDescriptionPanel : KMonoBehaviour
{
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

	public void Refresh(PermitResource permitResource, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		if (permitResource != null)
		{
			Refresh(permitResource.Name, new string[1] { permitResource.Id }, outfitType, personality);
		}
		else
		{
			Refresh(UI.OUTFIT_NAME.NONE, NO_ITEMS, outfitType, personality);
		}
	}

	public void Refresh(Option<ClothingOutfitTarget> outfit, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		if (outfit.IsSome())
		{
			Refresh(outfit.Unwrap().ReadName(), outfit.Unwrap().ReadItems(), outfitType, personality);
			if (personality.IsNone() && outfit.IsSome() && outfit.Unwrap().impl is ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate)
			{
				string dlcIdFrom = databaseAuthoredTemplate.resource.GetDlcIdFrom();
				if (DlcManager.IsDlcId(dlcIdFrom))
				{
					collectionLabel.text = UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom));
					collectionLabel.gameObject.SetActive(value: true);
					collectionLabel.transform.SetAsLastSibling();
				}
			}
		}
		else
		{
			Refresh(KleiItemsUI.GetNoneOutfitName(outfitType), NO_ITEMS, outfitType, personality);
		}
	}

	public void Refresh(OutfitDesignerScreen_OutfitState outfitState, Option<Personality> personality)
	{
		Refresh(outfitState.name, outfitState.GetItems(), outfitState.outfitType, personality);
	}

	public void Refresh(string outfitName, string[] outfitItemIds, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		ClearItemDescRows();
		using (DictionaryPool<PermitCategory, Option<PermitResource>, OutfitDescriptionPanel>.PooledDictionary pooledDictionary = PoolsFor<OutfitDescriptionPanel>.AllocateDict<PermitCategory, Option<PermitResource>>())
		{
			using ListPool<PermitResource, OutfitDescriptionPanel>.PooledList pooledList = PoolsFor<OutfitDescriptionPanel>.AllocateList<PermitResource>();
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			{
				outfitNameLabel.SetText(outfitName);
				outfitDescriptionLabel.gameObject.SetActive(value: false);
				PermitCategory[] pERMIT_CATEGORIES_FOR_CLOTHING = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING;
				foreach (PermitCategory key in pERMIT_CATEGORIES_FOR_CLOTHING)
				{
					pooledDictionary.Add(key, Option.None);
				}
				break;
			}
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
			{
				outfitNameLabel.SetText(outfitName);
				outfitDescriptionLabel.gameObject.SetActive(value: false);
				PermitCategory[] pERMIT_CATEGORIES_FOR_CLOTHING = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS;
				foreach (PermitCategory key2 in pERMIT_CATEGORIES_FOR_CLOTHING)
				{
					pooledDictionary.Add(key2, Option.None);
				}
				break;
			}
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				if (outfitItemIds != null && outfitItemIds.Length != 0)
				{
					if (Db.Get().Permits.BalloonArtistFacades.TryGet(outfitItemIds[0]) != null)
					{
						outfitDescriptionLabel.gameObject.SetActive(value: true);
						string text = DUPLICANTS.TRAITS.BALLOONARTIST.NAME;
						outfitNameLabel.SetText(text);
						outfitDescriptionLabel.SetText(outfitName);
					}
				}
				else
				{
					outfitNameLabel.SetText(outfitName);
					outfitDescriptionLabel.gameObject.SetActive(value: false);
				}
				pooledDictionary.Add(PermitCategory.JoyResponse, Option.None);
				break;
			}
			string[] array = outfitItemIds;
			foreach (string id in array)
			{
				PermitResource permitResource = Db.Get().Permits.Get(id);
				if (pooledDictionary.TryGetValue(permitResource.Category, out var value) && !value.HasValue)
				{
					pooledDictionary[permitResource.Category] = permitResource;
				}
				else
				{
					pooledList.Add(permitResource);
				}
			}
			foreach (var (category, option2) in pooledDictionary)
			{
				if (option2.HasValue)
				{
					AddItemDescRow(option2.Value);
				}
				else
				{
					AddItemDescRow(KleiItemsUI.GetNoneClothingItemIcon(category, personality), KleiItemsUI.GetNoneClothingItemStrings(category).name);
				}
			}
			foreach (ClothingItemResource item in pooledList)
			{
				AddItemDescRow(item);
			}
		}
		bool num = ClothingOutfitTarget.DoesContainLockedItems(outfitItemIds);
		usesUnownedItemsLabel.transform.SetAsLastSibling();
		if (!num)
		{
			usesUnownedItemsLabel.gameObject.SetActive(value: false);
		}
		else
		{
			usesUnownedItemsLabel.SetText(KleiItemsUI.WrapWithColor(UI.OUTFIT_DESCRIPTION.CONTAINS_NON_OWNED_ITEMS, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			usesUnownedItemsLabel.gameObject.SetActive(value: true);
		}
		collectionLabel.gameObject.SetActive(value: false);
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			Refresh(outfitName, outfitItemIds, outfitType, personality);
		});
	}

	private void ClearItemDescRows()
	{
		for (int i = 0; i < itemDescriptionRows.Count; i++)
		{
			Object.Destroy(itemDescriptionRows[i]);
		}
		itemDescriptionRows.Clear();
	}

	private void AddItemDescRow(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		bool flag = permit.IsUnlocked();
		string tooltip = (flag ? null : ((string)UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE));
		AddItemDescRow(permitPresentationInfo.sprite, permit.Name, tooltip, flag ? 1f : 0.7f);
	}

	private void AddItemDescRow(Sprite icon, string text, string tooltip = null, float alpha = 1f)
	{
		GameObject gameObject = Util.KInstantiateUI(itemDescriptionRowPrefab, itemDescriptionContainer, force_active: true);
		itemDescriptionRows.Add(gameObject);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = icon;
		component.GetReference<LocText>("Label").SetText(text);
		gameObject.AddOrGet<CanvasGroup>().alpha = alpha;
		gameObject.AddOrGet<NonDrawingGraphic>();
		if (tooltip != null)
		{
			gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(tooltip);
		}
		else
		{
			gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}
	}
}
