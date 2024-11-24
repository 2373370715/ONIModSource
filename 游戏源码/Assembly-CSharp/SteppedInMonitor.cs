using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020015F5 RID: 5621
public class SteppedInMonitor : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance>
{
	// Token: 0x0600746B RID: 29803 RVA: 0x00303598 File Offset: 0x00301798
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.carpetedFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.carpetedFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetCarpetFeet)).ToggleExpression(Db.Get().Expressions.Tickled, null).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetCarpetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet)), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetWetFeet)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetWetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet)), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetBody.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetSoaked)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetSoaked), UpdateRate.SIM_1000ms, false).Transition(this.wetFloor, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged)), UpdateRate.SIM_200ms);
	}

	// Token: 0x0600746C RID: 29804 RVA: 0x000EC6C2 File Offset: 0x000EA8C2
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetCarpetFeet(smi);
	}

	// Token: 0x0600746D RID: 29805 RVA: 0x00303734 File Offset: 0x00301934
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && !smi.effects.HasEffect("WetFeet") && smi.IsEffectAllowed("CarpetFeet"))
		{
			smi.effects.Add("CarpetFeet", true);
		}
	}

	// Token: 0x0600746E RID: 29806 RVA: 0x000EC6CA File Offset: 0x000EA8CA
	private static void GetWetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetWetFeet(smi);
	}

	// Token: 0x0600746F RID: 29807 RVA: 0x000EC6D2 File Offset: 0x000EA8D2
	private static void GetWetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && smi.IsEffectAllowed("WetFeet"))
		{
			smi.effects.Add("WetFeet", true);
		}
	}

	// Token: 0x06007470 RID: 29808 RVA: 0x000EC705 File Offset: 0x000EA905
	private static void GetSoaked(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetSoaked(smi);
	}

	// Token: 0x06007471 RID: 29809 RVA: 0x00303784 File Offset: 0x00301984
	private static void GetSoaked(SteppedInMonitor.Instance smi)
	{
		if (smi.effects.HasEffect("WetFeet"))
		{
			smi.effects.Remove("WetFeet");
		}
		if (smi.IsEffectAllowed("SoakingWet"))
		{
			smi.effects.Add("SoakingWet", true);
		}
	}

	// Token: 0x06007472 RID: 29810 RVA: 0x003037D4 File Offset: 0x003019D4
	private static bool IsOnCarpet(SteppedInMonitor.Instance smi)
	{
		int cell = Grid.CellBelow(Grid.PosToCell(smi));
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 9];
		return Grid.IsValidCell(cell) && gameObject != null && gameObject.HasTag(GameTags.Carpeted);
	}

	// Token: 0x06007473 RID: 29811 RVA: 0x00303824 File Offset: 0x00301A24
	private static bool IsFloorWet(SteppedInMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x06007474 RID: 29812 RVA: 0x00303850 File Offset: 0x00301A50
	private static bool IsSubmerged(SteppedInMonitor.Instance smi)
	{
		int num = Grid.CellAbove(Grid.PosToCell(smi));
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x0400571F RID: 22303
	public const string CARPET_EFFECT_NAME = "CarpetFeet";

	// Token: 0x04005720 RID: 22304
	public const string WET_FEET_EFFECT_NAME = "WetFeet";

	// Token: 0x04005721 RID: 22305
	public const string SOAK_EFFECT_NAME = "SoakingWet";

	// Token: 0x04005722 RID: 22306
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005723 RID: 22307
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State carpetedFloor;

	// Token: 0x04005724 RID: 22308
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor;

	// Token: 0x04005725 RID: 22309
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody;

	// Token: 0x020015F6 RID: 5622
	public new class Instance : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06007477 RID: 29815 RVA: 0x000EC71E File Offset: 0x000EA91E
		// (set) Token: 0x06007476 RID: 29814 RVA: 0x000EC715 File Offset: 0x000EA915
		public string[] effectsAllowed { get; private set; }

		// Token: 0x06007478 RID: 29816 RVA: 0x000EC726 File Offset: 0x000EA926
		public Instance(IStateMachineTarget master) : this(master, new string[]
		{
			"CarpetFeet",
			"WetFeet",
			"SoakingWet"
		})
		{
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x000EC74D File Offset: 0x000EA94D
		public Instance(IStateMachineTarget master, string[] effectsAllowed) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			this.effectsAllowed = effectsAllowed;
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x00303880 File Offset: 0x00301A80
		public bool IsEffectAllowed(string effectName)
		{
			if (this.effectsAllowed == null || this.effectsAllowed.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.effectsAllowed.Length; i++)
			{
				if (this.effectsAllowed[i] == effectName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005726 RID: 22310
		public Effects effects;
	}
}
