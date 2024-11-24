using TUNING;
using UnityEngine;

public class WoodSculptureConfig : IBuildingConfig
{
	public const string ID = "WoodSculpture";

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("WoodSculpture", 1, 1, "sculpture_wood_kanim", 10, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.WOODS, 800f, BuildLocationRule.Anywhere, noise: NOISE_POLLUTION.NONE, decor: new EffectorValues
		{
			amount = 4,
			radius = 4
		});
		obj.SceneLayer = Grid.SceneLayer.InteriorWall;
		obj.Floodable = false;
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		obj.BaseTimeUntilRepair = -1f;
		obj.ViewMode = OverlayModes.Decor.ID;
		obj.DefaultAnimState = "slab";
		obj.PermittedRotations = PermittedRotations.FlipH;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<LongRangeSculpture>().defaultAnimName = "slab";
	}
}
