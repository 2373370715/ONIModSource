public class RedAlertMonitor : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance>
{
	public new class Instance : GameInstance
	{
		public Instance(IStateMachineTarget master)
			: base(master)
		{
		}

		public void EnableRedAlert()
		{
			ChoreDriver component = GetComponent<ChoreDriver>();
			if (!(component != null))
			{
				return;
			}
			Chore currentChore = component.GetCurrentChore();
			if (currentChore == null)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < currentChore.GetPreconditions().Count; i++)
			{
				if (currentChore.GetPreconditions()[i].id == ChorePreconditions.instance.IsNotRedAlert.id)
				{
					flag = true;
				}
			}
			if (flag)
			{
				component.StopChore();
			}
		}
	}

	public State off;

	public State on;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = off;
		base.serializable = SerializeType.Both_DEPRECATED;
		off.EventTransition(GameHashes.EnteredRedAlert, (Instance smi) => Game.Instance, on, delegate(Instance smi)
		{
			WorldContainer myWorld2 = smi.master.gameObject.GetMyWorld();
			return !(myWorld2 == null) && myWorld2.AlertManager.IsRedAlert();
		});
		on.EventTransition(GameHashes.ExitedRedAlert, (Instance smi) => Game.Instance, off, delegate(Instance smi)
		{
			WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
			return !(myWorld == null) && !myWorld.AlertManager.IsRedAlert();
		}).Enter("EnableRedAlert", delegate(Instance smi)
		{
			smi.EnableRedAlert();
		}).ToggleEffect("RedAlert")
			.ToggleExpression(Db.Get().Expressions.RedAlert);
	}
}
