using System;
using UnityEngine;

// Token: 0x02001882 RID: 6274
[SkipSaveFileSerialization]
public class Snorer : StateMachineComponent<Snorer.StatesInstance>
{
	// Token: 0x060081E6 RID: 33254 RVA: 0x000F5752 File Offset: 0x000F3952
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04006292 RID: 25234
	private static readonly HashedString HeadHash = "snapTo_mouth";

	// Token: 0x02001883 RID: 6275
	public class StatesInstance : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.GameInstance
	{
		// Token: 0x060081E9 RID: 33257 RVA: 0x000F5778 File Offset: 0x000F3978
		public StatesInstance(Snorer master) : base(master)
		{
		}

		// Token: 0x060081EA RID: 33258 RVA: 0x0033AAD8 File Offset: 0x00338CD8
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x060081EB RID: 33259 RVA: 0x000F5781 File Offset: 0x000F3981
		public void StartSmallSnore()
		{
			this.snoreHandle = GameScheduler.Instance.Schedule("snorelines", 2f, new Action<object>(this.StartSmallSnoreInternal), null, null);
		}

		// Token: 0x060081EC RID: 33260 RVA: 0x0033AAFC File Offset: 0x00338CFC
		private void StartSmallSnoreInternal(object data)
		{
			this.snoreHandle.ClearScheduler();
			bool flag;
			Matrix4x4 symbolTransform = base.smi.master.GetComponent<KBatchedAnimController>().GetSymbolTransform(Snorer.HeadHash, out flag);
			if (flag)
			{
				Vector3 position = symbolTransform.GetColumn(3);
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				this.snoreEffect = FXHelpers.CreateEffect("snore_fx_kanim", position, null, false, Grid.SceneLayer.Front, false);
				this.snoreEffect.destroyOnAnimComplete = true;
				this.snoreEffect.Play("snore", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x060081ED RID: 33261 RVA: 0x000F57AB File Offset: 0x000F39AB
		public void StopSmallSnore()
		{
			this.snoreHandle.ClearScheduler();
			if (this.snoreEffect != null)
			{
				this.snoreEffect.PlayMode = KAnim.PlayMode.Once;
			}
			this.snoreEffect = null;
		}

		// Token: 0x060081EE RID: 33262 RVA: 0x000F57D9 File Offset: 0x000F39D9
		public void StartSnoreBGEffect()
		{
			AcousticDisturbance.Emit(base.smi.master.gameObject, 3);
		}

		// Token: 0x060081EF RID: 33263 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void StopSnoreBGEffect()
		{
		}

		// Token: 0x04006293 RID: 25235
		private SchedulerHandle snoreHandle;

		// Token: 0x04006294 RID: 25236
		private KBatchedAnimController snoreEffect;

		// Token: 0x04006295 RID: 25237
		private KBatchedAnimController snoreBGEffect;

		// Token: 0x04006296 RID: 25238
		private const float BGEmissionRadius = 3f;
	}

	// Token: 0x02001884 RID: 6276
	public class States : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer>
	{
		// Token: 0x060081F0 RID: 33264 RVA: 0x0033AB94 File Offset: 0x00338D94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.sleeping, (Snorer.StatesInstance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.DefaultState(this.sleeping.quiet).Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSmallSnore();
			}).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSmallSnore();
			}).Transition(this.idle, (Snorer.StatesInstance smi) => !smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.quiet.Enter("ScheduleNextSnore", delegate(Snorer.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.sleeping.snoring);
			});
			this.sleeping.snoring.Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSnoreBGEffect();
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.sleeping.quiet).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSnoreBGEffect();
			});
		}

		// Token: 0x060081F1 RID: 33265 RVA: 0x000F57F1 File Offset: 0x000F39F1
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(5f, 1f), 3f), 10f);
		}

		// Token: 0x04006297 RID: 25239
		public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State idle;

		// Token: 0x04006298 RID: 25240
		public Snorer.States.SleepStates sleeping;

		// Token: 0x02001885 RID: 6277
		public class SleepStates : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State
		{
			// Token: 0x04006299 RID: 25241
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State quiet;

			// Token: 0x0400629A RID: 25242
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State snoring;
		}
	}
}
