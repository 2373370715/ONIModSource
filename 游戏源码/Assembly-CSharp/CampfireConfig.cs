using TUNING;
using UnityEngine;

public class CampfireConfig : IBuildingConfig
{
	public const string ID = "Campfire";

	public const int RANGE_X = 4;

	public const int RANGE_Y = 3;

	public static Tag FUEL_TAG = SimHashes.WoodLog.ToString();

	public const float FUEL_CONSUMPTION_RATE = 0.025f;

	public const float FUEL_CONSTRUCTION_MASS = 5f;

	public const float FUEL_CAPACITY = 45f;

	public const float EXHAUST_RATE = 0.004f;

	public const SimHashes EXHAUST_TAG = SimHashes.CarbonDioxide;

	private const float EXHAUST_TEMPERATURE = 303.15f;

	public static readonly EffectorValues DECOR_ON = BUILDINGS.DECOR.BONUS.TIER3;

	public static readonly EffectorValues DECOR_OFF = BUILDINGS.DECOR.NONE;

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("Campfire", 1, 2, "campfire_small_kanim", 100, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.RAW_METALS, 9999f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: DECOR_ON, temperature_modification_mass_scale: 0.1f);
		obj.Floodable = true;
		obj.Entombable = true;
		obj.ExhaustKilowattsWhenActive = 0f;
		obj.SelfHeatKilowattsWhenActive = 0f;
		obj.ViewMode = OverlayModes.Temperature.ID;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "small";
		obj.UtilityInputOffset = new CellOffset(0, 0);
		obj.UtilityOutputOffset = new CellOffset(0, 0);
		obj.DefaultAnimState = "on";
		obj.OverheatTemperature = 10000f;
		obj.Overheatable = false;
		obj.POIUnlockable = true;
		obj.ShowInBuildMenu = true;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.WarmingStation);
		component.AddTag(RoomConstraints.ConstraintTags.Decoration);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 45f;
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.capacity = 45f;
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.requestedItemTag = FUEL_TAG;
		manualDeliveryKG.refillMass = 18f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.MinimumMass = 0.025f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
		{
			new ElementConverter.ConsumedElement(FUEL_TAG, 0.025f)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[1]
		{
			new ElementConverter.OutputElement(0.004f, SimHashes.CarbonDioxide, 303.15f, useEntityTemperature: false, storeOutput: false, 0f, 1f)
		};
		AddVisualizer(go);
		Operational operational = go.AddOrGet<Operational>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.Range = 6f;
		light2D.Color = new Color(0.8f, 0.6f, 0f, 1f);
		light2D.Lux = 450;
		Campfire.Def def = go.AddOrGetDef<Campfire.Def>();
		def.fuelTag = FUEL_TAG;
		def.initialFuelMass = 5f;
		WarmthProvider.Def def2 = go.AddOrGetDef<WarmthProvider.Def>();
		def2.RangeMax = new Vector2I(4, 3);
		def2.RangeMin = new Vector2I(-4, 0);
		go.AddOrGetDef<ColdImmunityProvider.Def>().range = new CellOffset[2][]
		{
			new CellOffset[2]
			{
				new CellOffset(-1, 0),
				new CellOffset(1, 0)
			},
			new CellOffset[1]
			{
				new CellOffset(0, 0)
			}
		};
		DirectVolumeHeater directVolumeHeater = go.AddOrGet<DirectVolumeHeater>();
		directVolumeHeater.operational = operational;
		directVolumeHeater.DTUs = 20000f;
		directVolumeHeater.width = 9;
		directVolumeHeater.height = 4;
		directVolumeHeater.maximumExternalTemperature = 343.15f;
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	private void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMax = new Vector2I(4, 3);
		rangeVisualizer.RangeMin = new Vector2I(-4, 0);
		rangeVisualizer.BlockingTileVisible = false;
		go.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
	}
}
