using TUNING;
using UnityEngine;

public class DevHeaterConfig : IBuildingConfig
{
	public const string ID = "DevHeater";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("DevHeater", 1, 1, "dev_generator_kanim", 100, 3f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.ALL_METALS, 9999f, BuildLocationRule.Anywhere, noise: NOISE_POLLUTION.NOISY.TIER5, decor: BUILDINGS.DECOR.PENALTY.TIER2);
		obj.RequiresPowerInput = false;
		obj.ViewMode = OverlayModes.Light.ID;
		obj.AudioCategory = "HollowMetal";
		obj.AudioSize = "large";
		obj.Floodable = false;
		obj.DebugOnly = true;
		obj.Overheatable = false;
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return obj;
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<DirectVolumeHeater>();
	}
}
