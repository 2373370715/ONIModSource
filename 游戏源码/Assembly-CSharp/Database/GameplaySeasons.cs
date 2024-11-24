using Klei.AI;

namespace Database;

public class GameplaySeasons : ResourceSet<GameplaySeason>
{
	public GameplaySeason NaturalRandomEvents;

	public GameplaySeason DupeRandomEvents;

	public GameplaySeason PrickleCropSeason;

	public GameplaySeason BonusEvents;

	public GameplaySeason MeteorShowers;

	public GameplaySeason TemporalTearMeteorShowers;

	public GameplaySeason SpacedOutStyleStartMeteorShowers;

	public GameplaySeason SpacedOutStyleRocketMeteorShowers;

	public GameplaySeason SpacedOutStyleWarpMeteorShowers;

	public GameplaySeason ClassicStyleStartMeteorShowers;

	public GameplaySeason ClassicStyleWarpMeteorShowers;

	public GameplaySeason TundraMoonletMeteorShowers;

	public GameplaySeason MarshyMoonletMeteorShowers;

	public GameplaySeason NiobiumMoonletMeteorShowers;

	public GameplaySeason WaterMoonletMeteorShowers;

	public GameplaySeason GassyMooteorShowers;

	public GameplaySeason RegolithMoonMeteorShowers;

	public GameplaySeason MiniMetallicSwampyMeteorShowers;

	public GameplaySeason MiniForestFrozenMeteorShowers;

	public GameplaySeason MiniBadlandsMeteorShowers;

	public GameplaySeason MiniFlippedMeteorShowers;

	public GameplaySeason MiniRadioactiveOceanMeteorShowers;

	public GameplaySeason CeresMeteorShowers;

	public GameplaySeasons(ResourceSet parent)
		: base("GameplaySeasons", parent)
	{
		VanillaSeasons();
		Expansion1Seasons();
		DLCSeasons();
		UnusedSeasons();
	}

	private void VanillaSeasons()
	{
		MeteorShowers = Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, "", 14f, synchronizedToPeriod: false, -1f, startActive: true).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
	}

	private void Expansion1Seasons()
	{
		RegolithMoonMeteorShowers = Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
		TemporalTearMeteorShowers = Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 1f, synchronizedToPeriod: false, 0f, startActive: false, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: false).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
		GassyMooteorShowers = Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: false, 6000f).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
		SpacedOutStyleStartMeteorShowers = Add(new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f));
		SpacedOutStyleRocketMeteorShowers = Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
		SpacedOutStyleWarpMeteorShowers = Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
		ClassicStyleStartMeteorShowers = Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
		ClassicStyleWarpMeteorShowers = Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
		TundraMoonletMeteorShowers = Add(new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f));
		MarshyMoonletMeteorShowers = Add(new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f));
		NiobiumMoonletMeteorShowers = Add(new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f));
		WaterMoonletMeteorShowers = Add(new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f));
		MiniMetallicSwampyMeteorShowers = Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
		MiniForestFrozenMeteorShowers = Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
		MiniBadlandsMeteorShowers = Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
		MiniFlippedMeteorShowers = Add(new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f));
		MiniRadioactiveOceanMeteorShowers = Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
	}

	private void DLCSeasons()
	{
		CeresMeteorShowers = Add(new MeteorShowerSeason("CeresMeteorShowers", GameplaySeason.Type.World, "DLC2_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 10f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceAndTreesShower));
	}

	private void UnusedSeasons()
	{
	}
}
