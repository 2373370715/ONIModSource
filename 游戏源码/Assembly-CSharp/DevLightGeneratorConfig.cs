using TUNING;
using UnityEngine;

public class DevLightGeneratorConfig : IBuildingConfig
{
	public const string ID = "DevLightGenerator";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("DevLightGenerator", 1, 1, "dev_generator_kanim", 100, 3f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.ALL_METALS, 2400f, BuildLocationRule.Anywhere, noise: NOISE_POLLUTION.NOISY.TIER5, decor: BUILDINGS.DECOR.PENALTY.TIER2);
		obj.RequiresPowerInput = false;
		obj.ViewMode = OverlayModes.Light.ID;
		obj.AudioCategory = "HollowMetal";
		obj.AudioSize = "large";
		obj.Floodable = false;
		obj.DebugOnly = true;
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return obj;
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 1800;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = LightShape.Circle;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		go.AddTag(RoomConstraints.ConstraintTags.LightSource);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		DevLightGenerator devLightGenerator = go.AddOrGet<DevLightGenerator>();
		devLightGenerator.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
		devLightGenerator.Color = LIGHT2D.CEILINGLIGHT_COLOR;
		devLightGenerator.Range = 8f;
		devLightGenerator.Angle = 2.6f;
		devLightGenerator.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
		devLightGenerator.Offset = new Vector2(0f, 0.5f);
		devLightGenerator.shape = LightShape.Circle;
		devLightGenerator.drawOverlay = true;
		devLightGenerator.Lux = 1800;
		go.AddOrGetDef<LightController.Def>();
	}
}
