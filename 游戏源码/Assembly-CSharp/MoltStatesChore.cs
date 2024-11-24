using System;

// Token: 0x020001E5 RID: 485
public class MoltStatesChore : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>
{
	// Token: 0x0600068D RID: 1677 RVA: 0x0015C210 File Offset: 0x0015A410
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.molting;
		this.molting.PlayAnim((MoltStatesChore.Instance smi) => smi.def.moltAnimName, KAnim.PlayMode.Once).ScheduleGoTo(5f, this.complete).OnAnimQueueComplete(this.complete);
		this.complete.BehaviourComplete(GameTags.Creatures.ReadyToMolt, false);
	}

	// Token: 0x040004C6 RID: 1222
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State molting;

	// Token: 0x040004C7 RID: 1223
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State complete;

	// Token: 0x020001E6 RID: 486
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040004C8 RID: 1224
		public string moltAnimName;
	}

	// Token: 0x020001E7 RID: 487
	public new class Instance : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.GameInstance
	{
		// Token: 0x06000690 RID: 1680 RVA: 0x000A9040 File Offset: 0x000A7240
		public Instance(Chore<MoltStatesChore.Instance> chore, MoltStatesChore.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ReadyToMolt);
		}
	}
}
