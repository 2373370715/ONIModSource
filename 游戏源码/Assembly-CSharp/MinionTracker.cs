using System;

// Token: 0x02000B50 RID: 2896
public abstract class MinionTracker : Tracker
{
	// Token: 0x060036E6 RID: 14054 RVA: 0x000C3A56 File Offset: 0x000C1C56
	public MinionTracker(MinionIdentity identity)
	{
		this.identity = identity;
	}

	// Token: 0x04002532 RID: 9522
	public MinionIdentity identity;
}
