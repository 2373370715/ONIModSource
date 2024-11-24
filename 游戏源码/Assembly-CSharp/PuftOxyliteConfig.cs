using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class PuftOxyliteConfig : IEntityConfig
{
	// Token: 0x060008F6 RID: 2294 RVA: 0x00164344 File Offset: 0x00162544
	public static GameObject CreatePuftOxylite(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftOxyliteBaseTrait", anim_file, is_baby, "com_", 273.15f, 333.15f, 223.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftOxyliteBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), SimHashes.OxyRock.CreateTag(), PuftOxyliteConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftOxyliteConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.OxyRock.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x001644C0 File Offset: 0x001626C0
	public GameObject CreatePrefab()
	{
		GameObject prefab = PuftOxyliteConfig.CreatePuftOxylite("PuftOxylite", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "puft_kanim", false);
		string eggId = "PuftOxyliteEgg";
		string eggName = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC;
		string egg_anim = "egg_puft_kanim";
		float egg_MASS = PuftTuning.EGG_MASS;
		string baby_id = "PuftOxyliteBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_OXYLITE = PuftTuning.EGG_CHANCES_OXYLITE;
		int egg_SORT_ORDER = PuftOxyliteConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_OXYLITE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x000AA361 File Offset: 0x000A8561
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006B5 RID: 1717
	public const string ID = "PuftOxylite";

	// Token: 0x040006B6 RID: 1718
	public const string BASE_TRAIT_ID = "PuftOxyliteBaseTrait";

	// Token: 0x040006B7 RID: 1719
	public const string EGG_ID = "PuftOxyliteEgg";

	// Token: 0x040006B8 RID: 1720
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x040006B9 RID: 1721
	public const SimHashes EMIT_ELEMENT = SimHashes.OxyRock;

	// Token: 0x040006BA RID: 1722
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040006BB RID: 1723
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftOxyliteConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040006BC RID: 1724
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040006BD RID: 1725
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 2;
}
