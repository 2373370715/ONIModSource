using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class PuftConfig : IEntityConfig
{
	// Token: 0x060008EA RID: 2282 RVA: 0x00164150 File Offset: 0x00162350
	public static GameObject CreatePuft(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BasePuftConfig.BasePuft(id, name, STRINGS.CREATURES.SPECIES.PUFT.DESC, "PuftBaseTrait", anim_file, is_baby, null, 288.15f, 328.15f, 223.15f, 373.15f);
		EntityTemplates.ExtendEntityToWildCreature(prefab, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		GameObject gameObject = BasePuftConfig.SetupDiet(prefab, SimHashes.ContaminatedOxygen.CreateTag(), SimHashes.SlimeMold.CreateTag(), PuftConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, "SlimeLung", 0f, PuftConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x001642C0 File Offset: 0x001624C0
	public GameObject CreatePrefab()
	{
		GameObject prefab = PuftConfig.CreatePuft("Puft", STRINGS.CREATURES.SPECIES.PUFT.NAME, STRINGS.CREATURES.SPECIES.PUFT.DESC, "puft_kanim", false);
		string eggId = "PuftEgg";
		string eggName = STRINGS.CREATURES.SPECIES.PUFT.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.PUFT.DESC;
		string egg_anim = "egg_puft_kanim";
		float egg_MASS = PuftTuning.EGG_MASS;
		string baby_id = "PuftBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = PuftTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = PuftConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x000AA361 File Offset: 0x000A8561
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006A9 RID: 1705
	public const string ID = "Puft";

	// Token: 0x040006AA RID: 1706
	public const string BASE_TRAIT_ID = "PuftBaseTrait";

	// Token: 0x040006AB RID: 1707
	public const string EGG_ID = "PuftEgg";

	// Token: 0x040006AC RID: 1708
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x040006AD RID: 1709
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x040006AE RID: 1710
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x040006AF RID: 1711
	public const float EMIT_DISEASE_PER_KG = 0f;

	// Token: 0x040006B0 RID: 1712
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040006B1 RID: 1713
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040006B2 RID: 1714
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x040006B3 RID: 1715
	public static int EGG_SORT_ORDER = 300;
}
