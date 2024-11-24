using System;
using TUNING;
using UnityEngine;

// Token: 0x020003C5 RID: 965
public class LiquidCooledFanConfig : IBuildingConfig
{
	// Token: 0x06001007 RID: 4103 RVA: 0x0017F2DC File Offset: 0x0017D4DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidCooledFan";
		int width = 2;
		int height = 2;
		string anim = "fanliquid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0017F378 File Offset: 0x0017D578
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 100f;
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		float num = 580f * (DUPLICANTSTATS.STANDARD.BaseStats.KCAL2JOULES / 1000f);
		float num2 = 0.01f;
		LiquidCooledFan liquidCooledFan = go.AddOrGet<LiquidCooledFan>();
		liquidCooledFan.gasStorage = storage;
		liquidCooledFan.liquidStorage = storage2;
		liquidCooledFan.waterKGConsumedPerKJ = 1f / (num * num2);
		liquidCooledFan.coolingKilowatts = 80f;
		liquidCooledFan.minCooledTemperature = 290f;
		liquidCooledFan.minEnvironmentMass = 0.25f;
		liquidCooledFan.minCoolingRange = new Vector2I(-2, 0);
		liquidCooledFan.maxCoolingRange = new Vector2I(2, 4);
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.RequestedItemTag = new Tag("Water");
		manualDeliveryKG.capacity = 500f;
		manualDeliveryKG.refillMass = 50f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.storeOnConsume = true;
		elementConsumer.storage = storage;
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.consumptionRadius = 8;
		elementConsumer.EnableConsumption(true);
		elementConsumer.sampleCellOffset = new Vector3(0f, 0f);
		elementConsumer.showDescriptor = false;
		LiquidCooledFanWorkable liquidCooledFanWorkable = go.AddOrGet<LiquidCooledFanWorkable>();
		liquidCooledFanWorkable.SetWorkTime(20f);
		liquidCooledFanWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_liquidfan_kanim")
		};
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000B55 RID: 2901
	public const string ID = "LiquidCooledFan";
}
