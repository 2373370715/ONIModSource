using System;
using STRINGS;

public class RanchedStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.ranch;
		this.root.Exit("AbandonedRanchStation", delegate(RanchedStates.Instance smi)
		{
			if (smi.Monitor.TargetRanchStation != null)
			{
				if (smi.Monitor.TargetRanchStation.IsCritterInQueue(smi.Monitor))
				{
					Debug.LogWarning("Why are we exiting RanchedStates while in the queue?");
					smi.Monitor.TargetRanchStation.Abandon(smi.Monitor);
				}
				smi.Monitor.TargetRanchStation = null;
			}
			smi.sm.ranchTarget.Set(null, smi);
		});
		this.ranch.EnterTransition(this.ranch.Cheer, (RanchedStates.Instance smi) => RanchedStates.IsCrittersTurn(smi)).EventHandler(GameHashes.RanchStationNoLongerAvailable, delegate(RanchedStates.Instance smi)
		{
			smi.GoTo(null);
		}).BehaviourComplete(GameTags.Creatures.WantsToGetRanched, true).Update(delegate(RanchedStates.Instance smi, float deltaSeconds)
		{
			RanchStation.Instance ranchStation = smi.GetRanchStation();
			if (ranchStation.IsNullOrDestroyed())
			{
				smi.StopSM("No more target ranch station.");
				return;
			}
			Option<CavityInfo> option = Option.Maybe<CavityInfo>(Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi)));
			Option<CavityInfo> cavityInfo = ranchStation.GetCavityInfo();
			if (option.IsNone() || cavityInfo.IsNone())
			{
				smi.StopSM("No longer in any cavity.");
				return;
			}
			if (option.Unwrap() != cavityInfo.Unwrap())
			{
				smi.StopSM("Critter is in a different cavity");
				return;
			}
		}, UpdateRate.SIM_200ms, false).EventHandler(GameHashes.RancherReadyAtRanchStation, delegate(RanchedStates.Instance smi)
		{
			smi.UpdateWaitingState();
		}).Exit(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride));
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State cheer = this.ranch.Cheer;
		string name = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		cheer.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter("FaceRancher", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Facing>().Face(smi.GetRanchStation().transform.GetPosition());
		}).PlayAnim("excited_loop").OnAnimQueueComplete(this.ranch.Cheer.Pst).ScheduleGoTo((RanchedStates.Instance smi) => smi.cheerAnimLength, this.ranch.Move);
		this.ranch.Cheer.Pst.ScheduleGoTo(0.2f, this.ranch.Move);
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state = this.ranch.Move.DefaultState(this.ranch.Move.MoveToRanch).Enter("Speedup", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed * 1.25f;
		});
		string name2 = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).Exit("RestoreSpeed", delegate(RanchedStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed;
		});
		this.ranch.Move.MoveToRanch.EnterTransition(this.ranch.Wait.WaitInLine, GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Not(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn))).MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRanchNavTarget), this.ranch.Wait.WaitInLine, null, false).Target(this.ranchTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranch.Wait.WaitInLine, (RanchedStates.Instance smi) => !RanchedStates.IsCrittersTurn(smi));
		this.ranch.Wait.WaitInLine.EnterTransition(this.ranch.Ranching, new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn)).Enter(delegate(RanchedStates.Instance smi)
		{
			smi.EnterQueue();
		}).EventTransition(GameHashes.DestinationReached, this.ranch.Wait.Waiting, null);
		this.ranch.Wait.Waiting.Face(this.ranchTarget, 0f).PlayAnim((RanchedStates.Instance smi) => smi.def.StartWaitingAnim, KAnim.PlayMode.Once).QueueAnim((RanchedStates.Instance smi) => smi.def.WaitingAnim, true, null);
		this.ranch.Wait.DoneWaiting.PlayAnim((RanchedStates.Instance smi) => smi.def.EndWaitingAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.ranch.Move.MoveToRanch);
		this.ranch.Ranching.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.GetOnTable)).Enter("SetCreatureAtRanchingStation", delegate(RanchedStates.Instance smi)
		{
			smi.GetRanchStation().MessageCreatureArrived(smi);
			smi.AnimController.SetSceneLayer(Grid.SceneLayer.BuildingUse);
		}).EventTransition(GameHashes.RanchingComplete, this.ranch.Wavegoodbye, null).ToggleMainStatusItem(delegate(RanchedStates.Instance smi)
		{
			RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
			if (ranchStation != null)
			{
				return ranchStation.def.CreatureRanchingStatusItem;
			}
			return Db.Get().CreatureStatusItems.GettingRanched;
		}, null);
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state2 = this.ranch.Wavegoodbye.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride)).OnAnimQueueComplete(this.ranch.Runaway);
		string name3 = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main);
		GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state3 = this.ranch.Runaway.MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRunawayCell), null, null, false);
		string name4 = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip4 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon4 = "";
		StatusItem.IconType icon_type4 = StatusItem.IconType.Info;
		NotificationType notification_type4 = NotificationType.Neutral;
		bool allow_multiples4 = false;
		main = Db.Get().StatusItemCategories.Main;
		state3.ToggleStatusItem(name4, tooltip4, icon4, icon_type4, notification_type4, allow_multiples4, default(HashedString), 129022, null, null, main);
	}

		private static void ClearLayerOverride(RanchedStates.Instance smi)
	{
		smi.AnimController.SetSceneLayer(Grid.SceneLayer.Creatures);
	}

		private static RanchStation.Instance GetRanchStation(RanchedStates.Instance smi)
	{
		return smi.GetRanchStation();
	}

		private static void GetOnTable(RanchedStates.Instance smi)
	{
		Navigator navigator = smi.Get<Navigator>();
		if (navigator.IsValidNavType(NavType.Floor))
		{
			navigator.SetCurrentNavType(NavType.Floor);
		}
		smi.Get<Facing>().SetFacing(false);
	}

		private static bool IsCrittersTurn(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		return ranchStation != null && ranchStation.IsRancherReady && ranchStation.TryGetRanched(smi);
	}

		private static int GetRanchNavTarget(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		return smi.ModifyNavTargetForCritter(ranchStation.GetRanchNavTarget());
	}

		private static int GetRunawayCell(RanchedStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		int num = Grid.OffsetCell(cell, 2, 0);
		if (Grid.Solid[num])
		{
			num = Grid.OffsetCell(cell, -2, 0);
		}
		return num;
	}

		private RanchedStates.RanchStates ranch;

		private StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.TargetParameter ranchTarget;

		public class Def : StateMachine.BaseDef
	{
				public string StartWaitingAnim = "queue_pre";

				public string WaitingAnim = "queue_loop";

				public string EndWaitingAnim = "queue_pst";

				public int WaitCellOffset = 1;
	}

		public new class Instance : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.GameInstance
	{
						public RanchableMonitor.Instance Monitor
		{
			get
			{
				if (this.ranchMonitor == null)
				{
					this.ranchMonitor = this.GetSMI<RanchableMonitor.Instance>();
				}
				return this.ranchMonitor;
			}
		}

						public KBatchedAnimController AnimController
		{
			get
			{
				return this.animController;
			}
		}

				public Instance(Chore<RanchedStates.Instance> chore, RanchedStates.Def def) : base(chore, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.OriginalSpeed = this.Monitor.NavComponent.defaultSpeed;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToGetRanched);
			KAnim.Anim anim = base.smi.Get<KBatchedAnimController>().AnimFiles[0].GetData().GetAnim("excited_loop");
			this.cheerAnimLength = ((anim != null) ? (anim.totalTime + 0.2f) : 1.2f);
		}

				public RanchStation.Instance GetRanchStation()
		{
			if (this.Monitor != null)
			{
				return this.Monitor.TargetRanchStation;
			}
			return null;
		}

				public void EnterQueue()
		{
			if (this.GetRanchStation() != null)
			{
				this.InitializeWaitCell();
				this.Monitor.NavComponent.GoTo(this.waitCell, null);
			}
		}

				public void AbandonRanchStation()
		{
			if (this.Monitor.TargetRanchStation == null || this.status == StateMachine.Status.Failed)
			{
				return;
			}
			this.StopSM("Abandoned Ranch");
		}

				public void SetRanchStation(RanchStation.Instance ranch_station)
		{
			if (this.Monitor.TargetRanchStation != null && this.Monitor.TargetRanchStation != ranch_station)
			{
				this.Monitor.TargetRanchStation.Abandon(base.smi.Monitor);
			}
			base.smi.sm.ranchTarget.Set(ranch_station.gameObject, base.smi, false);
			this.Monitor.TargetRanchStation = ranch_station;
		}

				public int ModifyNavTargetForCritter(int navCell)
		{
			if (base.smi.HasTag(GameTags.Creatures.Flyer))
			{
				return Grid.CellAbove(navCell);
			}
			return navCell;
		}

				private void InitializeWaitCell()
		{
			if (this.GetRanchStation() == null)
			{
				return;
			}
			int cell = 0;
			Extents stationExtents = this.Monitor.TargetRanchStation.StationExtents;
			int cell2 = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x, stationExtents.y));
			int num = 0;
			int num2;
			if (Grid.Raycast(cell2, new Vector2I(-1, 0), out num2, base.def.WaitCellOffset, ~(Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable)))
			{
				num = 1 + base.def.WaitCellOffset - num2;
				cell = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x + 1, stationExtents.y));
			}
			int num3 = 0;
			int num4;
			if (num != 0 && Grid.Raycast(cell, new Vector2I(1, 0), out num4, base.def.WaitCellOffset, ~(Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable)))
			{
				num3 = base.def.WaitCellOffset - num4;
			}
			int x = (base.def.WaitCellOffset - num) * -1;
			if (num == base.def.WaitCellOffset)
			{
				x = 1 + base.def.WaitCellOffset - num3;
			}
			CellOffset offset = new CellOffset(x, 0);
			this.waitCell = Grid.OffsetCell(cell2, offset);
		}

				public void UpdateWaitingState()
		{
			if (!RanchedStates.IsCrittersTurn(base.smi))
			{
				base.smi.GoTo(base.smi.sm.ranch.Wait.WaitInLine);
				return;
			}
			if (base.smi.IsInsideState(base.sm.ranch.Wait.Waiting))
			{
				base.smi.GoTo(base.smi.sm.ranch.Wait.DoneWaiting);
				return;
			}
			base.smi.GoTo(base.smi.sm.ranch.Cheer);
		}

				public float OriginalSpeed;

				private int waitCell;

				private KBatchedAnimController animController;

				private RanchableMonitor.Instance ranchMonitor;

				public float cheerAnimLength;
	}

		public class RanchStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
				public RanchedStates.CheerStates Cheer;

				public RanchedStates.MoveStates Move;

				public RanchedStates.WaitStates Wait;

				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Ranching;

				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Wavegoodbye;

				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Runaway;
	}

		public class CheerStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Cheer;

				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Pst;
	}

		public class MoveStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State MoveToRanch;
	}

		public class WaitStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State WaitInLine;

				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Waiting;

				public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State DoneWaiting;
	}
}
