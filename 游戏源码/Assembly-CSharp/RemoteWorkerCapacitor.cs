using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001753 RID: 5971
public class RemoteWorkerCapacitor : StateMachineComponent<RemoteWorkerCapacitor.StatesInstance>
{
	// Token: 0x06007AE4 RID: 31460 RVA: 0x000F0C66 File Offset: 0x000EEE66
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007AE5 RID: 31461 RVA: 0x00319E08 File Offset: 0x00318008
	public float ApplyDeltaEnergy(float delta)
	{
		float num = this.charge;
		this.charge = Mathf.Clamp(this.charge + delta, 0f, 60f);
		return this.charge - num;
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06007AE6 RID: 31462 RVA: 0x000F0C79 File Offset: 0x000EEE79
	public float ChargeRatio
	{
		get
		{
			return this.charge / 60f;
		}
	}

	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x06007AE7 RID: 31463 RVA: 0x000F0C87 File Offset: 0x000EEE87
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06007AE8 RID: 31464 RVA: 0x000F0C8F File Offset: 0x000EEE8F
	public bool IsLowPower
	{
		get
		{
			return this.charge < 15f;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06007AE9 RID: 31465 RVA: 0x000F0C9E File Offset: 0x000EEE9E
	public bool IsOutOfPower
	{
		get
		{
			return this.charge < float.Epsilon;
		}
	}

	// Token: 0x04005C23 RID: 23587
	[Serialize]
	private float charge;

	// Token: 0x04005C24 RID: 23588
	public const float LOW_LEVEL = 15f;

	// Token: 0x04005C25 RID: 23589
	public const float POWER_USE_RATE_J_PER_S = -0.1f;

	// Token: 0x04005C26 RID: 23590
	public const float POWER_CHARGE_RATE_J_PER_S = 4f;

	// Token: 0x04005C27 RID: 23591
	public const float CAPACITY = 60f;

	// Token: 0x02001754 RID: 5972
	public class StatesInstance : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.GameInstance
	{
		// Token: 0x06007AEB RID: 31467 RVA: 0x000F0CB5 File Offset: 0x000EEEB5
		public StatesInstance(RemoteWorkerCapacitor master) : base(master)
		{
		}
	}

	// Token: 0x02001755 RID: 5973
	public class States : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor>
	{
		// Token: 0x06007AEC RID: 31468 RVA: 0x00319E44 File Offset: 0x00318044
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerCapacitorStatus, (RemoteWorkerCapacitor.StatesInstance smi) => smi.master);
			this.ok.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms);
			this.low_power.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowPower, null);
			this.out_of_power.Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfPower, null);
		}

		// Token: 0x06007AED RID: 31469 RVA: 0x000F0CBE File Offset: 0x000EEEBE
		public static bool IsOkForPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return !smi.master.IsLowPower;
		}

		// Token: 0x06007AEE RID: 31470 RVA: 0x000F0CCE File Offset: 0x000EEECE
		public static bool IsLowPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsLowPower && !smi.master.IsOutOfPower;
		}

		// Token: 0x06007AEF RID: 31471 RVA: 0x000F0CED File Offset: 0x000EEEED
		public static bool IsOutOfPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsOutOfPower;
		}

		// Token: 0x04005C28 RID: 23592
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State ok;

		// Token: 0x04005C29 RID: 23593
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State low_power;

		// Token: 0x04005C2A RID: 23594
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State out_of_power;
	}
}
