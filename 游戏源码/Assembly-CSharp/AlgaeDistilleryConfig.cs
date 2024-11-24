using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class AlgaeDistilleryConfig : IBuildingConfig
{
	// Token: 0x0600007A RID: 122 RVA: 0x0013FC78 File Offset: 0x0013DE78
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AlgaeDistillery";
		int width = 3;
		int height = 4;
		string anim = "algae_distillery_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return buildingDef;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0013FD38 File Offset: 0x0013DF38
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		AlgaeDistillery algaeDistillery = go.AddOrGet<AlgaeDistillery>();
		algaeDistillery.emitTag = new Tag("Algae");
		algaeDistillery.emitMass = 30f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.DirtyWater
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		Tag tag = SimHashes.SlimeMold.CreateTag();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = tag;
		manualDeliveryKG.refillMass = 120f;
		manualDeliveryKG.capacity = 480f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(tag, 0.6f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.2f, SimHashes.Algae, 303.15f, false, true, 0f, 1f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.40000004f, SimHashes.DirtyWater, 303.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000A5FB5 File Offset: 0x000A41B5
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000058 RID: 88
	public const string ID = "AlgaeDistillery";

	// Token: 0x04000059 RID: 89
	public const float INPUT_SLIME_PER_SECOND = 0.6f;

	// Token: 0x0400005A RID: 90
	public const float ALGAE_PER_SECOND = 0.2f;

	// Token: 0x0400005B RID: 91
	public const float DIRTY_WATER_PER_SECOND = 0.40000004f;

	// Token: 0x0400005C RID: 92
	public const float OUTPUT_TEMP = 303.15f;

	// Token: 0x0400005D RID: 93
	public const float REFILL_RATE = 2400f;

	// Token: 0x0400005E RID: 94
	public const float ALGAE_STORAGE_AMOUNT = 480f;
}
