using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02001651 RID: 5713
[SkipSaveFileSerialization]
public class PrefersWarmer : StateMachineComponent<PrefersWarmer.StatesInstance>
{
	// Token: 0x06007614 RID: 30228 RVA: 0x000ED941 File Offset: 0x000EBB41
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001652 RID: 5714
	public class StatesInstance : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer, object>.GameInstance
	{
		// Token: 0x06007616 RID: 30230 RVA: 0x000ED956 File Offset: 0x000EBB56
		public StatesInstance(PrefersWarmer master) : base(master)
		{
		}
	}

	// Token: 0x02001653 RID: 5715
	public class States : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer>
	{
		// Token: 0x06007617 RID: 30231 RVA: 0x000ED95F File Offset: 0x000EBB5F
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (PrefersWarmer.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04005880 RID: 22656
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.SKINNY, DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, false, false, true);
	}
}
