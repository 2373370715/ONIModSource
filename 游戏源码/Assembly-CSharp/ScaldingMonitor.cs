using Klei.AI;
using STRINGS;
using UnityEngine;

public class ScaldingMonitor : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>
{
	public class Def : BaseDef
	{
		public float defaultScaldingTreshold = 345f;

		public float defaultScoldingTreshold = 183f;
	}

	public new class Instance : GameInstance
	{
		public float AverageExternalTemperature;

		private float lastScaldTime;

		private Attributes attributes;

		[MyCmpGet]
		private Health health;

		[MyCmpGet]
		private OccupyArea occupyArea;

		private AttributeModifier baseScalindingThreshold;

		private AttributeModifier baseScoldingThreshold;

		public AmountInstance internalTemperature;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			baseScalindingThreshold = new AttributeModifier("ScaldingThreshold", def.defaultScaldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME);
			baseScoldingThreshold = new AttributeModifier("ScoldingThreshold", def.defaultScoldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME);
			attributes = base.gameObject.GetAttributes();
		}

		public override void StartSM()
		{
			base.smi.attributes.Get(Db.Get().Attributes.ScaldingThreshold).Add(baseScalindingThreshold);
			base.smi.attributes.Get(Db.Get().Attributes.ScoldingThreshold).Add(baseScoldingThreshold);
			base.StartSM();
		}

		public bool IsScalding()
		{
			int num = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(num) || Grid.Element[num].id == SimHashes.Vacuum || Grid.Element[num].id == SimHashes.Void)
			{
				return false;
			}
			return AverageExternalTemperature > GetScaldingThreshold();
		}

		public float GetScaldingThreshold()
		{
			return base.smi.attributes.GetValue("ScaldingThreshold");
		}

		public bool IsScolding()
		{
			int num = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(num) || Grid.Element[num].id == SimHashes.Vacuum || Grid.Element[num].id == SimHashes.Void)
			{
				return false;
			}
			return AverageExternalTemperature < GetScoldingThreshold();
		}

		public float GetScoldingThreshold()
		{
			return base.smi.attributes.GetValue("ScoldingThreshold");
		}

		public void TemperatureDamage(float dt)
		{
			if (health != null && Time.time - lastScaldTime > 5f)
			{
				lastScaldTime = Time.time;
				health.Damage(dt * 10f);
			}
		}

		public void ResetExternalTemperatureAverage()
		{
			base.smi.AverageExternalTemperature = internalTemperature.value;
		}

		public float GetCurrentExternalTemperature()
		{
			int num = Grid.PosToCell(base.gameObject);
			if (occupyArea != null)
			{
				float num2 = 0f;
				int num3 = 0;
				for (int i = 0; i < occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					int num4 = Grid.OffsetCell(num, occupyArea.OccupiedCellsOffsets[i]);
					if (Grid.IsValidCell(num4))
					{
						bool flag = Grid.Element[num4].id == SimHashes.Vacuum || Grid.Element[num4].id == SimHashes.Void;
						num3++;
						num2 += (flag ? internalTemperature.value : Grid.Temperature[num4]);
					}
				}
				return num2 / (float)Mathf.Max(1, num3);
			}
			if (Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void)
			{
				return Grid.Temperature[num];
			}
			return internalTemperature.value;
		}
	}

	private const float TRANSITION_TO_DELAY = 1f;

	private const float TEMPERATURE_AVERAGING_RANGE = 6f;

	private const float MIN_SCALD_INTERVAL = 5f;

	private const float SCALDING_DAMAGE_AMOUNT = 10f;

	public State idle;

	public State transitionToScalding;

	public State transitionToScolding;

	public State scalding;

	public State scolding;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = idle;
		root.Enter(SetInitialAverageExternalTemperature).EventHandler(GameHashes.OnUnequip, OnSuitUnequipped).Update(AverageExternalTemperatureUpdate);
		idle.Transition(transitionToScalding, IsScalding).Transition(transitionToScolding, IsScolding);
		transitionToScalding.Transition(idle, GameStateMachine<ScaldingMonitor, Instance, IStateMachineTarget, Def>.Not(IsScalding)).Transition(scalding, IsScaldingTimed);
		transitionToScolding.Transition(idle, GameStateMachine<ScaldingMonitor, Instance, IStateMachineTarget, Def>.Not(IsScolding)).Transition(scolding, IsScoldingTimed);
		scalding.Transition(idle, CanEscapeScalding).ToggleExpression(Db.Get().Expressions.Hot).ToggleThought(Db.Get().Thoughts.Hot)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.Scalding, (Instance smi) => smi)
			.Update(TakeScaldDamage, UpdateRate.SIM_1000ms);
		scolding.Transition(idle, CanEscapeScolding).ToggleExpression(Db.Get().Expressions.Cold).ToggleThought(Db.Get().Thoughts.Cold)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.Scolding, (Instance smi) => smi)
			.Update(TakeColdDamage, UpdateRate.SIM_1000ms);
	}

	public static void OnSuitUnequipped(Instance smi, object obj)
	{
		if (obj != null && ((Equippable)obj).HasTag(GameTags.AirtightSuit))
		{
			smi.ResetExternalTemperatureAverage();
		}
	}

	public static void SetInitialAverageExternalTemperature(Instance smi)
	{
		smi.AverageExternalTemperature = smi.GetCurrentExternalTemperature();
	}

	public static bool CanEscapeScalding(Instance smi)
	{
		if (!smi.IsScalding())
		{
			return smi.timeinstate > 1f;
		}
		return false;
	}

	public static bool CanEscapeScolding(Instance smi)
	{
		if (!smi.IsScolding())
		{
			return smi.timeinstate > 1f;
		}
		return false;
	}

	public static bool IsScaldingTimed(Instance smi)
	{
		if (smi.IsScalding())
		{
			return smi.timeinstate > 1f;
		}
		return false;
	}

	public static bool IsScalding(Instance smi)
	{
		return smi.IsScalding();
	}

	public static bool IsScolding(Instance smi)
	{
		return smi.IsScolding();
	}

	public static bool IsScoldingTimed(Instance smi)
	{
		if (smi.IsScolding())
		{
			return smi.timeinstate > 1f;
		}
		return false;
	}

	public static void TakeScaldDamage(Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	public static void TakeColdDamage(Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	public static void AverageExternalTemperatureUpdate(Instance smi, float dt)
	{
		smi.AverageExternalTemperature *= Mathf.Max(0f, 1f - dt / 6f);
		smi.AverageExternalTemperature += smi.GetCurrentExternalTemperature() * (dt / 6f);
	}
}
