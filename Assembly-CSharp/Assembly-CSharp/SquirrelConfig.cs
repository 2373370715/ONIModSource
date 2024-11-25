using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class SquirrelConfig : IEntityConfig
{
		public static GameObject CreateSquirrel(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelBaseTrait", is_baby, null, false), SquirrelTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SquirrelBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		GameObject gameObject = BaseSquirrelConfig.SetupDiet(prefab, diet_infos, SquirrelConfig.MIN_POOP_SIZE_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

		public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public GameObject CreatePrefab()
	{
		GameObject prefab = SquirrelConfig.CreateSquirrel("Squirrel", CREATURES.SPECIES.SQUIRREL.NAME, CREATURES.SPECIES.SQUIRREL.DESC, "squirrel_kanim", false);
		string eggId = "SquirrelEgg";
		string eggName = CREATURES.SPECIES.SQUIRREL.EGG_NAME;
		string eggDesc = CREATURES.SPECIES.SQUIRREL.DESC;
		string egg_anim = "egg_squirrel_kanim";
		float egg_MASS = SquirrelTuning.EGG_MASS;
		string baby_id = "SquirrelBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = SquirrelTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = SquirrelConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

		public void OnPrefabInit(GameObject prefab)
	{
	}

		public void OnSpawn(GameObject inst)
	{
	}

		public const string ID = "Squirrel";

		public const string BASE_TRAIT_ID = "SquirrelBaseTrait";

		public const string EGG_ID = "SquirrelEgg";

		public const float OXYGEN_RATE = 0.023437504f;

		public const float BABY_OXYGEN_RATE = 0.011718752f;

		private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

		public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.4f;

		private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

		private static float KG_POOP_PER_DAY_OF_PLANT = 50f;

		private static float MIN_POOP_SIZE_KG = 40f;

		public static int EGG_SORT_ORDER = 0;
}
