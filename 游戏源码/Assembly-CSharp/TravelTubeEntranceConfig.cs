using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020005D3 RID: 1491
public class TravelTubeEntranceConfig : IBuildingConfig
{
	// Token: 0x06001ABF RID: 6847 RVA: 0x001A91B8 File Offset: 0x001A73B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TravelTubeEntrance";
		int width = 3;
		int height = 2;
		string anim = "tube_launcher_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		return buildingDef;
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x001A9248 File Offset: 0x001A7448
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		TravelTubeEntrance travelTubeEntrance = go.AddOrGet<TravelTubeEntrance>();
		travelTubeEntrance.waxPerLaunch = 0.05f;
		travelTubeEntrance.joulesPerLaunch = 10000f;
		travelTubeEntrance.jouleCapacity = 40000f;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		List<Storage.StoredItemModifier> defaultStoredItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Preserve
		};
		storage.SetDefaultStoredItemModifiers(defaultStoredItemModifiers);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.requestedItemTag = SimHashes.MilkFat.CreateTag();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.refillMass = 0.05f;
		manualDeliveryKG.SetStorage(storage);
		go.AddOrGet<TravelTubeEntrance.Work>();
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<EnergyConsumerSelfSustaining>();
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000B1616 File Offset: 0x000AF816
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
	}

	// Token: 0x040010F5 RID: 4341
	public const string ID = "TravelTubeEntrance";

	// Token: 0x040010F6 RID: 4342
	public const float WAX_PER_LAUNCH = 0.05f;

	// Token: 0x040010F7 RID: 4343
	public const int STORAGE_WAX_LAUNCHECOUNT_CAPACITY = 200;

	// Token: 0x040010F8 RID: 4344
	private const float JOULES_PER_LAUNCH = 10000f;

	// Token: 0x040010F9 RID: 4345
	private const float LAUNCHES_FROM_FULL_CHARGE = 4f;
}
