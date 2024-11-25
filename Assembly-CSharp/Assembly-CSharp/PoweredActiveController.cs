﻿using System;

public class PoweredActiveController : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredActiveController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (PoweredActiveController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working.Enter(delegate(PoweredActiveController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.Working, null);
			}
		}).Exit(delegate(PoweredActiveController.Instance smi)
		{
			if (smi.def.showWorkingStatus)
			{
				smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.Working, false);
			}
		});
		this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
		this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.working.pst, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pst, (PoweredActiveController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
	}

		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State off;

		public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State on;

		public PoweredActiveController.WorkingStates working;

		public class Def : StateMachine.BaseDef
	{
				public bool showWorkingStatus;
	}

		public class WorkingStates : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State
	{
				public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State pre;

				public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State loop;

				public GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.State pst;
	}

		public new class Instance : GameStateMachine<PoweredActiveController, PoweredActiveController.Instance, IStateMachineTarget, PoweredActiveController.Def>.GameInstance
	{
				public Instance(IStateMachineTarget master, PoweredActiveController.Def def) : base(master, def)
		{
		}
	}
}
