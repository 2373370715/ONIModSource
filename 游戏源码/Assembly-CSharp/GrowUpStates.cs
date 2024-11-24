using System;
using STRINGS;

// Token: 0x0200019D RID: 413
public class GrowUpStates : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
	// Token: 0x060005C5 RID: 1477 RVA: 0x0015A3E8 File Offset: 0x001585E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grow_up_pre;
		GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.grow_up_pre.Enter(delegate(GrowUpStates.Instance smi)
		{
			smi.PlayPreGrowAnimation();
		}).OnAnimQueueComplete(this.spawn_adult).ScheduleGoTo(4f, this.spawn_adult);
		this.spawn_adult.Enter(new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult));
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x000A87D5 File Offset: 0x000A69D5
	private static void SpawnAdult(GrowUpStates.Instance smi)
	{
		smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();
	}

	// Token: 0x04000435 RID: 1077
	public const float GROW_PRE_TIMEOUT = 4f;

	// Token: 0x04000436 RID: 1078
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;

	// Token: 0x04000437 RID: 1079
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

	// Token: 0x0200019E RID: 414
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200019F RID: 415
	public new class Instance : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
	{
		// Token: 0x060005C9 RID: 1481 RVA: 0x000A87EA File Offset: 0x000A69EA
		public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0015A4A4 File Offset: 0x001586A4
		public void PlayPreGrowAnimation()
		{
			if (base.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("growup_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
