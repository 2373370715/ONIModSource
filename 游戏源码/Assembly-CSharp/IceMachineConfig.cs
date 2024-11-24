using TUNING;
using UnityEngine;

public class IceMachineConfig : IBuildingConfig
{
	public const string ID = "IceMachine";

	private const float WATER_STORAGE = 60f;

	private const float ICE_STORAGE = 300f;

	private const float WATER_INPUT_RATE = 0.5f;

	private const float ICE_OUTPUT_RATE = 0.5f;

	private const float ICE_PER_LOAD = 30f;

	private const float TARGET_ICE_TEMP = 253.15f;

	private const float KDTU_TRANSFER_RATE = 80f;

	private const float THERMAL_CONSERVATION = 0.2f;

	private float energyConsumption = 240f;

	public static Tag[] ELEMENT_OPTIONS = new Tag[2]
	{
		SimHashes.Ice.CreateTag(),
		SimHashes.Snow.CreateTag()
	};

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("IceMachine", 2, 3, "freezerator_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.NONE);
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = energyConsumption;
		obj.ExhaustKilowattsWhenActive = 4f;
		obj.SelfHeatKilowattsWhenActive = 12f;
		obj.ViewMode = OverlayModes.Temperature.ID;
		obj.AudioCategory = "Metal";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage.showInUI = true;
		storage.capacityKg = 60f;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage2.showInUI = true;
		storage2.capacityKg = 300f;
		storage2.allowItemRemoval = true;
		storage2.ignoreSourcePriority = true;
		storage2.allowUIItemRemoval = true;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		IceMachine iceMachine = go.AddOrGet<IceMachine>();
		iceMachine.SetStorages(storage, storage2);
		iceMachine.targetTemperature = 253.15f;
		iceMachine.heatRemovalRate = 80f;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTags.Water;
		manualDeliveryKG.capacity = 60f;
		manualDeliveryKG.refillMass = 12f;
		manualDeliveryKG.MinimumMass = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
