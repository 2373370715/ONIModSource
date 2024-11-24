using System;

// Token: 0x02001552 RID: 5458
public class DebugGoToMonitor : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>
{
	// Token: 0x060071A8 RID: 29096 RVA: 0x002FB004 File Offset: 0x002F9204
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.hastarget.ToggleChore((DebugGoToMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.DebugGoTo, (MoveChore.StatesInstance smii) => smi.targetCellIndex, false), this.satisfied);
	}

	// Token: 0x040054DC RID: 21724
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State satisfied;

	// Token: 0x040054DD RID: 21725
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State hastarget;

	// Token: 0x02001553 RID: 5459
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001554 RID: 5460
	public new class Instance : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x060071AB RID: 29099 RVA: 0x000EA6AA File Offset: 0x000E88AA
		public Instance(IStateMachineTarget target, DebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x060071AC RID: 29100 RVA: 0x002FB058 File Offset: 0x002F9258
		public void GoToCursor()
		{
			this.targetCellIndex = DebugHandler.GetMouseCell();
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x060071AD RID: 29101 RVA: 0x002FB0A8 File Offset: 0x002F92A8
		public void GoToCell(int cellIndex)
		{
			this.targetCellIndex = cellIndex;
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x040054DE RID: 21726
		public int targetCellIndex = Grid.InvalidCell;
	}
}
