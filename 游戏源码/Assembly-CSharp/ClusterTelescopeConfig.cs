using TUNING;
using UnityEngine;

public class ClusterTelescopeConfig : IBuildingConfig
{
	public const string ID = "ClusterTelescope";

	public const int SCAN_RADIUS = 4;

	public const int VERTICAL_SCAN_OFFSET = 1;

	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 1), 4, new CellOffset(0, 1), 4, 0);

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("ClusterTelescope", 3, 3, "telescope_low_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER1, decor: BUILDINGS.DECOR.NONE);
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 120f;
		obj.ExhaustKilowattsWhenActive = 0.125f;
		obj.SelfHeatKilowattsWhenActive = 0f;
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
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.workableOverrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_telescope_low_kanim") };
		def.skyVisibilityInfo = SKY_VISIBILITY_INFO;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
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
		skyVisibilityVisualizer.OriginOffset.y = 1;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 4;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}
}
