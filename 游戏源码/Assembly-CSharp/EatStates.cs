using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class EatStates : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>
{
	// Token: 0x0600057C RID: 1404 RVA: 0x00159674 File Offset: 0x00157874
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetTarget)).Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetOffset)).Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.ReserveEdible)).Exit(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.UnreserveEdible));
		GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state = this.goingtoeat.MoveTo(new Func<EatStates.Instance, int>(EatStates.GetEdibleCell), this.eating, null, false);
		string name = CREATURES.STATUSITEMS.HUNGRY.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUNGRY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state2 = this.eating.Face(this.target, 0f).DefaultState(this.eating.pre);
		string name2 = CREATURES.STATUSITEMS.EATING.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.EATING.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.eating.pre.QueueAnim((EatStates.Instance smi) => smi.eatAnims[0], false, null).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.EatComplete)).QueueAnim((EatStates.Instance smi) => smi.eatAnims[1], true, null).ScheduleGoTo(3f, this.eating.pst);
		this.eating.pst.QueueAnim((EatStates.Instance smi) => smi.eatAnims[2], false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x000A83EB File Offset: 0x000A65EB
	private static void SetTarget(EatStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<SolidConsumerMonitor.Instance>().targetEdible, smi, false);
		smi.OverrideEatAnims(smi, smi.GetSMI<SolidConsumerMonitor.Instance>().GetTargetEdibleEatAnims());
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x000A841D File Offset: 0x000A661D
	private static void SetOffset(EatStates.Instance smi)
	{
		smi.sm.offset.Set(smi.GetSMI<SolidConsumerMonitor.Instance>().targetEdibleOffset, smi, false);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00159894 File Offset: 0x00157A94
	private static void ReserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x001598DC File Offset: 0x00157ADC
	private static void UnreserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			if (gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(smi.gameObject, "{0} UnreserveEdible but it wasn't reserved: {1}", new object[]
			{
				smi.gameObject,
				gameObject
			});
		}
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00159940 File Offset: 0x00157B40
	private static void EatComplete(EatStates.Instance smi)
	{
		PrimaryElement primaryElement = smi.sm.target.Get<PrimaryElement>(smi);
		if (primaryElement != null)
		{
			smi.lastMealElement = primaryElement.Element;
		}
		smi.Trigger(1386391852, smi.sm.target.Get<KPrefabID>(smi));
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x000A843D File Offset: 0x000A663D
	private static int GetEdibleCell(EatStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi).transform.GetPosition() + smi.sm.offset.Get(smi));
	}

	// Token: 0x040003FD RID: 1021
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat;

	// Token: 0x040003FE RID: 1022
	public EatStates.EatingState eating;

	// Token: 0x040003FF RID: 1023
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State behaviourcomplete;

	// Token: 0x04000400 RID: 1024
	public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.Vector3Parameter offset;

	// Token: 0x04000401 RID: 1025
	public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.TargetParameter target;

	// Token: 0x02000183 RID: 387
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000184 RID: 388
	public new class Instance : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.GameInstance
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x000A847D File Offset: 0x000A667D
		public void OverrideEatAnims(EatStates.Instance smi, string[] preLoopPstAnims)
		{
			global::Debug.Assert(preLoopPstAnims != null && preLoopPstAnims.Length == 3);
			smi.eatAnims = preLoopPstAnims;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00159990 File Offset: 0x00157B90
		public Instance(Chore<EatStates.Instance> chore, EatStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000A8497 File Offset: 0x000A6697
		public Element GetLatestMealElement()
		{
			return this.lastMealElement;
		}

		// Token: 0x04000402 RID: 1026
		public Element lastMealElement;

		// Token: 0x04000403 RID: 1027
		public string[] eatAnims = new string[]
		{
			"eat_pre",
			"eat_loop",
			"eat_pst"
		};
	}

	// Token: 0x02000185 RID: 389
	public class EatingState : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
	{
		// Token: 0x04000404 RID: 1028
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;

		// Token: 0x04000405 RID: 1029
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop;

		// Token: 0x04000406 RID: 1030
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pst;
	}
}
