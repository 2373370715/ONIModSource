using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
public class SuperProductive : GameStateMachine<SuperProductive, SuperProductive.Instance>
{
	// Token: 0x060023B2 RID: 9138 RVA: 0x001C6204 File Offset: 0x001C4404
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleStatusItem(Db.Get().DuplicantStatusItems.BeingProductive, null).Enter(delegate(SuperProductive.Instance smi)
		{
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, DUPLICANTS.TRAITS.SUPERPRODUCTIVE.NAME, smi.master.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			smi.fx = new SuperProductiveFX.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
		}).Exit(delegate(SuperProductive.Instance smi)
		{
			smi.fx.sm.destroyFX.Trigger(smi.fx);
		}).DefaultState(this.overjoyed.idle);
		this.overjoyed.idle.EventTransition(GameHashes.StartWork, this.overjoyed.working, null);
		this.overjoyed.working.ScheduleGoTo(0.33f, this.overjoyed.superProductive);
		this.overjoyed.superProductive.Enter(delegate(SuperProductive.Instance smi)
		{
			WorkerBase component = smi.GetComponent<WorkerBase>();
			if (component != null && component.GetState() == WorkerBase.State.Working)
			{
				Workable workable = component.GetWorkable();
				if (workable != null)
				{
					float num = workable.WorkTimeRemaining;
					if (workable.GetComponent<Diggable>() != null)
					{
						num = Diggable.GetApproximateDigTime(Grid.PosToCell(workable));
					}
					if (num > 1f && smi.ShouldSkipWork() && component.InstantlyFinish())
					{
						smi.ReactSuperProductive();
						smi.fx.sm.wasProductive.Trigger(smi.fx);
					}
				}
			}
			smi.GoTo(this.overjoyed.idle);
		});
	}

	// Token: 0x040017A7 RID: 6055
	public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x040017A8 RID: 6056
	public SuperProductive.OverjoyedStates overjoyed;

	// Token: 0x020007CA RID: 1994
	public class OverjoyedStates : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040017A9 RID: 6057
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040017AA RID: 6058
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x040017AB RID: 6059
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State superProductive;
	}

	// Token: 0x020007CB RID: 1995
	public new class Instance : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060023B6 RID: 9142 RVA: 0x000B7393 File Offset: 0x000B5593
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000B739C File Offset: 0x000B559C
		public bool ShouldSkipWork()
		{
			return UnityEngine.Random.Range(0f, 100f) <= TRAITS.JOY_REACTIONS.SUPER_PRODUCTIVE.INSTANT_SUCCESS_CHANCE;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x001C63D0 File Offset: 0x001C45D0
		public void ReactSuperProductive()
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi != null)
			{
				smi.AddSelfEmoteReactable(base.gameObject, "SuperProductive", Db.Get().Emotes.Minion.ProductiveCheer, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 1f, 1f, 0f, null);
			}
		}

		// Token: 0x040017AC RID: 6060
		public SuperProductiveFX.Instance fx;
	}
}
