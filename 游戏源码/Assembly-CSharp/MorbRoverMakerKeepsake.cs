using System;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
public class MorbRoverMakerKeepsake : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>
{
	// Token: 0x06001547 RID: 5447 RVA: 0x00192EAC File Offset: 0x001910AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.silent;
		this.silent.PlayAnim("silent").Enter(new StateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State.Callback(MorbRoverMakerKeepsake.CalculateNextActivationTime)).Update(new Action<MorbRoverMakerKeepsake.Instance, float>(MorbRoverMakerKeepsake.TimerUpdate), UpdateRate.SIM_200ms, false);
		this.talking.PlayAnim("idle").OnAnimQueueComplete(this.silent);
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x000AF81D File Offset: 0x000ADA1D
	public static void CalculateNextActivationTime(MorbRoverMakerKeepsake.Instance smi)
	{
		smi.CalculateNextActivationTime();
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x000AF825 File Offset: 0x000ADA25
	public static void TimerUpdate(MorbRoverMakerKeepsake.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() > smi.NextActivationTime)
		{
			smi.GoTo(smi.sm.talking);
		}
	}

	// Token: 0x04000E5C RID: 3676
	public const string SILENT_ANIMATION_NAME = "silent";

	// Token: 0x04000E5D RID: 3677
	public const string TALKING_ANIMATION_NAME = "idle";

	// Token: 0x04000E5E RID: 3678
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State silent;

	// Token: 0x04000E5F RID: 3679
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State talking;

	// Token: 0x020004B6 RID: 1206
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000E60 RID: 3680
		public Vector2 OperationalRandomnessRange = new Vector2(120f, 600f);
	}

	// Token: 0x020004B7 RID: 1207
	public new class Instance : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.GameInstance
	{
		// Token: 0x0600154C RID: 5452 RVA: 0x000AF86F File Offset: 0x000ADA6F
		public Instance(IStateMachineTarget master, MorbRoverMakerKeepsake.Def def) : base(master, def)
		{
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00192F1C File Offset: 0x0019111C
		public void CalculateNextActivationTime()
		{
			float time = GameClock.Instance.GetTime();
			float minInclusive = time + base.def.OperationalRandomnessRange.x;
			float maxInclusive = time + base.def.OperationalRandomnessRange.y;
			this.NextActivationTime = UnityEngine.Random.Range(minInclusive, maxInclusive);
		}

		// Token: 0x04000E61 RID: 3681
		public float NextActivationTime = -1f;
	}
}
