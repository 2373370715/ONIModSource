using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200158A RID: 5514
public class HeatImmunityMonitor : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance>
{
	// Token: 0x06007297 RID: 29335 RVA: 0x002FE378 File Offset: 0x002FC578
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingWarm, this.warm, false).ParamTransition<float>(this.heatCountdown, this.warm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToCooldownAfterBeingWarm.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle.feelingFine, this.idle.feelingFine);
		this.warm.DefaultState(this.warm.exiting).TagTransition(GameTags.FeelingCold, this.idle, false).ToggleAnims("anim_idle_hot_kanim", 0f).ToggleAnims("anim_loco_run_hot_kanim", 0f).ToggleAnims("anim_loco_walk_hot_kanim", 0f).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleEffect("WarmAir").Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle, this.warm);
		this.warm.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<HeatImmunityMonitor.Instance, object, bool>(HeatImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingWarm, this.warm.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingHot, null).ParamTransition<float>(this.heatCountdown, this.idle.leftWithDesireToCooldownAfterBeingWarm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.HeatTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ClearTimer));
		this.warm.idle.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ResetHeatTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hot, (HeatImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingWarm, this.warm.exiting, true);
	}

	// Token: 0x06007298 RID: 29336 RVA: 0x000A65EC File Offset: 0x000A47EC
	public static bool OnEffectAdded(HeatImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x06007299 RID: 29337 RVA: 0x000EB0CD File Offset: 0x000E92CD
	public static void ClearTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(0f, smi, false);
	}

	// Token: 0x0600729A RID: 29338 RVA: 0x000EB0E7 File Offset: 0x000E92E7
	public static void ResetHeatTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(5f, smi, false);
	}

	// Token: 0x0600729B RID: 29339 RVA: 0x002FE5FC File Offset: 0x002FC7FC
	public static void HeatTimerUpdate(HeatImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.HeatCountdown - dt, 0f, 5f);
		smi.sm.heatCountdown.Set(value, smi, false);
	}

	// Token: 0x0600729C RID: 29340 RVA: 0x000EB101 File Offset: 0x000E9301
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x0600729D RID: 29341 RVA: 0x000EB101 File Offset: 0x000E9301
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x0600729E RID: 29342 RVA: 0x002FE638 File Offset: 0x002FC838
	public static bool HasImmunityEffect(HeatImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("RefreshingTouch");
	}

	// Token: 0x0600729F RID: 29343 RVA: 0x000EB109 File Offset: 0x000E9309
	private static Chore CreateRecoverFromOverheatChore(HeatImmunityMonitor.Instance smi)
	{
		return new RecoverFromHeatChore(smi.master);
	}

	// Token: 0x040055B3 RID: 21939
	private const float EFFECT_DURATION = 5f;

	// Token: 0x040055B4 RID: 21940
	public HeatImmunityMonitor.IdleStates idle;

	// Token: 0x040055B5 RID: 21941
	public HeatImmunityMonitor.WarmStates warm;

	// Token: 0x040055B6 RID: 21942
	public StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter heatCountdown;

	// Token: 0x0200158B RID: 5515
	public class WarmStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040055B7 RID: 21943
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040055B8 RID: 21944
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x040055B9 RID: 21945
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x0200158C RID: 5516
	public class IdleStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040055BA RID: 21946
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x040055BB RID: 21947
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToCooldownAfterBeingWarm;
	}

	// Token: 0x0200158D RID: 5517
	public new class Instance : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060072A3 RID: 29347 RVA: 0x000EB126 File Offset: 0x000E9326
		// (set) Token: 0x060072A4 RID: 29348 RVA: 0x000EB12E File Offset: 0x000E932E
		public HeatImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x060072A5 RID: 29349 RVA: 0x000EB137 File Offset: 0x000E9337
		// (set) Token: 0x060072A6 RID: 29350 RVA: 0x000EB13F File Offset: 0x000E933F
		public int ShelterCell { get; private set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x060072A7 RID: 29351 RVA: 0x000EB148 File Offset: 0x000E9348
		public float HeatCountdown
		{
			get
			{
				return base.smi.sm.heatCountdown.Get(this);
			}
		}

		// Token: 0x060072A8 RID: 29352 RVA: 0x000EB160 File Offset: 0x000E9360
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060072A9 RID: 29353 RVA: 0x000EB169 File Offset: 0x000E9369
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x060072AA RID: 29354 RVA: 0x002FE664 File Offset: 0x002FC864
		public void UpdateShelterCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int shelterCell = Grid.InvalidCell;
			int num = int.MaxValue;
			HeatImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is HeatImmunityProvider.Instance))
			{
				HeatImmunityProvider.Instance instance2 = instance as HeatImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						shelterCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.ShelterCell = shelterCell;
		}

		// Token: 0x040055BE RID: 21950
		private Navigator navigator;
	}
}
