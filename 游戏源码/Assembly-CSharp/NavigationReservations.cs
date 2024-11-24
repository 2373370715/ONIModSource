using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A9F RID: 2719
[AddComponentMenu("KMonoBehaviour/scripts/NavigationReservations")]
public class NavigationReservations : KMonoBehaviour
{
	// Token: 0x0600327B RID: 12923 RVA: 0x000C0CBC File Offset: 0x000BEEBC
	public static void DestroyInstance()
	{
		NavigationReservations.Instance = null;
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x000C0CC4 File Offset: 0x000BEEC4
	public int GetOccupancyCount(int cell)
	{
		if (this.cellOccupancyDensity.ContainsKey(cell))
		{
			return this.cellOccupancyDensity[cell];
		}
		return 0;
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x00203BC0 File Offset: 0x00201DC0
	public void AddOccupancy(int cell)
	{
		if (!this.cellOccupancyDensity.ContainsKey(cell))
		{
			this.cellOccupancyDensity.Add(cell, 1);
			return;
		}
		Dictionary<int, int> dictionary = this.cellOccupancyDensity;
		dictionary[cell]++;
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x00203C04 File Offset: 0x00201E04
	public void RemoveOccupancy(int cell)
	{
		int num = 0;
		if (this.cellOccupancyDensity.TryGetValue(cell, out num))
		{
			if (num == 1)
			{
				this.cellOccupancyDensity.Remove(cell);
				return;
			}
			this.cellOccupancyDensity[cell] = num - 1;
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x000C0CE2 File Offset: 0x000BEEE2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NavigationReservations.Instance = this;
	}

	// Token: 0x040021E4 RID: 8676
	public static NavigationReservations Instance;

	// Token: 0x040021E5 RID: 8677
	public static int InvalidReservation = -1;

	// Token: 0x040021E6 RID: 8678
	private Dictionary<int, int> cellOccupancyDensity = new Dictionary<int, int>();
}
