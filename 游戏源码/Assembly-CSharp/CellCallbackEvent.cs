using System;
using System.Diagnostics;

// Token: 0x020012C6 RID: 4806
public class CellCallbackEvent : CellEvent
{
	// Token: 0x060062C9 RID: 25289 RVA: 0x000E0834 File Offset: 0x000DEA34
	public CellCallbackEvent(string id, bool is_send, bool enable_logging = true) : base(id, "Callback", is_send, enable_logging)
	{
	}

	// Token: 0x060062CA RID: 25290 RVA: 0x002B7944 File Offset: 0x002B5B44
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, callback_id, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060062CB RID: 25291 RVA: 0x002B7970 File Offset: 0x002B5B70
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Callback=" + cellEventInstance.data.ToString();
	}
}
