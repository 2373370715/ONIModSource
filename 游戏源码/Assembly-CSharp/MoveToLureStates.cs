using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class MoveToLureStates : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>
{
	// Token: 0x06000694 RID: 1684 RVA: 0x0015C280 File Offset: 0x0015A480
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.move;
		GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.CONSIDERINGLURE.NAME;
		string tooltip = CREATURES.STATUSITEMS.CONSIDERINGLURE.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.move.MoveTo(new Func<MoveToLureStates.Instance, int>(MoveToLureStates.GetLureCell), new Func<MoveToLureStates.Instance, CellOffset[]>(MoveToLureStates.GetLureOffsets), this.arrive_at_lure, this.behaviourcomplete, false);
		this.arrive_at_lure.Enter(delegate(MoveToLureStates.Instance smi)
		{
			Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
			if (targetLure != null && targetLure.HasTag(GameTags.OneTimeUseLure))
			{
				targetLure.GetComponent<KPrefabID>().AddTag(GameTags.LureUsed, false);
			}
		}).GoTo(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.MoveToLure, false);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0015C358 File Offset: 0x0015A558
	private static Lure.Instance GetTargetLure(MoveToLureStates.Instance smi)
	{
		GameObject targetLure = smi.GetSMI<LureableMonitor.Instance>().GetTargetLure();
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.GetSMI<Lure.Instance>();
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0015C384 File Offset: 0x0015A584
	private static int GetLureCell(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return Grid.InvalidCell;
		}
		return Grid.PosToCell(targetLure);
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0015C3A8 File Offset: 0x0015A5A8
	private static CellOffset[] GetLureOffsets(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.LurePoints;
	}

	// Token: 0x040004CB RID: 1227
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move;

	// Token: 0x040004CC RID: 1228
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arrive_at_lure;

	// Token: 0x040004CD RID: 1229
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete;

	// Token: 0x020001EA RID: 490
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001EB RID: 491
	public new class Instance : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.GameInstance
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x000A9085 File Offset: 0x000A7285
		public Instance(Chore<MoveToLureStates.Instance> chore, MoveToLureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.MoveToLure);
		}
	}
}
