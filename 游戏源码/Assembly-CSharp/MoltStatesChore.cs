public class MoltStatesChore : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>
{
	public class Def : BaseDef
	{
		public string moltAnimName;
	}

	public new class Instance : GameInstance
	{
		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ReadyToMolt);
		}
	}

	public State molting;

	public State complete;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = molting;
		molting.PlayAnim((Instance smi) => smi.def.moltAnimName).ScheduleGoTo(5f, complete).OnAnimQueueComplete(complete);
		complete.BehaviourComplete(GameTags.Creatures.ReadyToMolt);
	}
}
