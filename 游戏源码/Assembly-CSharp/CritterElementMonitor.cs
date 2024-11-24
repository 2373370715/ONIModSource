using System;

// Token: 0x0200114D RID: 4429
public class CritterElementMonitor : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06005A86 RID: 23174 RVA: 0x000DAFDC File Offset: 0x000D91DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateInElement", delegate(CritterElementMonitor.Instance smi, float dt)
		{
			smi.UpdateCurrentElement(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x0200114E RID: 4430
	public new class Instance : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06005A88 RID: 23176 RVA: 0x00294A94 File Offset: 0x00292C94
		// (remove) Token: 0x06005A89 RID: 23177 RVA: 0x00294ACC File Offset: 0x00292CCC
		public event Action<float> OnUpdateEggChances;

		// Token: 0x06005A8A RID: 23178 RVA: 0x000DB020 File Offset: 0x000D9220
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x000DB029 File Offset: 0x000D9229
		public void UpdateCurrentElement(float dt)
		{
			this.OnUpdateEggChances(dt);
		}
	}
}
