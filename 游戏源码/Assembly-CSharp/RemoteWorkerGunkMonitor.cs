using System;

// Token: 0x02001750 RID: 5968
public class RemoteWorkerGunkMonitor : StateMachineComponent<RemoteWorkerGunkMonitor.StatesInstance>
{
	// Token: 0x06007ADA RID: 31450 RVA: 0x000F0BC7 File Offset: 0x000EEDC7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x06007ADB RID: 31451 RVA: 0x000F0BDA File Offset: 0x000EEDDA
	public float Gunk
	{
		get
		{
			return this.storage.GetMassAvailable(SimHashes.LiquidGunk);
		}
	}

	// Token: 0x06007ADC RID: 31452 RVA: 0x000F0BEC File Offset: 0x000EEDEC
	public float GunkLevel()
	{
		return this.Gunk / 600f;
	}

	// Token: 0x04005C1C RID: 23580
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005C1D RID: 23581
	public const float CAPACITY_KG = 600f;

	// Token: 0x04005C1E RID: 23582
	public const float HIGH_LEVEL = 480f;

	// Token: 0x04005C1F RID: 23583
	public const float DRAIN_AMOUNT_KG_PER_S = 1f;

	// Token: 0x02001751 RID: 5969
	public class StatesInstance : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.GameInstance
	{
		// Token: 0x06007ADE RID: 31454 RVA: 0x000F0C02 File Offset: 0x000EEE02
		public StatesInstance(RemoteWorkerGunkMonitor master) : base(master)
		{
		}
	}

	// Token: 0x02001752 RID: 5970
	public class States : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor>
	{
		// Token: 0x06007ADF RID: 31455 RVA: 0x00319D1C File Offset: 0x00317F1C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms);
			this.high_gunk.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerHighGunkLevel, null);
			this.full_gunk.Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerFullGunkLevel, null);
		}

		// Token: 0x06007AE0 RID: 31456 RVA: 0x000F0C0B File Offset: 0x000EEE0B
		public static bool IsGunkLevelOk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk < 480f;
		}

		// Token: 0x06007AE1 RID: 31457 RVA: 0x000F0C1F File Offset: 0x000EEE1F
		public static bool IsGunkHigh(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 480f && smi.master.Gunk < 600f;
		}

		// Token: 0x06007AE2 RID: 31458 RVA: 0x000F0C47 File Offset: 0x000EEE47
		public static bool IsFullOfGunk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 600f;
		}

		// Token: 0x04005C20 RID: 23584
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State ok;

		// Token: 0x04005C21 RID: 23585
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State high_gunk;

		// Token: 0x04005C22 RID: 23586
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State full_gunk;
	}
}
