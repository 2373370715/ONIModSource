using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class LogicAlarmConfig : IBuildingConfig
{
	// Token: 0x06001044 RID: 4164 RVA: 0x001806DC File Offset: 0x0017E8DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicAlarmConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "alarm_sensor_kanim";
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
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicAlarm.INPUT_PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICALARM.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICALARM.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICALARM.INPUT_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicAlarmConfig.ID);
		return buildingDef;
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x000AD149 File Offset: 0x000AB349
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicAlarm>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000B74 RID: 2932
	public static string ID = "LogicAlarm";
}
