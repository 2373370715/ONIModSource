using System;
using System.Collections.Generic;
using System.Linq;

namespace TUNING
{
	// Token: 0x02002285 RID: 8837
	public class STORAGEFILTERS
	{
		// Token: 0x04009B08 RID: 39688
		public static List<Tag> DEHYDRATED = new List<Tag>
		{
			GameTags.Dehydrated
		};

		// Token: 0x04009B09 RID: 39689
		public static List<Tag> FOOD = new List<Tag>
		{
			GameTags.Edible,
			GameTags.CookingIngredient,
			GameTags.Medicine
		};

		// Token: 0x04009B0A RID: 39690
		public static List<Tag> BAGABLE_CREATURES = new List<Tag>
		{
			GameTags.BagableCreature
		};

		// Token: 0x04009B0B RID: 39691
		public static List<Tag> SWIMMING_CREATURES = new List<Tag>
		{
			GameTags.SwimmingCreature
		};

		// Token: 0x04009B0C RID: 39692
		public static List<Tag> NOT_EDIBLE_SOLIDS = new List<Tag>
		{
			GameTags.Alloy,
			GameTags.RefinedMetal,
			GameTags.Metal,
			GameTags.BuildableRaw,
			GameTags.BuildableProcessed,
			GameTags.Farmable,
			GameTags.Organics,
			GameTags.Compostable,
			GameTags.Seed,
			GameTags.Agriculture,
			GameTags.Filter,
			GameTags.ConsumableOre,
			GameTags.Sublimating,
			GameTags.Liquifiable,
			GameTags.IndustrialProduct,
			GameTags.IndustrialIngredient,
			GameTags.MedicalSupplies,
			GameTags.Clothes,
			GameTags.ManufacturedMaterial,
			GameTags.Egg,
			GameTags.RareMaterials,
			GameTags.Other,
			GameTags.StoryTraitResource,
			GameTags.Dehydrated,
			GameTags.ChargedPortableBattery
		};

		// Token: 0x04009B0D RID: 39693
		public static List<Tag> SPECIAL_STORAGE = new List<Tag>
		{
			GameTags.Clothes,
			GameTags.Egg,
			GameTags.Sublimating
		};

		// Token: 0x04009B0E RID: 39694
		public static List<Tag> STORAGE_LOCKERS_STANDARD = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Union(new List<Tag>
		{
			GameTags.Medicine
		}).ToList<Tag>();

		// Token: 0x04009B0F RID: 39695
		public static List<Tag> POWER_BANKS = new List<Tag>
		{
			GameTags.ChargedPortableBattery
		};

		// Token: 0x04009B10 RID: 39696
		public static List<Tag> LIQUIDS = new List<Tag>
		{
			GameTags.Liquid
		};

		// Token: 0x04009B11 RID: 39697
		public static List<Tag> GASES = new List<Tag>
		{
			GameTags.Breathable,
			GameTags.Unbreathable
		};

		// Token: 0x04009B12 RID: 39698
		public static List<Tag> PAYLOADS = new List<Tag>
		{
			"RailGunPayload"
		};

		// Token: 0x04009B13 RID: 39699
		public static Tag[] SOLID_TRANSFER_ARM_CONVEYABLE = new List<Tag>
		{
			GameTags.Seed,
			GameTags.CropSeed
		}.Concat(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD.Concat(STORAGEFILTERS.FOOD).Concat(STORAGEFILTERS.PAYLOADS)).ToArray<Tag>();
	}
}
