using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000B8F RID: 2959
[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06003894 RID: 14484 RVA: 0x000C49B7 File Offset: 0x000C2BB7
	// (set) Token: 0x06003895 RID: 14485 RVA: 0x000C49BF File Offset: 0x000C2BBF
	public Color color { get; private set; }

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06003896 RID: 14486 RVA: 0x000C49C8 File Offset: 0x000C2BC8
	// (set) Token: 0x06003897 RID: 14487 RVA: 0x000C49D0 File Offset: 0x000C2BD0
	public string description { get; private set; }

	// Token: 0x06003898 RID: 14488 RVA: 0x000C49D9 File Offset: 0x000C2BD9
	public ScheduleBlockType(string id, ResourceSet parent, string name, string description, Color color) : base(id, parent, name)
	{
		this.color = color;
		this.description = description;
	}
}
