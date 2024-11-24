using TUNING;
using UnityEngine;

public class CanvasWideConfig : IBuildingConfig
{
	public const string ID = "CanvasWide";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("CanvasWide", 3, 2, "painting_wide_off_kanim", 30, 120f, new float[2] { 400f, 1f }, new string[2] { "Metal", "BuildingFiber" }, 1600f, BuildLocationRule.Anywhere, noise: NOISE_POLLUTION.NONE, decor: new EffectorValues
		{
			amount = 15,
			radius = 6
		});
		obj.Floodable = false;
		obj.SceneLayer = Grid.SceneLayer.InteriorWall;
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		obj.BaseTimeUntilRepair = -1f;
		obj.ViewMode = OverlayModes.Decor.ID;
		obj.DefaultAnimState = "off";
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
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddComponent<Painting>().defaultAnimName = "off";
	}
}
