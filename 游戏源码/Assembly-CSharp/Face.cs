using System;

// Token: 0x020012DB RID: 4827
public class Face : Resource
{
	// Token: 0x06006316 RID: 25366 RVA: 0x000E0B91 File Offset: 0x000DED91
	public Face(string id, string headFXSymbol = null) : base(id, null, null)
	{
		this.hash = new HashedString(id);
		this.headFXHash = headFXSymbol;
	}

	// Token: 0x040046B0 RID: 18096
	public HashedString hash;

	// Token: 0x040046B1 RID: 18097
	public HashedString headFXHash;

	// Token: 0x040046B2 RID: 18098
	private const string SYMBOL_PREFIX = "headfx_";
}
