using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class StaterpillarLiquidConfig : IEntityConfig
{
	public const string ID = "StaterpillarLiquid";

	public const string BASE_TRAIT_ID = "StaterpillarLiquidBaseTrait";

	public const string EGG_ID = "StaterpillarLiquidEgg";

	public const int EGG_SORT_ORDER = 2;

	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;

	private static float STORAGE_CAPACITY = 1000f;

	private static float COOLDOWN_MIN = 20f;

	private static float COOLDOWN_MAX = 40f;

	private static float CONSUMPTION_RATE = 10f;

	private static float INHALE_TIME = 6f;

	public static GameObject CreateStaterpillarLiquid(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "liquid_consume_pre",
			inhaleAnimLoop = "liquid_consume_loop",
			inhaleAnimPst = "liquid_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForLiquid
		};
		GameObject prefab = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarLiquidBaseTrait", is_baby, ObjectLayer.LiquidConduit, StaterpillarLiquidConnectorConfig.ID, GameTags.Unbreathable, "wtr_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef);
		prefab = EntityTemplates.ExtendEntityToWildCreature(prefab, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Liquid;
			def.transitionTag = new Tag[1] { GameTags.Creature };
			def.minCooldown = COOLDOWN_MIN;
			def.maxCooldown = COOLDOWN_MAX;
			def.consumptionRate = CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarLiquidBaseTrait", name, name, null, should_save: false, null, positive_trait: true, is_valid_starter_trait: true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (0f - StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE) / 600f, UI.TOOLTIPS.BASE_VALUE));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		prefab = BaseStaterpillarConfig.SetupDiet(prefab, list);
		Storage storage = prefab.AddComponent<Storage>();
		storage.capacityKg = STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		return prefab;
	}

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(CreateStaterpillarLiquid("StaterpillarLiquid", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "caterpillar_kanim", is_baby: false), "StaterpillarLiquidEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarLiquidBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_LIQUID, GetDlcIds(), 2);
	}

	public void OnPrefabInit(GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("electric_bolt_c_bloom", is_visible: false);
		component.SetSymbolVisiblity("gulp", is_visible: false);
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
