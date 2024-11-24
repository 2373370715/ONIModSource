using System;

// Token: 0x02000D75 RID: 3445
public class FlowerVase : StateMachineComponent<FlowerVase.SMInstance>
{
	// Token: 0x0600437E RID: 17278 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x000CBA0C File Offset: 0x000C9C0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04002E39 RID: 11833
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04002E3A RID: 11834
	[MyCmpReq]
	private KBoxCollider2D boxCollider;

	// Token: 0x02000D76 RID: 3446
	public class SMInstance : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.GameInstance
	{
		// Token: 0x06004381 RID: 17281 RVA: 0x000CBA27 File Offset: 0x000C9C27
		public SMInstance(FlowerVase master) : base(master)
		{
		}
	}

	// Token: 0x02000D77 RID: 3447
	public class States : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase>
	{
		// Token: 0x06004382 RID: 17282 RVA: 0x002451E0 File Offset: 0x002433E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x04002E3B RID: 11835
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State empty;

		// Token: 0x04002E3C RID: 11836
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State full;
	}
}
