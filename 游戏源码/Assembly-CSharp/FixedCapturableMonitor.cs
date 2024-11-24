public class FixedCapturableMonitor : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
	public class Def : BaseDef
	{
	}

	public new class Instance : GameInstance
	{
		public FixedCapturePoint.Instance targetCapturePoint;

		public ChoreConsumer ChoreConsumer;

		public Navigator Navigator;

		public Tag PrefabTag;

		public bool isBaby;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			ChoreConsumer = GetComponent<ChoreConsumer>();
			Navigator = GetComponent<Navigator>();
			PrefabTag = GetComponent<KPrefabID>().PrefabTag;
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			isBaby = def2 != null;
		}

		public bool ShouldGoGetCaptured()
		{
			if (targetCapturePoint != null && targetCapturePoint.IsRunning() && targetCapturePoint.shouldCreatureGoGetCaptured)
			{
				if (isBaby)
				{
					return targetCapturePoint.def.allowBabies;
				}
				return true;
			}
			return false;
		}
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = root;
		root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (Instance smi) => smi.ShouldGoGetCaptured()).Enter(delegate(Instance smi)
		{
			Components.FixedCapturableMonitors.Add(smi);
		}).Exit(delegate(Instance smi)
		{
			Components.FixedCapturableMonitors.Remove(smi);
		});
	}
}
