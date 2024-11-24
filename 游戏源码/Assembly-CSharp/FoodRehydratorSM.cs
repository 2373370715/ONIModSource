using System;

// Token: 0x02000D84 RID: 3460
public class FoodRehydratorSM : GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>
{
	// Token: 0x060043D7 RID: 17367 RVA: 0x002464C4 File Offset: 0x002446C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional);
		this.off.PlayAnim("off", KAnim.PlayMode.Loop).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional).EventTransition(GameHashes.FunctionalChanged, this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional);
		this.on.PlayAnim("on", KAnim.PlayMode.Loop).EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.active, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsActive).EventTransition(GameHashes.FunctionalChanged, this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EventTransition(GameHashes.ActiveChanged, this.active, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsActive);
		this.active.EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsActive).EventTransition(GameHashes.FunctionalChanged, this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EventTransition(GameHashes.ActiveChanged, this.postactive, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsActive);
		this.postactive.OnAnimQueueComplete(this.on);
	}

	// Token: 0x04002E8A RID: 11914
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State off;

	// Token: 0x04002E8B RID: 11915
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State on;

	// Token: 0x04002E8C RID: 11916
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State active;

	// Token: 0x04002E8D RID: 11917
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State postactive;

	// Token: 0x02000D85 RID: 3461
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D86 RID: 3462
	public class StatesInstance : GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.GameInstance
	{
		// Token: 0x060043DA RID: 17370 RVA: 0x000CBE09 File Offset: 0x000CA009
		public StatesInstance(IStateMachineTarget master, FoodRehydratorSM.Def def) : base(master, def)
		{
		}

		// Token: 0x04002E8E RID: 11918
		[MyCmpReq]
		public Operational operational;
	}
}
