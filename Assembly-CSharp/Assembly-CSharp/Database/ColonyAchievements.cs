using System;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using TUNING;

namespace Database {
    public class ColonyAchievements : ResourceSet<ColonyAchievement> {
        public ColonyAchievement ActivateGeothermalPlant;
        public ColonyAchievement AutomateABuilding;
        public ColonyAchievement BasicComforts;
        public ColonyAchievement BasicPumping;
        public ColonyAchievement Build4NatureReserves;
        public ColonyAchievement BuildOutsideStartBiome;
        public ColonyAchievement BunkerDoorDefense;
        public ColonyAchievement ClearFOW;
        public ColonyAchievement Clothe8Dupes;
        public ColonyAchievement CollectedArtifacts;
        public ColonyAchievement CompleteResearchTree;
        public ColonyAchievement CompleteSkillBranch;
        public ColonyAchievement CoolBuildingTo6K;
        public ColonyAchievement CuredDisease;
        public ColonyAchievement EatCookedFood;
        public ColonyAchievement EatkCalFromMeatByCycle100;
        public ColonyAchievement ExosuitCycles;
        public ColonyAchievement ExploreOilBiome;
        public ColonyAchievement FirstTeleport;
        public ColonyAchievement Generate240000kJClean;
        public ColonyAchievement GeneratorTuneup;
        public ColonyAchievement GMOOK;
        public ColonyAchievement HatchACritter;
        public ColonyAchievement HatchRefinement;
        public ColonyAchievement IdleDuplicants;
        public ColonyAchievement InspectPOI;
        public ColonyAchievement LandedOnAllWorlds;
        public ColonyAchievement MasterpiecePainting;
        public ColonyAchievement MineTheGap;
        public ColonyAchievement Minimum20LivingDupes;
        public ColonyAchievement NoFarmTilesAndKCal;
        public ColonyAchievement PlumbedWashrooms;
        public ColonyAchievement RadicalTrip;
        public ColonyAchievement ReachedDistantPlanet;
        public ColonyAchievement ReachedSpace;
        public ColonyAchievement RunAReactor;
        public ColonyAchievement SoftLaunch;
        public ColonyAchievement Survived100Cycles;
        public ColonyAchievement SurviveInARocket;
        public ColonyAchievement SurviveOneYear;
        public ColonyAchievement SweeterThanHoney;
        public ColonyAchievement TameAGassyMoo;
        public ColonyAchievement TameAllBasicCritters;
        public ColonyAchievement Thriving;
        public ColonyAchievement Travel10000InTubes;
        public ColonyAchievement VarietyOfRooms;

