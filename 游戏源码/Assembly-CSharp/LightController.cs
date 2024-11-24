using System;

// Token: 0x02000063 RID: 99
public class LightController : GameStateMachine<LightController, LightController.Instance>
{
	// Token: 0x060001BD RID: 445 RVA: 0x0014509C File Offset: 0x0014329C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (LightController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (LightController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).Enter("SetActive", delegate(LightController.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		});
	}

	// Token: 0x0400010E RID: 270
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400010F RID: 271
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000064 RID: 100
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000065 RID: 101
	public new class Instance : GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x000A67A6 File Offset: 0x000A49A6
		public Instance(IStateMachineTarget master, LightController.Def def) : base(master, def)
		{
		}
	}
}
