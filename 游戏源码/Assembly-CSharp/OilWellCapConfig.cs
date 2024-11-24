using System;
using TUNING;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class OilWellCapConfig : IBuildingConfig
{
	// Token: 0x06001591 RID: 5521 RVA: 0x00194388 File Offset: 0x00192588
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OilWellCap";
		int width = 4;
		int height = 4;
		string anim = "geyser_oil_cap_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 1);
		buildingDef.PowerInputOffset = new CellOffset(1, 1);
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AttachmentSlotTag = GameTags.OilWell;
		buildingDef.BuildLocationRule = BuildLocationRule.BuildingAttachPoint;
		buildingDef.ObjectLayer = ObjectLayer.AttachableBuilding;
		return buildingDef;
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x00194460 File Offset: 0x00192660
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 2f;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.capacityTag = OilWellCapConfig.INPUT_WATER_TAG;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(OilWellCapConfig.INPUT_WATER_TAG, 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(3.3333333f, SimHashes.CrudeOil, 363.15f, false, false, 2f, 1.5f, 0f, byte.MaxValue, 0, true)
		};
		OilWellCap oilWellCap = go.AddOrGet<OilWellCap>();
		oilWellCap.gasElement = SimHashes.Methane;
		oilWellCap.gasTemperature = 573.15f;
		oilWellCap.addGasRate = 0.033333335f;
		oilWellCap.maxGasPressure = 80.00001f;
		oilWellCap.releaseGasRate = 0.44444448f;
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x000A6D53 File Offset: 0x000A4F53
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000EAB RID: 3755
	private const float WATER_INTAKE_RATE = 1f;

	// Token: 0x04000EAC RID: 3756
	private const float WATER_TO_OIL_RATIO = 3.3333333f;

	// Token: 0x04000EAD RID: 3757
	private const float LIQUID_STORAGE = 10f;

	// Token: 0x04000EAE RID: 3758
	private const float GAS_RATE = 0.033333335f;

	// Token: 0x04000EAF RID: 3759
	private const float OVERPRESSURE_TIME = 2400f;

	// Token: 0x04000EB0 RID: 3760
	private const float PRESSURE_RELEASE_TIME = 180f;

	// Token: 0x04000EB1 RID: 3761
	private const float PRESSURE_RELEASE_RATE = 0.44444448f;

	// Token: 0x04000EB2 RID: 3762
	private static readonly Tag INPUT_WATER_TAG = SimHashes.Water.CreateTag();

	// Token: 0x04000EB3 RID: 3763
	public const string ID = "OilWellCap";
}
