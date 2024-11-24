using Klei.AI;
using STRINGS;

public class HappinessMonitor : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>
{
	public class Def : BaseDef
	{
		public float happyThreshold = 4f;

		public float glumThreshold = -1f;

		public float miserableThreshold = -10f;
	}

	public class MiserableState : State
	{
		public State wild;

		public State tame;
	}

	public class NeutralState : State
	{
		public State wild;

		public State tame;
	}

	public class UnhappyState : State
	{
		public State wild;

		public State tame;
	}

	public class HappyState : State
	{
		public State wild;

		public State tame;
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

	private HappyState happy;

	private NeutralState neutral;

	private UnhappyState glum;

	private MiserableState miserable;

	private Effect happyWildEffect;

	private Effect happyTameEffect;

	private Effect neutralTameEffect;

	private Effect neutralWildEffect;

	private Effect glumWildEffect;

	private Effect glumTameEffect;

	private Effect miserableWildEffect;

	private Effect miserableTameEffect;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = satisfied;
		satisfied.Transition(happy, IsHappy, UpdateRate.SIM_1000ms).Transition(neutral, IsNeutral, UpdateRate.SIM_1000ms).Transition(glum, IsGlum, UpdateRate.SIM_1000ms)
			.Transition(miserable, IsMisirable, UpdateRate.SIM_1000ms);
		happy.DefaultState(happy.wild).Transition(satisfied, GameStateMachine<HappinessMonitor, Instance, IStateMachineTarget, Def>.Not(IsHappy), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Happy);
		happy.wild.ToggleEffect((Instance smi) => happyWildEffect).TagTransition(GameTags.Creatures.Wild, happy.tame, on_remove: true);
		happy.tame.ToggleEffect((Instance smi) => happyTameEffect).TagTransition(GameTags.Creatures.Wild, happy.wild);
		neutral.DefaultState(neutral.wild).Transition(satisfied, GameStateMachine<HappinessMonitor, Instance, IStateMachineTarget, Def>.Not(IsNeutral), UpdateRate.SIM_1000ms);
		neutral.wild.ToggleEffect((Instance smi) => neutralWildEffect).TagTransition(GameTags.Creatures.Wild, neutral.tame, on_remove: true);
		neutral.tame.ToggleEffect((Instance smi) => neutralTameEffect).TagTransition(GameTags.Creatures.Wild, neutral.wild);
		glum.DefaultState(glum.wild).Transition(satisfied, GameStateMachine<HappinessMonitor, Instance, IStateMachineTarget, Def>.Not(IsGlum), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		glum.wild.ToggleEffect((Instance smi) => glumWildEffect).TagTransition(GameTags.Creatures.Wild, glum.tame, on_remove: true);
		glum.tame.ToggleEffect((Instance smi) => glumTameEffect).TagTransition(GameTags.Creatures.Wild, glum.wild);
		miserable.DefaultState(miserable.wild).Transition(satisfied, GameStateMachine<HappinessMonitor, Instance, IStateMachineTarget, Def>.Not(IsMisirable), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		miserable.wild.ToggleEffect((Instance smi) => miserableWildEffect).TagTransition(GameTags.Creatures.Wild, miserable.tame, on_remove: true);
		miserable.tame.ToggleEffect((Instance smi) => miserableTameEffect).TagTransition(GameTags.Creatures.Wild, miserable.wild);
		happyWildEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		happyTameEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_TAME.NAME, CREATURES.MODIFIERS.HAPPY_TAME.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		neutralWildEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		neutralTameEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		glumWildEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
		glumTameEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
		miserableWildEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
		miserableTameEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
		happyTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 9f, CREATURES.MODIFIERS.HAPPY_TAME.NAME, is_multiplier: true));
		glumWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.GLUM.NAME));
		glumTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.GLUM.NAME));
		miserableTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.MISERABLE.NAME));
		miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, is_multiplier: true));
		miserableWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.MISERABLE.NAME));
		miserableWildEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, is_multiplier: true));
	}

	private static bool IsHappy(Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	private static bool IsNeutral(Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		if (totalValue > smi.def.glumThreshold)
		{
			return totalValue < smi.def.happyThreshold;
		}
		return false;
	}

	private static bool IsGlum(Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		if (totalValue > smi.def.miserableThreshold)
		{
			return totalValue <= smi.def.glumThreshold;
		}
		return false;
	}

	private static bool IsMisirable(Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.miserableThreshold;
	}
}
