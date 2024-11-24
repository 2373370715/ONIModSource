using TUNING;
using UnityEngine;

public class LandingBeaconConfig : IBuildingConfig
{
	public const string ID = "LandingBeacon";

	public const int LANDING_ACCURACY = 3;

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("LandingBeacon", 1, 3, "landing_beacon_kanim", 1000, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.PENALTY.TIER1);
		BuildingTemplates.CreateRocketBuildingDef(obj);
		obj.SceneLayer = Grid.SceneLayer.BuildingFront;
		obj.OverheatTemperature = 398.15f;
		obj.Floodable = false;
		obj.ObjectLayer = ObjectLayer.Building;
		obj.RequiresPowerInput = false;
		obj.CanMove = false;
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 60f;
		obj.ViewMode = OverlayModes.Power.ID;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		go.AddOrGetDef<LandingBeacon.Def>();
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		AddVisualizer(go);
	}

	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = BeaconSkyVisibility;
		};
	}

	private static bool BeaconSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "beacon assumes DLC");
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue)
		{
			int num = (int)ClusterManager.Instance.GetWorld(Grid.WorldIdx[cell]).maximumBounds.y;
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
}
