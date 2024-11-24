using Klei.AI;
using UnityEngine;

public class HeatImmunityMonitor : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance>
{
	public class WarmStates : State
	{
		public State idle;

		public State exiting;

		public State resetChore;
	}

	public class IdleStates : State
	{
		public State feelingFine;

		public State leftWithDesireToCooldownAfterBeingWarm;
	}

	public new class Instance : GameInstance
	{
		private Navigator navigator;

		public HeatImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		public int ShelterCell { get; private set; }

		public float HeatCountdown => base.smi.sm.heatCountdown.Get(this);

		public Instance(IStateMachineTarget master)
			: base(master)
		{
		}

		public override void StartSM()
		{
			navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		public void UpdateShelterCell()
		{
			int myWorldId = navigator.GetMyWorldId();
			int shelterCell = Grid.InvalidCell;
			int num = int.MaxValue;
			HeatImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance item in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is HeatImmunityProvider.Instance))
			{
				HeatImmunityProvider.Instance instance = item as HeatImmunityProvider.Instance;
				if (instance.GetMyWorldId() == myWorldId)
				{
					int _cost = int.MaxValue;
					int bestAvailableCell = instance.GetBestAvailableCell(navigator, out _cost);
					if (_cost < num)
					{
						num = _cost;
						nearestImmunityProvider = instance;
						shelterCell = bestAvailableCell;
					}
				}
			}
			NearestImmunityProvider = nearestImmunityProvider;
			ShelterCell = shelterCell;
		}
	}

	private const float EFFECT_DURATION = 5f;

	public IdleStates idle;

	public WarmStates warm;

	public FloatParameter heatCountdown;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = idle;
		idle.DefaultState(idle.feelingFine).TagTransition(GameTags.FeelingWarm, warm).ParamTransition(heatCountdown, warm, GameStateMachine<HeatImmunityMonitor, Instance, IStateMachineTarget, object>.IsGTZero);
		idle.feelingFine.DoNothing();
		idle.leftWithDesireToCooldownAfterBeingWarm.Enter(UpdateShelterCell).Update(UpdateShelterCell, UpdateRate.RENDER_1000ms).ToggleChore(CreateRecoverFromOverheatChore, idle.feelingFine, idle.feelingFine);
		warm.DefaultState(warm.exiting).TagTransition(GameTags.FeelingCold, idle).ToggleAnims("anim_idle_hot_kanim")
			.ToggleAnims("anim_loco_run_hot_kanim")
			.ToggleAnims("anim_loco_walk_hot_kanim")
			.ToggleExpression(Db.Get().Expressions.Hot)
			.ToggleThought(Db.Get().Thoughts.Hot)
			.ToggleEffect("WarmAir")
			.Enter(UpdateShelterCell)
			.Update(UpdateShelterCell, UpdateRate.RENDER_1000ms)
			.ToggleChore(CreateRecoverFromOverheatChore, idle, warm);
		warm.exiting.EventHandlerTransition(GameHashes.EffectAdded, idle, HasImmunityEffect).TagTransition(GameTags.FeelingWarm, warm.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingHot)
			.ParamTransition(heatCountdown, idle.leftWithDesireToCooldownAfterBeingWarm, GameStateMachine<HeatImmunityMonitor, Instance, IStateMachineTarget, object>.IsZero)
			.Update(HeatTimerUpdate)
			.Exit(ClearTimer);
		warm.idle.Enter(ResetHeatTimer).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hot, (Instance smi) => smi).TagTransition(GameTags.FeelingWarm, warm.exiting, on_remove: true);
	}

	public static bool OnEffectAdded(Instance smi, object data)
	{
		return true;
	}

	public static void ClearTimer(Instance smi)
	{
		smi.sm.heatCountdown.Set(0f, smi);
	}

	public static void ResetHeatTimer(Instance smi)
	{
		smi.sm.heatCountdown.Set(5f, smi);
	}

	public static void HeatTimerUpdate(Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.HeatCountdown - dt, 0f, 5f);
		smi.sm.heatCountdown.Set(value, smi);
	}

	private static void UpdateShelterCell(Instance smi, float dt)
	{
		smi.UpdateShelterCell();
	}

	private static void UpdateShelterCell(Instance smi)
	{
		smi.UpdateShelterCell();
	}

	public static bool HasImmunityEffect(Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		if (component != null)
		{
			return component.HasEffect("RefreshingTouch");
		}
		return false;
	}

	private static Chore CreateRecoverFromOverheatChore(Instance smi)
	{
		return new RecoverFromHeatChore(smi.master);
	}
}
