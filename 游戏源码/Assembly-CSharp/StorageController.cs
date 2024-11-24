using System;

// Token: 0x0200007C RID: 124
public class StorageController : GameStateMachine<StorageController, StorageController.Instance>
{
	// Token: 0x060001FB RID: 507 RVA: 0x001457BC File Offset: 0x001439BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OnStorageInteracted, this.working, null);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StorageController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StorageController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working.PlayAnim("working").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04000148 RID: 328
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000149 RID: 329
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x0400014A RID: 330
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x0200007D RID: 125
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200007E RID: 126
	public new class Instance : GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001FE RID: 510 RVA: 0x000A693A File Offset: 0x000A4B3A
		public Instance(IStateMachineTarget master, StorageController.Def def) : base(master)
		{
		}
	}
}
