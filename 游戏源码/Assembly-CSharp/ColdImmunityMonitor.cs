using Klei.AI;
using UnityEngine;

public class ColdImmunityMonitor : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance>
{
	public class ColdStates : State
	{
		public State idle;

		public State exiting;

		public State resetChore;
	}

	public class IdleStates : State
	{
		public State feelingFine;

		public State leftWithDesireToWarmupAfterBeingCold;
	}

	public new class Instance : GameInstance
	{
		private Navigator navigator;

		public ColdImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		public int WarmUpCell { get; private set; }

		public float ColdCountdown => base.smi.sm.coldCountdown.Get(this);

		public Instance(IStateMachineTarget master)
			: base(master)
		{
		}

		public override void StartSM()
		{
			navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		public void UpdateWarmUpCell()
		{
			int myWorldId = navigator.GetMyWorldId();
			int warmUpCell = Grid.InvalidCell;
			int num = int.MaxValue;
			ColdImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance item in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is ColdImmunityProvider.Instance))
			{
				ColdImmunityProvider.Instance instance = item as ColdImmunityProvider.Instance;
				if (instance.GetMyWorldId() == myWorldId)
				{
					int _cost = int.MaxValue;
					int bestAvailableCell = instance.GetBestAvailableCell(navigator, out _cost);
					if (_cost < num)
					{
						num = _cost;
						nearestImmunityProvider = instance;
						warmUpCell = bestAvailableCell;
					}
				}
			}
			NearestImmunityProvider = nearestImmunityProvider;
			WarmUpCell = warmUpCell;
		}
	}

	private const float EFFECT_DURATION = 5f;

	public IdleStates idle;

	public ColdStates cold;

	public FloatParameter coldCountdown;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = idle;
		idle.DefaultState(idle.feelingFine).TagTransition(GameTags.FeelingCold, cold).ParamTransition(coldCountdown, cold, GameStateMachine<ColdImmunityMonitor, Instance, IStateMachineTarget, object>.IsGTZero);
		idle.feelingFine.DoNothing();
		idle.leftWithDesireToWarmupAfterBeingCold.Enter(UpdateWarmUpCell).Update(UpdateWarmUpCell, UpdateRate.RENDER_1000ms).ToggleChore(CreateRecoverFromChillyBonesChore, idle.feelingFine, idle.feelingFine);
		cold.DefaultState(cold.exiting).TagTransition(GameTags.FeelingWarm, idle).ToggleAnims("anim_idle_cold_kanim")
			.ToggleAnims("anim_loco_run_cold_kanim")
			.ToggleAnims("anim_loco_walk_cold_kanim")
			.ToggleExpression(Db.Get().Expressions.Cold)
			.ToggleThought(Db.Get().Thoughts.Cold)
			.ToggleEffect("ColdAir")
			.Enter(UpdateWarmUpCell)
			.Update(UpdateWarmUpCell, UpdateRate.RENDER_1000ms)
			.ToggleChore(CreateRecoverFromChillyBonesChore, idle, cold);
		cold.exiting.EventHandlerTransition(GameHashes.EffectAdded, idle, HasImmunityEffect).TagTransition(GameTags.FeelingCold, cold.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingCold)
			.ParamTransition(coldCountdown, idle.leftWithDesireToWarmupAfterBeingCold, GameStateMachine<ColdImmunityMonitor, Instance, IStateMachineTarget, object>.IsZero)
			.Update(ColdTimerUpdate)
			.Exit(ClearTimer);
		cold.idle.Enter(ResetColdTimer).ToggleStatusItem(Db.Get().DuplicantStatusItems.Cold, (Instance smi) => smi).TagTransition(GameTags.FeelingCold, cold.exiting, on_remove: true);
	}

	public static bool OnEffectAdded(Instance smi, object data)
	{
		return true;
	}

	public static void ClearTimer(Instance smi)
	{
		smi.sm.coldCountdown.Set(0f, smi);
	}

	public static void ResetColdTimer(Instance smi)
	{
		smi.sm.coldCountdown.Set(5f, smi);
	}

	public static void ColdTimerUpdate(Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.ColdCountdown - dt, 0f, 5f);
		smi.sm.coldCountdown.Set(value, smi);
	}

	private static void UpdateWarmUpCell(Instance smi, float dt)
	{
		smi.UpdateWarmUpCell();
	}

	private static void UpdateWarmUpCell(Instance smi)
	{
		smi.UpdateWarmUpCell();
	}

	public static bool HasImmunityEffect(Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		if (component != null)
		{
			return component.HasEffect("WarmTouch");
		}
		return false;
	}

	private static Chore CreateRecoverFromChillyBonesChore(Instance smi)
	{
		return new RecoverFromColdChore(smi.master);
	}
}