        public ColonyAchievements(ResourceSet parent) : base("ColonyAchievements", parent) {
            Thriving = Add(new ColonyAchievement("Thriving",
                                                 "WINCONDITION_STAY",
                                                 COLONY_ACHIEVEMENTS.THRIVING.NAME,
                                                 COLONY_ACHIEVEMENTS.THRIVING.DESCRIPTION,
                                                 true,
                                                 new List<ColonyAchievementRequirement> {
                                                     new CycleNumber(200),
                                                     new MinimumMorale(),
                                                     new NumberOfDupes(12),
                                                     new MonumentBuilt()
                                                 },
                                                 COLONY_ACHIEVEMENTS.THRIVING.MESSAGE_TITLE,
                                                 COLONY_ACHIEVEMENTS.THRIVING.MESSAGE_BODY,
                                                 "victoryShorts/Stay",
                                                 "victoryLoops/Stay_loop",
                                                 ThrivingSequence.Start,
                                                 AudioMixerSnapshots.Get().VictoryNISGenericSnapshot,
                                                 "home_sweet_home"));

            ReachedDistantPlanet = DlcManager.IsExpansion1Active()
                                       ? Add(new ColonyAchievement("ReachedDistantPlanet",
                                                                   "WINCONDITION_LEAVE",
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.NAME,
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED
                                                                       .DESCRIPTION,
                                                                   true,
                                                                   new List<ColonyAchievementRequirement> {
                                                                       new EstablishColonies(),
                                                                       new OpenTemporalTear(),
                                                                       new SentCraftIntoTemporalTear()
                                                                   },
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED
                                                                       .MESSAGE_TITLE_DLC1,
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED
                                                                       .MESSAGE_BODY_DLC1,
                                                                   "victoryShorts/Leave",
                                                                   "victoryLoops/Leave_loop",
                                                                   EnterTemporalTearSequence.Start,
                                                                   AudioMixerSnapshots.Get().VictoryNISRocketSnapshot,
                                                                   "rocket"))
                                       : Add(new ColonyAchievement("ReachedDistantPlanet",
                                                                   "WINCONDITION_LEAVE",
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.NAME,
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED
                                                                       .DESCRIPTION,
                                                                   true,
                                                                   new List<ColonyAchievementRequirement> {
                                                                       new ReachedSpace(Db.Get()
                                                                           .SpaceDestinationTypes.Wormhole)
                                                                   },
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED
                                                                       .MESSAGE_TITLE,
                                                                   COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED
                                                                       .MESSAGE_BODY,
                                                                   "victoryShorts/Leave",
                                                                   "victoryLoops/Leave_loop",
                                                                   ReachedDistantPlanetSequence.Start,
                                                                   AudioMixerSnapshots.Get().VictoryNISRocketSnapshot,
                                                                   "rocket"));

            if (DlcManager.IsExpansion1Active()) {
                CollectedArtifacts = new ColonyAchievement("CollectedArtifacts",
                                                           "WINCONDITION_ARTIFACTS",
                                                           COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.NAME,
                                                           COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.DESCRIPTION,
                                                           true,
                                                           new List<ColonyAchievementRequirement> {
                                                               new CollectedArtifacts(), new CollectedSpaceArtifacts()
                                                           },
                                                           COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.MESSAGE_TITLE,
                                                           COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.MESSAGE_BODY,
                                                           "victoryShorts/Artifact",
                                                           "victoryLoops/Artifact_loop",
                                                           ArtifactSequence.Start,
                                                           AudioMixerSnapshots.Get().VictoryNISGenericSnapshot,
                                                           "cosmic_archaeology",
                                                           DlcManager.AVAILABLE_EXPANSION1_ONLY,
                                                           "EXPANSION1_ID");

                Add(CollectedArtifacts);
            }

            if (DlcManager.IsContentSubscribed("DLC2_ID"))
                ActivateGeothermalPlant = Add(new ColonyAchievement("ActivatedGeothermalPlant",
                                                                    "WINCONDITION_GEOPLANT",
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NAME,
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .DESCRIPTION,
                                                                    true,
                                                                    new List<ColonyAchievementRequirement> {
                                                                        new DiscoverGeothermalFacility(),
                                                                        new RepairGeothermalController(),
                                                                        new UseGeothermalPlant(),
                                                                        new ClearBlockedGeothermalVent()
                                                                    },
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .MESSAGE_TITLE,
                                                                    COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT
                                                                        .MESSAGE_BODY,
                                                                    "victoryShorts/Geothermal",
                                                                    "victoryLoops/Geothermal_loop",
                                                                    GeothermalVictorySequence.Start,
                                                                    AudioMixerSnapshots.Get().VictoryNISGenericSnapshot,
                                                                    "geothermalplant",
                                                                    DlcManager.AVAILABLE_DLC_2,
                                                                    "DLC2_ID",
                                                                    "GeothermalImperative"));

            Survived100Cycles = Add(new ColonyAchievement("Survived100Cycles",
                                                          "SURVIVE_HUNDRED_CYCLES",
                                                          COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_HUNDRED_CYCLES,
                                                          COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                             .SURVIVE_HUNDRED_CYCLES_DESCRIPTION,
                                                          false,
                                                          new List<ColonyAchievementRequirement> { new CycleNumber() },
                                                          "",
                                                          "",
                                                          "",
                                                          "",
                                                          null,
                                                          default(EventReference),
                                                          "Turn_of_the_Century"));

            ReachedSpace = DlcManager.IsExpansion1Active()
                               ? Add(new ColonyAchievement("ReachedSpace",
                                                           "REACH_SPACE_ANY_DESTINATION",
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                              .REACH_SPACE_ANY_DESTINATION,
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                              .REACH_SPACE_ANY_DESTINATION_DESCRIPTION,
                                                           false,
                                                           new List<ColonyAchievementRequirement> {
                                                               new LaunchedCraft()
                                                           },
                                                           "",
                                                           "",
                                                           "",
                                                           "",
                                                           null,
                                                           default(EventReference),
                                                           "space_race"))
                               : Add(new ColonyAchievement("ReachedSpace",
                                                           "REACH_SPACE_ANY_DESTINATION",
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                              .REACH_SPACE_ANY_DESTINATION,
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                              .REACH_SPACE_ANY_DESTINATION_DESCRIPTION,
                                                           false,
                                                           new List<ColonyAchievementRequirement> {
                                                               new ReachedSpace()
                                                           },
                                                           "",
                                                           "",
                                                           "",
                                                           "",
                                                           null,
                                                           default(EventReference),
                                                           "space_race"));

            CompleteSkillBranch = Add(new ColonyAchievement("CompleteSkillBranch",
                                                            "COMPLETED_SKILL_BRANCH",
                                                            COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                               .COMPLETED_SKILL_BRANCH,
                                                            COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                               .COMPLETED_SKILL_BRANCH_DESCRIPTION,
                                                            false,
                                                            new List<ColonyAchievementRequirement> {
                                                                new SkillBranchComplete(Db.Get()
                                                                    .Skills.GetTerminalSkills())
                                                            },
                                                            "",
                                                            "",
                                                            "",
                                                            "",
                                                            null,
                                                            default(EventReference),
                                                            "CompleteSkillBranch"));

            CompleteResearchTree = Add(new ColonyAchievement("CompleteResearchTree",
                                                             "COMPLETED_RESEARCH",
                                                             COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_RESEARCH,
                                                             COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                 .COMPLETED_RESEARCH_DESCRIPTION,
                                                             false,
                                                             new List<ColonyAchievementRequirement> {
                                                                 new ResearchComplete()
                                                             },
                                                             "",
                                                             "",
                                                             "",
                                                             "",
                                                             null,
                                                             default(EventReference),
                                                             "honorary_doctorate"));

            Clothe8Dupes = Add(new ColonyAchievement("Clothe8Dupes",
                                                     "EQUIP_EIGHT_DUPES",
                                                     COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EQUIP_N_DUPES,
                                                     string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                       .EQUIP_N_DUPES_DESCRIPTION,
                                                                   8),
                                                     false,
                                                     new List<ColonyAchievementRequirement> {
                                                         new EquipNDupes(Db.Get().AssignableSlots.Outfit, 8)
                                                     },
                                                     "",
                                                     "",
                                                     "",
                                                     "",
                                                     null,
                                                     default(EventReference),
                                                     "and_nowhere_to_go"));

