using System;
using KSerialization;

// Token: 0x020011D2 RID: 4562
public class RoverChoreMonitor : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>
{
	// Token: 0x06005CEE RID: 23790 RVA: 0x0029CBD8 File Offset: 0x0029ADD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.ToggleBehaviour(GameTags.Creatures.Tunnel, (RoverChoreMonitor.Instance smi) => true, null).ToggleBehaviour(GameTags.Creatures.Builder, (RoverChoreMonitor.Instance smi) => true, null);
	}

	// Token: 0x040041BF RID: 16831
	public GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.State loop;

	// Token: 0x020011D3 RID: 4563
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020011D4 RID: 4564
	public new class Instance : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.GameInstance
	{
		// Token: 0x06005CF1 RID: 23793 RVA: 0x000DCAF9 File Offset: 0x000DACF9
		public Instance(IStateMachineTarget master, RoverChoreMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x000DCB0A File Offset: 0x000DAD0A
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x040041C0 RID: 16832
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x040041C1 RID: 16833
		private Action<object> OnDestinationReachedDelegate;
	}
}
