using System;
using UnityEngine;

// Token: 0x02000714 RID: 1812
public class RemoveDischargedElectrobankChore : Chore<RemoveDischargedElectrobankChore.Instance>
{
	// Token: 0x0600206D RID: 8301 RVA: 0x001BBC10 File Offset: 0x001B9E10
	public RemoveDischargedElectrobankChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.UnloadElectrobank, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RemoveDischargedElectrobankChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x001BBC68 File Offset: 0x001B9E68
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("RemoveDischargedElectrobankChore null context.consumer");
			return;
		}
		BionicBatteryMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("RemoveDischargedElectrobankChore null RationMonitor.Instance");
			return;
		}
		GameObject firstDischargedElectrobankInInventory = smi.GetFirstDischargedElectrobankInInventory();
		if (firstDischargedElectrobankInInventory == null)
		{
			global::Debug.LogError("RemoveDischargedElectrobankChore dischargedElectrobank is null");
			return;
		}
		base.smi.sm.dischargedElectrobank.Set(firstDischargedElectrobankInInventory, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x000B5480 File Offset: 0x000B3680
	private static string GetAnimName(RemoveDischargedElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "discharge";
		}
		return "ladder_discharge";
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x001BBD18 File Offset: 0x001B9F18
	public static void SetOverrideAnimSymbol(RemoveDischargedElectrobankChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.dischargedElectrobank.Get(smi);
		if (gameObject != null)
		{
			KBatchedAnimTracker component3 = gameObject.GetComponent<KBatchedAnimTracker>();
			if (component3 != null)
			{
				component3.enabled = !overriding;
				Storage.MakeItemInvisible(gameObject, overriding, false);
			}
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		KAnim.Build.Symbol symbolByIndex = gameObject.GetComponent<KBatchedAnimController>().CurrentAnim.animFile.build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x001BBDD8 File Offset: 0x001B9FD8
	public static void RemoveDepletedElectrobank(RemoveDischargedElectrobankChore.Instance smi)
	{
		GameObject go = smi.sm.dischargedElectrobank.Get(smi);
		smi.batteryMonitor.storage.Drop(go, true);
	}

	// Token: 0x02000715 RID: 1813
	public class States : GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore>
	{
		// Token: 0x06002072 RID: 8306 RVA: 0x001BBE0C File Offset: 0x001BA00C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.working;
			base.Target(this.dupe);
			this.working.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<RemoveDischargedElectrobankChore.Instance, string>(RemoveDischargedElectrobankChore.GetAnimName), KAnim.PlayMode.Once).Enter("Add Symbol Override", delegate(RemoveDischargedElectrobankChore.Instance smi)
			{
				RemoveDischargedElectrobankChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(RemoveDischargedElectrobankChore.Instance smi)
			{
				RemoveDischargedElectrobankChore.SetOverrideAnimSymbol(smi, false);
			}).OnAnimQueueComplete(this.complete);
			this.complete.Enter(new StateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.State.Callback(RemoveDischargedElectrobankChore.RemoveDepletedElectrobank)).ReturnSuccess();
		}

		// Token: 0x0400152B RID: 5419
		public GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.State working;

		// Token: 0x0400152C RID: 5420
		public GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.State complete;

		// Token: 0x0400152D RID: 5421
		public StateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.TargetParameter dupe;

		// Token: 0x0400152E RID: 5422
		public StateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.TargetParameter dischargedElectrobank;
	}

	// Token: 0x02000717 RID: 1815
	public class Instance : GameStateMachine<RemoveDischargedElectrobankChore.States, RemoveDischargedElectrobankChore.Instance, RemoveDischargedElectrobankChore, object>.GameInstance
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x000B54C3 File Offset: 0x000B36C3
		public BionicBatteryMonitor.Instance batteryMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicBatteryMonitor.Instance>();
			}
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000B54DB File Offset: 0x000B36DB
		public Instance(RemoveDischargedElectrobankChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
