using System;
using UnityEngine;

// Token: 0x0200072F RID: 1839
public class SighChore : Chore<SighChore.StatesInstance>
{
	// Token: 0x060020CE RID: 8398 RVA: 0x001BD444 File Offset: 0x001BB644
	public SighChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Sigh, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SighChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02000730 RID: 1840
	public class StatesInstance : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.GameInstance
	{
		// Token: 0x060020CF RID: 8399 RVA: 0x000B57CA File Offset: 0x000B39CA
		public StatesInstance(SighChore master, GameObject sigher) : base(master)
		{
			base.sm.sigher.Set(sigher, base.smi, false);
		}
	}

	// Token: 0x02000731 RID: 1841
	public class States : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore>
	{
		// Token: 0x060020D0 RID: 8400 RVA: 0x000B57EC File Offset: 0x000B39EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.sigher);
			this.root.PlayAnim("emote_depressed").OnAnimQueueComplete(null);
		}

		// Token: 0x04001576 RID: 5494
		public StateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.TargetParameter sigher;
	}
}
