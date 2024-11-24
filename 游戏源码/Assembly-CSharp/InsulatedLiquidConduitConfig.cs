﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class InsulatedLiquidConduitConfig : IBuildingConfig
{
	// Token: 0x06000F52 RID: 3922 RVA: 0x0017C320 File Offset: 0x0017A520
	public override BuildingDef CreateBuildingDef()
	{
		string id = "InsulatedLiquidConduit";
		int width = 1;
		int height = 1;
		string anim = "utilities_liquid_insulated_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] plumbable = MATERIALS.PLUMBABLE;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plumbable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ThermalConductivity = 0.03125f;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.LiquidConduit;
		buildingDef.TileLayer = ObjectLayer.LiquidConduitTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementLiquidConduit;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduits;
		buildingDef.isKAnimTile = true;
		buildingDef.isUtility = true;
		buildingDef.DragBuild = true;
		buildingDef.ReplacementTags = new List<Tag>();
		buildingDef.ReplacementTags.Add(GameTags.Pipes);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "InsulatedLiquidConduit");
		return buildingDef;
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x000ACB17 File Offset: 0x000AAD17
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		go.AddOrGet<Conduit>().type = ConduitType.Liquid;
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x000ACB2B File Offset: 0x000AAD2B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x0017C42C File Offset: 0x0017A62C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
		go.AddComponent<EmptyConduitWorkable>();
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Pipes, false);
		LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
	}

	// Token: 0x04000B06 RID: 2822
	public const string ID = "InsulatedLiquidConduit";
}
