public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, ComplexFabricatorSM, object>.GameInstance
	{
		public StatesInstance(ComplexFabricatorSM master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, ComplexFabricatorSM>
	{
		public class IdleStates : State
		{
			public State idleQueue;

			public State waitingForMaterial;

			public State waitingForWorker;
		}

		public class OperatingStates : State
		{
			public State working_pre;

			public State working_loop;

			public State working_pst;

			public State working_pst_complete;
		}

		public IdleStates off;

		public IdleStates idle;

		public OperatingStates operating;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = off;
			off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, idle, (StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			idle.DefaultState(idle.idleQueue).PlayAnim(GetIdleAnimName).EventTransition(GameHashes.OperationalChanged, off, (StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational)
				.EventTransition(GameHashes.ActiveChanged, operating, (StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			idle.idleQueue.ToggleStatusItem((StatesInstance smi) => smi.master.idleQueue_StatusItem).EventTransition(GameHashes.FabricatorOrdersUpdated, idle.waitingForMaterial, (StatesInstance smi) => smi.master.fabricator.HasAnyOrder);
			idle.waitingForMaterial.ToggleStatusItem((StatesInstance smi) => smi.master.waitingForMaterial_StatusItem).EventTransition(GameHashes.FabricatorOrdersUpdated, idle.idleQueue, (StatesInstance smi) => !smi.master.fabricator.HasAnyOrder).EventTransition(GameHashes.FabricatorOrdersUpdated, idle.waitingForWorker, (StatesInstance smi) => smi.master.fabricator.WaitingForWorker)
				.EventHandler(GameHashes.FabricatorOrdersUpdated, RefreshHEPStatus)
				.EventHandler(GameHashes.OnParticleStorageChanged, RefreshHEPStatus)
				.Enter(delegate(StatesInstance smi)
				{
					RefreshHEPStatus(smi);
				});
			idle.waitingForWorker.ToggleStatusItem((StatesInstance smi) => smi.master.waitingForWorker_StatusItem).EventTransition(GameHashes.FabricatorOrdersUpdated, idle.idleQueue, (StatesInstance smi) => !smi.master.fabricator.WaitingForWorker).EnterTransition(operating, (StatesInstance smi) => !smi.master.fabricator.duplicantOperated)
				.EventHandler(GameHashes.FabricatorOrdersUpdated, RefreshHEPStatus)
				.EventHandler(GameHashes.OnParticleStorageChanged, RefreshHEPStatus)
				.Enter(delegate(StatesInstance smi)
				{
					RefreshHEPStatus(smi);
				});
			operating.DefaultState(operating.working_pre).ToggleStatusItem((StatesInstance smi) => smi.master.fabricator.workingStatusItem, (StatesInstance smi) => smi.GetComponent<ComplexFabricator>());
			operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(operating.working_loop).EventTransition(GameHashes.OperationalChanged, operating.working_pst, (StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational)
				.EventTransition(GameHashes.ActiveChanged, operating.working_pst, (StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, operating.working_pst, (StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, operating.working_pst, (StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((StatesInstance smi) => smi.master.fabricator.Workable, operating.working_pst_complete).OnAnimQueueComplete(idle);
			operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete(idle);
		}

		public void RefreshHEPStatus(StatesInstance smi)
		{
			if (smi.master.GetComponent<HighEnergyParticleStorage>() != null && smi.master.fabricator.NeedsMoreHEPForQueuedRecipe())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, smi.master.fabricator);
			}
			else
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP);
			}
		}

		public static string GetIdleAnimName(StatesInstance smi)
		{
			return smi.master.idleAnimationName;
		}
	}

	[MyCmpGet]
	private ComplexFabricator fabricator;

	public StatusItem idleQueue_StatusItem = Db.Get().BuildingStatusItems.FabricatorIdle;

	public StatusItem waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FabricatorEmpty;

	public StatusItem waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.PendingWork;

	public string idleAnimationName = "off";

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}
}
