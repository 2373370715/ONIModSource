using System;
using KSerialization;

// Token: 0x020012D0 RID: 4816
[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
	// Token: 0x060062E5 RID: 25317 RVA: 0x000E091E File Offset: 0x000DEB1E
	public EventInstanceBase(EventBase ev)
	{
		this.frame = GameClock.Instance.GetFrame();
		this.eventHash = ev.hash;
		this.ev = ev;
	}

	// Token: 0x060062E6 RID: 25318 RVA: 0x002B82F8 File Offset: 0x002B64F8
	public override string ToString()
	{
		string str = "[" + this.frame.ToString() + "] ";
		if (this.ev != null)
		{
			return str + this.ev.GetDescription(this);
		}
		return str + "Unknown event";
	}

	// Token: 0x04004690 RID: 18064
	[Serialize]
	public int frame;

	// Token: 0x04004691 RID: 18065
	[Serialize]
	public int eventHash;

	// Token: 0x04004692 RID: 18066
	public EventBase ev;
}
