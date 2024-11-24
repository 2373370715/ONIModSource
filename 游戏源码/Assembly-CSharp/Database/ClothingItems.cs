using System;

namespace Database;

public class ClothingItems : ResourceSet<ClothingItemResource>
{
	public ClothingItems(ResourceSet parent)
		: base("ClothingItems", parent)
	{
		Initialize();
		foreach (ClothingItemInfo clothingItem in Blueprints.Get().all.clothingItems)
		{
			Add(clothingItem.id, clothingItem.name, clothingItem.desc, clothingItem.outfitType, clothingItem.category, clothingItem.rarity, clothingItem.animFile, clothingItem.dlcIds);
		}
	}

	public ClothingItemResource TryResolveAccessoryResource(ResourceGuid AccessoryGuid)
	{
		if (AccessoryGuid.Guid != null)
		{
			string[] array = AccessoryGuid.Guid.Split('.');
			if (array.Length != 0)
			{
				string symbol_name = array[array.Length - 1];
				return resources.Find((ClothingItemResource ci) => symbol_name.Contains(ci.Id));
			}
		}
		return null;
	}

	[Obsolete("Please use Add(...) with dlcIds parameter")]
	public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile)
	{
		Add(id, name, desc, outfitType, category, rarity, animFile, DlcManager.AVAILABLE_ALL_VERSIONS);
	}

	public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] dlcIds)
	{
		ClothingItemResource item = new ClothingItemResource(id, name, desc, outfitType, category, rarity, animFile, dlcIds);
		resources.Add(item);
	}
}
