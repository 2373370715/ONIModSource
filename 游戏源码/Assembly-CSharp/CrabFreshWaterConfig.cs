using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000225 RID: 549
[EntityConfigOrder(1)]
public class CrabFreshWaterConfig : IEntityConfig
{
	// Token: 0x0600076F RID: 1903 RVA: 0x0015F94C File Offset: 0x0015DB4C
	public static GameObject CreateCrabFreshWater(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID = null)
	{
		GameObject gameObject = BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabFreshWaterBaseTrait", is_baby, CrabFreshWaterConfig.animPrefix, deathDropID, 1);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabFreshWaterBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.DietWithSlime(SimHashes.Sand.CreateTag(), CrabFreshWaterConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		gameObject = BaseCrabConfig.SetupDiet(gameObject, diet_infos, CrabFreshWaterConfig.CALORIES_PER_KG_OF_ORE, CrabFreshWaterConfig.MIN_POOP_SIZE_IN_KG);
		Butcherable component = gameObject.GetComponent<Butcherable>();
		if (component != null)
		{
			string[] drops = new string[]
			{
				"ShellfishMeat",
				"ShellfishMeat",
				"ShellfishMeat",
				"ShellfishMeat"
			};
			component.SetDrops(drops);
		}
		return gameObject;
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0015FAC8 File Offset: 0x0015DCC8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWater", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.DESC, "pincher_kanim", false, null);
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, "CrabFreshWaterEgg", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabFreshWaterBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_FRESH, this.GetDlcIds(), CrabFreshWaterConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		EggProtectionMonitor.Def def = gameObject.AddOrGetDef<EggProtectionMonitor.Def>();
		def.allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		def.animPrefix = CrabFreshWaterConfig.animPrefix;
		DiseaseEmitter diseaseEmitter = gameObject.AddComponent<DiseaseEmitter>();
		List<Disease> list = new List<Disease>
		{
			Db.Get().Diseases.FoodGerms,
			Db.Get().Diseases.PollenGerms,
			Db.Get().Diseases.SlimeGerms,
			Db.Get().Diseases.ZombieSpores
		};
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(Db.Get().Diseases.RadiationPoisoning);
		}
		diseaseEmitter.SetDiseases(list);
		diseaseEmitter.emitRange = 2;
		diseaseEmitter.emitCount = -1 * Mathf.RoundToInt((float)DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE / 600f * 6f * 2f * 4f / 9f);
		CleaningMonitor.Def def2 = gameObject.AddOrGetDef<CleaningMonitor.Def>();
		def2.elementState = Element.State.Liquid;
		def2.cellOffsets = new CellOffset[]
		{
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(0, 1),
			new CellOffset(-1, 1),
			new CellOffset(1, 1)
		};
		def2.coolDown = 30f;
		return gameObject;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000580 RID: 1408
	public const string ID = "CrabFreshWater";

	// Token: 0x04000581 RID: 1409
	public const string BASE_TRAIT_ID = "CrabFreshWaterBaseTrait";

	// Token: 0x04000582 RID: 1410
	public const string EGG_ID = "CrabFreshWaterEgg";

	// Token: 0x04000583 RID: 1411
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x04000584 RID: 1412
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x04000585 RID: 1413
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabFreshWaterConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000586 RID: 1414
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000587 RID: 1415
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x04000588 RID: 1416
	private static string animPrefix = "fresh_";
}
