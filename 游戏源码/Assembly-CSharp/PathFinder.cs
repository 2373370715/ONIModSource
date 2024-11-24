using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x020007E1 RID: 2017
public class PathFinder
{
	// Token: 0x06002413 RID: 9235 RVA: 0x001C8ABC File Offset: 0x001C6CBC
	public static void Initialize()
	{
		NavType[] array = new NavType[11];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (NavType)i;
		}
		PathFinder.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, array);
		for (int j = 0; j < Grid.CellCount; j++)
		{
			if (Grid.Visible[j] > 0 || Grid.Spawnable[j] > 0)
			{
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				GameUtil.FloodFillConditional(j, PathFinder.allowPathfindingFloodFillCb, pooledList, null);
				Grid.AllowPathfinding[j] = true;
				pooledList.Recycle();
			}
		}
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(PathFinder.OnReveal));
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000A5E40 File Offset: 0x000A4040
	private static void OnReveal(int cell)
	{
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000B7683 File Offset: 0x000B5883
	public static void UpdatePath(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query, ref path);
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x001C8B64 File Offset: 0x001C6D64
	public static bool ValidatePath(NavGrid nav_grid, PathFinderAbilities abilities, ref PathFinder.Path path)
	{
		if (!path.IsValid())
		{
			return false;
		}
		for (int i = 0; i < path.nodes.Count; i++)
		{
			PathFinder.Path.Node node = path.nodes[i];
			if (i < path.nodes.Count - 1)
			{
				PathFinder.Path.Node node2 = path.nodes[i + 1];
				int num = node.cell * nav_grid.maxLinksPerCell;
				bool flag = false;
				NavGrid.Link link = nav_grid.Links[num];
				while (link.link != PathFinder.InvalidHandle)
				{
					if (link.link == node2.cell && node2.navType == link.endNavType && node.navType == link.startNavType)
					{
						PathFinder.PotentialPath potentialPath = new PathFinder.PotentialPath(node.cell, node.navType, PathFinder.PotentialPath.Flags.None);
						flag = abilities.TraversePath(ref potentialPath, node.cell, node.navType, 0, (int)link.transitionId, false);
						if (flag)
						{
							break;
						}
					}
					num++;
					link = nav_grid.Links[num];
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x001C8C78 File Offset: 0x001C6E78
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query)
	{
		int invalidCell = PathFinder.InvalidCell;
		NavType nav_type = NavType.NumNavTypes;
		query.ClearResult();
		if (!Grid.IsValidCell(potential_path.cell))
		{
			return;
		}
		PathFinder.FindPaths(nav_grid, ref abilities, potential_path, query, PathFinder.Temp.Potentials, ref invalidCell, ref nav_type);
		if (invalidCell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(invalidCell, nav_type, out flag);
			query.SetResult(invalidCell, cell.cost, nav_type);
		}
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000B7690 File Offset: 0x000B5890
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query);
		if (query.GetResultCell() != PathFinder.InvalidCell)
		{
			PathFinder.BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
			return;
		}
		path.Clear();
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x001C8CDC File Offset: 0x001C6EDC
	private static void BuildResultPath(int path_cell, NavType path_nav_type, ref PathFinder.Path path)
	{
		if (path_cell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(path_cell, path_nav_type, out flag);
			path.Clear();
			path.cost = cell.cost;
			while (path_cell != PathFinder.InvalidCell)
			{
				path.AddNode(new PathFinder.Path.Node
				{
					cell = path_cell,
					navType = cell.navType,
					transitionId = cell.transitionId
				});
				path_cell = cell.parent;
				if (path_cell != PathFinder.InvalidCell)
				{
					cell = PathFinder.PathGrid.GetCell(path_cell, cell.parentNavType, out flag);
				}
			}
			if (path.nodes != null)
			{
				for (int i = 0; i < path.nodes.Count / 2; i++)
				{
					PathFinder.Path.Node value = path.nodes[i];
					path.nodes[i] = path.nodes[path.nodes.Count - i - 1];
					path.nodes[path.nodes.Count - i - 1] = value;
				}
			}
		}
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x001C8DE8 File Offset: 0x001C6FE8
	private static void FindPaths(NavGrid nav_grid, ref PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, PathFinder.PotentialList potentials, ref int result_cell, ref NavType result_nav_type)
	{
		potentials.Clear();
		PathFinder.PathGrid.ResetUpdate();
		PathFinder.PathGrid.BeginUpdate(potential_path.cell, false);
		bool flag;
		PathFinder.Cell cell = PathFinder.PathGrid.GetCell(potential_path, out flag);
		PathFinder.AddPotential(potential_path, Grid.InvalidCell, NavType.NumNavTypes, 0, 0, potentials, PathFinder.PathGrid, ref cell);
		int num = int.MaxValue;
		while (potentials.Count > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
			cell = PathFinder.PathGrid.GetCell(keyValuePair.Value, out flag);
			if (cell.cost == keyValuePair.Key)
			{
				if (cell.navType != NavType.Tube && query.IsMatch(keyValuePair.Value.cell, cell.parent, cell.cost) && cell.cost < num)
				{
					result_cell = keyValuePair.Value.cell;
					num = cell.cost;
					result_nav_type = cell.navType;
					break;
				}
				PathFinder.AddPotentials(nav_grid.potentialScratchPad, keyValuePair.Value, cell.cost, ref abilities, query, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, PathFinder.PathGrid, cell.parent, cell.parentNavType);
			}
		}
		PathFinder.PathGrid.EndUpdate(true);
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000B76C3 File Offset: 0x000B58C3
	public static void AddPotential(PathFinder.PotentialPath potential_path, int parent_cell, NavType parent_nav_type, int cost, byte transition_id, PathFinder.PotentialList potentials, PathGrid path_grid, ref PathFinder.Cell cell_data)
	{
		cell_data.cost = cost;
		cell_data.parent = parent_cell;
		cell_data.SetNavTypes(potential_path.navType, parent_nav_type);
		cell_data.transitionId = transition_id;
		potentials.Add(cost, potential_path);
		path_grid.SetCell(potential_path, ref cell_data);
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void BeginDetailSample(string region_name)
	{
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void EndDetailSample(string region_name)
	{
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x001C8F24 File Offset: 0x001C7124
	public static bool IsSubmerged(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num = Grid.CellAbove(cell);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsLiquid) || (Grid.Element[cell].IsLiquid && Grid.IsValidCell(num) && Grid.Solid[num]);
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x001C8F80 File Offset: 0x001C7180
	public static void AddPotentials(PathFinder.PotentialScratchPad potential_scratch_pad, PathFinder.PotentialPath potential, int cost, ref PathFinderAbilities abilities, PathFinderQuery query, int max_links_per_cell, NavGrid.Link[] links, PathFinder.PotentialList potentials, PathGrid path_grid, int parent_cell, NavType parent_nav_type)
	{
		if (!Grid.IsValidCell(potential.cell))
		{
			return;
		}
		int num = 0;
		NavGrid.Link[] linksWithCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
		int num2 = potential.cell * max_links_per_cell;
		NavGrid.Link link = links[num2];
		for (int link2 = link.link; link2 != PathFinder.InvalidHandle; link2 = link.link)
		{
			if (link.startNavType == potential.navType && (parent_cell != link2 || parent_nav_type != link.startNavType))
			{
				linksWithCorrectNavType[num++] = link;
			}
			num2++;
			link = links[num2];
		}
		int num3 = 0;
		PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange = potential_scratch_pad.linksInCellRange;
		for (int i = 0; i < num; i++)
		{
			NavGrid.Link link3 = linksWithCorrectNavType[i];
			int link4 = link3.link;
			bool flag = false;
			PathFinder.Cell cell = path_grid.GetCell(link4, link3.endNavType, out flag);
			if (flag)
			{
				int num4 = cost + (int)link3.cost;
				bool flag2 = cell.cost == -1;
				bool flag3 = num4 < cell.cost;
				if (flag2 || flag3)
				{
					linksInCellRange[num3++] = new PathFinder.PotentialScratchPad.PathGridCellData
					{
						pathGridCell = cell,
						link = link3
					};
				}
			}
		}
		for (int j = 0; j < num3; j++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[j];
			int link5 = pathGridCellData.link.link;
			pathGridCellData.isSubmerged = PathFinder.IsSubmerged(link5);
			linksInCellRange[j] = pathGridCellData;
		}
		for (int k = 0; k < num3; k++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData2 = linksInCellRange[k];
			NavGrid.Link link6 = pathGridCellData2.link;
			int link7 = link6.link;
			PathFinder.Cell pathGridCell = pathGridCellData2.pathGridCell;
			int num5 = cost + (int)link6.cost;
			PathFinder.PotentialPath potentialPath = potential;
			potentialPath.cell = link7;
			potentialPath.navType = link6.endNavType;
			if (pathGridCellData2.isSubmerged)
			{
				int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(potentialPath, link6);
				num5 += submergedPathCostPenalty;
			}
			PathFinder.PotentialPath.Flags flags = potentialPath.flags;
			bool flag4 = abilities.TraversePath(ref potentialPath, potential.cell, potential.navType, num5, (int)link6.transitionId, pathGridCellData2.isSubmerged);
			PathFinder.PotentialPath.Flags flags2 = potentialPath.flags;
			if (flag4)
			{
				PathFinder.AddPotential(potentialPath, potential.cell, potential.navType, num5, link6.transitionId, potentials, path_grid, ref pathGridCell);
			}
		}
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x000B76FF File Offset: 0x000B58FF
	public static void DestroyStatics()
	{
		PathFinder.PathGrid.OnCleanUp();
		PathFinder.PathGrid = null;
		PathFinder.Temp.Potentials.Clear();
	}

	// Token: 0x04001827 RID: 6183
	public static int InvalidHandle = -1;

	// Token: 0x04001828 RID: 6184
	public static int InvalidIdx = -1;

	// Token: 0x04001829 RID: 6185
	public static int InvalidCell = -1;

	// Token: 0x0400182A RID: 6186
	public static PathGrid PathGrid;

	// Token: 0x0400182B RID: 6187
	private static readonly Func<int, bool> allowPathfindingFloodFillCb = delegate(int cell)
	{
		if (Grid.Solid[cell])
		{
			return false;
		}
		if (Grid.AllowPathfinding[cell])
		{
			return false;
		}
		Grid.AllowPathfinding[cell] = true;
		return true;
	};

	// Token: 0x020007E2 RID: 2018
	public struct Cell
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x000B7744 File Offset: 0x000B5944
		public NavType navType
		{
			get
			{
				return (NavType)(this.navTypes & 15);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000B7750 File Offset: 0x000B5950
		public NavType parentNavType
		{
			get
			{
				return (NavType)(this.navTypes >> 4);
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x001C91CC File Offset: 0x001C73CC
		public void SetNavTypes(NavType type, NavType parent_type)
		{
			this.navTypes = (byte)(type | parent_type << 4);
		}

		// Token: 0x0400182C RID: 6188
		public int cost;

		// Token: 0x0400182D RID: 6189
		public int parent;

		// Token: 0x0400182E RID: 6190
		public short queryId;

		// Token: 0x0400182F RID: 6191
		private byte navTypes;

		// Token: 0x04001830 RID: 6192
		public byte transitionId;
	}

	// Token: 0x020007E3 RID: 2019
	public struct PotentialPath
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x000B775B File Offset: 0x000B595B
		public PotentialPath(int cell, NavType nav_type, PathFinder.PotentialPath.Flags flags)
		{
			this.cell = cell;
			this.navType = nav_type;
			this.flags = flags;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000B7772 File Offset: 0x000B5972
		public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags |= new_flags;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000B7782 File Offset: 0x000B5982
		public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags &= ~new_flags;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000B7794 File Offset: 0x000B5994
		public bool HasFlag(PathFinder.PotentialPath.Flags flag)
		{
			return this.HasAnyFlag(flag);
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000B779D File Offset: 0x000B599D
		public bool HasAnyFlag(PathFinder.PotentialPath.Flags mask)
		{
			return (this.flags & mask) > PathFinder.PotentialPath.Flags.None;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x000B77AA File Offset: 0x000B59AA
		// (set) Token: 0x0600242C RID: 9260 RVA: 0x000B77B2 File Offset: 0x000B59B2
		public PathFinder.PotentialPath.Flags flags { readonly get; private set; }

		// Token: 0x04001831 RID: 6193
		public int cell;

		// Token: 0x04001832 RID: 6194
		public NavType navType;

		// Token: 0x020007E4 RID: 2020
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x04001835 RID: 6197
			None = 0,
			// Token: 0x04001836 RID: 6198
			HasAtmoSuit = 1,
			// Token: 0x04001837 RID: 6199
			HasJetPack = 2,
			// Token: 0x04001838 RID: 6200
			HasOxygenMask = 4,
			// Token: 0x04001839 RID: 6201
			PerformSuitChecks = 8,
			// Token: 0x0400183A RID: 6202
			HasLeadSuit = 16
		}
	}

	// Token: 0x020007E5 RID: 2021
	public struct Path
	{
		// Token: 0x0600242D RID: 9261 RVA: 0x000B77BB File Offset: 0x000B59BB
		public void AddNode(PathFinder.Path.Node node)
		{
			if (this.nodes == null)
			{
				this.nodes = new List<PathFinder.Path.Node>();
			}
			this.nodes.Add(node);
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000B77DC File Offset: 0x000B59DC
		public bool IsValid()
		{
			return this.nodes != null && this.nodes.Count > 1;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000B77F6 File Offset: 0x000B59F6
		public bool HasArrived()
		{
			return this.nodes != null && this.nodes.Count > 0;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000B7810 File Offset: 0x000B5A10
		public void Clear()
		{
			this.cost = 0;
			if (this.nodes != null)
			{
				this.nodes.Clear();
			}
		}

		// Token: 0x0400183B RID: 6203
		public int cost;

		// Token: 0x0400183C RID: 6204
		public List<PathFinder.Path.Node> nodes;

		// Token: 0x020007E6 RID: 2022
		public struct Node
		{
			// Token: 0x0400183D RID: 6205
			public int cell;

			// Token: 0x0400183E RID: 6206
			public NavType navType;

			// Token: 0x0400183F RID: 6207
			public byte transitionId;
		}
	}

	// Token: 0x020007E7 RID: 2023
	public class PotentialList
	{
		// Token: 0x06002431 RID: 9265 RVA: 0x000B782C File Offset: 0x000B5A2C
		public KeyValuePair<int, PathFinder.PotentialPath> Next()
		{
			return this.queue.Dequeue();
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x000B7839 File Offset: 0x000B5A39
		public int Count
		{
			get
			{
				return this.queue.Count;
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000B7846 File Offset: 0x000B5A46
		public void Add(int cost, PathFinder.PotentialPath path)
		{
			this.queue.Enqueue(cost, path);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000B7855 File Offset: 0x000B5A55
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x04001840 RID: 6208
		private PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath> queue = new PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath>();

		// Token: 0x020007E8 RID: 2024
		public class PriorityQueue<TValue>
		{
			// Token: 0x06002436 RID: 9270 RVA: 0x000B7875 File Offset: 0x000B5A75
			public PriorityQueue()
			{
				this._baseHeap = new List<KeyValuePair<int, TValue>>();
			}

			// Token: 0x06002437 RID: 9271 RVA: 0x000B7888 File Offset: 0x000B5A88
			public void Enqueue(int priority, TValue value)
			{
				this.Insert(priority, value);
			}

			// Token: 0x06002438 RID: 9272 RVA: 0x000B7892 File Offset: 0x000B5A92
			public KeyValuePair<int, TValue> Dequeue()
			{
				KeyValuePair<int, TValue> result = this._baseHeap[0];
				this.DeleteRoot();
				return result;
			}

			// Token: 0x06002439 RID: 9273 RVA: 0x000B78A6 File Offset: 0x000B5AA6
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.Count > 0)
				{
					return this._baseHeap[0];
				}
				throw new InvalidOperationException("Priority queue is empty");
			}

			// Token: 0x0600243A RID: 9274 RVA: 0x001C91EC File Offset: 0x001C73EC
			private void ExchangeElements(int pos1, int pos2)
			{
				KeyValuePair<int, TValue> value = this._baseHeap[pos1];
				this._baseHeap[pos1] = this._baseHeap[pos2];
				this._baseHeap[pos2] = value;
			}

			// Token: 0x0600243B RID: 9275 RVA: 0x001C922C File Offset: 0x001C742C
			private void Insert(int priority, TValue value)
			{
				KeyValuePair<int, TValue> item = new KeyValuePair<int, TValue>(priority, value);
				this._baseHeap.Add(item);
				this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
			}

			// Token: 0x0600243C RID: 9276 RVA: 0x001C9264 File Offset: 0x001C7464
			private int HeapifyFromEndToBeginning(int pos)
			{
				if (pos >= this._baseHeap.Count)
				{
					return -1;
				}
				while (pos > 0)
				{
					int num = (pos - 1) / 2;
					if (this._baseHeap[num].Key - this._baseHeap[pos].Key <= 0)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
				return pos;
			}

			// Token: 0x0600243D RID: 9277 RVA: 0x001C92C4 File Offset: 0x001C74C4
			private void DeleteRoot()
			{
				if (this._baseHeap.Count <= 1)
				{
					this._baseHeap.Clear();
					return;
				}
				this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
				this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
				this.HeapifyFromBeginningToEnd(0);
			}

			// Token: 0x0600243E RID: 9278 RVA: 0x001C9330 File Offset: 0x001C7530
			private void HeapifyFromBeginningToEnd(int pos)
			{
				int count = this._baseHeap.Count;
				if (pos >= count)
				{
					return;
				}
				for (;;)
				{
					int num = pos;
					int num2 = 2 * pos + 1;
					int num3 = 2 * pos + 2;
					if (num2 < count && this._baseHeap[num].Key - this._baseHeap[num2].Key > 0)
					{
						num = num2;
					}
					if (num3 < count && this._baseHeap[num].Key - this._baseHeap[num3].Key > 0)
					{
						num = num3;
					}
					if (num == pos)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
			}

			// Token: 0x0600243F RID: 9279 RVA: 0x000B78C8 File Offset: 0x000B5AC8
			public void Clear()
			{
				this._baseHeap.Clear();
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06002440 RID: 9280 RVA: 0x000B78D5 File Offset: 0x000B5AD5
			public int Count
			{
				get
				{
					return this._baseHeap.Count;
				}
			}

			// Token: 0x04001841 RID: 6209
			private List<KeyValuePair<int, TValue>> _baseHeap;
		}

		// Token: 0x020007E9 RID: 2025
		private class HOTQueue<TValue>
		{
			// Token: 0x06002441 RID: 9281 RVA: 0x001C93D8 File Offset: 0x001C75D8
			public KeyValuePair<int, TValue> Dequeue()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				this.count--;
				return this.hotQueue.Dequeue();
			}

			// Token: 0x06002442 RID: 9282 RVA: 0x001C9434 File Offset: 0x001C7634
			public void Enqueue(int priority, TValue value)
			{
				if (priority <= this.hotThreshold)
				{
					this.hotQueue.Enqueue(priority, value);
				}
				else
				{
					this.coldQueue.Enqueue(priority, value);
					this.coldThreshold = Math.Max(this.coldThreshold, priority);
				}
				this.count++;
			}

			// Token: 0x06002443 RID: 9283 RVA: 0x001C9488 File Offset: 0x001C7688
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				return this.hotQueue.Peek();
			}

			// Token: 0x06002444 RID: 9284 RVA: 0x000B78E2 File Offset: 0x000B5AE2
			public void Clear()
			{
				this.count = 0;
				this.hotThreshold = int.MinValue;
				this.hotQueue.Clear();
				this.coldThreshold = int.MinValue;
				this.coldQueue.Clear();
			}

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x06002445 RID: 9285 RVA: 0x000B7917 File Offset: 0x000B5B17
			public int Count
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x04001842 RID: 6210
			private PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x04001843 RID: 6211
			private PathFinder.PotentialList.PriorityQueue<TValue> coldQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x04001844 RID: 6212
			private int hotThreshold = int.MinValue;

			// Token: 0x04001845 RID: 6213
			private int coldThreshold = int.MinValue;

			// Token: 0x04001846 RID: 6214
			private int count;
		}
	}

	// Token: 0x020007EA RID: 2026
	private class Temp
	{
		// Token: 0x04001847 RID: 6215
		public static PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
	}

	// Token: 0x020007EB RID: 2027
	public class PotentialScratchPad
	{
		// Token: 0x06002449 RID: 9289 RVA: 0x000B795F File Offset: 0x000B5B5F
		public PotentialScratchPad(int max_links_per_cell)
		{
			this.linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
			this.linksInCellRange = new PathFinder.PotentialScratchPad.PathGridCellData[max_links_per_cell];
		}

		// Token: 0x04001848 RID: 6216
		public NavGrid.Link[] linksWithCorrectNavType;

		// Token: 0x04001849 RID: 6217
		public PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange;

		// Token: 0x020007EC RID: 2028
		public struct PathGridCellData
		{
			// Token: 0x0400184A RID: 6218
			public PathFinder.Cell pathGridCell;

			// Token: 0x0400184B RID: 6219
			public NavGrid.Link link;

			// Token: 0x0400184C RID: 6220
			public bool isSubmerged;
		}
	}
}
