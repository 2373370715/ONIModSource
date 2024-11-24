public class DebugGoToMonitor : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>
{
	public class Def : BaseDef
	{
	}

	public new class Instance : GameInstance
	{
		public int targetCellIndex = Grid.InvalidCell;

		public Instance(IStateMachineTarget target, Def def)
			: base(target, def)
		{
		}

		public void GoToCursor()
		{
			targetCellIndex = DebugHandler.GetMouseCell();
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		public void GoToCell(int cellIndex)
		{
			targetCellIndex = cellIndex;
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}
	}

	public State satisfied;

	public State hastarget;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = satisfied;
		satisfied.DoNothing();
		hastarget.ToggleChore((Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.DebugGoTo, (MoveChore.StatesInstance smii) => smi.targetCellIndex), satisfied);
	}
}
