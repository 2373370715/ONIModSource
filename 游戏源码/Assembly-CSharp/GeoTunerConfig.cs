using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class GeoTunerConfig : IBuildingConfig
{
	public struct GeotunedGeyserSettings
	{
		public Tag material;

		public float quantity;

		public Geyser.GeyserModification template;

		public float duration;

		public GeotunedGeyserSettings(Tag material, float quantity, float duration, Geyser.GeyserModification template)
		{
			this.quantity = quantity;
			this.material = material;
			this.template = template;
			this.duration = duration;
		}
	}

	public enum Category
	{
		DEFAULT_CATEGORY,
		WATER_CATEGORY,
		ORGANIC_CATEGORY,
		HYDROCARBON_CATEGORY,
		VOLCANO_CATEGORY,
		METALS_CATEGORY,
		CO2_CATEGORY
	}

	public const int MAX_GEOTUNED = 5;

	public static Dictionary<Category, GeotunedGeyserSettings> CategorySettings = new Dictionary<Category, GeotunedGeyserSettings>
	{
		[Category.DEFAULT_CATEGORY] = new GeotunedGeyserSettings
		{
			material = SimHashes.Dirt.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.1f,
				temperatureModifier = 10f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		},
		[Category.WATER_CATEGORY] = new GeotunedGeyserSettings
		{
			material = SimHashes.BleachStone.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 20f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		},
		[Category.ORGANIC_CATEGORY] = new GeotunedGeyserSettings
		{
			material = SimHashes.Salt.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 15f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		},
		[Category.HYDROCARBON_CATEGORY] = new GeotunedGeyserSettings
		{
			material = SimHashes.Katairite.CreateTag(),
			quantity = 100f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 15f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		},
		[Category.VOLCANO_CATEGORY] = new GeotunedGeyserSettings
		{
			material = SimHashes.Katairite.CreateTag(),
			quantity = 100f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 150f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		},
		[Category.METALS_CATEGORY] = new GeotunedGeyserSettings
		{
			material = SimHashes.Phosphorus.CreateTag(),
			quantity = 80f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 50f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		},
		[Category.CO2_CATEGORY] = new GeotunedGeyserSettings
		{
			material = SimHashes.ToxicSand.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 5f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		}
	};

	public static Dictionary<HashedString, GeotunedGeyserSettings> geotunerGeyserSettings = new Dictionary<HashedString, GeotunedGeyserSettings>
	{
		{
			"steam",
			CategorySettings[Category.WATER_CATEGORY]
		},
		{
			"hot_steam",
			CategorySettings[Category.WATER_CATEGORY]
		},
		{
			"slimy_po2",
			CategorySettings[Category.ORGANIC_CATEGORY]
		},
		{
			"hot_po2",
			CategorySettings[Category.ORGANIC_CATEGORY]
		},
		{
			"methane",
			CategorySettings[Category.HYDROCARBON_CATEGORY]
		},
		{
			"chlorine_gas",
			CategorySettings[Category.ORGANIC_CATEGORY]
		},
		{
			"hot_co2",
			CategorySettings[Category.CO2_CATEGORY]
		},
		{
			"hot_hydrogen",
			CategorySettings[Category.HYDROCARBON_CATEGORY]
		},
		{
			"hot_water",
			CategorySettings[Category.WATER_CATEGORY]
		},
		{
			"salt_water",
			CategorySettings[Category.WATER_CATEGORY]
		},
		{
			"slush_salt_water",
			CategorySettings[Category.WATER_CATEGORY]
		},
		{
			"filthy_water",
			CategorySettings[Category.WATER_CATEGORY]
		},
		{
			"slush_water",
			CategorySettings[Category.WATER_CATEGORY]
		},
		{
			"liquid_sulfur",
			CategorySettings[Category.HYDROCARBON_CATEGORY]
		},
		{
			"liquid_co2",
			CategorySettings[Category.CO2_CATEGORY]
		},
		{
			"oil_drip",
			CategorySettings[Category.HYDROCARBON_CATEGORY]
		},
		{
			"small_volcano",
			CategorySettings[Category.VOLCANO_CATEGORY]
		},
		{
			"big_volcano",
			CategorySettings[Category.VOLCANO_CATEGORY]
		},
		{
			"molten_copper",
			CategorySettings[Category.METALS_CATEGORY]
		},
		{
			"molten_gold",
			CategorySettings[Category.METALS_CATEGORY]
		},
		{
			"molten_iron",
			CategorySettings[Category.METALS_CATEGORY]
		},
		{
			"molten_aluminum",
			CategorySettings[Category.METALS_CATEGORY]
		},
		{
			"molten_cobalt",
			CategorySettings[Category.METALS_CATEGORY]
		},
		{
			"molten_niobium",
			CategorySettings[Category.METALS_CATEGORY]
		},
		{
			"molten_tungsten",
			CategorySettings[Category.METALS_CATEGORY]
		}
	};

	public const string ID = "GeoTuner";

	public const string OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";

	public const string GeyserAnimationModelTarget = "geyser_target";

	public const string GeyserAnimation_GeyserSymbols_LogicLightSymbol = "light_bloom";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("GeoTuner", 4, 3, "geoTuner_kanim", 30, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.REFINED_METALS, 2400f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: TUNING.BUILDINGS.DECOR.NONE);
		obj.Floodable = true;
		obj.Entombable = true;
		obj.Overheatable = false;
		obj.ObjectLayer = ObjectLayer.Building;
		obj.SceneLayer = Grid.SceneLayer.Building;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "medium";
		obj.PermittedRotations = PermittedRotations.FlipH;
		obj.UseStructureTemperature = true;
		obj.LogicOutputPorts = new List<LogicPorts.Port> { LogicPorts.Port.OutputPort("GEYSER_ERUPTION_STATUS_PORT", new CellOffset(-1, 1), STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_INACTIVE) };
		obj.RequiresPowerInput = true;
		obj.ViewMode = OverlayModes.Power.ID;
		obj.EnergyConsumptionWhenActive = 120f;
		obj.ExhaustKilowattsWhenActive = 0.5f;
		obj.SelfHeatKilowattsWhenActive = 4f;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 0f;
		List<Storage.StoredItemModifier> defaultStoredItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Preserve
		};
		storage.SetDefaultStoredItemModifiers(defaultStoredItemModifiers);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		manualDeliveryKG.capacity = 0f;
		manualDeliveryKG.refillMass = 0f;
		manualDeliveryKG.SetStorage(storage);
		go.AddOrGet<GeoTunerWorkable>();
		go.AddOrGet<GeoTunerSwitchGeyserWorkable>();
		go.AddOrGet<CopyBuildingSettings>();
		GeoTuner.Def def = go.AddOrGetDef<GeoTuner.Def>();
		def.OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";
		def.geotunedGeyserSettings = geotunerGeyserSettings;
		def.defaultSetting = CategorySettings[Category.DEFAULT_CATEGORY];
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
