using System;
using Klei.AI;

// Token: 0x02001525 RID: 5413
public class BladderMonitor : GameStateMachine<BladderMonitor, BladderMonitor.Instance>
{
	// Token: 0x060070F3 RID: 28915 RVA: 0x002F955C File Offset: 0x002F775C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).Transition(this.breakwant, (BladderMonitor.Instance smi) => smi.WantsToPee(), UpdateRate.SIM_200ms);
		this.urgentwant.InitializeStates(this.satisfied).ToggleThought(Db.Get().Thoughts.FullBladder, null).ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleStateMachine((BladderMonitor.Instance smi) => new PeeChoreMonitor.Instance(smi.master)).ToggleEffect("FullBladder");
		this.breakwant.InitializeStates(this.satisfied);
		this.breakwant.wanting.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).EventTransition(GameHashes.ScheduleBlocksChanged, this.satisfied, (BladderMonitor.Instance smi) => !smi.WantsToPee());
		this.breakwant.peeing.ToggleThought(Db.Get().Thoughts.BreakBladder, null);
	}

	// Token: 0x0400545C RID: 21596
	public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400545D RID: 21597
	public BladderMonitor.WantsToPeeStates urgentwant;

	// Token: 0x0400545E RID: 21598
	public BladderMonitor.WantsToPeeStates breakwant;

	// Token: 0x02001526 RID: 5414
	public class WantsToPeeStates : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x060070F5 RID: 28917 RVA: 0x002F96D4 File Offset: 0x002F78D4
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State InitializeStates(GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State donePeeingState)
		{
			base.DefaultState(this.wanting).ToggleUrge(Db.Get().Urges.Pee).ToggleStateMachine((BladderMonitor.Instance smi) => new ToiletMonitor.Instance(smi.master));
			this.wanting.EventTransition(GameHashes.BeginChore, this.peeing, (BladderMonitor.Instance smi) => smi.IsPeeing());
			this.peeing.EventTransition(GameHashes.EndChore, donePeeingState, (BladderMonitor.Instance smi) => !smi.IsPeeing());
			return this;
		}

		// Token: 0x0400545F RID: 21599
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State wanting;

		// Token: 0x04005460 RID: 21600
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State peeing;
	}

	// Token: 0x02001528 RID: 5416
	public new class Instance : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060070FC RID: 28924 RVA: 0x000E9E8B File Offset: 0x000E808B
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(master.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}

		// Token: 0x060070FD RID: 28925 RVA: 0x002F9790 File Offset: 0x002F7990
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
			DebugUtil.DevAssert(this.bladder != null, "bladder is null", null);
			return this.bladder.value >= 100f;
		}

		// Token: 0x060070FE RID: 28926 RVA: 0x000E9EC0 File Offset: 0x000E80C0
		public bool WantsToPee()
		{
			return this.NeedsToPee() || (this.IsPeeTime() && this.bladder.value >= 40f);
		}

		// Token: 0x060070FF RID: 28927 RVA: 0x000E9EEB File Offset: 0x000E80EB
		public bool IsPeeing()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Pee);
		}

		// Token: 0x06007100 RID: 28928 RVA: 0x000E9F1B File Offset: 0x000E811B
		public bool IsPeeTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
		}

		// Token: 0x04005465 RID: 21605
		private AmountInstance bladder;

		// Token: 0x04005466 RID: 21606
		private ChoreDriver choreDriver;
	}
}
