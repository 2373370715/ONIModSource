using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001509 RID: 5385
public class BionicOxygenTankMonitor : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>
{
	// Token: 0x0600704F RID: 28751 RVA: 0x002F7C10 File Offset: 0x002F5E10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.fistSpawn;
		this.fistSpawn.ParamTransition<bool>(this.HasSpawnedBefore, this.safe, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.IsTrue).Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.StartWithFullTank));
		this.safe.Transition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)), UpdateRate.SIM_200ms);
		this.lowOxygen.Transition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe), UpdateRate.SIM_200ms).Transition(this.critical, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical), UpdateRate.SIM_200ms).DefaultState(this.lowOxygen.idle);
		this.lowOxygen.idle.EventTransition(GameHashes.ScheduleBlocksChanged, this.lowOxygen.scheduleSearch, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule)).EventTransition(GameHashes.ScheduleChanged, this.lowOxygen.scheduleSearch, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule));
		this.lowOxygen.scheduleSearch.EventTransition(GameHashes.ScheduleBlocksChanged, this.lowOxygen.idle, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule))).EventTransition(GameHashes.ScheduleChanged, this.lowOxygen.idle, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule))).Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).ToggleUrge(Db.Get().Urges.FindOxygenRefill).ToggleRecurringChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master), null).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors));
		this.critical.ToggleUrge(Db.Get().Urges.FindOxygenRefill).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors)).DefaultState(this.critical.enableSensors);
		this.critical.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo(this.critical.seekingOxygenSourceMode);
		this.critical.seekingOxygenSourceMode.Transition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical)), UpdateRate.SIM_200ms).OnSignal(this.ClosestOxygenSourceChanged, this.critical.environmentAbsorbMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.NoOxygenSourceAvailable)).OnSignal(this.OxygenSourceItemLostSignal, this.critical.environmentAbsorbMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.NoOxygenSourceAvailable)).ToggleRecurringChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master), null);
		this.critical.environmentAbsorbMode.DefaultState(this.critical.environmentAbsorbMode.running);
		this.critical.environmentAbsorbMode.running.ToggleChore((BionicOxygenTankMonitor.Instance smi) => new BionicMassOxygenAbsorbChore(smi.master), this.critical.environmentAbsorbMode.success, this.critical.environmentAbsorbMode.failed);
		this.critical.environmentAbsorbMode.failed.EnterTransition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.seekingOxygenSourceMode);
		this.critical.environmentAbsorbMode.success.EnterTransition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.seekingOxygenSourceMode);
	}

	// Token: 0x06007050 RID: 28752 RVA: 0x000E9787 File Offset: 0x000E7987
	public static bool IsAllowedToSeekOxygenSourceItemsBySchedule(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.IsAllowedToSeekOxygenBySchedule;
	}

	// Token: 0x06007051 RID: 28753 RVA: 0x000E978F File Offset: 0x000E798F
	public static bool AreOxygenLevelsSafe(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage >= 0.5f;
	}

	// Token: 0x06007052 RID: 28754 RVA: 0x000E97A1 File Offset: 0x000E79A1
	public static bool AreOxygenLevelsCritical(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage < 0.2f;
	}

	// Token: 0x06007053 RID: 28755 RVA: 0x000E97B0 File Offset: 0x000E79B0
	public static bool IsThereAnOxygenSourceItemAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() != null;
	}

	// Token: 0x06007054 RID: 28756 RVA: 0x000E97BE File Offset: 0x000E79BE
	public static bool NoOxygenSourceAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() == null;
	}

	// Token: 0x06007055 RID: 28757 RVA: 0x000E97CC File Offset: 0x000E79CC
	public static void StartWithFullTank(BionicOxygenTankMonitor.Instance smi)
	{
		smi.AddFirstTimeSpawnedOxygen();
	}

	// Token: 0x06007056 RID: 28758 RVA: 0x000E97D4 File Offset: 0x000E79D4
	public static void EnableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(true);
	}

	// Token: 0x06007057 RID: 28759 RVA: 0x000E97DD File Offset: 0x000E79DD
	public static void DisableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(false);
	}

	// Token: 0x040053F2 RID: 21490
	public const SimHashes INITIAL_TANK_ELEMENT = SimHashes.Oxygen;

	// Token: 0x040053F3 RID: 21491
	public static readonly Tag INITIAL_TANK_ELEMENT_TAG = SimHashes.Oxygen.CreateTag();

	// Token: 0x040053F4 RID: 21492
	public const float SAFE_TRESHOLD = 0.5f;

	// Token: 0x040053F5 RID: 21493
	public const float CRITICAL_TRESHOLD = 0.2f;

	// Token: 0x040053F6 RID: 21494
	public const float OXYGEN_TANK_CAPACITY_IN_SECONDS = 1800f;

	// Token: 0x040053F7 RID: 21495
	public static readonly float OXYGEN_TANK_CAPACITY_KG = 1800f * DUPLICANTSTATS.BIONICS.BaseStats.OXYGEN_USED_PER_SECOND;

	// Token: 0x040053F8 RID: 21496
	public static float INITIAL_OXYGEN_TEMP = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;

	// Token: 0x040053F9 RID: 21497
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State fistSpawn;

	// Token: 0x040053FA RID: 21498
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State safe;

	// Token: 0x040053FB RID: 21499
	public BionicOxygenTankMonitor.LowOxygenStates lowOxygen;

	// Token: 0x040053FC RID: 21500
	public BionicOxygenTankMonitor.CriticalStates critical;

	// Token: 0x040053FD RID: 21501
	private StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.BoolParameter HasSpawnedBefore;

	// Token: 0x040053FE RID: 21502
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal OxygenSourceItemLostSignal;

	// Token: 0x040053FF RID: 21503
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal ClosestOxygenSourceChanged;

	// Token: 0x0200150A RID: 5386
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200150B RID: 5387
	public class EnvironmentAbsorbStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x04005400 RID: 21504
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State running;

		// Token: 0x04005401 RID: 21505
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State success;

		// Token: 0x04005402 RID: 21506
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State failed;
	}

	// Token: 0x0200150C RID: 5388
	public class CriticalStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x04005403 RID: 21507
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State enableSensors;

		// Token: 0x04005404 RID: 21508
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State seekingOxygenSourceMode;

		// Token: 0x04005405 RID: 21509
		public BionicOxygenTankMonitor.EnvironmentAbsorbStates environmentAbsorbMode;
	}

	// Token: 0x0200150D RID: 5389
	public class LowOxygenStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x04005406 RID: 21510
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State idle;

		// Token: 0x04005407 RID: 21511
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State scheduleSearch;
	}

	// Token: 0x0200150E RID: 5390
	public new class Instance : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.GameInstance, OxygenBreather.IGasProvider
	{
		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600705E RID: 28766 RVA: 0x000E97F6 File Offset: 0x000E79F6
		public bool IsAllowedToSeekOxygenBySchedule
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600705F RID: 28767 RVA: 0x000E9812 File Offset: 0x000E7A12
		public bool IsEmpty
		{
			get
			{
				return this.AvailableOxygen == 0f;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06007060 RID: 28768 RVA: 0x000E9821 File Offset: 0x000E7A21
		public float OxygenPercentage
		{
			get
			{
				return this.AvailableOxygen / this.storage.capacityKg;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06007061 RID: 28769 RVA: 0x000E9835 File Offset: 0x000E7A35
		public float AvailableOxygen
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.Breathable);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06007062 RID: 28770 RVA: 0x000E9847 File Offset: 0x000E7A47
		public float SpaceAvailableInTank
		{
			get
			{
				return this.storage.capacityKg - this.AvailableOxygen;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06007064 RID: 28772 RVA: 0x000E9864 File Offset: 0x000E7A64
		// (set) Token: 0x06007063 RID: 28771 RVA: 0x000E985B File Offset: 0x000E7A5B
		public Storage storage { get; private set; }

		// Token: 0x06007065 RID: 28773 RVA: 0x002F8004 File Offset: 0x002F6204
		public Instance(IStateMachineTarget master, BionicOxygenTankMonitor.Def def) : base(master, def)
		{
			Sensors component = base.GetComponent<Sensors>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.oxygenSourceSensors = new ClosestPickupableSensor<Pickupable>[]
			{
				component.GetSensor<ClosestOxygenCanisterSensor>(),
				component.GetSensor<ClosestOxyliteSensor>()
			};
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.OnItemChanged = (Action<Pickupable>)Delegate.Combine(closestPickupableSensor.OnItemChanged, new Action<Pickupable>(this.OnOxygenSourceSensorItemChanged));
			}
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicOxygenTankStorage);
			this.oxygenTankAmountInstance = Db.Get().Amounts.BionicOxygenTank.Lookup(base.gameObject);
			this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x000E986C File Offset: 0x000E7A6C
		public Pickupable GetClosestOxygenSource()
		{
			return this.closestOxygenSource;
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x000E9874 File Offset: 0x000E7A74
		private void OnOxygenSourceSensorItemChanged(object o)
		{
			this.CompareOxygenSources();
		}

		// Token: 0x06007068 RID: 28776 RVA: 0x000E987C File Offset: 0x000E7A7C
		private void OnOxygenTankStorageChanged(object o)
		{
			this.RefreshAmountInstance();
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x000E9884 File Offset: 0x000E7A84
		public void RefreshAmountInstance()
		{
			this.oxygenTankAmountInstance.SetValue(this.AvailableOxygen);
		}

		// Token: 0x0600706A RID: 28778 RVA: 0x002F8124 File Offset: 0x002F6324
		public void AddFirstTimeSpawnedOxygen()
		{
			this.storage.AddElement(SimHashes.Oxygen, this.storage.capacityKg - this.AvailableOxygen, BionicOxygenTankMonitor.INITIAL_OXYGEN_TEMP, byte.MaxValue, 0, false, true);
			base.sm.HasSpawnedBefore.Set(true, this, false);
		}

		// Token: 0x0600706B RID: 28779 RVA: 0x002F8178 File Offset: 0x002F6378
		private void CompareOxygenSources()
		{
			Pickupable item = this.closestOxygenSource;
			float num = 2.1474836E+09f;
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				int itemNavCost = closestPickupableSensor.GetItemNavCost();
				if ((float)itemNavCost < num)
				{
					num = (float)itemNavCost;
					item = closestPickupableSensor.GetItem();
				}
			}
			bool flag = item != this.closestOxygenSource;
			this.closestOxygenSource = item;
			if (flag)
			{
				base.sm.ClosestOxygenSourceChanged.Trigger(this);
			}
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x000E9898 File Offset: 0x000E7A98
		public float AddGas(Sim.MassConsumedCallback mass_cb_info)
		{
			return this.AddGas(ElementLoader.elements[(int)mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
		}

		// Token: 0x0600706D RID: 28781 RVA: 0x002F81F0 File Offset: 0x002F63F0
		public float AddGas(SimHashes element, float mass, float temperature, byte disseaseIDX = 255, int _disseaseCount = 0)
		{
			float num = Mathf.Min(mass, this.SpaceAvailableInTank);
			float result = mass - num;
			float num2 = num / mass;
			int disease_count = Mathf.CeilToInt((float)_disseaseCount * num2);
			this.storage.AddElement(element, num, temperature, disseaseIDX, disease_count, false, true);
			return result;
		}

		// Token: 0x0600706E RID: 28782 RVA: 0x002F8230 File Offset: 0x002F6430
		public void SetOxygenSourceSensorsActiveState(bool shouldItBeActive)
		{
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.SetActive(shouldItBeActive);
				if (shouldItBeActive)
				{
					closestPickupableSensor.Update();
				}
			}
		}

		// Token: 0x0600706F RID: 28783 RVA: 0x002F826C File Offset: 0x002F646C
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.IsEmpty)
			{
				return false;
			}
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out num2);
			Game.Instance.accumulators.Accumulate(oxygen_breather.O2Accumulator, num);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, -num, oxygen_breather.GetProperName(), null);
			return true;
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06007072 RID: 28786 RVA: 0x000E98CD File Offset: 0x000E7ACD
		public bool IsLowOxygen()
		{
			return this.IsEmpty;
		}

		// Token: 0x06007073 RID: 28787 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06007074 RID: 28788 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06007075 RID: 28789 RVA: 0x000E98D5 File Offset: 0x000E7AD5
		protected override void OnCleanUp()
		{
			if (this.storage != null)
			{
				Storage storage = this.storage;
				storage.OnStorageChange = (Action<GameObject>)Delegate.Remove(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x04005408 RID: 21512
		public AttributeInstance airConsumptionRate;

		// Token: 0x04005409 RID: 21513
		private Schedulable schedulable;

		// Token: 0x0400540A RID: 21514
		private AmountInstance oxygenTankAmountInstance;

		// Token: 0x0400540B RID: 21515
		private ClosestPickupableSensor<Pickupable>[] oxygenSourceSensors;

		// Token: 0x0400540C RID: 21516
		private Pickupable closestOxygenSource;
	}
}
