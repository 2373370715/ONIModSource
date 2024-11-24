using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC0 RID: 2752
[AddComponentMenu("KMonoBehaviour/scripts/Pathfinding")]
public class Pathfinding : KMonoBehaviour
{
	// Token: 0x0600333D RID: 13117 RVA: 0x000C17B4 File Offset: 0x000BF9B4
	public static void DestroyInstance()
	{
		Pathfinding.Instance = null;
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x000C17C1 File Offset: 0x000BF9C1
	protected override void OnPrefabInit()
	{
		Pathfinding.Instance = this;
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x000C17C9 File Offset: 0x000BF9C9
	public void AddNavGrid(NavGrid nav_grid)
	{
		this.NavGrids.Add(nav_grid);
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x00205650 File Offset: 0x00203850
	public NavGrid GetNavGrid(string id)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			if (navGrid.id == id)
			{
				return navGrid;
			}
		}
		global::Debug.LogError("Could not find nav grid: " + id);
		return null;
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x000C17D7 File Offset: 0x000BF9D7
	public List<NavGrid> GetNavGrids()
	{
		return this.NavGrids;
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x002056C4 File Offset: 0x002038C4
	public void ResetNavGrids()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.InitializeGraph();
		}
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x000C17DF File Offset: 0x000BF9DF
	public void FlushNavGridsOnLoad()
	{
		if (this.navGridsHaveBeenFlushedOnLoad)
		{
			return;
		}
		this.navGridsHaveBeenFlushedOnLoad = true;
		this.UpdateNavGrids(true);
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x00205714 File Offset: 0x00203914
	public void UpdateNavGrids(bool update_all = false)
	{
		update_all = true;
		if (update_all)
		{
			using (List<NavGrid>.Enumerator enumerator = this.NavGrids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NavGrid navGrid = enumerator.Current;
					navGrid.UpdateGraph();
				}
				return;
			}
		}
		foreach (NavGrid navGrid2 in this.NavGrids)
		{
			if (navGrid2.updateEveryFrame)
			{
				navGrid2.UpdateGraph();
			}
		}
		this.NavGrids[this.UpdateIdx].UpdateGraph();
		this.UpdateIdx = (this.UpdateIdx + 1) % this.NavGrids.Count;
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x002057E4 File Offset: 0x002039E4
	public void RenderEveryTick()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.DebugUpdate();
		}
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x00205834 File Offset: 0x00203A34
	public void AddDirtyNavGridCell(int cell)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.AddDirtyCell(cell);
		}
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x00205888 File Offset: 0x00203A88
	public void RefreshNavCell(int cell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		hashSet.Add(cell);
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.UpdateGraph(hashSet);
		}
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x000C17F8 File Offset: 0x000BF9F8
	protected override void OnCleanUp()
	{
		this.NavGrids.Clear();
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x04002281 RID: 8833
	private List<NavGrid> NavGrids = new List<NavGrid>();

	// Token: 0x04002282 RID: 8834
	private int UpdateIdx;

	// Token: 0x04002283 RID: 8835
	private bool navGridsHaveBeenFlushedOnLoad;

	// Token: 0x04002284 RID: 8836
	public static Pathfinding Instance;
}
