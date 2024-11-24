using System;
using STRINGS;

// Token: 0x020001F1 RID: 497
public class RanchedStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>
{
	// Token: 0x060006AD RID: 1709 RVA: 0x0015C808 File Offset: 0x0015AA08
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

	// Token: 0x060006AE RID: 1710 RVA: 0x000A9185 File Offset: 0x000A7385
	private static void ClearLayerOverride(RanchedStates.Instance smi)
	{
		smi.AnimController.SetSceneLayer(Grid.SceneLayer.Creatures);
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x000A9194 File Offset: 0x000A7394
	private static RanchStation.Instance GetRanchStation(RanchedStates.Instance smi)
	{
		return smi.GetRanchStation();
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0015CDCC File Offset: 0x0015AFCC
	private static void GetOnTable(RanchedStates.Instance smi)
	{
		Navigator navigator = smi.Get<Navigator>();
		if (navigator.IsValidNavType(NavType.Floor))
		{
			navigator.SetCurrentNavType(NavType.Floor);
		}
		smi.Get<Facing>().SetFacing(false);
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0015CDFC File Offset: 0x0015AFFC
	private static bool IsCrittersTurn(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		return ranchStation != null && ranchStation.IsRancherReady && ranchStation.TryGetRanched(smi);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0015CE28 File Offset: 0x0015B028
	private static int GetRanchNavTarget(RanchedStates.Instance smi)
	{
		RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
		return smi.ModifyNavTargetForCritter(ranchStation.GetRanchNavTarget());
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0015CE48 File Offset: 0x0015B048
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

	// Token: 0x040004DD RID: 1245
	private RanchedStates.RanchStates ranch;

	// Token: 0x040004DE RID: 1246
	private StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.TargetParameter ranchTarget;

	// Token: 0x020001F2 RID: 498
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040004DF RID: 1247
		public string StartWaitingAnim = "queue_pre";

		// Token: 0x040004E0 RID: 1248
		public string WaitingAnim = "queue_loop";

		// Token: 0x040004E1 RID: 1249
		public string EndWaitingAnim = "queue_pst";

		// Token: 0x040004E2 RID: 1250
		public int WaitCellOffset = 1;
	}

	// Token: 0x020001F3 RID: 499
	public new class Instance : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.GameInstance
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x000A91D4 File Offset: 0x000A73D4
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x000A91F0 File Offset: 0x000A73F0
		public KBatchedAnimController AnimController
		{
			get
			{
				return this.animController;
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0015CE88 File Offset: 0x0015B088
		public Instance(Chore<RanchedStates.Instance> chore, RanchedStates.Def def) : base(chore, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.OriginalSpeed = this.Monitor.NavComponent.defaultSpeed;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToGetRanched);
			KAnim.Anim anim = base.smi.Get<KBatchedAnimController>().AnimFiles[0].GetData().GetAnim("excited_loop");
			this.cheerAnimLength = ((anim != null) ? (anim.totalTime + 0.2f) : 1.2f);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000A91F8 File Offset: 0x000A73F8
		public RanchStation.Instance GetRanchStation()
		{
			if (this.Monitor != null)
			{
				return this.Monitor.TargetRanchStation;
			}
			return null;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000A920F File Offset: 0x000A740F
		public void EnterQueue()
		{
			if (this.GetRanchStation() != null)
			{
				this.InitializeWaitCell();
				this.Monitor.NavComponent.GoTo(this.waitCell, null);
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000A9237 File Offset: 0x000A7437
		public void AbandonRanchStation()
		{
			if (this.Monitor.TargetRanchStation == null || this.status == StateMachine.Status.Failed)
			{
				return;
			}
			this.StopSM("Abandoned Ranch");
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0015CF18 File Offset: 0x0015B118
		public void SetRanchStation(RanchStation.Instance ranch_station)
		{
			if (this.Monitor.TargetRanchStation != null && this.Monitor.TargetRanchStation != ranch_station)
			{
				this.Monitor.TargetRanchStation.Abandon(base.smi.Monitor);
			}
			base.smi.sm.ranchTarget.Set(ranch_station.gameObject, base.smi, false);
			this.Monitor.TargetRanchStation = ranch_station;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000A925B File Offset: 0x000A745B
		public int ModifyNavTargetForCritter(int navCell)
		{
			if (base.smi.HasTag(GameTags.Creatures.Flyer))
			{
				return Grid.CellAbove(navCell);
			}
			return navCell;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0015CF8C File Offset: 0x0015B18C
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

		// Token: 0x060006BF RID: 1727 RVA: 0x0015D09C File Offset: 0x0015B29C
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

		// Token: 0x040004E3 RID: 1251
		public float OriginalSpeed;

		// Token: 0x040004E4 RID: 1252
		private int waitCell;

		// Token: 0x040004E5 RID: 1253
		private KBatchedAnimController animController;

		// Token: 0x040004E6 RID: 1254
		private RanchableMonitor.Instance ranchMonitor;

		// Token: 0x040004E7 RID: 1255
		public float cheerAnimLength;
	}

	// Token: 0x020001F4 RID: 500
	public class RanchStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040004E8 RID: 1256
		public RanchedStates.CheerStates Cheer;

		// Token: 0x040004E9 RID: 1257
		public RanchedStates.MoveStates Move;

		// Token: 0x040004EA RID: 1258
		public RanchedStates.WaitStates Wait;

		// Token: 0x040004EB RID: 1259
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Ranching;

		// Token: 0x040004EC RID: 1260
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Wavegoodbye;

		// Token: 0x040004ED RID: 1261
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Runaway;
	}

	// Token: 0x020001F5 RID: 501
	public class CheerStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040004EE RID: 1262
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Cheer;

		// Token: 0x040004EF RID: 1263
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Pst;
	}

	// Token: 0x020001F6 RID: 502
	public class MoveStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040004F0 RID: 1264
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State MoveToRanch;
	}

	// Token: 0x020001F7 RID: 503
	public class WaitStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
	{
		// Token: 0x040004F1 RID: 1265
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State WaitInLine;

		// Token: 0x040004F2 RID: 1266
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Waiting;

		// Token: 0x040004F3 RID: 1267
		public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State DoneWaiting;
	}
}
