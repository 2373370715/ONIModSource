using System;
using UnityEngine;

// Token: 0x0200073C RID: 1852
public class StressIdleChore : Chore<StressIdleChore.StatesInstance>
{
	// Token: 0x06002105 RID: 8453 RVA: 0x001BE3DC File Offset: 0x001BC5DC
	public StressIdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.StressIdle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressIdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200073D RID: 1853
	public class StatesInstance : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.GameInstance
	{
		// Token: 0x06002106 RID: 8454 RVA: 0x000B59CA File Offset: 0x000B3BCA
		public StatesInstance(StressIdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
		}
	}

	// Token: 0x0200073E RID: 1854
	public class States : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore>
	{
		// Token: 0x06002107 RID: 8455 RVA: 0x000B59EC File Offset: 0x000B3BEC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.PlayAnim("idle_default", KAnim.PlayMode.Loop);
		}

		// Token: 0x040015AC RID: 5548
		public StateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.TargetParameter idler;

		// Token: 0x040015AD RID: 5549
		public GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.State idle;
	}
}
