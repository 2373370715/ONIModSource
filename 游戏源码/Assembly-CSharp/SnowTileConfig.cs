﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000588 RID: 1416
public class SnowTileConfig : IBuildingConfig
{
	// Token: 0x06001914 RID: 6420 RVA: 0x001A1C0C File Offset: 0x0019FE0C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SnowTile";
		int width = 1;
		int height = 1;
		string anim = "floor_snow_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] construction_mass = new float[]
		{
			30f
		};
		string[] construction_materials = new string[]
		{
			this.CONSTRUCTION_ELEMENT.ToString()
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_snow");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_snow_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_place_info");
		buildingDef.Temperature = 263.15f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x001A1D24 File Offset: 0x0019FF24
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 1.5f;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = SnowTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.prefabInitFn += this.BuildingComplete_OnInit;
		component.prefabSpawnFn += this.BuildingComplete_OnSpawn;
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x000A6337 File Offset: 0x000A4537
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x000AC55B File Offset: 0x000AA75B
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x001A1DB8 File Offset: 0x0019FFB8
	private void BuildingComplete_OnInit(GameObject instance)
	{
		PrimaryElement component = instance.GetComponent<PrimaryElement>();
		component.SetElement(this.STABLE_SNOW_ELEMENT, true);
		Element element = component.Element;
		Deconstructable component2 = instance.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.constructionElements = new Tag[]
			{
				this.CONSTRUCTION_ELEMENT.CreateTag()
			};
		}
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x001A1E0C File Offset: 0x001A000C
	private void BuildingComplete_OnSpawn(GameObject instance)
	{
		instance.GetComponent<PrimaryElement>().SetElement(this.STABLE_SNOW_ELEMENT, true);
		Deconstructable component = instance.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.constructionElements = new Tag[]
			{
				this.CONSTRUCTION_ELEMENT.CreateTag()
			};
		}
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000A630A File Offset: 0x000A450A
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x0400100F RID: 4111
	public const string ID = "SnowTile";

	// Token: 0x04001010 RID: 4112
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_snow_tops");

	// Token: 0x04001011 RID: 4113
	private SimHashes CONSTRUCTION_ELEMENT = SimHashes.Snow;

	// Token: 0x04001012 RID: 4114
	private SimHashes STABLE_SNOW_ELEMENT = SimHashes.StableSnow;
}
