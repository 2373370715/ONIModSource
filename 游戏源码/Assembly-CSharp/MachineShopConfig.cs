using System;
using TUNING;
using UnityEngine;

// Token: 0x020003FD RID: 1021
public class MachineShopConfig : IBuildingConfig
{
	// Token: 0x06001122 RID: 4386 RVA: 0x00183134 File Offset: 0x00181334
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MachineShop";
		int width = 4;
		int height = 2;
		string anim = "machineshop_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x000ADA7E File Offset: 0x000ABC7E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.MachineShopType, false);
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BB6 RID: 2998
	public const string ID = "MachineShop";

	// Token: 0x04000BB7 RID: 2999
	public static readonly Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

	// Token: 0x04000BB8 RID: 3000
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000BB9 RID: 3001
	public static readonly string ROLE_PERK = "IncreaseMachinery";
}
