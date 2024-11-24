using Klei.AI;
using STRINGS;
using TUNING;

namespace Database;

public class GameplayEvents : ResourceSet<GameplayEvent>
{
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

	public GameplayEvents(ResourceSet parent)
		: base("GameplayEvents", parent)
	{
		HatchSpawnEvent = Add(new CreatureSpawnEvent());
		PartyEvent = Add(new PartyEvent());
		EclipseEvent = Add(new EclipseEvent());
		SatelliteCrashEvent = Add(new SatelliteCrashEvent());
		FoodFightEvent = Add(new FoodFightEvent());
		BaseGameMeteorEvents();
		Expansion1MeteorEvents();
		DLCMeteorEvents();
		PrickleFlowerBlightEvent = Add(new PlantBlightEvent("PrickleFlowerBlightEvent", "PrickleFlower", 3600f, 30f));
		CryoFriend = Add(new SimpleEvent("CryoFriend", GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.NAME, GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.DESCRIPTION, "cryofriend_kanim", GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.BUTTON));
		WarpWorldReveal = Add(new SimpleEvent("WarpWorldReveal", GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.NAME, GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.DESCRIPTION, "warpworldreveal_kanim", GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.BUTTON));
		ArtifactReveal = Add(new SimpleEvent("ArtifactReveal", GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.NAME, GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.DESCRIPTION, "analyzeartifact_kanim", GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.BUTTON));
	}

	private void BaseGameMeteorEvents()
	{
		MeteorShowerGoldEvent = Add(new MeteorShowerEvent("MeteorShowerGoldEvent", 3000f, 0.4f, secondsBombardmentOn: new MathUtil.MinMax(50f, 100f), secondsBombardmentOff: new MathUtil.MinMax(800f, 1200f)).AddMeteor(GoldCometConfig.ID, 2f).AddMeteor(RockCometConfig.ID, 0.5f).AddMeteor(DustCometConfig.ID, 5f));
		MeteorShowerCopperEvent = Add(new MeteorShowerEvent("MeteorShowerCopperEvent", 4200f, 5.5f, secondsBombardmentOn: new MathUtil.MinMax(100f, 400f), secondsBombardmentOff: new MathUtil.MinMax(300f, 1200f)).AddMeteor(CopperCometConfig.ID, 1f).AddMeteor(RockCometConfig.ID, 1f));
		MeteorShowerIronEvent = Add(new MeteorShowerEvent("MeteorShowerIronEvent", 6000f, 1.25f, secondsBombardmentOn: new MathUtil.MinMax(100f, 400f), secondsBombardmentOff: new MathUtil.MinMax(300f, 1200f)).AddMeteor(IronCometConfig.ID, 1f).AddMeteor(RockCometConfig.ID, 2f).AddMeteor(DustCometConfig.ID, 5f));
	}

