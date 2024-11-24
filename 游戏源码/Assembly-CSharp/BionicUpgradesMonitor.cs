using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02001516 RID: 5398
public class BionicUpgradesMonitor : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>
{
	// Token: 0x06007092 RID: 28818 RVA: 0x002F859C File Offset: 0x002F679C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.initialize;
		this.initialize.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.InitializeSlots)).EnterTransition(this.firstSpawn, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsFirstTimeSpawningThisBionic)).GoTo(this.inactive);
		this.firstSpawn.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.SpawnAndInstallInitialUpgrade)).GoTo(this.inactive);
		this.inactive.EventTransition(GameHashes.BionicOnline, this.active, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
		this.active.DefaultState(this.active.idle).EventTransition(GameHashes.BionicOffline, this.inactive, GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Not(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline))).OnSignal(this.UpgradeInstallationStarted, this.installing).OnSignal(this.UpgradeUninstallStarted, this.uninstalling).EventHandler(GameHashes.BionicUpgradeWattageChanged, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers)).ToggleStateMachineList(new Func<BionicUpgradesMonitor.Instance, Func<BionicUpgradesMonitor.Instance, StateMachine.Instance>[]>(BionicUpgradesMonitor.GetUpgradesSMIs));
		this.active.idle.OnSignal(this.UpgradeSlotAssignationChanged, this.active.seeking, new Func<BionicUpgradesMonitor.Instance, bool>(BionicUpgradesMonitor.WantsToInstallNewUpgrades));
		this.active.seeking.OnSignal(this.UpgradeSlotAssignationChanged, this.active.idle, new Func<BionicUpgradesMonitor.Instance, bool>(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).DefaultState(this.active.seeking.unreachable);
		this.active.seeking.unreachable.EventTransition(GameHashes.NavigationCellChanged, this.active.seeking.inProgress, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.CanReachUpgradeToInstall));
		this.active.seeking.inProgress.ToggleChore((BionicUpgradesMonitor.Instance smi) => new SeekAndInstallBionicUpgradeChore(smi.master), this.active.idle, this.active.seeking.failed);
		this.active.seeking.failed.EnterTransition(this.active.idle, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).GoTo(this.active.seeking.unreachable);
		this.installing.ScheduleActionNextFrame("Active Delay", delegate(BionicUpgradesMonitor.Instance smi)
		{
			smi.GoTo(this.active);
		});
		this.uninstalling.ScheduleActionNextFrame("Delayed Redirection", delegate(BionicUpgradesMonitor.Instance smi)
		{
			smi.GoTo(this.active);
		});
	}

	// Token: 0x06007093 RID: 28819 RVA: 0x000E9A47 File Offset: 0x000E7C47
	public static void InitializeSlots(BionicUpgradesMonitor.Instance smi)
	{
		smi.InitializeSlots();
	}

	// Token: 0x06007094 RID: 28820 RVA: 0x000E9A4F File Offset: 0x000E7C4F
	public static bool IsBionicOnline(BionicUpgradesMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06007095 RID: 28821 RVA: 0x000E9A57 File Offset: 0x000E7C57
	public static bool CanReachUpgradeToInstall(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeAssignedAndReachable;
	}

	// Token: 0x06007096 RID: 28822 RVA: 0x000E9A5F File Offset: 0x000E7C5F
	public static bool CanNotReachUpgradeToInstall(BionicUpgradesMonitor.Instance smi)
	{
		return !BionicUpgradesMonitor.CanReachUpgradeToInstall(smi);
	}

	// Token: 0x06007097 RID: 28823 RVA: 0x000E9A6A File Offset: 0x000E7C6A
	public static bool WantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeAssigned;
	}

	// Token: 0x06007098 RID: 28824 RVA: 0x000E9A72 File Offset: 0x000E7C72
	public static bool DoesNotWantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
	{
		return !BionicUpgradesMonitor.WantsToInstallNewUpgrades(smi);
	}

	// Token: 0x06007099 RID: 28825 RVA: 0x000E9A7D File Offset: 0x000E7C7D
	public static bool HasUpgradesInstalled(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeInstalled;
	}

	// Token: 0x0600709A RID: 28826 RVA: 0x000E9A85 File Offset: 0x000E7C85
	public static bool IsFirstTimeSpawningThisBionic(BionicUpgradesMonitor.Instance smi)
	{
		return !smi.sm.InitialUpgradeSpawned.Get(smi);
	}

	// Token: 0x0600709B RID: 28827 RVA: 0x000E9A9B File Offset: 0x000E7C9B
	public static void UpdateBatteryMonitorWattageModifiers(BionicUpgradesMonitor.Instance smi)
	{
		smi.UpdateBatteryMonitorWattageModifiers();
	}

	// Token: 0x0600709C RID: 28828 RVA: 0x000E9AA3 File Offset: 0x000E7CA3
	public static Func<StateMachine.Instance, StateMachine.Instance>[] GetUpgradesSMIs(BionicUpgradesMonitor.Instance smi)
	{
		return smi.GetUpgradesSMIs();
	}

	// Token: 0x0600709D RID: 28829 RVA: 0x002F8850 File Offset: 0x002F6A50
	public static void SpawnAndInstallInitialUpgrade(BionicUpgradesMonitor.Instance smi)
	{
		string text = smi.GetComponent<Traits>().GetTraitIds().Find((string t) => DUPLICANTSTATS.BIONICUPGRADETRAITS.Find((DUPLICANTSTATS.TraitVal st) => st.id == t).id == t);
		if (text != null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(BionicUpgradeComponentConfig.GetBionicUpgradePrefabIDWithTraitID(text)), smi.master.transform.position);
			gameObject.SetActive(true);
			IAssignableIdentity component = smi.GetComponent<IAssignableIdentity>();
			BionicUpgradeComponent component2 = gameObject.GetComponent<BionicUpgradeComponent>();
			component2.Assign(component);
			smi.InstallUpgrade(component2);
		}
		smi.sm.InitialUpgradeSpawned.Set(true, smi, false);
	}

	// Token: 0x04005426 RID: 21542
	public const int MAX_SLOT_COUNT = 3;

	// Token: 0x04005427 RID: 21543
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State initialize;

	// Token: 0x04005428 RID: 21544
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State firstSpawn;

	// Token: 0x04005429 RID: 21545
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State installing;

	// Token: 0x0400542A RID: 21546
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State uninstalling;

	// Token: 0x0400542B RID: 21547
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inactive;

	// Token: 0x0400542C RID: 21548
	public BionicUpgradesMonitor.ActiveStates active;

	// Token: 0x0400542D RID: 21549
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeSlotAssignationChanged;

	// Token: 0x0400542E RID: 21550
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeUninstallStarted;

	// Token: 0x0400542F RID: 21551
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeInstallationStarted;

	// Token: 0x04005430 RID: 21552
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.BoolParameter InitialUpgradeSpawned;

	// Token: 0x02001517 RID: 5399
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005431 RID: 21553
		public int SlotCount = 3;
	}

	// Token: 0x02001518 RID: 5400
	public class SeekingStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x04005432 RID: 21554
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State unreachable;

		// Token: 0x04005433 RID: 21555
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inProgress;

		// Token: 0x04005434 RID: 21556
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State failed;
	}

	// Token: 0x02001519 RID: 5401
	public class ActiveStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x04005435 RID: 21557
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State idle;

		// Token: 0x04005436 RID: 21558
		public BionicUpgradesMonitor.SeekingStates seeking;
	}

	// Token: 0x0200151A RID: 5402
	public new class Instance : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.GameInstance
	{
		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060070A4 RID: 28836 RVA: 0x000E9AD8 File Offset: 0x000E7CD8
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060070A5 RID: 28837 RVA: 0x000E9AEF File Offset: 0x000E7CEF
		public bool HasAnyUpgradeAssigned
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyAssignedSlot() != null;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060070A6 RID: 28838 RVA: 0x000E9B04 File Offset: 0x000E7D04
		public bool HasAnyUpgradeAssignedAndReachable
		{
			get
			{
				return this.GetAnyReachableAssignedSlot() != null;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060070A7 RID: 28839 RVA: 0x000E9B0F File Offset: 0x000E7D0F
		public bool HasAnyUpgradeInstalled
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyInstalledUpgradeSlot() != null;
			}
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x002F88E8 File Offset: 0x002F6AE8
		public Instance(IStateMachineTarget master, BionicUpgradesMonitor.Def def) : base(master, def)
		{
			IAssignableIdentity component = base.GetComponent<IAssignableIdentity>();
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.minionOwnables = component.GetSoleOwner();
			this.upgradesStorage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicUpgradeStorage);
			this.CreateAssignableSlots();
			this.CreateUpgradeSlots();
		}

		// Token: 0x060070A9 RID: 28841 RVA: 0x002F8964 File Offset: 0x002F6B64
		public void InstallUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot slotForAssignedUpgrade = this.GetSlotForAssignedUpgrade(upgradeComponent);
			if (slotForAssignedUpgrade == null)
			{
				return;
			}
			base.sm.UpgradeInstallationStarted.Trigger(this);
			slotForAssignedUpgrade.InternalInstall();
		}

		// Token: 0x060070AA RID: 28842 RVA: 0x000E9B24 File Offset: 0x000E7D24
		public void UninstallUpgrade(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			if (slot != null && slot.HasUpgradeInstalled)
			{
				base.sm.UpgradeUninstallStarted.Trigger(this);
				slot.InternalUninstall();
			}
		}

		// Token: 0x060070AB RID: 28843 RVA: 0x002F8994 File Offset: 0x002F6B94
		public void UpdateBatteryMonitorWattageModifiers()
		{
			bool flag = false;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				string text = "UPGRADE_SLOT_" + i.ToString();
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.HasUpgradeInstalled)
				{
					flag |= this.batteryMonitor.RemoveModifier(text, false);
				}
				else
				{
					BionicBatteryMonitor.WattageModifier modifier = new BionicBatteryMonitor.WattageModifier
					{
						id = text,
						name = upgradeComponentSlot.installedUpgradeComponent.CurrentWattageName,
						value = upgradeComponentSlot.installedUpgradeComponent.CurrentWattage,
						potentialValue = upgradeComponentSlot.installedUpgradeComponent.PotentialWattage
					};
					flag |= this.batteryMonitor.AddOrUpdateModifier(modifier, false);
				}
			}
			if (flag)
			{
				this.batteryMonitor.Trigger(1361471071, null);
			}
		}

		// Token: 0x060070AC RID: 28844 RVA: 0x002F8A60 File Offset: 0x002F6C60
		public Func<StateMachine.Instance, StateMachine.Instance>[] GetUpgradesSMIs()
		{
			List<Func<StateMachine.Instance, StateMachine.Instance>> list = new List<Func<StateMachine.Instance, StateMachine.Instance>>();
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot.installedUpgradeComponent != null && upgradeComponentSlot.StateMachine != null)
				{
					list.Add(upgradeComponentSlot.StateMachine);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060070AD RID: 28845 RVA: 0x002F8AB8 File Offset: 0x002F6CB8
		private void CreateAssignableSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			Equipment component = this.minionOwnables.GetComponent<Equipment>();
			int num = Mathf.Max(0, 2);
			for (int i = 0; i < num; i++)
			{
				string str = (i + 2).ToString();
				if (bionicUpgrade is OwnableSlot)
				{
					OwnableSlotInstance ownableSlotInstance = new OwnableSlotInstance(this.minionOwnables, (OwnableSlot)bionicUpgrade);
					OwnableSlotInstance ownableSlotInstance2 = ownableSlotInstance;
					ownableSlotInstance2.ID += str;
					this.minionOwnables.Add(ownableSlotInstance);
				}
				else if (bionicUpgrade is EquipmentSlot)
				{
					EquipmentSlotInstance equipmentSlotInstance = new EquipmentSlotInstance(component, (EquipmentSlot)bionicUpgrade);
					EquipmentSlotInstance equipmentSlotInstance2 = equipmentSlotInstance;
					equipmentSlotInstance2.ID += str;
					component.Add(equipmentSlotInstance);
				}
			}
		}

		// Token: 0x060070AE RID: 28846 RVA: 0x002F8B7C File Offset: 0x002F6D7C
		private void CreateUpgradeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			AssignableSlotInstance[] slots = this.minionOwnables.GetSlots(bionicUpgrade);
			this.upgradeComponentSlots = new BionicUpgradesMonitor.UpgradeComponentSlot[slots.Length];
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = new BionicUpgradesMonitor.UpgradeComponentSlot();
				this.upgradeComponentSlots[i] = upgradeComponentSlot;
			}
		}

		// Token: 0x060070AF RID: 28847 RVA: 0x002F8BD8 File Offset: 0x002F6DD8
		public void InitializeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			AssignableSlotInstance[] slots = this.minionOwnables.GetSlots(bionicUpgrade);
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				AssignableSlotInstance assignableSlotInstance = slots[i];
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				upgradeComponentSlot.Initialize(assignableSlotInstance, this.upgradesStorage);
				upgradeComponentSlot.OnInstalledUpgradeReassigned = (Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>)Delegate.Combine(upgradeComponentSlot.OnInstalledUpgradeReassigned, new Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>(this.OnInstalledUpgradeComponentReassigned));
				upgradeComponentSlot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(upgradeComponentSlot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnSlotAssignationChanged));
			}
			for (int j = 0; j < this.upgradeComponentSlots.Length; j++)
			{
				this.upgradeComponentSlots[j].OnSpawn(this);
			}
		}

		// Token: 0x060070B0 RID: 28848 RVA: 0x000E9B48 File Offset: 0x000E7D48
		private void OnSlotAssignationChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			base.sm.UpgradeSlotAssignationChanged.Trigger(this);
		}

		// Token: 0x060070B1 RID: 28849 RVA: 0x000E9B5B File Offset: 0x000E7D5B
		private void OnInstalledUpgradeComponentReassigned(BionicUpgradesMonitor.UpgradeComponentSlot slot, IAssignableIdentity new_assignee)
		{
			if (!slot.AssignedUpgradeMatchesInstalledUpgrade)
			{
				this.UninstallUpgrade(slot);
			}
		}

		// Token: 0x060070B2 RID: 28850 RVA: 0x002F8C98 File Offset: 0x002F6E98
		private BionicUpgradesMonitor.UpgradeComponentSlot GetSlotForAssignedUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && upgradeComponentSlot.assignedUpgradeComponent == upgradeComponent)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x060070B3 RID: 28851 RVA: 0x002F8CE8 File Offset: 0x002F6EE8
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyAssignedSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x060070B4 RID: 28852 RVA: 0x002F8D28 File Offset: 0x002F6F28
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyReachableAssignedSlot()
		{
			Navigator component = base.GetComponent<Navigator>();
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && component.CanReach(upgradeComponentSlot.assignedUpgradeComponent.GetComponent<IApproachable>()))
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x060070B5 RID: 28853 RVA: 0x002F8D84 File Offset: 0x002F6F84
		private BionicUpgradesMonitor.UpgradeComponentSlot GetAnyInstalledUpgradeSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && upgradeComponentSlot.HasUpgradeInstalled)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x060070B6 RID: 28854 RVA: 0x002F8DBC File Offset: 0x002F6FBC
		public BionicUpgradesMonitor.UpgradeComponentSlot GetFirstEmptySlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.HasUpgradeInstalled && !upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x04005437 RID: 21559
		[Serialize]
		public BionicUpgradesMonitor.UpgradeComponentSlot[] upgradeComponentSlots;

		// Token: 0x04005438 RID: 21560
		private BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x04005439 RID: 21561
		private Storage upgradesStorage;

		// Token: 0x0400543A RID: 21562
		private Ownables minionOwnables;
	}

	// Token: 0x0200151C RID: 5404
	[SerializationConfig(MemberSerialization.OptIn)]
	public class UpgradeComponentSlot
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060070BA RID: 28858 RVA: 0x000E9B8A File Offset: 0x000E7D8A
		public bool HasUpgradeInstalled
		{
			get
			{
				return this.installedUpgradePrefabID != Tag.Invalid;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060070BB RID: 28859 RVA: 0x000E9B9C File Offset: 0x000E7D9C
		public bool HasUpgradeComponentAssigned
		{
			get
			{
				return this.assignableSlotInstance.IsAssigned() && !this.assignableSlotInstance.IsUnassigning();
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060070BC RID: 28860 RVA: 0x000E9BBB File Offset: 0x000E7DBB
		public bool AssignedUpgradeMatchesInstalledUpgrade
		{
			get
			{
				return this.assignedUpgradeComponent == this.installedUpgradeComponent;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060070BE RID: 28862 RVA: 0x000E9BD7 File Offset: 0x000E7DD7
		// (set) Token: 0x060070BD RID: 28861 RVA: 0x000E9BCE File Offset: 0x000E7DCE
		public bool HasSpawned { get; private set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060070BF RID: 28863 RVA: 0x000E9BDF File Offset: 0x000E7DDF
		public float WattageCost
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return 0f;
				}
				return this.installedUpgradeComponent.CurrentWattage;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060070C0 RID: 28864 RVA: 0x000E9BFA File Offset: 0x000E7DFA
		public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return null;
				}
				return this.installedUpgradeComponent.StateMachine;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060070C1 RID: 28865 RVA: 0x000E9C11 File Offset: 0x000E7E11
		public Tag InstalledUpgradeID
		{
			get
			{
				return this.installedUpgradePrefabID;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060070C2 RID: 28866 RVA: 0x000E9C19 File Offset: 0x000E7E19
		public BionicUpgradeComponent assignedUpgradeComponent
		{
			get
			{
				if (!this.assignableSlotInstance.IsUnassigning())
				{
					return this.assignableSlotInstance.assignable as BionicUpgradeComponent;
				}
				return null;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060070C3 RID: 28867 RVA: 0x002F8DF8 File Offset: 0x002F6FF8
		public BionicUpgradeComponent installedUpgradeComponent
		{
			get
			{
				if (this.HasUpgradeInstalled)
				{
					if (this._installedUpgradeComponent == null)
					{
						global::Debug.LogWarning("Error on BionicUpgradeMonitor. storage does not contains bionic upgrade with id " + this.InstalledUpgradeID.ToString() + " this could be due to loading an old save on a new version");
						this.installedUpgradePrefabID = Tag.Invalid;
					}
					return this._installedUpgradeComponent;
				}
				this._installedUpgradeComponent = null;
				return null;
			}
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x002F8E60 File Offset: 0x002F7060
		public void Initialize(AssignableSlotInstance assignableSlotInstance, Storage storage)
		{
			this.assignableSlotInstance = assignableSlotInstance;
			this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().Subscribe(-1585839766, new Action<object>(this.OnAssignablesChanged));
			this.storage = storage;
			this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
		}

		// Token: 0x060070C6 RID: 28870 RVA: 0x000E9C54 File Offset: 0x000E7E54
		public AssignableSlotInstance GetAssignableSlotInstance()
		{
			return this.assignableSlotInstance;
		}

		// Token: 0x060070C7 RID: 28871 RVA: 0x002F8EB4 File Offset: 0x002F70B4
		public void OnSpawn(BionicUpgradesMonitor.Instance smi)
		{
			if (this.HasUpgradeInstalled && this._installedUpgradeComponent == null)
			{
				GameObject gameObject = null;
				int num = 0;
				List<GameObject> list = new List<GameObject>();
				this.storage.Find(this.InstalledUpgradeID, list);
				while (num < list.Count && this._installedUpgradeComponent == null)
				{
					GameObject gameObject2 = list[num];
					bool flag = false;
					foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
					{
						if (upgradeComponentSlot != this && upgradeComponentSlot.HasSpawned && !(upgradeComponentSlot.InstalledUpgradeID != this.InstalledUpgradeID) && upgradeComponentSlot.installedUpgradeComponent.gameObject == gameObject2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						gameObject = gameObject2;
						break;
					}
					num++;
				}
				if (gameObject != null)
				{
					this._installedUpgradeComponent = gameObject.GetComponent<BionicUpgradeComponent>();
				}
			}
			if (this.HasUpgradeInstalled && this.installedUpgradeComponent != null)
			{
				this.SubscribeToInstallledUpgradeAssignable();
			}
			this.HasSpawned = true;
		}

		// Token: 0x060070C8 RID: 28872 RVA: 0x000E9C5C File Offset: 0x000E7E5C
		public void SubscribeToInstallledUpgradeAssignable()
		{
			this.UnsubscribeFromInstalledUpgradeAssignable();
			this.installedUpgradeSubscribeCallbackIDX = this.installedUpgradeComponent.Subscribe(684616645, new Action<object>(this.OnInstalledComponentReassigned));
		}

		// Token: 0x060070C9 RID: 28873 RVA: 0x000E9C86 File Offset: 0x000E7E86
		public void UnsubscribeFromInstalledUpgradeAssignable()
		{
			if (this.installedUpgradeSubscribeCallbackIDX != -1)
			{
				this.installedUpgradeComponent.Unsubscribe(this.installedUpgradeSubscribeCallbackIDX);
				this.installedUpgradeSubscribeCallbackIDX = -1;
			}
		}

		// Token: 0x060070CA RID: 28874 RVA: 0x002F8FC4 File Offset: 0x002F71C4
		private void OnInstalledComponentReassigned(object obj)
		{
			IAssignableIdentity arg = (obj == null) ? null : ((IAssignableIdentity)obj);
			Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> onInstalledUpgradeReassigned = this.OnInstalledUpgradeReassigned;
			if (onInstalledUpgradeReassigned == null)
			{
				return;
			}
			onInstalledUpgradeReassigned(this, arg);
		}

		// Token: 0x060070CB RID: 28875 RVA: 0x000E9CA9 File Offset: 0x000E7EA9
		private void OnAssignablesChanged(object o)
		{
			if (this._lastAssignedUpgradeComponent != this.assignedUpgradeComponent)
			{
				this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
				Action<BionicUpgradesMonitor.UpgradeComponentSlot> onAssignedUpgradeChanged = this.OnAssignedUpgradeChanged;
				if (onAssignedUpgradeChanged == null)
				{
					return;
				}
				onAssignedUpgradeChanged(this);
			}
		}

		// Token: 0x060070CC RID: 28876 RVA: 0x002F8FF0 File Offset: 0x002F71F0
		public void InternalInstall()
		{
			if (!this.HasUpgradeInstalled && this.HasUpgradeComponentAssigned)
			{
				this.storage.Store(this.assignedUpgradeComponent.gameObject, true, false, true, false);
				this.installedUpgradePrefabID = this.assignedUpgradeComponent.PrefabID();
				this._installedUpgradeComponent = this.assignedUpgradeComponent;
				this.SubscribeToInstallledUpgradeAssignable();
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x060070CD RID: 28877 RVA: 0x002F9078 File Offset: 0x002F7278
		public void InternalUninstall()
		{
			if (this.HasUpgradeInstalled)
			{
				this.UnsubscribeFromInstalledUpgradeAssignable();
				GameObject gameObject = this.installedUpgradeComponent.gameObject;
				this.installedUpgradeComponent.Unassign();
				this.storage.Drop(gameObject, true);
				this.installedUpgradePrefabID = Tag.Invalid;
				this._installedUpgradeComponent = null;
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x0400543E RID: 21566
		private BionicUpgradeComponent _installedUpgradeComponent;

		// Token: 0x0400543F RID: 21567
		private BionicUpgradeComponent _lastAssignedUpgradeComponent;

		// Token: 0x04005440 RID: 21568
		[Serialize]
		private Tag installedUpgradePrefabID = Tag.Invalid;

		// Token: 0x04005441 RID: 21569
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> OnInstalledUpgradeReassigned;

		// Token: 0x04005442 RID: 21570
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot> OnAssignedUpgradeChanged;

		// Token: 0x04005443 RID: 21571
		private AssignableSlotInstance assignableSlotInstance;

		// Token: 0x04005444 RID: 21572
		private Storage storage;

		// Token: 0x04005445 RID: 21573
		private int installedUpgradeSubscribeCallbackIDX = -1;
	}
}
