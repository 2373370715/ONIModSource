using System;
using System.Diagnostics;

// Token: 0x020012C8 RID: 4808
public class CellElementEvent : CellEvent
{
	// Token: 0x060062CF RID: 25295 RVA: 0x000E086A File Offset: 0x000DEA6A
	public CellElementEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x060062D0 RID: 25296 RVA: 0x002B7944 File Offset: 0x002B5B44
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060062D1 RID: 25297 RVA: 0x002B79CC File Offset: 0x002B5BCC
	public override string GetDescription(EventInstanceBase ev)
	{
		SimHashes data = (SimHashes)(ev as CellEventInstance).data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
