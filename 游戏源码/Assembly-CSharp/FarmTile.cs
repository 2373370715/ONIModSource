using System;

// Token: 0x02000D64 RID: 3428
public class FarmTile : StateMachineComponent<FarmTile.SMInstance>
{
	// Token: 0x06004329 RID: 17193 RVA: 0x000CB68A File Offset: 0x000C988A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04002DF8 RID: 11768
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04002DF9 RID: 11769
	[MyCmpReq]
	private Storage storage;

	// Token: 0x02000D65 RID: 3429
	public class SMInstance : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.GameInstance
	{
		// Token: 0x0600432B RID: 17195 RVA: 0x000CB6A5 File Offset: 0x000C98A5
		public SMInstance(FarmTile master) : base(master)
		{
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00244020 File Offset: 0x00242220
		public bool HasWater()
		{
			PrimaryElement primaryElement = base.master.storage.FindPrimaryElement(SimHashes.Water);
			return primaryElement != null && primaryElement.Mass > 0f;
		}
	}

	// Token: 0x02000D66 RID: 3430
	public class States : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile>
	{
		// Token: 0x0600432D RID: 17197 RVA: 0x0024405C File Offset: 0x0024225C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant != null);
			this.empty.wet.EventTransition(GameHashes.OnStorageChange, this.empty.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.empty.dry.EventTransition(GameHashes.OnStorageChange, this.empty.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant == null);
			this.full.wet.EventTransition(GameHashes.OnStorageChange, this.full.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.dry.EventTransition(GameHashes.OnStorageChange, this.full.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
		}

		// Token: 0x04002DFA RID: 11770
		public FarmTile.States.FarmStates empty;

		// Token: 0x04002DFB RID: 11771
		public FarmTile.States.FarmStates full;

		// Token: 0x02000D67 RID: 3431
		public class FarmStates : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State
		{
			// Token: 0x04002DFC RID: 11772
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State wet;

			// Token: 0x04002DFD RID: 11773
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State dry;
		}
	}
}
