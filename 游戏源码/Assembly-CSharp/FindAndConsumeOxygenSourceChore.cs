using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006AF RID: 1711
public class FindAndConsumeOxygenSourceChore : Chore<FindAndConsumeOxygenSourceChore.Instance>
{
	// Token: 0x06001F0A RID: 7946 RVA: 0x001B6C98 File Offset: 0x001B4E98
	public FindAndConsumeOxygenSourceChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.FindOxygenSourceItem, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new FindAndConsumeOxygenSourceChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(FindAndConsumeOxygenSourceChore.OxygenSourceItemIsNotNull, null);
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x001B6CFC File Offset: 0x001B4EFC
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null context.consumer");
			return;
		}
		BionicOxygenTankMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null BionicOxygenTankMonitor.Instance");
			return;
		}
		Pickupable closestOxygenSource = smi.GetClosestOxygenSource();
		if (closestOxygenSource == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null oxygenSourceItem.gameObject");
			return;
		}
		base.smi.sm.oxygenSourceItem.Set(closestOxygenSource.gameObject, base.smi, false);
		base.smi.sm.amountRequested.Set(Mathf.Min(smi.SpaceAvailableInTank, closestOxygenSource.UnreservedAmount), base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x000B4715 File Offset: 0x000B2915
	private static string GetConsumePreAnimName(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_canister_pre";
		}
		return "ladder_consume";
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x000B4732 File Offset: 0x000B2932
	private static string GetConsumeLoopAnimName(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_canister_loop";
		}
		return "ladder_consume";
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x000B474F File Offset: 0x000B294F
	private static string GetConsumePstAnimName(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "consume_canister_pst";
		}
		return "ladder_consume";
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x001B6DE0 File Offset: 0x001B4FE0
	public static void ExtractOxygenFromItem(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		GameObject gameObject = smi.sm.pickedUpItem.Get(smi);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Element.IsGas)
		{
			Storage[] components = smi.gameObject.GetComponents<Storage>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] != smi.oxygenTankMonitor.storage)
				{
					List<GameObject> list = new List<GameObject>();
					components[i].Find(GameTags.Breathable, list);
					foreach (GameObject gameObject2 in list)
					{
						if (gameObject2 != null)
						{
							components[i].Transfer(gameObject2, smi.oxygenTankMonitor.storage, false, false);
							break;
						}
					}
				}
			}
			return;
		}
		SimHashes element = SimHashes.Oxygen;
		if (ElementLoader.GetElement(component.Element.sublimateId.CreateTag()).HasTag(GameTags.Breathable))
		{
			element = component.Element.sublimateId;
		}
		smi.oxygenTankMonitor.storage.AddGasChunk(element, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
		Util.KDestroyGameObject(gameObject);
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x001B6F2C File Offset: 0x001B512C
	public static void SetOverrideAnimSymbol(FindAndConsumeOxygenSourceChore.Instance smi, bool overriding)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpItem.Get(smi);
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
		KAnim.Build.Symbol symbolByIndex = gameObject.GetComponent<KBatchedAnimController>().CurrentAnim.animFile.build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000B476C File Offset: 0x000B296C
	public static void TriggerOxygenItemLostSignal(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.oxygenTankMonitor != null)
		{
			smi.oxygenTankMonitor.sm.OxygenSourceItemLostSignal.Trigger(smi.oxygenTankMonitor);
		}
	}

	// Token: 0x040013F7 RID: 5111
	public const float LOOP_LENGTH = 4.333f;

	// Token: 0x040013F8 RID: 5112
	public static readonly Chore.Precondition OxygenSourceItemIsNotNull = new Chore.Precondition
	{
		id = "OxygenSourceIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Pickupable closestOxygenSource = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>().GetClosestOxygenSource();
			return closestOxygenSource != null && closestOxygenSource.UnreservedAmount > 0f;
		}
	};

	// Token: 0x020006B0 RID: 1712
	public class States : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore>
	{
		// Token: 0x06001F13 RID: 7955 RVA: 0x001B703C File Offset: 0x001B523C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.oxygenSourceItem, this.pickedUpItem, this.amountRequested, this.actualunits, this.install, null).OnTargetLost(this.oxygenSourceItem, this.oxygenSourceLost);
			this.install.Target(this.pickedUpItem).OnTargetLost(this.pickedUpItem, this.oxygenSourceLost).Target(this.dupe).DefaultState(this.install.pre).ToggleAnims("anim_bionic_kanim", 0f).Enter("Add Symbol Override", delegate(FindAndConsumeOxygenSourceChore.Instance smi)
			{
				FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(FindAndConsumeOxygenSourceChore.Instance smi)
			{
				FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, false);
			});
			this.install.pre.PlayAnim(new Func<FindAndConsumeOxygenSourceChore.Instance, string>(FindAndConsumeOxygenSourceChore.GetConsumePreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.install.loop).ScheduleGoTo(3f, this.install.loop);
			this.install.loop.PlayAnim(new Func<FindAndConsumeOxygenSourceChore.Instance, string>(FindAndConsumeOxygenSourceChore.GetConsumeLoopAnimName), KAnim.PlayMode.Loop).ScheduleGoTo(4.333f, this.install.pst);
			this.install.pst.PlayAnim(new Func<FindAndConsumeOxygenSourceChore.Instance, string>(FindAndConsumeOxygenSourceChore.GetConsumePstAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.ExtractOxygenFromItem)).ReturnSuccess();
			this.oxygenSourceLost.Target(this.dupe).Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.TriggerOxygenItemLostSignal)).ReturnFailure();
		}

		// Token: 0x040013F9 RID: 5113
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FetchSubState fetch;

		// Token: 0x040013FA RID: 5114
		public FindAndConsumeOxygenSourceChore.States.InstallState install;

		// Token: 0x040013FB RID: 5115
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State complete;

		// Token: 0x040013FC RID: 5116
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State oxygenSourceLost;

		// Token: 0x040013FD RID: 5117
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter dupe;

		// Token: 0x040013FE RID: 5118
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter oxygenSourceItem;

		// Token: 0x040013FF RID: 5119
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter pickedUpItem;

		// Token: 0x04001400 RID: 5120
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter actualunits;

		// Token: 0x04001401 RID: 5121
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter amountRequested;

		// Token: 0x020006B1 RID: 1713
		public class InstallState : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State
		{
			// Token: 0x04001402 RID: 5122
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pre;

			// Token: 0x04001403 RID: 5123
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State loop;

			// Token: 0x04001404 RID: 5124
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pst;
		}
	}

	// Token: 0x020006B3 RID: 1715
	public class Instance : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.GameInstance
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x000B47BF File Offset: 0x000B29BF
		public BionicOxygenTankMonitor.Instance oxygenTankMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicOxygenTankMonitor.Instance>();
			}
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000B47D7 File Offset: 0x000B29D7
		public Instance(FindAndConsumeOxygenSourceChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
