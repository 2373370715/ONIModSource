using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

namespace Database;

public class BuildingStatusItems : StatusItems
{
	public interface ISkyVisInfo
	{
		float GetPercentVisible01();
	}

	public StatusItem MissingRequirements;

	public StatusItem GettingReady;

	public StatusItem Working;

	public MaterialsStatusItem MaterialsUnavailable;

	public MaterialsStatusItem MaterialsUnavailableForRefill;

	public StatusItem AngerDamage;

	public StatusItem ClinicOutsideHospital;

	public StatusItem DigUnreachable;

	public StatusItem MopUnreachable;

	public StatusItem StorageUnreachable;

	public StatusItem PassengerModuleUnreachable;

	public StatusItem ConstructableDigUnreachable;

	public StatusItem ConstructionUnreachable;

	public StatusItem CoolingWater;

	public StatusItem DispenseRequested;

	public StatusItem NewDuplicantsAvailable;

	public StatusItem NeedPlant;

	public StatusItem NeedPower;

	public StatusItem NotEnoughPower;

	public StatusItem PowerLoopDetected;

	public StatusItem NeedLiquidIn;

	public StatusItem NeedGasIn;

	public StatusItem NeedResourceMass;

	public StatusItem NeedSolidIn;

	public StatusItem NeedLiquidOut;

	public StatusItem NeedGasOut;

	public StatusItem NeedSolidOut;

	public StatusItem InvalidBuildingLocation;

	public StatusItem PendingDeconstruction;

	public StatusItem PendingDemolition;

	public StatusItem PendingSwitchToggle;

	public StatusItem GasVentObstructed;

	public StatusItem LiquidVentObstructed;

	public StatusItem LiquidPipeEmpty;

	public StatusItem LiquidPipeObstructed;

	public StatusItem GasPipeEmpty;

	public StatusItem GasPipeObstructed;

	public StatusItem SolidPipeObstructed;

	public StatusItem PartiallyDamaged;

	public StatusItem Broken;

	public StatusItem PendingRepair;

	public StatusItem PendingUpgrade;

	public StatusItem RequiresSkillPerk;

	public StatusItem DigRequiresSkillPerk;

	public StatusItem ColonyLacksRequiredSkillPerk;

	public StatusItem ClusterColonyLacksRequiredSkillPerk;

	public StatusItem WorkRequiresMinion;

	public StatusItem PendingWork;

	public StatusItem Flooded;

	public StatusItem NotSubmerged;

	public StatusItem PowerButtonOff;

	public StatusItem SwitchStatusActive;

	public StatusItem SwitchStatusInactive;

	public StatusItem LogicSwitchStatusActive;

	public StatusItem LogicSwitchStatusInactive;

	public StatusItem LogicSensorStatusActive;

	public StatusItem LogicSensorStatusInactive;

	public StatusItem ChangeDoorControlState;

	public StatusItem CurrentDoorControlState;

	public StatusItem ChangeStorageTileTarget;

	public StatusItem Entombed;

	public MaterialsStatusItem WaitingForMaterials;

	public StatusItem WaitingForHighEnergyParticles;

	public StatusItem WaitingForRepairMaterials;

	public StatusItem MissingFoundation;

	public StatusItem NeutroniumUnminable;

	public StatusItem NoStorageFilterSet;

	public StatusItem PendingFish;

	public StatusItem NoFishableWaterBelow;

	public StatusItem GasVentOverPressure;

	public StatusItem LiquidVentOverPressure;

	public StatusItem NoWireConnected;

	public StatusItem NoLogicWireConnected;

	public StatusItem NoTubeConnected;

	public StatusItem NoTubeExits;

	public StatusItem StoredCharge;

	public StatusItem NoPowerConsumers;

	public StatusItem PressureOk;

	public StatusItem UnderPressure;

	public StatusItem AssignedTo;

	public StatusItem Unassigned;

	public StatusItem AssignedPublic;

	public StatusItem AssignedToRoom;

	public StatusItem RationBoxContents;

	public StatusItem ConduitBlocked;

	public StatusItem OutputTileBlocked;

	public StatusItem OutputPipeFull;

	public StatusItem ConduitBlockedMultiples;

	public StatusItem SolidConduitBlockedMultiples;

	public StatusItem MeltingDown;

	public StatusItem DeadReactorCoolingOff;

	public StatusItem UnderConstruction;

	public StatusItem UnderConstructionNoWorker;

	public StatusItem Normal;

	public StatusItem ManualGeneratorChargingUp;

	public StatusItem ManualGeneratorReleasingEnergy;

	public StatusItem GeneratorOffline;

	public StatusItem Pipe;

	public StatusItem Conveyor;

	public StatusItem FabricatorIdle;

	public StatusItem FabricatorEmpty;

	public StatusItem FossilMineIdle;

	public StatusItem FossilMineEmpty;

	public StatusItem FossilEntombed;

	public StatusItem FossilMinePendingWork;

	public StatusItem FabricatorLacksHEP;

	public StatusItem FlushToilet;

	public StatusItem FlushToiletInUse;

	public StatusItem Toilet;

	public StatusItem ToiletNeedsEmptying;

	public StatusItem DesalinatorNeedsEmptying;

	public StatusItem MilkSeparatorNeedsEmptying;

	public StatusItem Unusable;

	public StatusItem NoResearchSelected;

	public StatusItem NoApplicableResearchSelected;

	public StatusItem NoApplicableAnalysisSelected;

	public StatusItem NoResearchOrDestinationSelected;

	public StatusItem Researching;

	public StatusItem ValveRequest;

	public StatusItem EmittingLight;

	public StatusItem EmittingElement;

	public StatusItem EmittingOxygenAvg;

	public StatusItem EmittingGasAvg;

	public StatusItem EmittingBlockedHighPressure;

	public StatusItem EmittingBlockedLowTemperature;

	public StatusItem PumpingLiquidOrGas;

	public StatusItem NoLiquidElementToPump;

	public StatusItem NoGasElementToPump;

	public StatusItem PipeFull;

	public StatusItem PipeMayMelt;

	public StatusItem ElementConsumer;

	public StatusItem ElementEmitterOutput;

	public StatusItem AwaitingWaste;

	public StatusItem AwaitingCompostFlip;

	public StatusItem JoulesAvailable;

	public StatusItem Wattage;

	public StatusItem SolarPanelWattage;

	public StatusItem ModuleSolarPanelWattage;

	public StatusItem SteamTurbineWattage;

	public StatusItem Wattson;

	public StatusItem WireConnected;

	public StatusItem WireNominal;

	public StatusItem WireDisconnected;

	public StatusItem Cooling;

	public StatusItem CoolingStalledHotEnv;

	public StatusItem CoolingStalledColdGas;

	public StatusItem CoolingStalledHotLiquid;

	public StatusItem CoolingStalledColdLiquid;

	public StatusItem CannotCoolFurther;

	public StatusItem NeedsValidRegion;

	public StatusItem NeedSeed;

	public StatusItem AwaitingSeedDelivery;

	public StatusItem AwaitingBaitDelivery;

	public StatusItem NoAvailableSeed;

	public StatusItem NeedEgg;

	public StatusItem AwaitingEggDelivery;

	public StatusItem NoAvailableEgg;

	public StatusItem Grave;

	public StatusItem GraveEmpty;

	public StatusItem NoFilterElementSelected;

	public StatusItem NoLureElementSelected;

	public StatusItem BuildingDisabled;

	public StatusItem Overheated;

	public StatusItem Overloaded;

	public StatusItem LogicOverloaded;

	public StatusItem Expired;

	public StatusItem PumpingStation;

	public StatusItem EmptyPumpingStation;

	public StatusItem GeneShuffleCompleted;

	public StatusItem GeneticAnalysisCompleted;

	public StatusItem DirectionControl;

	public StatusItem WellPressurizing;

	public StatusItem WellOverpressure;

	public StatusItem ReleasingPressure;

	public StatusItem ReactorMeltdown;

	public StatusItem NoSuitMarker;

	public StatusItem SuitMarkerWrongSide;

	public StatusItem SuitMarkerTraversalAnytime;

	public StatusItem SuitMarkerTraversalOnlyWhenRoomAvailable;

	public StatusItem TooCold;

	public StatusItem NotInAnyRoom;

	public StatusItem NotInRequiredRoom;

	public StatusItem NotInRecommendedRoom;

	public StatusItem IncubatorProgress;

	public StatusItem HabitatNeedsEmptying;

	public StatusItem DetectorScanning;

	public StatusItem IncomingMeteors;

	public StatusItem HasGantry;

	public StatusItem MissingGantry;

	public StatusItem DisembarkingDuplicant;

	public StatusItem RocketName;

	public StatusItem PathNotClear;

	public StatusItem InvalidPortOverlap;

	public StatusItem EmergencyPriority;

	public StatusItem SkillPointsAvailable;

	public StatusItem Baited;

	public StatusItem NoCoolant;

	public StatusItem TanningLightSufficient;

	public StatusItem TanningLightInsufficient;

	public StatusItem HotTubWaterTooCold;

	public StatusItem HotTubTooHot;

	public StatusItem HotTubFilling;

	public StatusItem WindTunnelIntake;

	public StatusItem CollectingHEP;

	public StatusItem ReactorRefuelDisabled;

	public StatusItem FridgeCooling;

	public StatusItem FridgeSteady;

	public StatusItem TrapNeedsArming;

	public StatusItem TrapArmed;

	public StatusItem TrapHasCritter;

	public StatusItem WarpPortalCharging;

	public StatusItem WarpConduitPartnerDisabled;

	public StatusItem InOrbit;

	public StatusItem InFlight;

	public StatusItem WaitingToLand;

	public StatusItem DestinationOutOfRange;

	public StatusItem RocketStranded;

	public StatusItem RailgunpayloadNeedsEmptying;

	public StatusItem AwaitingEmptyBuilding;

	public StatusItem DuplicantActivationRequired;

	public StatusItem RocketChecklistIncomplete;

	public StatusItem RocketCargoEmptying;

	public StatusItem RocketCargoFilling;

	public StatusItem RocketCargoFull;

	public StatusItem FlightAllCargoFull;

	public StatusItem FlightCargoRemaining;

	public StatusItem LandedRocketLacksPassengerModule;

	public StatusItem PilotNeeded;

	public StatusItem AutoPilotActive;

	public StatusItem InvalidMaskStationConsumptionState;

	public StatusItem ClusterTelescopeAllWorkComplete;

	public StatusItem RocketPlatformCloseToCeiling;

	public StatusItem ModuleGeneratorPowered;

	public StatusItem ModuleGeneratorNotPowered;

	public StatusItem InOrbitRequired;

	public StatusItem RailGunCooldown;

	public StatusItem NoSurfaceSight;

	public StatusItem LimitValveLimitReached;

	public StatusItem LimitValveLimitNotReached;

	public StatusItem SpacePOIHarvesting;

	public StatusItem SpacePOIWasting;

	public StatusItem RocketRestrictionActive;

	public StatusItem RocketRestrictionInactive;

	public StatusItem NoRocketRestriction;

	public StatusItem BroadcasterOutOfRange;

	public StatusItem LosingRadbolts;

	public StatusItem FabricatorAcceptsMutantSeeds;

	public StatusItem NoSpiceSelected;

	public StatusItem MissionControlAssistingRocket;

	public StatusItem NoRocketsToMissionControlBoost;

	public StatusItem NoRocketsToMissionControlClusterBoost;

	public StatusItem MissionControlBoosted;

	public StatusItem TransitTubeEntranceWaxReady;

	public StatusItem SpecialCargoBayClusterCritterStored;

	public StatusItem ComplexFabricatorCooking;

	public StatusItem ComplexFabricatorProducing;

	public StatusItem ComplexFabricatorTraining;

	public StatusItem ComplexFabricatorResearching;

	public StatusItem ArtifactAnalysisAnalyzing;

	public StatusItem TelescopeWorking;

	public StatusItem ClusterTelescopeMeteorWorking;

	public StatusItem MercuryLight_Charging;

	public StatusItem MercuryLight_Charged;

	public StatusItem MercuryLight_Depleating;

	public StatusItem MercuryLight_Depleated;

	public StatusItem GeoTunerNoGeyserSelected;

	public StatusItem GeoTunerResearchNeeded;

	public StatusItem GeoTunerResearchInProgress;

