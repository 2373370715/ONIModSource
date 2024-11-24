using TUNING;
using UnityEngine;

public class MissionControlConfig : IBuildingConfig
{
	public const string ID = "MissionControl";

	public const float EFFECT_DURATION = 600f;

	public const float SPEED_MULTIPLIER = 1.2f;

	public const int SCAN_RADIUS = 1;

	public const int VERTICAL_SCAN_OFFSET = 2;

	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_VANILLA_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MissionControl", 3, 3, "mission_control_station_kanim", 100, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER0);
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 960f;
		obj.ExhaustKilowattsWhenActive = 0.5f;
		obj.SelfHeatKilowattsWhenActive = 2f;
		obj.ViewMode = OverlayModes.Power.ID;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "large";
		obj.DefaultAnimState = "off";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
		_ = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControl.Def>();
		MissionControlWorkable missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
		missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		AddVisualizer(go);
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AddVisualizer(go);
	}

	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}
}
