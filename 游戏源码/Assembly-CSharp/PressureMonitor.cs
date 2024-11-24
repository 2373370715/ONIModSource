using System;
using Klei.AI;

// Token: 0x020015B7 RID: 5559
public class PressureMonitor : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>
{
	// Token: 0x0600735A RID: 29530 RVA: 0x002FFE54 File Offset: 0x002FE054
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.inPressure, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas), UpdateRate.SIM_200ms);
		this.inPressure.Transition(this.safe, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas)), UpdateRate.SIM_200ms).DefaultState(this.inPressure.idle);
		this.inPressure.idle.EventTransition(GameHashes.EffectImmunityAdded, this.inPressure.immune, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)).Update(new Action<PressureMonitor.Instance, float>(PressureMonitor.HighPressureUpdate), UpdateRate.SIM_200ms, false);
		this.inPressure.immune.EventTransition(GameHashes.EffectImmunityRemoved, this.inPressure.idle, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)));
	}

	// Token: 0x0600735B RID: 29531 RVA: 0x000EBAE8 File Offset: 0x000E9CE8
	public static bool IsInPressureGas(PressureMonitor.Instance smi)
	{
		return smi.IsInHighPressure();
	}

	// Token: 0x0600735C RID: 29532 RVA: 0x000EBAF0 File Offset: 0x000E9CF0
	public static bool IsImmuneToPressure(PressureMonitor.Instance smi)
	{
		return smi.IsImmuneToHighPressure();
	}

	// Token: 0x0600735D RID: 29533 RVA: 0x000EBAF8 File Offset: 0x000E9CF8
	public static void RemoveOverpressureEffect(PressureMonitor.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x0600735E RID: 29534 RVA: 0x000EBB00 File Offset: 0x000E9D00
	public static void HighPressureUpdate(PressureMonitor.Instance smi, float dt)
	{
		if (smi.timeinstate > 3f)
		{
			smi.AddEffect();
		}
	}

	// Token: 0x04005638 RID: 22072
	public const string OVER_PRESSURE_EFFECT_NAME = "PoppedEarDrums";

	// Token: 0x04005639 RID: 22073
	public const float TIME_IN_PRESSURE_BEFORE_EAR_POPS = 3f;

	// Token: 0x0400563A RID: 22074
	private static CellOffset[] PRESSURE_TEST_OFFSET = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1)
	};

	// Token: 0x0400563B RID: 22075
	public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State safe;

	// Token: 0x0400563C RID: 22076
	public PressureMonitor.PressureStates inPressure;

	// Token: 0x020015B8 RID: 5560
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015B9 RID: 5561
	public class PressureStates : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State
	{
		// Token: 0x0400563D RID: 22077
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State idle;

		// Token: 0x0400563E RID: 22078
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State immune;
	}

	// Token: 0x020015BA RID: 5562
	public new class Instance : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.GameInstance
	{
		// Token: 0x06007363 RID: 29539 RVA: 0x000EBB4E File Offset: 0x000E9D4E
		public Instance(IStateMachineTarget master, PressureMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x000EBB64 File Offset: 0x000E9D64
		public bool IsImmuneToHighPressure()
		{
			return this.effects.HasImmunityTo(Db.Get().effects.Get("PoppedEarDrums"));
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x002FFF38 File Offset: 0x002FE138
		public bool IsInHighPressure()
		{
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < PressureMonitor.PRESSURE_TEST_OFFSET.Length; i++)
			{
				int num = Grid.OffsetCell(cell, PressureMonitor.PRESSURE_TEST_OFFSET[i]);
				if (Grid.IsValidCell(num) && Grid.Element[num].IsGas && Grid.Mass[num] > 4f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x000EBB85 File Offset: 0x000E9D85
		public void RemoveEffect()
		{
			this.effects.Remove("PoppedEarDrums");
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x000EBB97 File Offset: 0x000E9D97
		public void AddEffect()
		{
			this.effects.Add("PoppedEarDrums", true);
		}

		// Token: 0x0400563F RID: 22079
		private Effects effects;
	}
}
