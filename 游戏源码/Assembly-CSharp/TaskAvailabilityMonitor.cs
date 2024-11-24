using System;

// Token: 0x02001604 RID: 5636
public class TaskAvailabilityMonitor : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance>
{
	// Token: 0x060074AA RID: 29866 RVA: 0x003041D4 File Offset: 0x003023D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventTransition(GameHashes.NewDay, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance, this.unavailable, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance.GetCycle() > 0);
		this.unavailable.Enter("RefreshStatusItem", delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).EventHandler(GameHashes.ScheduleChanged, delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		});
	}

	// Token: 0x04005757 RID: 22359
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005758 RID: 22360
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State unavailable;

	// Token: 0x02001605 RID: 5637
	public new class Instance : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060074AC RID: 29868 RVA: 0x000EC9EF File Offset: 0x000EABEF
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060074AD RID: 29869 RVA: 0x00304298 File Offset: 0x00302498
		public void RefreshStatusItem()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			WorldContainer myWorld = base.gameObject.GetMyWorld();
			if (myWorld != null && myWorld.IsModuleInterior && myWorld.ParentWorldId == myWorld.id)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.IdleInRockets, null);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Idle, null);
		}
	}
}
