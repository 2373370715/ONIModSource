﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
public class SweepBotStationConfig : IBuildingConfig
{
	// Token: 0x06001A7C RID: 6780 RVA: 0x001A82D8 File Offset: 0x001A64D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SweepBotStation";
		int width = 2;
		int height = 2;
		string anim = "sweep_bot_base_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0] - SweepBotConfig.MASS
		};
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		return buildingDef;
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x001A8374 File Offset: 0x001A6574
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.ignoreSourcePriority = true;
		storage.showDescriptor = false;
		storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.Building;
		storage.capacityKg = 25f;
		storage.allowClearable = false;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showInUI = true;
		storage2.allowItemRemoval = true;
		storage2.ignoreSourcePriority = true;
		storage2.showDescriptor = true;
		storage2.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
		storage2.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage2.fetchCategory = Storage.FetchCategory.StorageSweepOnly;
		storage2.capacityKg = 1000f;
		storage2.allowClearable = true;
		storage2.showCapacityStatusItem = true;
		go.AddOrGet<CharacterOverlay>().shouldShowName = true;
		go.AddOrGet<SweepBotStation>().SetStorages(storage, storage2);
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000A6A93 File Offset: 0x000A4C93
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
	}

	// Token: 0x040010D3 RID: 4307
	public const string ID = "SweepBotStation";

	// Token: 0x040010D4 RID: 4308
	public const float POWER_USAGE = 240f;
}
