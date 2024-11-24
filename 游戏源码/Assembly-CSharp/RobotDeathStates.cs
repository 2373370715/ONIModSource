using STRINGS;

public class RobotDeathStates : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>
{
	public class Def : BaseDef
	{
	}

	public new class Instance : GameInstance
	{
		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
			chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}
	}

	private State loop;

	private State pst;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = loop;
		loop.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).PlayAnim("death").OnAnimQueueComplete(pst);
		pst.TriggerOnEnter(GameHashes.DeathAnimComplete).TriggerOnEnter(GameHashes.Died, (Instance smi) => smi.gameObject).BehaviourComplete(GameTags.Creatures.Die);
	}
}
