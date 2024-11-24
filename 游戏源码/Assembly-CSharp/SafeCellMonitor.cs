using System;

// Token: 0x0200136A RID: 4970
public class SafeCellMonitor : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance>
{
	// Token: 0x06006614 RID: 26132 RVA: 0x002CE068 File Offset: 0x002CC268
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.ToggleUrge(Db.Get().Urges.MoveToSafety);
		this.satisfied.EventTransition(GameHashes.SafeCellDetected, this.danger, (SafeCellMonitor.Instance smi) => smi.IsAreaUnsafe());
		this.danger.EventTransition(GameHashes.SafeCellLost, this.satisfied, (SafeCellMonitor.Instance smi) => !smi.IsAreaUnsafe()).ToggleChore((SafeCellMonitor.Instance smi) => new MoveToSafetyChore(smi.master), this.satisfied);
	}

	// Token: 0x04004C9A RID: 19610
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04004C9B RID: 19611
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.State danger;

	// Token: 0x0200136B RID: 4971
	public new class Instance : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006616 RID: 26134 RVA: 0x000E294C File Offset: 0x000E0B4C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.safeCellSensor = base.GetComponent<Sensors>().GetSensor<SafeCellSensor>();
		}

		// Token: 0x06006617 RID: 26135 RVA: 0x000E2966 File Offset: 0x000E0B66
		public bool IsAreaUnsafe()
		{
			return this.safeCellSensor.HasSafeCell();
		}

		// Token: 0x04004C9C RID: 19612
		private SafeCellSensor safeCellSensor;
	}
}
