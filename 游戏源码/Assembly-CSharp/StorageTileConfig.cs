using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class StorageTileConfig : IBuildingConfig
{
	public const string ANIM_NAME = "storagetile_kanim";

	public const string ID = "StorageTile";

	public static float CAPACITY = 1000f;

	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal,
		Storage.StoredItemModifier.Hide
	};

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("StorageTile", 1, 1, "storagetile_kanim", 30, 30f, new float[2] { 100f, 100f }, new string[2] { "RefinedMetal", "Glass" }, 800f, BuildLocationRule.Tile, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER1);
		BuildingTemplates.CreateFoundationTileDef(obj);
		obj.Floodable = false;
		obj.Entombable = false;
		obj.Overheatable = false;
		obj.UseStructureTemperature = false;
		obj.AudioCategory = "Glass";
		obj.AudioSize = "small";
		obj.BaseTimeUntilRepair = -1f;
		obj.SceneLayer = Grid.SceneLayer.TileMain;
		obj.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.PENALTY_2;
		simCellOccupier.notifyOnMelt = true;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(StoredItemModifiers);
		storage.capacityKg = CAPACITY;
		storage.showInUI = true;
		storage.allowItemRemoval = true;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<StorageTileSwitchItemWorkable>();
		TreeFilterable treeFilterable = go.AddOrGet<TreeFilterable>();
		treeFilterable.copySettingsEnabled = false;
		treeFilterable.dropIncorrectOnFilterChange = false;
		treeFilterable.preventAutoAddOnDiscovery = true;
		StorageTile.Def def = go.AddOrGetDef<StorageTile.Def>();
		def.MaxCapacity = CAPACITY;
		def.specialItemCases = new StorageTile.SpecificItemTagSizeInstruction[2]
		{
			new StorageTile.SpecificItemTagSizeInstruction(GameTags.AirtightSuit, 0.5f),
			new StorageTile.SpecificItemTagSizeInstruction(GameTags.Dehydrated, 0.6f)
		};
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		Prioritizable.AddRef(go);
		go.AddOrGetDef<RocketUsageRestriction.Def>().restrictOperational = false;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);
	}
}
