using System;

// Token: 0x02001547 RID: 5447
public class CreatureDebugGoToMonitor : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>
{
	// Token: 0x0600717E RID: 29054 RVA: 0x000EA479 File Offset: 0x000E8679
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.HasDebugDestination, new StateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.Transition.ConditionCallback(CreatureDebugGoToMonitor.HasTargetCell), new Action<CreatureDebugGoToMonitor.Instance>(CreatureDebugGoToMonitor.ClearTargetCell));
	}

	// Token: 0x0600717F RID: 29055 RVA: 0x000EA4AC File Offset: 0x000E86AC
	private static bool HasTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		return smi.targetCell != Grid.InvalidCell;
	}

	// Token: 0x06007180 RID: 29056 RVA: 0x000EA4BE File Offset: 0x000E86BE
	private static void ClearTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		smi.targetCell = Grid.InvalidCell;
	}

	// Token: 0x02001548 RID: 5448
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001549 RID: 5449
	public new class Instance : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x06007183 RID: 29059 RVA: 0x000EA4D3 File Offset: 0x000E86D3
		public Instance(IStateMachineTarget target, CreatureDebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x06007184 RID: 29060 RVA: 0x000EA4E8 File Offset: 0x000E86E8
		public void GoToCursor()
		{
			this.targetCell = DebugHandler.GetMouseCell();
		}

		// Token: 0x06007185 RID: 29061 RVA: 0x000EA4F5 File Offset: 0x000E86F5
		public void GoToCell(int cellIndex)
		{
			this.targetCell = cellIndex;
		}

		// Token: 0x040054C3 RID: 21699
		public int targetCell = Grid.InvalidCell;
	}
}
