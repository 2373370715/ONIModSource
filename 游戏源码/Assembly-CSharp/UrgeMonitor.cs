using System;
using Klei.AI;

// Token: 0x0200161A RID: 5658
public class UrgeMonitor : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance>
{
	// Token: 0x0600751D RID: 29981 RVA: 0x00305924 File Offset: 0x00303B24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.hasurge, (UrgeMonitor.Instance smi) => smi.HasUrge(), UpdateRate.SIM_200ms);
		this.hasurge.Transition(this.satisfied, (UrgeMonitor.Instance smi) => !smi.HasUrge(), UpdateRate.SIM_200ms).ToggleUrge((UrgeMonitor.Instance smi) => smi.GetUrge());
	}

	// Token: 0x040057AA RID: 22442
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040057AB RID: 22443
	public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State hasurge;

	// Token: 0x0200161B RID: 5659
	public new class Instance : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600751F RID: 29983 RVA: 0x003059C4 File Offset: 0x00303BC4
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

		// Token: 0x06007520 RID: 29984 RVA: 0x000ECEFB File Offset: 0x000EB0FB
		private float GetThreshold()
		{
			if (this.schedulable.IsAllowed(this.scheduleBlock))
			{
				return this.inScheduleThreshold;
			}
			return this.outOfScheduleThreshold;
		}

		// Token: 0x06007521 RID: 29985 RVA: 0x000ECF1D File Offset: 0x000EB11D
		public Urge GetUrge()
		{
			return this.urge;
		}

		// Token: 0x06007522 RID: 29986 RVA: 0x000ECF25 File Offset: 0x000EB125
		public bool HasUrge()
		{
			if (this.isThresholdMinimum)
			{
				return this.amountInstance.value >= this.GetThreshold();
			}
			return this.amountInstance.value <= this.GetThreshold();
		}

		// Token: 0x040057AC RID: 22444
		private AmountInstance amountInstance;

		// Token: 0x040057AD RID: 22445
		private Urge urge;

		// Token: 0x040057AE RID: 22446
		private ScheduleBlockType scheduleBlock;

		// Token: 0x040057AF RID: 22447
		private Schedulable schedulable;

		// Token: 0x040057B0 RID: 22448
		private float inScheduleThreshold;

		// Token: 0x040057B1 RID: 22449
		private float outOfScheduleThreshold;

		// Token: 0x040057B2 RID: 22450
		private bool isThresholdMinimum;
	}
}
