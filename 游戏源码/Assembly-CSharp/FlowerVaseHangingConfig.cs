using System;
using TUNING;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class FlowerVaseHangingConfig : IBuildingConfig
{
	// Token: 0x06000BB7 RID: 2999 RVA: 0x00170228 File Offset: 0x0016E428
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseHanging";
		int width = 1;
		int height = 2;
		string anim = "flowervase_hanging_basic_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.GenerateOffsets(1, 1);
		return buildingDef;
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x001702A8 File Offset: 0x0016E4A8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.25f, 0f);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000900 RID: 2304
	public const string ID = "FlowerVaseHanging";
}
