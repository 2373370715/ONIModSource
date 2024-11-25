﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

namespace Database {
    public class BuildingStatusItems : StatusItems {
        public StatusItem          AngerDamage;
        public StatusItem          ArtifactAnalysisAnalyzing;
        public StatusItem          AssignedPublic;
        public StatusItem          AssignedTo;
        public StatusItem          AssignedToRoom;
        public StatusItem          AutoPilotActive;
        public StatusItem          AwaitingBaitDelivery;
        public StatusItem          AwaitingCompostFlip;
        public StatusItem          AwaitingEggDelivery;
        public StatusItem          AwaitingEmptyBuilding;
        public StatusItem          AwaitingSeedDelivery;
        public StatusItem          AwaitingWaste;
        public StatusItem          Baited;
        public StatusItem          BatteryJoulesAvailable;
        public StatusItem          BroadcasterOutOfRange;
        public StatusItem          Broken;
        public StatusItem          BuildingDisabled;
        public StatusItem          CannotCoolFurther;
        public StatusItem          ChangeDoorControlState;
        public StatusItem          ChangeStorageTileTarget;
        public StatusItem          ClinicOutsideHospital;
        public StatusItem          ClusterColonyLacksRequiredSkillPerk;
        public StatusItem          ClusterTelescopeAllWorkComplete;
        public StatusItem          ClusterTelescopeMeteorWorking;
        public StatusItem          CollectingHEP;
        public StatusItem          ColonyLacksRequiredSkillPerk;
        public StatusItem          ComplexFabricatorCooking;
        public StatusItem          ComplexFabricatorProducing;
        public StatusItem          ComplexFabricatorResearching;
        public StatusItem          ComplexFabricatorTraining;
        public StatusItem          ConduitBlocked;
        public StatusItem          ConduitBlockedMultiples;
        public StatusItem          ConstructableDigUnreachable;
        public StatusItem          ConstructionUnreachable;
        public StatusItem          Conveyor;
        public StatusItem          Cooling;
        public StatusItem          CoolingStalledColdGas;
        public StatusItem          CoolingStalledColdLiquid;
        public StatusItem          CoolingStalledHotEnv;
        public StatusItem          CoolingStalledHotLiquid;
        public StatusItem          CoolingWater;
        public StatusItem          CreatureManipulatorMorphMode;
        public StatusItem          CreatureManipulatorMorphModeLocked;
        public StatusItem          CreatureManipulatorProgress;
        public StatusItem          CreatureManipulatorWaiting;
        public StatusItem          CreatureManipulatorWorking;
        public StatusItem          CurrentDoorControlState;
        public StatusItem          DataMinerEfficiency;
        public StatusItem          DeadReactorCoolingOff;
        public StatusItem          DesalinatorNeedsEmptying;
        public StatusItem          DestinationOutOfRange;
        public StatusItem          DetectorScanning;
        public StatusItem          DigRequiresSkillPerk;
        public StatusItem          DigUnreachable;
        public StatusItem          DirectionControl;
        public StatusItem          DisembarkingDuplicant;
        public StatusItem          DispenseRequested;
        public StatusItem          DuplicantActivationRequired;
        public StatusItem          ElectrobankJoulesAvailable;
        public StatusItem          ElementConsumer;
        public StatusItem          ElementEmitterOutput;
        public StatusItem          EmergencyPriority;
        public StatusItem          EmittingBlockedHighPressure;
        public StatusItem          EmittingBlockedLowTemperature;
        public StatusItem          EmittingElement;
        public StatusItem          EmittingGasAvg;
        public StatusItem          EmittingLight;
        public StatusItem          EmittingOxygenAvg;
        public StatusItem          EmptyPumpingStation;
        public StatusItem          Entombed;
        public StatusItem          Expired;
        public StatusItem          FabricatorAcceptsMutantSeeds;
        public StatusItem          FabricatorEmpty;
        public StatusItem          FabricatorIdle;
        public StatusItem          FabricatorLacksHEP;
        public StatusItem          FlightAllCargoFull;
        public StatusItem          FlightCargoRemaining;
        public StatusItem          Flooded;
        public StatusItem          FlushToilet;
        public StatusItem          FlushToiletInUse;
        public StatusItem          FossilEntombed;
        public StatusItem          FossilHuntExcavationInProgress;
        public StatusItem          FossilHuntExcavationOrdered;
        public StatusItem          FossilMineEmpty;
        public StatusItem          FossilMineIdle;
        public StatusItem          FossilMinePendingWork;
        public StatusItem          FridgeCooling;
        public StatusItem          FridgeSteady;
        public StatusItem          GasPipeEmpty;
        public StatusItem          GasPipeObstructed;
        public StatusItem          GasVentObstructed;
        public StatusItem          GasVentOverPressure;
        public StatusItem          GeneratorOffline;
        public StatusItem          GeneShuffleCompleted;
        public StatusItem          GeneticAnalysisCompleted;
        public StatusItem          GeoControllerCantVent;
        public StatusItem          GeoControllerOffline;
        public StatusItem          GeoControllerStorageStatus;
        public StatusItem          GeoControllerTemperatureStatus;
        public StatusItem          GeoQuestPendingReconnectPipes;
        public StatusItem          GeoQuestPendingUncover;
        public StatusItem          GeoTunerBroadcasting;
        public StatusItem          GeoTunerGeyserStatus;
        public StatusItem          GeoTunerNoGeyserSelected;
        public StatusItem          GeoTunerResearchInProgress;
        public StatusItem          GeoTunerResearchNeeded;
        public StatusItem          GeoVentQuestBlockage;
        public StatusItem          GeoVentsDisconnected;
        public StatusItem          GeoVentsOverpressure;
        public StatusItem          GeoVentsReady;
        public StatusItem          GeoVentsVenting;
        public StatusItem          GettingReady;
        public StatusItem          GeyserGeotuned;
        public StatusItem          Grave;
        public StatusItem          GraveEmpty;
        public StatusItem          GunkEmptierFull;
        public StatusItem          HabitatNeedsEmptying;
        public StatusItem          HasGantry;
        public StatusItem          HotTubFilling;
        public StatusItem          HotTubTooHot;
        public StatusItem          HotTubWaterTooCold;
        public StatusItem          IncomingMeteors;
        public StatusItem          IncubatorProgress;
        public StatusItem          InFlight;
        public StatusItem          InOrbit;
        public StatusItem          InOrbitRequired;
        public StatusItem          InvalidBuildingLocation;
        public StatusItem          InvalidMaskStationConsumptionState;
        public StatusItem          InvalidPortOverlap;
        public StatusItem          KettleInsuficientFuel;
        public StatusItem          KettleInsuficientLiquidSpace;
        public StatusItem          KettleInsuficientSolids;
        public StatusItem          KettleMelting;
        public StatusItem          LandedRocketLacksPassengerModule;
        public StatusItem          LimitValveLimitNotReached;
        public StatusItem          LimitValveLimitReached;
        public StatusItem          LiquidPipeEmpty;
        public StatusItem          LiquidPipeObstructed;
        public StatusItem          LiquidVentObstructed;
        public StatusItem          LiquidVentOverPressure;
        public StatusItem          LogicOverloaded;
        public StatusItem          LogicSensorStatusActive;
        public StatusItem          LogicSensorStatusInactive;
        public StatusItem          LogicSwitchStatusActive;
        public StatusItem          LogicSwitchStatusInactive;
        public StatusItem          LosingRadbolts;
        public StatusItem          ManualGeneratorChargingUp;
        public StatusItem          ManualGeneratorReleasingEnergy;
        public MaterialsStatusItem MaterialsUnavailable;
        public MaterialsStatusItem MaterialsUnavailableForRefill;
        public StatusItem          MegaBrainNotEnoughOxygen;
        public StatusItem          MegaBrainTankActivationProgress;
        public StatusItem          MegaBrainTankAllDupesAreDead;
        public StatusItem          MegaBrainTankComplete;
        public StatusItem          MegaBrainTankDreamAnalysis;
        public StatusItem          MeltingDown;
        public StatusItem          MercuryLight_Charged;
        public StatusItem          MercuryLight_Charging;
        public StatusItem          MercuryLight_Depleated;
        public StatusItem          MercuryLight_Depleating;
        public StatusItem          MilkSeparatorNeedsEmptying;
        public StatusItem          MissingFoundation;
        public StatusItem          MissingGantry;
        public StatusItem          MissingRequirements;
        public StatusItem          MissionControlAssistingRocket;
        public StatusItem          MissionControlBoosted;
        public StatusItem          ModuleGeneratorNotPowered;
        public StatusItem          ModuleGeneratorPowered;
        public StatusItem          ModuleSolarPanelWattage;
        public StatusItem          MopUnreachable;
        public StatusItem          MorbRoverMakerBuildingRevealed;
        public StatusItem          MorbRoverMakerCraftingBody;
        public StatusItem          MorbRoverMakerDoctorWorking;
        public StatusItem          MorbRoverMakerDusty;
        public StatusItem          MorbRoverMakerGermCollectionProgress;
        public StatusItem          MorbRoverMakerNoGermsConsumedAlert;
        public StatusItem          MorbRoverMakerReadyForDoctor;
        public StatusItem          NeedEgg;
        public StatusItem          NeedGasIn;
        public StatusItem          NeedGasOut;
        public StatusItem          NeedLiquidIn;
        public StatusItem          NeedLiquidOut;
        public StatusItem          NeedPlant;
        public StatusItem          NeedPower;
        public StatusItem          NeedResourceMass;
        public StatusItem          NeedSeed;
        public StatusItem          NeedSolidIn;
        public StatusItem          NeedSolidOut;
        public StatusItem          NeedsValidRegion;
        public StatusItem          NeutroniumUnminable;
        public StatusItem          NewDuplicantsAvailable;
        public StatusItem          NoApplicableAnalysisSelected;
        public StatusItem          NoApplicableResearchSelected;
        public StatusItem          NoAvailableEgg;
        public StatusItem          NoAvailableSeed;
        public StatusItem          NoCoolant;
        public StatusItem          NoFilterElementSelected;
        public StatusItem          NoFishableWaterBelow;
        public StatusItem          NoGasElementToPump;
        public StatusItem          NoLiquidElementToPump;
        public StatusItem          NoLogicWireConnected;
        public StatusItem          NoLureElementSelected;
        public StatusItem          NoPowerConsumers;
        public StatusItem          NoResearchOrDestinationSelected;
        public StatusItem          NoResearchSelected;
        public StatusItem          Normal;
        public StatusItem          NoRocketRestriction;
        public StatusItem          NoRocketsToMissionControlBoost;
        public StatusItem          NoRocketsToMissionControlClusterBoost;
        public StatusItem          NoSpiceSelected;
        public StatusItem          NoStorageFilterSet;
        public StatusItem          NoSuitMarker;
        public StatusItem          NoSurfaceSight;
        public StatusItem          NotEnoughPower;
        public StatusItem          NotInAnyRoom;
        public StatusItem          NotInRecommendedRoom;
        public StatusItem          NotInRequiredRoom;
        public StatusItem          NotSubmerged;
        public StatusItem          NoTubeConnected;
        public StatusItem          NoTubeExits;
        public StatusItem          NoWireConnected;
        public StatusItem          OutputPipeFull;
        public StatusItem          OutputTileBlocked;
        public StatusItem          Overheated;
        public StatusItem          Overloaded;
        public StatusItem          PartiallyDamaged;
        public StatusItem          PassengerModuleUnreachable;
        public StatusItem          PathNotClear;
        public StatusItem          PendingDeconstruction;
        public StatusItem          PendingDemolition;
        public StatusItem          PendingFish;
        public StatusItem          PendingRepair;
        public StatusItem          PendingSwitchToggle;
        public StatusItem          PendingUpgrade;
        public StatusItem          PendingWork;
        public StatusItem          PilotNeeded;
        public StatusItem          Pipe;
        public StatusItem          PipeFull;
        public StatusItem          PipeMayMelt;
        public StatusItem          PowerButtonOff;
        public StatusItem          PowerLoopDetected;
        public StatusItem          PressureOk;
        public StatusItem          PumpingLiquidOrGas;
        public StatusItem          PumpingStation;
        public StatusItem          RailGunCooldown;
        public StatusItem          RailgunpayloadNeedsEmptying;
        public StatusItem          RationBoxContents;
        public StatusItem          ReactorMeltdown;
        public StatusItem          ReactorRefuelDisabled;
        public StatusItem          ReleasingPressure;
        public StatusItem          RemoteWorkDockMakingWorker;
        public StatusItem          RemoteWorkTerminalNoDock;
        public StatusItem          RequiresSkillPerk;
        public StatusItem          Researching;
        public StatusItem          RocketCargoEmptying;
        public StatusItem          RocketCargoFilling;
        public StatusItem          RocketCargoFull;
        public StatusItem          RocketChecklistIncomplete;
        public StatusItem          RocketName;
        public StatusItem          RocketPlatformCloseToCeiling;
        public StatusItem          RocketRestrictionActive;
        public StatusItem          RocketRestrictionInactive;
        public StatusItem          RocketStranded;
        public StatusItem          SkillPointsAvailable;
        public StatusItem          SkyVisLimited;
        public StatusItem          SkyVisNone;
        public StatusItem          SolarPanelWattage;
        public StatusItem          SolidConduitBlockedMultiples;
        public StatusItem          SolidPipeObstructed;
        public StatusItem          SpacePOIHarvesting;
        public StatusItem          SpacePOIWasting;
        public StatusItem          SpecialCargoBayClusterCritterStored;
        public StatusItem          SteamTurbineWattage;
        public StatusItem          StorageUnreachable;
        public StatusItem          StoredCharge;
        public StatusItem          SuitMarkerTraversalAnytime;
        public StatusItem          SuitMarkerTraversalOnlyWhenRoomAvailable;
        public StatusItem          SuitMarkerWrongSide;
        public StatusItem          SwitchStatusActive;
        public StatusItem          SwitchStatusInactive;
        public StatusItem          TanningLightInsufficient;
        public StatusItem          TanningLightSufficient;
        public StatusItem          TelescopeWorking;
        public StatusItem          Toilet;
        public StatusItem          ToiletNeedsEmptying;
        public StatusItem          TooCold;
        public StatusItem          TransitTubeEntranceWaxReady;
        public StatusItem          TrapArmed;
        public StatusItem          TrapHasCritter;
        public StatusItem          TrapNeedsArming;
        public StatusItem          Unassigned;
        public StatusItem          UnderConstruction;
        public StatusItem          UnderConstructionNoWorker;
        public StatusItem          UnderPressure;
        public StatusItem          Unusable;
        public StatusItem          ValveRequest;
        public StatusItem          WaitingForHighEnergyParticles;
        public MaterialsStatusItem WaitingForMaterials;
        public StatusItem          WaitingForRepairMaterials;
        public StatusItem          WaitingToLand;
        public StatusItem          WarpConduitPartnerDisabled;
        public StatusItem          WarpPortalCharging;
        public StatusItem          Wattage;
        public StatusItem          Wattson;
        public StatusItem          WellOverpressure;
        public StatusItem          WellPressurizing;
        public StatusItem          WindTunnelIntake;
        public StatusItem          WireConnected;
        public StatusItem          WireDisconnected;
        public StatusItem          WireNominal;
        public StatusItem          Working;
        public StatusItem          WorkRequiresMinion;

        public BuildingStatusItems(ResourceSet parent) : base("BuildingStatusItems", parent) {
            this.CreateStatusItems();
        }

        private StatusItem CreateStatusItem(string              id,
                                            string              prefix,
                                            string              icon,
                                            StatusItem.IconType icon_type,
                                            NotificationType    notification_type,
                                            bool                allow_multiples,
                                            HashedString        render_overlay,
                                            bool                showWorldIcon   = true,
                                            int                 status_overlays = 129022) {
            return base.Add(new StatusItem(id,
                                           prefix,
                                           icon,
                                           icon_type,
                                           notification_type,
                                           allow_multiples,
                                           render_overlay,
                                           showWorldIcon,
                                           status_overlays,
                                           null));
        }

        private StatusItem CreateStatusItem(string              id,
                                            string              name,
                                            string              tooltip,
                                            string              icon,
                                            StatusItem.IconType icon_type,
                                            NotificationType    notification_type,
                                            bool                allow_multiples,
                                            HashedString        render_overlay,
                                            int                 status_overlays = 129022) {
            return base.Add(new StatusItem(id,
                                           name,
                                           tooltip,
                                           icon,
                                           icon_type,
                                           notification_type,
                                           allow_multiples,
                                           render_overlay,
                                           status_overlays,
                                           true,
                                           null));
        }

