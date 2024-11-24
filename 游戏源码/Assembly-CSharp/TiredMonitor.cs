using System;

// Token: 0x02001612 RID: 5650
public class TiredMonitor : GameStateMachine<TiredMonitor, TiredMonitor.Instance>
{
	// Token: 0x060074F9 RID: 29945 RVA: 0x00305188 File Offset: 0x00303388
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventTransition(GameHashes.SleepFail, this.tired, null);
		this.tired.Enter(delegate(TiredMonitor.Instance smi)
		{
			smi.SetInterruptDay();
		}).EventTransition(GameHashes.NewDay, (TiredMonitor.Instance smi) => GameClock.Instance, this.root, (TiredMonitor.Instance smi) => smi.AllowInterruptClear()).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
	}

	// Token: 0x04005796 RID: 22422
	public GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State tired;

	// Token: 0x02001613 RID: 5651
	public new class Instance : GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060074FB RID: 29947 RVA: 0x000ECD89 File Offset: 0x000EAF89
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x000ECDA0 File Offset: 0x000EAFA0
		public void SetInterruptDay()
		{
			this.interruptedDay = GameClock.Instance.GetCycle();
		}

		// Token: 0x060074FD RID: 29949 RVA: 0x000ECDB2 File Offset: 0x000EAFB2
		public bool AllowInterruptClear()
		{
			bool flag = GameClock.Instance.GetCycle() > this.interruptedDay + 1;
			if (flag)
			{
				this.interruptedDay = -1;
			}
			return flag;
		}

		// Token: 0x04005797 RID: 22423
		public int disturbedDay = -1;

		// Token: 0x04005798 RID: 22424
		public int interruptedDay = -1;
	}
}
