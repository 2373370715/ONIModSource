using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class GasBottlerConfig : IBuildingConfig
{
	// Token: 0x06000DA5 RID: 3493 RVA: 0x00174378 File Offset: 0x00172578
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GasBottler", 3, 2, "gas_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasBottler");
		return buildingDef;
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x001743FC File Offset: 0x001725FC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(GasBottlerConfig.GasBottlerStoredItemModifiers);
		storage.allowItemRemoval = false;
		go.AddTag(GameTags.GasSource);
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.removeTags = new List<Tag>
		{
			GameTags.GasSource
		};
		dropAllWorkable.resetTargetWorkableOnCompleteWork = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.storage = storage;
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = 200f;
		conduitConsumer.keepZeroMassObject = false;
		Bottler bottler = go.AddOrGet<Bottler>();
		bottler.storage = storage;
		bottler.workTime = 9f;
		bottler.userMaxCapacity = 25f;
		bottler.consumer = conduitConsumer;
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x000ABFF8 File Offset: 0x000AA1F8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x040009CE RID: 2510
	public const string ID = "GasBottler";

	// Token: 0x040009CF RID: 2511
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x040009D0 RID: 2512
	private const int WIDTH = 3;

	// Token: 0x040009D1 RID: 2513
	private const int HEIGHT = 2;

	// Token: 0x040009D2 RID: 2514
	private const float DEFAULT_FILL_LEVEL = 25f;

	// Token: 0x040009D3 RID: 2515
	private const float CAPACITY = 200f;

	// Token: 0x040009D4 RID: 2516
	private static readonly List<Storage.StoredItemModifier> GasBottlerStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};
}
