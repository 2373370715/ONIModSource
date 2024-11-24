using System;
using TUNING;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class ClusterTelescopeEnclosedConfig : IBuildingConfig
{
	// Token: 0x06000160 RID: 352 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00143EFC File Offset: 0x001420FC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClusterTelescopeEnclosed";
		int width = 4;
		int height = 6;
		string anim = "telescope_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00143FA0 File Offset: 0x001421A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		go.AddOrGetDef<PoweredController.Def>();
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.analyzeClusterRadius = 4;
		def.workableOverrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_kanim")
		};
		def.skyVisibilityInfo = ClusterTelescopeEnclosedConfig.SKY_VISIBILITY_INFO;
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

	// Token: 0x06000163 RID: 355 RVA: 0x000A6509 File Offset: 0x000A4709
	public override void DoPostConfigureComplete(GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x000A6511 File Offset: 0x000A4711
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x000A6509 File Offset: 0x000A4709
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		ClusterTelescopeEnclosedConfig.AddVisualizer(go);
	}

	// Token: 0x06000166 RID: 358 RVA: 0x000A6519 File Offset: 0x000A4719
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040000D5 RID: 213
	public const string ID = "ClusterTelescopeEnclosed";

	// Token: 0x040000D6 RID: 214
	public const int SCAN_RADIUS = 4;

	// Token: 0x040000D7 RID: 215
	public const int VERTICAL_SCAN_OFFSET = 3;

	// Token: 0x040000D8 RID: 216
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);
}
