﻿using System;
using STRINGS;
using UnityEngine;

public class MoveToLureStates : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>
{
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

		private static Lure.Instance GetTargetLure(MoveToLureStates.Instance smi)
	{
		GameObject targetLure = smi.GetSMI<LureableMonitor.Instance>().GetTargetLure();
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.GetSMI<Lure.Instance>();
	}

		private static int GetLureCell(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return Grid.InvalidCell;
		}
		return Grid.PosToCell(targetLure);
	}

		private static CellOffset[] GetLureOffsets(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.LurePoints;
	}

		public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move;

		public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arrive_at_lure;

		public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete;

		public class Def : StateMachine.BaseDef
	{
	}

		public new class Instance : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.GameInstance
	{
				public Instance(Chore<MoveToLureStates.Instance> chore, MoveToLureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.MoveToLure);
		}
	}
}
