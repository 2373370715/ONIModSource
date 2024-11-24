using System;

// Token: 0x02000CF9 RID: 3321
[Serializable]
public class ConduitPortInfo
{
	// Token: 0x060040D6 RID: 16598 RVA: 0x000C9FAA File Offset: 0x000C81AA
	public ConduitPortInfo(ConduitType type, CellOffset offset)
	{
		this.conduitType = type;
		this.offset = offset;
	}

	// Token: 0x04002C4D RID: 11341
	public ConduitType conduitType;

	// Token: 0x04002C4E RID: 11342
	public CellOffset offset;
}
