using System;

// Token: 0x0200165A RID: 5722
[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
	// Token: 0x06007629 RID: 30249 RVA: 0x000EDA59 File Offset: 0x000EBC59
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600762A RID: 30250 RVA: 0x000EDA66 File Offset: 0x000EBC66
	protected bool IsUncomfortable()
	{
		return base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;
	}

	// Token: 0x0200165B RID: 5723
	public class StatesInstance : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance
	{
		// Token: 0x0600762C RID: 30252 RVA: 0x000EDA8D File Offset: 0x000EBC8D
		public StatesInstance(Workaholic master) : base(master)
		{
		}
	}

	// Token: 0x0200165C RID: 5724
	public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
	{
		// Token: 0x0600762D RID: 30253 RVA: 0x00308A14 File Offset: 0x00306C14
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("WorkaholicCheck", delegate(Workaholic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005885 RID: 22661
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;

		// Token: 0x04005886 RID: 22662
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;
	}
}
