using System;

namespace Database
{
	// Token: 0x02002128 RID: 8488
	public class ClothingItems : ResourceSet<ClothingItemResource>
	{
		// Token: 0x0600B4B5 RID: 46261 RVA: 0x00448580 File Offset: 0x00446780
		public ClothingItems(ResourceSet parent) : base("ClothingItems", parent)
		{
			base.Initialize();
			foreach (ClothingItemInfo clothingItemInfo in Blueprints.Get().all.clothingItems)
			{
				this.Add(clothingItemInfo.id, clothingItemInfo.name, clothingItemInfo.desc, clothingItemInfo.outfitType, clothingItemInfo.category, clothingItemInfo.rarity, clothingItemInfo.animFile, clothingItemInfo.dlcIds);
			}
		}

		// Token: 0x0600B4B6 RID: 46262 RVA: 0x00448620 File Offset: 0x00446820
		public ClothingItemResource TryResolveAccessoryResource(ResourceGuid AccessoryGuid)
		{
			if (AccessoryGuid.Guid != null)
			{
				string[] array = AccessoryGuid.Guid.Split('.', StringSplitOptions.None);
				if (array.Length != 0)
				{
					string symbol_name = array[array.Length - 1];
					return this.resources.Find((ClothingItemResource ci) => symbol_name.Contains(ci.Id));
				}
			}
			return null;
		}

		// Token: 0x0600B4B7 RID: 46263 RVA: 0x00448674 File Offset: 0x00446874
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile)
		{
			this.Add(id, name, desc, outfitType, category, rarity, animFile, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x0600B4B8 RID: 46264 RVA: 0x00448698 File Offset: 0x00446898
		public void Add(string id, string name, string desc, ClothingOutfitUtility.OutfitType outfitType, PermitCategory category, PermitRarity rarity, string animFile, string[] dlcIds)
		{
			ClothingItemResource item = new ClothingItemResource(id, name, desc, outfitType, category, rarity, animFile, dlcIds);
			this.resources.Add(item);
		}
	}
}
