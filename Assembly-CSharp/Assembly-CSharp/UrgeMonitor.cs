﻿using System;
using Klei.AI;

public class UrgeMonitor : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.hasurge, (UrgeMonitor.Instance smi) => smi.HasUrge(), UpdateRate.SIM_200ms);
		this.hasurge.Transition(this.satisfied, (UrgeMonitor.Instance smi) => !smi.HasUrge(), UpdateRate.SIM_200ms).ToggleUrge((UrgeMonitor.Instance smi) => smi.GetUrge());
	}

	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State hasurge;

	public new class Instance : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		public Instance(IStateMachineTarget master, Urge urge, Amount amount, ScheduleBlockType schedule_block, float in_schedule_threshold, float out_of_schedule_threshold, bool is_threshold_minimum) : base(master)
		{
			this.urge = urge;
			this.scheduleBlock = schedule_block;
			this.schedulable = base.GetComponent<Schedulable>();
			this.amountInstance = base.gameObject.GetAmounts().Get(amount);
			this.isThresholdMinimum = is_threshold_minimum;
			this.inScheduleThreshold = in_schedule_threshold;
			this.outOfScheduleThreshold = out_of_schedule_threshold;
		}

		private float GetThreshold()
		{
			if (this.schedulable.IsAllowed(this.scheduleBlock))
			{
				return this.inScheduleThreshold;
			}
			return this.outOfScheduleThreshold;
		}

		public Urge GetUrge()
		{
			return this.urge;
		}

		public bool HasUrge()
		{
			if (this.isThresholdMinimum)
			{
				return this.amountInstance.value >= this.GetThreshold();
			}
			return this.amountInstance.value <= this.GetThreshold();
		}

		private AmountInstance amountInstance;

		private Urge urge;

		private ScheduleBlockType scheduleBlock;

		private Schedulable schedulable;

		private float inScheduleThreshold;

		private float outOfScheduleThreshold;

		private bool isThresholdMinimum;
	}
}
