using System;

// Token: 0x020011CB RID: 4555
public class RobotIdleMonitor : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance>
{
	// Token: 0x06005CE0 RID: 23776 RVA: 0x0029CB00 File Offset: 0x0029AD00
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Transition(this.working, (RobotIdleMonitor.Instance smi) => !RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
		this.working.Transition(this.idle, (RobotIdleMonitor.Instance smi) => RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
	}

	// Token: 0x06005CE1 RID: 23777 RVA: 0x0029CB84 File Offset: 0x0029AD84
	private static bool CheckShouldIdle(RobotIdleMonitor.Instance smi)
	{
		FallMonitor.Instance smi2 = smi.master.gameObject.GetSMI<FallMonitor.Instance>();
		return smi2 == null || (!smi.master.gameObject.GetComponent<ChoreConsumer>().choreDriver.HasChore() && smi2.GetCurrentState() == smi2.sm.standing);
	}

	// Token: 0x040041B6 RID: 16822
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040041B7 RID: 16823
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x020011CC RID: 4556
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020011CD RID: 4557
	public new class Instance : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06005CE4 RID: 23780 RVA: 0x000DCA50 File Offset: 0x000DAC50
		public Instance(IStateMachineTarget master, RobotIdleMonitor.Def def) : base(master)
		{
		}

		// Token: 0x040041B8 RID: 16824
		public KBatchedAnimController eyes;
	}
}
