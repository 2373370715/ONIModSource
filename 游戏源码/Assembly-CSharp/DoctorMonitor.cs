using System;

// Token: 0x02001560 RID: 5472
public class DoctorMonitor : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance>
{
	// Token: 0x060071DE RID: 29150 RVA: 0x000EA84D File Offset: 0x000E8A4D
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleUrge(Db.Get().Urges.Doctor);
	}

	// Token: 0x02001561 RID: 5473
	public new class Instance : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060071E0 RID: 29152 RVA: 0x000EA881 File Offset: 0x000E8A81
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