	public StatusItem GeoTunerBroadcasting;

	public StatusItem GeoTunerGeyserStatus;

	public StatusItem GeyserGeotuned;

	public StatusItem SkyVisNone;

	public StatusItem SkyVisLimited;

	public StatusItem KettleInsuficientSolids;

	public StatusItem KettleInsuficientFuel;

	public StatusItem KettleInsuficientLiquidSpace;

	public StatusItem KettleMelting;

	public StatusItem CreatureManipulatorWaiting;

	public StatusItem CreatureManipulatorProgress;

	public StatusItem CreatureManipulatorMorphModeLocked;

	public StatusItem CreatureManipulatorMorphMode;

	public StatusItem CreatureManipulatorWorking;

	public StatusItem MegaBrainNotEnoughOxygen;

	public StatusItem MegaBrainTankActivationProgress;

	public StatusItem MegaBrainTankDreamAnalysis;

	public StatusItem MegaBrainTankAllDupesAreDead;

	public StatusItem MegaBrainTankComplete;

	public StatusItem FossilHuntExcavationOrdered;

	public StatusItem FossilHuntExcavationInProgress;

	public StatusItem MorbRoverMakerDusty;

	public StatusItem MorbRoverMakerBuildingRevealed;

	public StatusItem MorbRoverMakerGermCollectionProgress;

	public StatusItem MorbRoverMakerNoGermsConsumedAlert;

	public StatusItem MorbRoverMakerCraftingBody;

	public StatusItem MorbRoverMakerReadyForDoctor;

	public StatusItem MorbRoverMakerDoctorWorking;

	public StatusItem GeoVentQuestBlockage;

	public StatusItem GeoVentsDisconnected;

	public StatusItem GeoVentsOverpressure;

	public StatusItem GeoControllerCantVent;

	public StatusItem GeoVentsReady;

	public StatusItem GeoVentsVenting;

	public StatusItem GeoQuestPendingReconnectPipes;

	public StatusItem GeoQuestPendingUncover;

	public StatusItem GeoControllerOffline;

	public StatusItem GeoControllerStorageStatus;

	public StatusItem GeoControllerTemperatureStatus;

	public BuildingStatusItems(ResourceSet parent)
		: base("BuildingStatusItems", parent)
	{
		CreateStatusItems();
	}

