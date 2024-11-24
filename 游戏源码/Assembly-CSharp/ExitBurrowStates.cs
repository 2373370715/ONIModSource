using System;
using STRINGS;

// Token: 0x02000187 RID: 391
public class ExitBurrowStates : GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>
{
	// Token: 0x0600058E RID: 1422 RVA: 0x001599E4 File Offset: 0x00157BE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.exiting;
		GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.EMERGING.NAME;
		string tooltip = CREATURES.STATUSITEMS.EMERGING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.exiting.PlayAnim("emerge").Enter(new StateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State.Callback(ExitBurrowStates.MoveToCellAbove)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToExitBurrow, false);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x000A84D1 File Offset: 0x000A66D1
	private static void MoveToCellAbove(ExitBurrowStates.Instance smi)
	{
		smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellAbove(Grid.PosToCell(smi.transform.GetPosition())), Grid.SceneLayer.Creatures));
	}

	// Token: 0x0400040B RID: 1035
	private GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State exiting;

	// Token: 0x0400040C RID: 1036
	private GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State behaviourcomplete;

	// Token: 0x02000188 RID: 392
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000189 RID: 393
	public new class Instance : GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.GameInstance
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x000A8503 File Offset: 0x000A6703
		public Instance(Chore<ExitBurrowStates.Instance> chore, ExitBurrowStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToExitBurrow);
		}
	}
}
