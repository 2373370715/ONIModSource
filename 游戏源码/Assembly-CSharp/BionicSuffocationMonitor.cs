using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02001511 RID: 5393
public class BionicSuffocationMonitor : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>
{
	// Token: 0x0600707E RID: 28798 RVA: 0x002F82C8 File Offset: 0x002F64C8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.normal.ToggleAttributeModifier("Breathing", (BionicSuffocationMonitor.Instance smi) => smi.breathing, null).EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.noOxygen, (BionicSuffocationMonitor.Instance smi) => !smi.IsBreathing());
		this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.normal, (BionicSuffocationMonitor.Instance smi) => smi.IsBreathing()).TagTransition(GameTags.RecoveringBreath, this.normal, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (BionicSuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (BionicSuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (BionicSuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(BionicSuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x04005414 RID: 21524
	public BionicSuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x04005415 RID: 21525
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State normal;

	// Token: 0x04005416 RID: 21526
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State death;

	// Token: 0x04005417 RID: 21527
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State dead;

	// Token: 0x02001512 RID: 5394
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001513 RID: 5395
	public class NoOxygenState : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State
	{
		// Token: 0x04005418 RID: 21528
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State holdingbreath;

		// Token: 0x04005419 RID: 21529
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State suffocating;
	}

	// Token: 0x02001514 RID: 5396
	public new class Instance : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.GameInstance
	{
		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06007082 RID: 28802 RVA: 0x000E9966 File Offset: 0x000E7B66
		// (set) Token: 0x06007083 RID: 28803 RVA: 0x000E996E File Offset: 0x000E7B6E
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06007084 RID: 28804 RVA: 0x002F84F4 File Offset: 0x002F66F4
		public Instance(IStateMachineTarget master, BionicSuffocationMonitor.Def def) : base(master, def)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x06007085 RID: 28805 RVA: 0x000E9977 File Offset: 0x000E7B77
		public bool IsBreathing()
		{
			return !this.oxygenBreather.IsSuffocating || base.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.HasTag(GameTags.InTransitTube);
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x000E99AF File Offset: 0x000E7BAF
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x000E99C6 File Offset: 0x000E7BC6
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x06007088 RID: 28808 RVA: 0x000E28BF File Offset: 0x000E0ABF
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x0400541A RID: 21530
		private AmountInstance breath;

		// Token: 0x0400541B RID: 21531
		public AttributeModifier breathing;

		// Token: 0x0400541C RID: 21532
		public AttributeModifier holdingbreath;
	}
}
