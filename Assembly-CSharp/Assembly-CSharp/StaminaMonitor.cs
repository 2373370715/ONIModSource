﻿using System;
using Klei.AI;

public class StaminaMonitor : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleStateMachine((StaminaMonitor.Instance smi) => new UrgeMonitor.Instance(smi.master, Db.Get().Urges.Sleep, Db.Get().Amounts.Stamina, Db.Get().ScheduleBlockTypes.Sleep, 100f, 0f, false)).ToggleStateMachine((StaminaMonitor.Instance smi) => new SleepChoreMonitor.Instance(smi.master));
		this.satisfied.Transition(this.sleepy, (StaminaMonitor.Instance smi) => smi.NeedsToSleep() || smi.WantsToSleep(), UpdateRate.SIM_200ms);
		this.sleepy.Update("Check Sleep State", delegate(StaminaMonitor.Instance smi, float dt)
		{
			smi.TryExitSleepState();
		}, UpdateRate.SIM_1000ms, false).DefaultState(this.sleepy.needssleep);
		this.sleepy.needssleep.Transition(this.sleepy.sleeping, (StaminaMonitor.Instance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.Tired, null).ToggleThought(Db.Get().Thoughts.Sleepy, null);
		this.sleepy.sleeping.Enter(delegate(StaminaMonitor.Instance smi)
		{
			smi.CheckDebugFastWorkMode();
		}).Transition(this.satisfied, (StaminaMonitor.Instance smi) => !smi.IsSleeping(), UpdateRate.SIM_200ms);
	}

		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State satisfied;

		public StaminaMonitor.SleepyState sleepy;

		private const float OUTSIDE_SCHEDULE_STAMINA_THRESHOLD = 0f;

		public class SleepyState : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State
	{
				public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State needssleep;

				public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State sleeping;
	}

		public new class Instance : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
				public Instance(IStateMachineTarget master) : base(master)
		{
			this.stamina = Db.Get().Amounts.Stamina.Lookup(base.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
			this.schedulable = base.GetComponent<Schedulable>();
		}

				public bool NeedsToSleep()
		{
			return this.stamina.value <= 0f;
		}

				public bool WantsToSleep()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Sleep);
		}

				public void TryExitSleepState()
		{
			if (!this.NeedsToSleep() && !this.WantsToSleep())
			{
				base.smi.GoTo(base.smi.sm.satisfied);
			}
		}

				public bool IsSleeping()
		{
			bool result = false;
			if (this.WantsToSleep() && this.choreDriver.GetComponent<WorkerBase>().GetWorkable() != null)
			{
				result = true;
			}
			return result;
		}

				public void CheckDebugFastWorkMode()
		{
			if (Game.Instance.FastWorkersModeActive)
			{
				this.stamina.value = this.stamina.GetMax();
			}
		}

				public bool ShouldExitSleep()
		{
			if (this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return false;
			}
			Narcolepsy component = base.GetComponent<Narcolepsy>();
			return (!(component != null) || !component.IsNarcolepsing()) && this.stamina.value >= this.stamina.GetMax();
		}

				private ChoreDriver choreDriver;

				private Schedulable schedulable;

				public AmountInstance stamina;
	}
}
