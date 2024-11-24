using System;
using TUNING;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class ArcadeMachineConfig : IBuildingConfig
{
	// Token: 0x06000087 RID: 135 RVA: 0x00140304 File Offset: 0x0013E504
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArcadeMachine";
		int width = 3;
		int height = 3;
		string anim = "arcade_cabinet_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		return buildingDef;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x0014038C File Offset: 0x0013E58C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<ArcadeMachine>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000068 RID: 104
	public const string ID = "ArcadeMachine";

	// Token: 0x04000069 RID: 105
	public const string SPECIFIC_EFFECT = "PlayedArcade";

	// Token: 0x0400006A RID: 106
	public const string TRACKING_EFFECT = "RecentlyPlayedArcade";
}
