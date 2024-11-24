using System;

// Token: 0x02000DDB RID: 3547
public class GunkEmptier : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>
{
	// Token: 0x060045C5 RID: 17861 RVA: 0x0024CF50 File Offset: 0x0024B150
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.noOperational, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational))).DefaultState(this.operational.noStorageSpace);
		this.operational.noStorageSpace.ToggleStatusItem(Db.Get().BuildingStatusItems.GunkEmptierFull, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank));
		this.operational.ready.EventTransition(GameHashes.OnStorageChange, this.operational.noStorageSpace, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank))).ToggleChore(new Func<GunkEmptier.Instance, Chore>(GunkEmptier.CreateChore), this.operational.noStorageSpace);
	}

	// Token: 0x060045C6 RID: 17862 RVA: 0x000CD353 File Offset: 0x000CB553
	public static bool HasSpaceToEmptyABionicGunkTank(GunkEmptier.Instance smi)
	{
		return smi.RemainingStorageCapacity >= GunkMonitor.GUNK_CAPACITY;
	}

	// Token: 0x060045C7 RID: 17863 RVA: 0x000CD365 File Offset: 0x000CB565
	public static bool IsOperational(GunkEmptier.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x060045C8 RID: 17864 RVA: 0x0024D058 File Offset: 0x0024B258
	private static WorkChore<GunkEmptierWorkable> CreateChore(GunkEmptier.Instance smi)
	{
		return new WorkChore<GunkEmptierWorkable>(Db.Get().ChoreTypes.ExpellGunk, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
	}

	// Token: 0x0400302B RID: 12331
	public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noOperational;

	// Token: 0x0400302C RID: 12332
	public GunkEmptier.OperationalStates operational;

	// Token: 0x02000DDC RID: 3548
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000DDD RID: 3549
	public class OperationalStates : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State
	{
		// Token: 0x0400302D RID: 12333
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noStorageSpace;

		// Token: 0x0400302E RID: 12334
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State ready;
	}

	// Token: 0x02000DDE RID: 3550
	public new class Instance : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.GameInstance
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060045CC RID: 17868 RVA: 0x000CD37D File Offset: 0x000CB57D
		public float RemainingStorageCapacity
		{
			get
			{
				return this.storage.RemainingCapacity();
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060045CD RID: 17869 RVA: 0x000CD38A File Offset: 0x000CB58A
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x000CD397 File Offset: 0x000CB597
		public Instance(IStateMachineTarget master, GunkEmptier.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
		}

		// Token: 0x0400302F RID: 12335
		private Operational operational;

		// Token: 0x04003030 RID: 12336
		private Storage storage;
	}
}
