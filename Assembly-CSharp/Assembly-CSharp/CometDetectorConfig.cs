using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class CometDetectorConfig : IBuildingConfig
{
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

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		CometDetectorConfig.AddVisualizer(go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		CometDetectorConfig.AddVisualizer(go);
	}

	private static void AddVisualizer(GameObject prefab)
	{
		ScannerNetworkVisualizer scannerNetworkVisualizer = prefab.AddOrGet<ScannerNetworkVisualizer>();
		scannerNetworkVisualizer.RangeMin = -15;
		scannerNetworkVisualizer.RangeMax = 15;
	}

	public static string ID = "CometDetector";

	public const float COVERAGE_REQUIRED_01 = 0.5f;

	public const float BEST_WARNING_TIME_IN_SECONDS = 200f;

	public const float WORST_WARNING_TIME_IN_SECONDS = 1f;

	public const int SCAN_RADIUS = 15;

	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 0), 15, new CellOffset(0, 0), 15, 1);

	public const float LOGIC_SIGNAL_DELAY_ON_LOAD = 3f;
}
