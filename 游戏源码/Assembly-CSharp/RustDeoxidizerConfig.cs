using System;
using TUNING;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class RustDeoxidizerConfig : IBuildingConfig
{
	// Token: 0x06001891 RID: 6289 RVA: 0x0019FCB4 File Offset: 0x0019DEB4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RustDeoxidizer";
		int width = 2;
		int height = 3;
		string anim = "rust_deoxidizer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		return buildingDef;
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x0019FD58 File Offset: 0x0019DF58
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<RustDeoxidizer>().maxMass = 1.8f;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("Rust");
		manualDeliveryKG.capacity = 585f;
		manualDeliveryKG.refillMass = 193.05f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG2.SetStorage(storage);
		manualDeliveryKG2.RequestedItemTag = new Tag("Salt");
		manualDeliveryKG2.capacity = 195f;
		manualDeliveryKG2.refillMass = 64.350006f;
		manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Rust"), 0.75f, true),
			new ElementConverter.ConsumedElement(new Tag("Salt"), 0.25f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.57f, SimHashes.Oxygen, 348.15f, false, false, 0f, 1f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.029999971f, SimHashes.ChlorineGas, 348.15f, false, false, 0f, 1f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.4f, SimHashes.IronOre, 348.15f, false, true, 0f, 1f, 1f, byte.MaxValue, 0, true)
		};
		ElementDropper elementDropper = go.AddComponent<ElementDropper>();
		elementDropper.emitMass = 24f;
		elementDropper.emitTag = SimHashes.IronOre.CreateTag();
		elementDropper.emitOffset = new Vector3(0f, 1f, 0f);
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000A5FB5 File Offset: 0x000A41B5
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000FE2 RID: 4066
	public const string ID = "RustDeoxidizer";

	// Token: 0x04000FE3 RID: 4067
	private const float RUST_KG_CONSUMPTION_RATE = 0.75f;

	// Token: 0x04000FE4 RID: 4068
	private const float SALT_KG_CONSUMPTION_RATE = 0.25f;

	// Token: 0x04000FE5 RID: 4069
	private const float RUST_KG_PER_REFILL = 585f;

	// Token: 0x04000FE6 RID: 4070
	private const float SALT_KG_PER_REFILL = 195f;

	// Token: 0x04000FE7 RID: 4071
	private const float TOTAL_CONSUMPTION_RATE = 1f;

	// Token: 0x04000FE8 RID: 4072
	private const float IRON_CONVERSION_RATIO = 0.4f;

	// Token: 0x04000FE9 RID: 4073
	private const float OXYGEN_CONVERSION_RATIO = 0.57f;

	// Token: 0x04000FEA RID: 4074
	private const float CHLORINE_CONVERSION_RATIO = 0.029999971f;

	// Token: 0x04000FEB RID: 4075
	public const float OXYGEN_TEMPERATURE = 348.15f;
}
