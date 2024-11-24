using Klei.AI;

public class BladderMonitor : GameStateMachine<BladderMonitor, BladderMonitor.Instance>
{
	public class WantsToPeeStates : State
	{
		public State wanting;

		public State peeing;

		public State InitializeStates(State donePeeingState)
		{
			DefaultState(wanting).ToggleUrge(Db.Get().Urges.Pee).ToggleStateMachine((Instance smi) => new ToiletMonitor.Instance(smi.master));
			wanting.EventTransition(GameHashes.BeginChore, peeing, (Instance smi) => smi.IsPeeing());
			peeing.EventTransition(GameHashes.EndChore, donePeeingState, (Instance smi) => !smi.IsPeeing());
			return this;
		}
	}

	public new class Instance : GameInstance
	{
		private AmountInstance bladder;

		private ChoreDriver choreDriver;

		public Instance(IStateMachineTarget master)
			: base(master)
		{
			bladder = Db.Get().Amounts.Bladder.Lookup(master.gameObject);
			choreDriver = GetComponent<ChoreDriver>();
		}

		public bool NeedsToPee()
		{
			if (base.master.IsNullOrDestroyed())
			{
				return false;
			}
			if (base.master.GetComponent<KPrefabID>().HasTag(GameTags.Asleep))
			{
				return false;
			}
			DebugUtil.DevAssert(bladder != null, "bladder is null");
			return bladder.value >= 100f;
		}

		public bool WantsToPee()
		{
			if (!NeedsToPee())
			{
				if (IsPeeTime())
				{
					return bladder.value >= 40f;
				}
				return false;
			}
			return true;
		}

		public bool IsPeeing()
		{
			if (choreDriver.HasChore())
			{
				return choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Pee);
			}
			return false;
		}

		public bool IsPeeTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
		}
	}

	public State satisfied;

	public WantsToPeeStates urgentwant;

	public WantsToPeeStates breakwant;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = satisfied;
		satisfied.Transition(urgentwant, (Instance smi) => smi.NeedsToPee()).Transition(breakwant, (Instance smi) => smi.WantsToPee());
		urgentwant.InitializeStates(satisfied).ToggleThought(Db.Get().Thoughts.FullBladder).ToggleExpression(Db.Get().Expressions.FullBladder)
			.ToggleStateMachine((Instance smi) => new PeeChoreMonitor.Instance(smi.master))
			.ToggleEffect("FullBladder");
		breakwant.InitializeStates(satisfied);
		breakwant.wanting.Transition(urgentwant, (Instance smi) => smi.NeedsToPee()).EventTransition(GameHashes.ScheduleBlocksChanged, satisfied, (Instance smi) => !smi.WantsToPee());
		breakwant.peeing.ToggleThought(Db.Get().Thoughts.BreakBladder);
	}
}
