using System;

// Token: 0x020007DD RID: 2013
public class NavMask
{
	// Token: 0x06002406 RID: 9222 RVA: 0x000A65EC File Offset: 0x000A47EC
	public virtual bool IsTraversable(PathFinder.PotentialPath path, int from_cell, int cost, int transition_id, PathFinderAbilities abilities)
	{
		return true;
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void ApplyTraversalToPath(ref PathFinder.PotentialPath path, int from_cell)
	{
	}
}
