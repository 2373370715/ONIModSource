using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class CometDetectorConfig : IBuildingConfig
{
	// Token: 0x0600016F RID: 367 RVA: 0x00144240 File Offset: 0x00142440
	public override BuildingDef CreateBuildingDef()
	{
		string id = CometDetectorConfig.ID;
		int width = 2;
		int height = 4;
		string anim = "meteor_detector_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = true;
		buildingDef.Entombable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.AddLogicPowerPort = false;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, CometDetectorConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00144358 File Offset: 0x00142558
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		if (DlcManager.IsExpansion1Active())
		{
			go.AddOrGetDef<ClusterCometDetector.Def>();
		}
		else
		{
			go.AddOrGetDef<CometDetector.Def>();
		}
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
		CometDetectorConfig.AddVisualizer(go);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x000A6590 File Offset: 0x000A4790
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		CometDetectorConfig.AddVisualizer(go);
	}

	// Token: 0x06000172 RID: 370 RVA: 0x000A6598 File Offset: 0x000A4798
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		CometDetectorConfig.AddVisualizer(go);
	}

	// Token: 0x06000173 RID: 371 RVA: 0x000A65A0 File Offset: 0x000A47A0
	private static void AddVisualizer(GameObject prefab)
	{
		ScannerNetworkVisualizer scannerNetworkVisualizer = prefab.AddOrGet<ScannerNetworkVisualizer>();
		scannerNetworkVisualizer.RangeMin = -15;
		scannerNetworkVisualizer.RangeMax = 15;
	}

	// Token: 0x040000DA RID: 218
	public static string ID = "CometDetector";

	// Token: 0x040000DB RID: 219
	public const float COVERAGE_REQUIRED_01 = 0.5f;

	// Token: 0x040000DC RID: 220
	public const float BEST_WARNING_TIME_IN_SECONDS = 200f;

	// Token: 0x040000DD RID: 221
	public const float WORST_WARNING_TIME_IN_SECONDS = 1f;

	// Token: 0x040000DE RID: 222
	public const int SCAN_RADIUS = 15;

	// Token: 0x040000DF RID: 223
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 0), 15, new CellOffset(0, 0), 15, 1);

	// Token: 0x040000E0 RID: 224
	public const float LOGIC_SIGNAL_DELAY_ON_LOAD = 3f;
}
