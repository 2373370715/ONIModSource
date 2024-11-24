using System;
using Klei.AI;
using TUNING;

// Token: 0x0200156B RID: 5483
public class ExternalTemperatureMonitor : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance>
{
	// Token: 0x060071F7 RID: 29175 RVA: 0x000EA98D File Offset: 0x000E8B8D
	public static float GetExternalColdThreshold(Attributes affected_attributes)
	{
		return -0.039f;
	}

	// Token: 0x060071F8 RID: 29176 RVA: 0x000EA994 File Offset: 0x000E8B94
	public static float GetExternalWarmThreshold(Attributes affected_attributes)
	{
		return 0.008f;
	}

	// Token: 0x060071F9 RID: 29177 RVA: 0x002FBCF0 File Offset: 0x002F9EF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.comfortable.Transition(this.transitionToTooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).Transition(this.transitionToTooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms);
		this.transitionToTooWarm.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot(), UpdateRate.SIM_200ms).Transition(this.tooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.transitionToTooCool.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold(), UpdateRate.SIM_200ms).Transition(this.tooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.tooWarm.ToggleTag(GameTags.FeelingWarm).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooHot()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
		this.tooCool.ToggleTag(GameTags.FeelingCold).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooCold()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
	}

	// Token: 0x0400551A RID: 21786
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State comfortable;

	// Token: 0x0400551B RID: 21787
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooWarm;

	// Token: 0x0400551C RID: 21788
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooWarm;

	// Token: 0x0400551D RID: 21789
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooCool;

	// Token: 0x0400551E RID: 21790
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooCool;

	// Token: 0x0400551F RID: 21791
	private const float BODY_TEMPERATURE_AFFECT_EXTERNAL_FEEL_THRESHOLD = 0.5f;

	// Token: 0x04005520 RID: 21792
	public static readonly float BASE_STRESS_TOLERANCE_COLD = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS * 0.2f;

	// Token: 0x04005521 RID: 21793
	public static readonly float BASE_STRESS_TOLERANCE_WARM = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS * 0.2f;

	// Token: 0x04005522 RID: 21794
	private const float START_GAME_AVERAGING_DELAY = 6f;

	// Token: 0x04005523 RID: 21795
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x04005524 RID: 21796
	private const float TRANSITION_OUT_DELAY = 6f;

	// Token: 0x0200156C RID: 5484
	public new class Instance : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060071FC RID: 29180 RVA: 0x000EA9D9 File Offset: 0x000E8BD9
		public float GetCurrentColdThreshold
		{
			get
			{
				if (this.internalTemperatureMonitor.IdealTemperatureDelta() > 0.5f)
				{
					return 0f;
				}
				return CreatureSimTemperatureTransfer.PotentialEnergyFlowToCreature(Grid.PosToCell(base.gameObject), this.primaryElement, this.temperatureTransferer, 1f);
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060071FD RID: 29181 RVA: 0x000EAA14 File Offset: 0x000E8C14
		public float GetCurrentHotThreshold
		{
			get
			{
				return this.HotThreshold;
			}
		}

		// Token: 0x060071FE RID: 29182 RVA: 0x002FBF3C File Offset: 0x002FA13C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.attributes = base.gameObject.GetAttributes();
			this.internalTemperatureMonitor = base.gameObject.GetSMI<TemperatureMonitor.Instance>();
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.temperatureTransferer = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			this.effects = base.gameObject.GetComponent<Effects>();
			this.traits = base.gameObject.GetComponent<Traits>();
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x002FC050 File Offset: 0x002FA250
		public bool IsTooHot()
		{
			return !this.effects.HasEffect("RefreshingTouch") && !this.effects.HasImmunityTo(this.warmAirEffect) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage > ExternalTemperatureMonitor.GetExternalWarmThreshold(base.smi.attributes);
		}

		// Token: 0x06007200 RID: 29184 RVA: 0x002FC0C0 File Offset: 0x002FA2C0
		public bool IsTooCold()
		{
			for (int i = 0; i < this.immunityToColdEffects.Length; i++)
			{
				if (this.effects.HasEffect(this.immunityToColdEffects[i]))
				{
					return false;
				}
			}
			return !this.effects.HasImmunityTo(this.coldAirEffect) && (!(this.traits != null) || !this.traits.IsEffectIgnored(this.coldAirEffect)) && !WarmthProvider.IsWarmCell(Grid.PosToCell(this)) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage < ExternalTemperatureMonitor.GetExternalColdThreshold(base.smi.attributes);
		}

		// Token: 0x04005525 RID: 21797
		public float HotThreshold = 306.15f;

		// Token: 0x04005526 RID: 21798
		public Effects effects;

		// Token: 0x04005527 RID: 21799
		public Traits traits;

		// Token: 0x04005528 RID: 21800
		public Attributes attributes;

		// Token: 0x04005529 RID: 21801
		public AmountInstance internalTemperature;

		// Token: 0x0400552A RID: 21802
		private TemperatureMonitor.Instance internalTemperatureMonitor;

		// Token: 0x0400552B RID: 21803
		public CreatureSimTemperatureTransfer temperatureTransferer;

		// Token: 0x0400552C RID: 21804
		public PrimaryElement primaryElement;

		// Token: 0x0400552D RID: 21805
		private Effect warmAirEffect = Db.Get().effects.Get("WarmAir");

		// Token: 0x0400552E RID: 21806
		private Effect coldAirEffect = Db.Get().effects.Get("ColdAir");

		// Token: 0x0400552F RID: 21807
		private Effect[] immunityToColdEffects = new Effect[]
		{
			Db.Get().effects.Get("WarmTouch"),
			Db.Get().effects.Get("WarmTouchFood")
		};
	}
}
