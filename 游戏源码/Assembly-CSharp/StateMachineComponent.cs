using System;
using KSerialization;

// Token: 0x020008F4 RID: 2292
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class StateMachineComponent : KMonoBehaviour, ISaveLoadable, IStateMachineTarget
{
	// Token: 0x060028A6 RID: 10406
	public abstract StateMachine.Instance GetSMI();

	// Token: 0x04001B2C RID: 6956
	[MyCmpAdd]
	protected StateMachineController stateMachineController;
}
