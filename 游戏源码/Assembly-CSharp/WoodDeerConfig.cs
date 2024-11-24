using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class WoodDeerConfig : IEntityConfig
{
	public const string ID = "WoodDeer";

	public const string BASE_TRAIT_ID = "WoodDeerBaseTrait";

	public const string EGG_ID = "WoodDeerEgg";

	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	public const float CALORIES_PER_PLANT_BITE = 100000f;

	public const float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.2f;

	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == "HardSkinBerry").cropDuration / 600f;

	public static float KG_PLANT_EATEN_A_DAY = 0.2f * CONSUMABLE_PLANT_MATURITY_LEVELS;

	public static float CALORIES_PER_KG = 100000f / KG_PLANT_EATEN_A_DAY;

	public static float ANTLER_GROWTH_TIME_IN_CYCLES = 6f;

	public static float ANTLER_STARTING_GROWTH_PCT = 0.5f;

	public static float WOOD_PER_CYCLE = 60f;

	public static float WOOD_MASS_PER_ANTLER = WOOD_PER_CYCLE * ANTLER_GROWTH_TIME_IN_CYCLES;

	private static float POOP_MASS_CONVERSION_MULTIPLIER = 8.333334f;

	private static float MIN_KG_CONSUMED_BEFORE_POOPING = 1f;

	public static int EGG_SORT_ORDER = 0;

	public static GameObject CreateWoodDeer(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDeerConfig.BaseDeer(id, name, desc, anim_file, "WoodDeerBaseTrait", is_baby), DeerTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("WoodDeerBaseTrait", name, name, null, should_save: false, null, positive_trait: true, is_valid_starter_trait: true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.66667f, UI.TOOLTIPS.BASE_VALUE));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
		List<Diet.Info> list = BaseDeerConfig.BasicDiet(SimHashes.Dirt.CreateTag(), CALORIES_PER_KG, POOP_MASS_CONVERSION_MULTIPLIER, null, 0f);
		list.Add(new Diet.Info(new HashSet<Tag> { "HardSkinBerry" }, SimHashes.Dirt.CreateTag(), CONSUMABLE_PLANT_MATURITY_LEVELS * CALORIES_PER_KG / 1f, POOP_MASS_CONVERSION_MULTIPLIER * 3f));
		GameObject gameObject = BaseDeerConfig.SetupDiet(prefab, list.ToArray(), MIN_KG_CONSUMED_BEFORE_POOPING);
		gameObject.AddTag(GameTags.OriginalCreature);
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "WoodDeerWellFed";
		def.caloriesPerCycle = 100000f;
		def.growthDurationCycles = ANTLER_GROWTH_TIME_IN_CYCLES;
		def.dropMass = WOOD_MASS_PER_ANTLER;
		def.itemDroppedOnShear = WoodLogConfig.TAG;
		def.levelCount = 6;
		return gameObject;
	}

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(CreateWoodDeer("WoodDeer", STRINGS.CREATURES.SPECIES.WOODDEER.NAME, STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "ice_floof_kanim", is_baby: false), "WoodDeerEgg", STRINGS.CREATURES.SPECIES.WOODDEER.EGG_NAME, STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "egg_ice_floof_kanim", DeerTuning.EGG_MASS, "WoodDeerBaby", 60.000004f, 20f, DeerTuning.EGG_CHANCES_BASE, eggSortOrder: EGG_SORT_ORDER, dlcIds: GetDlcIds());
	}

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
