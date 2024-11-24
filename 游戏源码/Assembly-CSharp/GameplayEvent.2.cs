using System;

// Token: 0x02000791 RID: 1937
public abstract class GameplayEvent<StateMachineInstanceType> : GameplayEvent where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x060022E0 RID: 8928 RVA: 0x000B6B05 File Offset: 0x000B4D05
	public GameplayEvent(string id, int priority = 0, int importance = 0) : base(id, priority, importance)
	{
	}
}
