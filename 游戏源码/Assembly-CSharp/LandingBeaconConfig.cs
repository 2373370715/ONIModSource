using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B0 RID: 944
public class LandingBeaconConfig : IBuildingConfig
{
	// Token: 0x06000F9D RID: 3997 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x0017D92C File Offset: 0x0017BB2C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LandingBeacon";
		int width = 1;
		int height = 3;
		string anim = "landing_beacon_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 398.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		return buildingDef;
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x000ACC79 File Offset: 0x000AAE79
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGetDef<LandingBeacon.Def>();
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x000ACC9A File Offset: 0x000AAE9A
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x000ACCA2 File Offset: 0x000AAEA2
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x000ACCA2 File Offset: 0x000AAEA2
	public override void DoPostConfigureComplete(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x000ACCAA File Offset: 0x000AAEAA
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(LandingBeaconConfig.BeaconSkyVisibility);
		};
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x0017D9C8 File Offset: 0x0017BBC8
	private static bool BeaconSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "beacon assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000B28 RID: 2856
	public const string ID = "LandingBeacon";

	// Token: 0x04000B29 RID: 2857
	public const int LANDING_ACCURACY = 3;
}
