using System;
using System.Diagnostics;

// Token: 0x020012C5 RID: 4805
public class CellAddRemoveSubstanceEvent : CellEvent
{
	// Token: 0x060062C6 RID: 25286 RVA: 0x000E0828 File Offset: 0x000DEA28
	public CellAddRemoveSubstanceEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x060062C7 RID: 25287 RVA: 0x002B7890 File Offset: 0x002B5A90
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060062C8 RID: 25288 RVA: 0x002B78C4 File Offset: 0x002B5AC4
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
