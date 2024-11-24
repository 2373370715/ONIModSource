using System;

// Token: 0x02000668 RID: 1640
public class BeOfflineChore : Chore<BeOfflineChore.StatesInstance>
{
	// Token: 0x06001DCF RID: 7631 RVA: 0x001B098C File Offset: 0x001AEB8C
	public static string GetPowerDownAnimPre(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down";
		}
		return "power_down";
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x001B09C0 File Offset: 0x001AEBC0
	public static string GetPowerDownAnimLoop(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down_idle";
		}
		return "power_down_idle";
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x001B09F4 File Offset: 0x001AEBF4
	public BeOfflineChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeOffline, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeOfflineChore.StatesInstance(this);
	}

	// Token: 0x040012AE RID: 4782
	public const string EFFECT_NAME = "BionicOffline";

	// Token: 0x02000669 RID: 1641
	public class StatesInstance : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore, object>.GameInstance
	{
		// Token: 0x06001DD2 RID: 7634 RVA: 0x000B3B3E File Offset: 0x000B1D3E
		public StatesInstance(BeOfflineChore master) : base(master)
		{
		}
	}

	// Token: 0x0200066A RID: 1642
	public class States : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore>
	{
		// Token: 0x06001DD3 RID: 7635 RVA: 0x001B0A38 File Offset: 0x001AEC38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAnims("anim_bionic_kanim", 0f).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicOfflineIncapacitated, (BeOfflineChore.StatesInstance smi) => smi.master.gameObject.GetSMI<BionicBatteryMonitor.Instance>()).ToggleEffect("BionicOffline").PlayAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimPre), KAnim.PlayMode.Once).QueueAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimLoop), true, null);
		}
	}
}
