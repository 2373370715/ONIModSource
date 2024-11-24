using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ForestTreeConfig : IEntityConfig
{
	public const string ID = "ForestTree";

	public const string SEED_ID = "ForestTreeSeed";

	public const float FERTILIZATION_RATE = 1f / 60f;

	public const float WATER_RATE = 7f / 60f;

	public const float BRANCH_GROWTH_TIME = 2100f;

	public const int NUM_BRANCHES = 7;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("ForestTree", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME, STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC, 2f, decor: DECOR.BONUS.TIER1, anim: Assets.GetAnim("tree_kanim"), initialAnim: "idle_empty", sceneLayer: Grid.SceneLayer.Building, width: 1, height: 2, noise: default(EffectorValues), element: SimHashes.Creature, additionalTags: new List<Tag>(), defaultTemperature: 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 288.15f, 313.15f, 448.15f, null, pressure_sensitive: true, 0f, 0.15f, "WoodLog", can_drown: true, can_tinker: true, require_solid_tile: true, should_grow_old: false, 2400f, 0f, 9800f, "ForestTreeOriginal", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
		PlantBranchGrower.Def def = gameObject.AddOrGetDef<PlantBranchGrower.Def>();
		def.preventStartSMIOnSpawn = true;
		def.onBranchSpawned = RollChancesForSeed;
		def.onBranchHarvested = RollChancesForSeed;
		def.onEarlySpawn = TranslateOldBranchesToNewSystem;
		def.BRANCH_PREFAB_NAME = "ForestTreeBranch";
		def.harvestOnDrown = true;
		def.MAX_BRANCH_COUNT = 5;
		def.BRANCH_OFFSETS = new CellOffset[7]
		{
			new CellOffset(-1, 0),
			new CellOffset(-1, 1),
			new CellOffset(-1, 2),
			new CellOffset(0, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		};
		gameObject.AddOrGet<BuddingTrunk>();
		gameObject.AddOrGet<DirectlyEdiblePlant_TreeBranches>();
		gameObject.UpdateComponentRequirement<Harvestable>(required: false);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[1]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 7f / 60f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[1]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 1f / 60f
			}
		});
		gameObject.AddComponent<StandardCropPlant>().wiltsOnReadyToHarvest = true;
		gameObject.AddComponent<ForestTreeSeedMonitor>();
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Hidden, "ForestTreeSeed", STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.NAME, STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC, Assets.GetAnim("seed_tree_kanim"), "object", 1, new List<Tag> { GameTags.CropSeed }, SingleEntityReceptacle.ReceptacleDirection.Top, default(Tag), 4, STRINGS.CREATURES.SPECIES.WOOD_TREE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f), "ForestTree_preview", Assets.GetAnim("tree_kanim"), "place", 3, 3);
		return gameObject;
	}

	public void RollChancesForSeed(PlantBranch.Instance branch_smi, PlantBranchGrower.Instance trunk_smi)
	{
		trunk_smi.GetComponent<ForestTreeSeedMonitor>().TryRollNewSeed();
	}

	public void TranslateOldBranchesToNewSystem(PlantBranchGrower.Instance smi)
	{
		KPrefabID[] andForgetOldSerializedBranches = smi.GetComponent<BuddingTrunk>().GetAndForgetOldSerializedBranches();
		if (andForgetOldSerializedBranches != null)
		{
			smi.ManuallyDefineBranchArray(andForgetOldSerializedBranches);
		}
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