            TameAllBasicCritters = Add(new ColonyAchievement("TameAllBasicCritters",
                                                             "TAME_BASIC_CRITTERS",
                                                             COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_BASIC_CRITTERS,
                                                             COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                 .TAME_BASIC_CRITTERS_DESCRIPTION,
                                                             false,
                                                             new List<ColonyAchievementRequirement> {
                                                                 new CritterTypesWithTraits(new List<Tag> {
                                                                     "Drecko",
                                                                     "Hatch",
                                                                     "LightBug",
                                                                     "Mole",
                                                                     "Oilfloater",
                                                                     "Pacu",
                                                                     "Puft",
                                                                     "Moo",
                                                                     "Crab",
                                                                     "Squirrel"
                                                                 })
                                                             },
                                                             "",
                                                             "",
                                                             "",
                                                             "",
                                                             null,
                                                             default(EventReference),
                                                             "Animal_friends"));

            Build4NatureReserves = Add(new ColonyAchievement("Build4NatureReserves",
                                                             "BUILD_NATURE_RESERVES",
                                                             COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                 .BUILD_NATURE_RESERVES,
                                                             string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                               .BUILD_NATURE_RESERVES_DESCRIPTION,
                                                                           Db.Get().RoomTypes.NatureReserve.Name,
                                                                           4),
                                                             false,
                                                             new List<ColonyAchievementRequirement> {
                                                                 new BuildNRoomTypes(Db.Get().RoomTypes.NatureReserve,
                                                                  4)
                                                             },
                                                             "",
                                                             "",
                                                             "",
                                                             "",
                                                             null,
                                                             default(EventReference),
                                                             "Some_Reservations"));

            Minimum20LivingDupes = Add(new ColonyAchievement("Minimum20LivingDupes",
                                                             "TWENTY_DUPES",
                                                             COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TWENTY_DUPES,
                                                             COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                 .TWENTY_DUPES_DESCRIPTION,
                                                             false,
                                                             new List<ColonyAchievementRequirement> {
                                                                 new NumberOfDupes(20)
                                                             },
                                                             "",
                                                             "",
                                                             "",
                                                             "",
                                                             null,
                                                             default(EventReference),
                                                             "no_place_like_clone"));

            TameAGassyMoo = Add(new ColonyAchievement("TameAGassyMoo",
                                                      "TAME_GASSYMOO",
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_GASSYMOO,
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_GASSYMOO_DESCRIPTION,
                                                      false,
                                                      new List<ColonyAchievementRequirement> {
                                                          new CritterTypesWithTraits(new List<Tag> { "Moo" })
                                                      },
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      null,
                                                      default(EventReference),
                                                      "moovin_on_up"));

            CoolBuildingTo6K = Add(new ColonyAchievement("CoolBuildingTo6K",
                                                         "SIXKELVIN_BUILDING",
                                                         COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SIXKELVIN_BUILDING,
                                                         COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                            .SIXKELVIN_BUILDING_DESCRIPTION,
                                                         false,
                                                         new List<ColonyAchievementRequirement> {
                                                             new CoolBuildingToXKelvin(6)
                                                         },
                                                         "",
                                                         "",
                                                         "",
                                                         "",
                                                         null,
                                                         default(EventReference),
                                                         "not_0k"));

            EatkCalFromMeatByCycle100 = Add(new ColonyAchievement("EatkCalFromMeatByCycle100",
                                                                  "EAT_MEAT",
                                                                  COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EAT_MEAT,
                                                                  COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                      .EAT_MEAT_DESCRIPTION,
                                                                  false,
                                                                  new List<ColonyAchievementRequirement> {
                                                                      new BeforeCycleNumber(),
                                                                      new EatXCaloriesFromY(400000,
                                                                       new List<string> {
                                                                           FOOD.FOOD_TYPES.MEAT.Id,
                                                                           FOOD.FOOD_TYPES.DEEP_FRIED_MEAT.Id,
                                                                           FOOD.FOOD_TYPES.PEMMICAN.Id,
                                                                           FOOD.FOOD_TYPES.FISH_MEAT.Id,
                                                                           FOOD.FOOD_TYPES.COOKED_FISH.Id,
                                                                           FOOD.FOOD_TYPES.DEEP_FRIED_FISH.Id,
                                                                           FOOD.FOOD_TYPES.SHELLFISH_MEAT.Id,
                                                                           FOOD.FOOD_TYPES.DEEP_FRIED_SHELLFISH.Id,
                                                                           FOOD.FOOD_TYPES.COOKED_MEAT.Id,
                                                                           FOOD.FOOD_TYPES.SURF_AND_TURF.Id,
                                                                           FOOD.FOOD_TYPES.BURGER.Id
                                                                       })
                                                                  },
                                                                  "",
                                                                  "",
                                                                  "",
                                                                  "",
                                                                  null,
                                                                  default(EventReference),
                                                                  "Carnivore"));

