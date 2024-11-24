using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02001543 RID: 5443
public class CoughMonitor : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>
{
	// Token: 0x06007172 RID: 29042 RVA: 0x002FA818 File Offset: 0x002F8A18
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.PoorAirQuality, new GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameEvent.Callback(this.OnBreatheDirtyAir)).ParamTransition<bool>(this.shouldCough, this.coughing, (CoughMonitor.Instance smi, bool bShouldCough) => bShouldCough);
		this.coughing.ToggleStatusItem(Db.Get().DuplicantStatusItems.Coughing, null).ToggleReactable((CoughMonitor.Instance smi) => smi.GetReactable()).ParamTransition<bool>(this.shouldCough, this.idle, (CoughMonitor.Instance smi, bool bShouldCough) => !bShouldCough);
	}

	// Token: 0x06007173 RID: 29043 RVA: 0x002FA8F4 File Offset: 0x002F8AF4
	private void OnBreatheDirtyAir(CoughMonitor.Instance smi, object data)
	{
		float timeInCycles = GameClock.Instance.GetTimeInCycles();
		if (timeInCycles > 0.1f && timeInCycles - smi.lastCoughTime <= 0.1f)
		{
			return;
		}
		Sim.MassConsumedCallback massConsumedCallback = (Sim.MassConsumedCallback)data;
		float num = (smi.lastConsumeTime <= 0f) ? 0f : (timeInCycles - smi.lastConsumeTime);
		smi.lastConsumeTime = timeInCycles;
		smi.amountConsumed -= 0.05f * num;
		smi.amountConsumed = Mathf.Max(smi.amountConsumed, 0f);
		smi.amountConsumed += massConsumedCallback.mass;
		if (smi.amountConsumed >= 1f)
		{
			this.shouldCough.Set(true, smi, false);
			smi.lastConsumeTime = 0f;
			smi.amountConsumed = 0f;
		}
	}

	// Token: 0x040054B6 RID: 21686
	private const float amountToCough = 1f;

	// Token: 0x040054B7 RID: 21687
	private const float decayRate = 0.05f;

	// Token: 0x040054B8 RID: 21688
	private const float coughInterval = 0.1f;

	// Token: 0x040054B9 RID: 21689
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State idle;

	// Token: 0x040054BA RID: 21690
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State coughing;

	// Token: 0x040054BB RID: 21691
	public StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter shouldCough = new StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter(false);

	// Token: 0x02001544 RID: 5444
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001545 RID: 5445
	public new class Instance : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameInstance
	{
		// Token: 0x06007176 RID: 29046 RVA: 0x000EA45B File Offset: 0x000E865B
		public Instance(IStateMachineTarget master, CoughMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06007177 RID: 29047 RVA: 0x002FA9C0 File Offset: 0x002F8BC0
		public Reactable GetReactable()
		{
			Emote cough_Small = Db.Get().Emotes.Minion.Cough_Small;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "BadAirCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(cough_Small);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable.RegisterEmoteStepCallbacks("react_small", null, new Action<GameObject>(this.FinishedCoughing));
		}

		// Token: 0x06007178 RID: 29048 RVA: 0x002FAA4C File Offset: 0x002F8C4C
		private void FinishedCoughing(GameObject cougher)
		{
			cougher.GetComponent<Effects>().Add("ContaminatedLungs", true);
			base.sm.shouldCough.Set(false, base.smi, false);
			base.smi.lastCoughTime = GameClock.Instance.GetTimeInCycles();
		}

		// Token: 0x040054BC RID: 21692
		[Serialize]
		public float lastCoughTime;

		// Token: 0x040054BD RID: 21693
		[Serialize]
		public float lastConsumeTime;

		// Token: 0x040054BE RID: 21694
		[Serialize]
		public float amountConsumed;
	}
}
