using System;
using System.Collections.Generic;

// Token: 0x020017EF RID: 6127
public class RoleSlotUnlock
{
	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06007E2C RID: 32300 RVA: 0x000F32AA File Offset: 0x000F14AA
	// (set) Token: 0x06007E2D RID: 32301 RVA: 0x000F32B2 File Offset: 0x000F14B2
	public string id { get; protected set; }

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x06007E2E RID: 32302 RVA: 0x000F32BB File Offset: 0x000F14BB
	// (set) Token: 0x06007E2F RID: 32303 RVA: 0x000F32C3 File Offset: 0x000F14C3
	public string name { get; protected set; }

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x06007E30 RID: 32304 RVA: 0x000F32CC File Offset: 0x000F14CC
	// (set) Token: 0x06007E31 RID: 32305 RVA: 0x000F32D4 File Offset: 0x000F14D4
	public string description { get; protected set; }

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x06007E32 RID: 32306 RVA: 0x000F32DD File Offset: 0x000F14DD
	// (set) Token: 0x06007E33 RID: 32307 RVA: 0x000F32E5 File Offset: 0x000F14E5
	public List<global::Tuple<string, int>> slots { get; protected set; }

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x06007E34 RID: 32308 RVA: 0x000F32EE File Offset: 0x000F14EE
	// (set) Token: 0x06007E35 RID: 32309 RVA: 0x000F32F6 File Offset: 0x000F14F6
	public Func<bool> isSatisfied { get; protected set; }

	// Token: 0x06007E36 RID: 32310 RVA: 0x000F32FF File Offset: 0x000F14FF
	public RoleSlotUnlock(string id, string name, string description, List<global::Tuple<string, int>> slots, Func<bool> isSatisfied)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.slots = slots;
		this.isSatisfied = isSatisfied;
	}
}
