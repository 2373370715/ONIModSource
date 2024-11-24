using System;
using System.Diagnostics;

// Token: 0x020012CD RID: 4813
public class CellSolidEvent : CellEvent
{
	// Token: 0x060062DD RID: 25309 RVA: 0x000E086A File Offset: 0x000DEA6A
	public CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x060062DE RID: 25310 RVA: 0x002B8240 File Offset: 0x002B6440
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, bool solid)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, solid ? 1 : 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x002B8274 File Offset: 0x002B6474
	public override string GetDescription(EventInstanceBase ev)
	{
		if ((ev as CellEventInstance).data == 1)
		{
			return base.GetMessagePrefix() + "Solid=true (" + this.reason + ")";
		}
		return base.GetMessagePrefix() + "Solid=false (" + this.reason + ")";
	}
}
