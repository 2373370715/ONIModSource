using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class SealConfig : IEntityConfig
{
	public static GameObject CreateSeal(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSealConfig.BaseSeal(id, name, desc, anim_file, "SealBaseTrait", is_baby, null);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SealTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SealBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseSealConfig.BasicDiet(SimHashes.Ethanol.CreateTag(), 2500f, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f);
		gameObject = BaseSealConfig.SetupDiet(gameObject, diet_infos, 2500f, SealConfig.MIN_POOP_SIZE_IN_KG);
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

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

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

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "Seal";

	public const string BASE_TRAIT_ID = "SealBaseTrait";

	public const string EGG_ID = "SealEgg";

	public const float SUGAR_TREE_SEED_PROBABILITY_ON_POOP = 0.2f;

	public const float SUGAR_WATER_KG_CONSUMED_PER_DAY = 40f;

	public const float CALORIES_PER_1KG_OF_SUGAR_WATER = 2500f;

	private static float MIN_POOP_SIZE_IN_KG = 10f;

	public static int EGG_SORT_ORDER = 0;
}