        private void CreateStatusItems() {
            this.AngerDamage = this.CreateStatusItem("AngerDamage",
                                                     "BUILDING",
                                                     "",
                                                     StatusItem.IconType.Exclamation,
                                                     NotificationType.Bad,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.AssignedTo = this.CreateStatusItem("AssignedTo",
                                                    "BUILDING",
                                                    "",
                                                    StatusItem.IconType.Info,
                                                    NotificationType.Neutral,
                                                    false,
                                                    OverlayModes.None.ID,
                                                    true,
                                                    129022);

            this.AssignedTo.resolveStringCallback = delegate(string str, object data) {
                                                        IAssignableIdentity assignee = ((Assignable)data).assignee;
                                                        if (!assignee.IsNullOrDestroyed()) {
                                                            string properName = assignee.GetProperName();
                                                            str = str.Replace("{Assignee}", properName);
                                                        }

                                                        return str;
                                                    };

            this.AssignedToRoom = this.CreateStatusItem("AssignedToRoom",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Info,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.AssignedToRoom.resolveStringCallback = delegate(string str, object data) {
                                                            IAssignableIdentity assignee = ((Assignable)data).assignee;
                                                            if (!assignee.IsNullOrDestroyed()) {
                                                                string properName = assignee.GetProperName();
                                                                str = str.Replace("{Assignee}", properName);
                                                            }

                                                            return str;
                                                        };

            this.Broken = this.CreateStatusItem("Broken",
                                                "BUILDING",
                                                "status_item_broken",
                                                StatusItem.IconType.Custom,
                                                NotificationType.BadMinor,
                                                false,
                                                OverlayModes.None.ID,
                                                true,
                                                129022);

            this.Broken.resolveStringCallback = ((string str, object data) =>
                                                     str.Replace("{DamageInfo}",
                                                                 ((BuildingHP.SMInstance)data).master
                                                                 .GetDamageSourceInfo()
                                                                 .ToString()));

            this.Broken.conditionalOverlayCallback = new Func<HashedString, object, bool>(ShowInUtilityOverlay);
            this.ChangeStorageTileTarget = this.CreateStatusItem("ChangeStorageTileTarget",
                                                                 "BUILDING",
                                                                 "status_item_pending_switch_toggle",
                                                                 StatusItem.IconType.Custom,
                                                                 NotificationType.Neutral,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.ChangeStorageTileTarget.resolveStringCallback = delegate(string str, object data) {
                                                                     StorageTile.Instance instance
                                                                         = (StorageTile.Instance)data;

                                                                     return str.Replace("{TargetName}",
                                                                      (instance.TargetTag ==
                                                                       StorageTile.INVALID_TAG)
                                                                          ? BUILDING.STATUSITEMS
                                                                              .CHANGESTORAGETILETARGET.EMPTY.text
                                                                          : instance.TargetTag.ProperName());
                                                                 };

            this.ChangeDoorControlState = this.CreateStatusItem("ChangeDoorControlState",
                                                                "BUILDING",
                                                                "status_item_pending_switch_toggle",
                                                                StatusItem.IconType.Custom,
                                                                NotificationType.Neutral,
                                                                false,
                                                                OverlayModes.None.ID,
                                                                true,
                                                                129022);

            this.ChangeDoorControlState.resolveStringCallback = delegate(string str, object data) {
                                                                    Door door = (Door)data;
                                                                    return str.Replace("{ControlState}",
                                                                     door.RequestedState.ToString());
                                                                };

            this.CurrentDoorControlState = this.CreateStatusItem("CurrentDoorControlState",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Info,
                                                                 NotificationType.Neutral,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.CurrentDoorControlState.resolveStringCallback = delegate(string str, object data) {
                                                                     Door door = (Door)data;
                                                                     string newValue
                                                                         = Strings
                                                                             .Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." +
                                                                                 door.CurrentState.ToString()
                                                                                     .ToUpper());

                                                                     return str.Replace("{ControlState}", newValue);
                                                                 };

            this.GunkEmptierFull = this.CreateStatusItem("GunkEmptierFull",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         false,
                                                         129022);

            this.ClinicOutsideHospital = this.CreateStatusItem("ClinicOutsideHospital",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               false,
                                                               129022);

            this.ConduitBlocked = this.CreateStatusItem("ConduitBlocked",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Info,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.OutputPipeFull = this.CreateStatusItem("OutputPipeFull",
                                                        "BUILDING",
                                                        "status_item_no_liquid_to_pump",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.OutputTileBlocked = this.CreateStatusItem("OutputTileBlocked",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.BadMinor,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.ConstructionUnreachable = this.CreateStatusItem("ConstructionUnreachable",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Exclamation,
                                                                 NotificationType.BadMinor,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.ConduitBlockedMultiples = this.CreateStatusItem("ConduitBlockedMultiples",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Info,
                                                                 NotificationType.BadMinor,
                                                                 true,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.SolidConduitBlockedMultiples = this.CreateStatusItem("SolidConduitBlockedMultiples",
                                                                      "BUILDING",
                                                                      "",
                                                                      StatusItem.IconType.Info,
                                                                      NotificationType.BadMinor,
                                                                      true,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.DigUnreachable = this.CreateStatusItem("DigUnreachable",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Exclamation,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.MopUnreachable = this.CreateStatusItem("MopUnreachable",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Exclamation,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.StorageUnreachable = this.CreateStatusItem("StorageUnreachable",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Exclamation,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.PassengerModuleUnreachable = this.CreateStatusItem("PassengerModuleUnreachable",
                                                                    "BUILDING",
                                                                    "status_item_exclamation",
                                                                    StatusItem.IconType.Custom,
                                                                    NotificationType.BadMinor,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    true,
                                                                    129022);

            this.DirectionControl = this.CreateStatusItem("DirectionControl",
                                                          BUILDING.STATUSITEMS.DIRECTION_CONTROL.NAME,
                                                          BUILDING.STATUSITEMS.DIRECTION_CONTROL.TOOLTIP,
                                                          "",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          129022);

            this.DirectionControl.resolveStringCallback = delegate(string str, object data) {
                                                              DirectionControl directionControl
                                                                  = (DirectionControl)data;

                                                              string newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL
                                                                  .DIRECTIONS.BOTH;

                                                              WorkableReactable.AllowedDirection allowedDirection
                                                                  = directionControl.allowedDirection;

                                                              if (allowedDirection !=
                                                                  WorkableReactable.AllowedDirection.Left) {
                                                                  if (allowedDirection ==
                                                                      WorkableReactable.AllowedDirection.Right) {
                                                                      newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL
                                                                          .DIRECTIONS.RIGHT;
                                                                  }
                                                              } else {
                                                                  newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL
                                                                      .DIRECTIONS.LEFT;
                                                              }

                                                              str = str.Replace("{Direction}", newValue);
                                                              return str;
                                                          };

            this.DeadReactorCoolingOff = this.CreateStatusItem("DeadReactorCoolingOff",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.DeadReactorCoolingOff.resolveStringCallback = delegate(string str, object data) {
                                                                   Reactor.StatesInstance smi
                                                                       = (Reactor.StatesInstance)data;

                                                                   float num = ((Reactor.StatesInstance)data).sm
                                                                       .timeSinceMeltdown.Get(smi);

                                                                   str = str.Replace("{CyclesRemaining}",
                                                                    Util.FormatOneDecimalPlace(Mathf.Max(0f,
                                                                         3000f - num) /
                                                                        600f));

                                                                   return str;
                                                               };

            this.ConstructableDigUnreachable = this.CreateStatusItem("ConstructableDigUnreachable",
                                                                     "BUILDING",
                                                                     "",
                                                                     StatusItem.IconType.Exclamation,
                                                                     NotificationType.BadMinor,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     true,
                                                                     129022);

            this.Entombed = this.CreateStatusItem("Entombed",
                                                  "BUILDING",
                                                  "status_item_entombed",
                                                  StatusItem.IconType.Custom,
                                                  NotificationType.BadMinor,
                                                  false,
                                                  OverlayModes.None.ID,
                                                  true,
                                                  129022);

            this.Entombed.AddNotification(null, null, null);
            this.Flooded = this.CreateStatusItem("Flooded",
                                                 "BUILDING",
                                                 "status_item_flooded",
                                                 StatusItem.IconType.Custom,
                                                 NotificationType.BadMinor,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022);

            this.Flooded.AddNotification(null, null, null);
            this.NotSubmerged = this.CreateStatusItem("NotSubmerged",
                                                      "BUILDING",
                                                      "status_item_flooded",
                                                      StatusItem.IconType.Custom,
                                                      NotificationType.BadMinor,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.GasVentObstructed = this.CreateStatusItem("GasVentObstructed",
                                                           "BUILDING",
                                                           "status_item_vent_disabled",
                                                           StatusItem.IconType.Custom,
                                                           NotificationType.BadMinor,
                                                           false,
                                                           OverlayModes.GasConduits.ID,
                                                           true,
                                                           129022);

            this.GasVentOverPressure = this.CreateStatusItem("GasVentOverPressure",
                                                             "BUILDING",
                                                             "status_item_vent_disabled",
                                                             StatusItem.IconType.Custom,
                                                             NotificationType.BadMinor,
                                                             false,
                                                             OverlayModes.GasConduits.ID,
                                                             true,
                                                             129022);

            this.GeneShuffleCompleted = this.CreateStatusItem("GeneShuffleCompleted",
                                                              "BUILDING",
                                                              "status_item_pending_upgrade",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.GeneticAnalysisCompleted = this.CreateStatusItem("GeneticAnalysisCompleted",
                                                                  "BUILDING",
                                                                  "status_item_pending_upgrade",
                                                                  StatusItem.IconType.Custom,
                                                                  NotificationType.BadMinor,
                                                                  false,
                                                                  OverlayModes.None.ID,
                                                                  true,
                                                                  129022);

            this.InvalidBuildingLocation = this.CreateStatusItem("InvalidBuildingLocation",
                                                                 "BUILDING",
                                                                 "status_item_missing_foundation",
                                                                 StatusItem.IconType.Custom,
                                                                 NotificationType.BadMinor,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.LiquidVentObstructed = this.CreateStatusItem("LiquidVentObstructed",
                                                              "BUILDING",
                                                              "status_item_vent_disabled",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.LiquidConduits.ID,
                                                              true,
                                                              129022);

            this.LiquidVentOverPressure = this.CreateStatusItem("LiquidVentOverPressure",
                                                                "BUILDING",
                                                                "status_item_vent_disabled",
                                                                StatusItem.IconType.Custom,
                                                                NotificationType.BadMinor,
                                                                false,
                                                                OverlayModes.LiquidConduits.ID,
                                                                true,
                                                                129022);

            this.MaterialsUnavailable = new MaterialsStatusItem("MaterialsUnavailable",
                                                                "BUILDING",
                                                                "status_item_resource_unavailable",
                                                                StatusItem.IconType.Custom,
                                                                NotificationType.BadMinor,
                                                                true,
                                                                OverlayModes.None.ID);

            this.MaterialsUnavailable.AddNotification(null, null, null);
            this.MaterialsUnavailable.resolveStringCallback = delegate(string str, object data) {
                                                                  string                 text       = "";
                                                                  Dictionary<Tag, float> dictionary = null;
                                                                  if (data is IFetchList) {
                                                                      dictionary = ((IFetchList)data)
                                                                          .GetRemainingMinimum();
                                                                  } else if (data is Dictionary<Tag, float>) {
                                                                      dictionary = (data as Dictionary<Tag, float>);
                                                                  }

                                                                  if (dictionary.Count > 0) {
                                                                      bool flag = true;
                                                                      foreach (KeyValuePair<Tag, float> keyValuePair in
                                                                               dictionary) {
                                                                          if (keyValuePair.Value != 0f) {
                                                                              if (!flag) { text += "\n"; }

                                                                              if (Assets.IsTagCountable(keyValuePair
                                                                                  .Key)) {
                                                                                  text
                                                                                      += string.Format(BUILDING
                                                                                           .STATUSITEMS
                                                                                           .MATERIALSUNAVAILABLE
                                                                                           .LINE_ITEM_UNITS,
                                                                                       GameUtil
                                                                                           .GetUnitFormattedName(keyValuePair
                                                                                                .Key
                                                                                                .ProperName(),
                                                                                            keyValuePair.Value,
                                                                                            false));
                                                                              } else {
                                                                                  text
                                                                                      += string.Format(BUILDING
                                                                                           .STATUSITEMS
                                                                                           .MATERIALSUNAVAILABLE
                                                                                           .LINE_ITEM_MASS,
                                                                                       keyValuePair.Key
                                                                                           .ProperName(),
                                                                                       GameUtil
                                                                                           .GetFormattedMass(keyValuePair
                                                                                                .Value,
                                                                                            GameUtil.TimeSlice
                                                                                                .None,
                                                                                            GameUtil
                                                                                                .MetricMassFormat
                                                                                                .UseThreshold,
                                                                                            true,
                                                                                            "{0:0.#}"));
                                                                              }

                                                                              flag = false;
                                                                          }
                                                                      }
                                                                  }

                                                                  str = str.Replace("{ItemsRemaining}", text);
                                                                  return str;
                                                              };

            this.MaterialsUnavailableForRefill = new MaterialsStatusItem("MaterialsUnavailableForRefill",
                                                                         "BUILDING",
                                                                         "",
                                                                         StatusItem.IconType.Info,
                                                                         NotificationType.BadMinor,
                                                                         true,
                                                                         OverlayModes.None.ID);

            this.MaterialsUnavailableForRefill.resolveStringCallback = delegate(string str, object data) {
                                                                           IFetchList fetchList = (IFetchList)data;
                                                                           string     text      = "";
                                                                           Dictionary<Tag, float> remaining
                                                                               = fetchList.GetRemaining();

                                                                           if (remaining.Count > 0) {
                                                                               bool flag = true;
                                                                               foreach (KeyValuePair<Tag, float>
                                                                                    keyValuePair in remaining) {
                                                                                   if (keyValuePair.Value != 0f) {
                                                                                       if (!flag) { text += "\n"; }

                                                                                       text
                                                                                           += string.Format(BUILDING
                                                                                                .STATUSITEMS
                                                                                                .MATERIALSUNAVAILABLEFORREFILL
                                                                                                .LINE_ITEM,
                                                                                            keyValuePair.Key
                                                                                                .ProperName());

                                                                                       flag = false;
                                                                                   }
                                                                               }
                                                                           }

                                                                           str = str.Replace("{ItemsRemaining}", text);
                                                                           return str;
                                                                       };

            Func<string, object, string> resolveStringCallback = delegate(string str, object data) {
                                                                     RoomType roomType
                                                                         = Db.Get().RoomTypes.Get((string)data);

                                                                     if (roomType != null) {
                                                                         return str.Replace("{0}", roomType.Name);
                                                                     }

                                                                     return str;
                                                                 };

            this.NoCoolant = this.CreateStatusItem("NoCoolant",
                                                   "BUILDING",
                                                   "status_item_need_supply_in",
                                                   StatusItem.IconType.Custom,
                                                   NotificationType.BadMinor,
                                                   false,
                                                   OverlayModes.None.ID,
                                                   true,
                                                   129022);

            this.NotInAnyRoom = this.CreateStatusItem("NotInAnyRoom",
                                                      "BUILDING",
                                                      "status_item_room_required",
                                                      StatusItem.IconType.Custom,
                                                      NotificationType.BadMinor,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.NotInRequiredRoom = this.CreateStatusItem("NotInRequiredRoom",
                                                           "BUILDING",
                                                           "status_item_room_required",
                                                           StatusItem.IconType.Custom,
                                                           NotificationType.BadMinor,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.NotInRequiredRoom.resolveStringCallback = resolveStringCallback;
            this.NotInRecommendedRoom = this.CreateStatusItem("NotInRecommendedRoom",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.NotInRecommendedRoom.resolveStringCallback = resolveStringCallback;
            this.MercuryLight_Charging = this.CreateStatusItem("MercuryLight_Charging",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.Neutral,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.MercuryLight_Charging.resolveStringCallback = delegate(string str, object data) {
                                                                   MercuryLight.Instance instance
                                                                       = (MercuryLight.Instance)data;

                                                                   str = string.Format(str,
                                                                    GameUtil.GetFormattedPercent(instance
                                                                             .ChargeLevel *
                                                                         100f,
                                                                     GameUtil.TimeSlice.None));

                                                                   return str;
                                                               };

            this.MercuryLight_Charging.resolveTooltipCallback = delegate(string str, object data) {
                                                                    MercuryLight.Instance instance
                                                                        = (MercuryLight.Instance)data;

                                                                    str = string.Format(str,
                                                                     GameUtil.GetFormattedTime((1f -
                                                                                      instance.ChargeLevel) *
                                                                          instance.def.TURN_ON_DELAY,
                                                                      "F0"));

                                                                    return str;
                                                                };

            this.MercuryLight_Depleating = this.CreateStatusItem("MercuryLight_Depleating",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Info,
                                                                 NotificationType.Neutral,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.MercuryLight_Depleating.resolveStringCallback = delegate(string str, object data) {
                                                                     MercuryLight.Instance instance
                                                                         = (MercuryLight.Instance)data;

                                                                     str = string.Format(str,
                                                                      GameUtil.GetFormattedPercent(instance
                                                                               .ChargeLevel *
                                                                           100f,
                                                                       GameUtil.TimeSlice.None));

                                                                     return str;
                                                                 };

            this.MercuryLight_Charged = this.CreateStatusItem("MercuryLight_Charged",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.MercuryLight_Depleated = this.CreateStatusItem("MercuryLight_Depleated",
                                                                "BUILDING",
                                                                "",
                                                                StatusItem.IconType.Info,
                                                                NotificationType.Neutral,
                                                                false,
                                                                OverlayModes.None.ID,
                                                                true,
                                                                129022);

            this.WaitingForRepairMaterials = this.CreateStatusItem("WaitingForRepairMaterials",
                                                                   "BUILDING",
                                                                   "status_item_resource_unavailable",
                                                                   StatusItem.IconType.Exclamation,
                                                                   NotificationType.Neutral,
                                                                   true,
                                                                   OverlayModes.None.ID,
                                                                   false,
                                                                   129022);

            this.WaitingForRepairMaterials.resolveStringCallback = delegate(string str, object data) {
                                                                       KeyValuePair<Tag, float> keyValuePair
                                                                           = (KeyValuePair<Tag, float>)data;

                                                                       if (keyValuePair.Value != 0f) {
                                                                           string newValue
                                                                               = string.Format(BUILDING.STATUSITEMS
                                                                                    .WAITINGFORMATERIALS
                                                                                    .LINE_ITEM_MASS,
                                                                                keyValuePair.Key.ProperName(),
                                                                                GameUtil
                                                                                    .GetFormattedMass(keyValuePair
                                                                                         .Value,
                                                                                     GameUtil.TimeSlice.None,
                                                                                     GameUtil.MetricMassFormat
                                                                                         .UseThreshold,
                                                                                     true,
                                                                                     "{0:0.#}"));

                                                                           str = str.Replace("{ItemsRemaining}",
                                                                            newValue);
                                                                       }

                                                                       return str;
                                                                   };

            this.WaitingForMaterials = new MaterialsStatusItem("WaitingForMaterials",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Exclamation,
                                                               NotificationType.Neutral,
                                                               true,
                                                               OverlayModes.None.ID);

            this.WaitingForMaterials.resolveStringCallback = delegate(string str, object data) {
                                                                 IFetchList fetchList = (IFetchList)data;
                                                                 string     text      = "";
                                                                 Dictionary<Tag, float> remaining
                                                                     = fetchList.GetRemaining();

                                                                 if (remaining.Count > 0) {
                                                                     bool flag = true;
                                                                     foreach (KeyValuePair<Tag, float> keyValuePair in
                                                                              remaining) {
                                                                         if (keyValuePair.Value != 0f) {
                                                                             if (!flag) { text += "\n"; }

                                                                             if (Assets.IsTagCountable(keyValuePair
                                                                                 .Key)) {
                                                                                 text
                                                                                     += string.Format(BUILDING
                                                                                          .STATUSITEMS
                                                                                          .WAITINGFORMATERIALS
                                                                                          .LINE_ITEM_UNITS,
                                                                                      GameUtil
                                                                                          .GetUnitFormattedName(keyValuePair
                                                                                               .Key
                                                                                               .ProperName(),
                                                                                           keyValuePair.Value,
                                                                                           false));
                                                                             } else {
                                                                                 text
                                                                                     += string.Format(BUILDING
                                                                                          .STATUSITEMS
                                                                                          .WAITINGFORMATERIALS
                                                                                          .LINE_ITEM_MASS,
                                                                                      keyValuePair.Key.ProperName(),
                                                                                      GameUtil
                                                                                          .GetFormattedMass(keyValuePair
                                                                                               .Value,
                                                                                           GameUtil.TimeSlice
                                                                                               .None,
                                                                                           GameUtil
                                                                                               .MetricMassFormat
                                                                                               .UseThreshold,
                                                                                           true,
                                                                                           "{0:0.#}"));
                                                                             }

                                                                             flag = false;
                                                                         }
                                                                     }
                                                                 }

                                                                 str = str.Replace("{ItemsRemaining}", text);
                                                                 return str;
                                                             };

            this.WaitingForHighEnergyParticles = new StatusItem("WaitingForRadiation",
                                                                "BUILDING",
                                                                "status_item_need_high_energy_particles",
                                                                StatusItem.IconType.Custom,
                                                                NotificationType.BadMinor,
                                                                false,
                                                                OverlayModes.None.ID,
                                                                true,
                                                                129022,
                                                                null);

            this.MeltingDown = this.CreateStatusItem("MeltingDown",
                                                     "BUILDING",
                                                     "",
                                                     StatusItem.IconType.Info,
                                                     NotificationType.BadMinor,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.MissingFoundation = this.CreateStatusItem("MissingFoundation",
                                                           "BUILDING",
                                                           "status_item_missing_foundation",
                                                           StatusItem.IconType.Custom,
                                                           NotificationType.BadMinor,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.NeutroniumUnminable = this.CreateStatusItem("NeutroniumUnminable",
                                                             "BUILDING",
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.BadMinor,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.NeedGasIn = this.CreateStatusItem("NeedGasIn",
                                                   "BUILDING",
                                                   "status_item_need_supply_in",
                                                   StatusItem.IconType.Custom,
                                                   NotificationType.BadMinor,
                                                   false,
                                                   OverlayModes.GasConduits.ID,
                                                   true,
                                                   129022);

            this.NeedGasIn.resolveStringCallback = delegate(string str, object data) {
                                                       Tuple<ConduitType, Tag> tuple = (Tuple<ConduitType, Tag>)data;
                                                       string newValue
                                                           = string.Format(BUILDING.STATUSITEMS.NEEDGASIN.LINE_ITEM,
                                                                           tuple.second.ProperName());

                                                       str = str.Replace("{GasRequired}", newValue);
                                                       return str;
                                                   };

            this.NeedGasOut = this.CreateStatusItem("NeedGasOut",
                                                    "BUILDING",
                                                    "status_item_need_supply_out",
                                                    StatusItem.IconType.Custom,
                                                    NotificationType.BadMinor,
                                                    true,
                                                    OverlayModes.GasConduits.ID,
                                                    true,
                                                    129022);

            this.NeedLiquidIn = this.CreateStatusItem("NeedLiquidIn",
                                                      "BUILDING",
                                                      "status_item_need_supply_in",
                                                      StatusItem.IconType.Custom,
                                                      NotificationType.BadMinor,
                                                      false,
                                                      OverlayModes.LiquidConduits.ID,
                                                      true,
                                                      129022);

            this.NeedLiquidIn.resolveStringCallback = delegate(string str, object data) {
                                                          Tuple<ConduitType, Tag> tuple = (Tuple<ConduitType, Tag>)data;
                                                          string newValue
                                                              = string.Format(BUILDING.STATUSITEMS.NEEDLIQUIDIN
                                                                                  .LINE_ITEM,
                                                                              tuple.second.ProperName());

                                                          str = str.Replace("{LiquidRequired}", newValue);
                                                          return str;
                                                      };

            this.NeedLiquidOut = this.CreateStatusItem("NeedLiquidOut",
                                                       "BUILDING",
                                                       "status_item_need_supply_out",
                                                       StatusItem.IconType.Custom,
                                                       NotificationType.BadMinor,
                                                       true,
                                                       OverlayModes.LiquidConduits.ID,
                                                       true,
                                                       129022);

            this.NeedSolidIn = this.CreateStatusItem("NeedSolidIn",
                                                     "BUILDING",
                                                     "status_item_need_supply_in",
                                                     StatusItem.IconType.Custom,
                                                     NotificationType.BadMinor,
                                                     false,
                                                     OverlayModes.SolidConveyor.ID,
                                                     true,
                                                     129022);

            this.NeedSolidOut = this.CreateStatusItem("NeedSolidOut",
                                                      "BUILDING",
                                                      "status_item_need_supply_out",
                                                      StatusItem.IconType.Custom,
                                                      NotificationType.BadMinor,
                                                      true,
                                                      OverlayModes.SolidConveyor.ID,
                                                      true,
                                                      129022);

            this.NeedResourceMass = this.CreateStatusItem("NeedResourceMass",
                                                          "BUILDING",
                                                          "status_item_need_resource",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.BadMinor,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022);

            this.NeedResourceMass.resolveStringCallback = delegate(string str, object data) {
                                                              string text = "";
                                                              EnergyGenerator.Formula formula
                                                                  = (EnergyGenerator.Formula)data;

                                                              if (formula.inputs.Length != 0) {
                                                                  bool flag = true;
                                                                  foreach (EnergyGenerator.InputItem inputItem in
                                                                           formula.inputs) {
                                                                      if (!flag) {
                                                                          text += "\n";
                                                                          flag =  false;
                                                                      }

                                                                      text += string.Format(BUILDING.STATUSITEMS
                                                                           .NEEDRESOURCEMASS.LINE_ITEM,
                                                                       inputItem.tag.ProperName());
                                                                  }
                                                              }

                                                              str = str.Replace("{ResourcesRequired}", text);
                                                              return str;
                                                          };

            this.LiquidPipeEmpty = this.CreateStatusItem("LiquidPipeEmpty",
                                                         "BUILDING",
                                                         "status_item_no_liquid_to_pump",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.LiquidConduits.ID,
                                                         true,
                                                         129022);

            this.LiquidPipeObstructed = this.CreateStatusItem("LiquidPipeObstructed",
                                                              "BUILDING",
                                                              "status_item_wrong_resource_in_pipe",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              true,
                                                              OverlayModes.LiquidConduits.ID,
                                                              true,
                                                              129022);

            this.GasPipeEmpty = this.CreateStatusItem("GasPipeEmpty",
                                                      "BUILDING",
                                                      "status_item_no_gas_to_pump",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.BadMinor,
                                                      false,
                                                      OverlayModes.GasConduits.ID,
                                                      true,
                                                      129022);

            this.GasPipeObstructed = this.CreateStatusItem("GasPipeObstructed",
                                                           "BUILDING",
                                                           "status_item_wrong_resource_in_pipe",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           true,
                                                           OverlayModes.GasConduits.ID,
                                                           true,
                                                           129022);

            this.SolidPipeObstructed = this.CreateStatusItem("SolidPipeObstructed",
                                                             "BUILDING",
                                                             "status_item_wrong_resource_in_pipe",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Neutral,
                                                             true,
                                                             OverlayModes.SolidConveyor.ID,
                                                             true,
                                                             129022);

            this.NeedPlant = this.CreateStatusItem("NeedPlant",
                                                   "BUILDING",
                                                   "status_item_need_plant",
                                                   StatusItem.IconType.Custom,
                                                   NotificationType.BadMinor,
                                                   false,
                                                   OverlayModes.None.ID,
                                                   true,
                                                   129022);

            this.NeedPower = this.CreateStatusItem("NeedPower",
                                                   "BUILDING",
                                                   "status_item_need_power",
                                                   StatusItem.IconType.Custom,
                                                   NotificationType.BadMinor,
                                                   false,
                                                   OverlayModes.Power.ID,
                                                   true,
                                                   129022);

            this.NotEnoughPower = this.CreateStatusItem("NotEnoughPower",
                                                        "BUILDING",
                                                        "status_item_need_power",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.Power.ID,
                                                        true,
                                                        129022);

            this.PowerLoopDetected = this.CreateStatusItem("PowerLoopDetected",
                                                           "BUILDING",
                                                           "status_item_exclamation",
                                                           StatusItem.IconType.Custom,
                                                           NotificationType.BadMinor,
                                                           false,
                                                           OverlayModes.Power.ID,
                                                           true,
                                                           129022);

            this.CoolingWater = this.CreateStatusItem("CoolingWater",
                                                      "BUILDING",
                                                      "",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.Neutral,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.DispenseRequested = this.CreateStatusItem("DispenseRequested",
                                                           "BUILDING",
                                                           "status_item_exclamation",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.NewDuplicantsAvailable = this.CreateStatusItem("NewDuplicantsAvailable",
                                                                "BUILDING",
                                                                "status_item_new_duplicants_available",
                                                                StatusItem.IconType.Custom,
                                                                NotificationType.BadMinor,
                                                                false,
                                                                OverlayModes.None.ID,
                                                                true,
                                                                129022);

            this.NewDuplicantsAvailable.AddNotification(null, null, null);
            this.NewDuplicantsAvailable.notificationClickCallback = delegate(object data) {
                                                                        int idx = 0;
                                                                        for (int i = 0; i <
                                                                                 Components.Telepads.Items.Count; i++) {
                                                                            if (Components.Telepads[i]
                                                                                .GetComponent<KSelectable>()
                                                                                .IsSelected) {
                                                                                idx = (i + 1) %
                                                                                    Components.Telepads.Items.Count;

                                                                                break;
                                                                            }
                                                                        }

                                                                        Telepad targetTelepad
                                                                            = Components.Telepads[idx];

                                                                        int myWorldId = targetTelepad.GetMyWorldId();
                                                                        CameraController.Instance
                                                                            .ActiveWorldStarWipe(myWorldId,
                                                                             targetTelepad.transform.GetPosition(),
                                                                             10f,
                                                                             delegate() {
                                                                                 SelectTool.Instance
                                                                                     .Select(targetTelepad
                                                                                          .GetComponent<
                                                                                              KSelectable>(),
                                                                                      false);
                                                                             });
                                                                    };

            this.NoStorageFilterSet = this.CreateStatusItem("NoStorageFilterSet",
                                                            "BUILDING",
                                                            "status_item_no_filter_set",
                                                            StatusItem.IconType.Custom,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.NoSuitMarker = this.CreateStatusItem("NoSuitMarker",
                                                      "BUILDING",
                                                      "status_item_no_filter_set",
                                                      StatusItem.IconType.Custom,
                                                      NotificationType.BadMinor,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.SuitMarkerWrongSide = this.CreateStatusItem("suitMarkerWrongSide",
                                                             "BUILDING",
                                                             "status_item_no_filter_set",
                                                             StatusItem.IconType.Custom,
                                                             NotificationType.BadMinor,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.SuitMarkerTraversalAnytime = this.CreateStatusItem("suitMarkerTraversalAnytime",
                                                                    "BUILDING",
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    true,
                                                                    129022);

            this.SuitMarkerTraversalOnlyWhenRoomAvailable
                = this.CreateStatusItem("suitMarkerTraversalOnlyWhenRoomAvailable",
                                        "BUILDING",
                                        "",
                                        StatusItem.IconType.Info,
                                        NotificationType.Neutral,
                                        false,
                                        OverlayModes.None.ID,
                                        true,
                                        129022);

            this.NoFishableWaterBelow = this.CreateStatusItem("NoFishableWaterBelow",
                                                              "BUILDING",
                                                              "status_item_no_fishable_water_below",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.NoPowerConsumers = this.CreateStatusItem("NoPowerConsumers",
                                                          "BUILDING",
                                                          "status_item_no_power_consumers",
                                                          StatusItem.IconType.Custom,
                                                          NotificationType.BadMinor,
                                                          false,
                                                          OverlayModes.Power.ID,
                                                          true,
                                                          129022);

            this.NoWireConnected = this.CreateStatusItem("NoWireConnected",
                                                         "BUILDING",
                                                         "status_item_no_wire_connected",
                                                         StatusItem.IconType.Custom,
                                                         NotificationType.BadMinor,
                                                         true,
                                                         OverlayModes.Power.ID,
                                                         true,
                                                         129022);

            this.NoLogicWireConnected = this.CreateStatusItem("NoLogicWireConnected",
                                                              "BUILDING",
                                                              "status_item_no_logic_wire_connected",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.Logic.ID,
                                                              true,
                                                              129022);

            this.NoTubeConnected = this.CreateStatusItem("NoTubeConnected",
                                                         "BUILDING",
                                                         "status_item_need_supply_out",
                                                         StatusItem.IconType.Custom,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.NoTubeExits = this.CreateStatusItem("NoTubeExits",
                                                     "BUILDING",
                                                     "status_item_need_supply_out",
                                                     StatusItem.IconType.Custom,
                                                     NotificationType.BadMinor,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.StoredCharge = this.CreateStatusItem("StoredCharge",
                                                      "BUILDING",
                                                      "",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.Neutral,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.StoredCharge.resolveStringCallback = delegate(string str, object data) {
                                                          TravelTubeEntrance.SMInstance sminstance
                                                              = (TravelTubeEntrance.SMInstance)data;

                                                          if (sminstance != null) {
                                                              str = string.Format(str,
                                                               GameUtil.GetFormattedRoundedJoules(sminstance.master
                                                                   .AvailableJoules),
                                                               GameUtil.GetFormattedRoundedJoules(sminstance.master
                                                                   .TotalCapacity),
                                                               GameUtil.GetFormattedRoundedJoules(sminstance.master
                                                                   .UsageJoules));
                                                          }

                                                          return str;
                                                      };

            this.PendingDeconstruction = this.CreateStatusItem("PendingDeconstruction",
                                                               "BUILDING",
                                                               "status_item_pending_deconstruction",
                                                               StatusItem.IconType.Custom,
                                                               NotificationType.Neutral,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.PendingDeconstruction.conditionalOverlayCallback
                = new Func<HashedString, object, bool>(ShowInUtilityOverlay);

            this.PendingDemolition = this.CreateStatusItem("PendingDemolition",
                                                           "BUILDING",
                                                           "status_item_pending_deconstruction",
                                                           StatusItem.IconType.Custom,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.PendingDemolition.conditionalOverlayCallback
                = new Func<HashedString, object, bool>(ShowInUtilityOverlay);

            this.PendingRepair = this.CreateStatusItem("PendingRepair",
                                                       "BUILDING",
                                                       "status_item_pending_repair",
                                                       StatusItem.IconType.Custom,
                                                       NotificationType.BadMinor,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       true,
                                                       129022);

            this.PendingRepair.resolveStringCallback = ((string str, object data) =>
                                                            str.Replace("{DamageInfo}",
                                                                        ((Repairable.SMInstance)data).master
                                                                        .GetComponent<BuildingHP>()
                                                                        .GetDamageSourceInfo()
                                                                        .ToString()));

            this.PendingRepair.conditionalOverlayCallback = ((HashedString mode, object data) => true);
            this.RequiresSkillPerk = this.CreateStatusItem("RequiresSkillPerk",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.RequiresSkillPerk.resolveStringCallback = delegate(string str, object data) {
                                                               string    id   = (string)data;
                                                               SkillPerk perk = Db.Get().SkillPerks.Get(id);
                                                               List<Skill> skillsWithPerk
                                                                   = Db.Get().Skills.GetSkillsWithPerk(perk);

                                                               List<string> list = new List<string>();
                                                               foreach (Skill skill in skillsWithPerk) {
                                                                   if (!skill.deprecated) { list.Add(skill.Name); }
                                                               }

                                                               str = str.Replace("{Skills}",
                                                                string.Join(", ", list.ToArray()));

                                                               return str;
                                                           };

            this.DigRequiresSkillPerk = this.CreateStatusItem("DigRequiresSkillPerk",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.DigRequiresSkillPerk.resolveStringCallback = this.RequiresSkillPerk.resolveStringCallback;
            this.ColonyLacksRequiredSkillPerk = this.CreateStatusItem("ColonyLacksRequiredSkillPerk",
                                                                      "BUILDING",
                                                                      "status_item_role_required",
                                                                      StatusItem.IconType.Custom,
                                                                      NotificationType.BadMinor,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.ColonyLacksRequiredSkillPerk.resolveStringCallback = delegate(string str, object data) {
                                                                          string    id   = (string)data;
                                                                          SkillPerk perk = Db.Get().SkillPerks.Get(id);
                                                                          List<Skill> skillsWithPerk
                                                                              = Db.Get().Skills.GetSkillsWithPerk(perk);

                                                                          List<string> list = new List<string>();
                                                                          foreach (Skill skill in skillsWithPerk) {
                                                                              if (!skill.deprecated) {
                                                                                  list.Add(skill.Name);
                                                                              }
                                                                          }

                                                                          str = str.Replace("{Skills}",
                                                                           string.Join(", ", list.ToArray()));

                                                                          return str;
                                                                      };

            this.ColonyLacksRequiredSkillPerk.resolveTooltipCallback = delegate(string str, object data) {
                                                                           string    id   = (string)data;
                                                                           SkillPerk perk = Db.Get().SkillPerks.Get(id);
                                                                           List<Skill> skillsWithPerk
                                                                               = Db.Get()
                                                                                   .Skills.GetSkillsWithPerk(perk);

                                                                           List<string> list = new List<string>();
                                                                           foreach (Skill skill in skillsWithPerk) {
                                                                               if (!skill.deprecated) {
                                                                                   list.Add(skill.Name);
                                                                               }
                                                                           }

                                                                           str = str.Replace("{Skills}",
                                                                            string.Join(", ", list.ToArray()));

                                                                           return str;
                                                                       };

            this.ClusterColonyLacksRequiredSkillPerk = this.CreateStatusItem("ClusterColonyLacksRequiredSkillPerk",
                                                                             "BUILDING",
                                                                             "status_item_role_required",
                                                                             StatusItem.IconType.Custom,
                                                                             NotificationType.BadMinor,
                                                                             false,
                                                                             OverlayModes.None.ID,
                                                                             true,
                                                                             129022);

            this.ClusterColonyLacksRequiredSkillPerk.resolveStringCallback
                = this.ColonyLacksRequiredSkillPerk.resolveStringCallback;

            this.ClusterColonyLacksRequiredSkillPerk.resolveTooltipCallback
                = this.ColonyLacksRequiredSkillPerk.resolveTooltipCallback;

            this.WorkRequiresMinion = this.CreateStatusItem("WorkRequiresMinion",
                                                            "BUILDING",
                                                            "status_item_role_required",
                                                            StatusItem.IconType.Custom,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.SwitchStatusActive = this.CreateStatusItem("SwitchStatusActive",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Info,
                                                            NotificationType.Neutral,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.SwitchStatusInactive = this.CreateStatusItem("SwitchStatusInactive",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.LogicSwitchStatusActive = this.CreateStatusItem("LogicSwitchStatusActive",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Info,
                                                                 NotificationType.Neutral,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.LogicSwitchStatusInactive = this.CreateStatusItem("LogicSwitchStatusInactive",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.None.ID,
                                                                   true,
                                                                   129022);

            this.LogicSensorStatusActive = this.CreateStatusItem("LogicSensorStatusActive",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Info,
                                                                 NotificationType.Neutral,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.LogicSensorStatusInactive = this.CreateStatusItem("LogicSensorStatusInactive",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.None.ID,
                                                                   true,
                                                                   129022);

            this.PendingFish = this.CreateStatusItem("PendingFish",
                                                     "BUILDING",
                                                     "status_item_pending_fish",
                                                     StatusItem.IconType.Custom,
                                                     NotificationType.Neutral,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.PendingSwitchToggle = this.CreateStatusItem("PendingSwitchToggle",
                                                             "BUILDING",
                                                             "status_item_pending_switch_toggle",
                                                             StatusItem.IconType.Custom,
                                                             NotificationType.Neutral,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.PendingUpgrade = this.CreateStatusItem("PendingUpgrade",
                                                        "BUILDING",
                                                        "status_item_pending_upgrade",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.PendingWork = this.CreateStatusItem("PendingWork",
                                                     "BUILDING",
                                                     "",
                                                     StatusItem.IconType.Info,
                                                     NotificationType.BadMinor,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.PowerButtonOff = this.CreateStatusItem("PowerButtonOff",
                                                        "BUILDING",
                                                        "status_item_power_button_off",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.PressureOk = this.CreateStatusItem("PressureOk",
                                                    "BUILDING",
                                                    "",
                                                    StatusItem.IconType.Info,
                                                    NotificationType.BadMinor,
                                                    false,
                                                    OverlayModes.Oxygen.ID,
                                                    true,
                                                    129022);

            this.UnderPressure = this.CreateStatusItem("UnderPressure",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.BadMinor,
                                                       false,
                                                       OverlayModes.Oxygen.ID,
                                                       true,
                                                       129022);

            this.UnderPressure.resolveTooltipCallback = delegate(string str, object data) {
                                                            float mass = (float)data;
                                                            return str.Replace("{TargetPressure}",
                                                                               GameUtil.GetFormattedMass(mass,
                                                                                GameUtil.TimeSlice.None,
                                                                                GameUtil.MetricMassFormat
                                                                                    .UseThreshold,
                                                                                true,
                                                                                "{0:0.#}"));
                                                        };

            this.Unassigned = this.CreateStatusItem("Unassigned",
                                                    "BUILDING",
                                                    "",
                                                    StatusItem.IconType.Info,
                                                    NotificationType.BadMinor,
                                                    false,
                                                    OverlayModes.Rooms.ID,
                                                    true,
                                                    129022);

            this.AssignedPublic = this.CreateStatusItem("AssignedPublic",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Info,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.Rooms.ID,
                                                        true,
                                                        129022);

            this.UnderConstruction = this.CreateStatusItem("UnderConstruction",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.UnderConstructionNoWorker = this.CreateStatusItem("UnderConstructionNoWorker",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.None.ID,
                                                                   true,
                                                                   129022);

            this.Normal = this.CreateStatusItem("Normal",
                                                "BUILDING",
                                                "",
                                                StatusItem.IconType.Info,
                                                NotificationType.Neutral,
                                                false,
                                                OverlayModes.None.ID,
                                                true,
                                                129022);

            this.ManualGeneratorChargingUp = this.CreateStatusItem("ManualGeneratorChargingUp",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.Power.ID,
                                                                   true,
                                                                   129022);

            this.ManualGeneratorReleasingEnergy = this.CreateStatusItem("ManualGeneratorReleasingEnergy",
                                                                        "BUILDING",
                                                                        "",
                                                                        StatusItem.IconType.Info,
                                                                        NotificationType.Neutral,
                                                                        false,
                                                                        OverlayModes.Power.ID,
                                                                        true,
                                                                        129022);

            this.GeneratorOffline = this.CreateStatusItem("GeneratorOffline",
                                                          "BUILDING",
                                                          "",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.BadMinor,
                                                          false,
                                                          OverlayModes.Power.ID,
                                                          true,
                                                          129022);

            this.Pipe = this.CreateStatusItem("Pipe",
                                              "BUILDING",
                                              "",
                                              StatusItem.IconType.Info,
                                              NotificationType.Neutral,
                                              false,
                                              OverlayModes.LiquidConduits.ID,
                                              true,
                                              129022);

            this.Pipe.resolveStringCallback = delegate(string str, object data) {
                                                  Conduit conduit = (Conduit)data;
                                                  int     cell    = Grid.PosToCell(conduit);
                                                  ConduitFlow.ConduitContents contents
                                                      = conduit.GetFlowManager().GetContents(cell);

                                                  string text = BUILDING.STATUSITEMS.PIPECONTENTS.EMPTY;
                                                  if (contents.mass > 0f) {
                                                      Element element
                                                          = ElementLoader.FindElementByHash(contents.element);

                                                      text = string.Format(BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS,
                                                                           GameUtil.GetFormattedMass(contents.mass,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.MetricMassFormat.UseThreshold,
                                                                            true,
                                                                            "{0:0.#}"),
                                                                           element.name,
                                                                           GameUtil.GetFormattedTemperature(contents
                                                                                .temperature,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.TemperatureInterpretation
                                                                                .Absolute,
                                                                            true,
                                                                            false));

                                                      if (OverlayScreen.Instance      != null                    &&
                                                          OverlayScreen.Instance.mode == OverlayModes.Disease.ID &&
                                                          contents.diseaseIdx         != 255) {
                                                          text += string.Format(BUILDING.STATUSITEMS.PIPECONTENTS
                                                                                    .CONTENTS_WITH_DISEASE,
                                                                                GameUtil.GetFormattedDisease(contents
                                                                                     .diseaseIdx,
                                                                                 contents.diseaseCount,
                                                                                 true));
                                                      }
                                                  }

                                                  str = str.Replace("{Contents}", text);
                                                  return str;
                                              };

            this.Conveyor = this.CreateStatusItem("Conveyor",
                                                  "BUILDING",
                                                  "",
                                                  StatusItem.IconType.Info,
                                                  NotificationType.Neutral,
                                                  false,
                                                  OverlayModes.SolidConveyor.ID,
                                                  true,
                                                  129022);

            this.Conveyor.resolveStringCallback = delegate(string str, object data) {
                                                      int cell = Grid.PosToCell((SolidConduit)data);
                                                      SolidConduitFlow solidConduitFlow
                                                          = Game.Instance.solidConduitFlow;

                                                      SolidConduitFlow.ConduitContents contents
                                                          = solidConduitFlow.GetContents(cell);

                                                      string text = BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.EMPTY;
                                                      if (contents.pickupableHandle.IsValid()) {
                                                          Pickupable pickupable
                                                              = solidConduitFlow
                                                                  .GetPickupable(contents.pickupableHandle);

                                                          if (pickupable) {
                                                              PrimaryElement component
                                                                  = pickupable.GetComponent<PrimaryElement>();

                                                              float mass = component.Mass;
                                                              if (mass > 0f) {
                                                                  text
                                                                      = string.Format(BUILDING.STATUSITEMS
                                                                           .CONVEYOR_CONTENTS.CONTENTS,
                                                                       GameUtil.GetFormattedMass(mass,
                                                                        GameUtil.TimeSlice.None,
                                                                        GameUtil.MetricMassFormat.UseThreshold,
                                                                        true,
                                                                        "{0:0.#}"),
                                                                       pickupable.GetProperName(),
                                                                       GameUtil.GetFormattedTemperature(component
                                                                            .Temperature,
                                                                        GameUtil.TimeSlice.None,
                                                                        GameUtil.TemperatureInterpretation
                                                                            .Absolute,
                                                                        true,
                                                                        false));

                                                                  if (OverlayScreen.Instance != null &&
                                                                      OverlayScreen.Instance.mode ==
                                                                      OverlayModes.Disease.ID &&
                                                                      component.DiseaseIdx != 255) {
                                                                      text += string.Format(BUILDING.STATUSITEMS
                                                                           .CONVEYOR_CONTENTS.CONTENTS_WITH_DISEASE,
                                                                       GameUtil.GetFormattedDisease(component
                                                                            .DiseaseIdx,
                                                                        component.DiseaseCount,
                                                                        true));
                                                                  }
                                                              }
                                                          }
                                                      }

                                                      str = str.Replace("{Contents}", text);
                                                      return str;
                                                  };

            this.FabricatorIdle = this.CreateStatusItem("FabricatorIdle",
                                                        "BUILDING",
                                                        "status_item_fabricator_select",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.FabricatorEmpty = this.CreateStatusItem("FabricatorEmpty",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.FabricatorLacksHEP = this.CreateStatusItem("FabricatorLacksHEP",
                                                            "BUILDING",
                                                            "status_item_need_high_energy_particles",
                                                            StatusItem.IconType.Custom,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.FabricatorLacksHEP.resolveStringCallback = delegate(string str, object data) {
                                                                ComplexFabricator complexFabricator
                                                                    = (ComplexFabricator)data;

                                                                if (complexFabricator != null) {
                                                                    int num = complexFabricator.HighestHEPQueued();
                                                                    HighEnergyParticleStorage component
                                                                        = complexFabricator
                                                                            .GetComponent<HighEnergyParticleStorage>();

                                                                    str = str.Replace("{HEPRequired}", num.ToString());
                                                                    str = str.Replace("{CurrentHEP}",
                                                                     component.Particles.ToString());
                                                                }

                                                                return str;
                                                            };

            this.FossilMineIdle = this.CreateStatusItem("FossilIdle",
                                                        "CODEX.STORY_TRAITS.FOSSILHUNT",
                                                        "status_item_fabricator_select",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.FossilMineEmpty = this.CreateStatusItem("FossilEmpty",
                                                         "CODEX.STORY_TRAITS.FOSSILHUNT",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.FossilMinePendingWork = this.CreateStatusItem("FossilMinePendingWork",
                                                               "CODEX.STORY_TRAITS.FOSSILHUNT",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.FossilEntombed = new StatusItem("FossilEntombed",
                                                 "CODEX.STORY_TRAITS.FOSSILHUNT",
                                                 "status_item_entombed",
                                                 StatusItem.IconType.Custom,
                                                 NotificationType.BadMinor,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022,
                                                 null);

            this.Toilet = this.CreateStatusItem("Toilet",
                                                "BUILDING",
                                                "",
                                                StatusItem.IconType.Info,
                                                NotificationType.Neutral,
                                                false,
                                                OverlayModes.None.ID,
                                                true,
                                                129022);

            this.Toilet.resolveStringCallback = delegate(string str, object data) {
                                                    Toilet.StatesInstance statesInstance = (Toilet.StatesInstance)data;
                                                    if (statesInstance != null) {
                                                        str = str.Replace("{FlushesRemaining}",
                                                                          statesInstance.GetFlushesRemaining()
                                                                              .ToString());
                                                    }

                                                    return str;
                                                };

            this.ToiletNeedsEmptying = this.CreateStatusItem("ToiletNeedsEmptying",
                                                             "BUILDING",
                                                             "status_item_toilet_needs_emptying",
                                                             StatusItem.IconType.Custom,
                                                             NotificationType.BadMinor,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.DesalinatorNeedsEmptying = this.CreateStatusItem("DesalinatorNeedsEmptying",
                                                                  "BUILDING",
                                                                  "status_item_need_supply_out",
                                                                  StatusItem.IconType.Custom,
                                                                  NotificationType.BadMinor,
                                                                  false,
                                                                  OverlayModes.None.ID,
                                                                  true,
                                                                  129022);

            this.MilkSeparatorNeedsEmptying = this.CreateStatusItem("MilkSeparatorNeedsEmptying",
                                                                    "BUILDING",
                                                                    "status_item_need_supply_out",
                                                                    StatusItem.IconType.Custom,
                                                                    NotificationType.BadMinor,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    true,
                                                                    129022);

            this.Unusable = this.CreateStatusItem("Unusable",
                                                  "BUILDING",
                                                  "",
                                                  StatusItem.IconType.Info,
                                                  NotificationType.BadMinor,
                                                  false,
                                                  OverlayModes.None.ID,
                                                  true,
                                                  129022);

            this.NoResearchSelected = this.CreateStatusItem("NoResearchSelected",
                                                            "BUILDING",
                                                            "status_item_no_research_selected",
                                                            StatusItem.IconType.Custom,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.NoResearchSelected.AddNotification(null, null, null);
            StatusItem noResearchSelected = this.NoResearchSelected;
            noResearchSelected.resolveTooltipCallback
                = (Func<string, object, string>)Delegate.Combine(noResearchSelected.resolveTooltipCallback,
                                                                 new Func<string, object, string>(delegate(string str,
                                                                     object data) {
                                                                     string newValue = GameInputMapping
                                                                         .FindEntry(Action.ManageResearch)
                                                                         .mKeyCode.ToString();

                                                                     str = str.Replace("{RESEARCH_MENU_KEY}",
                                                                      newValue);

                                                                     return str;
                                                                 }));

            this.NoResearchSelected.notificationClickCallback
                = delegate(object d) { ManagementMenu.Instance.OpenResearch(null); };

            this.NoApplicableResearchSelected = this.CreateStatusItem("NoApplicableResearchSelected",
                                                                      "BUILDING",
                                                                      "status_item_no_research_selected",
                                                                      StatusItem.IconType.Custom,
                                                                      NotificationType.BadMinor,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.NoApplicableResearchSelected.AddNotification(null, null, null);
            this.NoApplicableAnalysisSelected = this.CreateStatusItem("NoApplicableAnalysisSelected",
                                                                      "BUILDING",
                                                                      "status_item_no_research_selected",
                                                                      StatusItem.IconType.Custom,
                                                                      NotificationType.BadMinor,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.NoApplicableAnalysisSelected.AddNotification(null, null, null);
            StatusItem noApplicableAnalysisSelected = this.NoApplicableAnalysisSelected;
            noApplicableAnalysisSelected.resolveTooltipCallback
                = (Func<string, object, string>)Delegate.Combine(noApplicableAnalysisSelected.resolveTooltipCallback,
                                                                 new Func<string, object, string>(delegate(string str,
                                                                     object data) {
                                                                     string newValue = GameInputMapping
                                                                         .FindEntry(Action.ManageStarmap)
                                                                         .mKeyCode.ToString();

                                                                     str = str.Replace("{STARMAP_MENU_KEY}",
                                                                      newValue);

                                                                     return str;
                                                                 }));

            this.NoApplicableAnalysisSelected.notificationClickCallback
                = delegate(object d) { ManagementMenu.Instance.OpenStarmap(); };

            this.NoResearchOrDestinationSelected = this.CreateStatusItem("NoResearchOrDestinationSelected",
                                                                         "BUILDING",
                                                                         "status_item_no_research_selected",
                                                                         StatusItem.IconType.Custom,
                                                                         NotificationType.BadMinor,
                                                                         false,
                                                                         OverlayModes.None.ID,
                                                                         true,
                                                                         129022);

            StatusItem noResearchOrDestinationSelected = this.NoResearchOrDestinationSelected;
            noResearchOrDestinationSelected.resolveTooltipCallback
                = (Func<string, object, string>)Delegate.Combine(noResearchOrDestinationSelected.resolveTooltipCallback,
                                                                 new Func<string, object, string>(delegate(string str,
                                                                     object data) {
                                                                     string newValue = GameInputMapping
                                                                         .FindEntry(Action.ManageStarmap)
                                                                         .mKeyCode.ToString();

                                                                     str = str.Replace("{STARMAP_MENU_KEY}",
                                                                      newValue);

                                                                     string newValue2
                                                                         = GameInputMapping
                                                                           .FindEntry(Action.ManageResearch)
                                                                           .mKeyCode.ToString();

                                                                     str = str.Replace("{RESEARCH_MENU_KEY}",
                                                                      newValue2);

                                                                     return str;
                                                                 }));

            this.NoResearchOrDestinationSelected.AddNotification(null, null, null);
            this.ValveRequest = this.CreateStatusItem("ValveRequest",
                                                      "BUILDING",
                                                      "",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.Neutral,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.ValveRequest.resolveStringCallback = delegate(string str, object data) {
                                                          Valve valve = (Valve)data;
                                                          str = str.Replace("{QueuedMaxFlow}",
                                                                            GameUtil.GetFormattedMass(valve
                                                                                 .QueuedMaxFlow,
                                                                             GameUtil.TimeSlice.PerSecond,
                                                                             GameUtil.MetricMassFormat.UseThreshold,
                                                                             true,
                                                                             "{0:0.#}"));

                                                          return str;
                                                      };

            this.EmittingLight = this.CreateStatusItem("EmittingLight",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       true,
                                                       129022);

            this.EmittingLight.resolveStringCallback = delegate(string str, object data) {
                                                           string newValue = GameInputMapping.FindEntry(Action.Overlay5)
                                                               .mKeyCode.ToString();

                                                           str = str.Replace("{LightGridOverlay}", newValue);
                                                           return str;
                                                       };

            this.KettleInsuficientSolids = this.CreateStatusItem("KettleInsuficientSolids",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Exclamation,
                                                                 NotificationType.Neutral,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 false,
                                                                 129022);

            this.KettleInsuficientSolids.resolveStringCallback = delegate(string str, object data) {
                                                                     IceKettle.Instance instance
                                                                         = (IceKettle.Instance)data;

                                                                     str = string.Format(str,
                                                                      GameUtil.GetFormattedMass(instance.def
                                                                           .KGToMeltPerBatch,
                                                                       GameUtil.TimeSlice.None,
                                                                       GameUtil.MetricMassFormat.UseThreshold,
                                                                       true,
                                                                       "{0:0.#}"));

                                                                     return str;
                                                                 };

            this.KettleInsuficientFuel = this.CreateStatusItem("KettleInsuficientFuel",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Exclamation,
                                                               NotificationType.Neutral,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               false,
                                                               129022);

            this.KettleInsuficientFuel.resolveStringCallback = delegate(string str, object data) {
                                                                   IceKettle.Instance instance
                                                                       = (IceKettle.Instance)data;

                                                                   str = string.Format(str,
                                                                    GameUtil.GetFormattedMass(instance
                                                                         .FuelRequiredForNextBratch,
                                                                     GameUtil.TimeSlice.None,
                                                                     GameUtil.MetricMassFormat.UseThreshold,
                                                                     true,
                                                                     "{0:0.#}"));

                                                                   return str;
                                                               };

            this.KettleInsuficientLiquidSpace = this.CreateStatusItem("KettleInsuficientLiquidSpace",
                                                                      "BUILDING",
                                                                      "",
                                                                      StatusItem.IconType.Info,
                                                                      NotificationType.Neutral,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.KettleInsuficientLiquidSpace.resolveStringCallback = delegate(string str, object data) {
                                                                          IceKettle.Instance instance
                                                                              = (IceKettle.Instance)data;

                                                                          str = string.Format(str,
                                                                           GameUtil.GetFormattedMass(instance
                                                                                .LiquidStored,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.MetricMassFormat
                                                                                .UseThreshold,
                                                                            true,
                                                                            "{0:0.#}"),
                                                                           GameUtil.GetFormattedMass(instance
                                                                                .LiquidTankCapacity,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.MetricMassFormat
                                                                                .UseThreshold,
                                                                            true,
                                                                            "{0:0.#}"),
                                                                           GameUtil.GetFormattedMass(instance.def
                                                                                .KGToMeltPerBatch,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.MetricMassFormat
                                                                                .UseThreshold,
                                                                            true,
                                                                            "{0:0.#}"));

                                                                          return str;
                                                                      };

            this.KettleMelting = this.CreateStatusItem("KettleMelting",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       true,
                                                       129022);

            this.KettleMelting.resolveStringCallback = delegate(string str, object data) {
                                                           IceKettle.Instance instance = (IceKettle.Instance)data;
                                                           str = string.Format(str,
                                                                               GameUtil.GetFormattedTemperature(instance
                                                                                    .def.TargetTemperature,
                                                                                GameUtil.TimeSlice.None,
                                                                                GameUtil.TemperatureInterpretation
                                                                                    .Absolute,
                                                                                true,
                                                                                false));

                                                           return str;
                                                       };

            this.RationBoxContents = this.CreateStatusItem("RationBoxContents",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.RationBoxContents.resolveStringCallback = delegate(string str, object data) {
                                                               RationBox rationBox = (RationBox)data;
                                                               if (rationBox == null) { return str; }

                                                               Storage component = rationBox.GetComponent<Storage>();
                                                               if (component == null) { return str; }

                                                               float num = 0f;
                                                               foreach (GameObject gameObject in component.items) {
                                                                   Edible component2
                                                                       = gameObject.GetComponent<Edible>();

                                                                   if (component2) { num += component2.Calories; }
                                                               }

                                                               str = str.Replace("{Stored}",
                                                                GameUtil.GetFormattedCalories(num,
                                                                 GameUtil.TimeSlice.None,
                                                                 true));

                                                               return str;
                                                           };

            this.EmittingElement = this.CreateStatusItem("EmittingElement",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.Neutral,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.EmittingElement.resolveStringCallback = delegate(string str, object data) {
                                                             IElementEmitter elementEmitter = (IElementEmitter)data;
                                                             string newValue = ElementLoader
                                                                               .FindElementByHash(elementEmitter
                                                                                   .Element)
                                                                               .tag.ProperName();

                                                             str = str.Replace("{ElementType}", newValue);
                                                             str = str.Replace("{FlowRate}",
                                                                               GameUtil.GetFormattedMass(elementEmitter
                                                                                    .AverageEmitRate,
                                                                                GameUtil.TimeSlice.PerSecond,
                                                                                GameUtil.MetricMassFormat
                                                                                    .UseThreshold,
                                                                                true,
                                                                                "{0:0.#}"));

                                                             return str;
                                                         };

            this.EmittingOxygenAvg = this.CreateStatusItem("EmittingOxygenAvg",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.EmittingOxygenAvg.resolveStringCallback = delegate(string str, object data) {
                                                               Sublimates sublimates = (Sublimates)data;
                                                               str = str.Replace("{FlowRate}",
                                                                GameUtil.GetFormattedMass(sublimates.AvgFlowRate(),
                                                                 GameUtil.TimeSlice.PerSecond,
                                                                 GameUtil.MetricMassFormat.UseThreshold,
                                                                 true,
                                                                 "{0:0.#}"));

                                                               return str;
                                                           };

            this.EmittingGasAvg = this.CreateStatusItem("EmittingGasAvg",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Info,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.EmittingGasAvg.resolveStringCallback = delegate(string str, object data) {
                                                            Sublimates sublimates = (Sublimates)data;
                                                            str = str.Replace("{Element}",
                                                                              ElementLoader
                                                                                  .FindElementByHash(sublimates.info
                                                                                      .sublimatedElement)
                                                                                  .name);

                                                            str = str.Replace("{FlowRate}",
                                                                              GameUtil.GetFormattedMass(sublimates
                                                                                   .AvgFlowRate(),
                                                                               GameUtil.TimeSlice.PerSecond,
                                                                               GameUtil.MetricMassFormat
                                                                                   .UseThreshold,
                                                                               true,
                                                                               "{0:0.#}"));

                                                            return str;
                                                        };

            this.EmittingBlockedHighPressure = this.CreateStatusItem("EmittingBlockedHighPressure",
                                                                     "BUILDING",
                                                                     "",
                                                                     StatusItem.IconType.Info,
                                                                     NotificationType.Neutral,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     true,
                                                                     129022);

            this.EmittingBlockedHighPressure.resolveStringCallback = delegate(string str, object data) {
                                                                         Sublimates sublimates = (Sublimates)data;
                                                                         str = str.Replace("{Element}",
                                                                          ElementLoader
                                                                              .FindElementByHash(sublimates.info
                                                                                  .sublimatedElement)
                                                                              .name);

                                                                         return str;
                                                                     };

            this.EmittingBlockedLowTemperature = this.CreateStatusItem("EmittingBlockedLowTemperature",
                                                                       "BUILDING",
                                                                       "",
                                                                       StatusItem.IconType.Info,
                                                                       NotificationType.Neutral,
                                                                       false,
                                                                       OverlayModes.None.ID,
                                                                       true,
                                                                       129022);

            this.EmittingBlockedLowTemperature.resolveStringCallback = delegate(string str, object data) {
                                                                           Sublimates sublimates = (Sublimates)data;
                                                                           str = str.Replace("{Element}",
                                                                            ElementLoader
                                                                                .FindElementByHash(sublimates.info
                                                                                    .sublimatedElement)
                                                                                .name);

                                                                           return str;
                                                                       };

            this.PumpingLiquidOrGas = this.CreateStatusItem("PumpingLiquidOrGas",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Info,
                                                            NotificationType.Neutral,
                                                            false,
                                                            OverlayModes.LiquidConduits.ID,
                                                            true,
                                                            129022);

            this.PumpingLiquidOrGas.resolveStringCallback = delegate(string str, object data) {
                                                                HandleVector<int>.Handle handle
                                                                    = (HandleVector<int>.Handle)data;

                                                                float averageRate = Game.Instance.accumulators
                                                                    .GetAverageRate(handle);

                                                                str = str.Replace("{FlowRate}",
                                                                 GameUtil.GetFormattedMass(averageRate,
                                                                  GameUtil.TimeSlice.PerSecond,
                                                                  GameUtil.MetricMassFormat.UseThreshold,
                                                                  true,
                                                                  "{0:0.#}"));

                                                                return str;
                                                            };

            this.PipeMayMelt = this.CreateStatusItem("PipeMayMelt",
                                                     "BUILDING",
                                                     "status_item_need_supply_out",
                                                     StatusItem.IconType.Custom,
                                                     NotificationType.BadMinor,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.NoLiquidElementToPump = this.CreateStatusItem("NoLiquidElementToPump",
                                                               "BUILDING",
                                                               "status_item_no_liquid_to_pump",
                                                               StatusItem.IconType.Custom,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.LiquidConduits.ID,
                                                               true,
                                                               129022);

            this.NoGasElementToPump = this.CreateStatusItem("NoGasElementToPump",
                                                            "BUILDING",
                                                            "status_item_no_gas_to_pump",
                                                            StatusItem.IconType.Custom,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.GasConduits.ID,
                                                            true,
                                                            129022);

            this.NoFilterElementSelected = this.CreateStatusItem("NoFilterElementSelected",
                                                                 "BUILDING",
                                                                 "status_item_need_supply_out",
                                                                 StatusItem.IconType.Custom,
                                                                 NotificationType.BadMinor,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.NoLureElementSelected = this.CreateStatusItem("NoLureElementSelected",
                                                               "BUILDING",
                                                               "status_item_need_supply_out",
                                                               StatusItem.IconType.Custom,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.ElementConsumer = this.CreateStatusItem("ElementConsumer",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.Neutral,
                                                         true,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.ElementConsumer.resolveStringCallback = delegate(string str, object data) {
                                                             ElementConsumer elementConsumer = (ElementConsumer)data;
                                                             string newValue = ElementLoader
                                                                               .FindElementByHash(elementConsumer
                                                                                   .elementToConsume)
                                                                               .tag.ProperName();

                                                             str = str.Replace("{ElementTypes}", newValue);
                                                             str = str.Replace("{FlowRate}",
                                                                               GameUtil.GetFormattedMass(elementConsumer
                                                                                    .AverageConsumeRate,
                                                                                GameUtil.TimeSlice.PerSecond,
                                                                                GameUtil.MetricMassFormat
                                                                                    .UseThreshold,
                                                                                true,
                                                                                "{0:0.#}"));

                                                             return str;
                                                         };

            this.ElementEmitterOutput = this.CreateStatusItem("ElementEmitterOutput",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              true,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.ElementEmitterOutput.resolveStringCallback = delegate(string str, object data) {
                                                                  ElementEmitter elementEmitter = (ElementEmitter)data;
                                                                  if (elementEmitter != null) {
                                                                      str = str.Replace("{ElementTypes}",
                                                                       elementEmitter.outputElement.Name);

                                                                      str = str.Replace("{FlowRate}",
                                                                       GameUtil.GetFormattedMass(elementEmitter
                                                                                .outputElement
                                                                                .massGenerationRate /
                                                                            elementEmitter.emissionFrequency,
                                                                        GameUtil.TimeSlice.PerSecond,
                                                                        GameUtil.MetricMassFormat.UseThreshold,
                                                                        true,
                                                                        "{0:0.#}"));
                                                                  }

                                                                  return str;
                                                              };

            this.AwaitingWaste = this.CreateStatusItem("AwaitingWaste",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       true,
                                                       OverlayModes.None.ID,
                                                       true,
                                                       129022);

            this.AwaitingCompostFlip = this.CreateStatusItem("AwaitingCompostFlip",
                                                             "BUILDING",
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Neutral,
                                                             true,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.BatteryJoulesAvailable = this.CreateStatusItem("JoulesAvailable",
                                                                "BUILDING",
                                                                "",
                                                                StatusItem.IconType.Info,
                                                                NotificationType.Neutral,
                                                                false,
                                                                OverlayModes.Power.ID,
                                                                true,
                                                                129022);

            this.BatteryJoulesAvailable.resolveStringCallback = delegate(string str, object data) {
                                                                    Battery battery = (Battery)data;
                                                                    str = str.Replace("{JoulesAvailable}",
                                                                     GameUtil.GetFormattedJoules(battery
                                                                          .JoulesAvailable,
                                                                      "F1",
                                                                      GameUtil.TimeSlice.None));

                                                                    str = str.Replace("{JoulesCapacity}",
                                                                     GameUtil.GetFormattedJoules(battery.Capacity,
                                                                      "F1",
                                                                      GameUtil.TimeSlice.None));

                                                                    return str;
                                                                };

            this.ElectrobankJoulesAvailable = this.CreateStatusItem("ElectrobankJoulesAvailable",
                                                                    "BUILDING",
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.Power.ID,
                                                                    true,
                                                                    129022);

            this.ElectrobankJoulesAvailable.resolveStringCallback = delegate(string str, object data) {
                                                                        ElectrobankDischarger electrobankDischarger
                                                                            = (ElectrobankDischarger)data;

                                                                        str = str.Replace("{JoulesAvailable}",
                                                                         GameUtil
                                                                             .GetFormattedJoules(electrobankDischarger
                                                                                  .ElectrobankJoulesStored,
                                                                              "F1",
                                                                              GameUtil.TimeSlice.None));

                                                                        str = str.Replace("{JoulesCapacity}",
                                                                         GameUtil.GetFormattedJoules(120000f,
                                                                          "F1",
                                                                          GameUtil.TimeSlice.None));

                                                                        return str;
                                                                    };

            this.Wattage = this.CreateStatusItem("Wattage",
                                                 "BUILDING",
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.Power.ID,
                                                 true,
                                                 129022);

            this.Wattage.resolveStringCallback = delegate(string str, object data) {
                                                     Generator generator = (Generator)data;
                                                     str = str.Replace("{Wattage}",
                                                                       GameUtil.GetFormattedWattage(generator
                                                                            .WattageRating,
                                                                        GameUtil.WattageFormatterUnit.Automatic,
                                                                        true));

                                                     return str;
                                                 };

            this.SolarPanelWattage = this.CreateStatusItem("SolarPanelWattage",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.Power.ID,
                                                           true,
                                                           129022);

            this.SolarPanelWattage.resolveStringCallback = delegate(string str, object data) {
                                                               SolarPanel solarPanel = (SolarPanel)data;
                                                               str = str.Replace("{Wattage}",
                                                                GameUtil.GetFormattedWattage(solarPanel
                                                                     .CurrentWattage,
                                                                 GameUtil.WattageFormatterUnit.Automatic,
                                                                 true));

                                                               return str;
                                                           };

            this.ModuleSolarPanelWattage = this.CreateStatusItem("ModuleSolarPanelWattage",
                                                                 "BUILDING",
                                                                 "",
                                                                 StatusItem.IconType.Info,
                                                                 NotificationType.Neutral,
                                                                 false,
                                                                 OverlayModes.Power.ID,
                                                                 true,
                                                                 129022);

            this.ModuleSolarPanelWattage.resolveStringCallback = delegate(string str, object data) {
                                                                     ModuleSolarPanel moduleSolarPanel
                                                                         = (ModuleSolarPanel)data;

                                                                     str = str.Replace("{Wattage}",
                                                                      GameUtil.GetFormattedWattage(moduleSolarPanel
                                                                           .CurrentWattage,
                                                                       GameUtil.WattageFormatterUnit.Automatic,
                                                                       true));

                                                                     return str;
                                                                 };

            this.SteamTurbineWattage = this.CreateStatusItem("SteamTurbineWattage",
                                                             "BUILDING",
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Neutral,
                                                             false,
                                                             OverlayModes.Power.ID,
                                                             true,
                                                             129022);

            this.SteamTurbineWattage.resolveStringCallback = delegate(string str, object data) {
                                                                 SteamTurbine steamTurbine = (SteamTurbine)data;
                                                                 str = str.Replace("{Wattage}",
                                                                  GameUtil.GetFormattedWattage(steamTurbine
                                                                       .CurrentWattage,
                                                                   GameUtil.WattageFormatterUnit.Automatic,
                                                                   true));

                                                                 return str;
                                                             };

            this.Wattson = this.CreateStatusItem("Wattson",
                                                 "BUILDING",
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022);

            this.Wattson.resolveStringCallback = delegate(string str, object data) {
                                                     Telepad telepad = (Telepad)data;
                                                     if (GameFlowManager.Instance != null &&
                                                         GameFlowManager.Instance.IsGameOver()) {
                                                         str = BUILDING.STATUSITEMS.WATTSONGAMEOVER.NAME;
                                                     } else if (telepad.GetComponent<Operational>().IsOperational) {
                                                         str = str.Replace("{TimeRemaining}",
                                                                           GameUtil.GetFormattedCycles(telepad
                                                                                .GetTimeRemaining(),
                                                                            "F1",
                                                                            false));
                                                     } else {
                                                         str = str.Replace("{TimeRemaining}",
                                                                           BUILDING.STATUSITEMS.WATTSON.UNAVAILABLE);
                                                     }

                                                     return str;
                                                 };

            this.FlushToilet = this.CreateStatusItem("FlushToilet",
                                                     "BUILDING",
                                                     "",
                                                     StatusItem.IconType.Info,
                                                     NotificationType.Neutral,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.FlushToilet.resolveStringCallback = delegate(string str, object data) {
                                                         FlushToilet.SMInstance sminstance
                                                             = (FlushToilet.SMInstance)data;

                                                         return BUILDING.STATUSITEMS.FLUSHTOILET.NAME
                                                                        .Replace("{toilet}",
                                                                         sminstance.master.GetProperName());
                                                     };

            this.FlushToilet.resolveTooltipCallback
                = ((string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILET.TOOLTIP);

            this.FlushToiletInUse = this.CreateStatusItem("FlushToiletInUse",
                                                          "BUILDING",
                                                          "",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022);

            this.FlushToiletInUse.resolveStringCallback = delegate(string str, object data) {
                                                              FlushToilet.SMInstance sminstance
                                                                  = (FlushToilet.SMInstance)data;

                                                              return BUILDING.STATUSITEMS.FLUSHTOILETINUSE.NAME
                                                                             .Replace("{toilet}",
                                                                              sminstance.master.GetProperName());
                                                          };

            this.FlushToiletInUse.resolveTooltipCallback
                = ((string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILETINUSE.TOOLTIP);

            this.WireNominal = this.CreateStatusItem("WireNominal",
                                                     "BUILDING",
                                                     "",
                                                     StatusItem.IconType.Info,
                                                     NotificationType.Neutral,
                                                     false,
                                                     OverlayModes.Power.ID,
                                                     true,
                                                     129022);

            this.WireConnected = this.CreateStatusItem("WireConnected",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.Power.ID,
                                                       true,
                                                       129022);

            this.WireDisconnected = this.CreateStatusItem("WireDisconnected",
                                                          "BUILDING",
                                                          "",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.BadMinor,
                                                          false,
                                                          OverlayModes.Power.ID,
                                                          true,
                                                          129022);

            this.Overheated = this.CreateStatusItem("Overheated",
                                                    "BUILDING",
                                                    "",
                                                    StatusItem.IconType.Exclamation,
                                                    NotificationType.Bad,
                                                    false,
                                                    OverlayModes.None.ID,
                                                    true,
                                                    129022);

            this.Overloaded = this.CreateStatusItem("Overloaded",
                                                    "BUILDING",
                                                    "",
                                                    StatusItem.IconType.Exclamation,
                                                    NotificationType.Bad,
                                                    false,
                                                    OverlayModes.None.ID,
                                                    true,
                                                    129022);

            this.LogicOverloaded = this.CreateStatusItem("LogicOverloaded",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Exclamation,
                                                         NotificationType.Bad,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.Cooling = this.CreateStatusItem("Cooling",
                                                 "BUILDING",
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022);

            Func<string, object, string> resolveStringCallback2 = delegate(string str, object data) {
                                                                      AirConditioner airConditioner
                                                                          = (AirConditioner)data;

                                                                      return string.Format(str,
                                                                       GameUtil
                                                                           .GetFormattedTemperature(airConditioner
                                                                                .lastGasTemp,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.TemperatureInterpretation
                                                                                .Absolute,
                                                                            true,
                                                                            false));
                                                                  };

            this.CoolingStalledColdGas = this.CreateStatusItem("CoolingStalledColdGas",
                                                               "BUILDING",
                                                               "status_item_vent_disabled",
                                                               StatusItem.IconType.Custom,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.CoolingStalledColdGas.resolveStringCallback = resolveStringCallback2;
            this.CoolingStalledColdLiquid = this.CreateStatusItem("CoolingStalledColdLiquid",
                                                                  "BUILDING",
                                                                  "status_item_vent_disabled",
                                                                  StatusItem.IconType.Custom,
                                                                  NotificationType.BadMinor,
                                                                  false,
                                                                  OverlayModes.None.ID,
                                                                  true,
                                                                  129022);

            this.CoolingStalledColdLiquid.resolveStringCallback = resolveStringCallback2;
            Func<string, object, string> resolveStringCallback3 = delegate(string str, object data) {
                                                                      AirConditioner airConditioner
                                                                          = (AirConditioner)data;

                                                                      return string.Format(str,
                                                                       GameUtil
                                                                           .GetFormattedTemperature(airConditioner
                                                                                .lastEnvTemp,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.TemperatureInterpretation
                                                                                .Absolute,
                                                                            true,
                                                                            false),
                                                                       GameUtil
                                                                           .GetFormattedTemperature(airConditioner
                                                                                .lastGasTemp,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.TemperatureInterpretation
                                                                                .Absolute,
                                                                            true,
                                                                            false),
                                                                       GameUtil
                                                                           .GetFormattedTemperature(airConditioner
                                                                                .maxEnvironmentDelta,
                                                                            GameUtil.TimeSlice.None,
                                                                            GameUtil.TemperatureInterpretation
                                                                                .Relative,
                                                                            true,
                                                                            false));
                                                                  };

            this.CoolingStalledHotEnv = this.CreateStatusItem("CoolingStalledHotEnv",
                                                              "BUILDING",
                                                              "status_item_vent_disabled",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.CoolingStalledHotEnv.resolveStringCallback = resolveStringCallback3;
            this.CoolingStalledHotLiquid = this.CreateStatusItem("CoolingStalledHotLiquid",
                                                                 "BUILDING",
                                                                 "status_item_vent_disabled",
                                                                 StatusItem.IconType.Custom,
                                                                 NotificationType.BadMinor,
                                                                 false,
                                                                 OverlayModes.None.ID,
                                                                 true,
                                                                 129022);

            this.CoolingStalledHotLiquid.resolveStringCallback = resolveStringCallback3;
            this.MissingRequirements = this.CreateStatusItem("MissingRequirements",
                                                             "BUILDING",
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Neutral,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.GettingReady = this.CreateStatusItem("GettingReady",
                                                      "BUILDING",
                                                      "",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.Neutral,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.Working = this.CreateStatusItem("Working",
                                                 "BUILDING",
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022);

            this.NeedsValidRegion = this.CreateStatusItem("NeedsValidRegion",
                                                          "BUILDING",
                                                          "status_item_exclamation",
                                                          StatusItem.IconType.Custom,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022);

            this.NeedSeed = this.CreateStatusItem("NeedSeed",
                                                  "BUILDING",
                                                  "status_item_fabricator_empty",
                                                  StatusItem.IconType.Custom,
                                                  NotificationType.BadMinor,
                                                  false,
                                                  OverlayModes.None.ID,
                                                  true,
                                                  129022);

            this.AwaitingSeedDelivery = this.CreateStatusItem("AwaitingSeedDelivery",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.AwaitingBaitDelivery = this.CreateStatusItem("AwaitingBaitDelivery",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.NoAvailableSeed = this.CreateStatusItem("NoAvailableSeed",
                                                         "BUILDING",
                                                         "status_item_resource_unavailable",
                                                         StatusItem.IconType.Custom,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.NeedEgg = this.CreateStatusItem("NeedEgg",
                                                 "BUILDING",
                                                 "status_item_fabricator_empty",
                                                 StatusItem.IconType.Custom,
                                                 NotificationType.BadMinor,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022);

            this.AwaitingEggDelivery = this.CreateStatusItem("AwaitingEggDelivery",
                                                             "BUILDING",
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Neutral,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.NoAvailableEgg = this.CreateStatusItem("NoAvailableEgg",
                                                        "BUILDING",
                                                        "status_item_resource_unavailable",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.Grave = this.CreateStatusItem("Grave",
                                               "BUILDING",
                                               "",
                                               StatusItem.IconType.Info,
                                               NotificationType.Neutral,
                                               false,
                                               OverlayModes.None.ID,
                                               true,
                                               129022);

            this.Grave.resolveStringCallback = delegate(string str, object data) {
                                                   Grave.StatesInstance statesInstance = (Grave.StatesInstance)data;
                                                   string text = str.Replace("{DeadDupe}",
                                                                             statesInstance.master.graveName);

                                                   string[] strings
                                                       = LocString.GetStrings(typeof(NAMEGEN.GRAVE.EPITAPHS));

                                                   int num = statesInstance.master.epitaphIdx % strings.Length;
                                                   return text.Replace("{Epitaph}", strings[num]);
                                               };

            this.GraveEmpty = this.CreateStatusItem("GraveEmpty",
                                                    "BUILDING",
                                                    "",
                                                    StatusItem.IconType.Info,
                                                    NotificationType.Neutral,
                                                    false,
                                                    OverlayModes.None.ID,
                                                    true,
                                                    129022);

            this.CannotCoolFurther = this.CreateStatusItem("CannotCoolFurther",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.CannotCoolFurther.resolveTooltipCallback = delegate(string str, object data) {
                                                                float temp = (float)data;
                                                                return str.Replace("{0}",
                                                                 GameUtil.GetFormattedTemperature(temp,
                                                                  GameUtil.TimeSlice.None,
                                                                  GameUtil.TemperatureInterpretation.Absolute,
                                                                  true,
                                                                  false));
                                                            };

            this.BuildingDisabled = this.CreateStatusItem("BuildingDisabled",
                                                          "BUILDING",
                                                          "status_item_building_disabled",
                                                          StatusItem.IconType.Custom,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022);

            this.Expired = this.CreateStatusItem("Expired",
                                                 "BUILDING",
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022);

            this.PumpingStation = this.CreateStatusItem("PumpingStation",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Info,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.PumpingStation.resolveStringCallback = delegate(string str, object data) {
                                                            LiquidPumpingStation liquidPumpingStation
                                                                = (LiquidPumpingStation)data;

                                                            if (liquidPumpingStation != null) {
                                                                return liquidPumpingStation.ResolveString(str);
                                                            }

                                                            return str;
                                                        };

            this.EmptyPumpingStation = this.CreateStatusItem("EmptyPumpingStation",
                                                             "BUILDING",
                                                             "status_item_no_liquid_to_pump",
                                                             StatusItem.IconType.Custom,
                                                             NotificationType.Neutral,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.WellPressurizing = this.CreateStatusItem("WellPressurizing",
                                                          BUILDING.STATUSITEMS.WELL_PRESSURIZING.NAME,
                                                          BUILDING.STATUSITEMS.WELL_PRESSURIZING.TOOLTIP,
                                                          "",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          129022);

            this.WellPressurizing.resolveStringCallback = delegate(string str, object data) {
                                                              OilWellCap.StatesInstance statesInstance
                                                                  = (OilWellCap.StatesInstance)data;

                                                              if (statesInstance != null) {
                                                                  return string.Format(str,
                                                                   GameUtil.GetFormattedPercent(100f *
                                                                        statesInstance.GetPressurePercent(),
                                                                    GameUtil.TimeSlice.None));
                                                              }

                                                              return str;
                                                          };

            this.WellOverpressure = this.CreateStatusItem("WellOverpressure",
                                                          BUILDING.STATUSITEMS.WELL_OVERPRESSURE.NAME,
                                                          BUILDING.STATUSITEMS.WELL_OVERPRESSURE.TOOLTIP,
                                                          "",
                                                          StatusItem.IconType.Exclamation,
                                                          NotificationType.BadMinor,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          129022);

            this.ReleasingPressure = this.CreateStatusItem("ReleasingPressure",
                                                           BUILDING.STATUSITEMS.RELEASING_PRESSURE.NAME,
                                                           BUILDING.STATUSITEMS.RELEASING_PRESSURE.TOOLTIP,
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           129022);

            this.ReactorMeltdown = this.CreateStatusItem("ReactorMeltdown",
                                                         BUILDING.STATUSITEMS.REACTORMELTDOWN.NAME,
                                                         BUILDING.STATUSITEMS.REACTORMELTDOWN.TOOLTIP,
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.Bad,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         129022);

            this.TooCold = this.CreateStatusItem("TooCold",
                                                 BUILDING.STATUSITEMS.TOO_COLD.NAME,
                                                 BUILDING.STATUSITEMS.TOO_COLD.TOOLTIP,
                                                 "",
                                                 StatusItem.IconType.Exclamation,
                                                 NotificationType.BadMinor,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 129022);

            this.IncubatorProgress = this.CreateStatusItem("IncubatorProgress",
                                                           "BUILDING",
                                                           "",
                                                           StatusItem.IconType.Info,
                                                           NotificationType.Neutral,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022);

            this.IncubatorProgress.resolveStringCallback = delegate(string str, object data) {
                                                               EggIncubator eggIncubator = (EggIncubator)data;
                                                               str = str.Replace("{Percent}",
                                                                GameUtil.GetFormattedPercent(eggIncubator
                                                                         .GetProgress() *
                                                                     100f,
                                                                 GameUtil.TimeSlice.None));

                                                               return str;
                                                           };

            this.HabitatNeedsEmptying = this.CreateStatusItem("HabitatNeedsEmptying",
                                                              "BUILDING",
                                                              "status_item_need_supply_out",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.DetectorScanning = this.CreateStatusItem("DetectorScanning",
                                                          "BUILDING",
                                                          "",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022);

            this.IncomingMeteors = this.CreateStatusItem("IncomingMeteors",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Exclamation,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.HasGantry = this.CreateStatusItem("HasGantry",
                                                   "BUILDING",
                                                   "",
                                                   StatusItem.IconType.Info,
                                                   NotificationType.Neutral,
                                                   false,
                                                   OverlayModes.None.ID,
                                                   true,
                                                   129022);

            this.MissingGantry = this.CreateStatusItem("MissingGantry",
                                                       "BUILDING",
                                                       "status_item_exclamation",
                                                       StatusItem.IconType.Custom,
                                                       NotificationType.BadMinor,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       true,
                                                       129022);

            this.DisembarkingDuplicant = this.CreateStatusItem("DisembarkingDuplicant",
                                                               "BUILDING",
                                                               "status_item_new_duplicants_available",
                                                               StatusItem.IconType.Custom,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.RocketName = this.CreateStatusItem("RocketName",
                                                    "BUILDING",
                                                    "",
                                                    StatusItem.IconType.Info,
                                                    NotificationType.Neutral,
                                                    false,
                                                    OverlayModes.None.ID,
                                                    true,
                                                    129022);

            this.RocketName.resolveStringCallback = delegate(string str, object data) {
                                                        RocketModule rocketModule = (RocketModule)data;
                                                        if (rocketModule != null) {
                                                            return str.Replace("{0}",
                                                                               rocketModule.GetParentRocketName());
                                                        }

                                                        return str;
                                                    };

            this.RocketName.resolveTooltipCallback = delegate(string str, object data) {
                                                         RocketModule rocketModule = (RocketModule)data;
                                                         if (rocketModule != null) {
                                                             return str.Replace("{0}",
                                                                                rocketModule.GetParentRocketName());
                                                         }

                                                         return str;
                                                     };

            this.LandedRocketLacksPassengerModule = this.CreateStatusItem("LandedRocketLacksPassengerModule",
                                                                          "BUILDING",
                                                                          "",
                                                                          StatusItem.IconType.Info,
                                                                          NotificationType.BadMinor,
                                                                          false,
                                                                          OverlayModes.None.ID,
                                                                          true,
                                                                          129022);

            this.PathNotClear = new StatusItem("PATH_NOT_CLEAR",
                                               "BUILDING",
                                               "status_item_no_sky",
                                               StatusItem.IconType.Custom,
                                               NotificationType.BadMinor,
                                               false,
                                               OverlayModes.None.ID,
                                               true,
                                               129022,
                                               null);

            this.PathNotClear.resolveTooltipCallback = delegate(string str, object data) {
                                                           ConditionFlightPathIsClear conditionFlightPathIsClear
                                                               = (ConditionFlightPathIsClear)data;

                                                           if (conditionFlightPathIsClear != null) {
                                                               str = string.Format(str,
                                                                conditionFlightPathIsClear.GetObstruction());
                                                           }

                                                           return str;
                                                       };

            this.InvalidPortOverlap = this.CreateStatusItem("InvalidPortOverlap",
                                                            "BUILDING",
                                                            "status_item_exclamation",
                                                            StatusItem.IconType.Custom,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.InvalidPortOverlap.AddNotification(null, null, null);
            this.EmergencyPriority = this.CreateStatusItem("EmergencyPriority",
                                                           BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME,
                                                           BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.TOOLTIP,
                                                           "status_item_doubleexclamation",
                                                           StatusItem.IconType.Custom,
                                                           NotificationType.Bad,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           129022);

            this.EmergencyPriority.AddNotification(null,
                                                   BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_NAME,
                                                   BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_TOOLTIP);

            this.SkillPointsAvailable = this.CreateStatusItem("SkillPointsAvailable",
                                                              BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.NAME,
                                                              BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.TOOLTIP,
                                                              "status_item_jobs",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              129022);

            this.Baited = this.CreateStatusItem("Baited",
                                                "BUILDING",
                                                "",
                                                StatusItem.IconType.Info,
                                                NotificationType.Neutral,
                                                false,
                                                OverlayModes.None.ID,
                                                false,
                                                129022);

            this.Baited.resolveStringCallback = delegate(string str, object data) {
                                                    Element element
                                                        = ElementLoader
                                                            .FindElementByName(((CreatureBait.StatesInstance)data)
                                                                               .master.baitElement.ToString());

                                                    str = str.Replace("{0}", element.name);
                                                    return str;
                                                };

            this.Baited.resolveTooltipCallback = delegate(string str, object data) {
                                                     Element element
                                                         = ElementLoader
                                                             .FindElementByName(((CreatureBait.StatesInstance)data)
                                                                                .master.baitElement.ToString());

                                                     str = str.Replace("{0}", element.name);
                                                     return str;
                                                 };

            this.TanningLightSufficient = this.CreateStatusItem("TanningLightSufficient",
                                                                BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.NAME,
                                                                BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.TOOLTIP,
                                                                "",
                                                                StatusItem.IconType.Info,
                                                                NotificationType.Neutral,
                                                                false,
                                                                OverlayModes.None.ID,
                                                                129022);

            this.TanningLightInsufficient = this.CreateStatusItem("TanningLightInsufficient",
                                                                  BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.NAME,
                                                                  BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.TOOLTIP,
                                                                  "",
                                                                  StatusItem.IconType.Info,
                                                                  NotificationType.Neutral,
                                                                  false,
                                                                  OverlayModes.None.ID,
                                                                  129022);

            this.HotTubWaterTooCold = this.CreateStatusItem("HotTubWaterTooCold",
                                                            "BUILDING",
                                                            "status_item_exclamation",
                                                            StatusItem.IconType.Custom,
                                                            NotificationType.BadMinor,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            false,
                                                            129022);

            this.HotTubWaterTooCold.resolveStringCallback = delegate(string str, object data) {
                                                                HotTub hotTub = (HotTub)data;
                                                                str = str.Replace("{temperature}",
                                                                 GameUtil.GetFormattedTemperature(hotTub
                                                                      .minimumWaterTemperature,
                                                                  GameUtil.TimeSlice.None,
                                                                  GameUtil.TemperatureInterpretation.Absolute,
                                                                  true,
                                                                  false));

                                                                return str;
                                                            };

            this.HotTubTooHot = this.CreateStatusItem("HotTubTooHot",
                                                      "BUILDING",
                                                      "status_item_exclamation",
                                                      StatusItem.IconType.Custom,
                                                      NotificationType.BadMinor,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      false,
                                                      129022);

            this.HotTubTooHot.resolveStringCallback = delegate(string str, object data) {
                                                          HotTub hotTub = (HotTub)data;
                                                          str = str.Replace("{temperature}",
                                                                            GameUtil.GetFormattedTemperature(hotTub
                                                                                 .maxOperatingTemperature,
                                                                             GameUtil.TimeSlice.None,
                                                                             GameUtil.TemperatureInterpretation
                                                                                 .Absolute,
                                                                             true,
                                                                             false));

                                                          return str;
                                                      };

            this.HotTubFilling = this.CreateStatusItem("HotTubFilling",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       false,
                                                       129022);

            this.HotTubFilling.resolveStringCallback = delegate(string str, object data) {
                                                           HotTub hotTub = (HotTub)data;
                                                           str = str.Replace("{fullness}",
                                                                             GameUtil.GetFormattedPercent(hotTub
                                                                                  .PercentFull,
                                                                              GameUtil.TimeSlice.None));

                                                           return str;
                                                       };

            this.WindTunnelIntake = this.CreateStatusItem("WindTunnelIntake",
                                                          "BUILDING",
                                                          "status_item_vent_disabled",
                                                          StatusItem.IconType.Custom,
                                                          NotificationType.BadMinor,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022);

            this.WarpPortalCharging = this.CreateStatusItem("WarpPortalCharging",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Info,
                                                            NotificationType.Neutral,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            false,
                                                            129022);

            this.WarpPortalCharging.resolveStringCallback = delegate(string str, object data) {
                                                                WarpPortal warpPortal = (WarpPortal)data;
                                                                str = str.Replace("{charge}",
                                                                 GameUtil.GetFormattedPercent(100f *
                                                                      (((WarpPortal)data).rechargeProgress /
                                                                       3000f),
                                                                  GameUtil.TimeSlice.None));

                                                                return str;
                                                            };

            this.WarpPortalCharging.resolveTooltipCallback = delegate(string str, object data) {
                                                                 WarpPortal warpPortal = (WarpPortal)data;
                                                                 str = str.Replace("{cycles}",
                                                                  string.Format("{0:0.0}",
                                                                                (3000f -
                                                                                        ((WarpPortal)data)
                                                                                        .rechargeProgress) /
                                                                                600f));

                                                                 return str;
                                                             };

            this.WarpConduitPartnerDisabled = this.CreateStatusItem("WarpConduitPartnerDisabled",
                                                                    "BUILDING",
                                                                    "status_item_exclamation",
                                                                    StatusItem.IconType.Custom,
                                                                    NotificationType.BadMinor,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    true,
                                                                    129022);

            this.WarpConduitPartnerDisabled.resolveStringCallback
                = ((string str, object data) => str.Replace("{x}", data.ToString()));

            this.CollectingHEP = this.CreateStatusItem("CollectingHEP",
                                                       "BUILDING",
                                                       "status_item_exclamation",
                                                       StatusItem.IconType.Custom,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.Radiation.ID,
                                                       false,
                                                       129022);

            this.CollectingHEP.resolveStringCallback = ((string str, object data) =>
                                                            str.Replace("{x}",
                                                                        ((HighEnergyParticleSpawner)data)
                                                                        .PredictedPerCycleConsumptionRate.ToString()));

            this.InOrbit = this.CreateStatusItem("InOrbit",
                                                 "BUILDING",
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022);

            this.InOrbit.resolveStringCallback = delegate(string str, object data) {
                                                     ClusterGridEntity clusterGridEntity = (ClusterGridEntity)data;
                                                     return str.Replace("{Destination}", clusterGridEntity.Name);
                                                 };

            this.WaitingToLand = this.CreateStatusItem("WaitingToLand",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       true,
                                                       129022);

            this.WaitingToLand.resolveStringCallback = delegate(string str, object data) {
                                                           ClusterGridEntity clusterGridEntity
                                                               = (ClusterGridEntity)data;

                                                           return str.Replace("{Destination}", clusterGridEntity.Name);
                                                       };

            this.InFlight = this.CreateStatusItem("InFlight",
                                                  "BUILDING",
                                                  "",
                                                  StatusItem.IconType.Info,
                                                  NotificationType.Neutral,
                                                  false,
                                                  OverlayModes.None.ID,
                                                  true,
                                                  129022);

            this.InFlight.resolveStringCallback = delegate(string str, object data) {
                                                      ClusterTraveler clusterTraveler = (ClusterTraveler)data;
                                                      ClusterDestinationSelector component
                                                          = clusterTraveler.GetComponent<ClusterDestinationSelector>();

                                                      RocketClusterDestinationSelector rocketClusterDestinationSelector
                                                          = component as RocketClusterDestinationSelector;

                                                      Sprite sprite;
                                                      string newValue;
                                                      string text;
                                                      ClusterGrid.Instance
                                                                 .GetLocationDescription(component.GetDestination(),
                                                                  out sprite,
                                                                  out newValue,
                                                                  out text);

                                                      if (rocketClusterDestinationSelector != null) {
                                                          LaunchPad destinationPad = rocketClusterDestinationSelector
                                                              .GetDestinationPad();

                                                          string newValue2
                                                              = (destinationPad != null)
                                                                    ? destinationPad.GetProperName()
                                                                    : UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN
                                                                        .FIRSTAVAILABLE.ToString();

                                                          return str.Replace("{Destination_Asteroid}", newValue)
                                                                    .Replace("{Destination_Pad}", newValue2)
                                                                    .Replace("{ETA}",
                                                                             GameUtil.GetFormattedCycles(clusterTraveler
                                                                                  .TravelETA(),
                                                                              "F1",
                                                                              false));
                                                      }

                                                      return str.Replace("{Destination_Asteroid}", newValue)
                                                                .Replace("{ETA}",
                                                                         GameUtil.GetFormattedCycles(clusterTraveler
                                                                              .TravelETA(),
                                                                          "F1",
                                                                          false));
                                                  };

            this.DestinationOutOfRange = this.CreateStatusItem("DestinationOutOfRange",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.DestinationOutOfRange.resolveStringCallback = delegate(string str, object data) {
                                                                   ClusterTraveler clusterTraveler
                                                                       = (ClusterTraveler)data;

                                                                   str = str.Replace("{Range}",
                                                                    GameUtil.GetFormattedRocketRange(clusterTraveler
                                                                         .GetComponent<CraftModuleInterface>()
                                                                         .RangeInTiles,
                                                                     false));

                                                                   return str.Replace("{Distance}",
                                                                    clusterTraveler.RemainingTravelNodes()
                                                                        .ToString() +
                                                                    " "             +
                                                                    UI.CLUSTERMAP.TILES);
                                                               };

            this.RocketStranded = this.CreateStatusItem("RocketStranded",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Info,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.MissionControlAssistingRocket = this.CreateStatusItem("MissionControlAssistingRocket",
                                                                       "BUILDING",
                                                                       "",
                                                                       StatusItem.IconType.Info,
                                                                       NotificationType.Neutral,
                                                                       false,
                                                                       OverlayModes.None.ID,
                                                                       true,
                                                                       129022);

            this.MissionControlAssistingRocket.resolveStringCallback = delegate(string str, object data) {
                                                                           Spacecraft spacecraft = data as Spacecraft;
                                                                           Clustercraft clustercraft
                                                                               = data as Clustercraft;

                                                                           return str.Replace("{0}",
                                                                            (spacecraft != null)
                                                                                ? spacecraft.rocketName
                                                                                : clustercraft.Name);
                                                                       };

            this.NoRocketsToMissionControlBoost = this.CreateStatusItem("NoRocketsToMissionControlBoost",
                                                                        "BUILDING",
                                                                        "",
                                                                        StatusItem.IconType.Info,
                                                                        NotificationType.Neutral,
                                                                        false,
                                                                        OverlayModes.None.ID,
                                                                        true,
                                                                        129022);

            this.NoRocketsToMissionControlClusterBoost = this.CreateStatusItem("NoRocketsToMissionControlClusterBoost",
                                                                               "BUILDING",
                                                                               "",
                                                                               StatusItem.IconType.Info,
                                                                               NotificationType.Neutral,
                                                                               false,
                                                                               OverlayModes.None.ID,
                                                                               true,
                                                                               129022);

            this.NoRocketsToMissionControlClusterBoost.resolveStringCallback = delegate(string str, object data) {
                                                                                   if (str.Contains("{0}")) {
                                                                                       str = str.Replace("{0}",
                                                                                        2.ToString());
                                                                                   }

                                                                                   return str;
                                                                               };

            this.MissionControlBoosted = this.CreateStatusItem("MissionControlBoosted",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.Neutral,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.MissionControlBoosted.resolveStringCallback = delegate(string str, object data) {
                                                                   Spacecraft   spacecraft   = data as Spacecraft;
                                                                   Clustercraft clustercraft = data as Clustercraft;
                                                                   str = str.Replace("{0}",
                                                                    GameUtil.GetFormattedPercent(20.000004f,
                                                                     GameUtil.TimeSlice.None));

                                                                   if (str.Contains("{1}")) {
                                                                       str = str.Replace("{1}",
                                                                        GameUtil.GetFormattedTime((spacecraft !=
                                                                                         null)
                                                                                 ? spacecraft
                                                                                     .controlStationBuffTimeRemaining
                                                                                 : clustercraft
                                                                                     .controlStationBuffTimeRemaining,
                                                                         "F0"));
                                                                   }

                                                                   return str;
                                                               };

            this.TransitTubeEntranceWaxReady = this.CreateStatusItem("TransitTubeEntranceWaxReady",
                                                                     "BUILDING",
                                                                     "",
                                                                     StatusItem.IconType.Info,
                                                                     NotificationType.Neutral,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     true,
                                                                     129022);

            this.TransitTubeEntranceWaxReady.resolveStringCallback = delegate(string str, object data) {
                                                                         TravelTubeEntrance travelTubeEntrance
                                                                             = data as TravelTubeEntrance;

                                                                         str = str.Replace("{0}",
                                                                          GameUtil
                                                                              .GetFormattedMass(travelTubeEntrance
                                                                                   .waxPerLaunch,
                                                                               GameUtil.TimeSlice.None,
                                                                               GameUtil.MetricMassFormat
                                                                                   .UseThreshold,
                                                                               true,
                                                                               "{0:0.#}"));

                                                                         str = str.Replace("{1}",
                                                                          travelTubeEntrance.WaxLaunchesAvailable
                                                                              .ToString());

                                                                         return str;
                                                                     };

            this.SpecialCargoBayClusterCritterStored = this.CreateStatusItem("SpecialCargoBayClusterCritterStored",
                                                                             "BUILDING",
                                                                             "",
                                                                             StatusItem.IconType.Info,
                                                                             NotificationType.Neutral,
                                                                             false,
                                                                             OverlayModes.None.ID,
                                                                             true,
                                                                             129022);

            this.SpecialCargoBayClusterCritterStored.resolveStringCallback = delegate(string str, object data) {
                                                                                 SpecialCargoBayClusterReceptacle
                                                                                     specialCargoBayClusterReceptacle
                                                                                         = data as
                                                                                             SpecialCargoBayClusterReceptacle;

                                                                                 if (specialCargoBayClusterReceptacle
                                                                                          .Occupant !=
                                                                                      null) {
                                                                                     str = str.Replace("{0}",
                                                                                      specialCargoBayClusterReceptacle
                                                                                          .Occupant
                                                                                          .GetProperName());
                                                                                 }

                                                                                 return str;
                                                                             };

            this.RailgunpayloadNeedsEmptying = this.CreateStatusItem("RailgunpayloadNeedsEmptying",
                                                                     "BUILDING",
                                                                     "",
                                                                     StatusItem.IconType.Info,
                                                                     NotificationType.Neutral,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     true,
                                                                     129022);

            this.AwaitingEmptyBuilding = this.CreateStatusItem("AwaitingEmptyBuilding",
                                                               "BUILDING",
                                                               "action_empty_contents",
                                                               StatusItem.IconType.Custom,
                                                               NotificationType.Neutral,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.DuplicantActivationRequired = this.CreateStatusItem("DuplicantActivationRequired",
                                                                     "BUILDING",
                                                                     "",
                                                                     StatusItem.IconType.Info,
                                                                     NotificationType.Neutral,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     true,
                                                                     129022);

            this.RocketChecklistIncomplete = this.CreateStatusItem("RocketChecklistIncomplete",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.BadMinor,
                                                                   false,
                                                                   OverlayModes.None.ID,
                                                                   true,
                                                                   129022);

            this.RocketCargoEmptying = this.CreateStatusItem("RocketCargoEmptying",
                                                             "BUILDING",
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Neutral,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             true,
                                                             129022);

            this.RocketCargoFilling = this.CreateStatusItem("RocketCargoFilling",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Info,
                                                            NotificationType.Neutral,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.RocketCargoFull = this.CreateStatusItem("RocketCargoFull",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.Neutral,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.FlightAllCargoFull = this.CreateStatusItem("FlightAllCargoFull",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Info,
                                                            NotificationType.Neutral,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.FlightCargoRemaining = this.CreateStatusItem("FlightCargoRemaining",
                                                              "BUILDING",
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              true,
                                                              129022);

            this.FlightCargoRemaining.resolveStringCallback = delegate(string str, object data) {
                                                                  float mass = (float)data;
                                                                  return str.Replace("{0}",
                                                                   GameUtil.GetFormattedMass(mass,
                                                                    GameUtil.TimeSlice.None,
                                                                    GameUtil.MetricMassFormat.UseThreshold,
                                                                    true,
                                                                    "{0:0.#}"));
                                                              };

            this.PilotNeeded = this.CreateStatusItem("PilotNeeded",
                                                     "BUILDING",
                                                     "",
                                                     StatusItem.IconType.Info,
                                                     NotificationType.Neutral,
                                                     false,
                                                     OverlayModes.None.ID,
                                                     true,
                                                     129022);

            this.PilotNeeded.resolveStringCallback = delegate(string str, object data) {
                                                         RocketControlStation master
                                                             = ((RocketControlStation.StatesInstance)data).master;

                                                         return str.Replace("{timeRemaining}",
                                                                            GameUtil.GetFormattedTime(master
                                                                                 .TimeRemaining,
                                                                             "F0"));
                                                     };

            this.AutoPilotActive = this.CreateStatusItem("AutoPilotActive",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.InvalidMaskStationConsumptionState = this.CreateStatusItem("InvalidMaskStationConsumptionState",
                                                                            "BUILDING",
                                                                            "status_item_no_gas_to_pump",
                                                                            StatusItem.IconType.Custom,
                                                                            NotificationType.BadMinor,
                                                                            false,
                                                                            OverlayModes.None.ID,
                                                                            true,
                                                                            129022);

            this.ClusterTelescopeAllWorkComplete = this.CreateStatusItem("ClusterTelescopeAllWorkComplete",
                                                                         "BUILDING",
                                                                         "",
                                                                         StatusItem.IconType.Info,
                                                                         NotificationType.Neutral,
                                                                         false,
                                                                         OverlayModes.None.ID,
                                                                         true,
                                                                         129022);

            this.RocketPlatformCloseToCeiling = this.CreateStatusItem("RocketPlatformCloseToCeiling",
                                                                      "BUILDING",
                                                                      "",
                                                                      StatusItem.IconType.Info,
                                                                      NotificationType.BadMinor,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.RocketPlatformCloseToCeiling.resolveStringCallback
                = ((string str, object data) => str.Replace("{distance}", data.ToString()));

            this.ModuleGeneratorPowered = this.CreateStatusItem("ModuleGeneratorPowered",
                                                                "BUILDING",
                                                                "",
                                                                StatusItem.IconType.Info,
                                                                NotificationType.Neutral,
                                                                false,
                                                                OverlayModes.Power.ID,
                                                                true,
                                                                129022);

            this.ModuleGeneratorPowered.resolveStringCallback = delegate(string str, object data) {
                                                                    Generator generator = (Generator)data;
                                                                    str = str.Replace("{ActiveWattage}",
                                                                     GameUtil.GetFormattedWattage(generator
                                                                          .WattageRating,
                                                                      GameUtil.WattageFormatterUnit.Automatic,
                                                                      true));

                                                                    str = str.Replace("{MaxWattage}",
                                                                     GameUtil.GetFormattedWattage(generator
                                                                          .WattageRating,
                                                                      GameUtil.WattageFormatterUnit.Automatic,
                                                                      true));

                                                                    return str;
                                                                };

            this.ModuleGeneratorNotPowered = this.CreateStatusItem("ModuleGeneratorNotPowered",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.Power.ID,
                                                                   true,
                                                                   129022);

            this.ModuleGeneratorNotPowered.resolveStringCallback = delegate(string str, object data) {
                                                                       Generator generator = (Generator)data;
                                                                       str = str.Replace("{ActiveWattage}",
                                                                        GameUtil.GetFormattedWattage(0f,
                                                                         GameUtil.WattageFormatterUnit
                                                                             .Automatic,
                                                                         true));

                                                                       str = str.Replace("{MaxWattage}",
                                                                        GameUtil.GetFormattedWattage(generator
                                                                             .WattageRating,
                                                                         GameUtil.WattageFormatterUnit
                                                                             .Automatic,
                                                                         true));

                                                                       return str;
                                                                   };

            this.InOrbitRequired = this.CreateStatusItem("InOrbitRequired",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.ReactorRefuelDisabled = this.CreateStatusItem("ReactorRefuelDisabled",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.Neutral,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               true,
                                                               129022);

            this.FridgeCooling = this.CreateStatusItem("FridgeCooling",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       true,
                                                       129022);

            this.FridgeCooling.resolveStringCallback = delegate(string str, object data) {
                                                           RefrigeratorController.StatesInstance statesInstance
                                                               = (RefrigeratorController.StatesInstance)data;

                                                           str = str
                                                                 .Replace("{UsedPower}",
                                                                          GameUtil.GetFormattedWattage(statesInstance
                                                                               .GetNormalPower(),
                                                                           GameUtil.WattageFormatterUnit.Automatic,
                                                                           true))
                                                                 .Replace("{MaxPower}",
                                                                          GameUtil.GetFormattedWattage(statesInstance
                                                                               .GetNormalPower(),
                                                                           GameUtil.WattageFormatterUnit.Automatic,
                                                                           true));

                                                           return str;
                                                       };

            this.FridgeSteady = this.CreateStatusItem("FridgeSteady",
                                                      "BUILDING",
                                                      "",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.Neutral,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022);

            this.FridgeSteady.resolveStringCallback = delegate(string str, object data) {
                                                          RefrigeratorController.StatesInstance statesInstance
                                                              = (RefrigeratorController.StatesInstance)data;

                                                          str = str
                                                                .Replace("{UsedPower}",
                                                                         GameUtil.GetFormattedWattage(statesInstance
                                                                              .GetSaverPower(),
                                                                          GameUtil.WattageFormatterUnit.Automatic,
                                                                          true))
                                                                .Replace("{MaxPower}",
                                                                         GameUtil.GetFormattedWattage(statesInstance
                                                                              .GetNormalPower(),
                                                                          GameUtil.WattageFormatterUnit.Automatic,
                                                                          true));

                                                          return str;
                                                      };

            this.TrapNeedsArming = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.NEEDS_ARMING",
                                                         "BUILDING",
                                                         "status_item_bait",
                                                         StatusItem.IconType.Custom,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.TrapArmed = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.READY",
                                                   "BUILDING",
                                                   "",
                                                   StatusItem.IconType.Info,
                                                   NotificationType.Neutral,
                                                   false,
                                                   OverlayModes.None.ID,
                                                   true,
                                                   129022);

            this.TrapHasCritter = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.SPRUNG",
                                                        "BUILDING",
                                                        "",
                                                        StatusItem.IconType.Info,
                                                        NotificationType.Neutral,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022);

            this.TrapHasCritter.resolveTooltipCallback = delegate(string str, object data) {
                                                             string newValue = "";
                                                             if (data != null) {
                                                                 newValue = ((GameObject)data).GetComponent<KPrefabID>()
                                                                     .GetProperName();
                                                             }

                                                             str = str.Replace("{0}", newValue);
                                                             return str;
                                                         };

            this.RailGunCooldown = this.CreateStatusItem("RailGunCooldown",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.Neutral,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.RailGunCooldown.resolveStringCallback = delegate(string str, object data) {
                                                             RailGun.StatesInstance statesInstance
                                                                 = (RailGun.StatesInstance)data;

                                                             str = str.Replace("{timeleft}",
                                                                               GameUtil.GetFormattedTime(statesInstance
                                                                                    .sm.cooldownTimer
                                                                                    .Get(statesInstance),
                                                                                "F0"));

                                                             return str;
                                                         };

            this.RailGunCooldown.resolveTooltipCallback = delegate(string str, object data) {
                                                              RailGun.StatesInstance statesInstance
                                                                  = (RailGun.StatesInstance)data;

                                                              str = str.Replace("{x}", 6.ToString());
                                                              return str;
                                                          };

            this.NoSurfaceSight = new StatusItem("NOSURFACESIGHT",
                                                 "BUILDING",
                                                 "status_item_no_sky",
                                                 StatusItem.IconType.Custom,
                                                 NotificationType.BadMinor,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022,
                                                 null);

            this.LimitValveLimitReached = this.CreateStatusItem("LimitValveLimitReached",
                                                                "BUILDING",
                                                                "",
                                                                StatusItem.IconType.Info,
                                                                NotificationType.Neutral,
                                                                false,
                                                                OverlayModes.None.ID,
                                                                true,
                                                                129022);

            this.LimitValveLimitNotReached = this.CreateStatusItem("LimitValveLimitNotReached",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.None.ID,
                                                                   true,
                                                                   129022);

            this.LimitValveLimitNotReached.resolveStringCallback = delegate(string str, object data) {
                                                                       LimitValve limitValve = (LimitValve)data;
                                                                       string     arg;
                                                                       if (limitValve.displayUnitsInsteadOfMass) {
                                                                           arg = GameUtil
                                                                               .GetFormattedUnits(limitValve
                                                                                    .RemainingCapacity,
                                                                                GameUtil.TimeSlice.None,
                                                                                true,
                                                                                LimitValveSideScreen.FLOAT_FORMAT);
                                                                       } else {
                                                                           arg
                                                                               = GameUtil.GetFormattedMass(limitValve
                                                                                    .RemainingCapacity,
                                                                                GameUtil.TimeSlice.None,
                                                                                GameUtil.MetricMassFormat.Kilogram,
                                                                                true,
                                                                                LimitValveSideScreen.FLOAT_FORMAT);
                                                                       }

                                                                       return string.Format(BUILDING.STATUSITEMS
                                                                            .LIMITVALVELIMITNOTREACHED.NAME,
                                                                        arg);
                                                                   };

            this.LimitValveLimitNotReached.resolveTooltipCallback
                = ((string str, object data) => BUILDING.STATUSITEMS.LIMITVALVELIMITNOTREACHED.TOOLTIP);

            this.SpacePOIHarvesting = this.CreateStatusItem("SpacePOIHarvesting",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Info,
                                                            NotificationType.Neutral,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022);

            this.SpacePOIHarvesting.resolveStringCallback = delegate(string str, object data) {
                                                                float mass = (float)data;
                                                                return string.Format(BUILDING.STATUSITEMS
                                                                     .SPACEPOIHARVESTING.NAME,
                                                                 GameUtil.GetFormattedMass(mass,
                                                                  GameUtil.TimeSlice.PerSecond,
                                                                  GameUtil.MetricMassFormat.UseThreshold,
                                                                  true,
                                                                  "{0:0.#}"));
                                                            };

            this.SpacePOIWasting = this.CreateStatusItem("SpacePOIWasting",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         true,
                                                         129022);

            this.SpacePOIWasting.resolveStringCallback = delegate(string str, object data) {
                                                             float mass = (float)data;
                                                             return string.Format(BUILDING.STATUSITEMS.SPACEPOIWASTING
                                                                  .NAME,
                                                              GameUtil.GetFormattedMass(mass,
                                                               GameUtil.TimeSlice.PerSecond,
                                                               GameUtil.MetricMassFormat.UseThreshold,
                                                               true,
                                                               "{0:0.#}"));
                                                         };

            this.RocketRestrictionActive = new StatusItem("ROCKETRESTRICTIONACTIVE",
                                                          "BUILDING",
                                                          "status_item_rocket_restricted",
                                                          StatusItem.IconType.Custom,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022,
                                                          null);

            this.RocketRestrictionInactive = new StatusItem("ROCKETRESTRICTIONINACTIVE",
                                                            "BUILDING",
                                                            "",
                                                            StatusItem.IconType.Info,
                                                            NotificationType.Neutral,
                                                            false,
                                                            OverlayModes.None.ID,
                                                            true,
                                                            129022,
                                                            null);

            this.NoRocketRestriction = new StatusItem("NOROCKETRESTRICTION",
                                                      "BUILDING",
                                                      "",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.Neutral,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      true,
                                                      129022,
                                                      null);

            this.BroadcasterOutOfRange = new StatusItem("BROADCASTEROUTOFRANGE",
                                                        "BUILDING",
                                                        "status_item_exclamation",
                                                        StatusItem.IconType.Custom,
                                                        NotificationType.BadMinor,
                                                        false,
                                                        OverlayModes.None.ID,
                                                        true,
                                                        129022,
                                                        null);

            this.LosingRadbolts = new StatusItem("LOSINGRADBOLTS",
                                                 "BUILDING",
                                                 "status_item_exclamation",
                                                 StatusItem.IconType.Custom,
                                                 NotificationType.BadMinor,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 true,
                                                 129022,
                                                 null);

            this.FabricatorAcceptsMutantSeeds = new StatusItem("FABRICATORACCEPTSMUTANTSEEDS",
                                                               "BUILDING",
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.Neutral,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               false,
                                                               129022,
                                                               null);

            this.NoSpiceSelected = new StatusItem("SPICEGRINDERNOSPICE",
                                                  "BUILDING",
                                                  "status_item_no_filter_set",
                                                  StatusItem.IconType.Custom,
                                                  NotificationType.BadMinor,
                                                  false,
                                                  OverlayModes.None.ID,
                                                  true,
                                                  129022,
                                                  null);

            this.GeoTunerNoGeyserSelected = new StatusItem("GEOTUNER_NEEDGEYSER",
                                                           "BUILDING",
                                                           "status_item_fabricator_select",
                                                           StatusItem.IconType.Custom,
                                                           NotificationType.BadMinor,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           true,
                                                           129022,
                                                           null);

            this.GeoTunerResearchNeeded = new StatusItem("GEOTUNER_CHARGE_REQUIRED",
                                                         "BUILDING",
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.BadMinor,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         false,
                                                         129022,
                                                         null);

            this.GeoTunerResearchInProgress = new StatusItem("GEOTUNER_CHARGING",
                                                             "BUILDING",
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Neutral,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             false,
                                                             129022,
                                                             null);

            this.GeoTunerBroadcasting = new StatusItem("GEOTUNER_CHARGED",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       false,
                                                       129022,
                                                       null);

            this.GeoTunerBroadcasting.resolveStringCallback = delegate(string str, object data) {
                                                                  GeoTuner.Instance instance = (GeoTuner.Instance)data;
                                                                  str = str.Replace("{0}",
                                                                   ((float)Mathf.CeilToInt(instance.sm
                                                                               .expirationTimer.Get(instance) /
                                                                           instance.enhancementDuration *
                                                                           100f)).ToString() +
                                                                   "%");

                                                                  return str;
                                                              };

            this.GeoTunerBroadcasting.resolveTooltipCallback = delegate(string str, object data) {
                                                                   GeoTuner.Instance instance = (GeoTuner.Instance)data;
                                                                   float seconds
                                                                       = instance.sm.expirationTimer.Get(instance);

                                                                   float num = 100f / instance.enhancementDuration;
                                                                   str = str.Replace("{0}",
                                                                    GameUtil.GetFormattedTime(seconds, "F0"));

                                                                   str = str.Replace("{1}",
                                                                    "-" + num.ToString("0.00") + "%");

                                                                   return str;
                                                               };

            this.GeoTunerGeyserStatus = new StatusItem("GEOTUNER_GEYSER_STATUS",
                                                       "BUILDING",
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       false,
                                                       129022,
                                                       null);

            this.GeoTunerGeyserStatus.resolveStringCallback = delegate(string str, object data) {
                                                                  Geyser assignedGeyser = ((GeoTuner.Instance)data)
                                                                      .GetAssignedGeyser();

                                                                  bool flag = assignedGeyser != null &&
                                                                              assignedGeyser.smi.GetCurrentState() !=
                                                                              null &&
                                                                              assignedGeyser.smi.GetCurrentState()
                                                                                  .parent ==
                                                                              assignedGeyser.smi.sm.erupt;

                                                                  bool flag2 = assignedGeyser != null &&
                                                                               assignedGeyser.smi.GetCurrentState() ==
                                                                               assignedGeyser.smi.sm.dormant;

                                                                  if (!flag2) {
                                                                      bool flag3 = !flag;
                                                                  }

                                                                  return flag
                                                                             ? BUILDING.STATUSITEMS
                                                                                 .GEOTUNER_GEYSER_STATUS.NAME_ERUPTING
                                                                             : (flag2
                                                                                       ? BUILDING.STATUSITEMS
                                                                                           .GEOTUNER_GEYSER_STATUS
                                                                                           .NAME_DORMANT
                                                                                       : BUILDING.STATUSITEMS
                                                                                           .GEOTUNER_GEYSER_STATUS
                                                                                           .NAME_IDLE);
                                                              };

            this.GeoTunerGeyserStatus.resolveTooltipCallback = delegate(string str, object data) {
                                                                   Geyser assignedGeyser = ((GeoTuner.Instance)data)
                                                                       .GetAssignedGeyser();

                                                                   if (assignedGeyser != null) {
                                                                       assignedGeyser.gameObject.GetProperName();
                                                                   }

                                                                   bool flag = assignedGeyser != null &&
                                                                               assignedGeyser.smi.GetCurrentState() !=
                                                                               null &&
                                                                               assignedGeyser.smi.GetCurrentState()
                                                                                   .parent ==
                                                                               assignedGeyser.smi.sm.erupt;

                                                                   bool flag2 = assignedGeyser != null &&
                                                                       assignedGeyser.smi.GetCurrentState() ==
                                                                       assignedGeyser.smi.sm.dormant;

                                                                   if (!flag2) {
                                                                       bool flag3 = !flag;
                                                                   }

                                                                   return flag
                                                                              ? BUILDING.STATUSITEMS
                                                                                  .GEOTUNER_GEYSER_STATUS
                                                                                  .TOOLTIP_ERUPTING
                                                                              : (flag2
                                                                                          ? BUILDING.STATUSITEMS
                                                                                              .GEOTUNER_GEYSER_STATUS
                                                                                              .TOOLTIP_DORMANT
                                                                                          : BUILDING.STATUSITEMS
                                                                                              .GEOTUNER_GEYSER_STATUS
                                                                                              .TOOLTIP_IDLE);
                                                               };

            this.GeyserGeotuned = new StatusItem("GEYSER_GEOTUNED",
                                                 "BUILDING",
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.None.ID,
                                                 false,
                                                 129022,
                                                 null);

            this.GeyserGeotuned.resolveStringCallback = delegate(string str, object data) {
                                                            Geyser geyser = (Geyser)data;
                                                            int    num    = 0;
                                                            int num2 = Components.GeoTuners
                                                                .GetItems(geyser.GetMyWorldId())
                                                                .Count((GeoTuner.Instance x) =>
                                                                           x.GetAssignedGeyser() == geyser);

                                                            for (int i = 0; i < geyser.modifications.Count; i++) {
                                                                if (geyser.modifications[i]
                                                                          .originID.Contains("GeoTuner")) { num++; }
                                                            }

                                                            str = str.Replace("{0}", num.ToString());
                                                            str = str.Replace("{1}", num2.ToString());
                                                            return str;
                                                        };

            this.GeyserGeotuned.resolveTooltipCallback = delegate(string str, object data) {
                                                             Geyser geyser = (Geyser)data;
                                                             int    num    = 0;
                                                             int num2 = Components.GeoTuners
                                                                 .GetItems(geyser.GetMyWorldId())
                                                                 .Count((GeoTuner.Instance x) =>
                                                                            x.GetAssignedGeyser() == geyser);

                                                             for (int i = 0; i < geyser.modifications.Count; i++) {
                                                                 if (geyser.modifications[i]
                                                                           .originID.Contains("GeoTuner")) { num++; }
                                                             }

                                                             str = str.Replace("{0}", num.ToString());
                                                             str = str.Replace("{1}", num2.ToString());
                                                             return str;
                                                         };

            this.SkyVisNone = new StatusItem("SkyVisNone",
                                             BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.NAME,
                                             BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.TOOLTIP,
                                             "status_item_no_sky",
                                             StatusItem.IconType.Custom,
                                             NotificationType.Bad,
                                             false,
                                             OverlayModes.None.ID,
                                             129022,
                                             true,
                                             new Func<string, object, string>(BuildingStatusItems. <
                                             CreateStatusItems >
                                             g__SkyVisResolveStringCallback |
                                             316_107));
            this.SkyVisLimited = new StatusItem("SkyVisLimited",
                                                BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.NAME,
                                                BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.TOOLTIP,
                                                "status_item_no_sky",
                                                StatusItem.IconType.Info,
                                                NotificationType.BadMinor,
                                                false,
                                                OverlayModes.None.ID,
                                                129022,
                                                false,
                                                new Func<string, object, string>(BuildingStatusItems. <
                                                CreateStatusItems >
                                                g__SkyVisResolveStringCallback |
                                                316_107));
            this.CreatureManipulatorWaiting = this.CreateStatusItem("CreatureManipulatorWaiting",
                                                                    "BUILDING",
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    true,
                                                                    129022);

            this.CreatureManipulatorProgress = this.CreateStatusItem("CreatureManipulatorProgress",
                                                                     "BUILDING",
                                                                     "",
                                                                     StatusItem.IconType.Info,
                                                                     NotificationType.Neutral,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     true,
                                                                     129022);

            this.CreatureManipulatorProgress.resolveStringCallback = delegate(string str, object data) {
                                                                         GravitasCreatureManipulator.Instance instance
                                                                             = (GravitasCreatureManipulator.Instance)
                                                                             data;

                                                                         return string.Format(str,
                                                                          instance.ScannedSpecies.Count,
                                                                          instance.def.numSpeciesToUnlockMorphMode);
                                                                     };

            this.CreatureManipulatorProgress.resolveTooltipCallback = delegate(string str, object data) {
                                                                          GravitasCreatureManipulator.Instance instance
                                                                              = (GravitasCreatureManipulator.Instance)
                                                                              data;

                                                                          if (instance.ScannedSpecies.Count == 0) {
                                                                              str = str   +
                                                                                  "\n • " +
                                                                                  BUILDING.STATUSITEMS
                                                                                      .CREATUREMANIPULATORPROGRESS
                                                                                      .NO_DATA;
                                                                          } else {
                                                                              foreach (Tag tag in instance
                                                                                  .ScannedSpecies) {
                                                                                  str = str   +
                                                                                      "\n • " +
                                                                                      Strings
                                                                                          .Get("STRINGS.CREATURES.FAMILY_PLURAL." +
                                                                                              tag.ToString()
                                                                                                  .ToUpper());
                                                                              }
                                                                          }

                                                                          return str;
                                                                      };

            this.CreatureManipulatorMorphModeLocked = this.CreateStatusItem("CreatureManipulatorMorphModeLocked",
                                                                            "BUILDING",
                                                                            "",
                                                                            StatusItem.IconType.Info,
                                                                            NotificationType.Neutral,
                                                                            false,
                                                                            OverlayModes.None.ID,
                                                                            true,
                                                                            129022);

            this.CreatureManipulatorMorphMode = this.CreateStatusItem("CreatureManipulatorMorphMode",
                                                                      "BUILDING",
                                                                      "",
                                                                      StatusItem.IconType.Info,
                                                                      NotificationType.Neutral,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.CreatureManipulatorWorking = this.CreateStatusItem("CreatureManipulatorWorking",
                                                                    "BUILDING",
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    true,
                                                                    129022);

            this.MegaBrainTankActivationProgress = this.CreateStatusItem("MegaBrainTankActivationProgress",
                                                                         BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS
                                                                             .PROGRESSIONRATE.NAME,
                                                                         BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS
                                                                             .PROGRESSIONRATE.TOOLTIP,
                                                                         "",
                                                                         StatusItem.IconType.Info,
                                                                         NotificationType.Neutral,
                                                                         false,
                                                                         OverlayModes.None.ID,
                                                                         129022);

            this.MegaBrainNotEnoughOxygen = this.CreateStatusItem("MegaBrainNotEnoughOxygen",
                                                                  "BUILDING",
                                                                  "status_item_suit_locker_no_oxygen",
                                                                  StatusItem.IconType.Custom,
                                                                  NotificationType.BadMinor,
                                                                  false,
                                                                  OverlayModes.None.ID,
                                                                  true,
                                                                  129022);

            this.MegaBrainTankActivationProgress.resolveStringCallback = delegate(string str, object data) {
                                                                             MegaBrainTank.StatesInstance statesInstance
                                                                                 = (MegaBrainTank.StatesInstance)data;

                                                                             return str.Replace("{ActivationProgress}",
                                                                              string.Format("{0}/{1}",
                                                                               statesInstance
                                                                                   .ActivationProgress,
                                                                               25));
                                                                         };

            this.MegaBrainTankDreamAnalysis = this.CreateStatusItem("MegaBrainTankDreamAnalysis",
                                                                    BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS
                                                                            .DREAMANALYSIS.NAME,
                                                                    BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS
                                                                            .DREAMANALYSIS.TOOLTIP,
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    129022);

            this.MegaBrainTankDreamAnalysis.resolveStringCallback = delegate(string str, object data) {
                                                                        MegaBrainTank.StatesInstance statesInstance
                                                                            = (MegaBrainTank.StatesInstance)data;

                                                                        return str.Replace("{TimeToComplete}",
                                                                         statesInstance.TimeTilDigested.ToString());
                                                                    };

            this.MegaBrainTankComplete = this.CreateStatusItem("MegaBrainTankComplete",
                                                               BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.NAME,
                                                               BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.TOOLTIP,
                                                               "",
                                                               StatusItem.IconType.Info,
                                                               NotificationType.Good,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               129022);

            this.FossilHuntExcavationOrdered = this.CreateStatusItem("FossilHuntExcavationOrdered",
                                                                     BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION
                                                                             .NAME,
                                                                     BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION
                                                                             .TOOLTIP,
                                                                     "",
                                                                     StatusItem.IconType.Info,
                                                                     NotificationType.Good,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     129022);

            this.FossilHuntExcavationInProgress = this.CreateStatusItem("FossilHuntExcavationInProgress",
                                                                        BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING.NAME,
                                                                        BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING
                                                                            .TOOLTIP,
                                                                        "",
                                                                        StatusItem.IconType.Info,
                                                                        NotificationType.Good,
                                                                        false,
                                                                        OverlayModes.None.ID,
                                                                        129022);

            this.ComplexFabricatorCooking = this.CreateStatusItem("COMPLEXFABRICATOR.COOKING",
                                                                  "BUILDING",
                                                                  "",
                                                                  StatusItem.IconType.Info,
                                                                  NotificationType.Neutral,
                                                                  false,
                                                                  OverlayModes.None.ID,
                                                                  true,
                                                                  129022);

            this.ComplexFabricatorCooking.resolveStringCallback = delegate(string str, object data) {
                                                                      ComplexFabricator complexFabricator
                                                                          = data as ComplexFabricator;

                                                                      if (complexFabricator != null &&
                                                                          complexFabricator.CurrentWorkingOrder !=
                                                                          null) {
                                                                          str = str.Replace("{Item}",
                                                                           complexFabricator.CurrentWorkingOrder
                                                                               .FirstResult.ProperName());
                                                                      }

                                                                      return str;
                                                                  };

            this.ComplexFabricatorProducing = this.CreateStatusItem("COMPLEXFABRICATOR.PRODUCING",
                                                                    "BUILDING",
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    true,
                                                                    129022);

            this.ComplexFabricatorProducing.resolveStringCallback = delegate(string str, object data) {
                                                                        ComplexFabricator complexFabricator
                                                                            = data as ComplexFabricator;

                                                                        if (complexFabricator != null) {
                                                                            if (complexFabricator.CurrentWorkingOrder !=
                                                                                    null) {
                                                                                string newValue
                                                                                    = complexFabricator
                                                                                        .CurrentWorkingOrder
                                                                                        .results[0]
                                                                                        .facadeID
                                                                                        .IsNullOrWhiteSpace()
                                                                                        ? complexFabricator
                                                                                            .CurrentWorkingOrder
                                                                                            .FirstResult
                                                                                            .ProperName()
                                                                                        : complexFabricator
                                                                                            .CurrentWorkingOrder
                                                                                            .results[0]
                                                                                            .facadeID.ProperName();

                                                                                str = str.Replace("{Item}", newValue);
                                                                            }

                                                                            return str;
                                                                        }

                                                                        TinkerStation tinkerStation
                                                                            = data as TinkerStation;

                                                                        if (tinkerStation != null) {
                                                                            str = str.Replace("{Item}",
                                                                             tinkerStation.outputPrefab
                                                                                 .ProperName());
                                                                        }

                                                                        return str;
                                                                    };

            this.ComplexFabricatorResearching = this.CreateStatusItem("COMPLEXFABRICATOR.RESEARCHING",
                                                                      "BUILDING",
                                                                      "",
                                                                      StatusItem.IconType.Info,
                                                                      NotificationType.Neutral,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      true,
                                                                      129022);

            this.ComplexFabricatorResearching.resolveStringCallback = delegate(string str, object data) {
                                                                          if (data is IResearchCenter) {
                                                                              TechInstance activeResearch
                                                                                  = Research.Instance
                                                                                      .GetActiveResearch();

                                                                              if (activeResearch != null) {
                                                                                  str = str.Replace("{Item}",
                                                                                   activeResearch.tech.Name);

                                                                                  return str;
                                                                              }
                                                                          }

                                                                          str = str.Replace("{Item}",
                                                                           (data as GameObject).GetProperName());

                                                                          return str;
                                                                      };

            this.ArtifactAnalysisAnalyzing = this.CreateStatusItem("COMPLEXFABRICATOR.ANALYZING",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.None.ID,
                                                                   true,
                                                                   129022);

            this.ArtifactAnalysisAnalyzing.resolveStringCallback = delegate(string str, object data) {
                                                                       if (data as GameObject != null) {
                                                                           str = str.Replace("{Item}",
                                                                            (data as GameObject).GetProperName());
                                                                       }

                                                                       return str;
                                                                   };

            this.ComplexFabricatorTraining = this.CreateStatusItem("COMPLEXFABRICATOR.UNTRAINING",
                                                                   "BUILDING",
                                                                   "",
                                                                   StatusItem.IconType.Info,
                                                                   NotificationType.Neutral,
                                                                   false,
                                                                   OverlayModes.None.ID,
                                                                   true,
                                                                   129022);

            this.ComplexFabricatorTraining.resolveStringCallback = delegate(string str, object data) {
                                                                       ResetSkillsStation resetSkillsStation
                                                                           = data as ResetSkillsStation;

                                                                       if (resetSkillsStation != null &&
                                                                           resetSkillsStation.assignable.assignee !=
                                                                           null) {
                                                                           str = str.Replace("{Duplicant}",
                                                                            resetSkillsStation.assignable.assignee
                                                                                .GetProperName());
                                                                       }

                                                                       return str;
                                                                   };

            this.TelescopeWorking = this.CreateStatusItem("COMPLEXFABRICATOR.TELESCOPE",
                                                          "BUILDING",
                                                          "",
                                                          StatusItem.IconType.Info,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.None.ID,
                                                          true,
                                                          129022);

            this.ClusterTelescopeMeteorWorking = this.CreateStatusItem("COMPLEXFABRICATOR.CLUSTERTELESCOPEMETEOR",
                                                                       "BUILDING",
                                                                       "",
                                                                       StatusItem.IconType.Info,
                                                                       NotificationType.Neutral,
                                                                       false,
                                                                       OverlayModes.None.ID,
                                                                       true,
                                                                       129022);

            this.MorbRoverMakerDusty = this.CreateStatusItem("MorbRoverMakerDusty",
                                                             CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DUSTY.NAME,
                                                             CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DUSTY
                                                                  .TOOLTIP,
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.BadMinor,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             129022);

            this.MorbRoverMakerBuildingRevealed = this.CreateStatusItem("MorbRoverMakerBuildingRevealed",
                                                                        CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                             .BUILDING_BEING_REVEALED.NAME,
                                                                        CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                             .BUILDING_BEING_REVEALED.TOOLTIP,
                                                                        "",
                                                                        StatusItem.IconType.Info,
                                                                        NotificationType.Neutral,
                                                                        false,
                                                                        OverlayModes.None.ID,
                                                                        129022);

            this.MorbRoverMakerGermCollectionProgress = this.CreateStatusItem("MorbRoverMakerGermCollectionProgress",
                                                                              CODEX.STORY_TRAITS.MORB_ROVER_MAKER
                                                                                  .STATUSITEMS.GERM_COLLECTION_PROGRESS
                                                                                  .NAME,
                                                                              CODEX.STORY_TRAITS.MORB_ROVER_MAKER
                                                                                  .STATUSITEMS.GERM_COLLECTION_PROGRESS
                                                                                  .TOOLTIP,
                                                                              "",
                                                                              StatusItem.IconType.Info,
                                                                              NotificationType.Neutral,
                                                                              false,
                                                                              OverlayModes.None.ID,
                                                                              129022);

            this.MorbRoverMakerGermCollectionProgress.resolveStringCallback = delegate(string str, object data) {
                                                                                  MorbRoverMaker.Instance instance
                                                                                      = (MorbRoverMaker.Instance)data;

                                                                                  return str.Replace("{0}",
                                                                                   GameUtil
                                                                                       .GetFormattedPercent(instance
                                                                                                .MorbDevelopment_Progress *
                                                                                            100f,
                                                                                        GameUtil.TimeSlice
                                                                                            .None));
                                                                              };

            this.MorbRoverMakerGermCollectionProgress.resolveTooltipCallback = delegate(string str, object data) {
                                                                                   MorbRoverMaker.Instance instance
                                                                                       = (MorbRoverMaker.Instance)data;

                                                                                   return str
                                                                                       .Replace("{GERM_NAME}",
                                                                                        Db.Get()
                                                                                            .Diseases
                                                                                                [instance.def
                                                                                                    .GERM_TYPE]
                                                                                            .Name)
                                                                                       .Replace("{0}",
                                                                                        GameUtil
                                                                                            .GetFormattedDiseaseAmount(instance
                                                                                                 .def
                                                                                                 .MAX_GERMS_TAKEN_PER_PACKAGE,
                                                                                             GameUtil.TimeSlice
                                                                                                 .PerSecond))
                                                                                       .Replace("{1}",
                                                                                        GameUtil
                                                                                            .GetFormattedDiseaseAmount(instance
                                                                                                 .MorbDevelopment_GermsCollected,
                                                                                             GameUtil.TimeSlice
                                                                                                 .None))
                                                                                       .Replace("{2}",
                                                                                        GameUtil
                                                                                            .GetFormattedDiseaseAmount(instance
                                                                                                 .def
                                                                                                 .GERMS_PER_ROVER,
                                                                                             GameUtil.TimeSlice
                                                                                                 .None));
                                                                               };

            this.MorbRoverMakerNoGermsConsumedAlert = this.CreateStatusItem("MorbRoverMakerNoGermsConsumedAlert",
                                                                            CODEX.STORY_TRAITS.MORB_ROVER_MAKER
                                                                                .STATUSITEMS.NOGERMSCONSUMEDALERT.NAME,
                                                                            CODEX.STORY_TRAITS.MORB_ROVER_MAKER
                                                                                .STATUSITEMS.NOGERMSCONSUMEDALERT
                                                                                .TOOLTIP,
                                                                            "status_item_no_germs",
                                                                            StatusItem.IconType.Custom,
                                                                            NotificationType.Bad,
                                                                            false,
                                                                            OverlayModes.None.ID,
                                                                            129022);

            this.MorbRoverMakerNoGermsConsumedAlert.resolveStringCallback = delegate(string str, object data) {
                                                                                MorbRoverMaker.Instance instance
                                                                                    = (MorbRoverMaker.Instance)data;

                                                                                return str.Replace("{0}",
                                                                                 Db.Get()
                                                                                     .Diseases[instance.def
                                                                                         .GERM_TYPE].Name);
                                                                            };

            this.MorbRoverMakerNoGermsConsumedAlert.resolveTooltipCallback = delegate(string str, object data) {
                                                                                 MorbRoverMaker.Instance instance
                                                                                     = (MorbRoverMaker.Instance)data;

                                                                                 return str.Replace("{0}",
                                                                                  Db.Get()
                                                                                      .Diseases[instance.def
                                                                                          .GERM_TYPE].Name);
                                                                             };

            this.MorbRoverMakerCraftingBody = this.CreateStatusItem("MorbRoverMakerCraftingBody",
                                                                    CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                         .CRAFTING_ROBOT_BODY.NAME,
                                                                    CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                         .CRAFTING_ROBOT_BODY.TOOLTIP,
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    129022);

            this.MorbRoverMakerReadyForDoctor = this.CreateStatusItem("MorbRoverMakerReadyForDoctor",
                                                                      CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                           .DOCTOR_READY.NAME,
                                                                      CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                           .DOCTOR_READY.TOOLTIP,
                                                                      "",
                                                                      StatusItem.IconType.Info,
                                                                      NotificationType.Neutral,
                                                                      false,
                                                                      OverlayModes.None.ID,
                                                                      129022);

            this.MorbRoverMakerDoctorWorking = this.CreateStatusItem("MorbRoverMakerDoctorWorking",
                                                                     CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                          .BUILDING_BEING_WORKED_BY_DOCTOR.NAME,
                                                                     CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS
                                                                          .BUILDING_BEING_WORKED_BY_DOCTOR.TOOLTIP,
                                                                     "",
                                                                     StatusItem.IconType.Info,
                                                                     NotificationType.Neutral,
                                                                     false,
                                                                     OverlayModes.None.ID,
                                                                     129022);

            this.GeoVentQuestBlockage = this.CreateStatusItem("GeoVentQuestBlockage",
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .VENT.QUEST_BLOCKED_NAME,
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .VENT.QUEST_BLOCKED_TOOLTIP,
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              129022);

            this.GeoVentQuestBlockage.resolveStringCallback = ((string str, object obj) =>
                                                                   str.Replace("{Name}",
                                                                               (obj as GeothermalVent)
                                                                               .GetProperName()));

            this.GeoVentQuestBlockage.resolveStringCallback_shouldStillCallIfDataIsNull = false;
            this.GeoVentsDisconnected = this.CreateStatusItem("GeoVentsDisconnected",
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .VENT.DISCONNECTED_NAME,
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .VENT.DISCONNECTED_TOOLTIP,
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              129022);

            this.GeoVentsDisconnected.resolveStringCallback = ((string str, object obj) =>
                                                                   str.Replace("{Name}",
                                                                               (obj as GeothermalVent)
                                                                               .GetProperName()));

            this.GeoVentsDisconnected.resolveStringCallback_shouldStillCallIfDataIsNull = false;
            this.GeoVentsOverpressure = this.CreateStatusItem("GeoVentsOverpressure",
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .VENT.OVERPRESSURE_NAME,
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .VENT.OVERPRESSURE_TOOLTIP,
                                                              "status_item_vent_disabled",
                                                              StatusItem.IconType.Custom,
                                                              NotificationType.BadMinor,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              129022);

            this.GeoVentsOverpressure.resolveStringCallback = ((string str, object obj) =>
                                                                   str.Replace("{Name}",
                                                                               (obj as GeothermalVent)
                                                                               .GetProperName()));

            this.GeoVentsOverpressure.resolveStringCallback_shouldStillCallIfDataIsNull = false;
            this.GeoControllerCantVent = this.CreateStatusItem("GeoControllerCantVent",
                                                               COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                   .CONTROLLER.CANNOT_PUSH_NO_CONNECTED_NAME,
                                                               COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                   .CONTROLLER.CANNOT_PUSH_NO_CONNECTED_TOOLTIP,
                                                               "status_item_vent_disabled",
                                                               StatusItem.IconType.Custom,
                                                               NotificationType.BadMinor,
                                                               false,
                                                               OverlayModes.None.ID,
                                                               129022);

            this.GeoControllerCantVent.resolveStringCallback = delegate(string str, object obj) {
                                                                   GeothermalController geothermalController
                                                                       = obj as GeothermalController;

                                                                   if (geothermalController == null) { return str; }

                                                                   GeothermalVent geothermalVent
                                                                       = geothermalController.FirstObstructedVent();

                                                                   if (geothermalVent == null) {
                                                                       return COLONY_ACHIEVEMENTS
                                                                              .ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                              .CONTROLLER.CANNOT_PUSH_NO_CONNECTED_NAME;
                                                                   }

                                                                   if (geothermalVent.IsEntombed()) {
                                                                       return COLONY_ACHIEVEMENTS
                                                                              .ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                              .CONTROLLER
                                                                              .CANNOT_PUSH_ENTOMBED_VENT_NAME;
                                                                   }

                                                                   return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                       .STATUSITEMS.CONTROLLER
                                                                       .CANNOT_PUSH_UNREADY_CONNECTION_NAME;
                                                               };

            this.GeoControllerCantVent.resolveStringCallback_shouldStillCallIfDataIsNull = false;
            this.GeoControllerCantVent.resolveTooltipCallback = delegate(string str, object obj) {
                                                                    GeothermalController geothermalController
                                                                        = obj as GeothermalController;

                                                                    if (geothermalController == null) { return str; }

                                                                    GeothermalVent geothermalVent
                                                                        = geothermalController.FirstObstructedVent();

                                                                    if (geothermalVent == null) {
                                                                        return COLONY_ACHIEVEMENTS
                                                                               .ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                               .CONTROLLER
                                                                               .CANNOT_PUSH_NO_CONNECTED_TOOLTIP;
                                                                    }

                                                                    if (geothermalVent.IsEntombed()) {
                                                                        return COLONY_ACHIEVEMENTS
                                                                               .ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                               .CONTROLLER
                                                                               .CANNOT_PUSH_ENTOMBED_VENT_TOOLTIP;
                                                                    }

                                                                    return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .STATUSITEMS.CONTROLLER
                                                                        .CANNOT_PUSH_UNREADY_CONNECTION_TOOLTIP;
                                                                };

            this.GeoControllerCantVent.resolveTooltipCallback_shouldStillCallIfDataIsNull = false;
            this.GeoControllerCantVent.statusItemClickCallback = delegate(object obj) {
                                                                     GeothermalController geothermalController
                                                                         = obj as GeothermalController;

                                                                     GeothermalVent geothermalVent
                                                                         = (geothermalController != null)
                                                                               ? geothermalController
                                                                                   .FirstObstructedVent()
                                                                               : null;

                                                                     if (geothermalVent != null) {
                                                                         SelectTool.Instance
                                                                             .SelectAndFocus(geothermalVent.transform
                                                                                  .position,
                                                                              geothermalVent
                                                                                  .GetComponent<KSelectable>());
                                                                     }
                                                                 };

            this.GeoVentsReady = this.CreateStatusItem("GeoVentsReady",
                                                       COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT
                                                                          .READY_NAME,
                                                       COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT
                                                                          .READY_TOOLTIP,
                                                       "",
                                                       StatusItem.IconType.Info,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.None.ID,
                                                       129022);

            this.GeoVentsReady.resolveStringCallback = ((string str, object obj) =>
                                                            str.Replace("{Name}",
                                                                        (obj as GeothermalVent).GetProperName()));

            this.GeoVentsReady.resolveStringCallback_shouldStillCallIfDataIsNull = false;
            this.GeoVentsVenting = this.CreateStatusItem("GeoVentsVenting",
                                                         COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT
                                                                            .VENTING_NAME,
                                                         COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT
                                                                            .VENTING_TOOLTIP,
                                                         "",
                                                         StatusItem.IconType.Info,
                                                         NotificationType.Neutral,
                                                         false,
                                                         OverlayModes.None.ID,
                                                         129022);

            this.GeoVentsVenting.resolveStringCallback = delegate(string str, object obj) {
                                                             GeothermalVent geothermalVent = obj as GeothermalVent;
                                                             return str
                                                                    .Replace("{Name}", geothermalVent.GetProperName())
                                                                    .Replace("{Quantity}",
                                                                             GameUtil.GetFormattedMass(geothermalVent
                                                                                  .MaterialAvailable(),
                                                                              GameUtil.TimeSlice.None,
                                                                              GameUtil.MetricMassFormat
                                                                                  .UseThreshold,
                                                                              true,
                                                                              "{0:0.#}"));
                                                         };

            this.GeoVentsVenting.resolveStringCallback_shouldStillCallIfDataIsNull = false;
            this.GeoVentsVenting.resolveTooltipCallback = delegate(string str, object data) {
                                                              GeothermalVent geothermalVent = data as GeothermalVent;
                                                              if (geothermalVent != null) {
                                                                  return str.Replace("{Quantity}",
                                                                   GameUtil.GetFormattedMass(geothermalVent
                                                                        .MaterialAvailable(),
                                                                    GameUtil.TimeSlice.None,
                                                                    GameUtil.MetricMassFormat.UseThreshold,
                                                                    true,
                                                                    "{0:0.#}"));
                                                              }

                                                              return str;
                                                          };

            this.GeoVentsReady.resolveTooltipCallback_shouldStillCallIfDataIsNull = false;
            this.GeoQuestPendingReconnectPipes = this.CreateStatusItem("GeoQuestPendingReconnectPipes",
                                                                       COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                           .STATUSITEMS.CONTROLLER
                                                                           .PENDING_RECONNECTION_NAME,
                                                                       COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                           .STATUSITEMS.CONTROLLER
                                                                           .PENDING_RECONNECTION_TOOLTIP,
                                                                       "",
                                                                       StatusItem.IconType.Info,
                                                                       NotificationType.BadMinor,
                                                                       false,
                                                                       OverlayModes.None.ID,
                                                                       129022);

            this.GeoQuestPendingUncover = this.CreateStatusItem("GeoQuestPendingUncover",
                                                                COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                    .VENT.PENDING_REVEAL_NAME,
                                                                COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                    .VENT.PENDING_REVEAL_TOOLTIP,
                                                                "",
                                                                StatusItem.IconType.Info,
                                                                NotificationType.BadMinor,
                                                                false,
                                                                OverlayModes.None.ID,
                                                                129022);

            this.GeoControllerOffline = this.CreateStatusItem("GeoControllerOffline",
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .CONTROLLER.OFFLINE_NAME,
                                                              COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS
                                                                  .CONTROLLER.OFFLINE_TOOLTIP,
                                                              "",
                                                              StatusItem.IconType.Info,
                                                              NotificationType.Neutral,
                                                              false,
                                                              OverlayModes.None.ID,
                                                              129022);

            this.GeoControllerStorageStatus = this.CreateStatusItem("GeoControllerStorageStatus",
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .STATUSITEMS.CONTROLLER.STORAGE_STATUS_NAME,
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .STATUSITEMS.CONTROLLER.STORAGE_STATUS_TOOLTIP,
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    129022);

            this.GeoControllerStorageStatus = this.CreateStatusItem("GeoControllerStorageStatus",
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .STATUSITEMS.CONTROLLER.STORAGE_STATUS_NAME,
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .STATUSITEMS.CONTROLLER.STORAGE_STATUS_TOOLTIP,
                                                                    "",
                                                                    StatusItem.IconType.Info,
                                                                    NotificationType.Neutral,
                                                                    false,
                                                                    OverlayModes.None.ID,
                                                                    129022);

            this.GeoControllerStorageStatus.resolveStringCallback = delegate(string str, object obj) {
                                                                        GeothermalController geothermalController
                                                                            = obj as GeothermalController;

                                                                        float percent
                                                                            = (geothermalController != null)
                                                                                  ? (geothermalController
                                                                                                  .GetPressure() *
                                                                                              100f)
                                                                                  : 0f;

                                                                        return str.Replace("{Amount}",
                                                                         GameUtil.GetFormattedPercent(percent,
                                                                          GameUtil.TimeSlice.None));
                                                                    };

            this.GeoControllerStorageStatus.resolveTooltipCallback = delegate(string str, object obj) {
                                                                         GeothermalController geothermalController
                                                                             = obj as GeothermalController;

                                                                         float num = (geothermalController != null)
                                                                             ? geothermalController.GetPressure()
                                                                             : 0f;

                                                                         return str
                                                                             .Replace("{Amount}",
                                                                              GameUtil
                                                                                  .GetFormattedMass(12000f * num,
                                                                                   GameUtil.TimeSlice.None,
                                                                                   GameUtil.MetricMassFormat
                                                                                       .UseThreshold,
                                                                                   true,
                                                                                   "{0:0.#}"))
                                                                             .Replace("{Threshold}",
                                                                              GameUtil.GetFormattedMass(12000f,
                                                                               GameUtil.TimeSlice.None,
                                                                               GameUtil.MetricMassFormat
                                                                                   .UseThreshold,
                                                                               true,
                                                                               "{0:0.#}"));
                                                                     };

            this.GeoControllerStorageStatus.resolveStringCallback_shouldStillCallIfDataIsNull
                = (this.GeoControllerStorageStatus.resolveTooltipCallback_shouldStillCallIfDataIsNull = false);

            this.GeoControllerTemperatureStatus = this.CreateStatusItem("GeoControllerTemperatureStatus",
                                                                        COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                            .STATUSITEMS.CONTROLLER
                                                                            .STORAGE_TEMPERATURE_NAME,
                                                                        COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                            .STATUSITEMS.CONTROLLER
                                                                            .STORAGE_TEMPERATURE_TOOLTIP,
                                                                        "",
                                                                        StatusItem.IconType.Info,
                                                                        NotificationType.Neutral,
                                                                        false,
                                                                        OverlayModes.None.ID,
                                                                        129022);

            this.GeoControllerTemperatureStatus.resolveStringCallback = delegate(string str, object obj) {
                                                                            GeothermalController geothermalController
                                                                                = obj as GeothermalController;

                                                                            float temp = (geothermalController != null)
                                                                                ? geothermalController
                                                                                    .ComputeContentTemperature()
                                                                                : 0f;

                                                                            return str.Replace("{Temp}",
                                                                             GameUtil.GetFormattedTemperature(temp,
                                                                              GameUtil.TimeSlice.None,
                                                                              GameUtil.TemperatureInterpretation
                                                                                  .Absolute,
                                                                              true,
                                                                              false));
                                                                        };

            this.GeoControllerTemperatureStatus.resolveStringCallback_shouldStillCallIfDataIsNull
                = (this.GeoControllerTemperatureStatus.resolveTooltipCallback_shouldStillCallIfDataIsNull = false);

            this.RemoteWorkDockMakingWorker = new StatusItem("RemoteWorkDockMakingWorker",
                                                             BUILDING.STATUSITEMS.REMOTEWORKERDEPOT.MAKINGWORKER.NAME,
                                                             BUILDING.STATUSITEMS.REMOTEWORKERDEPOT.MAKINGWORKER
                                                                     .TOOLTIP,
                                                             "",
                                                             StatusItem.IconType.Info,
                                                             NotificationType.Good,
                                                             false,
                                                             OverlayModes.None.ID,
                                                             129022,
                                                             true,
                                                             null);

            this.RemoteWorkTerminalNoDock = new StatusItem("RemoteWorkTerminalNoDock",
                                                           BUILDING.STATUSITEMS.REMOTEWORKTERMINAL.NODOCK.NAME,
                                                           BUILDING.STATUSITEMS.REMOTEWORKTERMINAL.NODOCK.TOOLTIP,
                                                           "",
                                                           StatusItem.IconType.Exclamation,
                                                           NotificationType.Bad,
                                                           false,
                                                           OverlayModes.None.ID,
                                                           129022,
                                                           true,
                                                           null);

            this.DataMinerEfficiency = new StatusItem("RemoteWorkTerminalNoDock",
                                                      BUILDING.STATUSITEMS.DATAMINER.PRODUCTIONRATE.NAME,
                                                      BUILDING.STATUSITEMS.DATAMINER.PRODUCTIONRATE.TOOLTIP,
                                                      "",
                                                      StatusItem.IconType.Info,
                                                      NotificationType.Neutral,
                                                      false,
                                                      OverlayModes.None.ID,
                                                      129022,
                                                      true,
                                                      null);

            this.DataMinerEfficiency.resolveStringCallback = delegate(string str, object obj) {
                                                                 DataMiner dataMiner = obj as DataMiner;
                                                                 return str
                                                                        .Replace("{RATE}",
                                                                         GameUtil.GetFormattedPercent(dataMiner
                                                                                  .EfficiencyRate *
                                                                              100f,
                                                                          GameUtil.TimeSlice.None))
                                                                        .Replace("{TEMP}",
                                                                         GameUtil.GetFormattedTemperature(dataMiner
                                                                              .OperatingTemp,
                                                                          GameUtil.TimeSlice.None,
                                                                          GameUtil.TemperatureInterpretation
                                                                              .Absolute,
                                                                          true,
                                                                          false));
                                                             };
        }

        private static bool ShowInUtilityOverlay(HashedString mode, object data) {
            Transform transform = (Transform)data;
            bool      result    = false;
            if (mode == OverlayModes.GasConduits.ID) {
                Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
                result = OverlayScreen.GasVentIDs.Contains(prefabTag);
            } else if (mode == OverlayModes.LiquidConduits.ID) {
                Tag prefabTag2 = transform.GetComponent<KPrefabID>().PrefabTag;
                result = OverlayScreen.LiquidVentIDs.Contains(prefabTag2);
            } else if (mode == OverlayModes.Power.ID) {
                Tag prefabTag3 = transform.GetComponent<KPrefabID>().PrefabTag;
                result = OverlayScreen.WireIDs.Contains(prefabTag3);
            } else if (mode == OverlayModes.Logic.ID) {
                Tag prefabTag4 = transform.GetComponent<KPrefabID>().PrefabTag;
                result = OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag4);
            } else if (mode == OverlayModes.SolidConveyor.ID) {
                Tag prefabTag5 = transform.GetComponent<KPrefabID>().PrefabTag;
                result = OverlayScreen.SolidConveyorIDs.Contains(prefabTag5);
            } else if (mode == OverlayModes.Radiation.ID) {
                Tag prefabTag6 = transform.GetComponent<KPrefabID>().PrefabTag;
                result = OverlayScreen.RadiationIDs.Contains(prefabTag6);
            }

            return result;
        }

        [CompilerGenerated]
        internal static string<CreateStatusItems>g__SkyVisResolveStringCallback|316_107(string str, object data)
        {
            ISkyVisInfo skyVisInfo = (ISkyVisInfo)data;
            return str.Replace("{VISIBILITY}",
                               GameUtil.GetFormattedPercent(skyVisInfo.GetPercentVisible01() * 100f,
                                                            GameUtil.TimeSlice.None));
        }

        public interface ISkyVisInfo {
            float GetPercentVisible01();
        }
    }
}