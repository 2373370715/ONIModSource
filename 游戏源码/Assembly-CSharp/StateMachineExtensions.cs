using System;

// Token: 0x020008F3 RID: 2291
public static class StateMachineExtensions
{
	// Token: 0x060028A5 RID: 10405 RVA: 0x000BA5E0 File Offset: 0x000B87E0
	public static bool IsNullOrStopped(this StateMachine.Instance smi)
	{
		return smi == null || !smi.IsRunning();
	}
}
