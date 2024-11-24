using System;
using TUNING;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class WoodSculptureConfig : IBuildingConfig
{
	// Token: 0x06001B70 RID: 7024 RVA: 0x000A6337 File Offset: 0x000A4537
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x001AB7E8 File Offset: 0x001A99E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodSculpture";
		int width = 1;
		int height = 1;
		string anim = "sculpture_wood_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] woods = MATERIALS.WOODS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, woods, melting_point, build_location_rule, new EffectorValues
		{
			amount = 4,
			radius = 4
		}, none, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x000A6350 File Offset: 0x000A4550
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x000B1E0E File Offset: 0x000B000E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<LongRangeSculpture>().defaultAnimName = "slab";
	}

	// Token: 0x0400114B RID: 4427
	public const string ID = "WoodSculpture";
}
