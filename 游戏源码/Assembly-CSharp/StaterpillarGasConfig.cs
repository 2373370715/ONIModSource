using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class StaterpillarGasConfig : IEntityConfig
{
	public const string ID = "StaterpillarGas";

	public const string BASE_TRAIT_ID = "StaterpillarGasBaseTrait";

	public const string EGG_ID = "StaterpillarGasEgg";

	public const int EGG_SORT_ORDER = 1;

	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;

	private static float STORAGE_CAPACITY = 100f;

	private static float COOLDOWN_MIN = 20f;

	private static float COOLDOWN_MAX = 40f;

	private static float CONSUMPTION_RATE = 0.5f;

	private static float INHALE_TIME = 6f;

	public static GameObject CreateStaterpillarGas(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "gas_consume_pre",
			inhaleAnimLoop = "gas_consume_loop",
			inhaleAnimPst = "gas_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas
		};
		GameObject prefab = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarGasBaseTrait", is_baby, ObjectLayer.GasConduit, StaterpillarGasConnectorConfig.ID, GameTags.Unbreathable, "gas_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef);
		prefab = EntityTemplates.ExtendEntityToWildCreature(prefab, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Unbreathable;
			def.transitionTag = new Tag[1] { GameTags.Creature };
			def.minCooldown = COOLDOWN_MIN;
			def.maxCooldown = COOLDOWN_MAX;
			def.consumptionRate = CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarGasBaseTrait", name, name, null, should_save: false, null, positive_trait: true, is_valid_starter_trait: true);
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
		return EntityTemplates.ExtendEntityToFertileCreature(CreateStaterpillarGas("StaterpillarGas", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "caterpillar_kanim", is_baby: false), "StaterpillarGasEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarGasBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_GAS, GetDlcIds(), 1);
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
