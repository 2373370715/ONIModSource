using System;
using KSerialization;

// Token: 0x020012CA RID: 4810
[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
	// Token: 0x060062D4 RID: 25300 RVA: 0x000E08AB File Offset: 0x000DEAAB
	public CellEventInstance(int cell, int data, int data2, CellEvent ev) : base(ev)
	{
		this.cell = cell;
		this.data = data;
		this.data2 = data2;
	}

	// Token: 0x0400464C RID: 17996
	[Serialize]
	public int cell;

	// Token: 0x0400464D RID: 17997
	[Serialize]
	public int data;

	// Token: 0x0400464E RID: 17998
	[Serialize]
	public int data2;
}
