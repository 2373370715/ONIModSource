using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020019D4 RID: 6612
public class SuitSuffocationMonitor : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance>
{
	// Token: 0x060089C1 RID: 35265 RVA: 0x00358504 File Offset: 0x00356704
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuitSuffocationMonitor.Instance smi) => smi.breathing, null).Transition(this.nooxygen, (SuitSuffocationMonitor.Instance smi) => smi.IsTankEmpty(), UpdateRate.SIM_200ms);
		this.satisfied.normal.Transition(this.satisfied.low, (SuitSuffocationMonitor.Instance smi) => smi.suitTank.NeedsRecharging(), UpdateRate.SIM_200ms);
		this.satisfied.low.DoNothing();
		this.nooxygen.ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuitSuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.nooxygen.holdingbreath);
		this.nooxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.nooxygen.suffocating, (SuitSuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.nooxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuitSuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuitSuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x040067A5 RID: 26533
	public SuitSuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x040067A6 RID: 26534
	public SuitSuffocationMonitor.NoOxygenState nooxygen;

	// Token: 0x040067A7 RID: 26535
	public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State death;

	// Token: 0x020019D5 RID: 6613
	public class NoOxygenState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040067A8 RID: 26536
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;

		// Token: 0x040067A9 RID: 26537
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
	}

	// Token: 0x020019D6 RID: 6614
	public class SatisfiedState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040067AA RID: 26538
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x040067AB RID: 26539
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
	}

	// Token: 0x020019D7 RID: 6615
	public new class Instance : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x060089C5 RID: 35269 RVA: 0x000FA435 File Offset: 0x000F8635
		// (set) Token: 0x060089C6 RID: 35270 RVA: 0x000FA43D File Offset: 0x000F863D
		public SuitTank suitTank { get; private set; }

		// Token: 0x060089C7 RID: 35271 RVA: 0x00358720 File Offset: 0x00356920
		public Instance(IStateMachineTarget master, SuitTank suit_tank) : base(master)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.suitTank = suit_tank;
		}

		// Token: 0x060089C8 RID: 35272 RVA: 0x000FA446 File Offset: 0x000F8646
		public bool IsTankEmpty()
		{
			return this.suitTank.IsEmpty();
		}

		// Token: 0x060089C9 RID: 35273 RVA: 0x000FA453 File Offset: 0x000F8653
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x060089CA RID: 35274 RVA: 0x000FA46A File Offset: 0x000F866A
		public bool IsSuffocating()
		{
			return this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x060089CB RID: 35275 RVA: 0x000E28BF File Offset: 0x000E0ABF
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x040067AC RID: 26540
		private AmountInstance breath;

		// Token: 0x040067AD RID: 26541
		public AttributeModifier breathing;

		// Token: 0x040067AE RID: 26542
		public AttributeModifier holdingbreath;

		// Token: 0x040067AF RID: 26543
		private OxygenBreather masterOxygenBreather;
	}
}
