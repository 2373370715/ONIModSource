using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class SealConfig : IEntityConfig
{
	// Token: 0x06000911 RID: 2321 RVA: 0x00164AD4 File Offset: 0x00162CD4
	public static GameObject CreateSeal(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSealConfig.BaseSeal(id, name, desc, anim_file, "SealBaseTrait", is_baby, null);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SealTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SealBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		gameObject = BaseSealConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>
			{
				"SpaceTree"
			}, SimHashes.Ethanol.CreateTag(), 2500f, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f, false, Diet.Info.FoodType.EatPlantStorage, false, null),
			new Diet.Info(new HashSet<Tag>
			{
				SimHashes.Sucrose.CreateTag()
			}, SimHashes.Ethanol.CreateTag(), 3246.7532f, 1.2987013f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, new string[]
			{
				"eat_ore_pre",
				"eat_ore_loop",
				"eat_ore_pst"
			})
		}, 2500f, SealConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<CreaturePoopLoot.Def>().Loot = new CreaturePoopLoot.LootData[]
		{
			new CreaturePoopLoot.LootData
			{
				tag = "SpaceTreeSeed",
				probability = 0.2f
			}
		};
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00164CD8 File Offset: 0x00162ED8
	public GameObject CreatePrefab()
	{
		GameObject prefab = SealConfig.CreateSeal("Seal", STRINGS.CREATURES.SPECIES.SEAL.NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "seal_kanim", false);
		string eggId = "SealEgg";
		string eggName = STRINGS.CREATURES.SPECIES.SEAL.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.SEAL.DESC;
		string egg_anim = "egg_seal_kanim";
		float egg_MASS = SealTuning.EGG_MASS;
		string baby_id = "SealBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = SealTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = SealConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006CC RID: 1740
	public const string ID = "Seal";

	// Token: 0x040006CD RID: 1741
	public const string BASE_TRAIT_ID = "SealBaseTrait";

	// Token: 0x040006CE RID: 1742
	public const string EGG_ID = "SealEgg";

	// Token: 0x040006CF RID: 1743
	public const float SUGAR_TREE_SEED_PROBABILITY_ON_POOP = 0.2f;

	// Token: 0x040006D0 RID: 1744
	public const float SUGAR_WATER_KG_CONSUMED_PER_DAY = 40f;

	// Token: 0x040006D1 RID: 1745
	public const float CALORIES_PER_1KG_OF_SUGAR_WATER = 2500f;

	// Token: 0x040006D2 RID: 1746
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x040006D3 RID: 1747
	public static int EGG_SORT_ORDER = 0;
}
