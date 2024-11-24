using System;
using TUNING;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class ModularLaunchpadPortBridgeConfig : IBuildingConfig
{
	// Token: 0x060014AE RID: 5294 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x001917DC File Offset: 0x0018F9DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ModularLaunchpadPortBridge";
		int width = 1;
		int height = 2;
		string anim = "rocket_loader_extension_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		return buildingDef;
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x00191868 File Offset: 0x0018FA68
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.ModularConduitPort, false);
		component.AddTag(GameTags.NotRocketInteriorBuilding, false);
		component.AddTag(BaseModularLaunchpadPortConfig.LinkTag, false);
		ChainedBuilding.Def def = go.AddOrGetDef<ChainedBuilding.Def>();
		def.headBuildingTag = "LaunchPad".ToTag();
		def.linkBuildingTag = BaseModularLaunchpadPortConfig.LinkTag;
		def.objectLayer = ObjectLayer.Building;
		go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000DE3 RID: 3555
	public const string ID = "ModularLaunchpadPortBridge";
}
