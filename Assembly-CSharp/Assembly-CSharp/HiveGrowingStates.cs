﻿using System;
using STRINGS;

public class HiveGrowingStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.growing.DefaultState(this.growing.loop);
		this.growing.loop.PlayAnim((HiveGrowingStates.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(delegate(HiveGrowingStates.Instance smi)
		{
			smi.RefreshPositionPercent();
		}).Update(delegate(HiveGrowingStates.Instance smi, float dt)
		{
			smi.RefreshPositionPercent();
			if (smi.hive.IsFullyGrown())
			{
				smi.GoTo(this.growing.pst);
			}
		}, UpdateRate.SIM_4000ms, false);
		this.growing.pst.PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.GrowUpBehaviour, false);
	}

		public HiveGrowingStates.GrowUpStates growing;

		public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State behaviourcomplete;

		public class Def : StateMachine.BaseDef
	{
	}

		public new class Instance : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.GameInstance
	{
				public Instance(Chore<HiveGrowingStates.Instance> chore, HiveGrowingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

				public void RefreshPositionPercent()
		{
			this.animController.SetPositionPercent(this.hive.sm.hiveGrowth.Get(this.hive));
		}

				[MySmiReq]
		public BeeHive.StatesInstance hive;

				[MyCmpReq]
		private KAnimControllerBase animController;
	}

		public class GrowUpStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State
	{
				public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State loop;

				public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State pst;
	}
}
