using System;
using TUNING;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class BatteryConfig : BaseBatteryConfig
{
	// Token: 0x060000BC RID: 188 RVA: 0x001411DC File Offset: 0x0013F3DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Battery";
		int width = 1;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterysm_kanim";
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, BUILDINGS.DECOR.PENALTY.TIER1, none);
		buildingDef.Breakable = true;
		SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x000A6141 File Offset: 0x000A4341
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 10000f;
		battery.joulesLostPerSecond = 1.6666666f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x000A6165 File Offset: 0x000A4365
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000081 RID: 129
	public const string ID = "Battery";
}
