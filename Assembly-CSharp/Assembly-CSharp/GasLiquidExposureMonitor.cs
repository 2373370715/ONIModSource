using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

public class GasLiquidExposureMonitor : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		this.root.Update(new Action<GasLiquidExposureMonitor.Instance, float>(this.UpdateExposure), UpdateRate.SIM_33ms, false);
		this.normal.ParamTransition<bool>(this.isIrritated, this.irritated, (GasLiquidExposureMonitor.Instance smi, bool p) => this.isIrritated.Get(smi));
		this.irritated.ParamTransition<bool>(this.isIrritated, this.normal, (GasLiquidExposureMonitor.Instance smi, bool p) => !this.isIrritated.Get(smi)).ToggleStatusItem(Db.Get().DuplicantStatusItems.GasLiquidIrritation, (GasLiquidExposureMonitor.Instance smi) => smi).DefaultState(this.irritated.irritated);
		this.irritated.irritated.Transition(this.irritated.rubbingEyes, new StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Transition.ConditionCallback(GasLiquidExposureMonitor.CanReact), UpdateRate.SIM_200ms);
		this.irritated.rubbingEyes.Exit(delegate(GasLiquidExposureMonitor.Instance smi)
		{
			smi.lastReactTime = GameClock.Instance.GetTime();
		}).ToggleReactable((GasLiquidExposureMonitor.Instance smi) => smi.GetReactable()).OnSignal(this.reactFinished, this.irritated.irritated);
	}

		private static bool CanReact(GasLiquidExposureMonitor.Instance smi)
	{
		return GameClock.Instance.GetTime() > smi.lastReactTime + 60f;
	}

		private static void InitializeCustomRates()
	{
		if (GasLiquidExposureMonitor.customExposureRates != null)
		{
			return;
		}
		GasLiquidExposureMonitor.minorIrritationEffect = Db.Get().effects.Get("MinorIrritation");
		GasLiquidExposureMonitor.majorIrritationEffect = Db.Get().effects.Get("MajorIrritation");
		GasLiquidExposureMonitor.customExposureRates = new Dictionary<SimHashes, float>();
		float value = -1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Water] = value;
		float value2 = -0.25f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CarbonDioxide] = value2;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen] = value2;
		float value3 = 0f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ContaminatedOxygen] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.DirtyWater] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ViscoGel] = value3;
		float value4 = 0.5f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Hydrogen] = value4;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SaltWater] = value4;
		float value5 = 1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ChlorineGas] = value5;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.EthanolGas] = value5;
		float value6 = 3f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Chlorine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SourGas] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Brine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Ethanol] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SuperCoolant] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CrudeOil] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Naphtha] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Petroleum] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Mercury] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.MercuryGas] = value6;
	}

		public float GetCurrentExposure(GasLiquidExposureMonitor.Instance smi)
	{
		float result;
		if (GasLiquidExposureMonitor.customExposureRates.TryGetValue(smi.CurrentlyExposedToElement().id, out result))
		{
			return result;
		}
		return 0f;
	}

		private void UpdateExposure(GasLiquidExposureMonitor.Instance smi, float dt)
	{
		GasLiquidExposureMonitor.InitializeCustomRates();
		float exposureRate = 0f;
		smi.isInAirtightEnvironment = false;
		smi.isImmuneToIrritability = false;
		int num = Grid.CellAbove(Grid.PosToCell(smi.gameObject));
		if (Grid.IsValidCell(num))
		{
			Element element = Grid.Element[num];
			float num2;
			if (!GasLiquidExposureMonitor.customExposureRates.TryGetValue(element.id, out num2))
			{
				if (Grid.Temperature[num] >= -13657.5f && Grid.Temperature[num] <= 27315f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = 2f;
				}
			}
			if (smi.effects.HasImmunityTo(GasLiquidExposureMonitor.minorIrritationEffect) || smi.effects.HasImmunityTo(GasLiquidExposureMonitor.majorIrritationEffect))
			{
				smi.isImmuneToIrritability = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if ((smi.master.gameObject.HasTag(GameTags.HasSuitTank) && smi.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit()) || smi.master.gameObject.HasTag(GameTags.InTransitTube))
			{
				smi.isInAirtightEnvironment = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if (!smi.isInAirtightEnvironment && !smi.isImmuneToIrritability)
			{
				if (element.IsGas)
				{
					exposureRate = num2 * Grid.Mass[num] / 1f;
				}
				else if (element.IsLiquid)
				{
					exposureRate = num2 * Grid.Mass[num] / 1000f;
				}
			}
		}
		smi.exposureRate = exposureRate;
		smi.exposure += smi.exposureRate * dt;
		smi.exposure = MathUtil.Clamp(0f, 30f, smi.exposure);
		this.ApplyEffects(smi);
	}

		private void ApplyEffects(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.minorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else if (smi.IsMajorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.majorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else
		{
			smi.effects.Remove(GasLiquidExposureMonitor.minorIrritationEffect);
			smi.effects.Remove(GasLiquidExposureMonitor.majorIrritationEffect);
			this.isIrritated.Set(false, smi, false);
		}
	}

		public Effect GetAppliedEffect(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			return GasLiquidExposureMonitor.minorIrritationEffect;
		}
		if (smi.IsMajorIrritation())
		{
			return GasLiquidExposureMonitor.majorIrritationEffect;
		}
		return null;
	}

		public const float MIN_REACT_INTERVAL = 60f;

		private static Dictionary<SimHashes, float> customExposureRates;

		private static Effect minorIrritationEffect;

		private static Effect majorIrritationEffect;

		public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.BoolParameter isIrritated;

		public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Signal reactFinished;

		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State normal;

		public GasLiquidExposureMonitor.IrritatedStates irritated;

		public class Def : StateMachine.BaseDef
	{
	}

		public class TUNING
	{
				public const float MINOR_IRRITATION_THRESHOLD = 8f;

				public const float MAJOR_IRRITATION_THRESHOLD = 15f;

				public const float MAX_EXPOSURE = 30f;

				public const float GAS_UNITS = 1f;

				public const float LIQUID_UNITS = 1000f;

				public const float REDUCE_EXPOSURE_RATE_FAST = -1f;

				public const float REDUCE_EXPOSURE_RATE_SLOW = -0.25f;

				public const float NO_CHANGE = 0f;

				public const float SLOW_EXPOSURE_RATE = 0.5f;

				public const float NORMAL_EXPOSURE_RATE = 1f;

				public const float QUICK_EXPOSURE_RATE = 3f;

				public const float DEFAULT_MIN_TEMPERATURE = -13657.5f;

				public const float DEFAULT_MAX_TEMPERATURE = 27315f;

				public const float DEFAULT_LOW_RATE = 1f;

				public const float DEFAULT_HIGH_RATE = 2f;
	}

		public class IrritatedStates : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State
	{
				public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State irritated;

				public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State rubbingEyes;
	}

		public new class Instance : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.GameInstance
	{
						public float minorIrritationThreshold
		{
			get
			{
				return 8f;
			}
		}

				public Instance(IStateMachineTarget master, GasLiquidExposureMonitor.Def def) : base(master, def)
		{
			this.effects = master.GetComponent<Effects>();
		}

				public Reactable GetReactable()
		{
			Emote iritatedEyes = Db.Get().Emotes.Minion.IritatedEyes;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "IrritatedEyes", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(iritatedEyes);
			selfEmoteReactable.preventChoreInterruption = true;
			selfEmoteReactable.RegisterEmoteStepCallbacks("irritated_eyes", null, delegate(GameObject go)
			{
				base.sm.reactFinished.Trigger(this);
			});
			return selfEmoteReactable;
		}

				public bool IsMinorIrritation()
		{
			return this.exposure >= 8f && this.exposure < 15f;
		}

				public bool IsMajorIrritation()
		{
			return this.exposure >= 15f;
		}

				public Element CurrentlyExposedToElement()
		{
			if (this.isInAirtightEnvironment)
			{
				return ElementLoader.GetElement(SimHashes.Oxygen.CreateTag());
			}
			int num = Grid.CellAbove(Grid.PosToCell(base.smi.gameObject));
			return Grid.Element[num];
		}

				public void ResetExposure()
		{
			this.exposure = 0f;
		}

				[Serialize]
		public float exposure;

				[Serialize]
		public float lastReactTime;

				[Serialize]
		public float exposureRate;

				public Effects effects;

				public bool isInAirtightEnvironment;

				public bool isImmuneToIrritability;
	}
}
