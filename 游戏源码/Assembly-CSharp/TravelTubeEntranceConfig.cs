using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class TravelTubeEntranceConfig : IBuildingConfig
{
	public const string ID = "TravelTubeEntrance";

	public const float WAX_PER_LAUNCH = 0.05f;

	public const int STORAGE_WAX_LAUNCHECOUNT_CAPACITY = 200;

	private const float JOULES_PER_LAUNCH = 10000f;

	private const float LAUNCHES_FROM_FULL_CHARGE = 4f;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("TravelTubeEntrance", 3, 2, "tube_launcher_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER1);
		obj.Overheatable = false;
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 960f;
		obj.Entombable = true;
		obj.AudioCategory = "Metal";
		obj.PowerInputOffset = new CellOffset(1, 0);
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		return obj;
	}

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

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
	}
}
