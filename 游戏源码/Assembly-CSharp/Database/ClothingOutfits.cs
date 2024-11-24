namespace Database;

public class ClothingOutfits : ResourceSet<ClothingOutfitResource>
{
	public ClothingOutfits(ResourceSet parent, ClothingItems items_resource)
		: base("ClothingOutfits", parent)
	{
		Initialize();
		resources.AddRange(Blueprints.Get().all.outfits);
		foreach (ClothingOutfitResource resource in resources)
		{
			string[] itemsInOutfit = resource.itemsInOutfit;
			foreach (string itemId in itemsInOutfit)
			{
				int num = items_resource.resources.FindIndex((ClothingItemResource e) => e.Id == itemId);
				if (num < 0)
				{
					DebugUtil.DevAssert(test: false, "Outfit \"" + resource.Id + "\" contains an item that doesn't exist. Given item id: \"" + itemId + "\"");
					continue;
				}
				ClothingItemResource clothingItemResource = items_resource.resources[num];
				if (clothingItemResource.outfitType != resource.outfitType)
				{
					DebugUtil.DevAssert(test: false, $"Outfit \"{resource.Id}\" contains an item that has a mis-matched outfit type. Defined outfit's type: \"{resource.outfitType}\". Given item: {{ id: \"{itemId}\" forOutfitType: \"{clothingItemResource.outfitType}\" }}");
				}
			}
		}
		ClothingOutfitUtility.LoadClothingOutfitData(this);
	}
}
