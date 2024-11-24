using TUNING;
using UnityEngine;

public class DevHEPSpawnerConfig : IBuildingConfig
{
	public const string ID = "DevHEPSpawner";

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("DevHEPSpawner", 1, 1, "dev_radbolt_generator_kanim", 30, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.RAW_MINERALS, 1600f, BuildLocationRule.NotInTiles, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.PENALTY.TIER1);
		obj.Floodable = false;
		obj.Invincible = true;
		obj.Overheatable = false;
		obj.Entombable = false;
		obj.AudioCategory = "Metal";
		obj.Overheatable = false;
		obj.ViewMode = OverlayModes.Radiation.ID;
		obj.PermittedRotations = PermittedRotations.R360;
		obj.UseHighEnergyParticleOutputPort = true;
		obj.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		obj.RequiresPowerInput = false;
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "DevHEPSpawner");
		obj.Deprecated = !Sim.IsRadiationEnabled();
		obj.DebugOnly = true;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevHEPSpawner>().boltAmount = 50f;
		go.AddOrGet<LogicOperationalController>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
