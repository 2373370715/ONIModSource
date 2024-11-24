using Klei.AI;
using STRINGS;

public class BeeHappinessMonitor : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>
{
	public class Def : BaseDef
	{
		public float happyThreshold = 4f;

		public float unhappyThreshold = -1f;
	}

	public new class Instance : GameInstance
	{
		public AttributeInstance happiness;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}
	}

	private State satisfied;

	private State happy;

	private State unhappy;

	private Effect happyEffect;

	private Effect neutralEffect;

	private Effect unhappyEffect;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = satisfied;
		satisfied.TriggerOnEnter(GameHashes.Satisfied).Transition(happy, IsHappy, UpdateRate.SIM_1000ms).Transition(unhappy, IsUnhappy, UpdateRate.SIM_1000ms)
			.ToggleEffect((Instance smi) => neutralEffect);
		happy.TriggerOnEnter(GameHashes.Happy).ToggleEffect((Instance smi) => happyEffect).Transition(satisfied, GameStateMachine<BeeHappinessMonitor, Instance, IStateMachineTarget, Def>.Not(IsHappy), UpdateRate.SIM_1000ms);
		unhappy.TriggerOnEnter(GameHashes.Unhappy).Transition(satisfied, GameStateMachine<BeeHappinessMonitor, Instance, IStateMachineTarget, Def>.Not(IsUnhappy), UpdateRate.SIM_1000ms).ToggleEffect((Instance smi) => unhappyEffect);
		happyEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		neutralEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		unhappyEffect = new Effect("Unhappy", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
	}

	private static bool IsHappy(Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	private static bool IsUnhappy(Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.unhappyThreshold;
	}
}
