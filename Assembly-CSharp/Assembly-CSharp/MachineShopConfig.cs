using System;
using TUNING;
using UnityEngine;

public class MachineShopConfig : IBuildingConfig
{
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

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.MachineShopType, false);
	}

		public override void DoPostConfigureComplete(GameObject go)
	{
	}

		public const string ID = "MachineShop";

		public static readonly Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

		public const float MASS_PER_TINKER = 5f;

		public static readonly string ROLE_PERK = "IncreaseMachinery";
}
