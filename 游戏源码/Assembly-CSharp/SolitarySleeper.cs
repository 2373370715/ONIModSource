using System;

// Token: 0x02001657 RID: 5719
[SkipSaveFileSerialization]
public class SolitarySleeper : StateMachineComponent<SolitarySleeper.StatesInstance>
{
	// Token: 0x06007621 RID: 30241 RVA: 0x000EDA1B File Offset: 0x000EBC1B
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06007622 RID: 30242 RVA: 0x0030882C File Offset: 0x00306A2C
	protected bool IsUncomfortable()
	{
		if (!base.gameObject.GetSMI<StaminaMonitor.Instance>().IsSleeping())
		{
			return false;
		}
		int num = 5;
		bool flag = true;
		bool flag2 = true;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 1; i < num; i++)
		{
			int num2 = Grid.OffsetCell(cell, i, 0);
			int num3 = Grid.OffsetCell(cell, -i, 0);
			if (Grid.Solid[num3])
			{
				flag = false;
			}
			if (Grid.Solid[num2])
			{
				flag2 = false;
			}
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				if (flag && Grid.PosToCell(minionIdentity.gameObject) == num3)
				{
					return true;
				}
				if (flag2 && Grid.PosToCell(minionIdentity.gameObject) == num2)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x02001658 RID: 5720
	public class StatesInstance : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.GameInstance
	{
		// Token: 0x06007624 RID: 30244 RVA: 0x000EDA30 File Offset: 0x000EBC30
		public StatesInstance(SolitarySleeper master) : base(master)
		{
		}
	}

	// Token: 0x02001659 RID: 5721
	public class States : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper>
	{
		// Token: 0x06007625 RID: 30245 RVA: 0x00308928 File Offset: 0x00306B28
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.TagTransition(GameTags.Dead, null, false).EventTransition(GameHashes.NewDay, this.satisfied, null).Update("SolitarySleeperCheck", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					if (smi.GetCurrentState() != this.suffering)
					{
						smi.GoTo(this.suffering);
						return;
					}
				}
				else if (smi.GetCurrentState() != this.satisfied)
				{
					smi.GoTo(this.satisfied);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.suffering.AddEffect("PeopleTooCloseWhileSleeping").ToggleExpression(Db.Get().Expressions.Uncomfortable, null).Update("PeopleTooCloseSleepFail", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				smi.master.gameObject.Trigger(1338475637, this);
			}, UpdateRate.SIM_1000ms, false);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005883 RID: 22659
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State satisfied;

		// Token: 0x04005884 RID: 22660
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State suffering;
	}
}
