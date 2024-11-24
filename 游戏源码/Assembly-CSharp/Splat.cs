using System;

// Token: 0x0200199F RID: 6559
public class Splat : GameStateMachine<Splat, Splat.StatesInstance>
{
	// Token: 0x060088B8 RID: 35000 RVA: 0x00355108 File Offset: 0x00353308
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleChore((Splat.StatesInstance smi) => new WorkChore<SplatWorkable>(Db.Get().ChoreTypes.Mop, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.complete);
		this.complete.Enter(delegate(Splat.StatesInstance smi)
		{
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x040066CC RID: 26316
	public GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.State complete;

	// Token: 0x020019A0 RID: 6560
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020019A1 RID: 6561
	public class StatesInstance : GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060088BB RID: 35003 RVA: 0x000F9756 File Offset: 0x000F7956
		public StatesInstance(IStateMachineTarget master, Splat.Def def) : base(master, def)
		{
		}
	}
}
