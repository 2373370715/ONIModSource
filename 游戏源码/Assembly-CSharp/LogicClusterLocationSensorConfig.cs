﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class LogicClusterLocationSensorConfig : IBuildingConfig
{
	// Token: 0x06001048 RID: 4168 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x001807E0 File Offset: 0x0017E9E0
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicClusterLocationSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "logic_location_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICCLUSTERLOCATIONSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICCLUSTERLOCATIONSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICCLUSTERLOCATIONSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicClusterLocationSensorConfig.ID);
		return buildingDef;
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x000AD16F File Offset: 0x000AB36F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x000AD18A File Offset: 0x000AB38A
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicClusterLocationSensor>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000B75 RID: 2933
	public static string ID = "LogicClusterLocationSensor";
}
