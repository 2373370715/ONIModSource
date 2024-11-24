using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x020021F6 RID: 8694
	public class BUILDINGS
	{
		// Token: 0x040096D6 RID: 38614
		public const float DEFAULT_STORAGE_CAPACITY = 2000f;

		// Token: 0x040096D7 RID: 38615
		public const float STANDARD_MANUAL_REFILL_LEVEL = 0.2f;

		// Token: 0x040096D8 RID: 38616
		public const float MASS_TEMPERATURE_SCALE = 0.2f;

		// Token: 0x040096D9 RID: 38617
		public const float AIRCONDITIONER_TEMPDELTA = -14f;

		// Token: 0x040096DA RID: 38618
		public const float MAX_ENVIRONMENT_DELTA = -50f;

		// Token: 0x040096DB RID: 38619
		public const float COMPOST_FLIP_TIME = 20f;

		// Token: 0x040096DC RID: 38620
		public const int TUBE_LAUNCHER_MAX_CHARGES = 3;

		// Token: 0x040096DD RID: 38621
		public const float TUBE_LAUNCHER_RECHARGE_TIME = 10f;

		// Token: 0x040096DE RID: 38622
		public const float TUBE_LAUNCHER_WORK_TIME = 1f;

		// Token: 0x040096DF RID: 38623
		public const float SMELTER_INGOT_INPUTKG = 500f;

		// Token: 0x040096E0 RID: 38624
		public const float SMELTER_INGOT_OUTPUTKG = 100f;

		// Token: 0x040096E1 RID: 38625
		public const float SMELTER_FABRICATIONTIME = 120f;

		// Token: 0x040096E2 RID: 38626
		public const float GEOREFINERY_SLAB_INPUTKG = 1000f;

		// Token: 0x040096E3 RID: 38627
		public const float GEOREFINERY_SLAB_OUTPUTKG = 200f;

		// Token: 0x040096E4 RID: 38628
		public const float GEOREFINERY_FABRICATIONTIME = 120f;

		// Token: 0x040096E5 RID: 38629
		public const float MASS_BURN_RATE_HYDROGENGENERATOR = 0.1f;

		// Token: 0x040096E6 RID: 38630
		public const float COOKER_FOOD_TEMPERATURE = 368.15f;

		// Token: 0x040096E7 RID: 38631
		public const float OVERHEAT_DAMAGE_INTERVAL = 7.5f;

		// Token: 0x040096E8 RID: 38632
		public const float MIN_BUILD_TEMPERATURE = 0f;

		// Token: 0x040096E9 RID: 38633
		public const float MAX_BUILD_TEMPERATURE = 318.15f;

		// Token: 0x040096EA RID: 38634
		public const float MELTDOWN_TEMPERATURE = 533.15f;

		// Token: 0x040096EB RID: 38635
		public const float REPAIR_FORCE_TEMPERATURE = 293.15f;

		// Token: 0x040096EC RID: 38636
		public const int REPAIR_EFFECTIVENESS_BASE = 10;

		// Token: 0x040096ED RID: 38637
		public static Dictionary<string, string> PLANSUBCATEGORYSORTING = new Dictionary<string, string>
		{
			{
				"Ladder",
				"ladders"
			},
			{
				"FirePole",
				"ladders"
			},
			{
				"LadderFast",
				"ladders"
			},
			{
				"Tile",
				"tiles"
			},
			{
				"SnowTile",
				"tiles"
			},
			{
				"WoodTile",
				"tiles"
			},
			{
				"GasPermeableMembrane",
				"tiles"
			},
			{
				"MeshTile",
				"tiles"
			},
			{
				"InsulationTile",
				"tiles"
			},
			{
				"PlasticTile",
				"tiles"
			},
			{
				"MetalTile",
				"tiles"
			},
			{
				"GlassTile",
				"tiles"
			},
			{
				"StorageTile",
				"tiles"
			},
			{
				"BunkerTile",
				"tiles"
			},
			{
				"ExteriorWall",
				"tiles"
			},
			{
				"CarpetTile",
				"tiles"
			},
			{
				"ExobaseHeadquarters",
				"printingpods"
			},
			{
				"Door",
				"doors"
			},
			{
				"ManualPressureDoor",
				"doors"
			},
			{
				"PressureDoor",
				"doors"
			},
			{
				"BunkerDoor",
				"doors"
			},
			{
				"StorageLocker",
				"storage"
			},
			{
				"StorageLockerSmart",
				"storage"
			},
			{
				"LiquidReservoir",
				"storage"
			},
			{
				"GasReservoir",
				"storage"
			},
			{
				"ObjectDispenser",
				"storage"
			},
			{
				"TravelTube",
				"transport"
			},
			{
				"TravelTubeEntrance",
				"transport"
			},
			{
				"TravelTubeWallBridge",
				"transport"
			},
			{
				RemoteWorkerDockConfig.ID,
				"operations"
			},
			{
				RemoteWorkTerminalConfig.ID,
				"operations"
			},
			{
				"MineralDeoxidizer",
				"producers"
			},
			{
				"SublimationStation",
				"producers"
			},
			{
				"Oxysconce",
				"producers"
			},
			{
				"Electrolyzer",
				"producers"
			},
			{
				"RustDeoxidizer",
				"producers"
			},
			{
				"AirFilter",
				"scrubbers"
			},
			{
				"CO2Scrubber",
				"scrubbers"
			},
			{
				"AlgaeHabitat",
				"scrubbers"
			},
			{
				"DevGenerator",
				"generators"
			},
			{
				"ManualGenerator",
				"generators"
			},
			{
				"Generator",
				"generators"
			},
			{
				"WoodGasGenerator",
				"generators"
			},
			{
				"HydrogenGenerator",
				"generators"
			},
			{
				"MethaneGenerator",
				"generators"
			},
			{
				"PetroleumGenerator",
				"generators"
			},
			{
				"SteamTurbine",
				"generators"
			},
			{
				"SteamTurbine2",
				"generators"
			},
			{
				"SolarPanel",
				"generators"
			},
			{
				"Wire",
				"wires"
			},
			{
				"WireBridge",
				"wires"
			},
			{
				"HighWattageWire",
				"wires"
			},
			{
				"WireBridgeHighWattage",
				"wires"
			},
			{
				"WireRefined",
				"wires"
			},
			{
				"WireRefinedBridge",
				"wires"
			},
			{
				"WireRefinedHighWattage",
				"wires"
			},
			{
				"WireRefinedBridgeHighWattage",
				"wires"
			},
			{
				"Battery",
				"batteries"
			},
			{
				"BatteryMedium",
				"batteries"
			},
			{
				"BatterySmart",
				"batteries"
			},
			{
				"ElectrobankCharger",
				"electrobankbuildings"
			},
			{
				"SmallElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"LargeElectrobankDischarger",
				"electrobankbuildings"
			},
			{
				"PowerTransformerSmall",
				"powercontrol"
			},
			{
				"PowerTransformer",
				"powercontrol"
			},
			{
				SwitchConfig.ID,
				"switches"
			},
			{
				LogicPowerRelayConfig.ID,
				"switches"
			},
			{
				TemperatureControlledSwitchConfig.ID,
				"switches"
			},
			{
				PressureSwitchLiquidConfig.ID,
				"switches"
			},
			{
				PressureSwitchGasConfig.ID,
				"switches"
			},
			{
				"MicrobeMusher",
				"cooking"
			},
			{
				"CookingStation",
				"cooking"
			},
			{
				"Deepfryer",
				"cooking"
			},
			{
				"GourmetCookingStation",
				"cooking"
			},
			{
				"SpiceGrinder",
				"cooking"
			},
			{
				"FoodDehydrator",
				"cooking"
			},
			{
				"FoodRehydrator",
				"cooking"
			},
			{
				"PlanterBox",
				"farming"
			},
			{
				"FarmTile",
				"farming"
			},
			{
				"HydroponicFarm",
				"farming"
			},
			{
				"RationBox",
				"storage"
			},
			{
				"Refrigerator",
				"storage"
			},
			{
				"CreatureDeliveryPoint",
				"ranching"
			},
			{
				"CritterDropOff",
				"ranching"
			},
			{
				"CritterPickUp",
				"ranching"
			},
			{
				"FishDeliveryPoint",
				"ranching"
			},
			{
				"CreatureFeeder",
				"ranching"
			},
			{
				"FishFeeder",
				"ranching"
			},
			{
				"MilkFeeder",
				"ranching"
			},
			{
				"EggIncubator",
				"ranching"
			},
			{
				"EggCracker",
				"ranching"
			},
			{
				"CreatureGroundTrap",
				"ranching"
			},
			{
				"CreatureAirTrap",
				"ranching"
			},
			{
				"WaterTrap",
				"ranching"
			},
			{
				"CritterCondo",
				"ranching"
			},
			{
				"UnderwaterCritterCondo",
				"ranching"
			},
			{
				"AirBorneCritterCondo",
				"ranching"
			},
			{
				"Outhouse",
				"washroom"
			},
			{
				"FlushToilet",
				"washroom"
			},
			{
				"WallToilet",
				"washroom"
			},
			{
				ShowerConfig.ID,
				"washroom"
			},
			{
				"GunkEmptier",
				"washroom"
			},
			{
				"LiquidConduit",
				"pipes"
			},
			{
				"InsulatedLiquidConduit",
				"pipes"
			},
			{
				"LiquidConduitRadiant",
				"pipes"
			},
			{
				"LiquidConduitBridge",
				"pipes"
			},
			{
				"ContactConductivePipeBridge",
				"pipes"
			},
			{
				"LiquidVent",
				"pipes"
			},
			{
				"LiquidPump",
				"pumps"
			},
			{
				"LiquidMiniPump",
				"pumps"
			},
			{
				"LiquidPumpingStation",
				"pumps"
			},
			{
				"DevPumpLiquid",
				"pumps"
			},
			{
				"BottleEmptier",
				"valves"
			},
			{
				"LiquidFilter",
				"valves"
			},
			{
				"LiquidConduitPreferentialFlow",
				"valves"
			},
			{
				"LiquidConduitOverflow",
				"valves"
			},
			{
				"LiquidValve",
				"valves"
			},
			{
				"LiquidLogicValve",
				"valves"
			},
			{
				"LiquidLimitValve",
				"valves"
			},
			{
				"LiquidBottler",
				"valves"
			},
			{
				"BottleEmptierConduitLiquid",
				"valves"
			},
			{
				LiquidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LiquidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortLiquid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortLiquidUnloader",
				"buildmenuports"
			},
			{
				"GasConduit",
				"pipes"
			},
			{
				"InsulatedGasConduit",
				"pipes"
			},
			{
				"GasConduitRadiant",
				"pipes"
			},
			{
				"GasConduitBridge",
				"pipes"
			},
			{
				"GasVent",
				"pipes"
			},
			{
				"GasVentHighPressure",
				"pipes"
			},
			{
				"GasPump",
				"pumps"
			},
			{
				"GasMiniPump",
				"pumps"
			},
			{
				"DevPumpGas",
				"pumps"
			},
			{
				"GasBottler",
				"valves"
			},
			{
				"BottleEmptierGas",
				"valves"
			},
			{
				"BottleEmptierConduitGas",
				"valves"
			},
			{
				"GasFilter",
				"valves"
			},
			{
				"GasConduitPreferentialFlow",
				"valves"
			},
			{
				"GasConduitOverflow",
				"valves"
			},
			{
				"GasValve",
				"valves"
			},
			{
				"GasLogicValve",
				"valves"
			},
			{
				"GasLimitValve",
				"valves"
			},
			{
				GasConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				GasConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"ModularLaunchpadPortGas",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortGasUnloader",
				"buildmenuports"
			},
			{
				"Compost",
				"organic"
			},
			{
				"FertilizerMaker",
				"organic"
			},
			{
				"AlgaeDistillery",
				"organic"
			},
			{
				"EthanolDistillery",
				"organic"
			},
			{
				"SludgePress",
				"organic"
			},
			{
				"MilkFatSeparator",
				"organic"
			},
			{
				"MilkPress",
				"organic"
			},
			{
				"IceKettle",
				"materials"
			},
			{
				"WaterPurifier",
				"materials"
			},
			{
				"Desalinator",
				"materials"
			},
			{
				"RockCrusher",
				"materials"
			},
			{
				"Kiln",
				"materials"
			},
			{
				"MetalRefinery",
				"materials"
			},
			{
				"GlassForge",
				"materials"
			},
			{
				"OilRefinery",
				"oil"
			},
			{
				"Polymerizer",
				"oil"
			},
			{
				"OxyliteRefinery",
				"advanced"
			},
			{
				"SupermaterialRefinery",
				"advanced"
			},
			{
				"DiamondPress",
				"advanced"
			},
			{
				"Chlorinator",
				"advanced"
			},
			{
				"WashBasin",
				"hygiene"
			},
			{
				"WashSink",
				"hygiene"
			},
			{
				"HandSanitizer",
				"hygiene"
			},
			{
				"DecontaminationShower",
				"hygiene"
			},
			{
				"Apothecary",
				"medical"
			},
			{
				"DoctorStation",
				"medical"
			},
			{
				"AdvancedDoctorStation",
				"medical"
			},
			{
				"MedicalCot",
				"medical"
			},
			{
				"DevLifeSupport",
				"medical"
			},
			{
				"MassageTable",
				"wellness"
			},
			{
				"Grave",
				"wellness"
			},
			{
				"OilChanger",
				"wellness"
			},
			{
				"Bed",
				"beds"
			},
			{
				"LuxuryBed",
				"beds"
			},
			{
				LadderBedConfig.ID,
				"beds"
			},
			{
				"FloorLamp",
				"lights"
			},
			{
				"CeilingLight",
				"lights"
			},
			{
				"SunLamp",
				"lights"
			},
			{
				"DevLightGenerator",
				"lights"
			},
			{
				"MercuryCeilingLight",
				"lights"
			},
			{
				"DiningTable",
				"dining"
			},
			{
				"WaterCooler",
				"recreation"
			},
			{
				"Phonobox",
				"recreation"
			},
			{
				"ArcadeMachine",
				"recreation"
			},
			{
				"EspressoMachine",
				"recreation"
			},
			{
				"HotTub",
				"recreation"
			},
			{
				"MechanicalSurfboard",
				"recreation"
			},
			{
				"Sauna",
				"recreation"
			},
			{
				"Juicer",
				"recreation"
			},
			{
				"SodaFountain",
				"recreation"
			},
			{
				"BeachChair",
				"recreation"
			},
			{
				"VerticalWindTunnel",
				"recreation"
			},
			{
				"Telephone",
				"recreation"
			},
			{
				"FlowerVase",
				"decor"
			},
			{
				"FlowerVaseWall",
				"decor"
			},
			{
				"FlowerVaseHanging",
				"decor"
			},
			{
				"FlowerVaseHangingFancy",
				"decor"
			},
			{
				PixelPackConfig.ID,
				"decor"
			},
			{
				"SmallSculpture",
				"decor"
			},
			{
				"Sculpture",
				"decor"
			},
			{
				"IceSculpture",
				"decor"
			},
			{
				"MarbleSculpture",
				"decor"
			},
			{
				"MetalSculpture",
				"decor"
			},
			{
				"WoodSculpture",
				"decor"
			},
			{
				"CrownMoulding",
				"decor"
			},
			{
				"CornerMoulding",
				"decor"
			},
			{
				"Canvas",
				"decor"
			},
			{
				"CanvasWide",
				"decor"
			},
			{
				"CanvasTall",
				"decor"
			},
			{
				"ItemPedestal",
				"decor"
			},
			{
				"ParkSign",
				"decor"
			},
			{
				"MonumentBottom",
				"decor"
			},
			{
				"MonumentMiddle",
				"decor"
			},
			{
				"MonumentTop",
				"decor"
			},
			{
				"ResearchCenter",
				"research"
			},
			{
				"AdvancedResearchCenter",
				"research"
			},
			{
				"GeoTuner",
				"research"
			},
			{
				"NuclearResearchCenter",
				"research"
			},
			{
				"OrbitalResearchCenter",
				"research"
			},
			{
				"CosmicResearchCenter",
				"research"
			},
			{
				"DLC1CosmicResearchCenter",
				"research"
			},
			{
				"DataMiner",
				"research"
			},
			{
				"ArtifactAnalysisStation",
				"archaeology"
			},
			{
				"MissileFabricator",
				"meteordefense"
			},
			{
				"AstronautTrainingCenter",
				"exploration"
			},
			{
				"PowerControlStation",
				"industrialstation"
			},
			{
				"ResetSkillsStation",
				"industrialstation"
			},
			{
				"RoleStation",
				"workstations"
			},
			{
				"RanchStation",
				"ranching"
			},
			{
				"ShearingStation",
				"ranching"
			},
			{
				"MilkingStation",
				"ranching"
			},
			{
				"FarmStation",
				"farming"
			},
			{
				"GeneticAnalysisStation",
				"farming"
			},
			{
				"CraftingTable",
				"manufacturing"
			},
			{
				"AdvancedCraftingTable",
				"manufacturing"
			},
			{
				"ClothingFabricator",
				"manufacturing"
			},
			{
				"ClothingAlterationStation",
				"manufacturing"
			},
			{
				"SuitFabricator",
				"manufacturing"
			},
			{
				"OxygenMaskMarker",
				"equipment"
			},
			{
				"OxygenMaskLocker",
				"equipment"
			},
			{
				"SuitMarker",
				"equipment"
			},
			{
				"SuitLocker",
				"equipment"
			},
			{
				"JetSuitMarker",
				"equipment"
			},
			{
				"JetSuitLocker",
				"equipment"
			},
			{
				"MissileLauncher",
				"missiles"
			},
			{
				"LeadSuitMarker",
				"equipment"
			},
			{
				"LeadSuitLocker",
				"equipment"
			},
			{
				"Campfire",
				"temperature"
			},
			{
				"DevHeater",
				"temperature"
			},
			{
				"SpaceHeater",
				"temperature"
			},
			{
				"LiquidHeater",
				"temperature"
			},
			{
				"LiquidConditioner",
				"temperature"
			},
			{
				"LiquidCooledFan",
				"temperature"
			},
			{
				"IceCooledFan",
				"temperature"
			},
			{
				"IceMachine",
				"temperature"
			},
			{
				"AirConditioner",
				"temperature"
			},
			{
				"ThermalBlock",
				"temperature"
			},
			{
				"OreScrubber",
				"sanitation"
			},
			{
				"OilWellCap",
				"oil"
			},
			{
				"SweepBotStation",
				"sanitation"
			},
			{
				"LogicWire",
				"wires"
			},
			{
				"LogicWireBridge",
				"wires"
			},
			{
				"LogicRibbon",
				"wires"
			},
			{
				"LogicRibbonBridge",
				"wires"
			},
			{
				LogicRibbonReaderConfig.ID,
				"wires"
			},
			{
				LogicRibbonWriterConfig.ID,
				"wires"
			},
			{
				"LogicDuplicantSensor",
				"sensors"
			},
			{
				LogicPressureSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicPressureSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				LogicLightSensorConfig.ID,
				"sensors"
			},
			{
				LogicWattageSensorConfig.ID,
				"sensors"
			},
			{
				LogicTimeOfDaySensorConfig.ID,
				"sensors"
			},
			{
				LogicTimerSensorConfig.ID,
				"sensors"
			},
			{
				LogicDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorGasConfig.ID,
				"sensors"
			},
			{
				LogicElementSensorLiquidConfig.ID,
				"sensors"
			},
			{
				LogicCritterCountSensorConfig.ID,
				"sensors"
			},
			{
				LogicRadiationSensorConfig.ID,
				"sensors"
			},
			{
				LogicHEPSensorConfig.ID,
				"sensors"
			},
			{
				CometDetectorConfig.ID,
				"sensors"
			},
			{
				LogicCounterConfig.ID,
				"logicmanager"
			},
			{
				"Checkpoint",
				"logicmanager"
			},
			{
				LogicAlarmConfig.ID,
				"logicmanager"
			},
			{
				LogicHammerConfig.ID,
				"logicaudio"
			},
			{
				LogicSwitchConfig.ID,
				"switches"
			},
			{
				"FloorSwitch",
				"switches"
			},
			{
				"LogicGateNOT",
				"logicgates"
			},
			{
				"LogicGateAND",
				"logicgates"
			},
			{
				"LogicGateOR",
				"logicgates"
			},
			{
				"LogicGateBUFFER",
				"logicgates"
			},
			{
				"LogicGateFILTER",
				"logicgates"
			},
			{
				"LogicGateXOR",
				"logicgates"
			},
			{
				LogicMemoryConfig.ID,
				"logicgates"
			},
			{
				"LogicGateMultiplexer",
				"logicgates"
			},
			{
				"LogicGateDemultiplexer",
				"logicgates"
			},
			{
				"LogicInterasteroidSender",
				"transmissions"
			},
			{
				"LogicInterasteroidReceiver",
				"transmissions"
			},
			{
				"SolidConduit",
				"conveyancestructures"
			},
			{
				"SolidConduitBridge",
				"conveyancestructures"
			},
			{
				"SolidConduitInbox",
				"conveyancestructures"
			},
			{
				"SolidConduitOutbox",
				"conveyancestructures"
			},
			{
				"SolidFilter",
				"conveyancestructures"
			},
			{
				"SolidVent",
				"conveyancestructures"
			},
			{
				"DevPumpSolid",
				"pumps"
			},
			{
				"SolidLogicValve",
				"valves"
			},
			{
				"SolidLimitValve",
				"valves"
			},
			{
				SolidConduitDiseaseSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitElementSensorConfig.ID,
				"sensors"
			},
			{
				SolidConduitTemperatureSensorConfig.ID,
				"sensors"
			},
			{
				"AutoMiner",
				"automated"
			},
			{
				"SolidTransferArm",
				"automated"
			},
			{
				"ModularLaunchpadPortSolid",
				"buildmenuports"
			},
			{
				"ModularLaunchpadPortSolidUnloader",
				"buildmenuports"
			},
			{
				"Telescope",
				"telescopes"
			},
			{
				"ClusterTelescope",
				"telescopes"
			},
			{
				"ClusterTelescopeEnclosed",
				"telescopes"
			},
			{
				"LaunchPad",
				"rocketstructures"
			},
			{
				"Gantry",
				"rocketstructures"
			},
			{
				"ModularLaunchpadPortBridge",
				"rocketstructures"
			},
			{
				"RailGun",
				"fittings"
			},
			{
				"RailGunPayloadOpener",
				"fittings"
			},
			{
				"LandingBeacon",
				"rocketnav"
			},
			{
				"SteamEngine",
				"engines"
			},
			{
				"KeroseneEngine",
				"engines"
			},
			{
				"HydrogenEngine",
				"engines"
			},
			{
				"SolidBooster",
				"engines"
			},
			{
				"LiquidFuelTank",
				"tanks"
			},
			{
				"OxidizerTank",
				"tanks"
			},
			{
				"OxidizerTankLiquid",
				"tanks"
			},
			{
				"CargoBay",
				"cargo"
			},
			{
				"GasCargoBay",
				"cargo"
			},
			{
				"LiquidCargoBay",
				"cargo"
			},
			{
				"SpecialCargoBay",
				"cargo"
			},
			{
				"CommandModule",
				"rocketnav"
			},
			{
				RocketControlStationConfig.ID,
				"rocketnav"
			},
			{
				LogicClusterLocationSensorConfig.ID,
				"rocketnav"
			},
			{
				"MissionControl",
				"rocketnav"
			},
			{
				"MissionControlCluster",
				"rocketnav"
			},
			{
				"RoboPilotCommandModule",
				"rocketnav"
			},
			{
				"TouristModule",
				"module"
			},
			{
				"ResearchModule",
				"module"
			},
			{
				"RocketInteriorPowerPlug",
				"fittings"
			},
			{
				"RocketInteriorLiquidInput",
				"fittings"
			},
			{
				"RocketInteriorLiquidOutput",
				"fittings"
			},
			{
				"RocketInteriorGasInput",
				"fittings"
			},
			{
				"RocketInteriorGasOutput",
				"fittings"
			},
			{
				"RocketInteriorSolidInput",
				"fittings"
			},
			{
				"RocketInteriorSolidOutput",
				"fittings"
			},
			{
				"ManualHighEnergyParticleSpawner",
				"producers"
			},
			{
				"HighEnergyParticleSpawner",
				"producers"
			},
			{
				"DevHEPSpawner",
				"producers"
			},
			{
				"HighEnergyParticleRedirector",
				"transmissions"
			},
			{
				"HEPBattery",
				"batteries"
			},
			{
				"HEPBridgeTile",
				"transmissions"
			},
			{
				"NuclearReactor",
				"producers"
			},
			{
				"UraniumCentrifuge",
				"producers"
			},
			{
				"RadiationLight",
				"producers"
			},
			{
				"DevRadiationGenerator",
				"producers"
			}
		};

		// Token: 0x040096EE RID: 38638
		public static List<PlanScreen.PlanInfo> PLANORDER = new List<PlanScreen.PlanInfo>
		{
			new PlanScreen.PlanInfo(new HashedString("Base"), false, new List<string>
			{
				"Ladder",
				"FirePole",
				"LadderFast",
				"Tile",
				"SnowTile",
				"WoodTile",
				"GasPermeableMembrane",
				"MeshTile",
				"InsulationTile",
				"PlasticTile",
				"MetalTile",
				"GlassTile",
				"StorageTile",
				"BunkerTile",
				"CarpetTile",
				"ExteriorWall",
				"ExobaseHeadquarters",
				"Door",
				"ManualPressureDoor",
				"PressureDoor",
				"BunkerDoor",
				"StorageLocker",
				"StorageLockerSmart",
				"LiquidReservoir",
				"GasReservoir",
				"ObjectDispenser",
				"TravelTube",
				"TravelTubeEntrance",
				"TravelTubeWallBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Oxygen"), false, new List<string>
			{
				"MineralDeoxidizer",
				"SublimationStation",
				"Oxysconce",
				"AlgaeHabitat",
				"AirFilter",
				"CO2Scrubber",
				"Electrolyzer",
				"RustDeoxidizer"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Power"), false, new List<string>
			{
				"DevGenerator",
				"ManualGenerator",
				"Generator",
				"WoodGasGenerator",
				"HydrogenGenerator",
				"MethaneGenerator",
				"PetroleumGenerator",
				"SteamTurbine",
				"SteamTurbine2",
				"SolarPanel",
				"Wire",
				"WireBridge",
				"HighWattageWire",
				"WireBridgeHighWattage",
				"WireRefined",
				"WireRefinedBridge",
				"WireRefinedHighWattage",
				"WireRefinedBridgeHighWattage",
				"Battery",
				"BatteryMedium",
				"BatterySmart",
				"ElectrobankCharger",
				"SmallElectrobankDischarger",
				"LargeElectrobankDischarger",
				"PowerTransformerSmall",
				"PowerTransformer",
				SwitchConfig.ID,
				LogicPowerRelayConfig.ID,
				TemperatureControlledSwitchConfig.ID,
				PressureSwitchLiquidConfig.ID,
				PressureSwitchGasConfig.ID
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Food"), false, new List<string>
			{
				"MicrobeMusher",
				"CookingStation",
				"Deepfryer",
				"GourmetCookingStation",
				"SpiceGrinder",
				"FoodDehydrator",
				"FoodRehydrator",
				"PlanterBox",
				"FarmTile",
				"HydroponicFarm",
				"RationBox",
				"Refrigerator",
				"CreatureDeliveryPoint",
				"CritterPickUp",
				"CritterDropOff",
				"FishDeliveryPoint",
				"CreatureFeeder",
				"FishFeeder",
				"MilkFeeder",
				"EggIncubator",
				"EggCracker",
				"CreatureGroundTrap",
				"WaterTrap",
				"CreatureAirTrap",
				"CritterCondo",
				"UnderwaterCritterCondo",
				"AirBorneCritterCondo"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Plumbing"), false, new List<string>
			{
				"DevPumpLiquid",
				"Outhouse",
				"FlushToilet",
				"WallToilet",
				ShowerConfig.ID,
				"GunkEmptier",
				"LiquidPumpingStation",
				"BottleEmptier",
				"BottleEmptierConduitLiquid",
				"LiquidBottler",
				"LiquidConduit",
				"InsulatedLiquidConduit",
				"LiquidConduitRadiant",
				"LiquidConduitBridge",
				"LiquidConduitPreferentialFlow",
				"LiquidConduitOverflow",
				"LiquidPump",
				"LiquidMiniPump",
				"LiquidVent",
				"LiquidFilter",
				"LiquidValve",
				"LiquidLogicValve",
				"LiquidLimitValve",
				LiquidConduitElementSensorConfig.ID,
				LiquidConduitDiseaseSensorConfig.ID,
				LiquidConduitTemperatureSensorConfig.ID,
				"ModularLaunchpadPortLiquid",
				"ModularLaunchpadPortLiquidUnloader",
				"ContactConductivePipeBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("HVAC"), false, new List<string>
			{
				"DevPumpGas",
				"GasConduit",
				"InsulatedGasConduit",
				"GasConduitRadiant",
				"GasConduitBridge",
				"GasConduitPreferentialFlow",
				"GasConduitOverflow",
				"GasPump",
				"GasMiniPump",
				"GasVent",
				"GasVentHighPressure",
				"GasFilter",
				"GasValve",
				"GasLogicValve",
				"GasLimitValve",
				"GasBottler",
				"BottleEmptierGas",
				"BottleEmptierConduitGas",
				"ModularLaunchpadPortGas",
				"ModularLaunchpadPortGasUnloader",
				GasConduitElementSensorConfig.ID,
				GasConduitDiseaseSensorConfig.ID,
				GasConduitTemperatureSensorConfig.ID
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Refining"), false, new List<string>
			{
				"Compost",
				"WaterPurifier",
				"Desalinator",
				"FertilizerMaker",
				"AlgaeDistillery",
				"EthanolDistillery",
				"RockCrusher",
				"Kiln",
				"SludgePress",
				"MetalRefinery",
				"GlassForge",
				"OilRefinery",
				"Polymerizer",
				"OxyliteRefinery",
				"Chlorinator",
				"SupermaterialRefinery",
				"DiamondPress",
				"MilkFatSeparator",
				"MilkPress"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Medical"), false, new List<string>
			{
				"DevLifeSupport",
				"WashBasin",
				"WashSink",
				"HandSanitizer",
				"DecontaminationShower",
				"OilChanger",
				"Apothecary",
				"DoctorStation",
				"AdvancedDoctorStation",
				"MedicalCot",
				"MassageTable",
				"Grave"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Furniture"), false, new List<string>
			{
				"Bed",
				"LuxuryBed",
				LadderBedConfig.ID,
				"FloorLamp",
				"CeilingLight",
				"SunLamp",
				"DevLightGenerator",
				"MercuryCeilingLight",
				"DiningTable",
				"WaterCooler",
				"Phonobox",
				"ArcadeMachine",
				"EspressoMachine",
				"HotTub",
				"MechanicalSurfboard",
				"Sauna",
				"Juicer",
				"SodaFountain",
				"BeachChair",
				"VerticalWindTunnel",
				PixelPackConfig.ID,
				"Telephone",
				"FlowerVase",
				"FlowerVaseWall",
				"FlowerVaseHanging",
				"FlowerVaseHangingFancy",
				"SmallSculpture",
				"Sculpture",
				"IceSculpture",
				"WoodSculpture",
				"MarbleSculpture",
				"MetalSculpture",
				"CrownMoulding",
				"CornerMoulding",
				"Canvas",
				"CanvasWide",
				"CanvasTall",
				"ItemPedestal",
				"MonumentBottom",
				"MonumentMiddle",
				"MonumentTop",
				"ParkSign"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Equipment"), false, new List<string>
			{
				"ResearchCenter",
				"AdvancedResearchCenter",
				"NuclearResearchCenter",
				"OrbitalResearchCenter",
				"CosmicResearchCenter",
				"DLC1CosmicResearchCenter",
				"Telescope",
				"GeoTuner",
				"DataMiner",
				"PowerControlStation",
				"FarmStation",
				"GeneticAnalysisStation",
				"RanchStation",
				"ShearingStation",
				"MilkingStation",
				"RoleStation",
				"ResetSkillsStation",
				"ArtifactAnalysisStation",
				RemoteWorkerDockConfig.ID,
				RemoteWorkTerminalConfig.ID,
				"MissileFabricator",
				"CraftingTable",
				"AdvancedCraftingTable",
				"ClothingFabricator",
				"ClothingAlterationStation",
				"SuitFabricator",
				"OxygenMaskMarker",
				"OxygenMaskLocker",
				"SuitMarker",
				"SuitLocker",
				"JetSuitMarker",
				"JetSuitLocker",
				"LeadSuitMarker",
				"LeadSuitLocker",
				"AstronautTrainingCenter"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Utilities"), true, new List<string>
			{
				"Campfire",
				"DevHeater",
				"IceKettle",
				"SpaceHeater",
				"LiquidHeater",
				"LiquidCooledFan",
				"IceCooledFan",
				"IceMachine",
				"AirConditioner",
				"LiquidConditioner",
				"OreScrubber",
				"OilWellCap",
				"ThermalBlock",
				"SweepBotStation"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Automation"), true, new List<string>
			{
				"LogicWire",
				"LogicWireBridge",
				"LogicRibbon",
				"LogicRibbonBridge",
				LogicSwitchConfig.ID,
				"LogicDuplicantSensor",
				LogicPressureSensorGasConfig.ID,
				LogicPressureSensorLiquidConfig.ID,
				LogicTemperatureSensorConfig.ID,
				LogicLightSensorConfig.ID,
				LogicWattageSensorConfig.ID,
				LogicTimeOfDaySensorConfig.ID,
				LogicTimerSensorConfig.ID,
				LogicDiseaseSensorConfig.ID,
				LogicElementSensorGasConfig.ID,
				LogicElementSensorLiquidConfig.ID,
				LogicCritterCountSensorConfig.ID,
				LogicRadiationSensorConfig.ID,
				LogicHEPSensorConfig.ID,
				LogicCounterConfig.ID,
				LogicAlarmConfig.ID,
				LogicHammerConfig.ID,
				"LogicInterasteroidSender",
				"LogicInterasteroidReceiver",
				LogicRibbonReaderConfig.ID,
				LogicRibbonWriterConfig.ID,
				"FloorSwitch",
				"Checkpoint",
				CometDetectorConfig.ID,
				"LogicGateNOT",
				"LogicGateAND",
				"LogicGateOR",
				"LogicGateBUFFER",
				"LogicGateFILTER",
				"LogicGateXOR",
				LogicMemoryConfig.ID,
				"LogicGateMultiplexer",
				"LogicGateDemultiplexer"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Conveyance"), true, new List<string>
			{
				"DevPumpSolid",
				"SolidTransferArm",
				"SolidConduit",
				"SolidConduitBridge",
				"SolidConduitInbox",
				"SolidConduitOutbox",
				"SolidFilter",
				"SolidVent",
				"SolidLogicValve",
				"SolidLimitValve",
				SolidConduitDiseaseSensorConfig.ID,
				SolidConduitElementSensorConfig.ID,
				SolidConduitTemperatureSensorConfig.ID,
				"AutoMiner",
				"ModularLaunchpadPortSolid",
				"ModularLaunchpadPortSolidUnloader"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("Rocketry"), true, new List<string>
			{
				"ClusterTelescope",
				"ClusterTelescopeEnclosed",
				"MissionControl",
				"MissionControlCluster",
				"LaunchPad",
				"Gantry",
				"SteamEngine",
				"KeroseneEngine",
				"SolidBooster",
				"LiquidFuelTank",
				"OxidizerTank",
				"OxidizerTankLiquid",
				"CargoBay",
				"GasCargoBay",
				"LiquidCargoBay",
				"CommandModule",
				"RoboPilotCommandModule",
				"TouristModule",
				"ResearchModule",
				"SpecialCargoBay",
				"HydrogenEngine",
				RocketControlStationConfig.ID,
				"RocketInteriorPowerPlug",
				"RocketInteriorLiquidInput",
				"RocketInteriorLiquidOutput",
				"RocketInteriorGasInput",
				"RocketInteriorGasOutput",
				"RocketInteriorSolidInput",
				"RocketInteriorSolidOutput",
				LogicClusterLocationSensorConfig.ID,
				"RailGun",
				"RailGunPayloadOpener",
				"LandingBeacon",
				"MissileLauncher",
				"ModularLaunchpadPortBridge"
			}, ""),
			new PlanScreen.PlanInfo(new HashedString("HEP"), true, new List<string>
			{
				"RadiationLight",
				"ManualHighEnergyParticleSpawner",
				"NuclearReactor",
				"UraniumCentrifuge",
				"HighEnergyParticleSpawner",
				"DevHEPSpawner",
				"HighEnergyParticleRedirector",
				"HEPBattery",
				"HEPBridgeTile",
				"DevRadiationGenerator"
			}, "EXPANSION1_ID")
		};

		// Token: 0x040096EF RID: 38639
		public static List<Type> COMPONENT_DESCRIPTION_ORDER = new List<Type>
		{
			typeof(BottleEmptier),
			typeof(CookingStation),
			typeof(GourmetCookingStation),
			typeof(RoleStation),
			typeof(ResearchCenter),
			typeof(NuclearResearchCenter),
			typeof(LiquidCooledFan),
			typeof(HandSanitizer),
			typeof(HandSanitizer.Work),
			typeof(PlantAirConditioner),
			typeof(Clinic),
			typeof(BuildingElementEmitter),
			typeof(ElementConverter),
			typeof(ElementConsumer),
			typeof(PassiveElementConsumer),
			typeof(TinkerStation),
			typeof(EnergyConsumer),
			typeof(AirConditioner),
			typeof(Storage),
			typeof(Battery),
			typeof(AirFilter),
			typeof(FlushToilet),
			typeof(Toilet),
			typeof(EnergyGenerator),
			typeof(MassageTable),
			typeof(Shower),
			typeof(Ownable),
			typeof(PlantablePlot),
			typeof(RelaxationPoint),
			typeof(BuildingComplete),
			typeof(Building),
			typeof(BuildingPreview),
			typeof(BuildingUnderConstruction),
			typeof(Crop),
			typeof(Growing),
			typeof(Equippable),
			typeof(ColdBreather),
			typeof(ResearchPointObject),
			typeof(SuitTank),
			typeof(IlluminationVulnerable),
			typeof(TemperatureVulnerable),
			typeof(ExternalTemperatureMonitor),
			typeof(CritterTemperatureMonitor),
			typeof(PressureVulnerable),
			typeof(SubmersionMonitor),
			typeof(BatterySmart),
			typeof(Compost),
			typeof(Refrigerator),
			typeof(Bed),
			typeof(OreScrubber),
			typeof(OreScrubber.Work),
			typeof(MinimumOperatingTemperature),
			typeof(RoomTracker),
			typeof(EnergyConsumerSelfSustaining),
			typeof(ArcadeMachine),
			typeof(Telescope),
			typeof(EspressoMachine),
			typeof(JetSuitTank),
			typeof(Phonobox),
			typeof(ArcadeMachine),
			typeof(BeachChair),
			typeof(Sauna),
			typeof(VerticalWindTunnel),
			typeof(HotTub),
			typeof(Juicer),
			typeof(SodaFountain),
			typeof(MechanicalSurfboard),
			typeof(BottleEmptier),
			typeof(AccessControl),
			typeof(GammaRayOven),
			typeof(Reactor),
			typeof(HighEnergyParticlePort),
			typeof(LeadSuitTank),
			typeof(ActiveParticleConsumer.Def),
			typeof(WaterCooler),
			typeof(Edible),
			typeof(PlantableSeed),
			typeof(SicknessTrigger),
			typeof(MedicinalPill),
			typeof(SeedProducer),
			typeof(Geyser),
			typeof(SpaceHeater),
			typeof(Overheatable),
			typeof(CreatureCalorieMonitor.Def),
			typeof(LureableMonitor.Def),
			typeof(CropSleepingMonitor.Def),
			typeof(FertilizationMonitor.Def),
			typeof(IrrigationMonitor.Def),
			typeof(ScaleGrowthMonitor.Def),
			typeof(TravelTubeEntrance.Work),
			typeof(ToiletWorkableUse),
			typeof(ReceptacleMonitor),
			typeof(Light2D),
			typeof(Ladder),
			typeof(SimCellOccupier),
			typeof(Vent),
			typeof(LogicPorts),
			typeof(Capturable),
			typeof(Trappable),
			typeof(SpaceArtifact),
			typeof(MessStation),
			typeof(PlantElementEmitter),
			typeof(Radiator),
			typeof(DecorProvider)
		};

		// Token: 0x020021F7 RID: 8695
		public class PHARMACY
		{
			// Token: 0x020021F8 RID: 8696
			public class FABRICATIONTIME
			{
				// Token: 0x040096F0 RID: 38640
				public const float TIER0 = 50f;

				// Token: 0x040096F1 RID: 38641
				public const float TIER1 = 100f;

				// Token: 0x040096F2 RID: 38642
				public const float TIER2 = 200f;
			}
		}

		// Token: 0x020021F9 RID: 8697
		public class NUCLEAR_REACTOR
		{
			// Token: 0x020021FA RID: 8698
			public class REACTOR_MASSES
			{
				// Token: 0x040096F3 RID: 38643
				public const float MIN = 1f;

				// Token: 0x040096F4 RID: 38644
				public const float MAX = 10f;
			}
		}

		// Token: 0x020021FB RID: 8699
		public class OVERPRESSURE
		{
			// Token: 0x040096F5 RID: 38645
			public const float TIER0 = 1.8f;
		}

		// Token: 0x020021FC RID: 8700
		public class OVERHEAT_TEMPERATURES
		{
			// Token: 0x040096F6 RID: 38646
			public const float LOW_3 = 10f;

			// Token: 0x040096F7 RID: 38647
			public const float LOW_2 = 328.15f;

			// Token: 0x040096F8 RID: 38648
			public const float LOW_1 = 338.15f;

			// Token: 0x040096F9 RID: 38649
			public const float NORMAL = 348.15f;

			// Token: 0x040096FA RID: 38650
			public const float HIGH_1 = 363.15f;

			// Token: 0x040096FB RID: 38651
			public const float HIGH_2 = 398.15f;

			// Token: 0x040096FC RID: 38652
			public const float HIGH_3 = 1273.15f;

			// Token: 0x040096FD RID: 38653
			public const float HIGH_4 = 2273.15f;
		}

		// Token: 0x020021FD RID: 8701
		public class OVERHEAT_MATERIAL_MOD
		{
			// Token: 0x040096FE RID: 38654
			public const float LOW_3 = -200f;

			// Token: 0x040096FF RID: 38655
			public const float LOW_2 = -20f;

			// Token: 0x04009700 RID: 38656
			public const float LOW_1 = -10f;

			// Token: 0x04009701 RID: 38657
			public const float NORMAL = 0f;

			// Token: 0x04009702 RID: 38658
			public const float HIGH_1 = 15f;

			// Token: 0x04009703 RID: 38659
			public const float HIGH_2 = 50f;

			// Token: 0x04009704 RID: 38660
			public const float HIGH_3 = 200f;

			// Token: 0x04009705 RID: 38661
			public const float HIGH_4 = 500f;

			// Token: 0x04009706 RID: 38662
			public const float HIGH_5 = 900f;
		}

		// Token: 0x020021FE RID: 8702
		public class DECOR_MATERIAL_MOD
		{
			// Token: 0x04009707 RID: 38663
			public const float NORMAL = 0f;

			// Token: 0x04009708 RID: 38664
			public const float HIGH_1 = 0.1f;

			// Token: 0x04009709 RID: 38665
			public const float HIGH_2 = 0.2f;

			// Token: 0x0400970A RID: 38666
			public const float HIGH_3 = 0.5f;

			// Token: 0x0400970B RID: 38667
			public const float HIGH_4 = 1f;
		}

		// Token: 0x020021FF RID: 8703
		public class CONSTRUCTION_MASS_KG
		{
			// Token: 0x0400970C RID: 38668
			public static readonly float[] TIER_TINY = new float[]
			{
				5f
			};

			// Token: 0x0400970D RID: 38669
			public static readonly float[] TIER0 = new float[]
			{
				25f
			};

			// Token: 0x0400970E RID: 38670
			public static readonly float[] TIER1 = new float[]
			{
				50f
			};

			// Token: 0x0400970F RID: 38671
			public static readonly float[] TIER2 = new float[]
			{
				100f
			};

			// Token: 0x04009710 RID: 38672
			public static readonly float[] TIER3 = new float[]
			{
				200f
			};

			// Token: 0x04009711 RID: 38673
			public static readonly float[] TIER4 = new float[]
			{
				400f
			};

			// Token: 0x04009712 RID: 38674
			public static readonly float[] TIER5 = new float[]
			{
				800f
			};

			// Token: 0x04009713 RID: 38675
			public static readonly float[] TIER6 = new float[]
			{
				1200f
			};

			// Token: 0x04009714 RID: 38676
			public static readonly float[] TIER7 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02002200 RID: 8704
		public class ROCKETRY_MASS_KG
		{
			// Token: 0x04009715 RID: 38677
			public static float[] COMMAND_MODULE_MASS = new float[]
			{
				200f
			};

			// Token: 0x04009716 RID: 38678
			public static float[] CARGO_MASS = new float[]
			{
				1000f
			};

			// Token: 0x04009717 RID: 38679
			public static float[] CARGO_MASS_SMALL = new float[]
			{
				400f
			};

			// Token: 0x04009718 RID: 38680
			public static float[] FUEL_TANK_DRY_MASS = new float[]
			{
				100f
			};

			// Token: 0x04009719 RID: 38681
			public static float[] FUEL_TANK_WET_MASS = new float[]
			{
				900f
			};

			// Token: 0x0400971A RID: 38682
			public static float[] FUEL_TANK_WET_MASS_SMALL = new float[]
			{
				300f
			};

			// Token: 0x0400971B RID: 38683
			public static float[] FUEL_TANK_WET_MASS_GAS = new float[]
			{
				100f
			};

			// Token: 0x0400971C RID: 38684
			public static float[] FUEL_TANK_WET_MASS_GAS_LARGE = new float[]
			{
				150f
			};

			// Token: 0x0400971D RID: 38685
			public static float[] OXIDIZER_TANK_OXIDIZER_MASS = new float[]
			{
				900f
			};

			// Token: 0x0400971E RID: 38686
			public static float[] ENGINE_MASS_SMALL = new float[]
			{
				200f
			};

			// Token: 0x0400971F RID: 38687
			public static float[] ENGINE_MASS_LARGE = new float[]
			{
				500f
			};

			// Token: 0x04009720 RID: 38688
			public static float[] NOSE_CONE_TIER1 = new float[]
			{
				200f,
				100f
			};

			// Token: 0x04009721 RID: 38689
			public static float[] NOSE_CONE_TIER2 = new float[]
			{
				400f,
				200f
			};

			// Token: 0x04009722 RID: 38690
			public static float[] HOLLOW_TIER1 = new float[]
			{
				200f
			};

			// Token: 0x04009723 RID: 38691
			public static float[] HOLLOW_TIER2 = new float[]
			{
				400f
			};

			// Token: 0x04009724 RID: 38692
			public static float[] HOLLOW_TIER3 = new float[]
			{
				800f
			};

			// Token: 0x04009725 RID: 38693
			public static float[] DENSE_TIER0 = new float[]
			{
				200f
			};

			// Token: 0x04009726 RID: 38694
			public static float[] DENSE_TIER1 = new float[]
			{
				500f
			};

			// Token: 0x04009727 RID: 38695
			public static float[] DENSE_TIER2 = new float[]
			{
				1000f
			};

			// Token: 0x04009728 RID: 38696
			public static float[] DENSE_TIER3 = new float[]
			{
				2000f
			};
		}

		// Token: 0x02002201 RID: 8705
		public class ENERGY_CONSUMPTION_WHEN_ACTIVE
		{
			// Token: 0x04009729 RID: 38697
			public const float TIER0 = 0f;

			// Token: 0x0400972A RID: 38698
			public const float TIER1 = 5f;

			// Token: 0x0400972B RID: 38699
			public const float TIER2 = 60f;

			// Token: 0x0400972C RID: 38700
			public const float TIER3 = 120f;

			// Token: 0x0400972D RID: 38701
			public const float TIER4 = 240f;

			// Token: 0x0400972E RID: 38702
			public const float TIER5 = 480f;

			// Token: 0x0400972F RID: 38703
			public const float TIER6 = 960f;

			// Token: 0x04009730 RID: 38704
			public const float TIER7 = 1200f;

			// Token: 0x04009731 RID: 38705
			public const float TIER8 = 1600f;
		}

		// Token: 0x02002202 RID: 8706
		public class EXHAUST_ENERGY_ACTIVE
		{
			// Token: 0x04009732 RID: 38706
			public const float TIER0 = 0f;

			// Token: 0x04009733 RID: 38707
			public const float TIER1 = 0.125f;

			// Token: 0x04009734 RID: 38708
			public const float TIER2 = 0.25f;

			// Token: 0x04009735 RID: 38709
			public const float TIER3 = 0.5f;

			// Token: 0x04009736 RID: 38710
			public const float TIER4 = 1f;

			// Token: 0x04009737 RID: 38711
			public const float TIER5 = 2f;

			// Token: 0x04009738 RID: 38712
			public const float TIER6 = 4f;

			// Token: 0x04009739 RID: 38713
			public const float TIER7 = 8f;

			// Token: 0x0400973A RID: 38714
			public const float TIER8 = 16f;
		}

		// Token: 0x02002203 RID: 8707
		public class JOULES_LEAK_PER_CYCLE
		{
			// Token: 0x0400973B RID: 38715
			public const float TIER0 = 400f;

			// Token: 0x0400973C RID: 38716
			public const float TIER1 = 1000f;

			// Token: 0x0400973D RID: 38717
			public const float TIER2 = 2000f;
		}

		// Token: 0x02002204 RID: 8708
		public class SELF_HEAT_KILOWATTS
		{
			// Token: 0x0400973E RID: 38718
			public const float TIER0 = 0f;

			// Token: 0x0400973F RID: 38719
			public const float TIER1 = 0.5f;

			// Token: 0x04009740 RID: 38720
			public const float TIER2 = 1f;

			// Token: 0x04009741 RID: 38721
			public const float TIER3 = 2f;

			// Token: 0x04009742 RID: 38722
			public const float TIER4 = 4f;

			// Token: 0x04009743 RID: 38723
			public const float TIER5 = 8f;

			// Token: 0x04009744 RID: 38724
			public const float TIER6 = 16f;

			// Token: 0x04009745 RID: 38725
			public const float TIER7 = 32f;

			// Token: 0x04009746 RID: 38726
			public const float TIER8 = 64f;

			// Token: 0x04009747 RID: 38727
			public const float TIER_NUCLEAR = 16384f;
		}

		// Token: 0x02002205 RID: 8709
		public class MELTING_POINT_KELVIN
		{
			// Token: 0x04009748 RID: 38728
			public const float TIER0 = 800f;

			// Token: 0x04009749 RID: 38729
			public const float TIER1 = 1600f;

			// Token: 0x0400974A RID: 38730
			public const float TIER2 = 2400f;

			// Token: 0x0400974B RID: 38731
			public const float TIER3 = 3200f;

			// Token: 0x0400974C RID: 38732
			public const float TIER4 = 9999f;
		}

		// Token: 0x02002206 RID: 8710
		public class CONSTRUCTION_TIME_SECONDS
		{
			// Token: 0x0400974D RID: 38733
			public const float TIER0 = 3f;

			// Token: 0x0400974E RID: 38734
			public const float TIER1 = 10f;

			// Token: 0x0400974F RID: 38735
			public const float TIER2 = 30f;

			// Token: 0x04009750 RID: 38736
			public const float TIER3 = 60f;

			// Token: 0x04009751 RID: 38737
			public const float TIER4 = 120f;

			// Token: 0x04009752 RID: 38738
			public const float TIER5 = 240f;

			// Token: 0x04009753 RID: 38739
			public const float TIER6 = 480f;
		}

		// Token: 0x02002207 RID: 8711
		public class HITPOINTS
		{
			// Token: 0x04009754 RID: 38740
			public const int TIER0 = 10;

			// Token: 0x04009755 RID: 38741
			public const int TIER1 = 30;

			// Token: 0x04009756 RID: 38742
			public const int TIER2 = 100;

			// Token: 0x04009757 RID: 38743
			public const int TIER3 = 250;

			// Token: 0x04009758 RID: 38744
			public const int TIER4 = 1000;
		}

		// Token: 0x02002208 RID: 8712
		public class DAMAGE_SOURCES
		{
			// Token: 0x04009759 RID: 38745
			public const int CONDUIT_CONTENTS_BOILED = 1;

			// Token: 0x0400975A RID: 38746
			public const int CONDUIT_CONTENTS_FROZE = 1;

			// Token: 0x0400975B RID: 38747
			public const int BAD_INPUT_ELEMENT = 1;

			// Token: 0x0400975C RID: 38748
			public const int BUILDING_OVERHEATED = 1;

			// Token: 0x0400975D RID: 38749
			public const int HIGH_LIQUID_PRESSURE = 10;

			// Token: 0x0400975E RID: 38750
			public const int MICROMETEORITE = 1;

			// Token: 0x0400975F RID: 38751
			public const int CORROSIVE_ELEMENT = 1;
		}

		// Token: 0x02002209 RID: 8713
		public class RELOCATION_TIME_SECONDS
		{
			// Token: 0x04009760 RID: 38752
			public const float DECONSTRUCT = 4f;

			// Token: 0x04009761 RID: 38753
			public const float CONSTRUCT = 4f;
		}

		// Token: 0x0200220A RID: 8714
		public class WORK_TIME_SECONDS
		{
			// Token: 0x04009762 RID: 38754
			public const float VERYSHORT_WORK_TIME = 5f;

			// Token: 0x04009763 RID: 38755
			public const float SHORT_WORK_TIME = 15f;

			// Token: 0x04009764 RID: 38756
			public const float MEDIUM_WORK_TIME = 30f;

			// Token: 0x04009765 RID: 38757
			public const float LONG_WORK_TIME = 90f;

			// Token: 0x04009766 RID: 38758
			public const float VERY_LONG_WORK_TIME = 150f;

			// Token: 0x04009767 RID: 38759
			public const float EXTENSIVE_WORK_TIME = 180f;
		}

		// Token: 0x0200220B RID: 8715
		public class FABRICATION_TIME_SECONDS
		{
			// Token: 0x04009768 RID: 38760
			public const float VERY_SHORT = 20f;

			// Token: 0x04009769 RID: 38761
			public const float SHORT = 40f;

			// Token: 0x0400976A RID: 38762
			public const float MODERATE = 80f;

			// Token: 0x0400976B RID: 38763
			public const float LONG = 250f;
		}

		// Token: 0x0200220C RID: 8716
		public class DECOR
		{
			// Token: 0x0400976C RID: 38764
			public static readonly EffectorValues NONE = new EffectorValues
			{
				amount = 0,
				radius = 1
			};

			// Token: 0x0200220D RID: 8717
			public class BONUS
			{
				// Token: 0x0400976D RID: 38765
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = 5,
					radius = 1
				};

				// Token: 0x0400976E RID: 38766
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = 10,
					radius = 2
				};

				// Token: 0x0400976F RID: 38767
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = 15,
					radius = 3
				};

				// Token: 0x04009770 RID: 38768
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = 20,
					radius = 4
				};

				// Token: 0x04009771 RID: 38769
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = 25,
					radius = 5
				};

				// Token: 0x04009772 RID: 38770
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = 30,
					radius = 6
				};

				// Token: 0x0200220E RID: 8718
				public class MONUMENT
				{
					// Token: 0x04009773 RID: 38771
					public static readonly EffectorValues COMPLETE = new EffectorValues
					{
						amount = 40,
						radius = 10
					};

					// Token: 0x04009774 RID: 38772
					public static readonly EffectorValues INCOMPLETE = new EffectorValues
					{
						amount = 10,
						radius = 5
					};
				}
			}

			// Token: 0x0200220F RID: 8719
			public class PENALTY
			{
				// Token: 0x04009775 RID: 38773
				public static readonly EffectorValues TIER0 = new EffectorValues
				{
					amount = -5,
					radius = 1
				};

				// Token: 0x04009776 RID: 38774
				public static readonly EffectorValues TIER1 = new EffectorValues
				{
					amount = -10,
					radius = 2
				};

				// Token: 0x04009777 RID: 38775
				public static readonly EffectorValues TIER2 = new EffectorValues
				{
					amount = -15,
					radius = 3
				};

				// Token: 0x04009778 RID: 38776
				public static readonly EffectorValues TIER3 = new EffectorValues
				{
					amount = -20,
					radius = 4
				};

				// Token: 0x04009779 RID: 38777
				public static readonly EffectorValues TIER4 = new EffectorValues
				{
					amount = -20,
					radius = 5
				};

				// Token: 0x0400977A RID: 38778
				public static readonly EffectorValues TIER5 = new EffectorValues
				{
					amount = -25,
					radius = 6
				};
			}
		}

		// Token: 0x02002210 RID: 8720
		public class MASS_KG
		{
			// Token: 0x0400977B RID: 38779
			public const float TIER0 = 25f;

			// Token: 0x0400977C RID: 38780
			public const float TIER1 = 50f;

			// Token: 0x0400977D RID: 38781
			public const float TIER2 = 100f;

			// Token: 0x0400977E RID: 38782
			public const float TIER3 = 200f;

			// Token: 0x0400977F RID: 38783
			public const float TIER4 = 400f;

			// Token: 0x04009780 RID: 38784
			public const float TIER5 = 800f;

			// Token: 0x04009781 RID: 38785
			public const float TIER6 = 1200f;

			// Token: 0x04009782 RID: 38786
			public const float TIER7 = 2000f;
		}

		// Token: 0x02002211 RID: 8721
		public class UPGRADES
		{
			// Token: 0x04009783 RID: 38787
			public const float BUILDTIME_TIER0 = 120f;

			// Token: 0x02002212 RID: 8722
			public class MATERIALTAGS
			{
				// Token: 0x04009784 RID: 38788
				public const string METAL = "Metal";

				// Token: 0x04009785 RID: 38789
				public const string REFINEDMETAL = "RefinedMetal";

				// Token: 0x04009786 RID: 38790
				public const string CARBON = "Carbon";
			}

			// Token: 0x02002213 RID: 8723
			public class MATERIALMASS
			{
				// Token: 0x04009787 RID: 38791
				public const int TIER0 = 100;

				// Token: 0x04009788 RID: 38792
				public const int TIER1 = 200;

				// Token: 0x04009789 RID: 38793
				public const int TIER2 = 400;

				// Token: 0x0400978A RID: 38794
				public const int TIER3 = 500;
			}

			// Token: 0x02002214 RID: 8724
			public class MODIFIERAMOUNTS
			{
				// Token: 0x0400978B RID: 38795
				public const float MANUALGENERATOR_ENERGYGENERATION = 1.2f;

				// Token: 0x0400978C RID: 38796
				public const float MANUALGENERATOR_CAPACITY = 2f;

				// Token: 0x0400978D RID: 38797
				public const float PROPANEGENERATOR_ENERGYGENERATION = 1.6f;

				// Token: 0x0400978E RID: 38798
				public const float PROPANEGENERATOR_HEATGENERATION = 1.6f;

				// Token: 0x0400978F RID: 38799
				public const float GENERATOR_HEATGENERATION = 0.8f;

				// Token: 0x04009790 RID: 38800
				public const float GENERATOR_ENERGYGENERATION = 1.3f;

				// Token: 0x04009791 RID: 38801
				public const float TURBINE_ENERGYGENERATION = 1.2f;

				// Token: 0x04009792 RID: 38802
				public const float TURBINE_CAPACITY = 1.2f;

				// Token: 0x04009793 RID: 38803
				public const float SUITRECHARGER_EXECUTIONTIME = 1.2f;

				// Token: 0x04009794 RID: 38804
				public const float SUITRECHARGER_HEATGENERATION = 1.2f;

				// Token: 0x04009795 RID: 38805
				public const float STORAGELOCKER_CAPACITY = 2f;

				// Token: 0x04009796 RID: 38806
				public const float SOLARPANEL_ENERGYGENERATION = 1.2f;

				// Token: 0x04009797 RID: 38807
				public const float SMELTER_HEATGENERATION = 0.7f;
			}
		}
	}
}
