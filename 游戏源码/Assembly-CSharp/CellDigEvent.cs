using System;
using System.Diagnostics;

// Token: 0x020012C7 RID: 4807
public class CellDigEvent : CellEvent
{
	// Token: 0x060062CC RID: 25292 RVA: 0x000E0844 File Offset: 0x000DEA44
	public CellDigEvent(bool enable_logging = true) : base("Dig", "Dig", true, enable_logging)
	{
	}

	// Token: 0x060062CD RID: 25293 RVA: 0x002B79A0 File Offset: 0x002B5BA0
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060062CE RID: 25294 RVA: 0x000E0858 File Offset: 0x000DEA58
	public override string GetDescription(EventInstanceBase ev)
	{
		return base.GetMessagePrefix() + "Dig=true";
	}
}
