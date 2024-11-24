using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020014F6 RID: 5366
public class BionicBatteryMonitor : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>
{
	// Token: 0x06006FD3 RID: 28627 RVA: 0x002F6384 File Offset: 0x002F4584
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.firstSpawn;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.RefreshCharge));
		this.firstSpawn.ParamTransition<bool>(this.InitialElectrobanksSpawned, this.online, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsTrue).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SpawnAndInstallInitialElectrobanks));
		this.online.TriggerOnEnter(GameHashes.BionicOnline, null).Transition(this.offline, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.DoesNotHaveCharge), UpdateRate.SIM_200ms).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.ReorganizeElectrobankStorage)).Update(new Action<BionicBatteryMonitor.Instance, float>(BionicBatteryMonitor.DischargeUpdate), UpdateRate.SIM_200ms, false).DefaultState(this.online.idle);
		this.online.idle.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.OnStorageChange, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep));
		this.online.batterySaveMode.EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).EventTransition(GameHashes.ScheduleChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).DefaultState(this.online.batterySaveMode.idle).Exit(delegate(BionicBatteryMonitor.Instance smi)
		{
			smi.master.Trigger(-120107884, null);
		});
		this.online.batterySaveMode.idle.ToggleChore((BionicBatteryMonitor.Instance smi) => new BeInBatterySaveModeChore(smi.master), this.online.idle, this.online.batterySaveMode.failed).DefaultState(this.online.batterySaveMode.idle.normal);
		this.online.batterySaveMode.idle.normal.ParamTransition<int>(this.ChargedElectrobankCount, this.online.batterySaveMode.idle.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int);
		this.online.batterySaveMode.idle.critical.ParamTransition<int>(this.ChargedElectrobankCount, this.online.batterySaveMode.idle.normal, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTOne_Int).OnSignal(this.OnClosestAvailableElectrobankChangedSignal, this.online.batterySaveMode.idle.exit, new Func<BionicBatteryMonitor.Instance, bool>(BionicBatteryMonitor.IsCriticallyLowAndThereIsAvailableElectrobank));
		this.online.batterySaveMode.idle.exit.ScheduleGoTo(10f, this.online.idle);
		this.online.batterySaveMode.failed.EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).GoTo(this.online.batterySaveMode.idle);
		this.online.upkeep.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.OnStorageChange, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).DefaultState(this.online.upkeep.emptyDepletedElectrobank);
		this.online.upkeep.emptyDepletedElectrobank.ParamTransition<int>(this.DepletedElectrobankCount, this.online.upkeep.seekElectrobank, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsZero_Int).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new RemoveDischargedElectrobankChore(smi.master), null).ToggleUrge(Db.Get().Urges.RemoveDischargedElectrobank);
		this.online.upkeep.seekElectrobank.ParamTransition<int>(this.DepletedElectrobankCount, this.online.upkeep.emptyDepletedElectrobank, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<int>.Callback(BionicBatteryMonitor.DoesNotHaveSpaceForElectrobankAndHasDepletedBatteriesStored)).EventTransition(GameHashes.OnStorageChange, this.online.upkeep.emptyDepletedElectrobank, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToGetRidOfDepletedBattery)).ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), this.online.idle);
		this.online.critical.DefaultState(this.online.critical.seekElectrobank).ParamTransition<int>(this.ChargedElectrobankCount, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTOne_Int).DoTutorial(Tutorial.TutorialMessages.TM_BionicBattery);
		this.online.critical.seekElectrobank.EventTransition(GameHashes.ScheduleBlocksChanged, this.online.batterySaveMode, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.IsBatterySaveModeTime(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EventTransition(GameHashes.ScheduleChanged, this.online.batterySaveMode, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.IsBatterySaveModeTime(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.batterySaveMode, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.IsBatterySaveModeTime(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EnterTransition(this.online.critical.emptyDepletedElectrobank, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.HasSpaceForNewElectrobank))).EnterTransition(this.online.critical.emptyDepletedElectrobank, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EventTransition(GameHashes.TargetElectrobankLost, this.online.critical.emptyDepletedElectrobank, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), null).ToggleUrge(Db.Get().Urges.ReloadElectrobank);
		this.online.critical.emptyDepletedElectrobank.ParamTransition<int>(this.DepletedElectrobankCount, this.online.critical.seekElectrobank, (BionicBatteryMonitor.Instance smi, int v) => BionicBatteryMonitor.HasSpaceForNewElectrobank(smi) && (BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi) || v == 0)).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new RemoveDischargedElectrobankChore(smi.master), null).ToggleUrge(Db.Get().Urges.RemoveDischargedElectrobank);
		this.offline.DefaultState(this.offline.waitingForBatteryDelivery).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new BeOfflineChore(smi.master), null).ToggleUrge(Db.Get().Urges.BeOffline).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOffline)).TriggerOnEnter(GameHashes.BionicOffline, null);
		this.offline.waitingForBatteryDelivery.ParamTransition<int>(this.ChargedElectrobankCount, this.offline.waitingForBatteryInstallation, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTZero_Int).Toggle("Enable Delivery of new Electrobanks", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnableManualDelivery), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisableManualDelivery));
		this.offline.waitingForBatteryInstallation.Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.StartReanimateWorkChore)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.CancelReanimateWorkChore)).WorkableCompleteTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.reboot).DefaultState(this.offline.waitingForBatteryInstallation.waiting);
		this.offline.waitingForBatteryInstallation.waiting.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWaitingForReboot, null).WorkableStartTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.working);
		this.offline.waitingForBatteryInstallation.working.WorkableStopTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.waiting);
		this.offline.reboot.PlayAnim("power_up").OnAnimQueueComplete(this.online).ScheduleGoTo(10f, this.online).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.AutomaticallyDropAllDepletedElectrobanks)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOnline));
	}

	// Token: 0x06006FD4 RID: 28628 RVA: 0x000E9227 File Offset: 0x000E7427
	public static ReanimateBionicWorkable GetReanimateWorkable(BionicBatteryMonitor.Instance smi)
	{
		return smi.reanimateWorkable;
	}

	// Token: 0x06006FD5 RID: 28629 RVA: 0x000E922F File Offset: 0x000E742F
	public static float CurrentCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge;
	}

	// Token: 0x06006FD6 RID: 28630 RVA: 0x000E9237 File Offset: 0x000E7437
	public static bool IsCriticallyLowAndThereIsAvailableElectrobank(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.IsCriticallyLow(smi) && BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi);
	}

	// Token: 0x06006FD7 RID: 28631 RVA: 0x000E9249 File Offset: 0x000E7449
	public static bool IsBatterySaveModeTime(BionicBatteryMonitor.Instance smi)
	{
		return smi.InBatterySaveModeTime;
	}

	// Token: 0x06006FD8 RID: 28632 RVA: 0x000E9251 File Offset: 0x000E7451
	public static bool HasCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge > 0f;
	}

	// Token: 0x06006FD9 RID: 28633 RVA: 0x000E9260 File Offset: 0x000E7460
	public static bool DoesNotHaveCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge <= 0f;
	}

	// Token: 0x06006FDA RID: 28634 RVA: 0x000E9272 File Offset: 0x000E7472
	public static bool IsCriticallyLow(BionicBatteryMonitor.Instance smi)
	{
		return smi.ChargedElectrobankCount <= 1;
	}

	// Token: 0x06006FDB RID: 28635 RVA: 0x000E9280 File Offset: 0x000E7480
	public static bool IsAnyElectrobankAvailableToBeFetched(BionicBatteryMonitor.Instance smi)
	{
		return smi.GetClosestElectrobank() != null;
	}

	// Token: 0x06006FDC RID: 28636 RVA: 0x000E928E File Offset: 0x000E748E
	public static bool WantsToGetRidOfDepletedBattery(BionicBatteryMonitor.Instance smi)
	{
		return smi.DepletedElectrobankCount > 0;
	}

	// Token: 0x06006FDD RID: 28637 RVA: 0x000E9299 File Offset: 0x000E7499
	public static bool WantsToInstallNewBattery(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.IsCriticallyLow(smi) || (smi.InUpkeepTime && smi.ChargedElectrobankCount < 3);
	}

	// Token: 0x06006FDE RID: 28638 RVA: 0x000E92B8 File Offset: 0x000E74B8
	public static bool WantsToUpkeep(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi) || BionicBatteryMonitor.WantsToInstallNewBattery(smi);
	}

	// Token: 0x06006FDF RID: 28639 RVA: 0x000E92CA File Offset: 0x000E74CA
	public static bool HasSpaceForNewElectrobank(BionicBatteryMonitor.Instance smi)
	{
		return smi.HasSpaceForNewElectrobank;
	}

	// Token: 0x06006FE0 RID: 28640 RVA: 0x000E92D2 File Offset: 0x000E74D2
	public static bool DoesNotHaveSpaceForElectrobankAndHasDepletedBatteriesStored(BionicBatteryMonitor.Instance smi, int depletedAmount)
	{
		return !BionicBatteryMonitor.HasSpaceForNewElectrobank(smi) && BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi);
	}

	// Token: 0x06006FE1 RID: 28641 RVA: 0x000E92E4 File Offset: 0x000E74E4
	public static void SpawnAndInstallInitialElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.SpawnAndInstallInitialElectrobanks();
	}

	// Token: 0x06006FE2 RID: 28642 RVA: 0x000E92EC File Offset: 0x000E74EC
	public static void RefreshCharge(BionicBatteryMonitor.Instance smi)
	{
		smi.RefreshCharge();
	}

	// Token: 0x06006FE3 RID: 28643 RVA: 0x000E92F4 File Offset: 0x000E74F4
	public static void EnableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(true);
	}

	// Token: 0x06006FE4 RID: 28644 RVA: 0x000E92FD File Offset: 0x000E74FD
	public static void DisableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(false);
	}

	// Token: 0x06006FE5 RID: 28645 RVA: 0x000E9306 File Offset: 0x000E7506
	public static void StartReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06006FE6 RID: 28646 RVA: 0x000E930E File Offset: 0x000E750E
	public static void CancelReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06006FE7 RID: 28647 RVA: 0x000E9316 File Offset: 0x000E7516
	public static void SetOffline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(false);
	}

	// Token: 0x06006FE8 RID: 28648 RVA: 0x000E931F File Offset: 0x000E751F
	public static void SetOnline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(true);
	}

	// Token: 0x06006FE9 RID: 28649 RVA: 0x000E9328 File Offset: 0x000E7528
	public static void AutomaticallyDropAllDepletedElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.AutomaticallyDropAllDepletedElectrobanks();
	}

	// Token: 0x06006FEA RID: 28650 RVA: 0x000E9330 File Offset: 0x000E7530
	public static void ReorganizeElectrobankStorage(BionicBatteryMonitor.Instance smi)
	{
		smi.ReorganizeElectrobanks();
	}

	// Token: 0x06006FEB RID: 28651 RVA: 0x002F6E24 File Offset: 0x002F5024
	public static void DischargeUpdate(BionicBatteryMonitor.Instance smi, float dt)
	{
		float joules = Mathf.Min(dt * smi.Wattage, smi.CurrentCharge);
		smi.ConsumePower(joules);
	}

	// Token: 0x04005399 RID: 21401
	public const int MAX_ELECTROBANK_COUNT = 3;

	// Token: 0x0400539A RID: 21402
	public const float BATTERY_SAVE_WATTAGE = 20f;

	// Token: 0x0400539B RID: 21403
	public const float MIN_WATTS = 200f;

	// Token: 0x0400539C RID: 21404
	public const float MAX_WATTS = 2000f;

	// Token: 0x0400539D RID: 21405
	public const float EXIT_BATTERY_SAVE_MODE_TIMEOUT = 10f;

	// Token: 0x0400539E RID: 21406
	public const string INITIAL_ELECTROBANK_TYPE_ID = "DisposableElectrobank_BasicSingleHarvestPlant";

	// Token: 0x0400539F RID: 21407
	public static readonly string ChargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_charged_electrobank\">";

	// Token: 0x040053A0 RID: 21408
	public static readonly string DischargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_discharged_electrobank\">";

	// Token: 0x040053A1 RID: 21409
	public static readonly string CriticalBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_critical_electrobank\">";

	// Token: 0x040053A2 RID: 21410
	public static readonly string SavingBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_saving_electrobank\">";

	// Token: 0x040053A3 RID: 21411
	public static readonly string EmptySlotBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_empty_slot_electrobank\">";

	// Token: 0x040053A4 RID: 21412
	private const string ANIM_NAME_REBOOT = "power_up";

	// Token: 0x040053A5 RID: 21413
	public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State firstSpawn;

	// Token: 0x040053A6 RID: 21414
	public BionicBatteryMonitor.OnlineStates online;

	// Token: 0x040053A7 RID: 21415
	public BionicBatteryMonitor.OfflineStates offline;

	// Token: 0x040053A8 RID: 21416
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter ChargedElectrobankCount;

	// Token: 0x040053A9 RID: 21417
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter DepletedElectrobankCount;

	// Token: 0x040053AA RID: 21418
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnClosestAvailableElectrobankChangedSignal;

	// Token: 0x040053AB RID: 21419
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter InitialElectrobanksSpawned;

	// Token: 0x040053AC RID: 21420
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter IsOnline;

	// Token: 0x020014F7 RID: 5367
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014F8 RID: 5368
	public struct WattageModifier
	{
		// Token: 0x06006FEF RID: 28655 RVA: 0x000E9374 File Offset: 0x000E7574
		public WattageModifier(string id, string name, float value, float potentialValue)
		{
			this.id = id;
			this.name = name;
			this.value = value;
			this.potentialValue = potentialValue;
		}

		// Token: 0x040053AD RID: 21421
		public float potentialValue;

		// Token: 0x040053AE RID: 21422
		public float value;

		// Token: 0x040053AF RID: 21423
		public string name;

		// Token: 0x040053B0 RID: 21424
		public string id;
	}

	// Token: 0x020014F9 RID: 5369
	public class OnlineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040053B1 RID: 21425
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State idle;

		// Token: 0x040053B2 RID: 21426
		public BionicBatteryMonitor.BatterySaveModeStates batterySaveMode;

		// Token: 0x040053B3 RID: 21427
		public BionicBatteryMonitor.UpkeepStates upkeep;

		// Token: 0x040053B4 RID: 21428
		public BionicBatteryMonitor.UpkeepStates critical;
	}

	// Token: 0x020014FA RID: 5370
	public class BatterySaveModeStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040053B5 RID: 21429
		public BionicBatteryMonitor.BatterySaveModeIdleStates idle;

		// Token: 0x040053B6 RID: 21430
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State failed;
	}

	// Token: 0x020014FB RID: 5371
	public class BatterySaveModeIdleStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040053B7 RID: 21431
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State normal;

		// Token: 0x040053B8 RID: 21432
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State critical;

		// Token: 0x040053B9 RID: 21433
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State exit;
	}

	// Token: 0x020014FC RID: 5372
	public class UpkeepStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040053BA RID: 21434
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State emptyDepletedElectrobank;

		// Token: 0x040053BB RID: 21435
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State seekElectrobank;
	}

	// Token: 0x020014FD RID: 5373
	public class OfflineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040053BC RID: 21436
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waitingForBatteryDelivery;

		// Token: 0x040053BD RID: 21437
		public BionicBatteryMonitor.RebootWorkableState waitingForBatteryInstallation;

		// Token: 0x040053BE RID: 21438
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State reboot;
	}

	// Token: 0x020014FE RID: 5374
	public class RebootWorkableState : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040053BF RID: 21439
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waiting;

		// Token: 0x040053C0 RID: 21440
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State working;
	}

	// Token: 0x020014FF RID: 5375
	public new class Instance : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.GameInstance
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06006FF6 RID: 28662 RVA: 0x000E939B File Offset: 0x000E759B
		public float Wattage
		{
			get
			{
				return this.GetBaseWattage() + this.GetModifiersWattage();
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06006FF7 RID: 28663 RVA: 0x000E93AA File Offset: 0x000E75AA
		public bool IsBatterySaveModeActive
		{
			get
			{
				return this.prefabID.HasTag(GameTags.BatterySaveMode);
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06006FF8 RID: 28664 RVA: 0x000E93BC File Offset: 0x000E75BC
		public bool IsOnline
		{
			get
			{
				return base.sm.IsOnline.Get(this);
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x000E93CF File Offset: 0x000E75CF
		public bool InBatterySaveModeTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep);
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06006FFA RID: 28666 RVA: 0x000E93EB File Offset: 0x000E75EB
		public bool InUpkeepTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06006FFB RID: 28667 RVA: 0x000E9407 File Offset: 0x000E7607
		public bool HaveInitialElectrobanksBeenSpawned
		{
			get
			{
				return base.sm.InitialElectrobanksSpawned.Get(this);
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06006FFC RID: 28668 RVA: 0x000E941A File Offset: 0x000E761A
		public bool HasSpaceForNewElectrobank
		{
			get
			{
				return this.ChargedElectrobankCount + this.DepletedElectrobankCount < 3;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06006FFD RID: 28669 RVA: 0x000E942C File Offset: 0x000E762C
		public int ChargedElectrobankCount
		{
			get
			{
				return base.sm.ChargedElectrobankCount.Get(this);
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06006FFE RID: 28670 RVA: 0x000E943F File Offset: 0x000E763F
		public int DepletedElectrobankCount
		{
			get
			{
				return (int)(this.storage.GetMassAvailable("EmptyElectrobank") / 20f);
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06006FFF RID: 28671 RVA: 0x000E945D File Offset: 0x000E765D
		public int DamagedElectrobankCount
		{
			get
			{
				return (int)(this.storage.GetMassAvailable("GarbageElectrobank") / 20f);
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06007000 RID: 28672 RVA: 0x000E947B File Offset: 0x000E767B
		public float CurrentCharge
		{
			get
			{
				return this.BionicBattery.value;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06007002 RID: 28674 RVA: 0x000E9491 File Offset: 0x000E7691
		// (set) Token: 0x06007001 RID: 28673 RVA: 0x000E9488 File Offset: 0x000E7688
		public ReanimateBionicWorkable reanimateWorkable { get; private set; }

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06007004 RID: 28676 RVA: 0x000E94A2 File Offset: 0x000E76A2
		// (set) Token: 0x06007003 RID: 28675 RVA: 0x000E9499 File Offset: 0x000E7699
		public List<BionicBatteryMonitor.WattageModifier> Modifiers { get; set; } = new List<BionicBatteryMonitor.WattageModifier>();

		// Token: 0x06007005 RID: 28677 RVA: 0x002F6E4C File Offset: 0x002F504C
		public Instance(IStateMachineTarget master, BionicBatteryMonitor.Def def) : base(master, def)
		{
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicBatteryStorage);
			this.reanimateWorkable = base.GetComponent<ReanimateBionicWorkable>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.manualDelivery = base.GetComponent<ManualDeliveryKG>();
			this.selectable = base.GetComponent<KSelectable>();
			this.prefabID = base.GetComponent<KPrefabID>();
			this.BionicBattery = Db.Get().Amounts.BionicInternalBattery.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeath));
			Storage storage2 = this.storage;
			storage2.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage2.OnStorageChange, new Action<GameObject>(this.OnElectrobankStorageChanged));
		}

		// Token: 0x06007006 RID: 28678 RVA: 0x002F6F4C File Offset: 0x002F514C
		public override void StartSM()
		{
			this.closestElectrobankSensor = base.GetComponent<Sensors>().GetSensor<ClosestElectrobankSensor>();
			ClosestElectrobankSensor closestElectrobankSensor = this.closestElectrobankSensor;
			closestElectrobankSensor.OnItemChanged = (Action<Electrobank>)Delegate.Combine(closestElectrobankSensor.OnItemChanged, new Action<Electrobank>(this.OnClosestElectrobankChanged));
			base.StartSM();
			this.RefreshCharge();
		}

		// Token: 0x06007007 RID: 28679 RVA: 0x000E94AA File Offset: 0x000E76AA
		private void OnClosestElectrobankChanged(Electrobank newItem)
		{
			base.sm.OnClosestAvailableElectrobankChangedSignal.Trigger(this);
		}

		// Token: 0x06007008 RID: 28680 RVA: 0x000E94BD File Offset: 0x000E76BD
		public float GetBaseWattage()
		{
			if (!this.IsBatterySaveModeActive)
			{
				return 200f;
			}
			return 20f;
		}

		// Token: 0x06007009 RID: 28681 RVA: 0x002F6FA0 File Offset: 0x002F51A0
		public float GetModifiersWattage()
		{
			float num = 0f;
			foreach (BionicBatteryMonitor.WattageModifier wattageModifier in this.Modifiers)
			{
				num += wattageModifier.value;
			}
			return num;
		}

		// Token: 0x0600700A RID: 28682 RVA: 0x000E94D2 File Offset: 0x000E76D2
		private void OnElectrobankStorageChanged(object o)
		{
			this.ReorganizeElectrobanks();
		}

		// Token: 0x0600700B RID: 28683 RVA: 0x000E94DA File Offset: 0x000E76DA
		public void ReorganizeElectrobanks()
		{
			this.storage.items.Sort(delegate(GameObject b1, GameObject b2)
			{
				Electrobank component = b1.GetComponent<Electrobank>();
				Electrobank component2 = b2.GetComponent<Electrobank>();
				if (component == null)
				{
					return -1;
				}
				if (component2 == null)
				{
					return 1;
				}
				return component.Charge.CompareTo(component2.Charge);
			});
		}

		// Token: 0x0600700C RID: 28684 RVA: 0x002F6FFC File Offset: 0x002F51FC
		public void CreateWorkableChore()
		{
			if (this.reanimateChore == null)
			{
				this.reanimateChore = new WorkChore<ReanimateBionicWorkable>(Db.Get().ChoreTypes.RescueIncapacitated, this.reanimateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x0600700D RID: 28685 RVA: 0x000E950B File Offset: 0x000E770B
		public void CancelWorkChore()
		{
			if (this.reanimateChore != null)
			{
				this.reanimateChore.Cancel("BionicBatteryMonitor.CancelChore");
				this.reanimateChore = null;
			}
		}

		// Token: 0x0600700E RID: 28686 RVA: 0x000E952C File Offset: 0x000E772C
		public void SetOnlineState(bool online)
		{
			base.sm.IsOnline.Set(online, this, false);
			this.RefreshCharge();
		}

		// Token: 0x0600700F RID: 28687 RVA: 0x002F7044 File Offset: 0x002F5244
		public void SetManualDeliveryEnableState(bool enable)
		{
			if (!enable)
			{
				this.manualDelivery.capacity = 0f;
				this.manualDelivery.refillMass = 0f;
				this.manualDelivery.RequestedItemTag = null;
				this.manualDelivery.AbortDelivery("Manual delivery disabled");
				return;
			}
			base.smi.storage.capacityKg = 3f;
			base.smi.manualDelivery.capacity = 1f;
			base.smi.manualDelivery.refillMass = 1f;
			base.smi.manualDelivery.MinimumMass = 1f;
			this.manualDelivery.RequestedItemTag = GameTags.ChargedPortableBattery;
		}

		// Token: 0x06007010 RID: 28688 RVA: 0x000E9548 File Offset: 0x000E7748
		public GameObject GetFirstDischargedElectrobankInInventory()
		{
			return this.storage.FindFirst(GameTags.EmptyPortableBattery);
		}

		// Token: 0x06007011 RID: 28689 RVA: 0x000E955A File Offset: 0x000E775A
		public Electrobank GetClosestElectrobank()
		{
			return this.closestElectrobankSensor.GetItem();
		}

		// Token: 0x06007012 RID: 28690 RVA: 0x002F70FC File Offset: 0x002F52FC
		public void RefreshCharge()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			this.storage.Find(GameTags.EmptyPortableBattery, list2);
			float num = 0f;
			if (this.IsOnline)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Electrobank component = list[i].GetComponent<Electrobank>();
					num += component.Charge;
				}
			}
			this.BionicBattery.SetValue(num);
			base.sm.ChargedElectrobankCount.Set(list.Count, this, false);
			base.sm.DepletedElectrobankCount.Set(list2.Count, this, false);
			this.UpdateNotifications();
		}

		// Token: 0x06007013 RID: 28691 RVA: 0x002F71B8 File Offset: 0x002F53B8
		public void ConsumePower(float joules)
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			float num = joules;
			for (int i = 0; i < list.Count; i++)
			{
				Electrobank component = list[i].GetComponent<Electrobank>();
				float joules2 = Mathf.Min(component.Charge, num);
				float num2 = component.RemovePower(joules2, false);
				num -= num2;
				WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountConsumed(component.ID, num2);
			}
			this.RefreshCharge();
		}

		// Token: 0x06007014 RID: 28692 RVA: 0x002F7234 File Offset: 0x002F5434
		public void DebugAddCharge(float joules)
		{
			float num = MathF.Min(joules, 360000f - this.CurrentCharge);
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			int num2 = 0;
			while (num > 0f && num2 < list.Count)
			{
				Electrobank component = list[num2].GetComponent<Electrobank>();
				float num3 = Mathf.Min(120000f - component.Charge, num);
				component.AddPower(num3);
				num -= num3;
				num2++;
			}
			if (num > 0f && list.Count < 3)
			{
				int num4 = this.storage.items.Count - 1;
				while (num > 0f && num4 >= 0)
				{
					GameObject gameObject = this.storage.items[num4];
					if (!(gameObject == null))
					{
						Electrobank component2 = gameObject.GetComponent<Electrobank>();
						if (component2 == null && gameObject.HasTag(GameTags.EmptyPortableBattery))
						{
							this.storage.Drop(gameObject, true);
							GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), base.transform.position);
							gameObject2.SetActive(true);
							component2 = gameObject2.GetComponent<Electrobank>();
							float joules2 = Mathf.Clamp(component2.Charge - num, 0f, float.MaxValue);
							component2.RemovePower(joules2, true);
							num -= component2.Charge;
							this.storage.Store(gameObject2, false, false, true, false);
						}
					}
					num4--;
				}
			}
			if (num > 0f && this.storage.items.Count < 3)
			{
				do
				{
					GameObject gameObject3 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), base.transform.position);
					gameObject3.SetActive(true);
					Electrobank component3 = gameObject3.GetComponent<Electrobank>();
					float joules3 = Mathf.Clamp(component3.Charge - num, 0f, float.MaxValue);
					component3.RemovePower(joules3, true);
					num -= component3.Charge;
					this.storage.Store(gameObject3, false, false, true, false);
				}
				while (num > 0f && this.storage.items.Count < 3 && num > 0f);
			}
			this.RefreshCharge();
		}

		// Token: 0x06007015 RID: 28693 RVA: 0x002F7484 File Offset: 0x002F5684
		private void UpdateNotifications()
		{
			this.criticalBatteryStatusItemGuid = this.selectable.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicCriticalBattery, this.criticalBatteryStatusItemGuid, BionicBatteryMonitor.IsCriticallyLow(base.smi) && !this.prefabID.HasTag(GameTags.Incapacitated), base.gameObject);
		}

		// Token: 0x06007016 RID: 28694 RVA: 0x002F74E0 File Offset: 0x002F56E0
		public bool AddOrUpdateModifier(BionicBatteryMonitor.WattageModifier modifier, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifier.id);
			bool flag;
			if (num >= 0)
			{
				flag = (this.Modifiers[num].name != modifier.name || this.Modifiers[num].value != modifier.value || this.Modifiers[num].potentialValue != modifier.potentialValue);
				this.Modifiers[num] = modifier;
			}
			else
			{
				this.Modifiers.Add(modifier);
				flag = true;
			}
			if (flag)
			{
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
			}
			if (triggerCallbacks)
			{
				base.Trigger(1361471071, this.Wattage);
			}
			return flag;
		}

		// Token: 0x06007017 RID: 28695 RVA: 0x002F75EC File Offset: 0x002F57EC
		public bool RemoveModifier(string modifierID, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifierID);
			if (num >= 0)
			{
				this.Modifiers.RemoveAt(num);
				if (triggerCallbacks)
				{
					base.Trigger(1361471071, this.Wattage);
				}
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
				return true;
			}
			return false;
		}

		// Token: 0x06007018 RID: 28696 RVA: 0x00242D70 File Offset: 0x00240F70
		private void OnBatteriesDroppedFromDeath(List<GameObject> items)
		{
			if (items != null)
			{
				for (int i = 0; i < items.Count; i++)
				{
					Electrobank component = items[i].GetComponent<Electrobank>();
					if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
					{
						component.RemovePower(component.Charge, true);
					}
				}
			}
		}

		// Token: 0x06007019 RID: 28697 RVA: 0x002F7674 File Offset: 0x002F5874
		public void SpawnAndInstallInitialElectrobanks()
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), base.transform.position);
				gameObject.SetActive(true);
				this.storage.Store(gameObject, false, false, true, false);
			}
			this.RefreshCharge();
			this.SetOnlineState(true);
			base.sm.InitialElectrobanksSpawned.Set(true, this, false);
		}

		// Token: 0x0600701A RID: 28698 RVA: 0x002F76E8 File Offset: 0x002F58E8
		public void AutomaticallyDropAllDepletedElectrobanks()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.EmptyPortableBattery, list);
			for (int i = 0; i < list.Count; i++)
			{
				GameObject go = list[i];
				this.storage.Drop(go, true);
			}
		}

		// Token: 0x040053C1 RID: 21441
		public Storage storage;

		// Token: 0x040053C2 RID: 21442
		public KPrefabID prefabID;

		// Token: 0x040053C4 RID: 21444
		private Schedulable schedulable;

		// Token: 0x040053C5 RID: 21445
		private AmountInstance BionicBattery;

		// Token: 0x040053C6 RID: 21446
		private ManualDeliveryKG manualDelivery;

		// Token: 0x040053C7 RID: 21447
		private ClosestElectrobankSensor closestElectrobankSensor;

		// Token: 0x040053C8 RID: 21448
		private KSelectable selectable;

		// Token: 0x040053C9 RID: 21449
		private Guid criticalBatteryStatusItemGuid;

		// Token: 0x040053CB RID: 21451
		private Chore reanimateChore;
	}
}
