using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02003B51 RID: 15185
	[DebuggerDisplay("{base.Id}")]
	public class GameplaySeason : Resource
	{
		// Token: 0x0600E9CF RID: 59855 RVA: 0x004C8014 File Offset: 0x004C6214
		public GameplaySeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1) : base(id, null, null)
		{
			this.type = type;
			this.dlcId = dlcId;
			this.period = period;
			this.synchronizedToPeriod = synchronizedToPeriod;
			global::Debug.Assert(period > 0f, "Season " + id + "'s Period cannot be 0 or negative");
			if (randomizedEventStartTime == -1f)
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(--0f * period, 0f * period);
			}
			else
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(-randomizedEventStartTime, randomizedEventStartTime);
				DebugUtil.DevAssert((this.randomizedEventStartTime.max - this.randomizedEventStartTime.min) * 0.4f < period, string.Format("Season {0} randomizedEventStartTime is greater than {1}% of its period.", id, 0.4f), null);
			}
			this.startActive = startActive;
			this.finishAfterNumEvents = finishAfterNumEvents;
			this.minCycle = minCycle;
			this.maxCycle = maxCycle;
			this.events = new List<GameplayEvent>();
			this.numEventsToStartEachPeriod = numEventsToStartEachPeriod;
		}

		// Token: 0x0600E9D0 RID: 59856 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
		}

		// Token: 0x0600E9D1 RID: 59857 RVA: 0x0013C418 File Offset: 0x0013A618
		public virtual float GetSeasonPeriod()
		{
			return this.period;
		}

		// Token: 0x0600E9D2 RID: 59858 RVA: 0x0013C420 File Offset: 0x0013A620
		public GameplaySeason AddEvent(GameplayEvent evt)
		{
			this.events.Add(evt);
			return this;
		}

		// Token: 0x0600E9D3 RID: 59859 RVA: 0x0013C42F File Offset: 0x0013A62F
		public virtual GameplaySeasonInstance Instantiate(int worldId)
		{
			return new GameplaySeasonInstance(this, worldId);
		}

		// Token: 0x0400E53D RID: 58685
		public const float DEFAULT_PERCENTAGE_RANDOMIZED_EVENT_START = 0f;

		// Token: 0x0400E53E RID: 58686
		public const float PERCENTAGE_WARNING = 0.4f;

		// Token: 0x0400E53F RID: 58687
		public const float USE_DEFAULT = -1f;

		// Token: 0x0400E540 RID: 58688
		public const int INFINITE = -1;

		// Token: 0x0400E541 RID: 58689
		public float period;

		// Token: 0x0400E542 RID: 58690
		public bool synchronizedToPeriod;

		// Token: 0x0400E543 RID: 58691
		public MathUtil.MinMax randomizedEventStartTime;

		// Token: 0x0400E544 RID: 58692
		public int finishAfterNumEvents = -1;

		// Token: 0x0400E545 RID: 58693
		public bool startActive;

		// Token: 0x0400E546 RID: 58694
		public int numEventsToStartEachPeriod;

		// Token: 0x0400E547 RID: 58695
		public float minCycle;

		// Token: 0x0400E548 RID: 58696
		public float maxCycle;

		// Token: 0x0400E549 RID: 58697
		public List<GameplayEvent> events;

		// Token: 0x0400E54A RID: 58698
		public GameplaySeason.Type type;

		// Token: 0x0400E54B RID: 58699
		public string dlcId;

		// Token: 0x02003B52 RID: 15186
		public enum Type
		{
			// Token: 0x0400E54D RID: 58701
			World,
			// Token: 0x0400E54E RID: 58702
			Cluster
		}
	}
}
