﻿using System;
using UnityEngine;

public class MoveToSafetyChore : Chore<MoveToSafetyChore.StatesInstance>
{
		public MoveToSafetyChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.MoveToSafety, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToSafetyChore.StatesInstance(this, target.gameObject);
	}

		public class StatesInstance : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.GameInstance
	{
				public StatesInstance(MoveToSafetyChore master, GameObject mover) : base(master)
		{
			base.sm.mover.Set(mover, base.smi, false);
			this.sensor = base.sm.mover.Get<Sensors>(base.smi).GetSensor<SafeCellSensor>();
			this.targetCell = this.sensor.GetSensorCell();
		}

				public void UpdateTargetCell()
		{
			this.targetCell = this.sensor.GetSensorCell();
		}

				private SafeCellSensor sensor;

				public int targetCell;
	}

		public class States : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.mover);
			this.root.ToggleTag(GameTags.Idle);
			this.move.Enter("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi)
			{
				smi.UpdateTargetCell();
			}).Update("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi, float dt)
			{
				smi.UpdateTargetCell();
			}, UpdateRate.SIM_200ms, false).MoveTo((MoveToSafetyChore.StatesInstance smi) => smi.targetCell, null, null, true);
		}

				public StateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.TargetParameter mover;

				public GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.State move;
	}
}
