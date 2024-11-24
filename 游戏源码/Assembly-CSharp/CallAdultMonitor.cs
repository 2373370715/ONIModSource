using System;
using UnityEngine;

// Token: 0x02001137 RID: 4407
public class CallAdultMonitor : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>
{
	// Token: 0x06005A28 RID: 23080 RVA: 0x00293C8C File Offset: 0x00291E8C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.CallAdultBehaviour, new StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback(CallAdultMonitor.ShouldCallAdult), delegate(CallAdultMonitor.Instance smi)
		{
			smi.RefreshCallTime();
		});
	}

	// Token: 0x06005A29 RID: 23081 RVA: 0x000DAC46 File Offset: 0x000D8E46
	public static bool ShouldCallAdult(CallAdultMonitor.Instance smi)
	{
		return Time.time >= smi.nextCallTime;
	}

	// Token: 0x02001138 RID: 4408
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003F9F RID: 16287
		public float callMinInterval = 120f;

		// Token: 0x04003FA0 RID: 16288
		public float callMaxInterval = 240f;
	}

	// Token: 0x02001139 RID: 4409
	public new class Instance : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.GameInstance
	{
		// Token: 0x06005A2C RID: 23084 RVA: 0x000DAC7E File Offset: 0x000D8E7E
		public Instance(IStateMachineTarget master, CallAdultMonitor.Def def) : base(master, def)
		{
			this.RefreshCallTime();
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x000DAC8E File Offset: 0x000D8E8E
		public void RefreshCallTime()
		{
			this.nextCallTime = Time.time + UnityEngine.Random.value * (base.def.callMaxInterval - base.def.callMinInterval) + base.def.callMinInterval;
		}

		// Token: 0x04003FA1 RID: 16289
		public float nextCallTime;
	}
}
