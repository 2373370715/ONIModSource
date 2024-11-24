using System;
using TUNING;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class AutoMinerConfig : IBuildingConfig
{
	// Token: 0x0600009D RID: 157 RVA: 0x00140AA4 File Offset: 0x0013ECA4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AutoMiner", 2, 2, "auto_miner_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "AutoMiner");
		return buildingDef;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000A5FF1 File Offset: 0x000A41F1
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<MiningSounds>();
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000A6019 File Offset: 0x000A4219
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, true);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000A6022 File Offset: 0x000A4222
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00140B4C File Offset: 0x0013ED4C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
		autoMiner.x = -7;
		autoMiner.y = 0;
		autoMiner.width = 16;
		autoMiner.height = 9;
		autoMiner.vision_offset = new CellOffset(0, 1);
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00140B98 File Offset: 0x0013ED98
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -7;
		rangeVisualizer.RangeMin.y = -1;
		rangeVisualizer.RangeMax.x = 8;
		rangeVisualizer.RangeMax.y = 7;
		rangeVisualizer.OriginOffset = new Vector2I(0, 1);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(AutoMiner.DigBlockingCB);
		};
	}

	// Token: 0x04000077 RID: 119
	public const string ID = "AutoMiner";

	// Token: 0x04000078 RID: 120
	private const int RANGE = 7;

	// Token: 0x04000079 RID: 121
	private const int X = -7;

	// Token: 0x0400007A RID: 122
	private const int Y = 0;

	// Token: 0x0400007B RID: 123
	private const int WIDTH = 16;

	// Token: 0x0400007C RID: 124
	private const int HEIGHT = 9;

	// Token: 0x0400007D RID: 125
	private const int VISION_OFFSET = 1;
}
