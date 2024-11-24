using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020017CB RID: 6091
public class RobotElectroBankMonitor : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>
{
	// Token: 0x06007D7B RID: 32123 RVA: 0x00326B28 File Offset: 0x00324D28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.powered;
		this.root.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.ElectroBankStorageChange(null);
		});
		this.powered.DefaultState(this.powered.highBattery).ParamTransition<bool>(this.hasElectrobank, this.powerdown.pre, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.IsFalse).Update(delegate(RobotElectroBankMonitor.Instance smi, float dt)
		{
			RobotElectroBankMonitor.ConsumePower(smi, dt);
		}, UpdateRate.SIM_200ms, false);
		this.powered.highBattery.Transition(this.powered.lowBattery, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Not(new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent)), UpdateRate.SIM_200ms).Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.gameObject.RemoveTag(GameTags.Dead);
		});
		this.powered.lowBattery.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
		}).Transition(this.powered.highBattery, new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent), UpdateRate.SIM_200ms).ToggleStatusItem((RobotElectroBankMonitor.Instance smi) => Db.Get().RobotStatusItems.LowBatteryNoCharge, null).EventHandlerTransition(GameHashes.OnStorageChange, this.powerup.flying, new Func<RobotElectroBankMonitor.Instance, object, bool>(RobotElectroBankMonitor.BatteryDelivered));
		this.powerdown.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			smi.Get<Brain>().Stop("dead battery");
		}).ToggleStatusItem(Db.Get().RobotStatusItems.DeadBatteryFlydo, (RobotElectroBankMonitor.Instance smi) => smi.gameObject).Exit(delegate(RobotElectroBankMonitor.Instance smi)
		{
			if (GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Remove(smi.gameObject);
			}
		});
		this.powerdown.pre.PlayAnim("power_down_pre").OnAnimQueueComplete(this.powerdown.fall);
		this.powerdown.fall.PlayAnim("power_down_loop", KAnim.PlayMode.Loop).Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			if (!GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Add(smi.gameObject, Vector2.zero);
			}
		}).Update(delegate(RobotElectroBankMonitor.Instance smi, float dt)
		{
			if (!GameComps.Gravities.Has(smi.gameObject))
			{
				smi.GoTo(this.powerdown.landed);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.Landed, this.powerdown.landed, null);
		this.powerdown.landed.PlayAnim("power_down_pst").OnAnimQueueComplete(this.powerdown.dead);
		this.powerdown.dead.PlayAnim("dead_battery").ParamTransition<bool>(this.hasElectrobank, this.powerup.grounded, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.IsTrue);
		this.powerup.Exit(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.Get<Brain>().Reset("power up");
		});
		this.powerup.flying.PlayAnim("battery_change_fly").OnAnimQueueComplete(this.powered);
		this.powerup.grounded.PlayAnim("battery_change_dead").OnAnimQueueComplete(this.powerup.takeoff);
		this.powerup.takeoff.PlayAnim("power_up").OnAnimQueueComplete(this.powered);
	}

	// Token: 0x06007D7C RID: 32124 RVA: 0x00326E9C File Offset: 0x0032509C
	public static bool ChargeDecent(RobotElectroBankMonitor.Instance smi)
	{
		float num = 0f;
		foreach (GameObject gameObject in smi.electroBankStorage.items)
		{
			num += gameObject.GetComponent<Electrobank>().Charge;
		}
		return num >= smi.def.lowBatteryWarningPercent * 120000f;
	}

	// Token: 0x06007D7D RID: 32125 RVA: 0x00326F18 File Offset: 0x00325118
	public static void ConsumePower(RobotElectroBankMonitor.Instance smi, float dt)
	{
		if (smi.electrobank == null)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			return;
		}
		float joules = Mathf.Min(dt * smi.def.wattage, smi.electrobank.Charge);
		smi.electrobank.RemovePower(joules, true);
		smi.bankAmount.value = smi.electrobank.Charge;
	}

	// Token: 0x06007D7E RID: 32126 RVA: 0x000F2A55 File Offset: 0x000F0C55
	public static void RequestBattery(RobotElectroBankMonitor.Instance smi)
	{
		if (smi.fetchBatteryChore.IsPaused)
		{
			smi.fetchBatteryChore.Pause(smi.electrobank != null && RobotElectroBankMonitor.ChargeDecent(smi), "FlydoBattery");
		}
	}

	// Token: 0x06007D7F RID: 32127 RVA: 0x000F2A8B File Offset: 0x000F0C8B
	public static bool BatteryDelivered(RobotElectroBankMonitor.Instance smi, object data)
	{
		return data as GameObject != null;
	}

	// Token: 0x04005F01 RID: 24321
	public RobotElectroBankMonitor.PoweredState powered;

	// Token: 0x04005F02 RID: 24322
	public RobotElectroBankMonitor.PowerDown powerdown;

	// Token: 0x04005F03 RID: 24323
	public RobotElectroBankMonitor.PowerUp powerup;

	// Token: 0x04005F04 RID: 24324
	public StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.BoolParameter hasElectrobank;

	// Token: 0x020017CC RID: 6092
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005F05 RID: 24325
		public float lowBatteryWarningPercent;

		// Token: 0x04005F06 RID: 24326
		public float wattage;
	}

	// Token: 0x020017CD RID: 6093
	public class PoweredState : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x04005F07 RID: 24327
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State highBattery;

		// Token: 0x04005F08 RID: 24328
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State lowBattery;
	}

	// Token: 0x020017CE RID: 6094
	public class PowerDown : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x04005F09 RID: 24329
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State pre;

		// Token: 0x04005F0A RID: 24330
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State fall;

		// Token: 0x04005F0B RID: 24331
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State landed;

		// Token: 0x04005F0C RID: 24332
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State dead;
	}

	// Token: 0x020017CF RID: 6095
	public class PowerUp : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x04005F0D RID: 24333
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State flying;

		// Token: 0x04005F0E RID: 24334
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State grounded;

		// Token: 0x04005F0F RID: 24335
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State takeoff;
	}

	// Token: 0x020017D0 RID: 6096
	public new class Instance : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.GameInstance
	{
		// Token: 0x06007D86 RID: 32134 RVA: 0x00326F7C File Offset: 0x0032517C
		public Instance(IStateMachineTarget master, RobotElectroBankMonitor.Def def) : base(master, def)
		{
			this.fetchBatteryChore = base.GetComponent<ManualDeliveryKG>();
			foreach (Storage storage in master.gameObject.GetComponents<Storage>())
			{
				if (storage.storageID == GameTags.ChargedPortableBattery)
				{
					this.electroBankStorage = storage;
					break;
				}
			}
			this.bankAmount = Db.Get().Amounts.InternalElectroBank.Lookup(master.gameObject);
			this.electroBankStorage.Subscribe(-1697596308, new Action<object>(this.ElectroBankStorageChange));
			this.ElectroBankStorageChange(null);
		}

		// Token: 0x06007D87 RID: 32135 RVA: 0x0032701C File Offset: 0x0032521C
		public void ElectroBankStorageChange(object data = null)
		{
			if (this.electroBankStorage.Count > 0)
			{
				this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
				this.bankAmount.value = this.electrobank.Charge;
			}
			else
			{
				this.electrobank = null;
			}
			this.fetchBatteryChore.Pause(this.electrobank != null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
			base.sm.hasElectrobank.Set(this.electrobank != null, this, false);
		}

		// Token: 0x04005F10 RID: 24336
		public Storage electroBankStorage;

		// Token: 0x04005F11 RID: 24337
		public Electrobank electrobank;

		// Token: 0x04005F12 RID: 24338
		public ManualDeliveryKG fetchBatteryChore;

		// Token: 0x04005F13 RID: 24339
		public AmountInstance bankAmount;
	}
}
