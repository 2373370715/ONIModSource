using System;
using TUNING;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class TilePOIConfig : IBuildingConfig
{
	// Token: 0x06001AAF RID: 6831 RVA: 0x001A8E94 File Offset: 0x001A7094
	public override BuildingDef CreateBuildingDef()
	{
		string id = TilePOIConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "floor_mesh_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_MINERALS = MATERIALS.ALL_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.Repairable = false;
		buildingDef.Replaceable = false;
		buildingDef.Invincible = true;
		buildingDef.IsFoundation = true;
		buildingDef.UseStructureTemperature = false;
		buildingDef.TileLayer = ObjectLayer.FoundationTile;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.isKAnimTile = true;
		buildingDef.DebugOnly = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_POI");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_POI_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000B150F File Offset: 0x000AF70F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SimCellOccupier>().doReplaceElement = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000B1546 File Offset: 0x000AF746
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Bunker, false);
		go.AddComponent<SimTemperatureTransfer>();
		go.GetComponent<Deconstructable>().allowDeconstruction = true;
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000A630A File Offset: 0x000A450A
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x040010F2 RID: 4338
	public static string ID = "TilePOI";
}
