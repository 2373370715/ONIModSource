using System;
using System.Diagnostics;

// Token: 0x0200168B RID: 5771
[DebuggerDisplay("{slot.Id}")]
public class OwnableSlotInstance : AssignableSlotInstance
{
	// Token: 0x06007730 RID: 30512 RVA: 0x000E038E File Offset: 0x000DE58E
	public OwnableSlotInstance(Assignables assignables, OwnableSlot slot) : base(assignables, slot)
	{
	}
}
