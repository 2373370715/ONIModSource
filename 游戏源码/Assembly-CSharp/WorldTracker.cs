using System;

// Token: 0x02000B4F RID: 2895
public abstract class WorldTracker : Tracker
{
	// Token: 0x17000261 RID: 609
	// (get) Token: 0x060036E3 RID: 14051 RVA: 0x000C3A36 File Offset: 0x000C1C36
	// (set) Token: 0x060036E4 RID: 14052 RVA: 0x000C3A3E File Offset: 0x000C1C3E
	public int WorldID { get; private set; }

	// Token: 0x060036E5 RID: 14053 RVA: 0x000C3A47 File Offset: 0x000C1C47
	public WorldTracker(int worldID)
	{
		this.WorldID = worldID;
	}
}
