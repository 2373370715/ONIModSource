using System;

// Token: 0x02000CE7 RID: 3303
public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
	// Token: 0x0600405D RID: 16477 RVA: 0x000C9B9F File Offset: 0x000C7D9F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04002BFD RID: 11261
	[MyCmpGet]
	private ComplexFabricator fabricator;

	// Token: 0x04002BFE RID: 11262
	public StatusItem idleQueue_StatusItem = Db.Get().BuildingStatusItems.FabricatorIdle;

	// Token: 0x04002BFF RID: 11263
	public StatusItem waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FabricatorEmpty;

	// Token: 0x04002C00 RID: 11264
	public StatusItem waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.PendingWork;

	// Token: 0x04002C01 RID: 11265
	public string idleAnimationName = "off";

	// Token: 0x02000CE8 RID: 3304
	public class StatesInstance : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.GameInstance
	{
		// Token: 0x0600405F RID: 16479 RVA: 0x000C9BB2 File Offset: 0x000C7DB2
		public StatesInstance(ComplexFabricatorSM master) : base(master)
		{
		}
	}

	// Token: 0x02000CE9 RID: 3305
	public class States : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM>
	{
		// Token: 0x06004060 RID: 16480 RVA: 0x0023B50C File Offset: 0x0023970C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.idle, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.DefaultState(this.idle.idleQueue).PlayAnim(new Func<ComplexFabricatorSM.StatesInstance, string>(ComplexFabricatorSM.States.GetIdleAnimName), KAnim.PlayMode.Once).EventTransition(GameHashes.OperationalChanged, this.off, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.idle.idleQueue.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.idleQueue_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForMaterial, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.HasAnyOrder);
			this.idle.waitingForMaterial.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForMaterial_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.HasAnyOrder).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForWorker, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.WaitingForWorker).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.idle.waitingForWorker.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForWorker_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.WaitingForWorker).EnterTransition(this.operating, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.duplicantOperated).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.operating.DefaultState(this.operating.working_pre).ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.workingStatusItem, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<ComplexFabricator>());
			this.operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operating.working_loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.Workable, this.operating.working_pst_complete).OnAnimQueueComplete(this.idle);
			this.operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x0023B9C8 File Offset: 0x00239BC8
		public void RefreshHEPStatus(ComplexFabricatorSM.StatesInstance smi)
		{
			if (smi.master.GetComponent<HighEnergyParticleStorage>() != null && smi.master.fabricator.NeedsMoreHEPForQueuedRecipe())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, smi.master.fabricator);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, false);
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x000C9BBB File Offset: 0x000C7DBB
		public static string GetIdleAnimName(ComplexFabricatorSM.StatesInstance smi)
		{
			return smi.master.idleAnimationName;
		}

		// Token: 0x04002C02 RID: 11266
		public ComplexFabricatorSM.States.IdleStates off;

		// Token: 0x04002C03 RID: 11267
		public ComplexFabricatorSM.States.IdleStates idle;

		// Token: 0x04002C04 RID: 11268
		public ComplexFabricatorSM.States.OperatingStates operating;

		// Token: 0x02000CEA RID: 3306
		public class IdleStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x04002C05 RID: 11269
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State idleQueue;

			// Token: 0x04002C06 RID: 11270
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForMaterial;

			// Token: 0x04002C07 RID: 11271
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForWorker;
		}

		// Token: 0x02000CEB RID: 3307
		public class OperatingStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x04002C08 RID: 11272
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pre;

			// Token: 0x04002C09 RID: 11273
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_loop;

			// Token: 0x04002C0A RID: 11274
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst;

			// Token: 0x04002C0B RID: 11275
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst_complete;
		}
	}
}
