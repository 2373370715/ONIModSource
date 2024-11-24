using System;

// Token: 0x02000688 RID: 1672
public class DieChore : Chore<DieChore.StatesInstance>
{
	// Token: 0x06001E56 RID: 7766 RVA: 0x001B3A74 File Offset: 0x001B1C74
	public DieChore(IStateMachineTarget master, Death death) : base(Db.Get().ChoreTypes.Die, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DieChore.StatesInstance(this, death);
	}

	// Token: 0x02000689 RID: 1673
	public class StatesInstance : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.GameInstance
	{
		// Token: 0x06001E57 RID: 7767 RVA: 0x000B406E File Offset: 0x000B226E
		public StatesInstance(DieChore master, Death death) : base(master)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x001B3AC0 File Offset: 0x001B1CC0
		public void PlayPreAnim()
		{
			string preAnim = base.sm.death.Get(base.smi).preAnim;
			base.GetComponent<KAnimControllerBase>().Play(preAnim, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0200068A RID: 1674
	public class States : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore>
	{
		// Token: 0x06001E59 RID: 7769 RVA: 0x001B3B08 File Offset: 0x001B1D08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dying;
			this.dying.OnAnimQueueComplete(this.dead).Enter("PlayAnim", delegate(DieChore.StatesInstance smi)
			{
				smi.PlayPreAnim();
			});
			this.dead.ReturnSuccess();
		}

		// Token: 0x04001367 RID: 4967
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dying;

		// Token: 0x04001368 RID: 4968
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dead;

		// Token: 0x04001369 RID: 4969
		public StateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.ResourceParameter<Death> death;
	}
}
