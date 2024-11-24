using TUNING;
using UnityEngine;

public class TelescopeConfig : IBuildingConfig
{
	public const string ID = "Telescope";

	public const float POINTS_PER_DAY = 2f;

	public const float MASS_PER_POINT = 2f;

	public const float CAPACITY = 30f;

	public const int SCAN_RADIUS = 4;

	public const int VERTICAL_SCAN_OFFSET = 3;

	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);

	public static readonly Tag INPUT_MATERIAL = GameTags.Glass;

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_VANILLA_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("Telescope", 4, 6, "telescope_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER1, decor: BUILDINGS.DECOR.NONE);
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 120f;
		obj.ExhaustKilowattsWhenActive = 0.125f;
		obj.SelfHeatKilowattsWhenActive = 0f;
		obj.InputConduitType = ConduitType.Gas;
		obj.UtilityInputOffset = new CellOffset(0, 0);
		obj.ViewMode = OverlayModes.Power.ID;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "large";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Telescope telescope = go.AddOrGet<Telescope>();
		telescope.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_telescope_kanim") };
		telescope.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		telescope.workLayer = Grid.SceneLayer.BuildingFront;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
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
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}
}
