using System;
using TUNING;
using UnityEngine;

// Token: 0x02000725 RID: 1829
public class RoboDancerChore : Chore<RoboDancerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060020AB RID: 8363 RVA: 0x001BCAFC File Offset: 0x001BACFC
	public RoboDancerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new RoboDancerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x000B566D File Offset: 0x000B386D
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04001557 RID: 5463
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x02000726 RID: 1830
	public class States : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore>
	{
		// Token: 0x060020AD RID: 8365 RVA: 0x001BCB98 File Offset: 0x001BAD98
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.roboDancer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksChanged, this.goToStand, (RoboDancerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((RoboDancerChore.StatesInstance smi) => smi.GetTargetCell(), this.dancing, this.idle, false);
			this.dancing.ToggleEffect("Dancing").ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.dancing.pre).Update(delegate(RoboDancerChore.StatesInstance smi, float dt)
			{
				RoboDancer.Instance smi2 = this.roboDancer.Get(smi).GetSMI<RoboDancer.Instance>();
				RoboDancer sm = smi2.sm;
				sm.timeSpentDancing.Set(sm.timeSpentDancing.Get(smi2) + dt, smi2, false);
			}, UpdateRate.SIM_33ms, false).Exit(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
			});
			this.dancing.pre.QueueAnim("robotdance_pre", false, null).OnAnimQueueComplete(this.dancing.variation_1).Enter(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
				smi.CreateAudienceWorkables();
			});
			this.dancing.variation_1.QueueAnim("robotdance_loop", false, null).OnAnimQueueComplete(this.dancing.variation_2);
			this.dancing.variation_2.QueueAnim("robotdance_2_loop", false, null).OnAnimQueueComplete(this.dancing.pst);
			this.dancing.pst.QueueAnim("robotdance_pst", false, null).OnAnimQueueComplete(this.dancing.pre);
		}

		// Token: 0x04001558 RID: 5464
		public StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.TargetParameter roboDancer;

		// Token: 0x04001559 RID: 5465
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State idle;

		// Token: 0x0400155A RID: 5466
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State goToStand;

		// Token: 0x0400155B RID: 5467
		public RoboDancerChore.States.DancingStates dancing;

		// Token: 0x02000727 RID: 1831
		public class DancingStates : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State
		{
			// Token: 0x0400155C RID: 5468
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pre;

			// Token: 0x0400155D RID: 5469
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_1;

			// Token: 0x0400155E RID: 5470
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_2;

			// Token: 0x0400155F RID: 5471
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pst;
		}
	}

	// Token: 0x02000729 RID: 1833
	public class StatesInstance : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.GameInstance
	{
		// Token: 0x060020B7 RID: 8375 RVA: 0x000B56BD File Offset: 0x000B38BD
		public StatesInstance(RoboDancerChore master, GameObject roboDancer) : base(master)
		{
			this.roboDancer = roboDancer;
			base.sm.roboDancer.Set(roboDancer, base.smi, false);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x000B56F2 File Offset: 0x000B38F2
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x001BCD98 File Offset: 0x001BAF98
		public int GetTargetCell()
		{
			Navigator component = base.GetComponent<Navigator>();
			float num = float.MaxValue;
			SocialGatheringPoint socialGatheringPoint = null;
			foreach (SocialGatheringPoint socialGatheringPoint2 in Components.SocialGatheringPoints.GetItems((int)Grid.WorldIdx[Grid.PosToCell(this)]))
			{
				float num2 = (float)component.GetNavigationCost(Grid.PosToCell(socialGatheringPoint2));
				if (num2 != -1f && num2 < num)
				{
					num = num2;
					socialGatheringPoint = socialGatheringPoint2;
				}
			}
			if (socialGatheringPoint != null)
			{
				return Grid.PosToCell(socialGatheringPoint);
			}
			return Grid.PosToCell(base.master.gameObject);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x001BCE48 File Offset: 0x001BB048
		public void CreateAudienceWorkables()
		{
			int num = Grid.PosToCell(base.gameObject);
			Vector3Int[] array = new Vector3Int[]
			{
				Vector3Int.left * 3,
				Vector3Int.left * 2,
				Vector3Int.left,
				Vector3Int.right,
				Vector3Int.right * 2,
				Vector3Int.right * 3
			};
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				int cell = Grid.OffsetCell(num, array[i].x, array[i].y);
				if (Grid.IsValidCellInWorld(cell, (int)Grid.WorldIdx[num]))
				{
					GameObject gameObject = ChoreHelpers.CreateLocator("WatchRoboDancerWorkable", Grid.CellToPos(cell));
					this.audienceWorkables[i] = gameObject;
					KSelectable kselectable = gameObject.AddOrGet<KSelectable>();
					kselectable.SetName("WatchRoboDancerWorkable");
					kselectable.IsSelectable = false;
					WatchRoboDancerWorkable watchRoboDancerWorkable = gameObject.AddOrGet<WatchRoboDancerWorkable>();
					watchRoboDancerWorkable.owner = this.roboDancer;
					new WorkChore<WatchRoboDancerWorkable>(Db.Get().ChoreTypes.JoyReaction, watchRoboDancerWorkable, null, true, null, null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true).AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
				}
			}
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x001BCFA4 File Offset: 0x001BB1A4
		public void ClearAudienceWorkables()
		{
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				if (!(this.audienceWorkables[i] == null))
				{
					WorkerBase worker = this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().worker;
					if (worker != null)
					{
						this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().CompleteWork(worker);
					}
					ChoreHelpers.DestroyLocator(this.audienceWorkables[i]);
				}
			}
		}

		// Token: 0x04001565 RID: 5477
		private GameObject roboDancer;

		// Token: 0x04001566 RID: 5478
		private GameObject[] audienceWorkables = new GameObject[4];
	}
}
