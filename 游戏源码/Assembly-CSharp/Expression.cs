using System;
using System.Diagnostics;

// Token: 0x020012D9 RID: 4825
[DebuggerDisplay("{face.hash} {priority}")]
public class Expression : Resource
{
	// Token: 0x06006314 RID: 25364 RVA: 0x000E0B7F File Offset: 0x000DED7F
	public Expression(string id, ResourceSet parent, Face face) : base(id, parent, null)
	{
		this.face = face;
	}

	// Token: 0x040046AE RID: 18094
	public Face face;

	// Token: 0x040046AF RID: 18095
	public int priority;
}
