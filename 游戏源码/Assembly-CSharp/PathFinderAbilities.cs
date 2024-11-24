using System;

// Token: 0x020007F0 RID: 2032
public abstract class PathFinderAbilities
{
	// Token: 0x0600244F RID: 9295 RVA: 0x000B79B8 File Offset: 0x000B5BB8
	public PathFinderAbilities(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x06002450 RID: 9296 RVA: 0x000B79C7 File Offset: 0x000B5BC7
	public void Refresh()
	{
		this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
		this.Refresh(this.navigator);
	}

	// Token: 0x06002451 RID: 9297
	protected abstract void Refresh(Navigator navigator);

	// Token: 0x06002452 RID: 9298
	public abstract bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged);

	// Token: 0x06002453 RID: 9299 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		return 0;
	}

	// Token: 0x04001868 RID: 6248
	protected Navigator navigator;

	// Token: 0x04001869 RID: 6249
	protected int prefabInstanceID;
}
