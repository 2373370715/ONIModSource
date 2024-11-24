using System;

// Token: 0x0200114A RID: 4426
public class CreatureSleepMonitor : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
	// Token: 0x06005A81 RID: 23169 RVA: 0x000DAFA2 File Offset: 0x000D91A2
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.SleepBehaviour, new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep), null);
	}

	// Token: 0x06005A82 RID: 23170 RVA: 0x000A7889 File Offset: 0x000A5A89
	public static bool ShouldSleep(CreatureSleepMonitor.Instance smi)
	{
		return GameClock.Instance.IsNighttime();
	}

	// Token: 0x0200114B RID: 4427
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200114C RID: 4428
	public new class Instance : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance
	{
		// Token: 0x06005A85 RID: 23173 RVA: 0x000DAFD2 File Offset: 0x000D91D2
		public Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
