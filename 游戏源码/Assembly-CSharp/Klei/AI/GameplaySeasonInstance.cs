using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B53 RID: 15187
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GameplaySeasonInstance : ISaveLoadable
	{
		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x0600E9D4 RID: 59860 RVA: 0x0013C438 File Offset: 0x0013A638
		public float NextEventTime
		{
			get
			{
				return this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x0600E9D5 RID: 59861 RVA: 0x0013C447 File Offset: 0x0013A647
		public GameplaySeason Season
		{
			get
			{
				if (this._season == null)
				{
					this._season = Db.Get().GameplaySeasons.TryGet(this.seasonId);
				}
				return this._season;
			}
		}

		// Token: 0x0600E9D6 RID: 59862 RVA: 0x004C8114 File Offset: 0x004C6314
		public GameplaySeasonInstance(GameplaySeason season, int worldId)
		{
			this.seasonId = season.Id;
			this.worldId = worldId;
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			if (season.synchronizedToPeriod)
			{
				float seasonPeriod = this.Season.GetSeasonPeriod();
				this.nextPeriodTime = (Mathf.Floor(currentTimeInCycles / seasonPeriod) + 1f) * seasonPeriod;
			}
			else
			{
				this.nextPeriodTime = currentTimeInCycles;
			}
			this.CalculateNextEventTime();
		}

		// Token: 0x0600E9D7 RID: 59863 RVA: 0x004C817C File Offset: 0x004C637C
		private void CalculateNextEventTime()
		{
			float seasonPeriod = this.Season.GetSeasonPeriod();
			this.randomizedNextTime = UnityEngine.Random.Range(this.Season.randomizedEventStartTime.min, this.Season.randomizedEventStartTime.max);
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			float num = this.nextPeriodTime + this.randomizedNextTime;
			while (num < currentTimeInCycles || num < this.Season.minCycle)
			{
				this.nextPeriodTime += seasonPeriod;
				num = this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x0600E9D8 RID: 59864 RVA: 0x004C8204 File Offset: 0x004C6404
		public bool StartEvent(bool ignorePreconditions = false)
		{
			bool result = false;
			this.CalculateNextEventTime();
			this.numStartEvents++;
			List<GameplayEvent> list;
			if (!ignorePreconditions)
			{
				list = (from x in this.Season.events
				where x.IsAllowed()
				select x).ToList<GameplayEvent>();
			}
			else
			{
				list = this.Season.events;
			}
			List<GameplayEvent> list2 = list;
			if (list2.Count > 0)
			{
				list2.ForEach(delegate(GameplayEvent x)
				{
					x.CalculatePriority();
				});
				list2.Sort();
				int maxExclusive = Mathf.Min(list2.Count, 5);
				GameplayEvent eventType = list2[UnityEngine.Random.Range(0, maxExclusive)];
				GameplayEventManager.Instance.StartNewEvent(eventType, this.worldId, new Action<StateMachine.Instance>(this.Season.AdditionalEventInstanceSetup));
				result = true;
			}
			this.allEventWillNotRunAgain = true;
			using (List<GameplayEvent>.Enumerator enumerator = this.Season.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.WillNeverRunAgain())
					{
						this.allEventWillNotRunAgain = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600E9D9 RID: 59865 RVA: 0x004C8340 File Offset: 0x004C6540
		public bool ShouldGenerateEvents()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(this.worldId);
			if (!world.IsDupeVisited && !world.IsRoverVisted)
			{
				return false;
			}
			if ((this.Season.finishAfterNumEvents != -1 && this.numStartEvents >= this.Season.finishAfterNumEvents) || this.allEventWillNotRunAgain)
			{
				return false;
			}
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			return currentTimeInCycles > this.Season.minCycle && currentTimeInCycles < this.Season.maxCycle;
		}

		// Token: 0x0400E54F RID: 58703
		public const int LIMIT_SELECTION = 5;

		// Token: 0x0400E550 RID: 58704
		[Serialize]
		public int numStartEvents;

		// Token: 0x0400E551 RID: 58705
		[Serialize]
		public int worldId;

		// Token: 0x0400E552 RID: 58706
		[Serialize]
		private readonly string seasonId;

		// Token: 0x0400E553 RID: 58707
		[Serialize]
		private float nextPeriodTime;

		// Token: 0x0400E554 RID: 58708
		[Serialize]
		private float randomizedNextTime;

		// Token: 0x0400E555 RID: 58709
		private bool allEventWillNotRunAgain;

		// Token: 0x0400E556 RID: 58710
		private GameplaySeason _season;
	}
}
