using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class WarmBlooded : StateMachineComponent<WarmBlooded.StatesInstance>
{
	public enum ComplexityType
	{
		SimpleHeatProduction,
		HomeostasisWithoutCaloriesImpact,
		FullHomeostasis
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, WarmBlooded, object>.GameInstance
	{
		public AttributeModifier baseTemperatureModification;

		public AttributeModifier bodyRegulator;

		public AttributeModifier burningCalories;

		public float IdealTemperature => base.master.IdealTemperature;

		public float TemperatureDelta => bodyRegulator.Value;

		public float BodyTemperature => base.master.primaryElement.Temperature;

		public StatesInstance(WarmBlooded smi)
			: base(smi)
		{
			baseTemperatureModification = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BaseTemperatureModifierDescription, is_multiplier: false, uiOnly: true, is_readonly: false);
			base.master.GetAttributes().Add(baseTemperatureModification);
			if (base.master.complexity != 0)
			{
				bodyRegulator = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BodyRegulatorModifierDescription, is_multiplier: false, uiOnly: true, is_readonly: false);
				base.master.GetAttributes().Add(bodyRegulator);
			}
			if (base.master.complexity == ComplexityType.FullHomeostasis)
			{
				burningCalories = new AttributeModifier("CaloriesDelta", 0f, base.master.CaloriesModifierDescription, is_multiplier: false, uiOnly: false, is_readonly: false);
				base.master.GetAttributes().Add(burningCalories);
			}
			base.master.SetTemperatureImmediate(IdealTemperature);
		}

		public bool IsSimpleHeatProducer()
		{
			return base.master.complexity == ComplexityType.SimpleHeatProduction;
		}

		public bool IsHot()
		{
			return BodyTemperature > IdealTemperature;
		}

		public bool IsCold()
		{
			return BodyTemperature < IdealTemperature;
		}
	}

	public class States : GameStateMachine<States, StatesInstance, WarmBlooded>
	{
		public class RegulatingState : State
		{
			public State transition;

			public State regulating;
		}

		public class AliveState : State
		{
			public State normal;

			public RegulatingState cold;

			public RegulatingState hot;
		}

		public AliveState alive;

		public State dead;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = alive.normal;
			root.TagTransition(GameTags.Dead, dead).Enter(delegate(StatesInstance smi)
			{
				PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
				float value = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
				smi.baseTemperatureModification.SetValue(value);
				CreatureSimTemperatureTransfer component2 = smi.master.GetComponent<CreatureSimTemperatureTransfer>();
				component2.NonSimTemperatureModifiers.Add(smi.baseTemperatureModification);
				if (!smi.IsSimpleHeatProducer())
				{
					component2.NonSimTemperatureModifiers.Add(smi.bodyRegulator);
				}
			});
			alive.normal.Transition(alive.cold.transition, IsCold).Transition(alive.hot.transition, IsHot);
			alive.cold.transition.ScheduleGoTo(3f, alive.cold.regulating).Transition(alive.normal, GameStateMachine<States, StatesInstance, WarmBlooded, object>.Not(IsCold));
			alive.cold.regulating.Transition(alive.normal, GameStateMachine<States, StatesInstance, WarmBlooded, object>.Not(IsCold)).Update("ColdRegulating", CoolingRegulator).Exit(delegate(StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
				if (smi.master.complexity == ComplexityType.FullHomeostasis)
				{
					smi.burningCalories.SetValue(0f);
				}
			});
			alive.hot.transition.ScheduleGoTo(3f, alive.hot.regulating).Transition(alive.normal, GameStateMachine<States, StatesInstance, WarmBlooded, object>.Not(IsHot));
			alive.hot.regulating.Transition(alive.normal, GameStateMachine<States, StatesInstance, WarmBlooded, object>.Not(IsHot)).Update("WarmRegulating", WarmingRegulator).Exit(delegate(StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
			});
			dead.Enter(delegate(StatesInstance smi)
			{
				smi.master.enabled = false;
			});
		}
	}

	[MyCmpAdd]
	private Notifier notifier;

	public AmountInstance temperature;

	private PrimaryElement primaryElement;

	public ComplexityType complexity = ComplexityType.FullHomeostasis;

	public string TemperatureAmountName = "Temperature";

	public float IdealTemperature = 310.15f;

	public float BaseGenerationKW = 0.08368001f;

	public string BaseTemperatureModifierDescription = DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME;

	public float KCal2Joules = 4184f;

	public float WarmingKW = 0.5578667f;

	public float CoolingKW = 0.5578667f;

	public string CaloriesModifierDescription = DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME;

	public string BodyRegulatorModifierDescription = DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME;

	public const float TRANSITION_DELAY_HOT = 3f;

	public const float TRANSITION_DELAY_COLD = 3f;

	public static bool IsCold(StatesInstance smi)
	{
		if (!smi.IsSimpleHeatProducer())
		{
			return smi.IsCold();
		}
		return false;
	}

	public static bool IsHot(StatesInstance smi)
	{
		if (!smi.IsSimpleHeatProducer())
		{
			return smi.IsHot();
		}
		return false;
	}

	public static void WarmingRegulator(StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.CoolingKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = smi.IdealTemperature - smi.BodyTemperature;
		float num3 = 1f;
		if ((num - smi.baseTemperatureModification.Value) * dt < num2)
		{
			num3 = Mathf.Clamp(num2 / ((num - smi.baseTemperatureModification.Value) * dt), 0f, 1f);
		}
		smi.bodyRegulator.SetValue((0f - num) * num3);
		if (smi.master.complexity == ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue((0f - smi.master.CoolingKW) * num3 / smi.master.KCal2Joules);
		}
	}

	public static void CoolingRegulator(StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = SimUtil.EnergyFlowToTemperatureDelta(smi.master.WarmingKW, component.Element.specificHeatCapacity, component.Mass);
		float num3 = smi.IdealTemperature - smi.BodyTemperature;
		float num4 = 1f;
		if (num2 + num > num3)
		{
			num4 = Mathf.Max(0f, num3 - num) / num2;
		}
		smi.bodyRegulator.SetValue(num2 * num4);
		if (smi.master.complexity == ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue((0f - smi.master.WarmingKW) * num4 * 1000f / smi.master.KCal2Joules);
		}
	}

	protected override void OnPrefabInit()
	{
		temperature = Db.Get().Amounts.Get(TemperatureAmountName).Lookup(base.gameObject);
		primaryElement = GetComponent<PrimaryElement>();
	}

	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	public void SetTemperatureImmediate(float t)
	{
		temperature.value = t;
	}
}
