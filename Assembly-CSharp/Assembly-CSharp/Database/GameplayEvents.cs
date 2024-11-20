using System;
using Klei.AI;
using STRINGS;
using TUNING;

namespace Database
{
	public class GameplayEvents : ResourceSet<GameplayEvent>
	{
		public GameplayEvents(ResourceSet parent) : base("GameplayEvents", parent)
		{
			this.HatchSpawnEvent = base.Add(new CreatureSpawnEvent());
			this.PartyEvent = base.Add(new PartyEvent());
			this.EclipseEvent = base.Add(new EclipseEvent());
			this.SatelliteCrashEvent = base.Add(new SatelliteCrashEvent());
			this.FoodFightEvent = base.Add(new FoodFightEvent());
			this.BaseGameMeteorEvents();
			this.Expansion1MeteorEvents();
			this.DLCMeteorEvents();
			this.PrickleFlowerBlightEvent = base.Add(new PlantBlightEvent("PrickleFlowerBlightEvent", "PrickleFlower", 3600f, 30f));
			this.CryoFriend = base.Add(new SimpleEvent("CryoFriend", GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.NAME, GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.DESCRIPTION, "cryofriend_kanim", GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.BUTTON, null));
			this.WarpWorldReveal = base.Add(new SimpleEvent("WarpWorldReveal", GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.NAME, GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.DESCRIPTION, "warpworldreveal_kanim", GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.BUTTON, null));
			this.ArtifactReveal = base.Add(new SimpleEvent("ArtifactReveal", GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.NAME, GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.DESCRIPTION, "analyzeartifact_kanim", GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.BUTTON, null));
		}

		private void BaseGameMeteorEvents()
		{
			string id = "MeteorShowerGoldEvent";
			float duration = 3000f;
			float secondsPerMeteor = 0.4f;
			MathUtil.MinMax secondsBombardmentOn = new MathUtil.MinMax(50f, 100f);
			this.MeteorShowerGoldEvent = base.Add(new MeteorShowerEvent(id, duration, secondsPerMeteor, new MathUtil.MinMax(800f, 1200f), secondsBombardmentOn, null, true).AddMeteor(GoldCometConfig.ID, 2f).AddMeteor(RockCometConfig.ID, 0.5f).AddMeteor(DustCometConfig.ID, 5f));
			string id2 = "MeteorShowerCopperEvent";
			float duration2 = 4200f;
			float secondsPerMeteor2 = 5.5f;
			secondsBombardmentOn = new MathUtil.MinMax(100f, 400f);
			this.MeteorShowerCopperEvent = base.Add(new MeteorShowerEvent(id2, duration2, secondsPerMeteor2, new MathUtil.MinMax(300f, 1200f), secondsBombardmentOn, null, true).AddMeteor(CopperCometConfig.ID, 1f).AddMeteor(RockCometConfig.ID, 1f));
			string id3 = "MeteorShowerIronEvent";
			float duration3 = 6000f;
			float secondsPerMeteor3 = 1.25f;
			secondsBombardmentOn = new MathUtil.MinMax(100f, 400f);
			this.MeteorShowerIronEvent = base.Add(new MeteorShowerEvent(id3, duration3, secondsPerMeteor3, new MathUtil.MinMax(300f, 1200f), secondsBombardmentOn, null, true).AddMeteor(IronCometConfig.ID, 1f).AddMeteor(RockCometConfig.ID, 2f).AddMeteor(DustCometConfig.ID, 5f));
		}

