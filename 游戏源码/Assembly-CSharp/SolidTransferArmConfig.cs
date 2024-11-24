using System;
using TUNING;
using UnityEngine;

// Token: 0x0200059A RID: 1434
public class SolidTransferArmConfig : IBuildingConfig
{
	// Token: 0x06001973 RID: 6515 RVA: 0x001A31F0 File Offset: 0x001A13F0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SolidTransferArm", 3, 1, "conveyor_transferarm_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidTransferArm");
		return buildingDef;
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000B0B50 File Offset: 0x000AED50
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000B0B60 File Offset: 0x000AED60
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, true);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000B0B69 File Offset: 0x000AED69
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		SolidTransferArmConfig.AddVisualizer(go, false);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000B0B91 File Offset: 0x000AED91
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<SolidTransferArm>().pickupRange = 4;
		SolidTransferArmConfig.AddVisualizer(go, false);
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x001A3298 File Offset: 0x001A1498
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.RangeMin.x = -4;
		rangeVisualizer.RangeMin.y = -4;
		rangeVisualizer.RangeMax.x = 4;
		rangeVisualizer.RangeMax.y = 4;
		rangeVisualizer.BlockingTileVisible = true;
	}

	// Token: 0x04001032 RID: 4146
	public const string ID = "SolidTransferArm";

	// Token: 0x04001033 RID: 4147
	private const int RANGE = 4;
}
