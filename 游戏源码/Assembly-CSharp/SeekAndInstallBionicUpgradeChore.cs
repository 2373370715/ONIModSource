using System;
using UnityEngine;

// Token: 0x0200072A RID: 1834
public class SeekAndInstallBionicUpgradeChore : Chore<SeekAndInstallBionicUpgradeChore.Instance>
{
	// Token: 0x060020BC RID: 8380 RVA: 0x001BD010 File Offset: 0x001BB210
	public SeekAndInstallBionicUpgradeChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.SeekAndInstallUpgrade, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SeekAndInstallBionicUpgradeChore.Instance(this, target.gameObject);
		BionicUpgradeComponent assignedUpgradeComponent = target.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().GetAnyReachableAssignedSlot().assignedUpgradeComponent;
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, assignedUpgradeComponent.GetComponent<Pickupable>());
		this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, assignedUpgradeComponent);
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x001BD0A8 File Offset: 0x001BB2A8
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null context.consumer");
			return;
		}
		BionicUpgradesMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null BionicUpgradesMonitor.Instance");
			return;
		}
		BionicUpgradesMonitor.UpgradeComponentSlot anyReachableAssignedSlot = smi.GetAnyReachableAssignedSlot();
		BionicUpgradeComponent bionicUpgradeComponent = (anyReachableAssignedSlot == null) ? null : anyReachableAssignedSlot.assignedUpgradeComponent;
		if (bionicUpgradeComponent == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null upgradeComponent.gameObject");
			return;
		}
		base.smi.sm.initialUpgradeComponent.Set(bionicUpgradeComponent.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x001BD16C File Offset: 0x001BB36C
	public static void SetOverrideAnimSymbol(SeekAndInstallBionicUpgradeChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpgrade.Get(smi);
		if (gameObject != null)
		{
			gameObject.GetComponent<KBatchedAnimTracker>().enabled = !overriding;
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		string animStateName = BionicUpgradeComponentConfig.UpgradesData[gameObject.PrefabID()].animStateName;
		KAnim.Build.Symbol symbol = gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(animStateName);
		component2.AddSymbolOverride(text, symbol, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x001BD23C File Offset: 0x001BB43C
	public static bool IsBionicUpgradeAssignedTo(GameObject bionicUpgradeGameObject, GameObject ownerInQuestion)
	{
		Assignable component = bionicUpgradeGameObject.GetComponent<BionicUpgradeComponent>();
		IAssignableIdentity component2 = ownerInQuestion.GetComponent<IAssignableIdentity>();
		return component.IsAssignedTo(component2);
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x001BD25C File Offset: 0x001BB45C
	public static void InstallUpgrade(SeekAndInstallBionicUpgradeChore.Instance smi)
	{
		Storage storage = smi.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.DefaultStorage);
		GameObject gameObject = storage.FindFirst(GameTags.BionicUpgrade);
		if (gameObject != null)
		{
			BionicUpgradeComponent component = gameObject.GetComponent<BionicUpgradeComponent>();
			storage.Remove(component.gameObject, true);
			smi.upgradeMonitor.InstallUpgrade(component);
		}
	}

	// Token: 0x0200072B RID: 1835
	public class States : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore>
	{
		// Token: 0x060020C1 RID: 8385 RVA: 0x001BD2D0 File Offset: 0x001BB4D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.initialUpgradeComponent, this.pickedUpgrade, this.amountRequested, this.actualunits, this.install, null).Target(this.initialUpgradeComponent).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.initialUpgradeComponent.Get(smi), smi.gameObject));
			this.install.Target(this.dupe).ToggleAnims("anim_bionic_booster_installation_kanim", 0f).PlayAnim("installation", KAnim.PlayMode.Once).Enter(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, true);
			}).Exit(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, false);
			}).OnAnimQueueComplete(this.complete).ScheduleGoTo(10f, this.complete).Target(this.pickedUpgrade).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.pickedUpgrade.Get(smi), smi.gameObject));
			this.complete.Target(this.dupe).Enter(new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State.Callback(SeekAndInstallBionicUpgradeChore.InstallUpgrade)).ReturnSuccess();
		}

		// Token: 0x04001567 RID: 5479
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FetchSubState fetch;

		// Token: 0x04001568 RID: 5480
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State install;

		// Token: 0x04001569 RID: 5481
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State complete;

		// Token: 0x0400156A RID: 5482
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter dupe;

		// Token: 0x0400156B RID: 5483
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter initialUpgradeComponent;

		// Token: 0x0400156C RID: 5484
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter pickedUpgrade;

		// Token: 0x0400156D RID: 5485
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter actualunits;

		// Token: 0x0400156E RID: 5486
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter amountRequested = new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter(1f);
	}

	// Token: 0x0200072D RID: 1837
	public class Instance : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.GameInstance
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x000B578B File Offset: 0x000B398B
		public BionicUpgradesMonitor.Instance upgradeMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicUpgradesMonitor.Instance>();
			}
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000B57A3 File Offset: 0x000B39A3
		public Instance(SeekAndInstallBionicUpgradeChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
