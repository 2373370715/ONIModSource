using System;
using TUNING;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class IceMachineConfig : IBuildingConfig
{
	// Token: 0x06000EFB RID: 3835 RVA: 0x0017B934 File Offset: 0x00179B34
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceMachine";
		int width = 2;
		int height = 3;
		string anim = "freezerator_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = this.energyConsumption;
		buildingDef.ExhaustKilowattsWhenActive = 4f;
		buildingDef.SelfHeatKilowattsWhenActive = 12f;
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x0017B9BC File Offset: 0x00179BBC
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

	// Token: 0x06000EFD RID: 3837 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000ACE RID: 2766
	public const string ID = "IceMachine";

	// Token: 0x04000ACF RID: 2767
	private const float WATER_STORAGE = 60f;

	// Token: 0x04000AD0 RID: 2768
	private const float ICE_STORAGE = 300f;

	// Token: 0x04000AD1 RID: 2769
	private const float WATER_INPUT_RATE = 0.5f;

	// Token: 0x04000AD2 RID: 2770
	private const float ICE_OUTPUT_RATE = 0.5f;

	// Token: 0x04000AD3 RID: 2771
	private const float ICE_PER_LOAD = 30f;

	// Token: 0x04000AD4 RID: 2772
	private const float TARGET_ICE_TEMP = 253.15f;

	// Token: 0x04000AD5 RID: 2773
	private const float KDTU_TRANSFER_RATE = 80f;

	// Token: 0x04000AD6 RID: 2774
	private const float THERMAL_CONSERVATION = 0.2f;

	// Token: 0x04000AD7 RID: 2775
	private float energyConsumption = 240f;

	// Token: 0x04000AD8 RID: 2776
	public static Tag[] ELEMENT_OPTIONS = new Tag[]
	{
		SimHashes.Ice.CreateTag(),
		SimHashes.Snow.CreateTag()
	};
}
