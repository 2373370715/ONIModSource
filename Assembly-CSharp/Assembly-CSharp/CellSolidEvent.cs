﻿using System;
using System.Diagnostics;

public class CellSolidEvent : CellEvent
{
		public CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

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

		public override string GetDescription(EventInstanceBase ev)
	{
		if ((ev as CellEventInstance).data == 1)
		{
			return base.GetMessagePrefix() + "Solid=true (" + this.reason + ")";
		}
		return base.GetMessagePrefix() + "Solid=false (" + this.reason + ")";
	}
}
