using System;
using TUNING;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class MetalSculptureConfig : IBuildingConfig
{
	// Token: 0x060011CC RID: 4556 RVA: 0x00185CB4 File Offset: 0x00183EB4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MetalSculpture";
		int width = 1;
		int height = 3;
		string anim = "sculpture_metal_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 20,
			radius = 8
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x000A6350 File Offset: 0x000A4550
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x000ACA03 File Offset: 0x000AAC03
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000C2A RID: 3114
	public const string ID = "MetalSculpture";
}
