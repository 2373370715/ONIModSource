using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02001577 RID: 5495
public class GasLiquidExposureMonitor : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>
{
	// Token: 0x06007236 RID: 29238 RVA: 0x002FC878 File Offset: 0x002FAA78
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

	// Token: 0x06007237 RID: 29239 RVA: 0x000EAD15 File Offset: 0x000E8F15
	private static bool CanReact(GasLiquidExposureMonitor.Instance smi)
	{
		return GameClock.Instance.GetTime() > smi.lastReactTime + 60f;
	}

	// Token: 0x06007238 RID: 29240 RVA: 0x002FC9C8 File Offset: 0x002FABC8
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

	// Token: 0x06007239 RID: 29241 RVA: 0x002FCB8C File Offset: 0x002FAD8C
	public float GetCurrentExposure(GasLiquidExposureMonitor.Instance smi)
	{
		float result;
		if (GasLiquidExposureMonitor.customExposureRates.TryGetValue(smi.CurrentlyExposedToElement().id, out result))
		{
			return result;
		}
		return 0f;
	}

	// Token: 0x0600723A RID: 29242 RVA: 0x002FCBBC File Offset: 0x002FADBC
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

	// Token: 0x0600723B RID: 29243 RVA: 0x002FCD6C File Offset: 0x002FAF6C
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

	// Token: 0x0600723C RID: 29244 RVA: 0x000EAD2F File Offset: 0x000E8F2F
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

	// Token: 0x04005553 RID: 21843
	public const float MIN_REACT_INTERVAL = 60f;

	// Token: 0x04005554 RID: 21844
	private static Dictionary<SimHashes, float> customExposureRates;

	// Token: 0x04005555 RID: 21845
	private static Effect minorIrritationEffect;

	// Token: 0x04005556 RID: 21846
	private static Effect majorIrritationEffect;

	// Token: 0x04005557 RID: 21847
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.BoolParameter isIrritated;

	// Token: 0x04005558 RID: 21848
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Signal reactFinished;

	// Token: 0x04005559 RID: 21849
	public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State normal;

	// Token: 0x0400555A RID: 21850
	public GasLiquidExposureMonitor.IrritatedStates irritated;

	// Token: 0x02001578 RID: 5496
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001579 RID: 5497
	public class TUNING
	{
		// Token: 0x0400555B RID: 21851
		public const float MINOR_IRRITATION_THRESHOLD = 8f;

		// Token: 0x0400555C RID: 21852
		public const float MAJOR_IRRITATION_THRESHOLD = 15f;

		// Token: 0x0400555D RID: 21853
		public const float MAX_EXPOSURE = 30f;

		// Token: 0x0400555E RID: 21854
		public const float GAS_UNITS = 1f;

		// Token: 0x0400555F RID: 21855
		public const float LIQUID_UNITS = 1000f;

		// Token: 0x04005560 RID: 21856
		public const float REDUCE_EXPOSURE_RATE_FAST = -1f;

		// Token: 0x04005561 RID: 21857
		public const float REDUCE_EXPOSURE_RATE_SLOW = -0.25f;

		// Token: 0x04005562 RID: 21858
		public const float NO_CHANGE = 0f;

		// Token: 0x04005563 RID: 21859
		public const float SLOW_EXPOSURE_RATE = 0.5f;

		// Token: 0x04005564 RID: 21860
		public const float NORMAL_EXPOSURE_RATE = 1f;

		// Token: 0x04005565 RID: 21861
		public const float QUICK_EXPOSURE_RATE = 3f;

		// Token: 0x04005566 RID: 21862
		public const float DEFAULT_MIN_TEMPERATURE = -13657.5f;

		// Token: 0x04005567 RID: 21863
		public const float DEFAULT_MAX_TEMPERATURE = 27315f;

		// Token: 0x04005568 RID: 21864
		public const float DEFAULT_LOW_RATE = 1f;

		// Token: 0x04005569 RID: 21865
		public const float DEFAULT_HIGH_RATE = 2f;
	}

	// Token: 0x0200157A RID: 5498
	public class IrritatedStates : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State
	{
		// Token: 0x0400556A RID: 21866
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State irritated;

		// Token: 0x0400556B RID: 21867
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State rubbingEyes;
	}

	// Token: 0x0200157B RID: 5499
	public new class Instance : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.GameInstance
	{
		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06007243 RID: 29251 RVA: 0x000EAD7D File Offset: 0x000E8F7D
		public float minorIrritationThreshold
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06007244 RID: 29252 RVA: 0x000EAD84 File Offset: 0x000E8F84
		public Instance(IStateMachineTarget master, GasLiquidExposureMonitor.Def def) : base(master, def)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x06007245 RID: 29253 RVA: 0x002FCE00 File Offset: 0x002FB000
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

		// Token: 0x06007246 RID: 29254 RVA: 0x000EAD9A File Offset: 0x000E8F9A
		public bool IsMinorIrritation()
		{
			return this.exposure >= 8f && this.exposure < 15f;
		}

		// Token: 0x06007247 RID: 29255 RVA: 0x000EADB8 File Offset: 0x000E8FB8
		public bool IsMajorIrritation()
		{
			return this.exposure >= 15f;
		}

		// Token: 0x06007248 RID: 29256 RVA: 0x002FCE8C File Offset: 0x002FB08C
		public Element CurrentlyExposedToElement()
		{
			if (this.isInAirtightEnvironment)
			{
				return ElementLoader.GetElement(SimHashes.Oxygen.CreateTag());
			}
			int num = Grid.CellAbove(Grid.PosToCell(base.smi.gameObject));
			return Grid.Element[num];
		}

		// Token: 0x06007249 RID: 29257 RVA: 0x000EADCA File Offset: 0x000E8FCA
		public void ResetExposure()
		{
			this.exposure = 0f;
		}

		// Token: 0x0400556C RID: 21868
		[Serialize]
		public float exposure;

		// Token: 0x0400556D RID: 21869
		[Serialize]
		public float lastReactTime;

		// Token: 0x0400556E RID: 21870
		[Serialize]
		public float exposureRate;

		// Token: 0x0400556F RID: 21871
		public Effects effects;

		// Token: 0x04005570 RID: 21872
		public bool isInAirtightEnvironment;

		// Token: 0x04005571 RID: 21873
		public bool isImmuneToIrritability;
	}
}
