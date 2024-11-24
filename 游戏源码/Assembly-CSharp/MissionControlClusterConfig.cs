using System;
using TUNING;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class MissionControlClusterConfig : IBuildingConfig
{
	// Token: 0x06001490 RID: 5264 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x001912FC File Offset: 0x0018F4FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControlCluster";
		int width = 3;
		int height = 3;
		string anim = "mission_control_station_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		return buildingDef;
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x00191398 File Offset: 0x0018F598
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlClusterConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControlCluster.Def>();
		MissionControlClusterWorkable missionControlClusterWorkable = go.AddOrGet<MissionControlClusterWorkable>();
		missionControlClusterWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlClusterWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x000AEFFD File Offset: 0x000AD1FD
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x000AF02B File Offset: 0x000AD22B
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x000AF033 File Offset: 0x000AD233
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x000AF03B File Offset: 0x000AD23B
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000DC0 RID: 3520
	public const string ID = "MissionControlCluster";

	// Token: 0x04000DC1 RID: 3521
	public const int WORK_RANGE_RADIUS = 2;

	// Token: 0x04000DC2 RID: 3522
	public const float EFFECT_DURATION = 600f;

	// Token: 0x04000DC3 RID: 3523
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x04000DC4 RID: 3524
	public const int SCAN_RADIUS = 1;

	// Token: 0x04000DC5 RID: 3525
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x04000DC6 RID: 3526
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
