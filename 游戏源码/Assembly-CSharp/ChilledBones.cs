using System;
using Klei.AI;

// Token: 0x0200106B RID: 4203
public class ChilledBones : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>
{
	// Token: 0x060055BB RID: 21947 RVA: 0x0027F990 File Offset: 0x0027DB90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.normal.UpdateTransition(this.chilled, new Func<ChilledBones.Instance, float, bool>(this.IsChilling), UpdateRate.SIM_200ms, false);
		this.chilled.ToggleEffect("ChilledBones").UpdateTransition(this.normal, new Func<ChilledBones.Instance, float, bool>(this.IsNotChilling), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060055BC RID: 21948 RVA: 0x000D7EC3 File Offset: 0x000D60C3
	public bool IsNotChilling(ChilledBones.Instance smi, float dt)
	{
		return !this.IsChilling(smi, dt);
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x000D7ED0 File Offset: 0x000D60D0
	public bool IsChilling(ChilledBones.Instance smi, float dt)
	{
		return smi.IsChilled;
	}

	// Token: 0x04003C31 RID: 15409
	public const string EFFECT_NAME = "ChilledBones";

	// Token: 0x04003C32 RID: 15410
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State normal;

	// Token: 0x04003C33 RID: 15411
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State chilled;

	// Token: 0x0200106C RID: 4204
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003C34 RID: 15412
		public float THRESHOLD = -1f;
	}

	// Token: 0x0200106D RID: 4205
	public new class Instance : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.GameInstance
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060055C0 RID: 21952 RVA: 0x000D7EF3 File Offset: 0x000D60F3
		public float TemperatureTransferAttribute
		{
			get
			{
				return this.minionModifiers.GetAttributes().GetValue(this.bodyTemperatureTransferAttribute.Id) * 600f;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060055C1 RID: 21953 RVA: 0x000D7F16 File Offset: 0x000D6116
		public bool IsChilled
		{
			get
			{
				return this.TemperatureTransferAttribute < base.def.THRESHOLD;
			}
		}

		// Token: 0x060055C2 RID: 21954 RVA: 0x000D7F2B File Offset: 0x000D612B
		public Instance(IStateMachineTarget master, ChilledBones.Def def) : base(master, def)
		{
			this.bodyTemperatureTransferAttribute = Db.Get().Attributes.TryGet("TemperatureDelta");
		}

		// Token: 0x04003C35 RID: 15413
		[MyCmpGet]
		public MinionModifiers minionModifiers;

		// Token: 0x04003C36 RID: 15414
		public Klei.AI.Attribute bodyTemperatureTransferAttribute;
	}
}
