using System;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class SeedPlantingMonitor : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>
{
	// Token: 0x06000700 RID: 1792 RVA: 0x0015DC78 File Offset: 0x0015BE78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToPlantSeed, new StateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.Transition.ConditionCallback(SeedPlantingMonitor.ShouldSearchForSeeds), delegate(SeedPlantingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x000A94E5 File Offset: 0x000A76E5
	public static bool ShouldSearchForSeeds(SeedPlantingMonitor.Instance smi)
	{
		return Time.time >= smi.nextSearchTime;
	}

	// Token: 0x02000206 RID: 518
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000526 RID: 1318
		public float searchMinInterval = 60f;

		// Token: 0x04000527 RID: 1319
		public float searchMaxInterval = 300f;
	}

	// Token: 0x02000207 RID: 519
	public new class Instance : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.GameInstance
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x000A951D File Offset: 0x000A771D
		public Instance(IStateMachineTarget master, SeedPlantingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000A952D File Offset: 0x000A772D
		public void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x04000528 RID: 1320
		public float nextSearchTime;
	}
}
