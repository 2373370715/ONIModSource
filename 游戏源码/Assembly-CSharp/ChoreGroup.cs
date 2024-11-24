using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;

// Token: 0x02000B88 RID: 2952
[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06003861 RID: 14433 RVA: 0x000C4834 File Offset: 0x000C2A34
	public int DefaultPersonalPriority
	{
		get
		{
			return this.defaultPersonalPriority;
		}
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x0021AEB0 File Offset: 0x002190B0
	public ChoreGroup(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true) : base(id, name)
	{
		this.attribute = attribute;
		this.description = Strings.Get("STRINGS.DUPLICANTS.CHOREGROUPS." + id.ToUpper() + ".DESC").String;
		this.sprite = sprite;
		this.defaultPersonalPriority = default_personal_priority;
		this.userPrioritizable = user_prioritizable;
	}

	// Token: 0x04002667 RID: 9831
	public List<ChoreType> choreTypes = new List<ChoreType>();

	// Token: 0x04002668 RID: 9832
	public Klei.AI.Attribute attribute;

	// Token: 0x04002669 RID: 9833
	public string description;

	// Token: 0x0400266A RID: 9834
	public string sprite;

	// Token: 0x0400266B RID: 9835
	private int defaultPersonalPriority;

	// Token: 0x0400266C RID: 9836
	public bool userPrioritizable;
}
