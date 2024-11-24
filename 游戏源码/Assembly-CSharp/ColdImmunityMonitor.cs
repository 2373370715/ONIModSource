using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001536 RID: 5430
public class ColdImmunityMonitor : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance>
{
	// Token: 0x06007143 RID: 28995 RVA: 0x002FA0D0 File Offset: 0x002F82D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingCold, this.cold, false).ParamTransition<float>(this.coldCountdown, this.cold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToWarmupAfterBeingCold.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle.feelingFine, this.idle.feelingFine);
		this.cold.DefaultState(this.cold.exiting).TagTransition(GameTags.FeelingWarm, this.idle, false).ToggleAnims("anim_idle_cold_kanim", 0f).ToggleAnims("anim_loco_run_cold_kanim", 0f).ToggleAnims("anim_loco_walk_cold_kanim", 0f).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleEffect("ColdAir").Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle, this.cold);
		this.cold.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<ColdImmunityMonitor.Instance, object, bool>(ColdImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingCold, this.cold.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingCold, null).ParamTransition<float>(this.coldCountdown, this.idle.leftWithDesireToWarmupAfterBeingCold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.ColdTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ClearTimer));
		this.cold.idle.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ResetColdTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Cold, (ColdImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingCold, this.cold.exiting, true);
	}

	// Token: 0x06007144 RID: 28996 RVA: 0x000A65EC File Offset: 0x000A47EC
	public static bool OnEffectAdded(ColdImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x06007145 RID: 28997 RVA: 0x000EA2DF File Offset: 0x000E84DF
	public static void ClearTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(0f, smi, false);
	}

	// Token: 0x06007146 RID: 28998 RVA: 0x000EA2F9 File Offset: 0x000E84F9
	public static void ResetColdTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(5f, smi, false);
	}

	// Token: 0x06007147 RID: 28999 RVA: 0x002FA354 File Offset: 0x002F8554
	public static void ColdTimerUpdate(ColdImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.ColdCountdown - dt, 0f, 5f);
		smi.sm.coldCountdown.Set(value, smi, false);
	}

	// Token: 0x06007148 RID: 29000 RVA: 0x000EA313 File Offset: 0x000E8513
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x06007149 RID: 29001 RVA: 0x000EA313 File Offset: 0x000E8513
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x0600714A RID: 29002 RVA: 0x002FA390 File Offset: 0x002F8590
	public static bool HasImmunityEffect(ColdImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("WarmTouch");
	}

	// Token: 0x0600714B RID: 29003 RVA: 0x000EA31B File Offset: 0x000E851B
	private static Chore CreateRecoverFromChillyBonesChore(ColdImmunityMonitor.Instance smi)
	{
		return new RecoverFromColdChore(smi.master);
	}

	// Token: 0x04005496 RID: 21654
	private const float EFFECT_DURATION = 5f;

	// Token: 0x04005497 RID: 21655
	public ColdImmunityMonitor.IdleStates idle;

	// Token: 0x04005498 RID: 21656
	public ColdImmunityMonitor.ColdStates cold;

	// Token: 0x04005499 RID: 21657
	public StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter coldCountdown;

	// Token: 0x02001537 RID: 5431
	public class ColdStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400549A RID: 21658
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x0400549B RID: 21659
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x0400549C RID: 21660
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x02001538 RID: 5432
	public class IdleStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400549D RID: 21661
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x0400549E RID: 21662
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToWarmupAfterBeingCold;
	}

	// Token: 0x02001539 RID: 5433
	public new class Instance : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x0600714F RID: 29007 RVA: 0x000EA338 File Offset: 0x000E8538
		// (set) Token: 0x06007150 RID: 29008 RVA: 0x000EA340 File Offset: 0x000E8540
		public ColdImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06007151 RID: 29009 RVA: 0x000EA349 File Offset: 0x000E8549
		// (set) Token: 0x06007152 RID: 29010 RVA: 0x000EA351 File Offset: 0x000E8551
		public int WarmUpCell { get; private set; }

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06007153 RID: 29011 RVA: 0x000EA35A File Offset: 0x000E855A
		public float ColdCountdown
		{
			get
			{
				return base.smi.sm.coldCountdown.Get(this);
			}
		}

		// Token: 0x06007154 RID: 29012 RVA: 0x000EA372 File Offset: 0x000E8572
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007155 RID: 29013 RVA: 0x000EA37B File Offset: 0x000E857B
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x06007156 RID: 29014 RVA: 0x002FA3BC File Offset: 0x002F85BC
		public void UpdateWarmUpCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int warmUpCell = Grid.InvalidCell;
			int num = int.MaxValue;
			ColdImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is ColdImmunityProvider.Instance))
			{
				ColdImmunityProvider.Instance instance2 = instance as ColdImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						warmUpCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.WarmUpCell = warmUpCell;
		}

		// Token: 0x040054A1 RID: 21665
		private Navigator navigator;
	}
}
