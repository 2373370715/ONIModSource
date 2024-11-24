using System;

// Token: 0x020012CF RID: 4815
public class EventBase : Resource
{
	// Token: 0x060062E3 RID: 25315 RVA: 0x000E0908 File Offset: 0x000DEB08
	public EventBase(string id) : base(id, id)
	{
		this.hash = Hash.SDBMLower(id);
	}

	// Token: 0x060062E4 RID: 25316 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public virtual string GetDescription(EventInstanceBase ev)
	{
		return "";
	}

	// Token: 0x0400468F RID: 18063
	public int hash;
}
