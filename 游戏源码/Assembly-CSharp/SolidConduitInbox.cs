using System;
using KSerialization;

// Token: 0x02000F8F RID: 3983
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitInbox : StateMachineComponent<SolidConduitInbox.SMInstance>, ISim1000ms
{
	// Token: 0x060050A5 RID: 20645 RVA: 0x000D4986 File Offset: 0x000D2B86
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.StorageFetch);
	}

	// Token: 0x060050A6 RID: 20646 RVA: 0x000D49AC File Offset: 0x000D2BAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filteredStorage.FilterChanged();
		base.smi.StartSM();
	}

	// Token: 0x060050A7 RID: 20647 RVA: 0x000D49CA File Offset: 0x000D2BCA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060050A8 RID: 20648 RVA: 0x000D49D2 File Offset: 0x000D2BD2
	public void Sim1000ms(float dt)
	{
		if (this.operational.IsOperational && this.dispenser.IsDispensing)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x04003838 RID: 14392
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003839 RID: 14393
	[MyCmpReq]
	private SolidConduitDispenser dispenser;

	// Token: 0x0400383A RID: 14394
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400383B RID: 14395
	private FilteredStorage filteredStorage;

	// Token: 0x02000F90 RID: 3984
	public class SMInstance : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.GameInstance
	{
		// Token: 0x060050AA RID: 20650 RVA: 0x000D4A11 File Offset: 0x000D2C11
		public SMInstance(SolidConduitInbox master) : base(master)
		{
		}
	}

	// Token: 0x02000F91 RID: 3985
	public class States : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox>
	{
		// Token: 0x060050AB RID: 20651 RVA: 0x0026F50C File Offset: 0x0026D70C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).EventTransition(GameHashes.ActiveChanged, this.on.post, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x0400383C RID: 14396
		public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State off;

		// Token: 0x0400383D RID: 14397
		public SolidConduitInbox.States.ReadyStates on;

		// Token: 0x02000F92 RID: 3986
		public class ReadyStates : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State
		{
			// Token: 0x0400383E RID: 14398
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State idle;

			// Token: 0x0400383F RID: 14399
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State working;

			// Token: 0x04003840 RID: 14400
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State post;
		}
	}
}
