using System;

// Token: 0x02001860 RID: 6240
public class Shirt : Resource
{
	// Token: 0x060080FB RID: 33019 RVA: 0x000F4DB9 File Offset: 0x000F2FB9
	public Shirt(string id) : base(id, null, null)
	{
		this.hash = new HashedString(id);
	}

	// Token: 0x040061C7 RID: 25031
	public HashedString hash;
}
