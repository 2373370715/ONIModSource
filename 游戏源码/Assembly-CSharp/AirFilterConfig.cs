using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class AirFilterConfig : IBuildingConfig
{
	// Token: 0x06000072 RID: 114 RVA: 0x0013F7E8 File Offset: 0x0013D9E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AirFilter";
		int width = 1;
		int height = 1;
		string anim = "co2filter_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 5f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		return buildingDef;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0013F880 File Offset: 0x0013DA80
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.elementToConsume = SimHashes.ContaminatedOxygen;
		elementConsumer.consumptionRate = 0.5f;
		elementConsumer.capacityKG = 0.5f;
		elementConsumer.consumptionRadius = 3;
		elementConsumer.showInStatusPanel = true;
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
		elementConsumer.isRequired = false;
		elementConsumer.storeOnConsume = true;
		elementConsumer.showDescriptor = false;
		elementConsumer.ignoreActiveChanged = true;
		ElementDropper elementDropper = go.AddComponent<ElementDropper>();
		elementDropper.emitMass = 10f;
		elementDropper.emitTag = new Tag("Clay");
		elementDropper.emitOffset = new Vector3(0f, 0f, 0f);
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Filter"), 0.13333334f, true),
			new ElementConverter.ConsumedElement(new Tag("ContaminatedOxygen"), 0.1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.14333335f, SimHashes.Clay, 0f, false, true, 0f, 0.5f, 0.25f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.089999996f, SimHashes.Oxygen, 0f, false, false, 0f, 0f, 0.75f, byte.MaxValue, 0, true)
		};
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("Filter");
		manualDeliveryKG.capacity = 320.00003f;
		manualDeliveryKG.refillMass = 32.000004f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		go.AddOrGet<AirFilter>().filterTag = new Tag("Filter");
		go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000A5FC5 File Offset: 0x000A41C5
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<ActiveController.Def>();
	}

	// Token: 0x04000051 RID: 81
	public const string ID = "AirFilter";

	// Token: 0x04000052 RID: 82
	public const float DIRTY_AIR_CONSUMPTION_RATE = 0.1f;

	// Token: 0x04000053 RID: 83
	private const float SAND_CONSUMPTION_RATE = 0.13333334f;

	// Token: 0x04000054 RID: 84
	private const float REFILL_RATE = 2400f;

	// Token: 0x04000055 RID: 85
	private const float SAND_STORAGE_AMOUNT = 320.00003f;

	// Token: 0x04000056 RID: 86
	private const float CLAY_PER_LOAD = 10f;
}
