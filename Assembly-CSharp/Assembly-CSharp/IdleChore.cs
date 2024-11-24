﻿using System;
using UnityEngine;

public class IdleChore : Chore<IdleChore.StatesInstance>
{
	public IdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Idle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.IdleTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new IdleChore.StatesInstance(this, target.gameObject);
	}

	public class StatesInstance : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.GameInstance
	{
		public StatesInstance(IdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
			this.idleCellSensor = base.GetComponent<Sensors>().GetSensor<IdleCellSensor>();
		}

		public void UpdateNavType()
		{
			NavType currentNavType = base.GetComponent<Navigator>().CurrentNavType;
			base.sm.isOnLadder.Set(currentNavType == NavType.Ladder || currentNavType == NavType.Pole, this, false);
			base.sm.isOnTube.Set(currentNavType == NavType.Tube, this, false);
			int num = Grid.PosToCell(base.smi);
			base.sm.isOnSuitMarkerCell.Set(Grid.IsValidCell(num) && Grid.HasSuitMarker[num], this, false);
		}

		public int GetIdleCell()
		{
			return this.idleCellSensor.GetCell();
		}

		public bool HasIdleCell()
		{
			return this.idleCellSensor.GetCell() != Grid.InvalidCell;
		}

		private IdleCellSensor idleCellSensor;
	}

	public class States : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.DefaultState(this.idle.onfloor).Enter("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				smi.UpdateNavType();
			}).Update("UpdateNavType", delegate(IdleChore.StatesInstance smi, float dt)
			{
				smi.UpdateNavType();
			}, UpdateRate.SIM_200ms, false).ToggleStateMachine((IdleChore.StatesInstance smi) => new TaskAvailabilityMonitor.Instance(smi.master)).ToggleTag(GameTags.Idle);
			this.idle.onfloor.PlayAnim("idle_default", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isOnLadder, this.idle.onladder, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnTube, this.idle.ontube, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnSuitMarkerCell, this.idle.onsuitmarker, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.onladder.PlayAnim("ladder_idle", KAnim.PlayMode.Loop).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.ontube.PlayAnim("tube_idle_loop", KAnim.PlayMode.Loop).Update("IdleMove", delegate(IdleChore.StatesInstance smi, float dt)
			{
				if (smi.HasIdleCell())
				{
					smi.GoTo(this.idle.move);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.idle.onsuitmarker.PlayAnim("idle_default", KAnim.PlayMode.Loop).Enter(delegate(IdleChore.StatesInstance smi)
			{
				Navigator component = smi.GetComponent<Navigator>();
				int cell = Grid.PosToCell(component);
				Grid.SuitMarker.Flags flags;
				PathFinder.PotentialPath.Flags flags2;
				Grid.TryGetSuitMarkerFlags(cell, out flags, out flags2);
				IdleSuitMarkerCellQuery idleSuitMarkerCellQuery = new IdleSuitMarkerCellQuery((flags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0, Grid.CellToXY(cell).X);
				component.RunQuery(idleSuitMarkerCellQuery);
				component.GoTo(idleSuitMarkerCellQuery.GetResultCell(), null);
			}).EventTransition(GameHashes.DestinationReached, this.idle, null).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.move.Transition(this.idle, (IdleChore.StatesInstance smi) => !smi.HasIdleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f).MoveTo((IdleChore.StatesInstance smi) => smi.GetIdleCell(), this.idle, this.idle, false).Exit("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				smi.UpdateNavType();
			}).Exit("ClearWalk", delegate(IdleChore.StatesInstance smi)
			{
				smi.GetComponent<KBatchedAnimController>().Play("idle_default", KAnim.PlayMode.Once, 1f, 0f);
			});
		}

		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnLadder;

		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnTube;

		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnSuitMarkerCell;

		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.TargetParameter idler;

		public IdleChore.States.IdleState idle;

		public class IdleState : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State
		{
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onfloor;

			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onladder;

			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State ontube;

			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onsuitmarker;

			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State move;
		}
	}
}
