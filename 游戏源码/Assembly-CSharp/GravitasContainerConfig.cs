using TUNING;
using UnityEngine;

public class GravitasContainerConfig : IBuildingConfig
{
	public const string ID = "GravitasContainer";

	private const float WORK_TIME = 1.5f;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("GravitasContainer", 2, 2, "gravitas_container_kanim", 30, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.ALL_METALS, 2400f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.NONE);
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
		go.AddOrGet<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.Building;
		Prioritizable.AddRef(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		PajamaDispenser pajamaDispenser = go.AddComponent<PajamaDispenser>();
		pajamaDispenser.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_gravitas_container_kanim") };
		pajamaDispenser.SetWorkTime(30f);
		go.AddOrGet<Demolishable>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}
}
