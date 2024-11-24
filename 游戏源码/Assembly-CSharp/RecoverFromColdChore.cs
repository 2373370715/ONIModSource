using System;
using UnityEngine;

// Token: 0x02000700 RID: 1792
public class RecoverFromColdChore : Chore<RecoverFromColdChore.Instance>
{
	// Token: 0x06002017 RID: 8215 RVA: 0x001BAD38 File Offset: 0x001B8F38
	public RecoverFromColdChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverWarmth, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromColdChore.Instance(this, target.gameObject);
		ColdImmunityMonitor.Instance coldImmunityMonitor = target.gameObject.GetSMI<ColdImmunityMonitor.Instance>();
		Func<int> data = () => coldImmunityMonitor.WarmUpCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x02000701 RID: 1793
	public class States : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore>
	{
		// Token: 0x06002018 RID: 8216 RVA: 0x001BADC4 File Offset: 0x001B8FC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateColdImmunityProvider", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.coldImmunityProvider, null).ToggleAnims(new Func<RecoverFromColdChore.Instance, HashedString>(RecoverFromColdChore.States.GetAnimFileName)).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringWarmnth);
			this.recover.pre.Face(this.coldImmunityProvider, 0f).PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Not(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback(RecoverFromColdChore.States.ApplyColdImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x001BB048 File Offset: 0x001B9248
		public static bool IsImmunityProviderStillValid(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x001BB068 File Offset: 0x001B9268
		public static void ApplyColdImmunityEffect(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x001BB08C File Offset: 0x001B928C
		public static HashedString GetAnimFileName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x000B5084 File Offset: 0x000B3284
		public static string GetPreAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x000B50AB File Offset: 0x000B32AB
		public static string GetLoopAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x000B50D2 File Offset: 0x000B32D2
		public static string GetPstAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x001BB0C4 File Offset: 0x001B92C4
		public static string GetAnimFromColdImmunityProvider(RecoverFromColdChore.Instance smi, Func<ColdImmunityProvider.Instance, string> getCallback)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x040014E8 RID: 5352
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040014E9 RID: 5353
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.PreLoopPostState recover;

		// Token: 0x040014EA RID: 5354
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State remove_suit;

		// Token: 0x040014EB RID: 5355
		public RecoverFromColdChore.States.CompleteStates complete;

		// Token: 0x040014EC RID: 5356
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter coldImmunityProvider;

		// Token: 0x040014ED RID: 5357
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter entityRecovering;

		// Token: 0x040014EE RID: 5358
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter locator;

		// Token: 0x02000702 RID: 1794
		public class CompleteStates : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State
		{
			// Token: 0x040014EF RID: 5359
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State evaluate;

			// Token: 0x040014F0 RID: 5360
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State fail;

			// Token: 0x040014F1 RID: 5361
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State success;
		}
	}

	// Token: 0x02000705 RID: 1797
	public class Instance : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.GameInstance
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x000B5170 File Offset: 0x000B3370
		public ColdImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.coldImmunityProvider.Get(this) == null))
				{
					return base.sm.coldImmunityProvider.Get(this).GetSMI<ColdImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x000B51A3 File Offset: 0x000B33A3
		public ColdImmunityMonitor.Instance coldImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<ColdImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x001BB0E4 File Offset: 0x001B92E4
		public Instance(RecoverFromColdChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			ColdImmunityMonitor.Instance coldImmunityMonitor = this.coldImmunityMonitor;
			if (coldImmunityMonitor.NearestImmunityProvider != null && !coldImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.coldImmunityProvider.Set(coldImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x001BB148 File Offset: 0x001B9348
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x001BB180 File Offset: 0x001B9380
		public void UpdateImmunityProvider()
		{
			ColdImmunityProvider.Instance nearestImmunityProvider = this.coldImmunityMonitor.NearestImmunityProvider;
			base.sm.coldImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x001BB1C0 File Offset: 0x001B93C0
		public void UpdateLocator()
		{
			int num = this.coldImmunityMonitor.WarmUpCell;
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

		// Token: 0x06002034 RID: 8244 RVA: 0x000B51BB File Offset: 0x000B33BB
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x040014FC RID: 5372
		private int targetCell;
	}
}