            NoFarmTilesAndKCal = Add(new ColonyAchievement("NoFarmTilesAndKCal",
                                                           "NO_PLANTERBOX",
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.NO_PLANTERBOX,
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                              .NO_PLANTERBOX_DESCRIPTION,
                                                           false,
                                                           new List<ColonyAchievementRequirement> {
                                                               new NoFarmables(), new EatXCalories(400000)
                                                           },
                                                           "",
                                                           "",
                                                           "",
                                                           "",
                                                           null,
                                                           default(EventReference),
                                                           "Locavore"));

            Generate240000kJClean = Add(new ColonyAchievement("Generate240000kJClean",
                                                              "CLEAN_ENERGY",
                                                              COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAN_ENERGY,
                                                              COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                  .CLEAN_ENERGY_DESCRIPTION,
                                                              false,
                                                              new List<ColonyAchievementRequirement> {
                                                                  new ProduceXEngeryWithoutUsingYList(240000f,
                                                                   new List<Tag> {
                                                                       "MethaneGenerator",
                                                                       "PetroleumGenerator",
                                                                       "WoodGasGenerator",
                                                                       "Generator"
                                                                   })
                                                              },
                                                              "",
                                                              "",
                                                              "",
                                                              "",
                                                              null,
                                                              default(EventReference),
                                                              "sustainably_sustaining"));

            BuildOutsideStartBiome = Add(new ColonyAchievement("BuildOutsideStartBiome",
                                                               "BUILD_OUTSIDE_BIOME",
                                                               COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                   .BUILD_OUTSIDE_BIOME,
                                                               COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                   .BUILD_OUTSIDE_BIOME_DESCRIPTION,
                                                               false,
                                                               new List<ColonyAchievementRequirement> {
                                                                   new BuildOutsideStartBiome()
                                                               },
                                                               "",
                                                               "",
                                                               "",
                                                               "",
                                                               null,
                                                               default(EventReference),
                                                               "build_outside"));

            Travel10000InTubes = Add(new ColonyAchievement("Travel10000InTubes",
                                                           "TUBE_TRAVEL_DISTANCE",
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TUBE_TRAVEL_DISTANCE,
                                                           COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                              .TUBE_TRAVEL_DISTANCE_DESCRIPTION,
                                                           false,
                                                           new List<ColonyAchievementRequirement> {
                                                               new TravelXUsingTransitTubes(NavType.Tube, 10000)
                                                           },
                                                           "",
                                                           "",
                                                           "",
                                                           "",
                                                           null,
                                                           default(EventReference),
                                                           "Totally-Tubular"));

            VarietyOfRooms = Add(new ColonyAchievement("VarietyOfRooms",
                                                       "VARIETY_OF_ROOMS",
                                                       COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.VARIETY_OF_ROOMS,
                                                       COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                          .VARIETY_OF_ROOMS_DESCRIPTION,
                                                       false,
                                                       new List<ColonyAchievementRequirement> {
                                                           new BuildRoomType(Db.Get().RoomTypes.NatureReserve),
                                                           new BuildRoomType(Db.Get().RoomTypes.Hospital),
                                                           new BuildRoomType(Db.Get().RoomTypes.RecRoom),
                                                           new BuildRoomType(Db.Get().RoomTypes.GreatHall),
                                                           new BuildRoomType(Db.Get().RoomTypes.Bedroom),
                                                           new BuildRoomType(Db.Get().RoomTypes.PlumbedBathroom),
                                                           new BuildRoomType(Db.Get().RoomTypes.Farm),
                                                           new BuildRoomType(Db.Get().RoomTypes.CreaturePen)
                                                       },
                                                       "",
                                                       "",
                                                       "",
                                                       "",
                                                       null,
                                                       default(EventReference),
                                                       "Get-a-Room"));

            SurviveOneYear = Add(new ColonyAchievement("SurviveOneYear",
                                                       "SURVIVE_ONE_YEAR",
                                                       COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_ONE_YEAR,
                                                       COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                          .SURVIVE_ONE_YEAR_DESCRIPTION,
                                                       false,
                                                       new List<ColonyAchievementRequirement> {
                                                           new FractionalCycleNumber(365.25f)
                                                       },
                                                       "",
                                                       "",
                                                       "",
                                                       "",
                                                       null,
                                                       default(EventReference),
                                                       "One_year"));

            ExploreOilBiome = Add(new ColonyAchievement("ExploreOilBiome",
                                                        "EXPLORE_OIL_BIOME",
                                                        COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXPLORE_OIL_BIOME,
                                                        COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                           .EXPLORE_OIL_BIOME_DESCRIPTION,
                                                        false,
                                                        new List<ColonyAchievementRequirement> {
                                                            new ExploreOilFieldSubZone()
                                                        },
                                                        "",
                                                        "",
                                                        "",
                                                        "",
                                                        null,
                                                        default(EventReference),
                                                        "enter_oil_biome"));

