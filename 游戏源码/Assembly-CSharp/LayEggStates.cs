using System;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class LayEggStates : GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>
{
	// Token: 0x06000678 RID: 1656 RVA: 0x0015BF5C File Offset: 0x0015A15C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.layeggpre;
		GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.LAYINGANEGG.NAME;
		string tooltip = CREATURES.STATUSITEMS.LAYINGANEGG.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.layeggpre.Enter(new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.LayEgg)).Exit(new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.ShowEgg)).PlayAnim("lay_egg_pre").OnAnimQueueComplete(this.layeggpst);
		this.layeggpst.PlayAnim("lay_egg_pst").OnAnimQueueComplete(this.moveaside);
		this.moveaside.MoveTo(new Func<LayEggStates.Instance, int>(LayEggStates.GetMoveAsideCell), this.lookategg, this.behaviourcomplete, false);
		this.lookategg.Enter(new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.FaceEgg)).GoTo(this.behaviourcomplete);
		this.behaviourcomplete.QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.Fertile, false);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000A8F73 File Offset: 0x000A7173
	private static void LayEgg(LayEggStates.Instance smi)
	{
		smi.eggPos = smi.transform.GetPosition();
		smi.GetSMI<FertilityMonitor.Instance>().LayEgg();
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0015C07C File Offset: 0x0015A27C
	private static void ShowEgg(LayEggStates.Instance smi)
	{
		FertilityMonitor.Instance smi2 = smi.GetSMI<FertilityMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.ShowEgg();
		}
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x000A8F91 File Offset: 0x000A7191
	private static void FaceEgg(LayEggStates.Instance smi)
	{
		smi.Get<Facing>().Face(smi.eggPos);
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x0015C09C File Offset: 0x0015A29C
	private static int GetMoveAsideCell(LayEggStates.Instance smi)
	{
		int num = 1;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 8;
		}
		int cell = Grid.PosToCell(smi);
		if (Grid.IsValidCell(cell))
		{
			int num2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			int num3 = Grid.OffsetCell(cell, -num, 0);
			if (Grid.IsValidCell(num3))
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x040004B9 RID: 1209
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State layeggpre;

	// Token: 0x040004BA RID: 1210
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State layeggpst;

	// Token: 0x040004BB RID: 1211
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State moveaside;

	// Token: 0x040004BC RID: 1212
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State lookategg;

	// Token: 0x040004BD RID: 1213
	public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State behaviourcomplete;

	// Token: 0x020001DC RID: 476
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001DD RID: 477
	public new class Instance : GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.GameInstance
	{
		// Token: 0x0600067F RID: 1663 RVA: 0x000A8FAC File Offset: 0x000A71AC
		public Instance(Chore<LayEggStates.Instance> chore, LayEggStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Fertile);
		}

		// Token: 0x040004BE RID: 1214
		public Vector3 eggPos;
	}
}
