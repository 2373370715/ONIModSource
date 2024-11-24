using TUNING;
using UnityEngine;

public class SnowTileConfig : IBuildingConfig
{
	public const string ID = "SnowTile";

	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_snow_tops");

	private SimHashes CONSTRUCTION_ELEMENT = SimHashes.Snow;

	private SimHashes STABLE_SNOW_ELEMENT = SimHashes.StableSnow;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("SnowTile", 1, 1, "floor_snow_kanim", 100, 3f, new float[1] { 30f }, new string[1] { CONSTRUCTION_ELEMENT.ToString() }, 1600f, BuildLocationRule.Tile, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.NONE);
		BuildingTemplates.CreateFoundationTileDef(obj);
		obj.Floodable = false;
		obj.Overheatable = false;
		obj.Entombable = false;
		obj.UseStructureTemperature = false;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "small";
		obj.BaseTimeUntilRepair = -1f;
		obj.SceneLayer = Grid.SceneLayer.TileMain;
		obj.isKAnimTile = true;
		obj.BlockTileAtlas = Assets.GetTextureAtlas("tiles_snow");
		obj.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_snow_place");
		obj.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		obj.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_info");
		obj.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_place_info");
		obj.Temperature = 263.15f;
		obj.DragBuild = true;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 1.5f;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.prefabInitFn += BuildingComplete_OnInit;
		component.prefabSpawnFn += BuildingComplete_OnSpawn;
	}

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);
	}

	private void BuildingComplete_OnInit(GameObject instance)
	{
		PrimaryElement component = instance.GetComponent<PrimaryElement>();
		component.SetElement(STABLE_SNOW_ELEMENT);
		_ = component.Element;
		Deconstructable component2 = instance.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.constructionElements = new Tag[1] { CONSTRUCTION_ELEMENT.CreateTag() };
		}
	}

	private void BuildingComplete_OnSpawn(GameObject instance)
	{
		instance.GetComponent<PrimaryElement>().SetElement(STABLE_SNOW_ELEMENT);
		Deconstructable component = instance.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.constructionElements = new Tag[1] { CONSTRUCTION_ELEMENT.CreateTag() };
		}
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}
}
