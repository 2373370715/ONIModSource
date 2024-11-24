using System;

// Token: 0x0200005A RID: 90
public class ActiveController : GameStateMachine<ActiveController, ActiveController.Instance>
{
	// Token: 0x060001A6 RID: 422 RVA: 0x00144E14 File Offset: 0x00143014
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (ActiveController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.working_pst, (ActiveController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000F8 RID: 248
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000F9 RID: 249
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000FA RID: 250
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000FB RID: 251
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x0200005B RID: 91
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200005C RID: 92
	public new class Instance : GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x000A66B5 File Offset: 0x000A48B5
		public Instance(IStateMachineTarget master, ActiveController.Def def) : base(master, def)
		{
		}
	}
}
