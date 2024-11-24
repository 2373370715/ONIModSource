using System;

// Token: 0x02001123 RID: 4387
public class AnimInterruptMonitor : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
	// Token: 0x060059E6 RID: 23014 RVA: 0x000DA89E File Offset: 0x000D8A9E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.PlayInterruptAnim, new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim), new Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim));
	}

	// Token: 0x060059E7 RID: 23015 RVA: 0x000DA8D1 File Offset: 0x000D8AD1
	private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi)
	{
		return smi.anims != null;
	}

	// Token: 0x060059E8 RID: 23016 RVA: 0x000DA8DC File Offset: 0x000D8ADC
	private static void ClearAnim(AnimInterruptMonitor.Instance smi)
	{
		smi.anims = null;
	}

	// Token: 0x02001124 RID: 4388
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001125 RID: 4389
	public new class Instance : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance
	{
		// Token: 0x060059EB RID: 23019 RVA: 0x000DA8ED File Offset: 0x000D8AED
		public Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x000DA8F7 File Offset: 0x000D8AF7
		public void PlayAnim(HashedString anim)
		{
			this.PlayAnimSequence(new HashedString[]
			{
				anim
			});
		}

		// Token: 0x060059ED RID: 23021 RVA: 0x000DA90D File Offset: 0x000D8B0D
		public void PlayAnimSequence(HashedString[] anims)
		{
			this.anims = anims;
			base.GetComponent<CreatureBrain>().UpdateBrain();
		}

		// Token: 0x04003F71 RID: 16241
		public HashedString[] anims;
	}
}
