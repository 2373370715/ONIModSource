public class Dreamer : GameStateMachine<Dreamer, Dreamer.Instance>
{
	public class DreamingState : State
	{
		public State hidden;

		public State visible;
	}

	public new class Instance : GameInstance
	{
		public Dream currentDream;

		public Instance(IStateMachineTarget master)
			: base(master)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this);
		}

		public void SetDream(Dream dream)
		{
			currentDream = dream;
		}

		public void StartDreaming()
		{
			base.sm.startDreaming.Trigger(base.smi);
		}

		public void StopDreaming()
		{
			SetDream(null);
			base.sm.stopDreaming.Trigger(base.smi);
		}
	}

	public Signal stopDreaming;

	public Signal startDreaming;

	public State notDreaming;

	public State dreaming;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = notDreaming;
		notDreaming.OnSignal(startDreaming, dreaming, (Instance smi) => smi.currentDream != null);
		dreaming.Enter(PrepareDream).OnSignal(stopDreaming, notDreaming).Update(UpdateDream, UpdateRate.SIM_EVERY_TICK)
			.Exit(RemoveDream);
	}

	private void RemoveDream(Instance smi)
	{
		smi.SetDream(null);
		NameDisplayScreen.Instance.StopDreaming(smi.gameObject);
	}

	private void UpdateDream(Instance smi, float dt)
	{
		NameDisplayScreen.Instance.DreamTick(smi.gameObject, dt);
	}

	private static void PrepareDream(Instance smi)
	{
		NameDisplayScreen.Instance.SetDream(smi.gameObject, smi.currentDream);
	}
}
