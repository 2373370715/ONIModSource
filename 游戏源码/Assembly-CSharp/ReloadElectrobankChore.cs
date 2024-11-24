using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200070E RID: 1806
public class ReloadElectrobankChore : Chore<ReloadElectrobankChore.Instance>
{
	// Token: 0x06002059 RID: 8281 RVA: 0x001BB774 File Offset: 0x001B9974
	public ReloadElectrobankChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ReloadElectrobank, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new ReloadElectrobankChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ReloadElectrobankChore.ElectrobankIsNotNull, null);
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x001BB7D8 File Offset: 0x001B99D8
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null context.consumer");
			return;
		}
		BionicBatteryMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null RationMonitor.Instance");
			return;
		}
		Electrobank closestElectrobank = smi.GetClosestElectrobank();
		if (closestElectrobank == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null electrobank.gameObject");
			return;
		}
		base.smi.cachedElectrobankSourcePrefabRef = Assets.GetPrefab(closestElectrobank.PrefabID());
		base.smi.sm.electrobankSource.Set(closestElectrobank.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x000B53A1 File Offset: 0x000B35A1
	private static string GetConsumePreAnimName(ReloadElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_pre";
		}
		return "ladder_consume";
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000B53BE File Offset: 0x000B35BE
	private static string GetConsumeLoopAnimName(ReloadElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_loop";
		}
		return "ladder_consume";
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000B53DB File Offset: 0x000B35DB
	private static string GetConsumePstAnimName(ReloadElectrobankChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_pst";
		}
		return "ladder_consume";
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x001BB8A4 File Offset: 0x001B9AA4
	public static void InstallElectrobank(ReloadElectrobankChore.Instance smi)
	{
		Storage[] components = smi.gameObject.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] != smi.batteryMonitor.storage && components[i].FindFirst(GameTags.ChargedPortableBattery) != null)
			{
				components[i].Transfer(smi.batteryMonitor.storage, false, false);
				break;
			}
		}
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_BionicBattery, true);
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x001BB91C File Offset: 0x001B9B1C
	public static void SetOverrideAnimSymbol(ReloadElectrobankChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpElectrobank.Get(smi);
		if (gameObject != null)
		{
			KBatchedAnimTracker component3 = gameObject.GetComponent<KBatchedAnimTracker>();
			if (component3 != null)
			{
				component3.enabled = !overriding;
			}
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		KAnim.Build.Symbol symbolByIndex = ((gameObject != null) ? gameObject.GetComponent<KBatchedAnimController>() : smi.cachedElectrobankSourcePrefabRef.GetComponent<KBatchedAnimController>()).AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x04001517 RID: 5399
	public const float LOOP_LENGTH = 4.333f;

	// Token: 0x04001518 RID: 5400
	public static readonly Chore.Precondition ElectrobankIsNotNull = new Chore.Precondition
	{
		id = "ElectrobankIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>().GetClosestElectrobank();
		}
	};

	// Token: 0x0200070F RID: 1807
	public class States : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore>
	{
		// Token: 0x06002061 RID: 8289 RVA: 0x001BBA44 File Offset: 0x001B9C44
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.electrobankSource, this.pickedUpElectrobank, this.amountRequested, this.actualunits, this.install, null).OnTargetLost(this.electrobankSource, this.electrobankLost);
			this.install.DefaultState(this.install.pre).ToggleAnims("anim_bionic_kanim", 0f).Enter("Add Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, false);
			});
			this.install.pre.PlayAnim(new Func<ReloadElectrobankChore.Instance, string>(ReloadElectrobankChore.GetConsumePreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.install.loop).ScheduleGoTo(3f, this.install.loop);
			this.install.loop.PlayAnim(new Func<ReloadElectrobankChore.Instance, string>(ReloadElectrobankChore.GetConsumeLoopAnimName), KAnim.PlayMode.Loop).ScheduleGoTo(4.333f, this.install.pst);
			this.install.pst.PlayAnim(new Func<ReloadElectrobankChore.Instance, string>(ReloadElectrobankChore.GetConsumePstAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.InstallElectrobank)).ReturnSuccess();
			this.electrobankLost.Target(this.dupe).TriggerOnEnter(GameHashes.TargetElectrobankLost, null).ReturnFailure();
		}

		// Token: 0x04001519 RID: 5401
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FetchSubState fetch;

		// Token: 0x0400151A RID: 5402
		public ReloadElectrobankChore.States.InstallState install;

		// Token: 0x0400151B RID: 5403
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State complete;

		// Token: 0x0400151C RID: 5404
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State electrobankLost;

		// Token: 0x0400151D RID: 5405
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter dupe;

		// Token: 0x0400151E RID: 5406
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter electrobankSource;

		// Token: 0x0400151F RID: 5407
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter pickedUpElectrobank;

		// Token: 0x04001520 RID: 5408
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter messstation;

		// Token: 0x04001521 RID: 5409
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter actualunits;

		// Token: 0x04001522 RID: 5410
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter amountRequested = new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter(1f);

		// Token: 0x02000710 RID: 1808
		public class InstallState : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x04001523 RID: 5411
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pre;

			// Token: 0x04001524 RID: 5412
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State loop;

			// Token: 0x04001525 RID: 5413
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pst;
		}
	}

	// Token: 0x02000712 RID: 1810
	public class Instance : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.GameInstance
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000B5436 File Offset: 0x000B3636
		public BionicBatteryMonitor.Instance batteryMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicBatteryMonitor.Instance>();
			}
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000B544E File Offset: 0x000B364E
		public Instance(ReloadElectrobankChore master, GameObject duplicant) : base(master)
		{
		}

		// Token: 0x04001529 RID: 5417
		public GameObject cachedElectrobankSourcePrefabRef;
	}
}
