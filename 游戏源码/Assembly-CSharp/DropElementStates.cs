using System;
using STRINGS;

// Token: 0x02000179 RID: 377
public class DropElementStates : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>
{
	// Token: 0x0600056B RID: 1387 RVA: 0x00159480 File Offset: 0x00157680
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.dropping;
		GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.EXPELLING_GAS.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.dropping.PlayAnim("dirty").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.Enter("DropElement", delegate(DropElementStates.Instance smi)
		{
			smi.GetSMI<ElementDropperMonitor.Instance>().DropPeriodicElement();
		}).QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.WantsToDropElements, false);
	}

	// Token: 0x040003F2 RID: 1010
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State dropping;

	// Token: 0x040003F3 RID: 1011
	public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State behaviourcomplete;

	// Token: 0x0200017A RID: 378
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200017B RID: 379
	public new class Instance : GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.GameInstance
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x000A8346 File Offset: 0x000A6546
		public Instance(Chore<DropElementStates.Instance> chore, DropElementStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToDropElements);
		}
	}
}
