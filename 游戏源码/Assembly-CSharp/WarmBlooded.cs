using System;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001A33 RID: 6707
public class WarmBlooded : StateMachineComponent<WarmBlooded.StatesInstance>
{
	// Token: 0x06008BDA RID: 35802 RVA: 0x000FB719 File Offset: 0x000F9919
	public static bool IsCold(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsCold();
	}

	// Token: 0x06008BDB RID: 35803 RVA: 0x000FB72B File Offset: 0x000F992B
	public static bool IsHot(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsHot();
	}

	// Token: 0x06008BDC RID: 35804 RVA: 0x00360F0C File Offset: 0x0035F10C
	public static void WarmingRegulator(WarmBlooded.StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.CoolingKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = smi.IdealTemperature - smi.BodyTemperature;
		float num3 = 1f;
		if ((num - smi.baseTemperatureModification.Value) * dt < num2)
		{
			num3 = Mathf.Clamp(num2 / ((num - smi.baseTemperatureModification.Value) * dt), 0f, 1f);
		}
		smi.bodyRegulator.SetValue(-num * num3);
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.CoolingKW * num3 / smi.master.KCal2Joules);
		}
	}

	// Token: 0x06008BDD RID: 35805 RVA: 0x00360FD0 File Offset: 0x0035F1D0
	public static void CoolingRegulator(WarmBlooded.StatesInstance smi, float dt)
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
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.WarmingKW * num4 * 1000f / smi.master.KCal2Joules);
		}
	}

	// Token: 0x06008BDE RID: 35806 RVA: 0x000FB73D File Offset: 0x000F993D
	protected override void OnPrefabInit()
	{
		this.temperature = Db.Get().Amounts.Get(this.TemperatureAmountName).Lookup(base.gameObject);
		this.primaryElement = base.GetComponent<PrimaryElement>();
	}

	// Token: 0x06008BDF RID: 35807 RVA: 0x000FB771 File Offset: 0x000F9971
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06008BE0 RID: 35808 RVA: 0x000FB77E File Offset: 0x000F997E
	public void SetTemperatureImmediate(float t)
	{
		this.temperature.value = t;
	}

	// Token: 0x0400693F RID: 26943
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04006940 RID: 26944
	public AmountInstance temperature;

	// Token: 0x04006941 RID: 26945
	private PrimaryElement primaryElement;

	// Token: 0x04006942 RID: 26946
	public WarmBlooded.ComplexityType complexity = WarmBlooded.ComplexityType.FullHomeostasis;

	// Token: 0x04006943 RID: 26947
	public string TemperatureAmountName = "Temperature";

	// Token: 0x04006944 RID: 26948
	public float IdealTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;

	// Token: 0x04006945 RID: 26949
	public float BaseGenerationKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS;

	// Token: 0x04006946 RID: 26950
	public string BaseTemperatureModifierDescription = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04006947 RID: 26951
	public float KCal2Joules = DUPLICANTSTATS.STANDARD.BaseStats.KCAL2JOULES;

	// Token: 0x04006948 RID: 26952
	public float WarmingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS;

	// Token: 0x04006949 RID: 26953
	public float CoolingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS;

	// Token: 0x0400694A RID: 26954
	public string CaloriesModifierDescription = DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME;

	// Token: 0x0400694B RID: 26955
	public string BodyRegulatorModifierDescription = DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME;

	// Token: 0x0400694C RID: 26956
	public const float TRANSITION_DELAY_HOT = 3f;

	// Token: 0x0400694D RID: 26957
	public const float TRANSITION_DELAY_COLD = 3f;

	// Token: 0x02001A34 RID: 6708
	public enum ComplexityType
	{
		// Token: 0x0400694F RID: 26959
		SimpleHeatProduction,
		// Token: 0x04006950 RID: 26960
		HomeostasisWithoutCaloriesImpact,
		// Token: 0x04006951 RID: 26961
		FullHomeostasis
	}

	// Token: 0x02001A35 RID: 6709
	public class StatesInstance : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.GameInstance
	{
		// Token: 0x06008BE2 RID: 35810 RVA: 0x00361168 File Offset: 0x0035F368
		public StatesInstance(WarmBlooded smi) : base(smi)
		{
			this.baseTemperatureModification = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BaseTemperatureModifierDescription, false, true, false);
			base.master.GetAttributes().Add(this.baseTemperatureModification);
			if (base.master.complexity != WarmBlooded.ComplexityType.SimpleHeatProduction)
			{
				this.bodyRegulator = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BodyRegulatorModifierDescription, false, true, false);
				base.master.GetAttributes().Add(this.bodyRegulator);
			}
			if (base.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
			{
				this.burningCalories = new AttributeModifier("CaloriesDelta", 0f, base.master.CaloriesModifierDescription, false, false, false);
				base.master.GetAttributes().Add(this.burningCalories);
			}
			base.master.SetTemperatureImmediate(this.IdealTemperature);
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06008BE3 RID: 35811 RVA: 0x000FB78C File Offset: 0x000F998C
		public float IdealTemperature
		{
			get
			{
				return base.master.IdealTemperature;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06008BE4 RID: 35812 RVA: 0x000FB799 File Offset: 0x000F9999
		public float TemperatureDelta
		{
			get
			{
				return this.bodyRegulator.Value;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06008BE5 RID: 35813 RVA: 0x000FB7A6 File Offset: 0x000F99A6
		public float BodyTemperature
		{
			get
			{
				return base.master.primaryElement.Temperature;
			}
		}

		// Token: 0x06008BE6 RID: 35814 RVA: 0x000FB7B8 File Offset: 0x000F99B8
		public bool IsSimpleHeatProducer()
		{
			return base.master.complexity == WarmBlooded.ComplexityType.SimpleHeatProduction;
		}

		// Token: 0x06008BE7 RID: 35815 RVA: 0x000FB7C8 File Offset: 0x000F99C8
		public bool IsHot()
		{
			return this.BodyTemperature > this.IdealTemperature;
		}

		// Token: 0x06008BE8 RID: 35816 RVA: 0x000FB7D8 File Offset: 0x000F99D8
		public bool IsCold()
		{
			return this.BodyTemperature < this.IdealTemperature;
		}

		// Token: 0x04006952 RID: 26962
		public AttributeModifier baseTemperatureModification;

		// Token: 0x04006953 RID: 26963
		public AttributeModifier bodyRegulator;

		// Token: 0x04006954 RID: 26964
		public AttributeModifier burningCalories;
	}

	// Token: 0x02001A36 RID: 6710
	public class States : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded>
	{
		// Token: 0x06008BE9 RID: 35817 RVA: 0x00361274 File Offset: 0x0035F474
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.normal;
			this.root.TagTransition(GameTags.Dead, this.dead, false).Enter(delegate(WarmBlooded.StatesInstance smi)
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
			this.alive.normal.Transition(this.alive.cold.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold), UpdateRate.SIM_200ms).Transition(this.alive.hot.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot), UpdateRate.SIM_200ms);
			this.alive.cold.transition.ScheduleGoTo(3f, this.alive.cold.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms);
			this.alive.cold.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms).Update("ColdRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.CoolingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
				if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
				{
					smi.burningCalories.SetValue(0f);
				}
			});
			this.alive.hot.transition.ScheduleGoTo(3f, this.alive.hot.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms);
			this.alive.hot.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms).Update("WarmRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.WarmingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
			});
			this.dead.Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.master.enabled = false;
			});
		}

		// Token: 0x04006955 RID: 26965
		public WarmBlooded.States.AliveState alive;

		// Token: 0x04006956 RID: 26966
		public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State dead;

		// Token: 0x02001A37 RID: 6711
		public class RegulatingState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x04006957 RID: 26967
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State transition;

			// Token: 0x04006958 RID: 26968
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State regulating;
		}

		// Token: 0x02001A38 RID: 6712
		public class AliveState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x04006959 RID: 26969
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State normal;

			// Token: 0x0400695A RID: 26970
			public WarmBlooded.States.RegulatingState cold;

			// Token: 0x0400695B RID: 26971
			public WarmBlooded.States.RegulatingState hot;
		}
	}
}
