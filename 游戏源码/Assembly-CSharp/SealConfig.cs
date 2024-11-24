using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class SealConfig : IEntityConfig
{
	public const string ID = "Seal";

	public const string BASE_TRAIT_ID = "SealBaseTrait";

	public const string EGG_ID = "SealEgg";

	public const float SUGAR_TREE_SEED_PROBABILITY_ON_POOP = 0.2f;

	public const float SUGAR_WATER_KG_CONSUMED_PER_DAY = 40f;

	public const float CALORIES_PER_1KG_OF_SUGAR_WATER = 2500f;

	private static float MIN_POOP_SIZE_IN_KG = 10f;

	public static int EGG_SORT_ORDER = 0;

	public static GameObject CreateSeal(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseSealConfig.BaseSeal(id, name, desc, anim_file, "SealBaseTrait", is_baby);
		prefab = EntityTemplates.ExtendEntityToWildCreature(prefab, SealTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SealBaseTrait", name, name, null, should_save: false, null, positive_trait: true, is_valid_starter_trait: true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (0f - SquirrelTuning.STANDARD_CALORIES_PER_CYCLE) / 600f, UI.TOOLTIPS.BASE_VALUE));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
		List<Diet.Info> diet_infos = BaseSealConfig.BasicDiet(SimHashes.Ethanol.CreateTag(), 2500f, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f);
		prefab = BaseSealConfig.SetupDiet(prefab, diet_infos, 2500f, MIN_POOP_SIZE_IN_KG);
		prefab.AddOrGetDef<CreaturePoopLoot.Def>().Loot = new CreaturePoopLoot.LootData[1]
		{
			new CreaturePoopLoot.LootData
			{
				tag = "SpaceTreeSeed",
				probability = 0.2f
			}
		};
		prefab.AddTag(GameTags.OriginalCreature);
		return prefab;
	}

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(CreateSeal("Seal", STRINGS.CREATURES.SPECIES.SEAL.NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "seal_kanim", is_baby: false), "SealEgg", STRINGS.CREATURES.SPECIES.SEAL.EGG_NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "egg_seal_kanim", SealTuning.EGG_MASS, "SealBaby", 60.000004f, 20f, SealTuning.EGG_CHANCES_BASE, eggSortOrder: EGG_SORT_ORDER, dlcIds: GetDlcIds());
	}

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
