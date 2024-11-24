using System;
using TUNING;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class MissionControlConfig : IBuildingConfig
{
	// Token: 0x06001499 RID: 5273 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x0019141C File Offset: 0x0018F61C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControl";
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

	// Token: 0x0600149B RID: 5275 RVA: 0x001914B8 File Offset: 0x0018F6B8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControl.Def>();
		MissionControlWorkable missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
		missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x000AF080 File Offset: 0x000AD280
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x000AF0AE File Offset: 0x000AD2AE
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x000AF0B6 File Offset: 0x000AD2B6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000AF03B File Offset: 0x000AD23B
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000DC7 RID: 3527
	public const string ID = "MissionControl";

	// Token: 0x04000DC8 RID: 3528
	public const float EFFECT_DURATION = 600f;

	// Token: 0x04000DC9 RID: 3529
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x04000DCA RID: 3530
	public const int SCAN_RADIUS = 1;

	// Token: 0x04000DCB RID: 3531
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x04000DCC RID: 3532
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
