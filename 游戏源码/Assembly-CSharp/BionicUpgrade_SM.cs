using System;

// Token: 0x02000C55 RID: 3157
public class BionicUpgrade_SM<SMType, StateMachineInstanceType> : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where SMType : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where StateMachineInstanceType : BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance
{
	// Token: 0x06003C77 RID: 15479 RVA: 0x000C7141 File Offset: 0x000C5341
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x000C7152 File Offset: 0x000C5352
	public static bool IsOnline(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x000C715A File Offset: 0x000C535A
	public static bool IsInBatterySaveMode(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsInBatterySavingMode;
	}

	// Token: 0x04002945 RID: 10565
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Active;

	// Token: 0x04002946 RID: 10566
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Inactive;

	// Token: 0x02000C56 RID: 3158
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06003C7B RID: 15483 RVA: 0x000C716A File Offset: 0x000C536A
		public Def(string upgradeID)
		{
			this.UpgradeID = upgradeID;
		}

		// Token: 0x04002947 RID: 10567
		public string UpgradeID;

		// Token: 0x04002948 RID: 10568
		public Func<StateMachine.Instance, StateMachine.Instance>[] StateMachinesWhenActive;
	}

	// Token: 0x02000C57 RID: 3159
	public abstract class BaseInstance : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.GameInstance, BionicUpgradeComponent.IWattageController
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06003C7C RID: 15484 RVA: 0x000C7179 File Offset: 0x000C5379
		public bool IsInBatterySavingMode
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsBatterySaveModeActive;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x000C7190 File Offset: 0x000C5390
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06003C7E RID: 15486 RVA: 0x000C71A7 File Offset: 0x000C53A7
		public BionicUpgradeComponentConfig.BionicUpgradeData Data
		{
			get
			{
				return BionicUpgradeComponentConfig.UpgradesData[base.def.UpgradeID];
			}
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x000C71C3 File Offset: 0x000C53C3
		public BaseInstance(IStateMachineTarget master, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def def) : base(master, def)
		{
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.RegisterMonitorToUpgradeComponent();
			base.Subscribe(-426516281, new Action<object>(this.OnBatterySavingModeChanged));
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0022DFE8 File Offset: 0x0022C1E8
		private void RegisterMonitorToUpgradeComponent()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgradeComponent installedUpgradeComponent = upgradeComponentSlot.installedUpgradeComponent;
					if (installedUpgradeComponent != null && !installedUpgradeComponent.HasWattageController)
					{
						this.upgradeComponent = installedUpgradeComponent;
						installedUpgradeComponent.SetWattageController(this);
						return;
					}
				}
			}
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x000C71FB File Offset: 0x000C53FB
		private void UnregisterMonitorToUpgradeComponent()
		{
			if (this.upgradeComponent != null)
			{
				this.upgradeComponent.SetWattageController(null);
			}
		}

		// Token: 0x06003C82 RID: 15490
		public abstract float GetCurrentWattageCost();

		// Token: 0x06003C83 RID: 15491
		public abstract string GetCurrentWattageCostName();

		// Token: 0x06003C84 RID: 15492 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected virtual void OnEnteringBatterySavingMode()
		{
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected virtual void OnExitingBatterySavingMode()
		{
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x000C7217 File Offset: 0x000C5417
		private void OnBatterySavingModeChanged(object o)
		{
			if ((bool)o)
			{
				this.OnEnteringBatterySavingMode();
				return;
			}
			this.OnExitingBatterySavingMode();
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x000C722E File Offset: 0x000C542E
		protected override void OnCleanUp()
		{
			this.UnregisterMonitorToUpgradeComponent();
			base.Unsubscribe(-426516281, new Action<object>(this.OnBatterySavingModeChanged));
			base.OnCleanUp();
		}

		// Token: 0x04002949 RID: 10569
		protected BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x0400294A RID: 10570
		protected BionicUpgradeComponent upgradeComponent;
	}
}
