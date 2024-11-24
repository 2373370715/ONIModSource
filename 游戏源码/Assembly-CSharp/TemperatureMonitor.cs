using Klei.AI;

public class TemperatureMonitor : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance>
{
	public new class Instance : GameInstance
	{
		public AmountInstance temperature;

		public PrimaryElement primaryElement;

		private Navigator navigator;

		private SafetyQuery warmUpQuery;

		private SafetyQuery coolDownQuery;

		public float averageTemperature;

		public float HypothermiaThreshold = 307.15f;

		public float HyperthermiaThreshold = 313.15f;

		public Instance(IStateMachineTarget master)
			: base(master)
		{
			primaryElement = GetComponent<PrimaryElement>();
			temperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			warmUpQuery = new SafetyQuery(Game.Instance.safetyConditions.WarmUpChecker, GetComponent<KMonoBehaviour>(), int.MaxValue);
			coolDownQuery = new SafetyQuery(Game.Instance.safetyConditions.CoolDownChecker, GetComponent<KMonoBehaviour>(), int.MaxValue);
			navigator = GetComponent<Navigator>();
		}

		public void UpdateTemperature(float dt)
		{
			base.smi.averageTemperature *= 1f - dt / 4f;
			base.smi.averageTemperature += base.smi.primaryElement.Temperature * (dt / 4f);
			base.smi.temperature.SetValue(base.smi.averageTemperature);
		}

		public bool IsHyperthermic()
		{
			return temperature.value > HyperthermiaThreshold;
		}

		public bool IsHypothermic()
		{
			return temperature.value < HypothermiaThreshold;
		}

		public float ExtremeTemperatureDelta()
		{
			if (temperature.value > HyperthermiaThreshold)
			{
				return temperature.value - HyperthermiaThreshold;
			}
			if (temperature.value < HypothermiaThreshold)
			{
				return temperature.value - HypothermiaThreshold;
			}
			return 0f;
		}

		public float IdealTemperatureDelta()
		{
			return temperature.value - 310.15f;
		}

		public int GetWarmUpCell()
		{
			return base.sm.warmUpCell.Get(base.smi);
		}

		public int GetCoolDownCell()
		{
			return base.sm.coolDownCell.Get(base.smi);
		}

		public void UpdateWarmUpCell()
		{
			warmUpQuery.Reset();
			navigator.RunQuery(warmUpQuery);
			base.sm.warmUpCell.Set(warmUpQuery.GetResultCell(), base.smi);
		}

		public void UpdateCoolDownCell()
		{
			coolDownQuery.Reset();
			navigator.RunQuery(coolDownQuery);
			base.sm.coolDownCell.Set(coolDownQuery.GetResultCell(), base.smi);
		}
	}

	public State homeostatic;

	public State hyperthermic;

	public State hypothermic;

	public State hyperthermic_pre;

	public State hypothermic_pre;

	private const float TEMPERATURE_AVERAGING_RANGE = 4f;

	public IntParameter warmUpCell;

	public IntParameter coolDownCell;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = homeostatic;
		root.Enter(delegate(Instance smi)
		{
			smi.averageTemperature = smi.primaryElement.Temperature;
		}).Update("UpdateTemperature", delegate(Instance smi, float dt)
		{
			smi.UpdateTemperature(dt);
		});
		homeostatic.Transition(hyperthermic_pre, (Instance smi) => smi.IsHyperthermic()).Transition(hypothermic_pre, (Instance smi) => smi.IsHypothermic()).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved);
		hyperthermic_pre.Enter(delegate(Instance smi)
		{
			smi.GoTo(hyperthermic);
		});
		hypothermic_pre.Enter(delegate(Instance smi)
		{
			smi.GoTo(hypothermic);
		});
		hyperthermic.Transition(homeostatic, (Instance smi) => !smi.IsHyperthermic()).ToggleUrge(Db.Get().Urges.CoolDown);
		hypothermic.Transition(homeostatic, (Instance smi) => !smi.IsHypothermic()).ToggleUrge(Db.Get().Urges.WarmUp);
	}
}
