﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

[SkipSaveFileSerialization]
public class Narcolepsy : StateMachineComponent<Narcolepsy.StatesInstance>
{
		protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

		public bool IsNarcolepsing()
	{
		return base.smi.IsNarcolepsing();
	}

		public static readonly Chore.Precondition IsNarcolepsingPrecondition = new Chore.Precondition
	{
		id = "IsNarcolepsingPrecondition",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NARCOLEPSING,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			return component != null && component.IsNarcolepsing();
		}
	};

		public class StatesInstance : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.GameInstance
	{
				public StatesInstance(Narcolepsy master) : base(master)
		{
		}

				public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

				public bool IsNarcolepsing()
		{
			return this.GetCurrentState() == base.sm.sleepy;
		}

				public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "NarcolepticSleep";
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}
	}

		public class States : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Enter("ScheduleNextSleep", delegate(Narcolepsy.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_INTERVAL_MIN, TRAITS.NARCOLEPSY_INTERVAL_MAX), this.sleepy);
			});
			this.sleepy.Enter("Is Already Sleeping Check", delegate(Narcolepsy.StatesInstance smi)
			{
				if (smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping())
				{
					smi.GoTo(this.idle);
					return;
				}
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_SLEEPDURATION_MIN, TRAITS.NARCOLEPSY_SLEEPDURATION_MAX), this.idle);
			}).ToggleUrge(Db.Get().Urges.Narcolepsy).ToggleChore(new Func<Narcolepsy.StatesInstance, Chore>(this.CreateNarcolepsyChore), this.idle);
		}

				private Chore CreateNarcolepsyChore(Narcolepsy.StatesInstance smi)
		{
			GameObject bed = smi.CreateFloorLocator();
			SleepChore sleepChore = new SleepChore(Db.Get().ChoreTypes.Narcolepsy, smi.master, bed, true, false);
			sleepChore.AddPrecondition(Narcolepsy.IsNarcolepsingPrecondition, null);
			return sleepChore;
		}

				private float GetNewInterval(float min, float max)
		{
			Mathf.Min(Mathf.Max(Util.GaussianRandom(max - min, 1f), min), max);
			return UnityEngine.Random.Range(min, max);
		}

				public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State idle;

				public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State sleepy;
	}
}
