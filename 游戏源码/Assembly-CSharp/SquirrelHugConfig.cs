using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class SquirrelHugConfig : IEntityConfig
{
	// Token: 0x0600092E RID: 2350 RVA: 0x00164FF8 File Offset: 0x001631F8
	public static GameObject CreateSquirrelHug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelHugBaseTrait", is_baby, "hug_", true);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SquirrelTuning.PEN_SIZE_PER_CREATURE_HUG);
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER3);
		Trait trait = Db.Get().CreateTrait("SquirrelHugBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelHugConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelHugConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		gameObject = BaseSquirrelConfig.SetupDiet(gameObject, diet_infos, SquirrelHugConfig.MIN_POOP_SIZE_KG);
		if (!is_baby)
		{
			gameObject.AddOrGetDef<HugMonitor.Def>();
		}
		return gameObject;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0016514C File Offset: 0x0016334C
	public GameObject CreatePrefab()
	{
		GameObject prefab = SquirrelHugConfig.CreateSquirrelHug("SquirrelHug", STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.NAME, STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC, "squirrel_kanim", false);
		string eggId = "SquirrelHugEgg";
		string eggName = STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC;
		string egg_anim = "egg_squirrel_kanim";
		float egg_MASS = SquirrelTuning.EGG_MASS;
		string baby_id = "SquirrelHugBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_HUG = SquirrelTuning.EGG_CHANCES_HUG;
		int egg_SORT_ORDER = SquirrelHugConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_HUG, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006E2 RID: 1762
	public const string ID = "SquirrelHug";

	// Token: 0x040006E3 RID: 1763
	public const string BASE_TRAIT_ID = "SquirrelHugBaseTrait";

	// Token: 0x040006E4 RID: 1764
	public const string EGG_ID = "SquirrelHugEgg";

	// Token: 0x040006E5 RID: 1765
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x040006E6 RID: 1766
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x040006E7 RID: 1767
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x040006E8 RID: 1768
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.5f;

	// Token: 0x040006E9 RID: 1769
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelHugConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040006EA RID: 1770
	private static float KG_POOP_PER_DAY_OF_PLANT = 25f;

	// Token: 0x040006EB RID: 1771
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x040006EC RID: 1772
	public static int EGG_SORT_ORDER = 0;
}
