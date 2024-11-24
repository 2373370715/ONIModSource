using TUNING;
using UnityEngine;

public class LeadSuitMarkerConfig : IBuildingConfig
{
	public const string ID = "LeadSuitMarker";

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		string[] rEFINED_METALS = MATERIALS.REFINED_METALS;
		BuildingDef obj = BuildingTemplates.CreateBuildingDef(construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, construction_materials: rEFINED_METALS, melting_point: 1600f, build_location_rule: BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, id: "LeadSuitMarker", width: 2, height: 4, anim: "changingarea_radiation_arrow_kanim", hitpoints: 30, construction_time: 30f, decor: BUILDINGS.DECOR.BONUS.TIER1);
		obj.PermittedRotations = PermittedRotations.FlipH;
		obj.PreventIdleTraversalPastBuilding = true;
		obj.Deprecated = !Sim.IsRadiationEnabled();
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "LeadSuitMarker");
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[1]
		{
			new Tag("LeadSuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasLeadSuit;
		go.AddOrGet<AnimTileable>().tags = new Tag[2]
		{
			new Tag("LeadSuitMarker"),
			new Tag("LeadSuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}
}
