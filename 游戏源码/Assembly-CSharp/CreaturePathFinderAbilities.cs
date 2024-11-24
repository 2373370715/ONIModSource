using System;
using Klei.AI;

// Token: 0x0200063C RID: 1596
public class CreaturePathFinderAbilities : PathFinderAbilities
{
	// Token: 0x06001D10 RID: 7440 RVA: 0x000B326B File Offset: 0x000B146B
	public CreaturePathFinderAbilities(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x001ADF0C File Offset: 0x001AC10C
	protected override void Refresh(Navigator navigator)
	{
		if (PathFinder.IsSubmerged(Grid.PosToCell(navigator)))
		{
			this.canTraverseSubmered = true;
			return;
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.MaxUnderwaterTravelCost.Lookup(navigator);
		this.canTraverseSubmered = (attributeInstance == null);
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000B3274 File Offset: 0x000B1474
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		return !submerged || this.canTraverseSubmered;
	}

	// Token: 0x04001219 RID: 4633
	public bool canTraverseSubmered;
}
