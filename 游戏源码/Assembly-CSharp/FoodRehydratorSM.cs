public class FoodRehydratorSM : GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>
{
	public class Def : BaseDef
	{
	}

	public class StatesInstance : GameInstance
	{
		[MyCmpReq]
		public Operational operational;

		public StatesInstance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}
	}

	private State off;

	private State on;

	private State active;

	private State postactive;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = root;
		root.EnterTransition(off, (StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(on, (StatesInstance smi) => smi.operational.IsFunctional);
		off.PlayAnim("off", KAnim.PlayMode.Loop).EnterTransition(on, (StatesInstance smi) => smi.operational.IsFunctional).EventTransition(GameHashes.FunctionalChanged, on, (StatesInstance smi) => smi.operational.IsFunctional);
		on.PlayAnim("on", KAnim.PlayMode.Loop).EnterTransition(off, (StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(active, (StatesInstance smi) => smi.operational.IsActive)
			.EventTransition(GameHashes.FunctionalChanged, off, (StatesInstance smi) => !smi.operational.IsFunctional)
			.EventTransition(GameHashes.ActiveChanged, active, (StatesInstance smi) => smi.operational.IsActive);
		active.EnterTransition(off, (StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(on, (StatesInstance smi) => !smi.operational.IsActive).EventTransition(GameHashes.FunctionalChanged, off, (StatesInstance smi) => !smi.operational.IsFunctional)
			.EventTransition(GameHashes.ActiveChanged, postactive, (StatesInstance smi) => !smi.operational.IsActive);
		postactive.OnAnimQueueComplete(on);
	}
}
