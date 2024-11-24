using System;
using Klei.AI;

namespace Database
{
	// Token: 0x0200213F RID: 8511
	public class GameplaySeasons : ResourceSet<GameplaySeason>
	{
		// Token: 0x0600B560 RID: 46432 RVA: 0x001150A1 File Offset: 0x001132A1
		public GameplaySeasons(ResourceSet parent) : base("GameplaySeasons", parent)
		{
			this.VanillaSeasons();
			this.Expansion1Seasons();
			this.DLCSeasons();
			this.UnusedSeasons();
		}

		// Token: 0x0600B561 RID: 46433 RVA: 0x00450190 File Offset: 0x0044E390
		private void VanillaSeasons()
		{
			this.MeteorShowers = base.Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, "", 14f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
		}

		// Token: 0x0600B562 RID: 46434 RVA: 0x00450214 File Offset: 0x0044E414
		private void Expansion1Seasons()
		{
			this.RegolithMoonMeteorShowers = base.Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.TemporalTearMeteorShowers = base.Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 1f, false, 0f, false, -1, 0f, float.PositiveInfinity, 1, false, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
			this.GassyMooteorShowers = base.Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, false, 6000f).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
			this.SpacedOutStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.SpacedOutStyleRocketMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.SpacedOutStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
			this.TundraMoonletMeteorShowers = base.Add(new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MarshyMoonletMeteorShowers = base.Add(new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.NiobiumMoonletMeteorShowers = base.Add(new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.WaterMoonletMeteorShowers = base.Add(new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniMetallicSwampyMeteorShowers = base.Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
			this.MiniForestFrozenMeteorShowers = base.Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.MiniBadlandsMeteorShowers = base.Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.MiniFlippedMeteorShowers = base.Add(new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniRadioactiveOceanMeteorShowers = base.Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
		}

		// Token: 0x0600B563 RID: 46435 RVA: 0x00450778 File Offset: 0x0044E978
		private void DLCSeasons()
		{
			this.CeresMeteorShowers = base.Add(new MeteorShowerSeason("CeresMeteorShowers", GameplaySeason.Type.World, "DLC2_ID", 20f, false, -1f, true, -1, 10f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceAndTreesShower));
			this.MiniCeresStartShowers = base.Add(new MeteorShowerSeason("MiniCeresStartShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower).AddEvent(Db.Get().GameplayEvents.ClusterSnowShower));
		}

		// Token: 0x0600B564 RID: 46436 RVA: 0x000A5E40 File Offset: 0x000A4040
		private void UnusedSeasons()
		{
		}

		// Token: 0x040092E9 RID: 37609
		public GameplaySeason NaturalRandomEvents;

		// Token: 0x040092EA RID: 37610
		public GameplaySeason DupeRandomEvents;

		// Token: 0x040092EB RID: 37611
		public GameplaySeason PrickleCropSeason;

		// Token: 0x040092EC RID: 37612
		public GameplaySeason BonusEvents;

		// Token: 0x040092ED RID: 37613
		public GameplaySeason MeteorShowers;

		// Token: 0x040092EE RID: 37614
		public GameplaySeason TemporalTearMeteorShowers;

		// Token: 0x040092EF RID: 37615
		public GameplaySeason SpacedOutStyleStartMeteorShowers;

		// Token: 0x040092F0 RID: 37616
		public GameplaySeason SpacedOutStyleRocketMeteorShowers;

		// Token: 0x040092F1 RID: 37617
		public GameplaySeason SpacedOutStyleWarpMeteorShowers;

		// Token: 0x040092F2 RID: 37618
		public GameplaySeason ClassicStyleStartMeteorShowers;

		// Token: 0x040092F3 RID: 37619
		public GameplaySeason ClassicStyleWarpMeteorShowers;

		// Token: 0x040092F4 RID: 37620
		public GameplaySeason TundraMoonletMeteorShowers;

		// Token: 0x040092F5 RID: 37621
		public GameplaySeason MarshyMoonletMeteorShowers;

		// Token: 0x040092F6 RID: 37622
		public GameplaySeason NiobiumMoonletMeteorShowers;

		// Token: 0x040092F7 RID: 37623
		public GameplaySeason WaterMoonletMeteorShowers;

		// Token: 0x040092F8 RID: 37624
		public GameplaySeason GassyMooteorShowers;

		// Token: 0x040092F9 RID: 37625
		public GameplaySeason RegolithMoonMeteorShowers;

		// Token: 0x040092FA RID: 37626
		public GameplaySeason MiniMetallicSwampyMeteorShowers;

		// Token: 0x040092FB RID: 37627
		public GameplaySeason MiniForestFrozenMeteorShowers;

		// Token: 0x040092FC RID: 37628
		public GameplaySeason MiniBadlandsMeteorShowers;

		// Token: 0x040092FD RID: 37629
		public GameplaySeason MiniFlippedMeteorShowers;

		// Token: 0x040092FE RID: 37630
		public GameplaySeason MiniRadioactiveOceanMeteorShowers;

		// Token: 0x040092FF RID: 37631
		public GameplaySeason MiniCeresStartShowers;

		// Token: 0x04009300 RID: 37632
		public GameplaySeason CeresMeteorShowers;
	}
}
