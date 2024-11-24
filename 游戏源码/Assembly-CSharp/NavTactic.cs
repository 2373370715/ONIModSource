using System;
using UnityEngine;

// Token: 0x02000AA1 RID: 2721
public class NavTactic
{
	// Token: 0x06003283 RID: 12931 RVA: 0x000C0D3C File Offset: 0x000BEF3C
	public NavTactic(int preferredRange, int rangePenalty = 1, int overlapPenalty = 1, int pathCostPenalty = 1)
	{
		this._overlapPenalty = overlapPenalty;
		this._preferredRange = preferredRange;
		this._rangePenalty = rangePenalty;
		this._pathCostPenalty = pathCostPenalty;
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x00203C44 File Offset: 0x00201E44
	public int GetCellPreferences(int root, CellOffset[] offsets, Navigator navigator)
	{
		int result = NavigationReservations.InvalidReservation;
		int num = int.MaxValue;
		for (int i = 0; i < offsets.Length; i++)
		{
			int num2 = Grid.OffsetCell(root, offsets[i]);
			int num3 = 0;
			num3 += this._overlapPenalty * NavigationReservations.Instance.GetOccupancyCount(num2);
			num3 += this._rangePenalty * Mathf.Abs(this._preferredRange - Grid.GetCellDistance(root, num2));
			num3 += this._pathCostPenalty * Mathf.Max(navigator.GetNavigationCost(num2), 0);
			if (num3 < num && navigator.CanReach(num2))
			{
				num = num3;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x040021EA RID: 8682
	private int _overlapPenalty = 3;

	// Token: 0x040021EB RID: 8683
	private int _preferredRange;

	// Token: 0x040021EC RID: 8684
	private int _rangePenalty = 2;

	// Token: 0x040021ED RID: 8685
	private int _pathCostPenalty = 1;
}
