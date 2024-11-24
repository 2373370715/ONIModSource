using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class PuftAlphaConfig : IEntityConfig
{
	// Token: 0x060008D2 RID: 2258 RVA: 0x00163CA4 File Offset: 0x00161EA4
	public static GameObject CreatePuftAlpha(string id, string name, string desc, string anim_file, bool is_baby)
	{
		string symbol_override_prefix = "alp_";
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftAlphaBaseTrait", anim_file, is_baby, symbol_override_prefix, 293.15f, 313.15f, 223.15f, 373.15f);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftAlphaBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ContaminatedOxygen.CreateTag()
			}), SimHashes.SlimeMold.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ChlorineGas.CreateTag()
			}), SimHashes.BleachStone.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.Oxygen.CreateTag()
			}), SimHashes.OxyRock.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, PuftAlphaConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		return gameObject;
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x00163ECC File Offset: 0x001620CC
	public GameObject CreatePrefab()
	{
		GameObject prefab = PuftAlphaConfig.CreatePuftAlpha("PuftAlpha", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "puft_kanim", false);
		string eggId = "PuftAlphaEgg";
		string eggName = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC;
		string egg_anim = "egg_puft_kanim";
		float egg_MASS = PuftTuning.EGG_MASS;
		string baby_id = "PuftAlphaBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_ALPHA = PuftTuning.EGG_CHANCES_ALPHA;
		int egg_SORT_ORDER = PuftAlphaConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_ALPHA, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x000AA348 File Offset: 0x000A8548
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<KBatchedAnimController>().animScale *= 1.1f;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x000AA361 File Offset: 0x000A8561
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000693 RID: 1683
	public const string ID = "PuftAlpha";

	// Token: 0x04000694 RID: 1684
	public const string BASE_TRAIT_ID = "PuftAlphaBaseTrait";

	// Token: 0x04000695 RID: 1685
	public const string EGG_ID = "PuftAlphaEgg";

	// Token: 0x04000696 RID: 1686
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x04000697 RID: 1687
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x04000698 RID: 1688
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x04000699 RID: 1689
	public const float EMIT_DISEASE_PER_KG = 0f;

	// Token: 0x0400069A RID: 1690
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x0400069B RID: 1691
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftAlphaConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400069C RID: 1692
	private static float MIN_POOP_SIZE_IN_KG = 5f;

	// Token: 0x0400069D RID: 1693
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 1;
}
