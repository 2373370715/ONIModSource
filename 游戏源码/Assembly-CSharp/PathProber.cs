using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F4 RID: 2036
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/PathProber")]
public class PathProber : KMonoBehaviour
{
	// Token: 0x06002468 RID: 9320 RVA: 0x000B7A80 File Offset: 0x000B5C80
	protected override void OnCleanUp()
	{
		if (this.PathGrid != null)
		{
			this.PathGrid.OnCleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x000B7A9B File Offset: 0x000B5C9B
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.PathGrid.SetGroupProber(group_prober);
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000B7AA9 File Offset: 0x000B5CA9
	public void SetValidNavTypes(NavType[] nav_types, int max_probing_radius)
	{
		if (max_probing_radius != 0)
		{
			this.PathGrid = new PathGrid(max_probing_radius * 2, max_probing_radius * 2, true, nav_types);
			return;
		}
		this.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, nav_types);
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000B7AD9 File Offset: 0x000B5CD9
	public int GetCost(int cell)
	{
		return this.PathGrid.GetCost(cell);
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000B7AE7 File Offset: 0x000B5CE7
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathGrid.GetCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x000B7AF6 File Offset: 0x000B5CF6
	public PathGrid GetPathGrid()
	{
		return this.PathGrid;
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x001C99A0 File Offset: 0x001C7BA0
	public void UpdateProbe(NavGrid nav_grid, int cell, NavType nav_type, PathFinderAbilities abilities, PathFinder.PotentialPath.Flags flags)
	{
		if (this.scratchPad == null)
		{
			this.scratchPad = new PathFinder.PotentialScratchPad(nav_grid.maxLinksPerCell);
		}
		bool flag = this.updateCount == -1;
		bool flag2 = this.Potentials.Count == 0 || flag;
		this.PathGrid.BeginUpdate(cell, !flag2);
		if (flag2)
		{
			this.updateCount = 0;
			bool flag3;
			PathFinder.Cell cell2 = this.PathGrid.GetCell(cell, nav_type, out flag3);
			PathFinder.AddPotential(new PathFinder.PotentialPath(cell, nav_type, flags), Grid.InvalidCell, NavType.NumNavTypes, 0, 0, this.Potentials, this.PathGrid, ref cell2);
		}
		int num = (this.potentialCellsPerUpdate <= 0 || flag) ? int.MaxValue : this.potentialCellsPerUpdate;
		this.updateCount++;
		while (this.Potentials.Count > 0 && num > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = this.Potentials.Next();
			num--;
			bool flag3;
			PathFinder.Cell cell3 = this.PathGrid.GetCell(keyValuePair.Value, out flag3);
			if (cell3.cost == keyValuePair.Key)
			{
				PathFinder.AddPotentials(this.scratchPad, keyValuePair.Value, cell3.cost, ref abilities, null, nav_grid.maxLinksPerCell, nav_grid.Links, this.Potentials, this.PathGrid, cell3.parent, cell3.parentNavType);
			}
		}
		bool flag4 = this.Potentials.Count == 0;
		this.PathGrid.EndUpdate(flag4);
		if (flag4)
		{
			int num2 = this.updateCount;
		}
	}

	// Token: 0x0400187D RID: 6269
	public const int InvalidHandle = -1;

	// Token: 0x0400187E RID: 6270
	public const int InvalidIdx = -1;

	// Token: 0x0400187F RID: 6271
	public const int InvalidCell = -1;

	// Token: 0x04001880 RID: 6272
	public const int InvalidCost = -1;

	// Token: 0x04001881 RID: 6273
	private PathGrid PathGrid;

	// Token: 0x04001882 RID: 6274
	private PathFinder.PotentialList Potentials = new PathFinder.PotentialList();

	// Token: 0x04001883 RID: 6275
	public int updateCount = -1;

	// Token: 0x04001884 RID: 6276
	private const int updateCountThreshold = 25;

	// Token: 0x04001885 RID: 6277
	private PathFinder.PotentialScratchPad scratchPad;

	// Token: 0x04001886 RID: 6278
	public int potentialCellsPerUpdate = -1;
}
