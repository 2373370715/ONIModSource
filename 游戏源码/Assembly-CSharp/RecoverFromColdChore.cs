using System;
using UnityEngine;

public class RecoverFromColdChore : Chore<RecoverFromColdChore.Instance>
{
	public class States : GameStateMachine<States, Instance, RecoverFromColdChore>
	{
		public class CompleteStates : State
		{
			public State evaluate;

			public State fail;

			public State success;
		}

		public ApproachSubState<IApproachable> approach;

		public PreLoopPostState recover;

		public State remove_suit;

		public CompleteStates complete;

		public TargetParameter coldImmunityProvider;

		public TargetParameter entityRecovering;

		public TargetParameter locator;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = approach;
			Target(entityRecovering);
			root.Enter("CreateLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.DestroyLocator();
			})
				.Update("UpdateLocator", delegate(RecoverFromColdChore.Instance smi, float dt)
				{
					smi.UpdateLocator();
				}, UpdateRate.SIM_200ms, load_balance: true)
				.Update("UpdateColdImmunityProvider", delegate(RecoverFromColdChore.Instance smi, float dt)
				{
					smi.UpdateImmunityProvider();
				}, UpdateRate.SIM_200ms, load_balance: true);
			approach.InitializeStates(entityRecovering, locator, recover);
			recover.OnTargetLost(coldImmunityProvider, null).ToggleAnims((Func<RecoverFromColdChore.Instance, HashedString>)GetAnimFileName).DefaultState(recover.pre)
				.ToggleTag(GameTags.RecoveringWarmnth);
			recover.pre.Face(coldImmunityProvider).PlayAnim(GetPreAnimName).OnAnimQueueComplete(recover.loop);
			recover.loop.PlayAnim(GetLoopAnimName).OnAnimQueueComplete(recover.pst);
			recover.pst.QueueAnim(GetPstAnimName).OnAnimQueueComplete(complete);
			complete.DefaultState(complete.evaluate);
			complete.evaluate.EnterTransition(complete.success, IsImmunityProviderStillValid).EnterTransition(complete.fail, GameStateMachine<States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Not(IsImmunityProviderStillValid));
			complete.success.Enter(ApplyColdImmunityEffect).ReturnSuccess();
			complete.fail.ReturnFailure();
		}

		public static bool IsImmunityProviderStillValid(RecoverFromColdChore.Instance smi)
		{
			return smi.lastKnownImmunityProvider?.CanBeUsed ?? false;
		}

		public static void ApplyColdImmunityEffect(RecoverFromColdChore.Instance smi)
		{
			smi.lastKnownImmunityProvider?.ApplyImmunityEffect(smi.gameObject);
		}

		public static HashedString GetAnimFileName(RecoverFromColdChore.Instance smi)
		{
			return GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.AnimFileName);
		}

		public static string GetPreAnimName(RecoverFromColdChore.Instance smi)
		{
			return GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PreAnimName);
		}

		public static string GetLoopAnimName(RecoverFromColdChore.Instance smi)
		{
			return GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.LoopAnimName);
		}

		public static string GetPstAnimName(RecoverFromColdChore.Instance smi)
		{
			return GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PstAnimName);
		}

		public static string GetAnimFromColdImmunityProvider(RecoverFromColdChore.Instance smi, Func<ColdImmunityProvider.Instance, string> getCallback)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}
	}

	public class Instance : GameStateMachine<States, Instance, RecoverFromColdChore, object>.GameInstance
	{
		private int targetCell;

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

		public ColdImmunityMonitor.Instance coldImmunityMonitor => base.sm.entityRecovering.Get(this).GetSMI<ColdImmunityMonitor.Instance>();

		public Instance(RecoverFromColdChore master, GameObject entityRecovering)
			: base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this);
			ColdImmunityMonitor.Instance instance = coldImmunityMonitor;
			if (instance.NearestImmunityProvider != null && !instance.NearestImmunityProvider.isMasterNull)
			{
				base.sm.coldImmunityProvider.Set(instance.NearestImmunityProvider.gameObject, this);
			}
		}

		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this);
			UpdateLocator();
		}

		public void UpdateImmunityProvider()
		{
			ColdImmunityProvider.Instance nearestImmunityProvider = coldImmunityMonitor.NearestImmunityProvider;
			base.sm.coldImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this);
		}

		public void UpdateLocator()
		{
			int num = coldImmunityMonitor.WarmUpCell;
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.entityRecovering.Get<Transform>(base.smi).GetPosition());
				DestroyLocator();
			}
			else
			{
				Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
				base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
			}
			targetCell = num;
		}

		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}
	}

	public RecoverFromColdChore(IStateMachineTarget target)
		: base(Db.Get().ChoreTypes.RecoverWarmth, target, target.GetComponent<ChoreProvider>(), run_until_complete: false, (Action<Chore>)null, (Action<Chore>)null, (Action<Chore>)null, PriorityScreen.PriorityClass.personalNeeds, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new Instance(this, target.gameObject);
		ColdImmunityMonitor.Instance coldImmunityMonitor = target.gameObject.GetSMI<ColdImmunityMonitor.Instance>();
		Func<int> data = () => coldImmunityMonitor.WarmUpCell;
		AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
	}
}
