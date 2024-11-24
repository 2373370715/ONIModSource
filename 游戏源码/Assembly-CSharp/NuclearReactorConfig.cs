using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class NuclearReactorConfig : IBuildingConfig
{
	// Token: 0x0600157A RID: 5498 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x001939FC File Offset: 0x00191BFC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NuclearReactor";
		int width = 5;
		int height = 6;
		string anim = "generatornuclear_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 0f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.ThermalConductivity = 0.1f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = false;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(-2, 2);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CONTROL_FUEL_DELIVERY", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_INACTIVE, false, true)
		};
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Breakable = false;
		buildingDef.Invincible = true;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x00193B5C File Offset: 0x00191D5C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		UnityEngine.Object.Destroy(go.GetComponent<BuildingEnabledButton>());
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 25;
		radiationEmitter.emitRadiusY = 25;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emissionOffset = new Vector3(0f, 2f, 0f);
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag;
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
		manualDeliveryKG.capacity = 180f;
		manualDeliveryKG.MinimumMass = 0.5f;
		go.AddOrGet<Reactor>();
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 90f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x000AFAFC File Offset: 0x000ADCFC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddTag(GameTags.CorrosionProof);
	}

	// Token: 0x04000E85 RID: 3717
	public const string ID = "NuclearReactor";

	// Token: 0x04000E86 RID: 3718
	private const float FUEL_CAPACITY = 180f;

	// Token: 0x04000E87 RID: 3719
	public const float VENT_STEAM_TEMPERATURE = 673.15f;

	// Token: 0x04000E88 RID: 3720
	public const float MELT_DOWN_TEMPERATURE = 3000f;

	// Token: 0x04000E89 RID: 3721
	public const float MAX_VENT_PRESSURE = 150f;

	// Token: 0x04000E8A RID: 3722
	public const float INCREASED_CONDUCTION_SCALE = 5f;

	// Token: 0x04000E8B RID: 3723
	public const float REACTION_STRENGTH = 100f;

	// Token: 0x04000E8C RID: 3724
	public const int RADIATION_EMITTER_RANGE = 25;

	// Token: 0x04000E8D RID: 3725
	public const float OPERATIONAL_RADIATOR_INTENSITY = 2400f;

	// Token: 0x04000E8E RID: 3726
	public const float MELT_DOWN_RADIATOR_INTENSITY = 4800f;

	// Token: 0x04000E8F RID: 3727
	public const float FUEL_CONSUMPTION_SPEED = 0.016666668f;

	// Token: 0x04000E90 RID: 3728
	public const float BEGIN_REACTION_MASS = 0.5f;

	// Token: 0x04000E91 RID: 3729
	public const float STOP_REACTION_MASS = 0.25f;

	// Token: 0x04000E92 RID: 3730
	public const float DUMP_WASTE_AMOUNT = 100f;

	// Token: 0x04000E93 RID: 3731
	public const float WASTE_MASS_MULTIPLIER = 100f;

	// Token: 0x04000E94 RID: 3732
	public const float REACTION_MASS_TARGET = 60f;

	// Token: 0x04000E95 RID: 3733
	public const float COOLANT_AMOUNT = 30f;

	// Token: 0x04000E96 RID: 3734
	public const float COOLANT_CAPACITY = 90f;

	// Token: 0x04000E97 RID: 3735
	public const float MINIMUM_COOLANT_MASS = 30f;

	// Token: 0x04000E98 RID: 3736
	public const float WASTE_GERMS_PER_KG = 50f;

	// Token: 0x04000E99 RID: 3737
	public const float PST_MELTDOWN_COOLING_TIME = 3000f;

	// Token: 0x04000E9A RID: 3738
	public const string INPUT_PORT_ID = "CONTROL_FUEL_DELIVERY";
}
