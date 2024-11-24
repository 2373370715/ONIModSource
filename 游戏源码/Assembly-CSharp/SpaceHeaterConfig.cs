using TUNING;
using UnityEngine;

public class SpaceHeaterConfig : IBuildingConfig
{
	public const string ID = "SpaceHeater";

	public const float MAX_SELF_HEAT = 32f;

	public const float MAX_EXHAUST_HEAT = 4f;

	public const float MIN_POWER_USAGE = 120f;

	public const float MAX_POWER_USAGE = 240f;

	public static Vector2I MAX_RANGE = new Vector2I(5, 5);

	public static Vector2I MIN_RANGE = new Vector2I(-4, -4);

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("SpaceHeater", 2, 2, "spaceheater_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.BONUS.TIER1);
		obj.RequiresPowerInput = true;
		obj.EnergyConsumptionWhenActive = 240f;
		obj.ExhaustKilowattsWhenActive = 0f;
		obj.SelfHeatKilowattsWhenActive = 0f;
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
		obj.ViewMode = OverlayModes.Temperature.ID;
		obj.AudioCategory = "HollowMetal";
		obj.OverheatTemperature = 398.15f;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WarmingStation);
		go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
		SpaceHeater spaceHeater = go.AddOrGet<SpaceHeater>();
		spaceHeater.targetTemperature = 343.15f;
		spaceHeater.produceHeat = true;
		WarmthProvider.Def def = go.AddOrGetDef<WarmthProvider.Def>();
		def.RangeMax = MAX_RANGE;
		def.RangeMin = MIN_RANGE;
		go.AddOrGetDef<ColdImmunityProvider.Def>().range = new CellOffset[2][]
		{
			new CellOffset[2]
			{
				new CellOffset(-1, 0),
				new CellOffset(2, 0)
			},
			new CellOffset[2]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			}
		};
		AddVisualizer(go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	private void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMax = MAX_RANGE;
		rangeVisualizer.RangeMin = MIN_RANGE;
		rangeVisualizer.BlockingTileVisible = false;
		go.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
	}
}
