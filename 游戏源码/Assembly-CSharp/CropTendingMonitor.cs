using System;
using UnityEngine;

// Token: 0x0200115C RID: 4444
public class CropTendingMonitor : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>
{
	// Token: 0x06005AC7 RID: 23239 RVA: 0x000DB269 File Offset: 0x000D9469
	private bool InterestedInTendingCrops(CropTendingMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.Hungry) || UnityEngine.Random.value <= smi.def.unsatisfiedTendChance;
	}

	// Token: 0x06005AC8 RID: 23240 RVA: 0x00295720 File Offset: 0x00293920
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.cooldown.ParamTransition<float>(this.cooldownTimer, this.lookingForCrop, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && this.InterestedInTendingCrops(smi)).ParamTransition<float>(this.cooldownTimer, this.reset, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && !this.InterestedInTendingCrops(smi)).Update(delegate(CropTendingMonitor.Instance smi, float dt)
		{
			this.cooldownTimer.Delta(-dt, smi);
		}, UpdateRate.SIM_1000ms, false);
		this.lookingForCrop.ToggleBehaviour(GameTags.Creatures.WantsToTendCrops, (CropTendingMonitor.Instance smi) => true, delegate(CropTendingMonitor.Instance smi)
		{
			smi.GoTo(this.reset);
		});
		this.reset.Exit(delegate(CropTendingMonitor.Instance smi)
		{
			this.cooldownTimer.Set(600f / smi.def.numCropsTendedPerCycle, smi, false);
		}).GoTo(this.cooldown);
	}

	// Token: 0x04004009 RID: 16393
	private StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.FloatParameter cooldownTimer;

	// Token: 0x0400400A RID: 16394
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State cooldown;

	// Token: 0x0400400B RID: 16395
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State lookingForCrop;

	// Token: 0x0400400C RID: 16396
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State reset;

	// Token: 0x0200115D RID: 4445
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400400D RID: 16397
		public float numCropsTendedPerCycle = 8f;

		// Token: 0x0400400E RID: 16398
		public float unsatisfiedTendChance = 0.5f;
	}

	// Token: 0x0200115E RID: 4446
	public new class Instance : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.GameInstance
	{
		// Token: 0x06005AD0 RID: 23248 RVA: 0x000DB332 File Offset: 0x000D9532
		public Instance(IStateMachineTarget master, CropTendingMonitor.Def def) : base(master, def)
		{
		}
	}
}
