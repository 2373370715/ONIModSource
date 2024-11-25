﻿using System;
using KSerialization;
using STRINGS;

public class SameSpotPoopStates : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.root.Enter("SetTarget", delegate(SameSpotPoopStates.Instance smi)
		{
			this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi, false);
		});
		this.goingtopoop.MoveTo((SameSpotPoopStates.Instance smi) => smi.GetLastPoopCell(), this.pooping, this.updatepoopcell, false);
		GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State state = this.pooping.PlayAnim("poop");
		string name = CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).OnAnimQueueComplete(this.behaviourcomplete);
		this.updatepoopcell.Enter(delegate(SameSpotPoopStates.Instance smi)
		{
			smi.SetLastPoopCell();
		}).GoTo(this.pooping);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

		public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State goingtopoop;

		public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State pooping;

		public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State behaviourcomplete;

		public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State updatepoopcell;

		public StateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.IntParameter targetCell;

		public class Def : StateMachine.BaseDef
	{
	}

		public new class Instance : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.GameInstance
	{
				public Instance(Chore<SameSpotPoopStates.Instance> chore, SameSpotPoopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

				public int GetLastPoopCell()
		{
			if (this.lastPoopCell == -1)
			{
				this.SetLastPoopCell();
			}
			return this.lastPoopCell;
		}

				public void SetLastPoopCell()
		{
			this.lastPoopCell = Grid.PosToCell(this);
		}

				[Serialize]
		private int lastPoopCell = -1;
	}
}
