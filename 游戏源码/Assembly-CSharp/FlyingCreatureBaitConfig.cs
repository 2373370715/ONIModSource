using TUNING;
using UnityEngine;

public class FlyingCreatureBaitConfig : IBuildingConfig
{
	public const string ID = "FlyingCreatureBait";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FlyingCreatureBait", 1, 2, "airborne_critter_bait_kanim", 10, 10f, new float[2] { 50f, 10f }, new string[2] { "Metal", "FlyingCritterEdible" }, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Deprecated = true;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<CreatureBait>();
		go.AddTag(GameTags.OneTimeUseLure);
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<SymbolOverrideController>().applySymbolOverridesEveryFrame = true;
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[1]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		Prioritizable.AddRef(go);
	}
}
