using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200164E RID: 5710
[SkipSaveFileSerialization]
public class PrefersColder : StateMachineComponent<PrefersColder.StatesInstance>
{
	// Token: 0x0600760E RID: 30222 RVA: 0x000ED8B5 File Offset: 0x000EBAB5
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0200164F RID: 5711
	public class StatesInstance : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder, object>.GameInstance
	{
		// Token: 0x06007610 RID: 30224 RVA: 0x000ED8CA File Offset: 0x000EBACA
		public StatesInstance(PrefersColder master) : base(master)
		{
		}
	}

	// Token: 0x02001650 RID: 5712
	public class States : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder>
	{
		// Token: 0x06007611 RID: 30225 RVA: 0x000ED8D3 File Offset: 0x000EBAD3
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (PrefersColder.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x0400587F RID: 22655
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.PUDGY, DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, false, false, true);
	}
}
