using System;
using System.Diagnostics;

// Token: 0x020012CC RID: 4812
public class CellModifyMassEvent : CellEvent
{
	// Token: 0x060062DA RID: 25306 RVA: 0x000E0828 File Offset: 0x000DEA28
	public CellModifyMassEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x060062DB RID: 25307 RVA: 0x002B7890 File Offset: 0x002B5A90
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060062DC RID: 25308 RVA: 0x002B78C4 File Offset: 0x002B5AC4
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		SimHashes data = (SimHashes)cellEventInstance.data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			", Mass=",
			((float)cellEventInstance.data2 / 1000f).ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
