using System;

// Token: 0x020019E6 RID: 6630
[SkipSaveFileSerialization]
public class Thriver : StateMachineComponent<Thriver.StatesInstance>
{
	// Token: 0x06008A1A RID: 35354 RVA: 0x000FA69C File Offset: 0x000F889C
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020019E7 RID: 6631
	public class StatesInstance : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.GameInstance
	{
		// Token: 0x06008A1C RID: 35356 RVA: 0x000FA6B1 File Offset: 0x000F88B1
		public StatesInstance(Thriver master) : base(master)
		{
		}

		// Token: 0x06008A1D RID: 35357 RVA: 0x0035A254 File Offset: 0x00358454
		public bool IsStressed()
		{
			StressMonitor.Instance smi = base.master.GetSMI<StressMonitor.Instance>();
			return smi != null && smi.IsStressed();
		}
	}

	// Token: 0x020019E8 RID: 6632
	public class States : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver>
	{
		// Token: 0x06008A1E RID: 35358 RVA: 0x0035A278 File Offset: 0x00358478
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.EventTransition(GameHashes.NotStressed, this.idle, null).EventTransition(GameHashes.Stressed, this.stressed, null).EventTransition(GameHashes.StressedHadEnough, this.stressed, null).Enter(delegate(Thriver.StatesInstance smi)
			{
				StressMonitor.Instance smi2 = smi.master.GetSMI<StressMonitor.Instance>();
				if (smi2 != null && smi2.IsStressed())
				{
					smi.GoTo(this.stressed);
				}
			});
			this.idle.DoNothing();
			this.stressed.ToggleEffect("Thriver");
			this.toostressed.DoNothing();
		}

		// Token: 0x040067FA RID: 26618
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State idle;

		// Token: 0x040067FB RID: 26619
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State stressed;

		// Token: 0x040067FC RID: 26620
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State toostressed;
	}
}
