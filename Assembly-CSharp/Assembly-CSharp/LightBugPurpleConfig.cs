﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class LightBugPurpleConfig : IEntityConfig
{
		public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPurpleBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PURPLE, DECOR.BONUS.TIER6, is_baby, "prp_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugPurpleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create("FriedMushroom"));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(TagManager.Create(SpiceNutConfig.ID));
		hashSet.Add(TagManager.Create("SpiceBread"));
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		return BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugPurpleConfig.CALORIES_PER_KG_OF_ORE);
	}

		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurple", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC, "lightbug_kanim", false);
		string eggId = "LightBugPurpleEgg";
		string eggName = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC;
		string egg_anim = "egg_lightbug_kanim";
		float egg_MASS = LightBugTuning.EGG_MASS;
		string baby_id = "LightBugPurpleBaby";
		float fertility_cycles = 15.000001f;
		float incubation_cycles = 5f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_PURPLE = LightBugTuning.EGG_CHANCES_PURPLE;
		int egg_SORT_ORDER = LightBugPurpleConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_PURPLE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

		public const string ID = "LightBugPurple";

		public const string BASE_TRAIT_ID = "LightBugPurpleBaseTrait";

		public const string EGG_ID = "LightBugPurpleEgg";

		private static float KG_ORE_EATEN_PER_CYCLE = 1f;

		private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPurpleConfig.KG_ORE_EATEN_PER_CYCLE;

		public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 2;
}
