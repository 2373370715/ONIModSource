using System;

// Token: 0x020008BE RID: 2238
public abstract class StateEvent
{
	// Token: 0x060027AA RID: 10154 RVA: 0x000B9C59 File Offset: 0x000B7E59
	public StateEvent(string name)
	{
		this.name = name;
		this.debugName = "(Event)" + name;
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000B9C79 File Offset: 0x000B7E79
	public virtual StateEvent.Context Subscribe(StateMachine.Instance smi)
	{
		return new StateEvent.Context(this);
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
	{
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x000B9C81 File Offset: 0x000B7E81
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000B9C89 File Offset: 0x000B7E89
	public string GetDebugName()
	{
		return this.debugName;
	}

	// Token: 0x04001ACA RID: 6858
	protected string name;

	// Token: 0x04001ACB RID: 6859
	private string debugName;

	// Token: 0x020008BF RID: 2239
	public struct Context
	{
		// Token: 0x060027AF RID: 10159 RVA: 0x000B9C91 File Offset: 0x000B7E91
		public Context(StateEvent state_event)
		{
			this.stateEvent = state_event;
			this.data = 0;
		}

		// Token: 0x04001ACC RID: 6860
		public StateEvent stateEvent;

		// Token: 0x04001ACD RID: 6861
		public int data;
	}
}
