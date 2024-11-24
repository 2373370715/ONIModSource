using System;

// Token: 0x020012C9 RID: 4809
public class CellEvent : EventBase
{
	// Token: 0x060062D2 RID: 25298 RVA: 0x000E0877 File Offset: 0x000DEA77
	public CellEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id)
	{
		this.reason = reason;
		this.isSend = is_send;
		this.enableLogging = enable_logging;
	}

	// Token: 0x060062D3 RID: 25299 RVA: 0x000E0896 File Offset: 0x000DEA96
	public string GetMessagePrefix()
	{
		if (this.isSend)
		{
			return ">>>: ";
		}
		return "<<<: ";
	}

	// Token: 0x04004649 RID: 17993
	public string reason;

	// Token: 0x0400464A RID: 17994
	public bool isSend;

	// Token: 0x0400464B RID: 17995
	public bool enableLogging;
}
