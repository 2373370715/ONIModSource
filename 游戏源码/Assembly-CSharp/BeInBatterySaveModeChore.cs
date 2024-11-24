using System;
using UnityEngine;

// Token: 0x0200065E RID: 1630
public class BeInBatterySaveModeChore : Chore<BeInBatterySaveModeChore.StatesInstance>
{
	// Token: 0x06001DAC RID: 7596 RVA: 0x001B0304 File Offset: 0x001AE504
	public BeInBatterySaveModeChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeBatterySaveMode, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeInBatterySaveModeChore.StatesInstance(this, master.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x000B398A File Offset: 0x000B1B8A
	public static bool IsBatteryMonitorWaitingForUsToExit(BeInBatterySaveModeChore.StatesInstance smi, float dt)
	{
		return smi.batteryMonitor.IsInsideState(smi.batteryMonitor.sm.online.batterySaveMode.idle.exit);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x001B035C File Offset: 0x001AE55C
	public static string GetEnterAnim(BeInBatterySaveModeChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Ladder)
		{
		}
		return "low_power_pre";
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x001B0388 File Offset: 0x001AE588
	public static string GetIdleAnim(BeInBatterySaveModeChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Ladder)
		{
		}
		return "low_power_loop";
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x001B03B4 File Offset: 0x001AE5B4
	public static string GetExitAnim(BeInBatterySaveModeChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Ladder)
		{
		}
		return "low_power_pst";
	}

	// Token: 0x0400128F RID: 4751
	public const string EFFECT_NAME = "BionicBatterySaveMode";

	// Token: 0x0200065F RID: 1631
	public class States : GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore>
	{
		// Token: 0x06001DB1 RID: 7601 RVA: 0x001B03E0 File Offset: 0x001AE5E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			this.root.ToggleTag(GameTags.BatterySaveMode).TriggerOnEnter(GameHashes.BionicBatterySaveModeChanged, (BeInBatterySaveModeChore.StatesInstance smi) => true).TriggerOnExit(GameHashes.BionicBatterySaveModeChanged, (BeInBatterySaveModeChore.StatesInstance smi) => false).ToggleEffect("BionicBatterySaveMode");
			this.enter.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<BeInBatterySaveModeChore.StatesInstance, string>(BeInBatterySaveModeChore.GetEnterAnim), KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.idle.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<BeInBatterySaveModeChore.StatesInstance, string>(BeInBatterySaveModeChore.GetIdleAnim), KAnim.PlayMode.Loop).UpdateTransition(this.exit, new Func<BeInBatterySaveModeChore.StatesInstance, float, bool>(BeInBatterySaveModeChore.IsBatteryMonitorWaitingForUsToExit), UpdateRate.SIM_1000ms, false);
			this.exit.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<BeInBatterySaveModeChore.StatesInstance, string>(BeInBatterySaveModeChore.GetExitAnim), KAnim.PlayMode.Once).OnAnimQueueComplete(this.end);
			this.end.ReturnSuccess();
		}

		// Token: 0x04001290 RID: 4752
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State enter;

		// Token: 0x04001291 RID: 4753
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State idle;

		// Token: 0x04001292 RID: 4754
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State exit;

		// Token: 0x04001293 RID: 4755
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State end;
	}

	// Token: 0x02000661 RID: 1633
	public class StatesInstance : GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.GameInstance
	{
		// Token: 0x06001DB7 RID: 7607 RVA: 0x000B39DA File Offset: 0x000B1BDA
		public StatesInstance(BeInBatterySaveModeChore master, GameObject duplicant) : base(master)
		{
			this.batteryMonitor = duplicant.GetSMI<BionicBatteryMonitor.Instance>();
		}

		// Token: 0x04001297 RID: 4759
		public BionicBatteryMonitor.Instance batteryMonitor;
	}
}
