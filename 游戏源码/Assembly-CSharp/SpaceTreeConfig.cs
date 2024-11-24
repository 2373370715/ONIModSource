using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class SpaceTreeConfig : IEntityConfig
{
	public const string ID = "SpaceTree";

	public const string SEED_ID = "SpaceTreeSeed";

	public const float Temperature_lethal_low = 173.15f;

	public const float Temperature_warning_low = 198.15f;

	public const float Temperature_warning_high = 258.15f;

	public const float Temperature_lethal_high = 293.15f;

	public const float SNOW_RATE = 1f / 6f;

	public const float ENTOMB_DEFENSE_COOLDOWN = 5f;

	public static CellOffset OUTPUT_CONDUIT_CELL_OFFSET = new CellOffset(0, 1);

	public const float TRUNK_GROWTH_DURATION = 2700f;

	public const int MAX_BRANCH_NUMBER = 5;

	public const int OPTIMAL_LUX = 10000;

	public const float MIN_REQUIRED_LIGHT_TO_GROW_BRANCHES = 300f;

	public const float SUGAR_WATER_PRODUCTION_DURATION = 150f;

	public const float SUGAR_WATER_CAPACITY = 20f;

	public const string MANUAL_HARVEST_PRE_ANIM_NAME = "syrup_harvest_trunk_pre";

	public const string MANUAL_HARVEST_LOOP_ANIM_NAME = "syrup_harvest_trunk_loop";

	public const string MANUAL_HARVEST_PST_ANIM_NAME = "syrup_harvest_trunk_pst";

	public const string MANUAL_HARVEST_INTERRUPT_ANIM_NAME = "syrup_harvest_trunk_loop";

	private static readonly List<Storage.StoredItemModifier> storedItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("SpaceTree", STRINGS.CREATURES.SPECIES.SPACETREE.NAME, STRINGS.CREATURES.SPECIES.SPACETREE.DESC, 1f, decor: DECOR.PENALTY.TIER1, anim: Assets.GetAnim("syrup_tree_kanim"), initialAnim: "idle_empty", sceneLayer: Grid.SceneLayer.BuildingBack, width: 1, height: 2, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: null, defaultTemperature: 255f);
		string text = "SpaceTreeOriginal";
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 173.15f, 198.15f, 258.15f, 293.15f, new SimHashes[5]
		{
			SimHashes.Oxygen,
			SimHashes.ContaminatedOxygen,
			SimHashes.CarbonDioxide,
			SimHashes.Snow,
			SimHashes.Vacuum
		}, pressure_sensitive: false, 0f, 0.15f, null, can_drown: true, can_tinker: false, require_solid_tile: true, should_grow_old: false, 2400f, 0f, 12200f, text, STRINGS.CREATURES.SPECIES.SPACETREE.NAME);
		WiltCondition component = gameObject.GetComponent<WiltCondition>();
		component.WiltDelay = 0f;
		component.RecoveryDelay = 0f;
		Modifiers component2 = gameObject.GetComponent<Modifiers>();
		if (gameObject.GetComponent<Traits>() == null)
		{
			gameObject.AddOrGet<Traits>();
			component2.initialTraits.Add(text);
		}
		Crop.CropVal cropval = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == SimHashes.SugarWater.CreateTag());
		Trait trait = Db.Get().traits.Get(component2.initialTraits[0]);
		component2.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, 4.5f, STRINGS.CREATURES.SPECIES.SPACETREE.NAME));
		gameObject.AddOrGet<Crop>().Configure(cropval);
		KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component3.PrefabID().ToString());
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			gameObject.AddOrGet<MutantPlant>().SpeciesID = component3.PrefabTag;
			SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		}
		Growing growing = gameObject.AddOrGet<Growing>();
		growing.shouldGrowOld = false;
		growing.maxAge = 2400f;
		gameObject.AddOrGet<HarvestDesignatable>();
		gameObject.AddOrGet<LoopingSounds>();
		GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "SpaceTreeSeed", STRINGS.CREATURES.SPECIES.SEEDS.SPACETREE.NAME, STRINGS.CREATURES.SPECIES.SEEDS.SPACETREE.DESC, Assets.GetAnim("seed_syrup_tree_kanim"), "object", 1, new List<Tag> { GameTags.CropSeed }, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 1, STRINGS.CREATURES.SPECIES.SPACETREE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", ignoreDefaultSeedTag: false, GetDlcIds());
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[1]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = SimHashes.Snow.CreateTag(),
				massConsumptionRate = 1f / 6f
			}
		});
		EntityTemplates.CreateAndRegisterPreviewForPlant(seed, "SpaceTree_preview", Assets.GetAnim("syrup_tree_kanim"), "place", 1, 2);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
		DirectlyEdiblePlant_StorageElement directlyEdiblePlant_StorageElement = gameObject.AddOrGet<DirectlyEdiblePlant_StorageElement>();
		directlyEdiblePlant_StorageElement.tagToConsume = SimHashes.SugarWater.CreateTag();
		directlyEdiblePlant_StorageElement.rateProducedPerCycle = 4f;
		directlyEdiblePlant_StorageElement.storageCapacity = 20f;
		directlyEdiblePlant_StorageElement.edibleCellOffsets = new CellOffset[4]
		{
			new CellOffset(-1, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 1),
			new CellOffset(1, 1)
		};
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showInUI = true;
		storage.capacityKg = 20f;
		storage.SetDefaultStoredItemModifiers(storedItemModifiers);
		ConduitDispenser conduitDispenser = gameObject.AddOrGet<ConduitDispenser>();
		conduitDispenser.noBuildingOutputCellOffset = OUTPUT_CONDUIT_CELL_OFFSET;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.SetOnState(onState: false);
		gameObject.AddOrGet<SpaceTreeSyrupHarvestWorkable>();
		UnstableEntombDefense.Def def = gameObject.AddOrGetDef<UnstableEntombDefense.Def>();
		def.defaultAnimName = "shake_trunk";
		def.Cooldown = 5f;
		PlantBranchGrower.Def def2 = gameObject.AddOrGetDef<PlantBranchGrower.Def>();
		def2.BRANCH_OFFSETS = new CellOffset[5]
		{
			new CellOffset(-1, 1),
			new CellOffset(-1, 2),
			new CellOffset(0, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		};
		def2.BRANCH_PREFAB_NAME = "SpaceTreeBranch";
		def2.harvestOnDrown = true;
		def2.propagateHarvestDesignation = false;
		def2.MAX_BRANCH_COUNT = 5;
		SpaceTreePlant.Def def3 = gameObject.AddOrGetDef<SpaceTreePlant.Def>();
		def3.OptimalProductionDuration = 150f;
		def3.OptimalAmountOfBranches = 5;
		return gameObject;
	}

	public void OnPrefabInit(GameObject prefab)
	{
	}

	public void OnSpawn(GameObject inst)
	{
		EntityCellVisualizer entityCellVisualizer = inst.AddOrGet<EntityCellVisualizer>();
		entityCellVisualizer.AddPort(EntityCellVisualizer.Ports.LiquidOut, OUTPUT_CONDUIT_CELL_OFFSET, entityCellVisualizer.Resources.liquidIOColours.output.connected);
	}
}
