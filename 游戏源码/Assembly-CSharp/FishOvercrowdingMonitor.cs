using System;

// Token: 0x0200118B RID: 4491
public class FishOvercrowdingMonitor : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>
{
	// Token: 0x06005B9F RID: 23455 RVA: 0x00298718 File Offset: 0x00296918
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Register)).Exit(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Unregister));
		this.satisfied.DoNothing();
		this.overcrowded.DoNothing();
	}

	// Token: 0x06005BA0 RID: 23456 RVA: 0x000DBC31 File Offset: 0x000D9E31
	private static void Register(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager.Instance.Add(smi);
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x00298770 File Offset: 0x00296970
	private static void Unregister(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager instance = FishOvercrowingManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Remove(smi);
	}

	// Token: 0x040040BA RID: 16570
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State satisfied;

	// Token: 0x040040BB RID: 16571
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State overcrowded;

	// Token: 0x0200118C RID: 4492
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200118D RID: 4493
	public new class Instance : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x06005BA4 RID: 23460 RVA: 0x000DBC46 File Offset: 0x000D9E46
		public Instance(IStateMachineTarget master, FishOvercrowdingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x000DBC50 File Offset: 0x000D9E50
		public void SetOvercrowdingInfo(int cell_count, int fish_count)
		{
			this.cellCount = cell_count;
			this.fishCount = fish_count;
		}

		// Token: 0x040040BC RID: 16572
		public int cellCount;

		// Token: 0x040040BD RID: 16573
		public int fishCount;
	}
}