            EatCookedFood = Add(new ColonyAchievement("EatCookedFood",
                                                      "COOKED_FOOD",
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COOKED_FOOD,
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COOKED_FOOD_DESCRIPTION,
                                                      false,
                                                      new List<ColonyAchievementRequirement> {
                                                          new EatXKCalProducedByY(1,
                                                           new List<Tag> {
                                                               "GourmetCookingStation", "CookingStation"
                                                           })
                                                      },
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      null,
                                                      default(EventReference),
                                                      "its_not_raw"));

            BasicPumping = Add(new ColonyAchievement("BasicPumping",
                                                     "BASIC_PUMPING",
                                                     COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_PUMPING,
                                                     COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_PUMPING_DESCRIPTION,
                                                     false,
                                                     new List<ColonyAchievementRequirement> {
                                                         new VentXKG(SimHashes.Oxygen, 1000f)
                                                     },
                                                     "",
                                                     "",
                                                     "",
                                                     "",
                                                     null,
                                                     default(EventReference),
                                                     "BasicPumping"));

            BasicComforts = Add(new ColonyAchievement("BasicComforts",
                                                      "BASIC_COMFORTS",
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_COMFORTS,
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_COMFORTS_DESCRIPTION,
                                                      false,
                                                      new List<ColonyAchievementRequirement> {
                                                          new AtLeastOneBuildingForEachDupe(new List<Tag> {
                                                              "FlushToilet", "Outhouse"
                                                          }),
                                                          new AtLeastOneBuildingForEachDupe(new List<Tag> {
                                                              "Bed", "LuxuryBed"
                                                          })
                                                      },
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      null,
                                                      default(EventReference),
                                                      "1bed_1toilet"));

            PlumbedWashrooms = Add(new ColonyAchievement("PlumbedWashrooms",
                                                         "PLUMBED_WASHROOMS",
                                                         COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.PLUMBED_WASHROOMS,
                                                         COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                            .PLUMBED_WASHROOMS_DESCRIPTION,
                                                         false,
                                                         new List<ColonyAchievementRequirement> {
                                                             new UpgradeAllBasicBuildings("Outhouse",  "FlushToilet"),
                                                             new UpgradeAllBasicBuildings("WashBasin", "WashSink")
                                                         },
                                                         "",
                                                         "",
                                                         "",
                                                         "",
                                                         null,
                                                         default(EventReference),
                                                         "royal_flush"));

            AutomateABuilding = Add(new ColonyAchievement("AutomateABuilding",
                                                          "AUTOMATE_A_BUILDING",
                                                          COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.AUTOMATE_A_BUILDING,
                                                          COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                             .AUTOMATE_A_BUILDING_DESCRIPTION,
                                                          false,
                                                          new List<ColonyAchievementRequirement> {
                                                              new AutomateABuilding()
                                                          },
                                                          "",
                                                          "",
                                                          "",
                                                          "",
                                                          null,
                                                          default(EventReference),
                                                          "red_light_green_light"));

            MasterpiecePainting = Add(new ColonyAchievement("MasterpiecePainting",
                                                            "MASTERPIECE_PAINTING",
                                                            COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MASTERPIECE_PAINTING,
                                                            COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                               .MASTERPIECE_PAINTING_DESCRIPTION,
                                                            false,
                                                            new List<ColonyAchievementRequirement> {
                                                                new CreateMasterPainting()
                                                            },
                                                            "",
                                                            "",
                                                            "",
                                                            "",
                                                            null,
                                                            default(EventReference),
                                                            "art_underground"));

            InspectPOI = Add(new ColonyAchievement("InspectPOI",
                                                   "INSPECT_POI",
                                                   COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.INSPECT_POI,
                                                   COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.INSPECT_POI_DESCRIPTION,
                                                   false,
                                                   new List<ColonyAchievementRequirement> { new ActivateLorePOI() },
                                                   "",
                                                   "",
                                                   "",
                                                   "",
                                                   null,
                                                   default(EventReference),
                                                   "ghosts_of_gravitas"));

            HatchACritter = Add(new ColonyAchievement("HatchACritter",
                                                      "HATCH_A_CRITTER",
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_A_CRITTER,
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_A_CRITTER_DESCRIPTION,
                                                      false,
                                                      new List<ColonyAchievementRequirement> {
                                                          new CritterTypeExists(new List<Tag> {
                                                              "DreckoPlasticBaby",
                                                              "HatchHardBaby",
                                                              "HatchMetalBaby",
                                                              "HatchVeggieBaby",
                                                              "LightBugBlackBaby",
                                                              "LightBugBlueBaby",
                                                              "LightBugCrystalBaby",
                                                              "LightBugOrangeBaby",
                                                              "LightBugPinkBaby",
                                                              "LightBugPurpleBaby",
                                                              "OilfloaterDecorBaby",
                                                              "OilfloaterHighTempBaby",
                                                              "PacuCleanerBaby",
                                                              "PacuTropicalBaby",
                                                              "PuftBleachstoneBaby",
                                                              "PuftOxyliteBaby",
                                                              "SquirrelHugBaby",
                                                              "CrabWoodBaby",
                                                              "CrabFreshWaterBaby",
                                                              "MoleDelicacyBaby"
                                                          })
                                                      },
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      null,
                                                      default(EventReference),
                                                      "good_egg"));

