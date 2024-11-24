using System;

// Token: 0x02000F1A RID: 3866
[SkipSaveFileSerialization]
public class PlanterBox : StateMachineComponent<PlanterBox.SMInstance>
{
	// Token: 0x06004DF7 RID: 19959 RVA: 0x000D2AF7 File Offset: 0x000D0CF7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04003629 RID: 13865
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x02000F1B RID: 3867
	public class SMInstance : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.GameInstance
	{
		// Token: 0x06004DF9 RID: 19961 RVA: 0x000D2B12 File Offset: 0x000D0D12
		public SMInstance(PlanterBox master) : base(master)
		{
		}
	}

	// Token: 0x02000F1C RID: 3868
	public class States : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox>
	{
		// Token: 0x06004DFA RID: 19962 RVA: 0x00266A08 File Offset: 0x00264C08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x0400362A RID: 13866
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State empty;

		// Token: 0x0400362B RID: 13867
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State full;
	}
}
