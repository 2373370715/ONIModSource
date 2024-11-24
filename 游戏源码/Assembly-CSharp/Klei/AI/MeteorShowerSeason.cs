using System;
using System.Diagnostics;
using Klei.CustomSettings;

namespace Klei.AI
{
	// Token: 0x02003B5C RID: 15196
	[DebuggerDisplay("{base.Id}")]
	public class MeteorShowerSeason : GameplaySeason
	{
		// Token: 0x0600EA09 RID: 59913 RVA: 0x004C8E28 File Offset: 0x004C7028
		public MeteorShowerSeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1, bool affectedByDifficultySettings = true, float clusterTravelDuration = -1f) : base(id, type, dlcId, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod)
		{
			this.affectedByDifficultySettings = affectedByDifficultySettings;
			this.clusterTravelDuration = clusterTravelDuration;
		}

		// Token: 0x0600EA0A RID: 59914 RVA: 0x0013C65E File Offset: 0x0013A85E
		public override void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
			(generic_smi as MeteorShowerEvent.StatesInstance).clusterTravelDuration = this.clusterTravelDuration;
		}

		// Token: 0x0600EA0B RID: 59915 RVA: 0x004C8E70 File Offset: 0x004C7070
		public override float GetSeasonPeriod()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
			float num = base.GetSeasonPeriod();
			if (this.affectedByDifficultySettings && currentQualitySetting != null)
			{
				string id = currentQualitySetting.id;
				if (!(id == "Infrequent"))
				{
					if (!(id == "Intense"))
					{
						if (id == "Doomed")
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				else
				{
					num *= 2f;
				}
			}
			return num;
		}

		// Token: 0x0400E57C RID: 58748
		public bool affectedByDifficultySettings = true;

		// Token: 0x0400E57D RID: 58749
		public float clusterTravelDuration = -1f;
	}
}
