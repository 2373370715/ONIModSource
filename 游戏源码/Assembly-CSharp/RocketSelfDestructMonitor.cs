using System;

// Token: 0x020011CF RID: 4559
public class RocketSelfDestructMonitor : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance>
{
	// Token: 0x06005CE9 RID: 23785 RVA: 0x000DCA78 File Offset: 0x000DAC78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.RocketSelfDestructRequested, this.exploding, null);
		this.exploding.Update(delegate(RocketSelfDestructMonitor.Instance smi, float dt)
		{
			if (smi.timeinstate >= 3f)
			{
				smi.master.Trigger(-1311384361, null);
				smi.GoTo(this.idle);
			}
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x040041BC RID: 16828
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040041BD RID: 16829
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State exploding;

	// Token: 0x020011D0 RID: 4560
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020011D1 RID: 4561
	public new class Instance : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06005CED RID: 23789 RVA: 0x000DCAE8 File Offset: 0x000DACE8
		public Instance(IStateMachineTarget master, RocketSelfDestructMonitor.Def def) : base(master)
		{
		}

		// Token: 0x040041BE RID: 16830
		public KBatchedAnimController eyes;
	}
}
