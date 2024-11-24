using System;
using UnityEngine;

// Token: 0x02000707 RID: 1799
public class RecoverFromHeatChore : Chore<RecoverFromHeatChore.Instance>
{
	// Token: 0x06002037 RID: 8247 RVA: 0x001BB238 File Offset: 0x001B9438
	public RecoverFromHeatChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverFromHeat, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromHeatChore.Instance(this, target.gameObject);
		HeatImmunityMonitor.Instance chillyBones = target.gameObject.GetSMI<HeatImmunityMonitor.Instance>();
		Func<int> data = () => chillyBones.ShelterCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x02000708 RID: 1800
	public class States : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore>
	{
		// Token: 0x06002038 RID: 8248 RVA: 0x001BB2C4 File Offset: 0x001B94C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateHeatImmunityProvider", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.heatImmunityProvider, null).Enter("AnimOverride", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.cachedAnimName = RecoverFromHeatChore.States.GetAnimFileName(smi);
				smi.GetComponent<KAnimControllerBase>().AddAnimOverrides(Assets.GetAnim(smi.cachedAnimName), 0f);
			}).Exit(delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(Assets.GetAnim(smi.cachedAnimName));
			}).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringFromHeat);
			this.recover.pre.Face(this.heatImmunityProvider, 0f).PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Not(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback(RecoverFromHeatChore.States.ApplyHeatImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x001BB584 File Offset: 0x001B9784
		public static bool IsImmunityProviderStillValid(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x001BB5A4 File Offset: 0x001B97A4
		public static void ApplyHeatImmunityEffect(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x001BB5C8 File Offset: 0x001B97C8
		public static HashedString GetAnimFileName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000B51F2 File Offset: 0x000B33F2
		public static string GetPreAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000B5219 File Offset: 0x000B3419
		public static string GetLoopAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000B5240 File Offset: 0x000B3440
		public static string GetPstAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x001BB600 File Offset: 0x001B9800
		public static string GetAnimFromHeatImmunityProvider(RecoverFromHeatChore.Instance smi, Func<HeatImmunityProvider.Instance, string> getCallback)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x040014FE RID: 5374
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040014FF RID: 5375
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.PreLoopPostState recover;

		// Token: 0x04001500 RID: 5376
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State remove_suit;

		// Token: 0x04001501 RID: 5377
		public RecoverFromHeatChore.States.CompleteStates complete;

		// Token: 0x04001502 RID: 5378
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter heatImmunityProvider;

		// Token: 0x04001503 RID: 5379
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter entityRecovering;

		// Token: 0x04001504 RID: 5380
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter locator;

		// Token: 0x02000709 RID: 1801
		public class CompleteStates : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State
		{
			// Token: 0x04001505 RID: 5381
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State evaluate;

			// Token: 0x04001506 RID: 5382
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State fail;

			// Token: 0x04001507 RID: 5383
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State success;
		}
	}

	// Token: 0x0200070C RID: 1804
	public class Instance : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.GameInstance
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06002050 RID: 8272 RVA: 0x000B531F File Offset: 0x000B351F
		public HeatImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.heatImmunityProvider.Get(this) == null))
				{
					return base.sm.heatImmunityProvider.Get(this).GetSMI<HeatImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06002051 RID: 8273 RVA: 0x000B5352 File Offset: 0x000B3552
		public HeatImmunityMonitor.Instance heatImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<HeatImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x001BB620 File Offset: 0x001B9820
		public Instance(RecoverFromHeatChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			HeatImmunityMonitor.Instance heatImmunityMonitor = this.heatImmunityMonitor;
			if (heatImmunityMonitor.NearestImmunityProvider != null && !heatImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.heatImmunityProvider.Set(heatImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x001BB684 File Offset: 0x001B9884
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x001BB6BC File Offset: 0x001B98BC
		public void UpdateImmunityProvider()
		{
			HeatImmunityProvider.Instance nearestImmunityProvider = this.heatImmunityMonitor.NearestImmunityProvider;
			base.sm.heatImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x001BB6FC File Offset: 0x001B98FC
		public void UpdateLocator()
		{
			int num = this.heatImmunityMonitor.ShelterCell;
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.entityRecovering.Get<Transform>(base.smi).GetPosition());
				this.DestroyLocator();
			}
			else
			{
				Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
				base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
			}
			this.targetCell = num;
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x000B536A File Offset: 0x000B356A
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x04001514 RID: 5396
		private int targetCell;

		// Token: 0x04001515 RID: 5397
		public HashedString cachedAnimName;
	}
}