	private void Expansion1MeteorEvents()
	{
		MeteorShowerDustEvent = Add(new MeteorShowerEvent("MeteorShowerDustEvent", 9000f, 1.25f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Regolith"), secondsBombardmentOn: new MathUtil.MinMax(100f, 400f), secondsBombardmentOff: new MathUtil.MinMax(300f, 1200f)).AddMeteor(RockCometConfig.ID, 1f).AddMeteor(DustCometConfig.ID, 6f));
		GassyMooteorEvent = Add(new MeteorShowerEvent("GassyMooteorEvent", 15f, 3.125f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Moo"), secondsBombardmentOn: new MathUtil.MinMax(15f, 15f), secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE, affectedByDifficulty: false).AddMeteor(GassyMooCometConfig.ID, 1f));
		MeteorShowerFullereneEvent = Add(new MeteorShowerEvent("MeteorShowerFullereneEvent", 30f, 0.5f, secondsBombardmentOn: new MathUtil.MinMax(80f, 80f), secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE, clusterMapMeteorShowerID: null, affectedByDifficulty: false).AddMeteor(FullereneCometConfig.ID, 6f).AddMeteor(DustCometConfig.ID, 1f));
		ClusterSnowShower = Add(new MeteorShowerEvent("ClusterSnowShower", 600f, 3f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Snow"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(SnowballCometConfig.ID, 2f).AddMeteor(LightDustCometConfig.ID, 1f));
		ClusterIceShower = Add(new MeteorShowerEvent("ClusterIceShower", 300f, 1.4f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Ice"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(SnowballCometConfig.ID, 14f).AddMeteor(HardIceCometConfig.ID, 1f));
		ClusterOxyliteShower = Add(new MeteorShowerEvent("ClusterOxyliteShower", 300f, 4f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Oxylite"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(OxyliteCometConfig.ID, 4f).AddMeteor(LightDustCometConfig.ID, 4f));
		ClusterBleachStoneShower = Add(new MeteorShowerEvent("ClusterBleachStoneShower", 300f, 3f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("BleachStone"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(BleachStoneCometConfig.ID, 13f).AddMeteor(LightDustCometConfig.ID, 3f));
		ClusterBiologicalShower = Add(new MeteorShowerEvent("ClusterBiologicalShower", 300f, 3f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Biological"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(SlimeCometConfig.ID, 2f).AddMeteor(AlgaeCometConfig.ID, 1f).AddMeteor(PhosphoricCometConfig.ID, 1f));
		ClusterLightRegolithShower = Add(new MeteorShowerEvent("ClusterLightRegolithShower", 300f, 4f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("LightDust"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 1f));
		ClusterRegolithShower = Add(new MeteorShowerEvent("ClusterRegolithShower", 300f, 3.5f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("HeavyDust"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(DustCometConfig.ID, 3f).AddMeteor(RockCometConfig.ID, 2f).AddMeteor(LightDustCometConfig.ID, 1f));
		ClusterGoldShower = Add(new MeteorShowerEvent("ClusterGoldShower", 75f, 1f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Gold"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(GoldCometConfig.ID, 4f).AddMeteor(RockCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
		ClusterCopperShower = Add(new MeteorShowerEvent("ClusterCopperShower", 150f, 2.5f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Copper"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(CopperCometConfig.ID, 2f).AddMeteor(RockCometConfig.ID, 1f));
		ClusterIronShower = Add(new MeteorShowerEvent("ClusterIronShower", 300f, 4.5f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Iron"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(IronCometConfig.ID, 4f).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
		ClusterUraniumShower = Add(new MeteorShowerEvent("ClusterUraniumShower", 150f, 4.5f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("Uranium"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(UraniumCometConfig.ID, 2.5f).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
	}

	private void DLCMeteorEvents()
	{
		ClusterIceAndTreesShower = Add(new MeteorShowerEvent("ClusterIceAndTreesShower", 300f, 1.4f, clusterMapMeteorShowerID: ClusterMapMeteorShowerConfig.GetFullID("IceAndTrees"), secondsBombardmentOn: METEORS.BOMBARDMENT_ON.UNLIMITED, secondsBombardmentOff: METEORS.BOMBARDMENT_OFF.NONE).AddMeteor(SpaceTreeSeedCometConfig.ID, 1f).AddMeteor(HardIceCometConfig.ID, 2f).AddMeteor(SnowballCometConfig.ID, 22f));
	}

	private void BonusEvents()
	{
		GameplayEventMinionFilters instance = GameplayEventMinionFilters.Instance;
		GameplayEventPreconditions instance2 = GameplayEventPreconditions.Instance;
		Skills skills = Db.Get().Skills;
		RoomTypes roomTypes = Db.Get().RoomTypes;
		BonusDream1 = Add(new BonusEvent("BonusDream1").TriggerOnUseBuilding(1, "Bed", "LuxuryBed").SetRoomConstraints(false, roomTypes.Barracks).AddPrecondition(instance2.BuildingExists("Bed", 2))
			.AddPriorityBoost(instance2.BuildingExists("Bed", 5), 1)
			.AddPriorityBoost(instance2.BuildingExists("LuxuryBed"), 5)
			.TrySpawnEventOnSuccess("BonusDream2"));
		BonusDream2 = Add(new BonusEvent("BonusDream2", null, 1, preSelectMinion: false, 10).TriggerOnUseBuilding(10, "Bed", "LuxuryBed").AddPrecondition(instance2.PastEventCountAndNotActive(BonusDream1)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom)))
			.AddPriorityBoost(instance2.BuildingExists("LuxuryBed"), 5)
			.TrySpawnEventOnSuccess("BonusDream3"));
		BonusDream3 = Add(new BonusEvent("BonusDream3", null, 1, preSelectMinion: false, 20).TriggerOnUseBuilding(10, "Bed", "LuxuryBed").AddPrecondition(instance2.PastEventCountAndNotActive(BonusDream2)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom)))
			.TrySpawnEventOnSuccess("BonusDream4"));
		BonusDream4 = Add(new BonusEvent("BonusDream4", null, 1, preSelectMinion: false, 30).TriggerOnUseBuilding(10, "LuxuryBed").AddPrecondition(instance2.PastEventCountAndNotActive(BonusDream2)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom))));
		BonusToilet1 = Add(new BonusEvent("BonusToilet1").TriggerOnUseBuilding(1, "Outhouse", "FlushToilet").AddPrecondition(instance2.Or(instance2.BuildingExists("Outhouse", 2), instance2.BuildingExists("FlushToilet"))).AddPrecondition(instance2.Or(instance2.BuildingExists("WashBasin", 2), instance2.BuildingExists("WashSink")))
			.AddPriorityBoost(instance2.BuildingExists("FlushToilet"), 1)
			.TrySpawnEventOnSuccess("BonusToilet2"));
		BonusToilet2 = Add(new BonusEvent("BonusToilet2", null, 1, preSelectMinion: false, 10).TriggerOnUseBuilding(5, "FlushToilet").AddPrecondition(instance2.BuildingExists("FlushToilet")).AddPrecondition(instance2.PastEventCountAndNotActive(BonusToilet1))
			.AddPriorityBoost(instance2.BuildingExists("FlushToilet", 2), 5)
			.TrySpawnEventOnSuccess("BonusToilet3"));
		BonusToilet3 = Add(new BonusEvent("BonusToilet3", null, 1, preSelectMinion: false, 20).TriggerOnUseBuilding(5, "FlushToilet").SetRoomConstraints(false, roomTypes.Latrine, roomTypes.PlumbedBathroom).AddPrecondition(instance2.PastEventCountAndNotActive(BonusToilet2))
			.AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Latrine), instance2.RoomBuilt(roomTypes.PlumbedBathroom)))
			.AddPriorityBoost(instance2.BuildingExists("FlushToilet", 2), 10)
			.TrySpawnEventOnSuccess("BonusToilet4"));
		BonusToilet4 = Add(new BonusEvent("BonusToilet4", null, 1, preSelectMinion: false, 30).TriggerOnUseBuilding(5, "FlushToilet").SetRoomConstraints(false, roomTypes.PlumbedBathroom).AddPrecondition(instance2.PastEventCountAndNotActive(BonusToilet3))
			.AddPrecondition(instance2.RoomBuilt(roomTypes.PlumbedBathroom)));
		BonusResearch = Add(new BonusEvent("BonusResearch").AddPrecondition(instance2.BuildingExists("ResearchCenter")).AddPrecondition(instance2.ResearchCompleted("FarmingTech")).AddMinionFilter(instance.HasSkillAptitude(skills.Researching1)));
		BonusDigging1 = Add(new BonusEvent("BonusDigging1", null, 1, preSelectMinion: true).TriggerOnWorkableComplete(30, typeof(Diggable)).AddMinionFilter(instance.Or(instance.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Dig, 4), instance.HasSkillAptitude(skills.Mining1))).AddPriorityBoost(instance2.MinionsWithChoreGroupPriorityOrGreater(Db.Get().ChoreGroups.Dig, 1, 4), 1));
		BonusStorage = Add(new BonusEvent("BonusStorage", null, 1, preSelectMinion: true).TriggerOnUseBuilding(10, "StorageLocker").AddMinionFilter(instance.Or(instance.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Hauling, 4), instance.HasSkillAptitude(skills.Hauling1))).AddPrecondition(instance2.BuildingExists("StorageLocker")));
		BonusBuilder = Add(new BonusEvent("BonusBuilder", null, 1, preSelectMinion: true).TriggerOnNewBuilding(10).AddMinionFilter(instance.Or(instance.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Build, 4), instance.HasSkillAptitude(skills.Building1))));
		BonusOxygen = Add(new BonusEvent("BonusOxygen").TriggerOnUseBuilding(1, "MineralDeoxidizer").AddPrecondition(instance2.BuildingExists("MineralDeoxidizer")).AddPrecondition(instance2.Not(instance2.PastEventCount("BonusAlgae"))));
		BonusAlgae = Add(new BonusEvent("BonusAlgae", "BonusOxygen").TriggerOnUseBuilding(1, "AlgaeHabitat").AddPrecondition(instance2.BuildingExists("AlgaeHabitat")).AddPrecondition(instance2.Not(instance2.PastEventCount("BonusOxygen"))));
		BonusGenerator = Add(new BonusEvent("BonusGenerator").TriggerOnUseBuilding(1, "ManualGenerator").AddPrecondition(instance2.BuildingExists("ManualGenerator")));
		BonusDoor = Add(new BonusEvent("BonusDoor").TriggerOnUseBuilding(1, "Door").SetExtraCondition((BonusEvent.GameplayEventData data) => data.building.GetComponent<Door>().RequestedState == Door.ControlState.Locked).AddPrecondition(instance2.RoomBuilt(roomTypes.Barracks)));
		BonusHitTheBooks = Add(new BonusEvent("BonusHitTheBooks", null, 1, preSelectMinion: true).TriggerOnWorkableComplete(1, typeof(ResearchCenter), typeof(NuclearResearchCenterWorkable)).AddPrecondition(instance2.BuildingExists("ResearchCenter")).AddMinionFilter(instance.HasSkillAptitude(skills.Researching1)));
		BonusLitWorkspace = Add(new BonusEvent("BonusLitWorkspace").TriggerOnWorkableComplete(1).SetExtraCondition((BonusEvent.GameplayEventData data) => data.workable.currentlyLit).AddPrecondition(instance2.CycleRestriction(10f)));
		BonusTalker = Add(new BonusEvent("BonusTalker", null, 1, preSelectMinion: true).TriggerOnWorkableComplete(3, typeof(SocialGatheringPointWorkable)).SetExtraCondition((BonusEvent.GameplayEventData data) => (data.workable as SocialGatheringPointWorkable).timesConversed > 0).AddPrecondition(instance2.CycleRestriction(10f)));
	}

	private void VerifyEvents()
	{
		foreach (GameplayEvent resource in resources)
		{
			if (resource.animFileName == null)
			{
				DebugUtil.LogWarningArgs("Gameplay event anim missing: " + resource.Id);
			}
			if (resource is BonusEvent)
			{
				VerifyBonusEvent(resource as BonusEvent);
			}
		}
	}

	private void VerifyBonusEvent(BonusEvent e)
	{
		if (!Strings.TryGet("STRINGS.GAMEPLAY_EVENTS.BONUS." + e.Id.ToUpper() + ".NAME", out var result))
		{
			DebugUtil.DevLogError("Event [" + e.Id + "]: STRINGS.GAMEPLAY_EVENTS.BONUS." + e.Id.ToUpper() + " is missing");
		}
		Effect effect = Db.Get().effects.TryGet(e.effect);
		if (effect == null)
		{
			DebugUtil.DevLogError("Effect " + e.effect + "[" + e.Id + "]: Missing from spreadsheet");
			return;
		}
		if (!Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".NAME", out result))
		{
			DebugUtil.DevLogError("Effect " + e.effect + "[" + e.Id + "]: STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".NAME is missing");
		}
		if (!Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".TOOLTIP", out result))
		{
			DebugUtil.DevLogError("Effect " + e.effect + "[" + e.Id + "]: STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".TOOLTIP is missing");
		}
	}
}
