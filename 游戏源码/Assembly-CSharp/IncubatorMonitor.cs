using System;

// Token: 0x02000A1C RID: 2588
public class IncubatorMonitor : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>
{
	// Token: 0x06002F56 RID: 12118 RVA: 0x001F7C74 File Offset: 0x001F5E74
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.not;
		this.not.EventTransition(GameHashes.OnStore, this.in_incubator, new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator));
		this.in_incubator.ToggleTag(GameTags.Creatures.InIncubator).EventTransition(GameHashes.OnStore, this.not, GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Not(new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator)));
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x000BEBC0 File Offset: 0x000BCDC0
	public static bool InIncubator(IncubatorMonitor.Instance smi)
	{
		return smi.gameObject.transform.parent && smi.gameObject.transform.parent.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x04001FFD RID: 8189
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not;

	// Token: 0x04001FFE RID: 8190
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State in_incubator;

	// Token: 0x02000A1D RID: 2589
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A1E RID: 2590
	public new class Instance : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.GameInstance
	{
		// Token: 0x06002F5A RID: 12122 RVA: 0x000BEBFE File Offset: 0x000BCDFE
		public Instance(IStateMachineTarget master, IncubatorMonitor.Def def) : base(master, def)
		{
		}
	}
}
