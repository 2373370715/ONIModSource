using System;

// Token: 0x02000078 RID: 120
public class PoweredController : GameStateMachine<PoweredController, PoweredController.Instance>
{
	// Token: 0x060001F3 RID: 499 RVA: 0x00145724 File Offset: 0x00143924
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x04000142 RID: 322
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000143 RID: 323
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000079 RID: 121
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200007A RID: 122
	public new class Instance : GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x000A691C File Offset: 0x000A4B1C
		public Instance(IStateMachineTarget master, PoweredController.Def def) : base(master, def)
		{
		}

		// Token: 0x04000144 RID: 324
		public bool ShowWorkingStatus;
	}
}
