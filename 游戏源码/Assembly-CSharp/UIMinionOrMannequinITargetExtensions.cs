using System;
using System.Collections.Generic;
using System.Linq;
using Database;

public static class UIMinionOrMannequinITargetExtensions
{
	public static readonly ClothingItemResource[] EMPTY_OUTFIT = new ClothingItemResource[0];

	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitResource outfit)
	{
		self.SetOutfit(outfit.outfitType, outfit.itemsInOutfit.Select((string itemId) => Db.Get().Permits.ClothingItems.Get(itemId)));
	}

	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, OutfitDesignerScreen_OutfitState outfit)
	{
		self.SetOutfit(outfit.outfitType, from itemId in outfit.GetItems()
			select Db.Get().Permits.ClothingItems.Get(itemId));
	}

	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitTarget outfit)
	{
		self.SetOutfit(outfit.OutfitType, outfit.ReadItemValues());
	}

	public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType, Option<ClothingOutfitTarget> outfit)
	{
		if (outfit.HasValue)
		{
			self.SetOutfit(outfit.Value);
		}
		else
		{
			self.ClearOutfit(outfitType);
		}
	}

	public static void ClearOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitUtility.OutfitType outfitType)
	{
		self.SetOutfit(outfitType, EMPTY_OUTFIT);
	}

	public static void React(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.None);
	}

	public static void ReactToClothingItemChange(this UIMinionOrMannequin.ITarget self, PermitCategory clothingChangedCategory)
	{
		self.React(GetSource(clothingChangedCategory));
		static UIMinionOrMannequinReactSource GetSource(PermitCategory clothingChangedCategory)
		{
			switch (clothingChangedCategory)
			{
			case PermitCategory.DupeHats:
			case PermitCategory.AtmoSuitHelmet:
				return UIMinionOrMannequinReactSource.OnHatChanged;
			case PermitCategory.DupeTops:
			case PermitCategory.AtmoSuitBody:
			case PermitCategory.AtmoSuitBelt:
				return UIMinionOrMannequinReactSource.OnTopChanged;
			case PermitCategory.DupeBottoms:
				return UIMinionOrMannequinReactSource.OnBottomChanged;
			case PermitCategory.DupeGloves:
			case PermitCategory.AtmoSuitGloves:
				return UIMinionOrMannequinReactSource.OnGlovesChanged;
			case PermitCategory.DupeShoes:
			case PermitCategory.AtmoSuitShoes:
				return UIMinionOrMannequinReactSource.OnShoesChanged;
			default:
				DebugUtil.DevAssert(test: false, $"Couldn't find a reaction for \"{clothingChangedCategory}\" clothing item category being changed");
				return UIMinionOrMannequinReactSource.None;
			}
		}
	}

	public static void ReactToPersonalityChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnPersonalityChanged);
	}

	public static void ReactToFullOutfitChange(this UIMinionOrMannequin.ITarget self)
	{
		self.React(UIMinionOrMannequinReactSource.OnWholeOutfitChanged);
	}

	public static IEnumerable<ClothingItemResource> GetOutfitWithDefaultItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.Clothing:
			return outfit;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
		{
			using DictionaryPool<PermitCategory, ClothingItemResource, UIMinionOrMannequin.ITarget>.PooledDictionary pooledDictionary = PoolsFor<UIMinionOrMannequin.ITarget>.AllocateDict<PermitCategory, ClothingItemResource>();
			foreach (ClothingItemResource item in outfit)
			{
				DebugUtil.DevAssert(!pooledDictionary.ContainsKey(item.Category), "Duplicate item for category");
				pooledDictionary[item.Category] = item;
			}
			if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitHelmet))
			{
				pooledDictionary[PermitCategory.AtmoSuitHelmet] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoHelmetClear");
			}
			if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBody))
			{
				pooledDictionary[PermitCategory.AtmoSuitBody] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoSuitBasicBlue");
			}
			if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitGloves))
			{
				pooledDictionary[PermitCategory.AtmoSuitGloves] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoGlovesBasicBlue");
			}
			if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBelt))
			{
				pooledDictionary[PermitCategory.AtmoSuitBelt] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoBeltBasicBlue");
			}
			if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitShoes))
			{
				pooledDictionary[PermitCategory.AtmoSuitShoes] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoShoesBasicBlack");
			}
			return pooledDictionary.Values.ToArray();
		}
		default:
			throw new NotImplementedException();
		}
	}
}
