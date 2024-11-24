using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class BeeSleepMonitor : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>
{
	// Token: 0x06000489 RID: 1161 RVA: 0x000A7991 File Offset: 0x000A5B91
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<BeeSleepMonitor.Instance, float>(this.UpdateCO2Exposure), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.BeeWantsToSleep, new StateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.Transition.ConditionCallback(this.ShouldSleep), null);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000A79CC File Offset: 0x000A5BCC
	public bool ShouldSleep(BeeSleepMonitor.Instance smi)
	{
		return smi.CO2Exposure >= 5f;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00156D18 File Offset: 0x00154F18
	public void UpdateCO2Exposure(BeeSleepMonitor.Instance smi, float dt)
	{
		if (this.IsInCO2(smi))
		{
			smi.CO2Exposure += 1f;
		}
		else
		{
			smi.CO2Exposure -= 0.5f;
		}
		smi.CO2Exposure = Mathf.Clamp(smi.CO2Exposure, 0f, 10f);
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00156D70 File Offset: 0x00154F70
	public bool IsInCO2(BeeSleepMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.gameObject);
		return Grid.IsValidCell(num) && Grid.Element[num].id == SimHashes.CarbonDioxide;
	}

	// Token: 0x02000134 RID: 308
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000135 RID: 309
	public new class Instance : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.GameInstance
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x000A79E6 File Offset: 0x000A5BE6
		public Instance(IStateMachineTarget master, BeeSleepMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0400034A RID: 842
		public float CO2Exposure;
	}
}
