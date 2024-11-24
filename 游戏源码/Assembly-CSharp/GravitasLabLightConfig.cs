using TUNING;
using UnityEngine;

public class GravitasLabLightConfig : IBuildingConfig
{
	public const string ID = "GravitasLabLight";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("GravitasLabLight", 1, 1, "gravitas_lab_light_kanim", 30, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.ALL_METALS, 2400f, BuildLocationRule.OnCeiling, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.NONE);
		obj.ShowInBuildMenu = false;
		obj.Entombable = false;
		obj.Floodable = false;
		obj.Invincible = true;
		obj.AudioCategory = "Metal";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
