using System;
using System.Diagnostics;

// Token: 0x020012CE RID: 4814
public class CellSolidFilterEvent : CellEvent
{
	// Token: 0x060062E0 RID: 25312 RVA: 0x000E08F8 File Offset: 0x000DEAF8
	public CellSolidFilterEvent(string id, bool enable_logging = true) : base(id, "filtered", false, enable_logging)
	{
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x002B8240 File Offset: 0x002B6440
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

	// Token: 0x060062E2 RID: 25314 RVA: 0x002B82C8 File Offset: 0x002B64C8
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Filtered Solid Event solid=" + cellEventInstance.data.ToString();
	}
}
