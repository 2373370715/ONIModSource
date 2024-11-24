using System;

// Token: 0x02000A2B RID: 2603
public class WorldSpawnableMonitor : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>
{
	// Token: 0x06002F8E RID: 12174 RVA: 0x000BEEA2 File Offset: 0x000BD0A2
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x02000A2C RID: 2604
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002022 RID: 8226
		public Func<int, int> adjustSpawnLocationCb;
	}

	// Token: 0x02000A2D RID: 2605
	public new class Instance : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>.GameInstance
	{
		// Token: 0x06002F91 RID: 12177 RVA: 0x000BEEB4 File Offset: 0x000BD0B4
		public Instance(IStateMachineTarget master, WorldSpawnableMonitor.Def def) : base(master, def)
		{
		}
	}
}
