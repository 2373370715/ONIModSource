using System;

// Token: 0x0200174D RID: 5965
public class RemoteWorkerOilMonitor : StateMachineComponent<RemoteWorkerOilMonitor.StatesInstance>
{
	// Token: 0x06007AD0 RID: 31440 RVA: 0x000F0B2B File Offset: 0x000EED2B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x06007AD1 RID: 31441 RVA: 0x000F0B3E File Offset: 0x000EED3E
	public float Oil
	{
		get
		{
			return this.storage.GetMassAvailable(GameTags.LubricatingOil);
		}
	}

	// Token: 0x06007AD2 RID: 31442 RVA: 0x000F0B50 File Offset: 0x000EED50
	public float OilLevel()
	{
		return this.Oil / 60f;
	}

	// Token: 0x04005C14 RID: 23572
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005C15 RID: 23573
	public const float CAPACITY_KG = 60f;

	// Token: 0x04005C16 RID: 23574
	public const float LOW_LEVEL = 12f;

	// Token: 0x04005C17 RID: 23575
	public const float FILL_RATE_KG_PER_S = 1f;

	// Token: 0x04005C18 RID: 23576
	public const float CONSUMPTION_RATE_KG_PER_S = 0.1f;

	// Token: 0x0200174E RID: 5966
	public class StatesInstance : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.GameInstance
	{
		// Token: 0x06007AD4 RID: 31444 RVA: 0x000F0B66 File Offset: 0x000EED66
		public StatesInstance(RemoteWorkerOilMonitor master) : base(master)
		{
		}
	}

	// Token: 0x0200174F RID: 5967
	public class States : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor>
	{
		// Token: 0x06007AD5 RID: 31445 RVA: 0x00319C30 File Offset: 0x00317E30
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms);
			this.low_oil.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowOil, null);
			this.out_of_oil.Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfOil, null);
		}

		// Token: 0x06007AD6 RID: 31446 RVA: 0x000F0B6F File Offset: 0x000EED6F
		public static bool IsOkForOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil > 12f;
		}

		// Token: 0x06007AD7 RID: 31447 RVA: 0x000F0B83 File Offset: 0x000EED83
		public static bool IsLowOnOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil >= float.Epsilon && smi.master.Oil < 12f;
		}

		// Token: 0x06007AD8 RID: 31448 RVA: 0x000F0BAB File Offset: 0x000EEDAB
		public static bool IsOutOfOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil < float.Epsilon;
		}

		// Token: 0x04005C19 RID: 23577
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State ok;

		// Token: 0x04005C1A RID: 23578
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State low_oil;

		// Token: 0x04005C1B RID: 23579
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State out_of_oil;
	}
}
