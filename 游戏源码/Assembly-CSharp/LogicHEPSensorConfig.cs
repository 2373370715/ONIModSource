using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E5 RID: 997
public class LogicHEPSensorConfig : IBuildingConfig
{
	// Token: 0x060010AF RID: 4271 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00181400 File Offset: 0x0017F600
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicHEPSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = LogicHEPSensorConfig.kanim;
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
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICHEPSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICHEPSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICHEPSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume(LogicHEPSensorConfig.kanim, "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume(LogicHEPSensorConfig.kanim, "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicHEPSensorConfig.ID);
		return buildingDef;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x000AD5C4 File Offset: 0x000AB7C4
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicHEPSensor logicHEPSensor = go.AddOrGet<LogicHEPSensor>();
		logicHEPSensor.manuallyControlled = false;
		logicHEPSensor.activateOnHigherThan = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000B85 RID: 2949
	public static string ID = "LogicHEPSensor";

	// Token: 0x04000B86 RID: 2950
	private static readonly string kanim = "radbolt_sensor_kanim";
}
