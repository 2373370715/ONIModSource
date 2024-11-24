using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02001365 RID: 4965
public class SuffocationMonitor : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance>
{
	// Token: 0x060065FD RID: 26109 RVA: 0x002CDC90 File Offset: 0x002CBE90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuffocationMonitor.Instance smi) => smi.breathing, null).EventTransition(GameHashes.ExitedBreathableArea, this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.IsInBreathableArea());
		this.satisfied.normal.Transition(this.satisfied.low, (SuffocationMonitor.Instance smi) => smi.oxygenBreather.IsLowOxygenAtMouthCell(), UpdateRate.SIM_200ms);
		this.satisfied.low.Transition(this.satisfied.normal, (SuffocationMonitor.Instance smi) => !smi.oxygenBreather.IsLowOxygenAtMouthCell(), UpdateRate.SIM_200ms).Transition(this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.IsInBreathableArea(), UpdateRate.SIM_200ms).ToggleEffect("LowOxygen");
		this.noOxygen.EventTransition(GameHashes.EnteredBreathableArea, this.satisfied, (SuffocationMonitor.Instance smi) => smi.IsInBreathableArea()).TagTransition(GameTags.RecoveringBreath, this.satisfied, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (SuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x04004C83 RID: 19587
	public SuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x04004C84 RID: 19588
	public SuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x04004C85 RID: 19589
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State death;

	// Token: 0x04004C86 RID: 19590
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x02001366 RID: 4966
	public class NoOxygenState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04004C87 RID: 19591
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;

		// Token: 0x04004C88 RID: 19592
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
	}

	// Token: 0x02001367 RID: 4967
	public class SatisfiedState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04004C89 RID: 19593
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x04004C8A RID: 19594
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
	}

	// Token: 0x02001368 RID: 4968
	public new class Instance : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06006601 RID: 26113 RVA: 0x000E285D File Offset: 0x000E0A5D
		// (set) Token: 0x06006602 RID: 26114 RVA: 0x000E2865 File Offset: 0x000E0A65
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06006603 RID: 26115 RVA: 0x002CDF74 File Offset: 0x002CC174
		public Instance(OxygenBreather oxygen_breather) : base(oxygen_breather)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(base.master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = oxygen_breather;
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x002CE018 File Offset: 0x002CC218
		public bool IsInBreathableArea()
		{
			return base.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || base.master.GetComponent<Sensors>().GetSensor<BreathableAreaSensor>().IsBreathable() || this.oxygenBreather.HasTag(GameTags.InTransitTube);
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x000E286E File Offset: 0x000E0A6E
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x000E2885 File Offset: 0x000E0A85
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x06006607 RID: 26119 RVA: 0x000E28BF File Offset: 0x000E0ABF
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x04004C8B RID: 19595
		private AmountInstance breath;

		// Token: 0x04004C8C RID: 19596
		public AttributeModifier breathing;

		// Token: 0x04004C8D RID: 19597
		public AttributeModifier holdingbreath;
	}
}
