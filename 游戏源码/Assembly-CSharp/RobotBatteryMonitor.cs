using System;
using Klei.AI;

// Token: 0x020017C5 RID: 6085
public class RobotBatteryMonitor : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>
{
	// Token: 0x06007D67 RID: 32103 RVA: 0x00326828 File Offset: 0x00324A28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drainingStates;
		this.drainingStates.DefaultState(this.drainingStates.highBattery).Transition(this.deadBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.BatteryDead), UpdateRate.SIM_200ms).Transition(this.needsRechargeStates, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.NeedsRecharge), UpdateRate.SIM_200ms);
		this.drainingStates.highBattery.Transition(this.drainingStates.lowBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent)), UpdateRate.SIM_200ms);
		this.drainingStates.lowBattery.Transition(this.drainingStates.highBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent), UpdateRate.SIM_200ms).ToggleStatusItem(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (!smi.def.canCharge)
			{
				return Db.Get().RobotStatusItems.LowBatteryNoCharge;
			}
			return Db.Get().RobotStatusItems.LowBattery;
		}, (RobotBatteryMonitor.Instance smi) => smi.gameObject);
		this.needsRechargeStates.DefaultState(this.needsRechargeStates.lowBattery).Transition(this.deadBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.BatteryDead), UpdateRate.SIM_200ms).Transition(this.drainingStates, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeComplete), UpdateRate.SIM_200ms).ToggleBehaviour(GameTags.Robots.Behaviours.RechargeBehaviour, (RobotBatteryMonitor.Instance smi) => smi.def.canCharge, null);
		this.needsRechargeStates.lowBattery.ToggleStatusItem(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (!smi.def.canCharge)
			{
				return Db.Get().RobotStatusItems.LowBatteryNoCharge;
			}
			return Db.Get().RobotStatusItems.LowBattery;
		}, (RobotBatteryMonitor.Instance smi) => smi.gameObject).Transition(this.needsRechargeStates.mediumBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent), UpdateRate.SIM_200ms);
		this.needsRechargeStates.mediumBattery.Transition(this.needsRechargeStates.lowBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent)), UpdateRate.SIM_200ms).Transition(this.needsRechargeStates.trickleCharge, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeFull), UpdateRate.SIM_200ms);
		this.needsRechargeStates.trickleCharge.Transition(this.needsRechargeStates.mediumBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeFull)), UpdateRate.SIM_200ms);
		this.deadBattery.ToggleStatusItem(Db.Get().RobotStatusItems.DeadBattery, (RobotBatteryMonitor.Instance smi) => smi.gameObject).Enter(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (smi.GetSMI<DeathMonitor.Instance>() != null)
			{
				smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.DeadBattery);
			}
		});
	}

	// Token: 0x06007D68 RID: 32104 RVA: 0x000F291D File Offset: 0x000F0B1D
	public static bool NeedsRecharge(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value <= 0f || GameClock.Instance.IsNighttime();
	}

	// Token: 0x06007D69 RID: 32105 RVA: 0x000F293D File Offset: 0x000F0B3D
	public static bool ChargeDecent(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax() * smi.def.lowBatteryWarningPercent;
	}

	// Token: 0x06007D6A RID: 32106 RVA: 0x000F2966 File Offset: 0x000F0B66
	public static bool ChargeFull(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax();
	}

	// Token: 0x06007D6B RID: 32107 RVA: 0x000F2983 File Offset: 0x000F0B83
	public static bool ChargeComplete(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax() && !GameClock.Instance.IsNighttime();
	}

	// Token: 0x06007D6C RID: 32108 RVA: 0x000F29AC File Offset: 0x000F0BAC
	public static bool BatteryDead(RobotBatteryMonitor.Instance smi)
	{
		return !smi.def.canCharge && smi.amountInstance.value == 0f;
	}

	// Token: 0x04005EEC RID: 24300
	public StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage>();

	// Token: 0x04005EED RID: 24301
	public RobotBatteryMonitor.NeedsRechargeStates needsRechargeStates;

	// Token: 0x04005EEE RID: 24302
	public RobotBatteryMonitor.DrainingStates drainingStates;

	// Token: 0x04005EEF RID: 24303
	public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State deadBattery;

	// Token: 0x020017C6 RID: 6086
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005EF0 RID: 24304
		public string batteryAmountId;

		// Token: 0x04005EF1 RID: 24305
		public float lowBatteryWarningPercent;

		// Token: 0x04005EF2 RID: 24306
		public bool canCharge;
	}

	// Token: 0x020017C7 RID: 6087
	public class DrainingStates : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State
	{
		// Token: 0x04005EF3 RID: 24307
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State highBattery;

		// Token: 0x04005EF4 RID: 24308
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State lowBattery;
	}

	// Token: 0x020017C8 RID: 6088
	public class NeedsRechargeStates : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State
	{
		// Token: 0x04005EF5 RID: 24309
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State lowBattery;

		// Token: 0x04005EF6 RID: 24310
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State mediumBattery;

		// Token: 0x04005EF7 RID: 24311
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State trickleCharge;
	}

	// Token: 0x020017C9 RID: 6089
	public new class Instance : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.GameInstance
	{
		// Token: 0x06007D71 RID: 32113 RVA: 0x00326AD4 File Offset: 0x00324CD4
		public Instance(IStateMachineTarget master, RobotBatteryMonitor.Def def) : base(master, def)
		{
			this.amountInstance = Db.Get().Amounts.Get(def.batteryAmountId).Lookup(base.gameObject);
			this.amountInstance.SetValue(this.amountInstance.GetMax());
		}

		// Token: 0x04005EF8 RID: 24312
		public AmountInstance amountInstance;
	}
}