            CuredDisease = Add(new ColonyAchievement("CuredDisease",
                                                     "CURED_DISEASE",
                                                     COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CURED_DISEASE,
                                                     COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CURED_DISEASE_DESCRIPTION,
                                                     false,
                                                     new List<ColonyAchievementRequirement> { new CureDisease() },
                                                     "",
                                                     "",
                                                     "",
                                                     "",
                                                     null,
                                                     default(EventReference),
                                                     "medic"));

            GeneratorTuneup = Add(new ColonyAchievement("GeneratorTuneup",
                                                        "GENERATOR_TUNEUP",
                                                        COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GENERATOR_TUNEUP,
                                                        string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                          .GENERATOR_TUNEUP_DESCRIPTION,
                                                                      100),
                                                        false,
                                                        new List<ColonyAchievementRequirement> {
                                                            new TuneUpGenerator(100f)
                                                        },
                                                        "",
                                                        "",
                                                        "",
                                                        "",
                                                        null,
                                                        default(EventReference),
                                                        "tune_up_for_what"));

            ClearFOW = Add(new ColonyAchievement("ClearFOW",
                                                 "CLEAR_FOW",
                                                 COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAR_FOW,
                                                 COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAR_FOW_DESCRIPTION,
                                                 false,
                                                 new List<ColonyAchievementRequirement> { new RevealAsteriod(0.8f) },
                                                 "",
                                                 "",
                                                 "",
                                                 "",
                                                 null,
                                                 default(EventReference),
                                                 "pulling_back_the_veil"));

            HatchRefinement = Add(new ColonyAchievement("HatchRefinement",
                                                        "HATCH_REFINEMENT",
                                                        COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_REFINEMENT,
                                                        string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                          .HATCH_REFINEMENT_DESCRIPTION,
                                                                      GameUtil.GetFormattedMass(10000f,
                                                                       GameUtil.TimeSlice.None,
                                                                       GameUtil.MetricMassFormat.Tonne)),
                                                        false,
                                                        new List<ColonyAchievementRequirement> {
                                                            new CreaturePoopKGProduction("HatchMetal", 10000f)
                                                        },
                                                        "",
                                                        "",
                                                        "",
                                                        "",
                                                        null,
                                                        default(EventReference),
                                                        "down_the_hatch"));

            BunkerDoorDefense = Add(new ColonyAchievement("BunkerDoorDefense",
                                                          "BUNKER_DOOR_DEFENSE",
                                                          COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUNKER_DOOR_DEFENSE,
                                                          COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                             .BUNKER_DOOR_DEFENSE_DESCRIPTION,
                                                          false,
                                                          new List<ColonyAchievementRequirement> {
                                                              new BlockedCometWithBunkerDoor()
                                                          },
                                                          "",
                                                          "",
                                                          "",
                                                          "",
                                                          null,
                                                          default(EventReference),
                                                          "Immovable_Object"));

            IdleDuplicants = Add(new ColonyAchievement("IdleDuplicants",
                                                       "IDLE_DUPLICANTS",
                                                       COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.IDLE_DUPLICANTS,
                                                       COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                          .IDLE_DUPLICANTS_DESCRIPTION,
                                                       false,
                                                       new List<ColonyAchievementRequirement> {
                                                           new DupesVsSolidTransferArmFetch(1f, 5)
                                                       },
                                                       "",
                                                       "",
                                                       "",
                                                       "",
                                                       null,
                                                       default(EventReference),
                                                       "easy_livin"));

            ExosuitCycles = Add(new ColonyAchievement("ExosuitCycles",
                                                      "EXOSUIT_CYCLES",
                                                      COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXOSUIT_CYCLES,
                                                      string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS
                                                                        .EXOSUIT_CYCLES_DESCRIPTION,
                                                                    10),
                                                      false,
                                                      new List<ColonyAchievementRequirement> {
                                                          new DupesCompleteChoreInExoSuitForCycles(10)
                                                      },
                                                      "",
                                                      "",
                                                      "",
                                                      "",
                                                      null,
                                                      default(EventReference),
                                                      "job_suitability"));

            if (DlcManager.IsExpansion1Active()) {
                var    id                    = "FirstTeleport";
                var    platformAchievementId = "FIRST_TELEPORT";
                string name                  = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.FIRST_TELEPORT;
                string description           = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.FIRST_TELEPORT_DESCRIPTION;
                var    isVictoryCondition    = false;
                var    list                  = new List<ColonyAchievementRequirement>();
                list.Add(new TeleportDuplicant());
                list.Add(new DefrostDuplicant());
                var                    messageTitle              = "";
                var                    messageBody               = "";
                var                    videoDataName             = "";
                var                    victoryLoopVideo          = "";
                Action<KMonoBehaviour> victorySequence           = null;
                var                    available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                FirstTeleport = Add(new ColonyAchievement(id,
                                                          platformAchievementId,
                                                          name,
                                                          description,
                                                          isVictoryCondition,
                                                          list,
                                                          messageTitle,
                                                          messageBody,
                                                          videoDataName,
                                                          victoryLoopVideo,
                                                          victorySequence,
                                                          default(EventReference),
                                                          "first_teleport_of_call",
                                                          available_EXPANSION1_ONLY,
                                                          "EXPANSION1_ID"));

                var    id2                    = "SoftLaunch";
                var    platformAchievementId2 = "SOFT_LAUNCH";
                string name2                  = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SOFT_LAUNCH;
                string description2           = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SOFT_LAUNCH_DESCRIPTION;
                var    isVictoryCondition2    = false;
                var    list2                  = new List<ColonyAchievementRequirement>();
                list2.Add(new BuildALaunchPad());
                var                    messageTitle2     = "";
                var                    messageBody2      = "";
                var                    videoDataName2    = "";
                var                    victoryLoopVideo2 = "";
                Action<KMonoBehaviour> victorySequence2  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                SoftLaunch = Add(new ColonyAchievement(id2,
                                                       platformAchievementId2,
                                                       name2,
                                                       description2,
                                                       isVictoryCondition2,
                                                       list2,
                                                       messageTitle2,
                                                       messageBody2,
                                                       videoDataName2,
                                                       victoryLoopVideo2,
                                                       victorySequence2,
                                                       default(EventReference),
                                                       "soft_launch",
                                                       available_EXPANSION1_ONLY,
                                                       "EXPANSION1_ID"));

                var    id3                    = "GMOOK";
                var    platformAchievementId3 = "GMO_OK";
                string name3                  = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GMO_OK;
                string description3           = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GMO_OK_DESCRIPTION;
                var    isVictoryCondition3    = false;
                var    list3                  = new List<ColonyAchievementRequirement>();
                list3.Add(new AnalyzeSeed(BasicFabricMaterialPlantConfig.ID));
                list3.Add(new AnalyzeSeed("BasicSingleHarvestPlant"));
                list3.Add(new AnalyzeSeed("GasGrass"));
                list3.Add(new AnalyzeSeed("MushroomPlant"));
                list3.Add(new AnalyzeSeed("PrickleFlower"));
                list3.Add(new AnalyzeSeed("SaltPlant"));
                list3.Add(new AnalyzeSeed(SeaLettuceConfig.ID));
                list3.Add(new AnalyzeSeed("SpiceVine"));
                list3.Add(new AnalyzeSeed("SwampHarvestPlant"));
                list3.Add(new AnalyzeSeed(SwampLilyConfig.ID));
                list3.Add(new AnalyzeSeed("WormPlant"));
                list3.Add(new AnalyzeSeed("ColdWheat"));
                list3.Add(new AnalyzeSeed("BeanPlant"));
                var                    messageTitle3     = "";
                var                    messageBody3      = "";
                var                    videoDataName3    = "";
                var                    victoryLoopVideo3 = "";
                Action<KMonoBehaviour> victorySequence3  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                GMOOK = Add(new ColonyAchievement(id3,
                                                  platformAchievementId3,
                                                  name3,
                                                  description3,
                                                  isVictoryCondition3,
                                                  list3,
                                                  messageTitle3,
                                                  messageBody3,
                                                  videoDataName3,
                                                  victoryLoopVideo3,
                                                  victorySequence3,
                                                  default(EventReference),
                                                  "gmo_ok",
                                                  available_EXPANSION1_ONLY,
                                                  "EXPANSION1_ID"));

                var    id4                    = "MineTheGap";
                var    platformAchievementId4 = "MINE_THE_GAP";
                string name4                  = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MINE_THE_GAP;
                string description4           = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MINE_THE_GAP_DESCRIPTION;
                var    isVictoryCondition4    = false;
                var    list4                  = new List<ColonyAchievementRequirement>();
                list4.Add(new HarvestAmountFromSpacePOI(1000000f));
                var                    messageTitle4     = "";
                var                    messageBody4      = "";
                var                    videoDataName4    = "";
                var                    victoryLoopVideo4 = "";
                Action<KMonoBehaviour> victorySequence4  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                MineTheGap = Add(new ColonyAchievement(id4,
                                                       platformAchievementId4,
                                                       name4,
                                                       description4,
                                                       isVictoryCondition4,
                                                       list4,
                                                       messageTitle4,
                                                       messageBody4,
                                                       videoDataName4,
                                                       victoryLoopVideo4,
                                                       victorySequence4,
                                                       default(EventReference),
                                                       "mine_the_gap",
                                                       available_EXPANSION1_ONLY,
                                                       "EXPANSION1_ID"));

                var    id5                    = "LandedOnAllWorlds";
                var    platformAchievementId5 = "LANDED_ON_ALL_WORLDS";
                string name5                  = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.LAND_ON_ALL_WORLDS;
                string description5           = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.LAND_ON_ALL_WORLDS_DESCRIPTION;
                var    isVictoryCondition5    = false;
                var    list5                  = new List<ColonyAchievementRequirement>();
                list5.Add(new LandOnAllWorlds());
                var                    messageTitle5     = "";
                var                    messageBody5      = "";
                var                    videoDataName5    = "";
                var                    victoryLoopVideo5 = "";
                Action<KMonoBehaviour> victorySequence5  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                LandedOnAllWorlds = Add(new ColonyAchievement(id5,
                                                              platformAchievementId5,
                                                              name5,
                                                              description5,
                                                              isVictoryCondition5,
                                                              list5,
                                                              messageTitle5,
                                                              messageBody5,
                                                              videoDataName5,
                                                              victoryLoopVideo5,
                                                              victorySequence5,
                                                              default(EventReference),
                                                              "land_on_all_worlds",
                                                              available_EXPANSION1_ONLY,
                                                              "EXPANSION1_ID"));

                var    id6 = "RadicalTrip";
                var    platformAchievementId6 = "RADICAL_TRIP";
                string name6 = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.RADICAL_TRIP;
                var    description6 = string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.RADICAL_TRIP_DESCRIPTION, 10);
                var    isVictoryCondition6 = false;
                var    list6 = new List<ColonyAchievementRequirement>();
                list6.Add(new RadBoltTravelDistance(10000));
                var                    messageTitle6     = "";
                var                    messageBody6      = "";
                var                    videoDataName6    = "";
                var                    victoryLoopVideo6 = "";
                Action<KMonoBehaviour> victorySequence6  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                RadicalTrip = Add(new ColonyAchievement(id6,
                                                        platformAchievementId6,
                                                        name6,
                                                        description6,
                                                        isVictoryCondition6,
                                                        list6,
                                                        messageTitle6,
                                                        messageBody6,
                                                        videoDataName6,
                                                        victoryLoopVideo6,
                                                        victorySequence6,
                                                        default(EventReference),
                                                        "radical_trip",
                                                        available_EXPANSION1_ONLY,
                                                        "EXPANSION1_ID"));

                var    id7                    = "SweeterThanHoney";
                var    platformAchievementId7 = "SWEETER_THAN_HONEY";
                string name7                  = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SWEETER_THAN_HONEY;
                string description7           = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SWEETER_THAN_HONEY_DESCRIPTION;
                var    isVictoryCondition7    = false;
                var    list7                  = new List<ColonyAchievementRequirement>();
                list7.Add(new HarvestAHiveWithoutBeingStung());
                var                    messageTitle7     = "";
                var                    messageBody7      = "";
                var                    videoDataName7    = "";
                var                    victoryLoopVideo7 = "";
                Action<KMonoBehaviour> victorySequence7  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                SweeterThanHoney = Add(new ColonyAchievement(id7,
                                                             platformAchievementId7,
                                                             name7,
                                                             description7,
                                                             isVictoryCondition7,
                                                             list7,
                                                             messageTitle7,
                                                             messageBody7,
                                                             videoDataName7,
                                                             victoryLoopVideo7,
                                                             victorySequence7,
                                                             default(EventReference),
                                                             "sweeter_than_honey",
                                                             available_EXPANSION1_ONLY,
                                                             "EXPANSION1_ID"));

                var    id8                    = "SurviveInARocket";
                var    platformAchievementId8 = "SURVIVE_IN_A_ROCKET";
                string name8                  = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_IN_A_ROCKET;
                var description8 = string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_IN_A_ROCKET_DESCRIPTION,
                                                 10,
                                                 25);

                var isVictoryCondition8 = false;
                var list8               = new List<ColonyAchievementRequirement>();
                list8.Add(new SurviveARocketWithMinimumMorale(25f, 10));
                var                    messageTitle8     = "";
                var                    messageBody8      = "";
                var                    videoDataName8    = "";
                var                    victoryLoopVideo8 = "";
                Action<KMonoBehaviour> victorySequence8  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                SurviveInARocket = Add(new ColonyAchievement(id8,
                                                             platformAchievementId8,
                                                             name8,
                                                             description8,
                                                             isVictoryCondition8,
                                                             list8,
                                                             messageTitle8,
                                                             messageBody8,
                                                             videoDataName8,
                                                             victoryLoopVideo8,
                                                             victorySequence8,
                                                             default(EventReference),
                                                             "survive_a_rocket",
                                                             available_EXPANSION1_ONLY,
                                                             "EXPANSION1_ID"));

                var    id9 = "RunAReactor";
                var    platformAchievementId9 = "REACTOR_USAGE";
                string name9 = COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACTOR_USAGE;
                var    description9 = string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACTOR_USAGE_DESCRIPTION, 5);
                var    isVictoryCondition9 = false;
                var    list9 = new List<ColonyAchievementRequirement>();
                list9.Add(new RunReactorForXDays(5));
                var                    messageTitle9     = "";
                var                    messageBody9      = "";
                var                    videoDataName9    = "";
                var                    victoryLoopVideo9 = "";
                Action<KMonoBehaviour> victorySequence9  = null;
                available_EXPANSION1_ONLY = DlcManager.AVAILABLE_EXPANSION1_ONLY;
                RunAReactor = Add(new ColonyAchievement(id9,
                                                        platformAchievementId9,
                                                        name9,
                                                        description9,
                                                        isVictoryCondition9,
                                                        list9,
                                                        messageTitle9,
                                                        messageBody9,
                                                        videoDataName9,
                                                        victoryLoopVideo9,
                                                        victorySequence9,
                                                        default(EventReference),
                                                        "thats_rad",
                                                        available_EXPANSION1_ONLY,
                                                        "EXPANSION1_ID"));
            }
        }
    }
}