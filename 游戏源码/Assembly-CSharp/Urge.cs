using System;

// Token: 0x02000787 RID: 1927
public class Urge : Resource
{
	// Token: 0x060022AC RID: 8876 RVA: 0x000B68D8 File Offset: 0x000B4AD8
	public Urge(string id) : base(id, null, null)
	{
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000B68E3 File Offset: 0x000B4AE3
	public override string ToString()
	{
		return this.Id;
	}
}
