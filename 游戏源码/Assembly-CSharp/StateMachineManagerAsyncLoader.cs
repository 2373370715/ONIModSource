using System;
using System.Collections.Generic;

// Token: 0x020008FD RID: 2301
internal class StateMachineManagerAsyncLoader : GlobalAsyncLoader<StateMachineManagerAsyncLoader>
{
	// Token: 0x060028DB RID: 10459 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void Run()
	{
	}

	// Token: 0x04001B3E RID: 6974
	public List<StateMachine> stateMachines = new List<StateMachine>();
}
