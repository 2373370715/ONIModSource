using System;
using Klei.AI;
using TUNING;

// Token: 0x02001532 RID: 5426
public class CalorieMonitor : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance>
{
	// Token: 0x06007129 RID: 28969 RVA: 0x002F9D60 File Offset: 0x002F7F60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Transition(this.hungry, (CalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_200ms);
		this.hungry.DefaultState(this.hungry.normal).Transition(this.satisfied, (CalorieMonitor.Instance smi) => smi.IsSatisfied(), UpdateRate.SIM_200ms).EventTransition(GameHashes.BeginChore, this.eating, (CalorieMonitor.Instance smi) => smi.IsEating());
		this.hungry.working.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.normal, (CalorieMonitor.Instance smi) => smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null);
		this.hungry.normal.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.working, (CalorieMonitor.Instance smi) => !smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.hungry.starving.Transition(this.hungry.normal, (CalorieMonitor.Instance smi) => !smi.IsStarving(), UpdateRate.SIM_200ms).Transition(this.depleted, (CalorieMonitor.Instance smi) => smi.IsDepleted(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Starving, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.eating.EventTransition(GameHashes.EndChore, this.satisfied, (CalorieMonitor.Instance smi) => !smi.IsEating());
		this.depleted.ToggleTag(GameTags.CaloriesDepleted).Enter(delegate(CalorieMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x04005481 RID: 21633
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005482 RID: 21634
	public CalorieMonitor.HungryState hungry;

	// Token: 0x04005483 RID: 21635
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State eating;

	// Token: 0x04005484 RID: 21636
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x04005485 RID: 21637
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State depleted;

	// Token: 0x02001533 RID: 5427
	public class HungryState : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005486 RID: 21638
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x04005487 RID: 21639
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x04005488 RID: 21640
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State starving;
	}

	// Token: 0x02001534 RID: 5428
	public new class Instance : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600712C RID: 28972 RVA: 0x000EA17C File Offset: 0x000E837C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
		}

		// Token: 0x0600712D RID: 28973 RVA: 0x000EA1A5 File Offset: 0x000E83A5
		private float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x0600712E RID: 28974 RVA: 0x000EA1BE File Offset: 0x000E83BE
		public bool IsEatTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
		}

		// Token: 0x0600712F RID: 28975 RVA: 0x000EA1DF File Offset: 0x000E83DF
		public bool IsHungry()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.HUNGRY_THRESHOLD;
		}

		// Token: 0x06007130 RID: 28976 RVA: 0x000EA1F8 File Offset: 0x000E83F8
		public bool IsStarving()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.STARVING_THRESHOLD;
		}

		// Token: 0x06007131 RID: 28977 RVA: 0x000EA211 File Offset: 0x000E8411
		public bool IsSatisfied()
		{
			return this.GetCalories0to1() > DUPLICANTSTATS.STANDARD.BaseStats.SATISFIED_THRESHOLD;
		}

		// Token: 0x06007132 RID: 28978 RVA: 0x002FA08C File Offset: 0x002F828C
		public bool IsEating()
		{
			ChoreDriver component = base.master.GetComponent<ChoreDriver>();
			return component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x06007133 RID: 28979 RVA: 0x000EA22A File Offset: 0x000E842A
		public bool IsDepleted()
		{
			return this.calories.value <= 0f;
		}

		// Token: 0x06007134 RID: 28980 RVA: 0x000EA241 File Offset: 0x000E8441
		public bool ShouldExitInfirmary()
		{
			return !this.IsStarving();
		}

		// Token: 0x06007135 RID: 28981 RVA: 0x000EA24C File Offset: 0x000E844C
		public void Kill()
		{
			if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
			{
				base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
			}
		}

		// Token: 0x04005489 RID: 21641
		public AmountInstance calories;
	}
}
