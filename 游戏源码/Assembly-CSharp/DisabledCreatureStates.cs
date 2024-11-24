using System;
using STRINGS;

// Token: 0x0200016F RID: 367
public class DisabledCreatureStates : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>
{
	// Token: 0x06000545 RID: 1349 RVA: 0x00158DCC File Offset: 0x00156FCC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.disableCreature;
		GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.DISABLED.NAME;
		string tooltip = CREATURES.STATUSITEMS.DISABLED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TagTransition(GameTags.Creatures.Behaviours.DisableCreature, this.behaviourcomplete, true);
		this.disableCreature.PlayAnim((DisabledCreatureStates.Instance smi) => smi.def.disabledAnim, KAnim.PlayMode.Once);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.DisableCreature, false);
	}

	// Token: 0x040003DE RID: 990
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State disableCreature;

	// Token: 0x040003DF RID: 991
	public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State behaviourcomplete;

	// Token: 0x02000170 RID: 368
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06000547 RID: 1351 RVA: 0x000A81C0 File Offset: 0x000A63C0
		public Def(string anim)
		{
			this.disabledAnim = anim;
		}

		// Token: 0x040003E0 RID: 992
		public string disabledAnim = "off";
	}

	// Token: 0x02000171 RID: 369
	public new class Instance : GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.GameInstance
	{
		// Token: 0x06000548 RID: 1352 RVA: 0x000A81DA File Offset: 0x000A63DA
		public Instance(Chore<DisabledCreatureStates.Instance> chore, DisabledCreatureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Behaviours.DisableCreature);
		}
	}
}
