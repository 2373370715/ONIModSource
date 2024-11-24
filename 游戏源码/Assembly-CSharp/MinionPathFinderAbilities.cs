using System;
using System.Diagnostics;

// Token: 0x0200063D RID: 1597
public class MinionPathFinderAbilities : PathFinderAbilities
{
	// Token: 0x06001D13 RID: 7443 RVA: 0x001ADF50 File Offset: 0x001AC150
	public MinionPathFinderAbilities(Navigator navigator) : base(navigator)
	{
		this.transitionVoidOffsets = new CellOffset[navigator.NavGrid.transitions.Length][];
		for (int i = 0; i < this.transitionVoidOffsets.Length; i++)
		{
			this.transitionVoidOffsets[i] = navigator.NavGrid.transitions[i].voidOffsets;
		}
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000B3285 File Offset: 0x000B1485
	protected override void Refresh(Navigator navigator)
	{
		this.proxyID = navigator.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<KPrefabID>().InstanceID;
		this.out_of_fuel = navigator.HasTag(GameTags.JetSuitOutOfFuel);
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000B32B8 File Offset: 0x000B14B8
	public void SetIdleNavMaskEnabled(bool enabled)
	{
		this.idleNavMaskEnabled = enabled;
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x000B32C1 File Offset: 0x000B14C1
	private static bool IsAccessPermitted(int proxyID, int cell, int from_cell, NavType from_nav_type)
	{
		return Grid.HasPermission(cell, proxyID, from_cell, from_nav_type);
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000B32CC File Offset: 0x000B14CC
	public override int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		if (!path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasLeadSuit))
		{
			return (int)(link.cost * 2);
		}
		return 0;
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x001ADFB0 File Offset: 0x001AC1B0
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, path.cell, from_cell, from_nav_type))
		{
			return false;
		}
		foreach (CellOffset offset in this.transitionVoidOffsets[transition_id])
		{
			int cell = Grid.OffsetCell(from_cell, offset);
			if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, cell, from_cell, from_nav_type))
			{
				return false;
			}
		}
		if (path.navType == NavType.Tube && from_nav_type == NavType.Floor && !Grid.HasUsableTubeEntrance(from_cell, this.prefabInstanceID))
		{
			return false;
		}
		if (path.navType == NavType.Hover && (this.out_of_fuel || !path.HasFlag(PathFinder.PotentialPath.Flags.HasJetPack)))
		{
			return false;
		}
		Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
		PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
		bool flag = path.HasFlag(PathFinder.PotentialPath.Flags.PerformSuitChecks) && Grid.TryGetSuitMarkerFlags(from_cell, out flags, out flags2) && (flags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		bool flag2 = SuitMarker.DoesTraversalDirectionRequireSuit(from_cell, path.cell, flags);
		bool flag3 = path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
		if (flag)
		{
			bool flag4 = path.HasFlag(flags2);
			if (flag2)
			{
				if (!flag3 && !Grid.HasSuit(from_cell, this.prefabInstanceID))
				{
					return false;
				}
			}
			else if (flag3 && (flags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != (Grid.SuitMarker.Flags)0 && (!flag4 || !Grid.HasEmptyLocker(from_cell, this.prefabInstanceID)))
			{
				return false;
			}
		}
		if (this.idleNavMaskEnabled && (Grid.PreventIdleTraversal[path.cell] || Grid.PreventIdleTraversal[from_cell]))
		{
			return false;
		}
		if (flag)
		{
			if (flag2)
			{
				if (!flag3)
				{
					path.SetFlags(flags2);
				}
			}
			else
			{
				path.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
			}
		}
		return true;
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void BeginSample(string region_name)
	{
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void EndSample(string region_name)
	{
	}

	// Token: 0x0400121A RID: 4634
	private CellOffset[][] transitionVoidOffsets;

	// Token: 0x0400121B RID: 4635
	private int proxyID;

	// Token: 0x0400121C RID: 4636
	private bool out_of_fuel;

	// Token: 0x0400121D RID: 4637
	private bool idleNavMaskEnabled;
}