		private void Expansion1MeteorEvents()
		{
			string id = "MeteorShowerDustEvent";
			float duration = 9000f;
			float secondsPerMeteor = 1.25f;
			string fullID = ClusterMapMeteorShowerConfig.GetFullID("Regolith");
			MathUtil.MinMax unlimited = new MathUtil.MinMax(100f, 400f);
			this.MeteorShowerDustEvent = base.Add(new MeteorShowerEvent(id, duration, secondsPerMeteor, new MathUtil.MinMax(300f, 1200f), unlimited, fullID, true).AddMeteor(RockCometConfig.ID, 1f).AddMeteor(DustCometConfig.ID, 6f));
			string id2 = "GassyMooteorEvent";
			float duration2 = 15f;
			float secondsPerMeteor2 = 3.125f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Moo");
			unlimited = new MathUtil.MinMax(15f, 15f);
			this.GassyMooteorEvent = base.Add(new MeteorShowerEvent(id2, duration2, secondsPerMeteor2, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, false).AddMeteor(GassyMooCometConfig.ID, 1f));
			string id3 = "MeteorShowerFullereneEvent";
			float duration3 = 30f;
			float secondsPerMeteor3 = 0.5f;
			unlimited = new MathUtil.MinMax(80f, 80f);
			this.MeteorShowerFullereneEvent = base.Add(new MeteorShowerEvent(id3, duration3, secondsPerMeteor3, METEORS.BOMBARDMENT_OFF.NONE, unlimited, null, false).AddMeteor(FullereneCometConfig.ID, 6f).AddMeteor(DustCometConfig.ID, 1f));
			string id4 = "ClusterSnowShower";
			float duration4 = 600f;
			float secondsPerMeteor4 = 3f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Snow");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterSnowShower = base.Add(new MeteorShowerEvent(id4, duration4, secondsPerMeteor4, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(SnowballCometConfig.ID, 2f).AddMeteor(LightDustCometConfig.ID, 1f));
			string id5 = "ClusterIceShower";
			float duration5 = 300f;
			float secondsPerMeteor5 = 1.4f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Ice");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterIceShower = base.Add(new MeteorShowerEvent(id5, duration5, secondsPerMeteor5, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(SnowballCometConfig.ID, 14f).AddMeteor(HardIceCometConfig.ID, 1f));
			string id6 = "ClusterOxyliteShower";
			float duration6 = 300f;
			float secondsPerMeteor6 = 4f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Oxylite");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterOxyliteShower = base.Add(new MeteorShowerEvent(id6, duration6, secondsPerMeteor6, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(OxyliteCometConfig.ID, 4f).AddMeteor(LightDustCometConfig.ID, 4f));
			string id7 = "ClusterBleachStoneShower";
			float duration7 = 300f;
			float secondsPerMeteor7 = 3f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("BleachStone");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterBleachStoneShower = base.Add(new MeteorShowerEvent(id7, duration7, secondsPerMeteor7, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(BleachStoneCometConfig.ID, 13f).AddMeteor(LightDustCometConfig.ID, 3f));
			string id8 = "ClusterBiologicalShower";
			float duration8 = 300f;
			float secondsPerMeteor8 = 3f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Biological");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterBiologicalShower = base.Add(new MeteorShowerEvent(id8, duration8, secondsPerMeteor8, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(SlimeCometConfig.ID, 2f).AddMeteor(AlgaeCometConfig.ID, 1f).AddMeteor(PhosphoricCometConfig.ID, 1f));
			string id9 = "ClusterLightRegolithShower";
			float duration9 = 300f;
			float secondsPerMeteor9 = 4f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("LightDust");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterLightRegolithShower = base.Add(new MeteorShowerEvent(id9, duration9, secondsPerMeteor9, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 1f));
			string id10 = "ClusterRegolithShower";
			float duration10 = 300f;
			float secondsPerMeteor10 = 3.5f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("HeavyDust");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterRegolithShower = base.Add(new MeteorShowerEvent(id10, duration10, secondsPerMeteor10, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(DustCometConfig.ID, 3f).AddMeteor(RockCometConfig.ID, 2f).AddMeteor(LightDustCometConfig.ID, 1f));
			string id11 = "ClusterGoldShower";
			float duration11 = 75f;
			float secondsPerMeteor11 = 1f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Gold");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterGoldShower = base.Add(new MeteorShowerEvent(id11, duration11, secondsPerMeteor11, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(GoldCometConfig.ID, 4f).AddMeteor(RockCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
			string id12 = "ClusterCopperShower";
			float duration12 = 150f;
			float secondsPerMeteor12 = 2.5f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Copper");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterCopperShower = base.Add(new MeteorShowerEvent(id12, duration12, secondsPerMeteor12, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(CopperCometConfig.ID, 2f).AddMeteor(RockCometConfig.ID, 1f));
			string id13 = "ClusterIronShower";
			float duration13 = 300f;
			float secondsPerMeteor13 = 4.5f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Iron");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterIronShower = base.Add(new MeteorShowerEvent(id13, duration13, secondsPerMeteor13, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(IronCometConfig.ID, 4f).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
			string id14 = "ClusterUraniumShower";
			float duration14 = 150f;
			float secondsPerMeteor14 = 4.5f;
			fullID = ClusterMapMeteorShowerConfig.GetFullID("Uranium");
			unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterUraniumShower = base.Add(new MeteorShowerEvent(id14, duration14, secondsPerMeteor14, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(UraniumCometConfig.ID, 2.5f).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
		}

		private void DLCMeteorEvents()
		{
			string id = "ClusterIceAndTreesShower";
			float duration = 300f;
			float secondsPerMeteor = 1.4f;
			string fullID = ClusterMapMeteorShowerConfig.GetFullID("IceAndTrees");
			MathUtil.MinMax unlimited = METEORS.BOMBARDMENT_ON.UNLIMITED;
			this.ClusterIceAndTreesShower = base.Add(new MeteorShowerEvent(id, duration, secondsPerMeteor, METEORS.BOMBARDMENT_OFF.NONE, unlimited, fullID, true).AddMeteor(SpaceTreeSeedCometConfig.ID, 1f).AddMeteor(HardIceCometConfig.ID, 2f).AddMeteor(SnowballCometConfig.ID, 22f));
		}

		private void BonusEvents()
		{
			GameplayEventMinionFilters instance = GameplayEventMinionFilters.Instance;
			GameplayEventPreconditions instance2 = GameplayEventPreconditions.Instance;
			Skills skills = Db.Get().Skills;
			RoomTypes roomTypes = Db.Get().RoomTypes;
			this.BonusDream1 = base.Add(new BonusEvent("BonusDream1", null, 1, false, 0).TriggerOnUseBuilding(1, new string[]
			{
				"Bed",
				"LuxuryBed"
			}).SetRoomConstraints(false, new RoomType[]
			{
				roomTypes.Barracks
			}).AddPrecondition(instance2.BuildingExists("Bed", 2)).AddPriorityBoost(instance2.BuildingExists("Bed", 5), 1).AddPriorityBoost(instance2.BuildingExists("LuxuryBed", 1), 5).TrySpawnEventOnSuccess("BonusDream2"));
			this.BonusDream2 = base.Add(new BonusEvent("BonusDream2", null, 1, false, 10).TriggerOnUseBuilding(10, new string[]
			{
				"Bed",
				"LuxuryBed"
			}).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusDream1, 1)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom))).AddPriorityBoost(instance2.BuildingExists("LuxuryBed", 1), 5).TrySpawnEventOnSuccess("BonusDream3"));
			this.BonusDream3 = base.Add(new BonusEvent("BonusDream3", null, 1, false, 20).TriggerOnUseBuilding(10, new string[]
			{
				"Bed",
				"LuxuryBed"
			}).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusDream2, 1)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom))).TrySpawnEventOnSuccess("BonusDream4"));
			this.BonusDream4 = base.Add(new BonusEvent("BonusDream4", null, 1, false, 30).TriggerOnUseBuilding(10, new string[]
			{
				"LuxuryBed"
			}).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusDream2, 1)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom))));
			this.BonusToilet1 = base.Add(new BonusEvent("BonusToilet1", null, 1, false, 0).TriggerOnUseBuilding(1, new string[]
			{
				"Outhouse",
				"FlushToilet"
			}).AddPrecondition(instance2.Or(instance2.BuildingExists("Outhouse", 2), instance2.BuildingExists("FlushToilet", 1))).AddPrecondition(instance2.Or(instance2.BuildingExists("WashBasin", 2), instance2.BuildingExists("WashSink", 1))).AddPriorityBoost(instance2.BuildingExists("FlushToilet", 1), 1).TrySpawnEventOnSuccess("BonusToilet2"));
			this.BonusToilet2 = base.Add(new BonusEvent("BonusToilet2", null, 1, false, 10).TriggerOnUseBuilding(5, new string[]
			{
				"FlushToilet"
			}).AddPrecondition(instance2.BuildingExists("FlushToilet", 1)).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusToilet1, 1)).AddPriorityBoost(instance2.BuildingExists("FlushToilet", 2), 5).TrySpawnEventOnSuccess("BonusToilet3"));
			this.BonusToilet3 = base.Add(new BonusEvent("BonusToilet3", null, 1, false, 20).TriggerOnUseBuilding(5, new string[]
			{
				"FlushToilet"
			}).SetRoomConstraints(false, new RoomType[]
			{
				roomTypes.Latrine,
				roomTypes.PlumbedBathroom
			}).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusToilet2, 1)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Latrine), instance2.RoomBuilt(roomTypes.PlumbedBathroom))).AddPriorityBoost(instance2.BuildingExists("FlushToilet", 2), 10).TrySpawnEventOnSuccess("BonusToilet4"));
			this.BonusToilet4 = base.Add(new BonusEvent("BonusToilet4", null, 1, false, 30).TriggerOnUseBuilding(5, new string[]
			{
				"FlushToilet"
			}).SetRoomConstraints(false, new RoomType[]
			{
				roomTypes.PlumbedBathroom
			}).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusToilet3, 1)).AddPrecondition(instance2.RoomBuilt(roomTypes.PlumbedBathroom)));
			this.BonusResearch = base.Add(new BonusEvent("BonusResearch", null, 1, false, 0).AddPrecondition(instance2.BuildingExists("ResearchCenter", 1)).AddPrecondition(instance2.ResearchCompleted("FarmingTech")).AddMinionFilter(instance.HasSkillAptitude(skills.Researching1)));
			this.BonusDigging1 = base.Add(new BonusEvent("BonusDigging1", null, 1, true, 0).TriggerOnWorkableComplete(30, new Type[]
			{
				typeof(Diggable)
			}).AddMinionFilter(instance.Or(instance.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Dig, 4), instance.HasSkillAptitude(skills.Mining1))).AddPriorityBoost(instance2.MinionsWithChoreGroupPriorityOrGreater(Db.Get().ChoreGroups.Dig, 1, 4), 1));
			this.BonusStorage = base.Add(new BonusEvent("BonusStorage", null, 1, true, 0).TriggerOnUseBuilding(10, new string[]
			{
				"StorageLocker"
			}).AddMinionFilter(instance.Or(instance.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Hauling, 4), instance.HasSkillAptitude(skills.Hauling1))).AddPrecondition(instance2.BuildingExists("StorageLocker", 1)));
			this.BonusBuilder = base.Add(new BonusEvent("BonusBuilder", null, 1, true, 0).TriggerOnNewBuilding(10, Array.Empty<string>()).AddMinionFilter(instance.Or(instance.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Build, 4), instance.HasSkillAptitude(skills.Building1))));
			this.BonusOxygen = base.Add(new BonusEvent("BonusOxygen", null, 1, false, 0).TriggerOnUseBuilding(1, new string[]
			{
				"MineralDeoxidizer"
			}).AddPrecondition(instance2.BuildingExists("MineralDeoxidizer", 1)).AddPrecondition(instance2.Not(instance2.PastEventCount("BonusAlgae", 1))));
			this.BonusAlgae = base.Add(new BonusEvent("BonusAlgae", "BonusOxygen", 1, false, 0).TriggerOnUseBuilding(1, new string[]
			{
				"AlgaeHabitat"
			}).AddPrecondition(instance2.BuildingExists("AlgaeHabitat", 1)).AddPrecondition(instance2.Not(instance2.PastEventCount("BonusOxygen", 1))));
			this.BonusGenerator = base.Add(new BonusEvent("BonusGenerator", null, 1, false, 0).TriggerOnUseBuilding(1, new string[]
			{
				"ManualGenerator"
			}).AddPrecondition(instance2.BuildingExists("ManualGenerator", 1)));
			this.BonusDoor = base.Add(new BonusEvent("BonusDoor", null, 1, false, 0).TriggerOnUseBuilding(1, new string[]
			{
				"Door"
			}).SetExtraCondition((BonusEvent.GameplayEventData data) => data.building.GetComponent<Door>().RequestedState == Door.ControlState.Locked).AddPrecondition(instance2.RoomBuilt(roomTypes.Barracks)));
			this.BonusHitTheBooks = base.Add(new BonusEvent("BonusHitTheBooks", null, 1, true, 0).TriggerOnWorkableComplete(1, new Type[]
			{
				typeof(ResearchCenter),
				typeof(NuclearResearchCenterWorkable)
			}).AddPrecondition(instance2.BuildingExists("ResearchCenter", 1)).AddMinionFilter(instance.HasSkillAptitude(skills.Researching1)));
			this.BonusLitWorkspace = base.Add(new BonusEvent("BonusLitWorkspace", null, 1, false, 0).TriggerOnWorkableComplete(1, Array.Empty<Type>()).SetExtraCondition((BonusEvent.GameplayEventData data) => data.workable.currentlyLit).AddPrecondition(instance2.CycleRestriction(10f, float.PositiveInfinity)));
			this.BonusTalker = base.Add(new BonusEvent("BonusTalker", null, 1, true, 0).TriggerOnWorkableComplete(3, new Type[]
			{
				typeof(SocialGatheringPointWorkable)
			}).SetExtraCondition((BonusEvent.GameplayEventData data) => (data.workable as SocialGatheringPointWorkable).timesConversed > 0).AddPrecondition(instance2.CycleRestriction(10f, float.PositiveInfinity)));
		}

		private void VerifyEvents()
		{
			foreach (GameplayEvent gameplayEvent in this.resources)
			{
				if (gameplayEvent.animFileName == null)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Gameplay event anim missing: " + gameplayEvent.Id
					});
				}
				if (gameplayEvent is BonusEvent)
				{
					this.VerifyBonusEvent(gameplayEvent as BonusEvent);
				}
			}
		}

		private void VerifyBonusEvent(BonusEvent e)
		{
			StringEntry stringEntry;
			if (!Strings.TryGet("STRINGS.GAMEPLAY_EVENTS.BONUS." + e.Id.ToUpper() + ".NAME", out stringEntry))
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"Event [",
					e.Id,
					"]: STRINGS.GAMEPLAY_EVENTS.BONUS.",
					e.Id.ToUpper(),
					" is missing"
				}));
			}
			Effect effect = Db.Get().effects.TryGet(e.effect);
			if (effect == null)
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"Effect ",
					e.effect,
					"[",
					e.Id,
					"]: Missing from spreadsheet"
				}));
				return;
			}
			if (!Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".NAME", out stringEntry))
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"Effect ",
					e.effect,
					"[",
					e.Id,
					"]: STRINGS.DUPLICANTS.MODIFIERS.",
					effect.Id.ToUpper(),
					".NAME is missing"
				}));
			}
			if (!Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".TOOLTIP", out stringEntry))
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"Effect ",
					e.effect,
					"[",
					e.Id,
					"]: STRINGS.DUPLICANTS.MODIFIERS.",
					effect.Id.ToUpper(),
					".TOOLTIP is missing"
				}));
			}
		}

		public GameplayEvent HatchSpawnEvent;

		public GameplayEvent PartyEvent;

		public GameplayEvent EclipseEvent;

		public GameplayEvent SatelliteCrashEvent;

		public GameplayEvent FoodFightEvent;

		public GameplayEvent PrickleFlowerBlightEvent;

		public GameplayEvent MeteorShowerIronEvent;

		public GameplayEvent MeteorShowerGoldEvent;

		public GameplayEvent MeteorShowerCopperEvent;

		public GameplayEvent MeteorShowerDustEvent;

		public GameplayEvent MeteorShowerFullereneEvent;

		public GameplayEvent GassyMooteorEvent;

		public GameplayEvent ClusterSnowShower;

		public GameplayEvent ClusterIceShower;

		public GameplayEvent ClusterBiologicalShower;

		public GameplayEvent ClusterLightRegolithShower;

		public GameplayEvent ClusterRegolithShower;

		public GameplayEvent ClusterGoldShower;

		public GameplayEvent ClusterCopperShower;

		public GameplayEvent ClusterIronShower;

		public GameplayEvent ClusterUraniumShower;

		public GameplayEvent ClusterOxyliteShower;

		public GameplayEvent ClusterBleachStoneShower;

		public GameplayEvent ClusterIceAndTreesShower;

		public GameplayEvent BonusDream1;

		public GameplayEvent BonusDream2;

		public GameplayEvent BonusDream3;

		public GameplayEvent BonusDream4;

		public GameplayEvent BonusToilet1;

		public GameplayEvent BonusToilet2;

		public GameplayEvent BonusToilet3;

		public GameplayEvent BonusToilet4;

		public GameplayEvent BonusResearch;

		public GameplayEvent BonusDigging1;

		public GameplayEvent BonusStorage;

		public GameplayEvent BonusBuilder;

		public GameplayEvent BonusOxygen;

		public GameplayEvent BonusAlgae;

		public GameplayEvent BonusGenerator;

		public GameplayEvent BonusDoor;

		public GameplayEvent BonusHitTheBooks;

		public GameplayEvent BonusLitWorkspace;

		public GameplayEvent BonusTalker;

		public GameplayEvent CryoFriend;

		public GameplayEvent WarpWorldReveal;

		public GameplayEvent ArtifactReveal;
	}
}
