using System;

// Token: 0x02000788 RID: 1928
public abstract class Usable : KMonoBehaviour, IStateMachineTarget
{
	// Token: 0x060022AE RID: 8878
	public abstract void StartUsing(User user);

	// Token: 0x060022AF RID: 8879 RVA: 0x001C3E58 File Offset: 0x001C2058
	protected void StartUsing(StateMachine.Instance smi, User user)
	{
		DebugUtil.Assert(this.smi == null);
		DebugUtil.Assert(smi != null);
		this.smi = smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
		smi.StartSM();
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x001C3EAC File Offset: 0x001C20AC
	public void StopUsing(User user)
	{
		if (this.smi != null)
		{
			StateMachine.Instance instance = this.smi;
			instance.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(instance.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
			this.smi.StopSM("Usable.StopUsing");
			this.smi = null;
		}
	}

	// Token: 0x040016DF RID: 5855
	private StateMachine.Instance smi;
}
