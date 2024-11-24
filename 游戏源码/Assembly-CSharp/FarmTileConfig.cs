using System;
using TUNING;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class FarmTileConfig : IBuildingConfig
{
	// Token: 0x0600036E RID: 878 RVA: 0x0014E4E8 File Offset: 0x0014C6E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FarmTile";
		int width = 1;
		int height = 1;
		string anim = "farmtilerotating_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] farmable = MATERIALS.FARMABLE;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.PermittedRotations = PermittedRotations.FlipV;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0014E594 File Offset: 0x0014C794
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.occupyingObjectRelativePosition = new Vector3(0f, 1f, 0f);
		plantablePlot.AddDepositTag(GameTags.CropSeed);
		plantablePlot.AddDepositTag(GameTags.WaterSeed);
		plantablePlot.SetFertilizationFlags(true, false);
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
		go.AddOrGet<AnimTileable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000370 RID: 880 RVA: 0x000A70E1 File Offset: 0x000A52E1
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles, false);
		FarmTileConfig.SetUpFarmPlotTags(go);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x000A7100 File Offset: 0x000A5300
	public static void SetUpFarmPlotTags(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject inst)
		{
			Rotatable component = inst.GetComponent<Rotatable>();
			PlantablePlot component2 = inst.GetComponent<PlantablePlot>();
			switch (component.GetOrientation())
			{
			case Orientation.Neutral:
			case Orientation.FlipH:
				component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
				return;
			case Orientation.R90:
			case Orientation.R270:
				component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Side);
				break;
			case Orientation.R180:
			case Orientation.FlipV:
				component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Bottom);
				return;
			case Orientation.NumRotations:
				break;
			default:
				return;
			}
		};
	}

	// Token: 0x04000214 RID: 532
	public const string ID = "FarmTile";
}