	private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022)
	{
		return Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays));
	}

	private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022)
	{
		return Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays));
	}

	private void CreateStatusItems()
	{
		AngerDamage = CreateStatusItem("AngerDamage", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID);
		AssignedTo = CreateStatusItem("AssignedTo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		AssignedTo.resolveStringCallback = delegate(string str, object data)
		{
			IAssignableIdentity assignee2 = ((Assignable)data).assignee;
			if (!assignee2.IsNullOrDestroyed())
			{
				string properName2 = assignee2.GetProperName();
				str = str.Replace("{Assignee}", properName2);
			}
			return str;
		};
		AssignedToRoom = CreateStatusItem("AssignedToRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		AssignedToRoom.resolveStringCallback = delegate(string str, object data)
		{
			IAssignableIdentity assignee = ((Assignable)data).assignee;
			if (!assignee.IsNullOrDestroyed())
			{
				string properName = assignee.GetProperName();
				str = str.Replace("{Assignee}", properName);
			}
			return str;
		};
		Broken = CreateStatusItem("Broken", "BUILDING", "status_item_broken", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		Broken.resolveStringCallback = (string str, object data) => str.Replace("{DamageInfo}", ((BuildingHP.SMInstance)data).master.GetDamageSourceInfo().ToString());
		Broken.conditionalOverlayCallback = ShowInUtilityOverlay;
		ChangeStorageTileTarget = CreateStatusItem("ChangeStorageTileTarget", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ChangeStorageTileTarget.resolveStringCallback = delegate(string str, object data)
		{
			StorageTile.Instance instance16 = (StorageTile.Instance)data;
			return str.Replace("{TargetName}", (instance16.TargetTag == StorageTile.INVALID_TAG) ? BUILDING.STATUSITEMS.CHANGESTORAGETILETARGET.EMPTY.text : instance16.TargetTag.ProperName());
		};
		ChangeDoorControlState = CreateStatusItem("ChangeDoorControlState", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ChangeDoorControlState.resolveStringCallback = delegate(string str, object data)
		{
			Door door2 = (Door)data;
			return str.Replace("{ControlState}", door2.RequestedState.ToString());
		};
		CurrentDoorControlState = CreateStatusItem("CurrentDoorControlState", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		CurrentDoorControlState.resolveStringCallback = delegate(string str, object data)
		{
			Door door = (Door)data;
			string newValue15 = Strings.Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." + door.CurrentState.ToString().ToUpper());
			return str.Replace("{ControlState}", newValue15);
		};
		ClinicOutsideHospital = CreateStatusItem("ClinicOutsideHospital", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		ConduitBlocked = CreateStatusItem("ConduitBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		OutputPipeFull = CreateStatusItem("OutputPipeFull", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		OutputTileBlocked = CreateStatusItem("OutputTileBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ConstructionUnreachable = CreateStatusItem("ConstructionUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ConduitBlockedMultiples = CreateStatusItem("ConduitBlockedMultiples", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: true, OverlayModes.None.ID);
		SolidConduitBlockedMultiples = CreateStatusItem("SolidConduitBlockedMultiples", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: true, OverlayModes.None.ID);
		DigUnreachable = CreateStatusItem("DigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		MopUnreachable = CreateStatusItem("MopUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		StorageUnreachable = CreateStatusItem("StorageUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		PassengerModuleUnreachable = CreateStatusItem("PassengerModuleUnreachable", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		DirectionControl = CreateStatusItem("DirectionControl", BUILDING.STATUSITEMS.DIRECTION_CONTROL.NAME, BUILDING.STATUSITEMS.DIRECTION_CONTROL.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		DirectionControl.resolveStringCallback = delegate(string str, object data)
		{
			DirectionControl obj5 = (DirectionControl)data;
			string newValue14 = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.BOTH;
			switch (obj5.allowedDirection)
			{
			case WorkableReactable.AllowedDirection.Left:
				newValue14 = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.LEFT;
				break;
			case WorkableReactable.AllowedDirection.Right:
				newValue14 = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.RIGHT;
				break;
			}
			str = str.Replace("{Direction}", newValue14);
			return str;
		};
		DeadReactorCoolingOff = CreateStatusItem("DeadReactorCoolingOff", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		DeadReactorCoolingOff.resolveStringCallback = delegate(string str, object data)
		{
			Reactor.StatesInstance smi = (Reactor.StatesInstance)data;
			float num10 = ((Reactor.StatesInstance)data).sm.timeSinceMeltdown.Get(smi);
			str = str.Replace("{CyclesRemaining}", Util.FormatOneDecimalPlace(Mathf.Max(0f, 3000f - num10) / 600f));
			return str;
		};
		ConstructableDigUnreachable = CreateStatusItem("ConstructableDigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		Entombed = CreateStatusItem("Entombed", "BUILDING", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		Entombed.AddNotification();
		Flooded = CreateStatusItem("Flooded", "BUILDING", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		Flooded.AddNotification();
		NotSubmerged = CreateStatusItem("NotSubmerged", "BUILDING", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GasVentObstructed = CreateStatusItem("GasVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.GasConduits.ID);
		GasVentOverPressure = CreateStatusItem("GasVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.GasConduits.ID);
		GeneShuffleCompleted = CreateStatusItem("GeneShuffleCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeneticAnalysisCompleted = CreateStatusItem("GeneticAnalysisCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		InvalidBuildingLocation = CreateStatusItem("InvalidBuildingLocation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		LiquidVentObstructed = CreateStatusItem("LiquidVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.LiquidConduits.ID);
		LiquidVentOverPressure = CreateStatusItem("LiquidVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.LiquidConduits.ID);
		MaterialsUnavailable = new MaterialsStatusItem("MaterialsUnavailable", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: true, OverlayModes.None.ID);
		MaterialsUnavailable.AddNotification();
		MaterialsUnavailable.resolveStringCallback = delegate(string str, object data)
		{
			string text8 = "";
			Dictionary<Tag, float> dictionary = null;
			if (data is IFetchList)
			{
				dictionary = ((IFetchList)data).GetRemainingMinimum();
			}
			else if (data is Dictionary<Tag, float>)
			{
				dictionary = data as Dictionary<Tag, float>;
			}
			if (dictionary.Count > 0)
			{
				bool flag8 = true;
				foreach (KeyValuePair<Tag, float> item in dictionary)
				{
					if (item.Value != 0f)
					{
						if (!flag8)
						{
							text8 += "\n";
						}
						text8 = ((!Assets.IsTagCountable(item.Key)) ? (text8 + string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_MASS, item.Key.ProperName(), GameUtil.GetFormattedMass(item.Value))) : (text8 + string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_UNITS, GameUtil.GetUnitFormattedName(item.Key.ProperName(), item.Value))));
						flag8 = false;
					}
				}
			}
			str = str.Replace("{ItemsRemaining}", text8);
			return str;
		};
		MaterialsUnavailableForRefill = new MaterialsStatusItem("MaterialsUnavailableForRefill", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: true, OverlayModes.None.ID);
		MaterialsUnavailableForRefill.resolveStringCallback = delegate(string str, object data)
		{
			IFetchList obj4 = (IFetchList)data;
			string text7 = "";
			Dictionary<Tag, float> remaining2 = obj4.GetRemaining();
			if (remaining2.Count > 0)
			{
				bool flag7 = true;
				foreach (KeyValuePair<Tag, float> item2 in remaining2)
				{
					if (item2.Value != 0f)
					{
						if (!flag7)
						{
							text7 += "\n";
						}
						text7 += string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLEFORREFILL.LINE_ITEM, item2.Key.ProperName());
						flag7 = false;
					}
				}
			}
			str = str.Replace("{ItemsRemaining}", text7);
			return str;
		};
		Func<string, object, string> resolveStringCallback = delegate(string str, object data)
		{
			RoomType roomType = Db.Get().RoomTypes.Get((string)data);
			return (roomType != null) ? str.Replace("{0}", roomType.Name) : str;
		};
		NoCoolant = CreateStatusItem("NoCoolant", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NotInAnyRoom = CreateStatusItem("NotInAnyRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NotInRequiredRoom = CreateStatusItem("NotInRequiredRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NotInRequiredRoom.resolveStringCallback = resolveStringCallback;
		NotInRecommendedRoom = CreateStatusItem("NotInRecommendedRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NotInRecommendedRoom.resolveStringCallback = resolveStringCallback;
		MercuryLight_Charging = CreateStatusItem("MercuryLight_Charging", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MercuryLight_Charging.resolveStringCallback = delegate(string str, object data)
		{
			MercuryLight.Instance instance15 = (MercuryLight.Instance)data;
			str = string.Format(str, GameUtil.GetFormattedPercent(instance15.ChargeLevel * 100f));
			return str;
		};
		MercuryLight_Charging.resolveTooltipCallback = delegate(string str, object data)
		{
			MercuryLight.Instance instance14 = (MercuryLight.Instance)data;
			str = string.Format(str, GameUtil.GetFormattedTime((1f - instance14.ChargeLevel) * instance14.def.TURN_ON_DELAY));
			return str;
		};
		MercuryLight_Depleating = CreateStatusItem("MercuryLight_Depleating", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MercuryLight_Depleating.resolveStringCallback = delegate(string str, object data)
		{
			MercuryLight.Instance instance13 = (MercuryLight.Instance)data;
			str = string.Format(str, GameUtil.GetFormattedPercent(instance13.ChargeLevel * 100f));
			return str;
		};
		MercuryLight_Charged = CreateStatusItem("MercuryLight_Charged", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MercuryLight_Depleated = CreateStatusItem("MercuryLight_Depleated", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		WaitingForRepairMaterials = CreateStatusItem("WaitingForRepairMaterials", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Exclamation, NotificationType.Neutral, allow_multiples: true, OverlayModes.None.ID, showWorldIcon: false);
		WaitingForRepairMaterials.resolveStringCallback = delegate(string str, object data)
		{
			KeyValuePair<Tag, float> keyValuePair = (KeyValuePair<Tag, float>)data;
			if (keyValuePair.Value != 0f)
			{
				string newValue13 = string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, keyValuePair.Key.ProperName(), GameUtil.GetFormattedMass(keyValuePair.Value));
				str = str.Replace("{ItemsRemaining}", newValue13);
			}
			return str;
		};
		WaitingForMaterials = new MaterialsStatusItem("WaitingForMaterials", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, allow_multiples: true, OverlayModes.None.ID);
		WaitingForMaterials.resolveStringCallback = delegate(string str, object data)
		{
			IFetchList obj3 = (IFetchList)data;
			string text6 = "";
			Dictionary<Tag, float> remaining = obj3.GetRemaining();
			if (remaining.Count > 0)
			{
				bool flag6 = true;
				foreach (KeyValuePair<Tag, float> item3 in remaining)
				{
					if (item3.Value != 0f)
					{
						if (!flag6)
						{
							text6 += "\n";
						}
						text6 = ((!Assets.IsTagCountable(item3.Key)) ? (text6 + string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, item3.Key.ProperName(), GameUtil.GetFormattedMass(item3.Value))) : (text6 + string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_UNITS, GameUtil.GetUnitFormattedName(item3.Key.ProperName(), item3.Value))));
						flag6 = false;
					}
				}
			}
			str = str.Replace("{ItemsRemaining}", text6);
			return str;
		};
		WaitingForHighEnergyParticles = new StatusItem("WaitingForRadiation", "BUILDING", "status_item_need_high_energy_particles", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		MeltingDown = CreateStatusItem("MeltingDown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		MissingFoundation = CreateStatusItem("MissingFoundation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NeutroniumUnminable = CreateStatusItem("NeutroniumUnminable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NeedGasIn = CreateStatusItem("NeedGasIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.GasConduits.ID);
		NeedGasIn.resolveStringCallback = delegate(string str, object data)
		{
			Tuple<ConduitType, Tag> tuple2 = (Tuple<ConduitType, Tag>)data;
			string newValue12 = string.Format(BUILDING.STATUSITEMS.NEEDGASIN.LINE_ITEM, tuple2.second.ProperName());
			str = str.Replace("{GasRequired}", newValue12);
			return str;
		};
		NeedGasOut = CreateStatusItem("NeedGasOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: true, OverlayModes.GasConduits.ID);
		NeedLiquidIn = CreateStatusItem("NeedLiquidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.LiquidConduits.ID);
		NeedLiquidIn.resolveStringCallback = delegate(string str, object data)
		{
			Tuple<ConduitType, Tag> tuple = (Tuple<ConduitType, Tag>)data;
			string newValue11 = string.Format(BUILDING.STATUSITEMS.NEEDLIQUIDIN.LINE_ITEM, tuple.second.ProperName());
			str = str.Replace("{LiquidRequired}", newValue11);
			return str;
		};
		NeedLiquidOut = CreateStatusItem("NeedLiquidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: true, OverlayModes.LiquidConduits.ID);
		NeedSolidIn = CreateStatusItem("NeedSolidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.SolidConveyor.ID);
		NeedSolidOut = CreateStatusItem("NeedSolidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: true, OverlayModes.SolidConveyor.ID);
		NeedResourceMass = CreateStatusItem("NeedResourceMass", "BUILDING", "status_item_need_resource", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NeedResourceMass.resolveStringCallback = delegate(string str, object data)
		{
			string text5 = "";
			EnergyGenerator.Formula formula = (EnergyGenerator.Formula)data;
			if (formula.inputs.Length != 0)
			{
				bool flag5 = true;
				EnergyGenerator.InputItem[] inputs = formula.inputs;
				for (int l = 0; l < inputs.Length; l++)
				{
					EnergyGenerator.InputItem inputItem = inputs[l];
					if (!flag5)
					{
						text5 += "\n";
						flag5 = false;
					}
					text5 += string.Format(BUILDING.STATUSITEMS.NEEDRESOURCEMASS.LINE_ITEM, inputItem.tag.ProperName());
				}
			}
			str = str.Replace("{ResourcesRequired}", text5);
			return str;
		};
		LiquidPipeEmpty = CreateStatusItem("LiquidPipeEmpty", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.LiquidConduits.ID);
		LiquidPipeObstructed = CreateStatusItem("LiquidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: true, OverlayModes.LiquidConduits.ID);
		GasPipeEmpty = CreateStatusItem("GasPipeEmpty", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.GasConduits.ID);
		GasPipeObstructed = CreateStatusItem("GasPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: true, OverlayModes.GasConduits.ID);
		SolidPipeObstructed = CreateStatusItem("SolidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: true, OverlayModes.SolidConveyor.ID);
		NeedPlant = CreateStatusItem("NeedPlant", "BUILDING", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NeedPower = CreateStatusItem("NeedPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Power.ID);
		NotEnoughPower = CreateStatusItem("NotEnoughPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Power.ID);
		PowerLoopDetected = CreateStatusItem("PowerLoopDetected", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Power.ID);
		CoolingWater = CreateStatusItem("CoolingWater", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		DispenseRequested = CreateStatusItem("DispenseRequested", "BUILDING", "status_item_exclamation", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NewDuplicantsAvailable = CreateStatusItem("NewDuplicantsAvailable", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NewDuplicantsAvailable.AddNotification();
		NewDuplicantsAvailable.notificationClickCallback = delegate
		{
			int idx = 0;
			for (int k = 0; k < Components.Telepads.Items.Count; k++)
			{
				if (Components.Telepads[k].GetComponent<KSelectable>().IsSelected)
				{
					idx = (k + 1) % Components.Telepads.Items.Count;
					break;
				}
			}
			Telepad targetTelepad = Components.Telepads[idx];
			int myWorldId = targetTelepad.GetMyWorldId();
			CameraController.Instance.ActiveWorldStarWipe(myWorldId, targetTelepad.transform.GetPosition(), 10f, delegate
			{
				SelectTool.Instance.Select(targetTelepad.GetComponent<KSelectable>());
			});
		};
		NoStorageFilterSet = CreateStatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoSuitMarker = CreateStatusItem("NoSuitMarker", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		SuitMarkerWrongSide = CreateStatusItem("suitMarkerWrongSide", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		SuitMarkerTraversalAnytime = CreateStatusItem("suitMarkerTraversalAnytime", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		SuitMarkerTraversalOnlyWhenRoomAvailable = CreateStatusItem("suitMarkerTraversalOnlyWhenRoomAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NoFishableWaterBelow = CreateStatusItem("NoFishableWaterBelow", "BUILDING", "status_item_no_fishable_water_below", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoPowerConsumers = CreateStatusItem("NoPowerConsumers", "BUILDING", "status_item_no_power_consumers", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Power.ID);
		NoWireConnected = CreateStatusItem("NoWireConnected", "BUILDING", "status_item_no_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: true, OverlayModes.Power.ID);
		NoLogicWireConnected = CreateStatusItem("NoLogicWireConnected", "BUILDING", "status_item_no_logic_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Logic.ID);
		NoTubeConnected = CreateStatusItem("NoTubeConnected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoTubeExits = CreateStatusItem("NoTubeExits", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		StoredCharge = CreateStatusItem("StoredCharge", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		StoredCharge.resolveStringCallback = delegate(string str, object data)
		{
			TravelTubeEntrance.SMInstance sMInstance3 = (TravelTubeEntrance.SMInstance)data;
			if (sMInstance3 != null)
			{
				str = string.Format(str, GameUtil.GetFormattedRoundedJoules(sMInstance3.master.AvailableJoules), GameUtil.GetFormattedRoundedJoules(sMInstance3.master.TotalCapacity), GameUtil.GetFormattedRoundedJoules(sMInstance3.master.UsageJoules));
			}
			return str;
		};
		PendingDeconstruction = CreateStatusItem("PendingDeconstruction", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PendingDeconstruction.conditionalOverlayCallback = ShowInUtilityOverlay;
		PendingDemolition = CreateStatusItem("PendingDemolition", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PendingDemolition.conditionalOverlayCallback = ShowInUtilityOverlay;
		PendingRepair = CreateStatusItem("PendingRepair", "BUILDING", "status_item_pending_repair", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		PendingRepair.resolveStringCallback = (string str, object data) => str.Replace("{DamageInfo}", ((Repairable.SMInstance)data).master.GetComponent<BuildingHP>().GetDamageSourceInfo().ToString());
		PendingRepair.conditionalOverlayCallback = (HashedString mode, object data) => true;
		RequiresSkillPerk = CreateStatusItem("RequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RequiresSkillPerk.resolveStringCallback = delegate(string str, object data)
		{
			string id3 = (string)data;
			SkillPerk perk3 = Db.Get().SkillPerks.Get(id3);
			List<Skill> skillsWithPerk3 = Db.Get().Skills.GetSkillsWithPerk(perk3);
			List<string> list3 = new List<string>();
			foreach (Skill item4 in skillsWithPerk3)
			{
				if (!item4.deprecated)
				{
					list3.Add(item4.Name);
				}
			}
			str = str.Replace("{Skills}", string.Join(", ", list3.ToArray()));
			return str;
		};
		DigRequiresSkillPerk = CreateStatusItem("DigRequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		DigRequiresSkillPerk.resolveStringCallback = RequiresSkillPerk.resolveStringCallback;
		ColonyLacksRequiredSkillPerk = CreateStatusItem("ColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ColonyLacksRequiredSkillPerk.resolveStringCallback = delegate(string str, object data)
		{
			string id2 = (string)data;
			SkillPerk perk2 = Db.Get().SkillPerks.Get(id2);
			List<Skill> skillsWithPerk2 = Db.Get().Skills.GetSkillsWithPerk(perk2);
			List<string> list2 = new List<string>();
			foreach (Skill item5 in skillsWithPerk2)
			{
				if (!item5.deprecated)
				{
					list2.Add(item5.Name);
				}
			}
			str = str.Replace("{Skills}", string.Join(", ", list2.ToArray()));
			return str;
		};
		ColonyLacksRequiredSkillPerk.resolveTooltipCallback = delegate(string str, object data)
		{
			string id = (string)data;
			SkillPerk perk = Db.Get().SkillPerks.Get(id);
			List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(perk);
			List<string> list = new List<string>();
			foreach (Skill item6 in skillsWithPerk)
			{
				if (!item6.deprecated)
				{
					list.Add(item6.Name);
				}
			}
			str = str.Replace("{Skills}", string.Join(", ", list.ToArray()));
			return str;
		};
		ClusterColonyLacksRequiredSkillPerk = CreateStatusItem("ClusterColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ClusterColonyLacksRequiredSkillPerk.resolveStringCallback = ColonyLacksRequiredSkillPerk.resolveStringCallback;
		ClusterColonyLacksRequiredSkillPerk.resolveTooltipCallback = ColonyLacksRequiredSkillPerk.resolveTooltipCallback;
		WorkRequiresMinion = CreateStatusItem("WorkRequiresMinion", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		SwitchStatusActive = CreateStatusItem("SwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		SwitchStatusInactive = CreateStatusItem("SwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		LogicSwitchStatusActive = CreateStatusItem("LogicSwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		LogicSwitchStatusInactive = CreateStatusItem("LogicSwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		LogicSensorStatusActive = CreateStatusItem("LogicSensorStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		LogicSensorStatusInactive = CreateStatusItem("LogicSensorStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PendingFish = CreateStatusItem("PendingFish", "BUILDING", "status_item_pending_fish", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PendingSwitchToggle = CreateStatusItem("PendingSwitchToggle", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PendingUpgrade = CreateStatusItem("PendingUpgrade", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PendingWork = CreateStatusItem("PendingWork", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		PowerButtonOff = CreateStatusItem("PowerButtonOff", "BUILDING", "status_item_power_button_off", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		PressureOk = CreateStatusItem("PressureOk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Oxygen.ID);
		UnderPressure = CreateStatusItem("UnderPressure", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Oxygen.ID);
		UnderPressure.resolveTooltipCallback = delegate(string str, object data)
		{
			float mass5 = (float)data;
			return str.Replace("{TargetPressure}", GameUtil.GetFormattedMass(mass5));
		};
		Unassigned = CreateStatusItem("Unassigned", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Rooms.ID);
		AssignedPublic = CreateStatusItem("AssignedPublic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Rooms.ID);
		UnderConstruction = CreateStatusItem("UnderConstruction", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		UnderConstructionNoWorker = CreateStatusItem("UnderConstructionNoWorker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Normal = CreateStatusItem("Normal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ManualGeneratorChargingUp = CreateStatusItem("ManualGeneratorChargingUp", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		ManualGeneratorReleasingEnergy = CreateStatusItem("ManualGeneratorReleasingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		GeneratorOffline = CreateStatusItem("GeneratorOffline", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Power.ID);
		Pipe = CreateStatusItem("Pipe", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.LiquidConduits.ID);
		Pipe.resolveStringCallback = delegate(string str, object data)
		{
			Conduit obj2 = (Conduit)data;
			int cell2 = Grid.PosToCell(obj2);
			ConduitFlow.ConduitContents contents2 = obj2.GetFlowManager().GetContents(cell2);
			string text4 = BUILDING.STATUSITEMS.PIPECONTENTS.EMPTY;
			if (contents2.mass > 0f)
			{
				Element element3 = ElementLoader.FindElementByHash(contents2.element);
				text4 = string.Format(BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS, GameUtil.GetFormattedMass(contents2.mass), element3.name, GameUtil.GetFormattedTemperature(contents2.temperature));
				if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && contents2.diseaseIdx != byte.MaxValue)
				{
					text4 += string.Format(BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS_WITH_DISEASE, GameUtil.GetFormattedDisease(contents2.diseaseIdx, contents2.diseaseCount, color: true));
				}
			}
			str = str.Replace("{Contents}", text4);
			return str;
		};
		Conveyor = CreateStatusItem("Conveyor", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.SolidConveyor.ID);
		Conveyor.resolveStringCallback = delegate(string str, object data)
		{
			int cell = Grid.PosToCell((SolidConduit)data);
			SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
			SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
			string text3 = BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.EMPTY;
			if (contents.pickupableHandle.IsValid())
			{
				Pickupable pickupable = solidConduitFlow.GetPickupable(contents.pickupableHandle);
				if ((bool)pickupable)
				{
					PrimaryElement component5 = pickupable.GetComponent<PrimaryElement>();
					float mass4 = component5.Mass;
					if (mass4 > 0f)
					{
						text3 = string.Format(BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS, GameUtil.GetFormattedMass(mass4), pickupable.GetProperName(), GameUtil.GetFormattedTemperature(component5.Temperature));
						if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && component5.DiseaseIdx != byte.MaxValue)
						{
							text3 += string.Format(BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS_WITH_DISEASE, GameUtil.GetFormattedDisease(component5.DiseaseIdx, component5.DiseaseCount, color: true));
						}
					}
				}
			}
			str = str.Replace("{Contents}", text3);
			return str;
		};
		FabricatorIdle = CreateStatusItem("FabricatorIdle", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FabricatorEmpty = CreateStatusItem("FabricatorEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		FabricatorLacksHEP = CreateStatusItem("FabricatorLacksHEP", "BUILDING", "status_item_need_high_energy_particles", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		FabricatorLacksHEP.resolveStringCallback = delegate(string str, object data)
		{
			ComplexFabricator complexFabricator3 = (ComplexFabricator)data;
			if (complexFabricator3 != null)
			{
				int num9 = complexFabricator3.HighestHEPQueued();
				HighEnergyParticleStorage component4 = complexFabricator3.GetComponent<HighEnergyParticleStorage>();
				str = str.Replace("{HEPRequired}", num9.ToString());
				str = str.Replace("{CurrentHEP}", component4.Particles.ToString());
			}
			return str;
		};
		FossilMineIdle = CreateStatusItem("FossilIdle", "CODEX.STORY_TRAITS.FOSSILHUNT", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FossilMineEmpty = CreateStatusItem("FossilEmpty", "CODEX.STORY_TRAITS.FOSSILHUNT", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		FossilMinePendingWork = CreateStatusItem("FossilMinePendingWork", "CODEX.STORY_TRAITS.FOSSILHUNT", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		FossilEntombed = new StatusItem("FossilEntombed", "CODEX.STORY_TRAITS.FOSSILHUNT", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		Toilet = CreateStatusItem("Toilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Toilet.resolveStringCallback = delegate(string str, object data)
		{
			Toilet.StatesInstance statesInstance8 = (Toilet.StatesInstance)data;
			if (statesInstance8 != null)
			{
				str = str.Replace("{FlushesRemaining}", statesInstance8.GetFlushesRemaining().ToString());
			}
			return str;
		};
		ToiletNeedsEmptying = CreateStatusItem("ToiletNeedsEmptying", "BUILDING", "status_item_toilet_needs_emptying", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		DesalinatorNeedsEmptying = CreateStatusItem("DesalinatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		MilkSeparatorNeedsEmptying = CreateStatusItem("MilkSeparatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		Unusable = CreateStatusItem("Unusable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoResearchSelected = CreateStatusItem("NoResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoResearchSelected.AddNotification();
		StatusItem noResearchSelected = NoResearchSelected;
		noResearchSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noResearchSelected.resolveTooltipCallback, (Func<string, object, string>)delegate(string str, object data)
		{
			string newValue10 = GameInputMapping.FindEntry(Action.ManageResearch).mKeyCode.ToString();
			str = str.Replace("{RESEARCH_MENU_KEY}", newValue10);
			return str;
		});
		NoResearchSelected.notificationClickCallback = delegate
		{
			ManagementMenu.Instance.OpenResearch();
		};
		NoApplicableResearchSelected = CreateStatusItem("NoApplicableResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoApplicableResearchSelected.AddNotification();
		NoApplicableAnalysisSelected = CreateStatusItem("NoApplicableAnalysisSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoApplicableAnalysisSelected.AddNotification();
		StatusItem noApplicableAnalysisSelected = NoApplicableAnalysisSelected;
		noApplicableAnalysisSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noApplicableAnalysisSelected.resolveTooltipCallback, (Func<string, object, string>)delegate(string str, object data)
		{
			string newValue9 = GameInputMapping.FindEntry(Action.ManageStarmap).mKeyCode.ToString();
			str = str.Replace("{STARMAP_MENU_KEY}", newValue9);
			return str;
		});
		NoApplicableAnalysisSelected.notificationClickCallback = delegate
		{
			ManagementMenu.Instance.OpenStarmap();
		};
		NoResearchOrDestinationSelected = CreateStatusItem("NoResearchOrDestinationSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		StatusItem noResearchOrDestinationSelected = NoResearchOrDestinationSelected;
		noResearchOrDestinationSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noResearchOrDestinationSelected.resolveTooltipCallback, (Func<string, object, string>)delegate(string str, object data)
		{
			string newValue7 = GameInputMapping.FindEntry(Action.ManageStarmap).mKeyCode.ToString();
			str = str.Replace("{STARMAP_MENU_KEY}", newValue7);
			string newValue8 = GameInputMapping.FindEntry(Action.ManageResearch).mKeyCode.ToString();
			str = str.Replace("{RESEARCH_MENU_KEY}", newValue8);
			return str;
		});
		NoResearchOrDestinationSelected.AddNotification();
		ValveRequest = CreateStatusItem("ValveRequest", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ValveRequest.resolveStringCallback = delegate(string str, object data)
		{
			Valve valve = (Valve)data;
			str = str.Replace("{QueuedMaxFlow}", GameUtil.GetFormattedMass(valve.QueuedMaxFlow, GameUtil.TimeSlice.PerSecond));
			return str;
		};
		EmittingLight = CreateStatusItem("EmittingLight", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		EmittingLight.resolveStringCallback = delegate(string str, object data)
		{
			string newValue6 = GameInputMapping.FindEntry(Action.Overlay5).mKeyCode.ToString();
			str = str.Replace("{LightGridOverlay}", newValue6);
			return str;
		};
		KettleInsuficientSolids = CreateStatusItem("KettleInsuficientSolids", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		KettleInsuficientSolids.resolveStringCallback = delegate(string str, object data)
		{
			IceKettle.Instance instance12 = (IceKettle.Instance)data;
			str = string.Format(str, GameUtil.GetFormattedMass(instance12.def.KGToMeltPerBatch));
			return str;
		};
		KettleInsuficientFuel = CreateStatusItem("KettleInsuficientFuel", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		KettleInsuficientFuel.resolveStringCallback = delegate(string str, object data)
		{
			IceKettle.Instance instance11 = (IceKettle.Instance)data;
			str = string.Format(str, GameUtil.GetFormattedMass(instance11.FuelRequiredForNextBratch));
			return str;
		};
		KettleInsuficientLiquidSpace = CreateStatusItem("KettleInsuficientLiquidSpace", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		KettleInsuficientLiquidSpace.resolveStringCallback = delegate(string str, object data)
		{
			IceKettle.Instance instance10 = (IceKettle.Instance)data;
			str = string.Format(str, GameUtil.GetFormattedMass(instance10.LiquidStored), GameUtil.GetFormattedMass(instance10.LiquidTankCapacity), GameUtil.GetFormattedMass(instance10.def.KGToMeltPerBatch));
			return str;
		};
		KettleMelting = CreateStatusItem("KettleMelting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		KettleMelting.resolveStringCallback = delegate(string str, object data)
		{
			IceKettle.Instance instance9 = (IceKettle.Instance)data;
			str = string.Format(str, GameUtil.GetFormattedTemperature(instance9.def.TargetTemperature));
			return str;
		};
		RationBoxContents = CreateStatusItem("RationBoxContents", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RationBoxContents.resolveStringCallback = delegate(string str, object data)
		{
			RationBox rationBox = (RationBox)data;
			if (rationBox == null)
			{
				return str;
			}
			Storage component2 = rationBox.GetComponent<Storage>();
			if (component2 == null)
			{
				return str;
			}
			float num8 = 0f;
			foreach (GameObject item7 in component2.items)
			{
				Edible component3 = item7.GetComponent<Edible>();
				if ((bool)component3)
				{
					num8 += component3.Calories;
				}
			}
			str = str.Replace("{Stored}", GameUtil.GetFormattedCalories(num8));
			return str;
		};
		EmittingElement = CreateStatusItem("EmittingElement", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		EmittingElement.resolveStringCallback = delegate(string str, object data)
		{
			IElementEmitter elementEmitter2 = (IElementEmitter)data;
			string newValue5 = ElementLoader.FindElementByHash(elementEmitter2.Element).tag.ProperName();
			str = str.Replace("{ElementType}", newValue5);
			str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter2.AverageEmitRate, GameUtil.TimeSlice.PerSecond));
			return str;
		};
		EmittingOxygenAvg = CreateStatusItem("EmittingOxygenAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		EmittingOxygenAvg.resolveStringCallback = delegate(string str, object data)
		{
			Sublimates sublimates4 = (Sublimates)data;
			str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates4.AvgFlowRate(), GameUtil.TimeSlice.PerSecond));
			return str;
		};
		EmittingGasAvg = CreateStatusItem("EmittingGasAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		EmittingGasAvg.resolveStringCallback = delegate(string str, object data)
		{
			Sublimates sublimates3 = (Sublimates)data;
			str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates3.info.sublimatedElement).name);
			str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates3.AvgFlowRate(), GameUtil.TimeSlice.PerSecond));
			return str;
		};
		EmittingBlockedHighPressure = CreateStatusItem("EmittingBlockedHighPressure", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		EmittingBlockedHighPressure.resolveStringCallback = delegate(string str, object data)
		{
			Sublimates sublimates2 = (Sublimates)data;
			str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates2.info.sublimatedElement).name);
			return str;
		};
		EmittingBlockedLowTemperature = CreateStatusItem("EmittingBlockedLowTemperature", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		EmittingBlockedLowTemperature.resolveStringCallback = delegate(string str, object data)
		{
			Sublimates sublimates = (Sublimates)data;
			str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
			return str;
		};
		PumpingLiquidOrGas = CreateStatusItem("PumpingLiquidOrGas", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.LiquidConduits.ID);
		PumpingLiquidOrGas.resolveStringCallback = delegate(string str, object data)
		{
			HandleVector<int>.Handle handle = (HandleVector<int>.Handle)data;
			float averageRate = Game.Instance.accumulators.GetAverageRate(handle);
			str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(averageRate, GameUtil.TimeSlice.PerSecond));
			return str;
		};
		PipeMayMelt = CreateStatusItem("PipeMayMelt", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoLiquidElementToPump = CreateStatusItem("NoLiquidElementToPump", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.LiquidConduits.ID);
		NoGasElementToPump = CreateStatusItem("NoGasElementToPump", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.GasConduits.ID);
		NoFilterElementSelected = CreateStatusItem("NoFilterElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NoLureElementSelected = CreateStatusItem("NoLureElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ElementConsumer = CreateStatusItem("ElementConsumer", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: true, OverlayModes.None.ID);
		ElementConsumer.resolveStringCallback = delegate(string str, object data)
		{
			ElementConsumer elementConsumer = (ElementConsumer)data;
			string newValue4 = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag.ProperName();
			str = str.Replace("{ElementTypes}", newValue4);
			str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementConsumer.AverageConsumeRate, GameUtil.TimeSlice.PerSecond));
			return str;
		};
		ElementEmitterOutput = CreateStatusItem("ElementEmitterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: true, OverlayModes.None.ID);
		ElementEmitterOutput.resolveStringCallback = delegate(string str, object data)
		{
			ElementEmitter elementEmitter = (ElementEmitter)data;
			if (elementEmitter != null)
			{
				str = str.Replace("{ElementTypes}", elementEmitter.outputElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.outputElement.massGenerationRate / elementEmitter.emissionFrequency, GameUtil.TimeSlice.PerSecond));
			}
			return str;
		};
		AwaitingWaste = CreateStatusItem("AwaitingWaste", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: true, OverlayModes.None.ID);
		AwaitingCompostFlip = CreateStatusItem("AwaitingCompostFlip", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: true, OverlayModes.None.ID);
		JoulesAvailable = CreateStatusItem("JoulesAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		JoulesAvailable.resolveStringCallback = delegate(string str, object data)
		{
			Battery battery = (Battery)data;
			str = str.Replace("{JoulesAvailable}", GameUtil.GetFormattedJoules(battery.JoulesAvailable));
			str = str.Replace("{JoulesCapacity}", GameUtil.GetFormattedJoules(battery.Capacity));
			return str;
		};
		Wattage = CreateStatusItem("Wattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		Wattage.resolveStringCallback = delegate(string str, object data)
		{
			Generator generator3 = (Generator)data;
			str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(generator3.WattageRating));
			return str;
		};
		SolarPanelWattage = CreateStatusItem("SolarPanelWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		SolarPanelWattage.resolveStringCallback = delegate(string str, object data)
		{
			SolarPanel solarPanel = (SolarPanel)data;
			str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(solarPanel.CurrentWattage));
			return str;
		};
		ModuleSolarPanelWattage = CreateStatusItem("ModuleSolarPanelWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		ModuleSolarPanelWattage.resolveStringCallback = delegate(string str, object data)
		{
			ModuleSolarPanel moduleSolarPanel = (ModuleSolarPanel)data;
			str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(moduleSolarPanel.CurrentWattage));
			return str;
		};
		SteamTurbineWattage = CreateStatusItem("SteamTurbineWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		SteamTurbineWattage.resolveStringCallback = delegate(string str, object data)
		{
			SteamTurbine steamTurbine = (SteamTurbine)data;
			str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage));
			return str;
		};
		Wattson = CreateStatusItem("Wattson", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Wattson.resolveStringCallback = delegate(string str, object data)
		{
			Telepad telepad = (Telepad)data;
			str = ((GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver()) ? ((string)BUILDING.STATUSITEMS.WATTSONGAMEOVER.NAME) : ((!telepad.GetComponent<Operational>().IsOperational) ? str.Replace("{TimeRemaining}", BUILDING.STATUSITEMS.WATTSON.UNAVAILABLE) : str.Replace("{TimeRemaining}", GameUtil.GetFormattedCycles(telepad.GetTimeRemaining()))));
			return str;
		};
		FlushToilet = CreateStatusItem("FlushToilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FlushToilet.resolveStringCallback = delegate(string str, object data)
		{
			FlushToilet.SMInstance sMInstance2 = (FlushToilet.SMInstance)data;
			return BUILDING.STATUSITEMS.FLUSHTOILET.NAME.Replace("{toilet}", sMInstance2.master.GetProperName());
		};
		FlushToilet.resolveTooltipCallback = (string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILET.TOOLTIP;
		FlushToiletInUse = CreateStatusItem("FlushToiletInUse", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FlushToiletInUse.resolveStringCallback = delegate(string str, object data)
		{
			FlushToilet.SMInstance sMInstance = (FlushToilet.SMInstance)data;
			return BUILDING.STATUSITEMS.FLUSHTOILETINUSE.NAME.Replace("{toilet}", sMInstance.master.GetProperName());
		};
		FlushToiletInUse.resolveTooltipCallback = (string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILETINUSE.TOOLTIP;
		WireNominal = CreateStatusItem("WireNominal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		WireConnected = CreateStatusItem("WireConnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		WireDisconnected = CreateStatusItem("WireDisconnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.Power.ID);
		Overheated = CreateStatusItem("Overheated", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID);
		Overloaded = CreateStatusItem("Overloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID);
		LogicOverloaded = CreateStatusItem("LogicOverloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID);
		Cooling = CreateStatusItem("Cooling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Func<string, object, string> resolveStringCallback2 = delegate(string str, object data)
		{
			AirConditioner airConditioner2 = (AirConditioner)data;
			return string.Format(str, GameUtil.GetFormattedTemperature(airConditioner2.lastGasTemp));
		};
		CoolingStalledColdGas = CreateStatusItem("CoolingStalledColdGas", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		CoolingStalledColdGas.resolveStringCallback = resolveStringCallback2;
		CoolingStalledColdLiquid = CreateStatusItem("CoolingStalledColdLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		CoolingStalledColdLiquid.resolveStringCallback = resolveStringCallback2;
		Func<string, object, string> resolveStringCallback3 = delegate(string str, object data)
		{
			AirConditioner airConditioner = (AirConditioner)data;
			return string.Format(str, GameUtil.GetFormattedTemperature(airConditioner.lastEnvTemp), GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp), GameUtil.GetFormattedTemperature(airConditioner.maxEnvironmentDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative));
		};
		CoolingStalledHotEnv = CreateStatusItem("CoolingStalledHotEnv", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		CoolingStalledHotEnv.resolveStringCallback = resolveStringCallback3;
		CoolingStalledHotLiquid = CreateStatusItem("CoolingStalledHotLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		CoolingStalledHotLiquid.resolveStringCallback = resolveStringCallback3;
		MissingRequirements = CreateStatusItem("MissingRequirements", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GettingReady = CreateStatusItem("GettingReady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Working = CreateStatusItem("Working", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NeedsValidRegion = CreateStatusItem("NeedsValidRegion", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NeedSeed = CreateStatusItem("NeedSeed", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		AwaitingSeedDelivery = CreateStatusItem("AwaitingSeedDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		AwaitingBaitDelivery = CreateStatusItem("AwaitingBaitDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NoAvailableSeed = CreateStatusItem("NoAvailableSeed", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		NeedEgg = CreateStatusItem("NeedEgg", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		AwaitingEggDelivery = CreateStatusItem("AwaitingEggDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NoAvailableEgg = CreateStatusItem("NoAvailableEgg", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		Grave = CreateStatusItem("Grave", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Grave.resolveStringCallback = delegate(string str, object data)
		{
			Grave.StatesInstance statesInstance7 = (Grave.StatesInstance)data;
			string text2 = str.Replace("{DeadDupe}", statesInstance7.master.graveName);
			string[] strings = LocString.GetStrings(typeof(NAMEGEN.GRAVE.EPITAPHS));
			int num7 = statesInstance7.master.epitaphIdx % strings.Length;
			return text2.Replace("{Epitaph}", strings[num7]);
		};
		GraveEmpty = CreateStatusItem("GraveEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		CannotCoolFurther = CreateStatusItem("CannotCoolFurther", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		CannotCoolFurther.resolveTooltipCallback = delegate(string str, object data)
		{
			float temp2 = (float)data;
			return str.Replace("{0}", GameUtil.GetFormattedTemperature(temp2));
		};
		BuildingDisabled = CreateStatusItem("BuildingDisabled", "BUILDING", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Expired = CreateStatusItem("Expired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PumpingStation = CreateStatusItem("PumpingStation", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PumpingStation.resolveStringCallback = delegate(string str, object data)
		{
			LiquidPumpingStation liquidPumpingStation = (LiquidPumpingStation)data;
			return (liquidPumpingStation != null) ? liquidPumpingStation.ResolveString(str) : str;
		};
		EmptyPumpingStation = CreateStatusItem("EmptyPumpingStation", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		WellPressurizing = CreateStatusItem("WellPressurizing", BUILDING.STATUSITEMS.WELL_PRESSURIZING.NAME, BUILDING.STATUSITEMS.WELL_PRESSURIZING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		WellPressurizing.resolveStringCallback = delegate(string str, object data)
		{
			OilWellCap.StatesInstance statesInstance6 = (OilWellCap.StatesInstance)data;
			return (statesInstance6 != null) ? string.Format(str, GameUtil.GetFormattedPercent(100f * statesInstance6.GetPressurePercent())) : str;
		};
		WellOverpressure = CreateStatusItem("WellOverpressure", BUILDING.STATUSITEMS.WELL_OVERPRESSURE.NAME, BUILDING.STATUSITEMS.WELL_OVERPRESSURE.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ReleasingPressure = CreateStatusItem("ReleasingPressure", BUILDING.STATUSITEMS.RELEASING_PRESSURE.NAME, BUILDING.STATUSITEMS.RELEASING_PRESSURE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ReactorMeltdown = CreateStatusItem("ReactorMeltdown", BUILDING.STATUSITEMS.REACTORMELTDOWN.NAME, BUILDING.STATUSITEMS.REACTORMELTDOWN.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID);
		TooCold = CreateStatusItem("TooCold", BUILDING.STATUSITEMS.TOO_COLD.NAME, BUILDING.STATUSITEMS.TOO_COLD.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		IncubatorProgress = CreateStatusItem("IncubatorProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		IncubatorProgress.resolveStringCallback = delegate(string str, object data)
		{
			EggIncubator eggIncubator = (EggIncubator)data;
			str = str.Replace("{Percent}", GameUtil.GetFormattedPercent(eggIncubator.GetProgress() * 100f));
			return str;
		};
		HabitatNeedsEmptying = CreateStatusItem("HabitatNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		DetectorScanning = CreateStatusItem("DetectorScanning", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		IncomingMeteors = CreateStatusItem("IncomingMeteors", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		HasGantry = CreateStatusItem("HasGantry", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MissingGantry = CreateStatusItem("MissingGantry", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		DisembarkingDuplicant = CreateStatusItem("DisembarkingDuplicant", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		RocketName = CreateStatusItem("RocketName", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RocketName.resolveStringCallback = delegate(string str, object data)
		{
			RocketModule rocketModule2 = (RocketModule)data;
			return (rocketModule2 != null) ? str.Replace("{0}", rocketModule2.GetParentRocketName()) : str;
		};
		RocketName.resolveTooltipCallback = delegate(string str, object data)
		{
			RocketModule rocketModule = (RocketModule)data;
			return (rocketModule != null) ? str.Replace("{0}", rocketModule.GetParentRocketName()) : str;
		};
		LandedRocketLacksPassengerModule = CreateStatusItem("LandedRocketLacksPassengerModule", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		PathNotClear = new StatusItem("PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		PathNotClear.resolveTooltipCallback = delegate(string str, object data)
		{
			ConditionFlightPathIsClear conditionFlightPathIsClear = (ConditionFlightPathIsClear)data;
			if (conditionFlightPathIsClear != null)
			{
				str = string.Format(str, conditionFlightPathIsClear.GetObstruction());
			}
			return str;
		};
		InvalidPortOverlap = CreateStatusItem("InvalidPortOverlap", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		InvalidPortOverlap.AddNotification();
		EmergencyPriority = CreateStatusItem("EmergencyPriority", BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.TOOLTIP, "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID);
		EmergencyPriority.AddNotification(null, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_NAME, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_TOOLTIP);
		SkillPointsAvailable = CreateStatusItem("SkillPointsAvailable", BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.NAME, BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.TOOLTIP, "status_item_jobs", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		Baited = CreateStatusItem("Baited", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		Baited.resolveStringCallback = delegate(string str, object data)
		{
			Element element2 = ElementLoader.FindElementByName(((CreatureBait.StatesInstance)data).master.baitElement.ToString());
			str = str.Replace("{0}", element2.name);
			return str;
		};
		Baited.resolveTooltipCallback = delegate(string str, object data)
		{
			Element element = ElementLoader.FindElementByName(((CreatureBait.StatesInstance)data).master.baitElement.ToString());
			str = str.Replace("{0}", element.name);
			return str;
		};
		TanningLightSufficient = CreateStatusItem("TanningLightSufficient", BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.NAME, BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		TanningLightInsufficient = CreateStatusItem("TanningLightInsufficient", BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.NAME, BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		HotTubWaterTooCold = CreateStatusItem("HotTubWaterTooCold", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		HotTubWaterTooCold.resolveStringCallback = delegate(string str, object data)
		{
			HotTub hotTub3 = (HotTub)data;
			str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub3.minimumWaterTemperature));
			return str;
		};
		HotTubTooHot = CreateStatusItem("HotTubTooHot", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		HotTubTooHot.resolveStringCallback = delegate(string str, object data)
		{
			HotTub hotTub2 = (HotTub)data;
			str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub2.maxOperatingTemperature));
			return str;
		};
		HotTubFilling = CreateStatusItem("HotTubFilling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		HotTubFilling.resolveStringCallback = delegate(string str, object data)
		{
			HotTub hotTub = (HotTub)data;
			str = str.Replace("{fullness}", GameUtil.GetFormattedPercent(hotTub.PercentFull));
			return str;
		};
		WindTunnelIntake = CreateStatusItem("WindTunnelIntake", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		WarpPortalCharging = CreateStatusItem("WarpPortalCharging", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		WarpPortalCharging.resolveStringCallback = delegate(string str, object data)
		{
			_ = (WarpPortal)data;
			str = str.Replace("{charge}", GameUtil.GetFormattedPercent(100f * (((WarpPortal)data).rechargeProgress / 3000f)));
			return str;
		};
		WarpPortalCharging.resolveTooltipCallback = delegate(string str, object data)
		{
			_ = (WarpPortal)data;
			str = str.Replace("{cycles}", $"{(3000f - ((WarpPortal)data).rechargeProgress) / 600f:0.0}");
			return str;
		};
		WarpConduitPartnerDisabled = CreateStatusItem("WarpConduitPartnerDisabled", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		WarpConduitPartnerDisabled.resolveStringCallback = (string str, object data) => str.Replace("{x}", data.ToString());
		CollectingHEP = CreateStatusItem("CollectingHEP", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.Radiation.ID, showWorldIcon: false);
		CollectingHEP.resolveStringCallback = (string str, object data) => str.Replace("{x}", ((HighEnergyParticleSpawner)data).PredictedPerCycleConsumptionRate.ToString());
		InOrbit = CreateStatusItem("InOrbit", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		InOrbit.resolveStringCallback = delegate(string str, object data)
		{
			ClusterGridEntity clusterGridEntity2 = (ClusterGridEntity)data;
			return str.Replace("{Destination}", clusterGridEntity2.Name);
		};
		WaitingToLand = CreateStatusItem("WaitingToLand", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		WaitingToLand.resolveStringCallback = delegate(string str, object data)
		{
			ClusterGridEntity clusterGridEntity = (ClusterGridEntity)data;
			return str.Replace("{Destination}", clusterGridEntity.Name);
		};
		InFlight = CreateStatusItem("InFlight", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		InFlight.resolveStringCallback = delegate(string str, object data)
		{
			ClusterTraveler clusterTraveler2 = (ClusterTraveler)data;
			ClusterDestinationSelector component = clusterTraveler2.GetComponent<ClusterDestinationSelector>();
			RocketClusterDestinationSelector rocketClusterDestinationSelector = component as RocketClusterDestinationSelector;
			ClusterGrid.Instance.GetLocationDescription(component.GetDestination(), out var _, out var label, out var _);
			if (rocketClusterDestinationSelector != null)
			{
				LaunchPad destinationPad = rocketClusterDestinationSelector.GetDestinationPad();
				string newValue3 = ((destinationPad != null) ? destinationPad.GetProperName() : UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE.ToString());
				return str.Replace("{Destination_Asteroid}", label).Replace("{Destination_Pad}", newValue3).Replace("{ETA}", GameUtil.GetFormattedCycles(clusterTraveler2.TravelETA()));
			}
			return str.Replace("{Destination_Asteroid}", label).Replace("{ETA}", GameUtil.GetFormattedCycles(clusterTraveler2.TravelETA()));
		};
		DestinationOutOfRange = CreateStatusItem("DestinationOutOfRange", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		DestinationOutOfRange.resolveStringCallback = delegate(string str, object data)
		{
			ClusterTraveler clusterTraveler = (ClusterTraveler)data;
			str = str.Replace("{Range}", GameUtil.GetFormattedRocketRange(clusterTraveler.GetComponent<CraftModuleInterface>().RangeInTiles, displaySuffix: false));
			return str.Replace("{Distance}", clusterTraveler.RemainingTravelNodes() + " " + UI.CLUSTERMAP.TILES);
		};
		RocketStranded = CreateStatusItem("RocketStranded", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		MissionControlAssistingRocket = CreateStatusItem("MissionControlAssistingRocket", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MissionControlAssistingRocket.resolveStringCallback = delegate(string str, object data)
		{
			Spacecraft spacecraft2 = data as Spacecraft;
			Clustercraft clustercraft2 = data as Clustercraft;
			return str.Replace("{0}", (spacecraft2 != null) ? spacecraft2.rocketName : clustercraft2.Name);
		};
		NoRocketsToMissionControlBoost = CreateStatusItem("NoRocketsToMissionControlBoost", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NoRocketsToMissionControlClusterBoost = CreateStatusItem("NoRocketsToMissionControlClusterBoost", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NoRocketsToMissionControlClusterBoost.resolveStringCallback = delegate(string str, object data)
		{
			if (str.Contains("{0}"))
			{
				str = str.Replace("{0}", 2.ToString());
			}
			return str;
		};
		MissionControlBoosted = CreateStatusItem("MissionControlBoosted", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MissionControlBoosted.resolveStringCallback = delegate(string str, object data)
		{
			Spacecraft spacecraft = data as Spacecraft;
			Clustercraft clustercraft = data as Clustercraft;
			str = str.Replace("{0}", GameUtil.GetFormattedPercent(20.000004f));
			if (str.Contains("{1}"))
			{
				str = str.Replace("{1}", GameUtil.GetFormattedTime(spacecraft?.controlStationBuffTimeRemaining ?? clustercraft.controlStationBuffTimeRemaining));
			}
			return str;
		};
		TransitTubeEntranceWaxReady = CreateStatusItem("TransitTubeEntranceWaxReady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		TransitTubeEntranceWaxReady.resolveStringCallback = delegate(string str, object data)
		{
			TravelTubeEntrance travelTubeEntrance = data as TravelTubeEntrance;
			str = str.Replace("{0}", GameUtil.GetFormattedMass(travelTubeEntrance.waxPerLaunch));
			str = str.Replace("{1}", travelTubeEntrance.WaxLaunchesAvailable.ToString());
			return str;
		};
		SpecialCargoBayClusterCritterStored = CreateStatusItem("SpecialCargoBayClusterCritterStored", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		SpecialCargoBayClusterCritterStored.resolveStringCallback = delegate(string str, object data)
		{
			SpecialCargoBayClusterReceptacle specialCargoBayClusterReceptacle = data as SpecialCargoBayClusterReceptacle;
			if (specialCargoBayClusterReceptacle.Occupant != null)
			{
				str = str.Replace("{0}", specialCargoBayClusterReceptacle.Occupant.GetProperName());
			}
			return str;
		};
		RailgunpayloadNeedsEmptying = CreateStatusItem("RailgunpayloadNeedsEmptying", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		AwaitingEmptyBuilding = CreateStatusItem("AwaitingEmptyBuilding", "BUILDING", "action_empty_contents", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		DuplicantActivationRequired = CreateStatusItem("DuplicantActivationRequired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RocketChecklistIncomplete = CreateStatusItem("RocketChecklistIncomplete", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		RocketCargoEmptying = CreateStatusItem("RocketCargoEmptying", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RocketCargoFilling = CreateStatusItem("RocketCargoFilling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RocketCargoFull = CreateStatusItem("RocketCargoFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FlightAllCargoFull = CreateStatusItem("FlightAllCargoFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FlightCargoRemaining = CreateStatusItem("FlightCargoRemaining", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FlightCargoRemaining.resolveStringCallback = delegate(string str, object data)
		{
			float mass3 = (float)data;
			return str.Replace("{0}", GameUtil.GetFormattedMass(mass3));
		};
		PilotNeeded = CreateStatusItem("PilotNeeded", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		PilotNeeded.resolveStringCallback = delegate(string str, object data)
		{
			RocketControlStation master = ((RocketControlStation.StatesInstance)data).master;
			return str.Replace("{timeRemaining}", GameUtil.GetFormattedTime(master.TimeRemaining));
		};
		AutoPilotActive = CreateStatusItem("AutoPilotActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		InvalidMaskStationConsumptionState = CreateStatusItem("InvalidMaskStationConsumptionState", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ClusterTelescopeAllWorkComplete = CreateStatusItem("ClusterTelescopeAllWorkComplete", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RocketPlatformCloseToCeiling = CreateStatusItem("RocketPlatformCloseToCeiling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		RocketPlatformCloseToCeiling.resolveStringCallback = (string str, object data) => str.Replace("{distance}", data.ToString());
		ModuleGeneratorPowered = CreateStatusItem("ModuleGeneratorPowered", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		ModuleGeneratorPowered.resolveStringCallback = delegate(string str, object data)
		{
			Generator generator2 = (Generator)data;
			str = str.Replace("{ActiveWattage}", GameUtil.GetFormattedWattage(generator2.WattageRating));
			str = str.Replace("{MaxWattage}", GameUtil.GetFormattedWattage(generator2.WattageRating));
			return str;
		};
		ModuleGeneratorNotPowered = CreateStatusItem("ModuleGeneratorNotPowered", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.Power.ID);
		ModuleGeneratorNotPowered.resolveStringCallback = delegate(string str, object data)
		{
			Generator generator = (Generator)data;
			str = str.Replace("{ActiveWattage}", GameUtil.GetFormattedWattage(0f));
			str = str.Replace("{MaxWattage}", GameUtil.GetFormattedWattage(generator.WattageRating));
			return str;
		};
		InOrbitRequired = CreateStatusItem("InOrbitRequired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		ReactorRefuelDisabled = CreateStatusItem("ReactorRefuelDisabled", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FridgeCooling = CreateStatusItem("FridgeCooling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FridgeCooling.resolveStringCallback = delegate(string str, object data)
		{
			RefrigeratorController.StatesInstance statesInstance5 = (RefrigeratorController.StatesInstance)data;
			str = str.Replace("{UsedPower}", GameUtil.GetFormattedWattage(statesInstance5.GetNormalPower())).Replace("{MaxPower}", GameUtil.GetFormattedWattage(statesInstance5.GetNormalPower()));
			return str;
		};
		FridgeSteady = CreateStatusItem("FridgeSteady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		FridgeSteady.resolveStringCallback = delegate(string str, object data)
		{
			RefrigeratorController.StatesInstance statesInstance4 = (RefrigeratorController.StatesInstance)data;
			str = str.Replace("{UsedPower}", GameUtil.GetFormattedWattage(statesInstance4.GetSaverPower())).Replace("{MaxPower}", GameUtil.GetFormattedWattage(statesInstance4.GetNormalPower()));
			return str;
		};
		TrapNeedsArming = CreateStatusItem("CREATURE_REUSABLE_TRAP.NEEDS_ARMING", "BUILDING", "status_item_bait", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		TrapArmed = CreateStatusItem("CREATURE_REUSABLE_TRAP.READY", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		TrapHasCritter = CreateStatusItem("CREATURE_REUSABLE_TRAP.SPRUNG", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		TrapHasCritter.resolveTooltipCallback = delegate(string str, object data)
		{
			string newValue2 = "";
			if (data != null)
			{
				newValue2 = ((GameObject)data).GetComponent<KPrefabID>().GetProperName();
			}
			str = str.Replace("{0}", newValue2);
			return str;
		};
		RailGunCooldown = CreateStatusItem("RailGunCooldown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RailGunCooldown.resolveStringCallback = delegate(string str, object data)
		{
			RailGun.StatesInstance statesInstance3 = (RailGun.StatesInstance)data;
			str = str.Replace("{timeleft}", GameUtil.GetFormattedTime(statesInstance3.sm.cooldownTimer.Get(statesInstance3)));
			return str;
		};
		RailGunCooldown.resolveTooltipCallback = delegate(string str, object data)
		{
			_ = (RailGun.StatesInstance)data;
			str = str.Replace("{x}", 6.ToString());
			return str;
		};
		NoSurfaceSight = new StatusItem("NOSURFACESIGHT", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		LimitValveLimitReached = CreateStatusItem("LimitValveLimitReached", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		LimitValveLimitNotReached = CreateStatusItem("LimitValveLimitNotReached", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		LimitValveLimitNotReached.resolveStringCallback = delegate(string str, object data)
		{
			LimitValve limitValve = (LimitValve)data;
			string text = "";
			return string.Format(arg0: (!limitValve.displayUnitsInsteadOfMass) ? GameUtil.GetFormattedMass(limitValve.RemainingCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, includeSuffix: true, LimitValveSideScreen.FLOAT_FORMAT) : GameUtil.GetFormattedUnits(limitValve.RemainingCapacity, GameUtil.TimeSlice.None, displaySuffix: true, LimitValveSideScreen.FLOAT_FORMAT), format: BUILDING.STATUSITEMS.LIMITVALVELIMITNOTREACHED.NAME);
		};
		LimitValveLimitNotReached.resolveTooltipCallback = (string str, object data) => BUILDING.STATUSITEMS.LIMITVALVELIMITNOTREACHED.TOOLTIP;
		SpacePOIHarvesting = CreateStatusItem("SpacePOIHarvesting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		SpacePOIHarvesting.resolveStringCallback = delegate(string str, object data)
		{
			float mass2 = (float)data;
			return string.Format(BUILDING.STATUSITEMS.SPACEPOIHARVESTING.NAME, GameUtil.GetFormattedMass(mass2, GameUtil.TimeSlice.PerSecond));
		};
		SpacePOIWasting = CreateStatusItem("SpacePOIWasting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		SpacePOIWasting.resolveStringCallback = delegate(string str, object data)
		{
			float mass = (float)data;
			return string.Format(BUILDING.STATUSITEMS.SPACEPOIWASTING.NAME, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond));
		};
		RocketRestrictionActive = new StatusItem("ROCKETRESTRICTIONACTIVE", "BUILDING", "status_item_rocket_restricted", StatusItem.IconType.Custom, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		RocketRestrictionInactive = new StatusItem("ROCKETRESTRICTIONINACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		NoRocketRestriction = new StatusItem("NOROCKETRESTRICTION", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		BroadcasterOutOfRange = new StatusItem("BROADCASTEROUTOFRANGE", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		LosingRadbolts = new StatusItem("LOSINGRADBOLTS", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		FabricatorAcceptsMutantSeeds = new StatusItem("FABRICATORACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		NoSpiceSelected = new StatusItem("SPICEGRINDERNOSPICE", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoTunerNoGeyserSelected = new StatusItem("GEOTUNER_NEEDGEYSER", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoTunerResearchNeeded = new StatusItem("GEOTUNER_CHARGE_REQUIRED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		GeoTunerResearchInProgress = new StatusItem("GEOTUNER_CHARGING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		GeoTunerBroadcasting = new StatusItem("GEOTUNER_CHARGED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		GeoTunerBroadcasting.resolveStringCallback = delegate(string str, object data)
		{
			GeoTuner.Instance instance8 = (GeoTuner.Instance)data;
			str = str.Replace("{0}", (float)Mathf.CeilToInt(instance8.sm.expirationTimer.Get(instance8) / instance8.enhancementDuration * 100f) + "%");
			return str;
		};
		GeoTunerBroadcasting.resolveTooltipCallback = delegate(string str, object data)
		{
			GeoTuner.Instance instance7 = (GeoTuner.Instance)data;
			float seconds = instance7.sm.expirationTimer.Get(instance7);
			float num6 = 100f / instance7.enhancementDuration;
			str = str.Replace("{0}", GameUtil.GetFormattedTime(seconds));
			str = str.Replace("{1}", "-" + num6.ToString("0.00") + "%");
			return str;
		};
		GeoTunerGeyserStatus = new StatusItem("GEOTUNER_GEYSER_STATUS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		GeoTunerGeyserStatus.resolveStringCallback = delegate(string str, object data)
		{
			Geyser assignedGeyser2 = ((GeoTuner.Instance)data).GetAssignedGeyser();
			bool flag3 = assignedGeyser2 != null && assignedGeyser2.smi.GetCurrentState() != null && assignedGeyser2.smi.GetCurrentState().parent == assignedGeyser2.smi.sm.erupt;
			bool flag4 = assignedGeyser2 != null && assignedGeyser2.smi.GetCurrentState() == assignedGeyser2.smi.sm.dormant;
			str = (flag3 ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_ERUPTING : (flag4 ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_DORMANT : BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_IDLE));
			return str;
		};
		GeoTunerGeyserStatus.resolveTooltipCallback = delegate(string str, object data)
		{
			Geyser assignedGeyser = ((GeoTuner.Instance)data).GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				assignedGeyser.gameObject.GetProperName();
			}
			bool flag = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt;
			bool flag2 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.dormant;
			str = (flag ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_ERUPTING : (flag2 ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_DORMANT : BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_IDLE));
			return str;
		};
		GeyserGeotuned = new StatusItem("GEYSER_GEOTUNED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
		GeyserGeotuned.resolveStringCallback = delegate(string str, object data)
		{
			Geyser geyser2 = (Geyser)data;
			int num4 = 0;
			int num5 = Components.GeoTuners.GetItems(geyser2.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == geyser2);
			for (int j = 0; j < geyser2.modifications.Count; j++)
			{
				if (geyser2.modifications[j].originID.Contains("GeoTuner"))
				{
					num4++;
				}
			}
			str = str.Replace("{0}", num4.ToString());
			str = str.Replace("{1}", num5.ToString());
			return str;
		};
		GeyserGeotuned.resolveTooltipCallback = delegate(string str, object data)
		{
			Geyser geyser = (Geyser)data;
			int num2 = 0;
			int num3 = Components.GeoTuners.GetItems(geyser.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == geyser);
			for (int i = 0; i < geyser.modifications.Count; i++)
			{
				if (geyser.modifications[i].originID.Contains("GeoTuner"))
				{
					num2++;
				}
			}
			str = str.Replace("{0}", num2.ToString());
			str = str.Replace("{1}", num3.ToString());
			return str;
		};
		SkyVisNone = new StatusItem("SkyVisNone", BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.NAME, BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.TOOLTIP, "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID, 129022, showWorldIcon: true, SkyVisResolveStringCallback);
		SkyVisLimited = new StatusItem("SkyVisLimited", BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.NAME, BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.TOOLTIP, "status_item_no_sky", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID, 129022, showWorldIcon: false, SkyVisResolveStringCallback);
		CreatureManipulatorWaiting = CreateStatusItem("CreatureManipulatorWaiting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		CreatureManipulatorProgress = CreateStatusItem("CreatureManipulatorProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		CreatureManipulatorProgress.resolveStringCallback = delegate(string str, object data)
		{
			GravitasCreatureManipulator.Instance instance6 = (GravitasCreatureManipulator.Instance)data;
			return string.Format(str, instance6.ScannedSpecies.Count, instance6.def.numSpeciesToUnlockMorphMode);
		};
		CreatureManipulatorProgress.resolveTooltipCallback = delegate(string str, object data)
		{
			GravitasCreatureManipulator.Instance instance5 = (GravitasCreatureManipulator.Instance)data;
			if (instance5.ScannedSpecies.Count == 0)
			{
				str = str + "\n • " + BUILDING.STATUSITEMS.CREATUREMANIPULATORPROGRESS.NO_DATA;
			}
			else
			{
				foreach (Tag scannedSpecy in instance5.ScannedSpecies)
				{
					str = str + "\n • " + Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + scannedSpecy.ToString().ToUpper());
				}
			}
			return str;
		};
		CreatureManipulatorMorphModeLocked = CreateStatusItem("CreatureManipulatorMorphModeLocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		CreatureManipulatorMorphMode = CreateStatusItem("CreatureManipulatorMorphMode", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		CreatureManipulatorWorking = CreateStatusItem("CreatureManipulatorWorking", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MegaBrainTankActivationProgress = CreateStatusItem("MegaBrainTankActivationProgress", BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.PROGRESSIONRATE.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.PROGRESSIONRATE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MegaBrainNotEnoughOxygen = CreateStatusItem("MegaBrainNotEnoughOxygen", "BUILDING", "status_item_suit_locker_no_oxygen", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		MegaBrainTankActivationProgress.resolveStringCallback = delegate(string str, object data)
		{
			MegaBrainTank.StatesInstance statesInstance2 = (MegaBrainTank.StatesInstance)data;
			return str.Replace("{ActivationProgress}", $"{statesInstance2.ActivationProgress}/{(short)25}");
		};
		MegaBrainTankDreamAnalysis = CreateStatusItem("MegaBrainTankDreamAnalysis", BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.DREAMANALYSIS.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.DREAMANALYSIS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MegaBrainTankDreamAnalysis.resolveStringCallback = delegate(string str, object data)
		{
			MegaBrainTank.StatesInstance statesInstance = (MegaBrainTank.StatesInstance)data;
			return str.Replace("{TimeToComplete}", statesInstance.TimeTilDigested.ToString());
		};
		MegaBrainTankComplete = CreateStatusItem("MegaBrainTankComplete", BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, allow_multiples: false, OverlayModes.None.ID);
		FossilHuntExcavationOrdered = CreateStatusItem("FossilHuntExcavationOrdered", BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION.NAME, BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, allow_multiples: false, OverlayModes.None.ID);
		FossilHuntExcavationInProgress = CreateStatusItem("FossilHuntExcavationInProgress", BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING.NAME, BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, allow_multiples: false, OverlayModes.None.ID);
		ComplexFabricatorCooking = CreateStatusItem("COMPLEXFABRICATOR.COOKING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ComplexFabricatorCooking.resolveStringCallback = delegate(string str, object data)
		{
			ComplexFabricator complexFabricator2 = data as ComplexFabricator;
			if (complexFabricator2 != null && complexFabricator2.CurrentWorkingOrder != null)
			{
				str = str.Replace("{Item}", complexFabricator2.CurrentWorkingOrder.FirstResult.ProperName());
			}
			return str;
		};
		ComplexFabricatorProducing = CreateStatusItem("COMPLEXFABRICATOR.PRODUCING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ComplexFabricatorProducing.resolveStringCallback = delegate(string str, object data)
		{
			ComplexFabricator complexFabricator = data as ComplexFabricator;
			if (complexFabricator != null)
			{
				if (complexFabricator.CurrentWorkingOrder != null)
				{
					string newValue = (complexFabricator.CurrentWorkingOrder.results[0].facadeID.IsNullOrWhiteSpace() ? complexFabricator.CurrentWorkingOrder.FirstResult.ProperName() : GameTagExtensions.ProperName(complexFabricator.CurrentWorkingOrder.results[0].facadeID));
					str = str.Replace("{Item}", newValue);
				}
				return str;
			}
			TinkerStation tinkerStation = data as TinkerStation;
			if (tinkerStation != null)
			{
				str = str.Replace("{Item}", tinkerStation.outputPrefab.ProperName());
			}
			return str;
		};
		ComplexFabricatorResearching = CreateStatusItem("COMPLEXFABRICATOR.RESEARCHING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ComplexFabricatorResearching.resolveStringCallback = delegate(string str, object data)
		{
			if (data is IResearchCenter)
			{
				TechInstance activeResearch = Research.Instance.GetActiveResearch();
				if (activeResearch != null)
				{
					str = str.Replace("{Item}", activeResearch.tech.Name);
					return str;
				}
			}
			str = str.Replace("{Item}", (data as GameObject).GetProperName());
			return str;
		};
		ArtifactAnalysisAnalyzing = CreateStatusItem("COMPLEXFABRICATOR.ANALYZING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ArtifactAnalysisAnalyzing.resolveStringCallback = delegate(string str, object data)
		{
			if (data as GameObject != null)
			{
				str = str.Replace("{Item}", (data as GameObject).GetProperName());
			}
			return str;
		};
		ComplexFabricatorTraining = CreateStatusItem("COMPLEXFABRICATOR.UNTRAINING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ComplexFabricatorTraining.resolveStringCallback = delegate(string str, object data)
		{
			ResetSkillsStation resetSkillsStation = data as ResetSkillsStation;
			if (resetSkillsStation != null && resetSkillsStation.assignable.assignee != null)
			{
				str = str.Replace("{Duplicant}", resetSkillsStation.assignable.assignee.GetProperName());
			}
			return str;
		};
		TelescopeWorking = CreateStatusItem("COMPLEXFABRICATOR.TELESCOPE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		ClusterTelescopeMeteorWorking = CreateStatusItem("COMPLEXFABRICATOR.CLUSTERTELESCOPEMETEOR", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MorbRoverMakerDusty = CreateStatusItem("MorbRoverMakerDusty", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DUSTY.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DUSTY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		MorbRoverMakerBuildingRevealed = CreateStatusItem("MorbRoverMakerBuildingRevealed", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_REVEALED.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_REVEALED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MorbRoverMakerGermCollectionProgress = CreateStatusItem("MorbRoverMakerGermCollectionProgress", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.GERM_COLLECTION_PROGRESS.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.GERM_COLLECTION_PROGRESS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MorbRoverMakerGermCollectionProgress.resolveStringCallback = delegate(string str, object data)
		{
			MorbRoverMaker.Instance instance4 = (MorbRoverMaker.Instance)data;
			return str.Replace("{0}", GameUtil.GetFormattedPercent(instance4.MorbDevelopment_Progress * 100f));
		};
		MorbRoverMakerGermCollectionProgress.resolveTooltipCallback = delegate(string str, object data)
		{
			MorbRoverMaker.Instance instance3 = (MorbRoverMaker.Instance)data;
			return str.Replace("{GERM_NAME}", Db.Get().Diseases[instance3.def.GERM_TYPE].Name).Replace("{0}", GameUtil.GetFormattedDiseaseAmount(instance3.def.MAX_GERMS_TAKEN_PER_PACKAGE, GameUtil.TimeSlice.PerSecond)).Replace("{1}", GameUtil.GetFormattedDiseaseAmount(instance3.MorbDevelopment_GermsCollected))
				.Replace("{2}", GameUtil.GetFormattedDiseaseAmount(instance3.def.GERMS_PER_ROVER));
		};
		MorbRoverMakerNoGermsConsumedAlert = CreateStatusItem("MorbRoverMakerNoGermsConsumedAlert", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.NOGERMSCONSUMEDALERT.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.NOGERMSCONSUMEDALERT.TOOLTIP, "status_item_no_germs", StatusItem.IconType.Custom, NotificationType.Bad, allow_multiples: false, OverlayModes.None.ID);
		MorbRoverMakerNoGermsConsumedAlert.resolveStringCallback = delegate(string str, object data)
		{
			MorbRoverMaker.Instance instance2 = (MorbRoverMaker.Instance)data;
			return str.Replace("{0}", Db.Get().Diseases[instance2.def.GERM_TYPE].Name);
		};
		MorbRoverMakerNoGermsConsumedAlert.resolveTooltipCallback = delegate(string str, object data)
		{
			MorbRoverMaker.Instance instance = (MorbRoverMaker.Instance)data;
			return str.Replace("{0}", Db.Get().Diseases[instance.def.GERM_TYPE].Name);
		};
		MorbRoverMakerCraftingBody = CreateStatusItem("MorbRoverMakerCraftingBody", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.CRAFTING_ROBOT_BODY.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.CRAFTING_ROBOT_BODY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MorbRoverMakerReadyForDoctor = CreateStatusItem("MorbRoverMakerReadyForDoctor", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_READY.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		MorbRoverMakerDoctorWorking = CreateStatusItem("MorbRoverMakerDoctorWorking", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_WORKED_BY_DOCTOR.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_WORKED_BY_DOCTOR.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GeoVentQuestBlockage = CreateStatusItem("GeoVentQuestBlockage", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.QUEST_BLOCKED_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.QUEST_BLOCKED_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoVentQuestBlockage.resolveStringCallback = (string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName());
		GeoVentQuestBlockage.resolveStringCallback_shouldStillCallIfDataIsNull = false;
		GeoVentsDisconnected = CreateStatusItem("GeoVentsDisconnected", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.DISCONNECTED_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.DISCONNECTED_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoVentsDisconnected.resolveStringCallback = (string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName());
		GeoVentsDisconnected.resolveStringCallback_shouldStillCallIfDataIsNull = false;
		GeoVentsOverpressure = CreateStatusItem("GeoVentsOverpressure", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.OVERPRESSURE_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.OVERPRESSURE_TOOLTIP, "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoVentsOverpressure.resolveStringCallback = (string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName());
		GeoVentsOverpressure.resolveStringCallback_shouldStillCallIfDataIsNull = false;
		GeoControllerCantVent = CreateStatusItem("GeoControllerCantVent", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_TOOLTIP, "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoControllerCantVent.resolveStringCallback = delegate(string str, object obj)
		{
			GeothermalController geothermalController6 = obj as GeothermalController;
			if (geothermalController6 == null)
			{
				return str;
			}
			GeothermalVent geothermalVent5 = geothermalController6.FirstObstructedVent();
			if (geothermalVent5 == null)
			{
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_NAME;
			}
			return geothermalVent5.IsEntombed() ? ((string)COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_ENTOMBED_VENT_NAME) : ((string)COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_UNREADY_CONNECTION_NAME);
		};
		GeoControllerCantVent.resolveStringCallback_shouldStillCallIfDataIsNull = false;
		GeoControllerCantVent.resolveTooltipCallback = delegate(string str, object obj)
		{
			GeothermalController geothermalController5 = obj as GeothermalController;
			if (geothermalController5 == null)
			{
				return str;
			}
			GeothermalVent geothermalVent4 = geothermalController5.FirstObstructedVent();
			if (geothermalVent4 == null)
			{
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_TOOLTIP;
			}
			return geothermalVent4.IsEntombed() ? ((string)COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_ENTOMBED_VENT_TOOLTIP) : ((string)COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_UNREADY_CONNECTION_TOOLTIP);
		};
		GeoControllerCantVent.resolveTooltipCallback_shouldStillCallIfDataIsNull = false;
		GeoControllerCantVent.statusItemClickCallback = delegate(object obj)
		{
			GeothermalController geothermalController4 = obj as GeothermalController;
			GeothermalVent geothermalVent3 = ((geothermalController4 != null) ? geothermalController4.FirstObstructedVent() : null);
			if (geothermalVent3 != null)
			{
				SelectTool.Instance.SelectAndFocus(geothermalVent3.transform.position, geothermalVent3.GetComponent<KSelectable>());
			}
		};
		GeoVentsReady = CreateStatusItem("GeoVentsReady", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.READY_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.READY_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GeoVentsReady.resolveStringCallback = (string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName());
		GeoVentsReady.resolveStringCallback_shouldStillCallIfDataIsNull = false;
		GeoVentsVenting = CreateStatusItem("GeoVentsVenting", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.VENTING_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.VENTING_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GeoVentsVenting.resolveStringCallback = delegate(string str, object obj)
		{
			GeothermalVent geothermalVent2 = obj as GeothermalVent;
			return str.Replace("{Name}", geothermalVent2.GetProperName()).Replace("{Quantity}", GameUtil.GetFormattedMass(geothermalVent2.MaterialAvailable()));
		};
		GeoVentsVenting.resolveStringCallback_shouldStillCallIfDataIsNull = false;
		GeoVentsVenting.resolveTooltipCallback = delegate(string str, object data)
		{
			GeothermalVent geothermalVent = data as GeothermalVent;
			return (geothermalVent != null) ? str.Replace("{Quantity}", GameUtil.GetFormattedMass(geothermalVent.MaterialAvailable())) : str;
		};
		GeoVentsReady.resolveTooltipCallback_shouldStillCallIfDataIsNull = false;
		GeoQuestPendingReconnectPipes = CreateStatusItem("GeoQuestPendingReconnectPipes", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.PENDING_RECONNECTION_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.PENDING_RECONNECTION_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoQuestPendingUncover = CreateStatusItem("GeoQuestPendingUncover", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.PENDING_REVEAL_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.PENDING_REVEAL_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		GeoControllerOffline = CreateStatusItem("GeoControllerOffline", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.OFFLINE_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.OFFLINE_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GeoControllerStorageStatus = CreateStatusItem("GeoControllerStorageStatus", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GeoControllerStorageStatus = CreateStatusItem("GeoControllerStorageStatus", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GeoControllerStorageStatus.resolveStringCallback = delegate(string str, object obj)
		{
			GeothermalController geothermalController3 = obj as GeothermalController;
			float percent = ((geothermalController3 != null) ? (geothermalController3.GetPressure() * 100f) : 0f);
			return str.Replace("{Amount}", GameUtil.GetFormattedPercent(percent));
		};
		GeoControllerStorageStatus.resolveTooltipCallback = delegate(string str, object obj)
		{
			GeothermalController geothermalController2 = obj as GeothermalController;
			float num = ((geothermalController2 != null) ? geothermalController2.GetPressure() : 0f);
			return str.Replace("{Amount}", GameUtil.GetFormattedMass(12000f * num)).Replace("{Threshold}", GameUtil.GetFormattedMass(12000f));
		};
		GeoControllerStorageStatus.resolveStringCallback_shouldStillCallIfDataIsNull = (GeoControllerStorageStatus.resolveTooltipCallback_shouldStillCallIfDataIsNull = false);
		GeoControllerTemperatureStatus = CreateStatusItem("GeoControllerTemperatureStatus", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_TEMPERATURE_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_TEMPERATURE_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
		GeoControllerTemperatureStatus.resolveStringCallback = delegate(string str, object obj)
		{
			GeothermalController geothermalController = obj as GeothermalController;
			float temp = ((geothermalController != null) ? geothermalController.ComputeContentTemperature() : 0f);
			return str.Replace("{Temp}", GameUtil.GetFormattedTemperature(temp));
		};
		GeoControllerTemperatureStatus.resolveStringCallback_shouldStillCallIfDataIsNull = (GeoControllerTemperatureStatus.resolveTooltipCallback_shouldStillCallIfDataIsNull = false);
		static string SkyVisResolveStringCallback(string str, object data)
		{
			ISkyVisInfo skyVisInfo = (ISkyVisInfo)data;
			return str.Replace("{VISIBILITY}", GameUtil.GetFormattedPercent(skyVisInfo.GetPercentVisible01() * 100f));
		}
	}

	private static bool ShowInUtilityOverlay(HashedString mode, object data)
	{
		Transform transform = (Transform)data;
		bool result = false;
		if (mode == OverlayModes.GasConduits.ID)
		{
			Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
			result = OverlayScreen.GasVentIDs.Contains(prefabTag);
		}
		else if (mode == OverlayModes.LiquidConduits.ID)
		{
			Tag prefabTag2 = transform.GetComponent<KPrefabID>().PrefabTag;
			result = OverlayScreen.LiquidVentIDs.Contains(prefabTag2);
		}
		else if (mode == OverlayModes.Power.ID)
		{
			Tag prefabTag3 = transform.GetComponent<KPrefabID>().PrefabTag;
			result = OverlayScreen.WireIDs.Contains(prefabTag3);
		}
		else if (mode == OverlayModes.Logic.ID)
		{
			Tag prefabTag4 = transform.GetComponent<KPrefabID>().PrefabTag;
			result = OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag4);
		}
		else if (mode == OverlayModes.SolidConveyor.ID)
		{
			Tag prefabTag5 = transform.GetComponent<KPrefabID>().PrefabTag;
			result = OverlayScreen.SolidConveyorIDs.Contains(prefabTag5);
		}
		else if (mode == OverlayModes.Radiation.ID)
		{
			Tag prefabTag6 = transform.GetComponent<KPrefabID>().PrefabTag;
			result = OverlayScreen.RadiationIDs.Contains(prefabTag6);
		}
		return result;
	}
}
