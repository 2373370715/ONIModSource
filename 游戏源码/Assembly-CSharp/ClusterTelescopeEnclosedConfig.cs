using TUNING;
using UnityEngine;

public class ClusterTelescopeEnclosedConfig : IBuildingConfig
{
	public const string ID = "ClusterTelescopeEnclosed";

	public const int SCAN_RADIUS = 4;

	public const int VERTICAL_SCAN_OFFSET = 3;

	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("ClusterTelescopeEnclosed", 4, 6, "telescope_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER1, decor: BUILDINGS.DECOR.NONE);
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
		go.AddOrGetDef<PoweredController.Def>();
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.analyzeClusterRadius = 4;
		def.workableOverrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_telescope_kanim") };
		def.skyVisibilityInfo = SKY_VISIBILITY_INFO;
		def.providesOxygen = true;
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
