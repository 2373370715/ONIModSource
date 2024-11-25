﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class LightBugBlackConfig : IEntityConfig
{
		public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlackBaseTrait", Color.black, DECOR.BONUS.TIER7, is_baby, "blk_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBlackBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("Salsa"),
			TagManager.Create("Meat"),
			TagManager.Create("CookedMeat"),
			SimHashes.Katairite.CreateTag(),
			SimHashes.Phosphorus.CreateTag()
		}, Tag.Invalid, LightBugBlackConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Phosphorus.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlack", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "lightbug_kanim", false);
		string eggId = "LightBugBlackEgg";
		string eggName = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC;
		string egg_anim = "egg_lightbug_kanim";
		float egg_MASS = LightBugTuning.EGG_MASS;
		string baby_id = "LightBugBlackBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BLACK = LightBugTuning.EGG_CHANCES_BLACK;
		int egg_SORT_ORDER = LightBugBlackConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BLACK, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

		public void OnPrefabInit(GameObject inst)
	{
	}

		public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

		public const string ID = "LightBugBlack";

		public const string BASE_TRAIT_ID = "LightBugBlackBaseTrait";

		public const string EGG_ID = "LightBugBlackEgg";

		private static float KG_ORE_EATEN_PER_CYCLE = 1f;

		private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlackConfig.KG_ORE_EATEN_PER_CYCLE;

		public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 5;
}
