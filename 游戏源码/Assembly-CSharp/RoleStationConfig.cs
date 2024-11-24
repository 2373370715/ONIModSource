using System;
using TUNING;
using UnityEngine;

// Token: 0x0200056B RID: 1387
public class RoleStationConfig : IBuildingConfig
{
	// Token: 0x0600188D RID: 6285 RVA: 0x0019FC48 File Offset: 0x0019DE48
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RoleStation";
		int width = 2;
		int height = 2;
		string anim = "job_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600188E RID: 6286 RVA: 0x000B06B2 File Offset: 0x000AE8B2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000FE1 RID: 4065
	public const string ID = "RoleStation";
}
