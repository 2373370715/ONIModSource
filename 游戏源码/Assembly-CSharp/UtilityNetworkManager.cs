using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A1D RID: 6685
public class UtilityNetworkManager<NetworkType, ItemType> : IUtilityNetworkMgr where NetworkType : UtilityNetwork, new() where ItemType : MonoBehaviour
{
	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x06008B49 RID: 35657 RVA: 0x000FB10C File Offset: 0x000F930C
	public bool IsDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x06008B4A RID: 35658 RVA: 0x0035E834 File Offset: 0x0035CA34
	public UtilityNetworkManager(int game_width, int game_height, int tile_layer)
	{
		this.tileLayer = tile_layer;
		this.networks = new List<UtilityNetwork>();
		this.Initialize(game_width, game_height);
	}

	// Token: 0x06008B4B RID: 35659 RVA: 0x0035E8C0 File Offset: 0x0035CAC0
	public void Initialize(int game_width, int game_height)
	{
		this.networks.Clear();
		this.physicalGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.visualGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.stashedVisualGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.physicalNodes = new HashSet<int>();
		this.visualNodes = new HashSet<int>();
		this.visitedCells = new HashSet<int>();
		this.visitedVirtualKeys = new HashSet<object>();
		this.queuedVirtualKeys = new HashSet<object>();
		for (int i = 0; i < this.visualGrid.Length; i++)
		{
			this.visualGrid[i] = new UtilityNetworkGridNode
			{
				networkIdx = -1,
				connections = (UtilityConnections)0
			};
			this.physicalGrid[i] = new UtilityNetworkGridNode
			{
				networkIdx = -1,
				connections = (UtilityConnections)0
			};
		}
	}

	// Token: 0x06008B4C RID: 35660 RVA: 0x0035E998 File Offset: 0x0035CB98
	public void Update()
	{
		if (this.dirty)
		{
			this.dirty = false;
			for (int i = 0; i < this.networks.Count; i++)
			{
				this.networks[i].Reset(this.physicalGrid);
			}
			this.networks.Clear();
			this.virtualKeyToNetworkIdx.Clear();
			this.RebuildNetworks(this.tileLayer, false);
			this.RebuildNetworks(this.tileLayer, true);
			if (this.onNetworksRebuilt != null)
			{
				this.onNetworksRebuilt(this.networks, this.GetNodes(true));
			}
		}
	}

	// Token: 0x06008B4D RID: 35661 RVA: 0x000FB114 File Offset: 0x000F9314
	protected UtilityNetworkGridNode[] GetGrid(bool is_physical_building)
	{
		if (!is_physical_building)
		{
			return this.visualGrid;
		}
		return this.physicalGrid;
	}

	// Token: 0x06008B4E RID: 35662 RVA: 0x000FB126 File Offset: 0x000F9326
	private HashSet<int> GetNodes(bool is_physical_building)
	{
		if (!is_physical_building)
		{
			return this.visualNodes;
		}
		return this.physicalNodes;
	}

	// Token: 0x06008B4F RID: 35663 RVA: 0x0035EA34 File Offset: 0x0035CC34
	public void ClearCell(int cell, bool is_physical_building)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
		HashSet<int> nodes = this.GetNodes(is_physical_building);
		UtilityConnections connections = grid[cell].connections;
		grid[cell].connections = (UtilityConnections)0;
		Vector2I vector2I = Grid.CellToXY(cell);
		if (vector2I.x > 0 && (connections & UtilityConnections.Left) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array = grid;
			int num = Grid.CellLeft(cell);
			array[num].connections = (array[num].connections & ~UtilityConnections.Right);
		}
		if (vector2I.x < Grid.WidthInCells - 1 && (connections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array2 = grid;
			int num2 = Grid.CellRight(cell);
			array2[num2].connections = (array2[num2].connections & ~UtilityConnections.Left);
		}
		if (vector2I.y > 0 && (connections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array3 = grid;
			int num3 = Grid.CellBelow(cell);
			array3[num3].connections = (array3[num3].connections & ~UtilityConnections.Up);
		}
		if (vector2I.y < Grid.HeightInCells - 1 && (connections & UtilityConnections.Up) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array4 = grid;
			int num4 = Grid.CellAbove(cell);
			array4[num4].connections = (array4[num4].connections & ~UtilityConnections.Down);
		}
		nodes.Remove(cell);
		if (is_physical_building)
		{
			this.dirty = true;
			this.ClearCell(cell, false);
		}
	}

	// Token: 0x06008B50 RID: 35664 RVA: 0x0035EB34 File Offset: 0x0035CD34
	private void QueueCellForVisit(UtilityNetworkGridNode[] grid, int dest_cell, UtilityConnections direction)
	{
		if (!Grid.IsValidCell(dest_cell))
		{
			return;
		}
		if (this.visitedCells.Contains(dest_cell))
		{
			return;
		}
		if (direction != (UtilityConnections)0 && (grid[dest_cell].connections & direction.InverseDirection()) == (UtilityConnections)0)
		{
			return;
		}
		if (Grid.Objects[dest_cell, this.tileLayer] != null)
		{
			this.visitedCells.Add(dest_cell);
			this.queued.Enqueue(dest_cell);
		}
	}

	// Token: 0x06008B51 RID: 35665 RVA: 0x000FB138 File Offset: 0x000F9338
	public void ForceRebuildNetworks()
	{
		this.dirty = true;
	}

	// Token: 0x06008B52 RID: 35666 RVA: 0x0035EBA4 File Offset: 0x0035CDA4
	public void AddToNetworks(int cell, object item, bool is_endpoint)
	{
		if (item != null)
		{
			if (is_endpoint)
			{
				if (this.endpoints.ContainsKey(cell))
				{
					global::Debug.LogWarning(string.Format("Cell {0} already has a utility network endpoint assigned. Adding {1} will stomp previous endpoint, destroying the object that's already there.", cell, item.ToString()));
					KMonoBehaviour kmonoBehaviour = this.endpoints[cell] as KMonoBehaviour;
					if (kmonoBehaviour != null)
					{
						Util.KDestroyGameObject(kmonoBehaviour);
					}
				}
				this.endpoints[cell] = item;
			}
			else
			{
				if (this.items.ContainsKey(cell))
				{
					global::Debug.LogWarning(string.Format("Cell {0} already has a utility network connector assigned. Adding {1} will stomp previous item, destroying the object that's already there.", cell, item.ToString()));
					KMonoBehaviour kmonoBehaviour2 = this.items[cell] as KMonoBehaviour;
					if (kmonoBehaviour2 != null)
					{
						Util.KDestroyGameObject(kmonoBehaviour2);
					}
				}
				this.items[cell] = item;
			}
		}
		this.dirty = true;
	}

	// Token: 0x06008B53 RID: 35667 RVA: 0x0035EC74 File Offset: 0x0035CE74
	public void AddToVirtualNetworks(object key, object item, bool is_endpoint)
	{
		if (item != null)
		{
			if (is_endpoint)
			{
				if (!this.virtualEndpoints.ContainsKey(key))
				{
					this.virtualEndpoints[key] = new List<object>();
				}
				this.virtualEndpoints[key].Add(item);
			}
			else
			{
				if (!this.virtualItems.ContainsKey(key))
				{
					this.virtualItems[key] = new List<object>();
				}
				this.virtualItems[key].Add(item);
			}
		}
		this.dirty = true;
	}

	// Token: 0x06008B54 RID: 35668 RVA: 0x0035ECF4 File Offset: 0x0035CEF4
	private unsafe void Reconnect(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		int* ptr3 = stackalloc int[(UIntPtr)16];
		int num = 0;
		if (vector2I.y < Grid.HeightInCells - 1)
		{
			ptr[num] = Grid.CellAbove(cell);
			ptr2[num] = 4;
			ptr3[num] = 8;
			num++;
		}
		if (vector2I.y > 0)
		{
			ptr[num] = Grid.CellBelow(cell);
			ptr2[num] = 8;
			ptr3[num] = 4;
			num++;
		}
		if (vector2I.x > 0)
		{
			ptr[num] = Grid.CellLeft(cell);
			ptr2[num] = 1;
			ptr3[num] = 2;
			num++;
		}
		if (vector2I.x < Grid.WidthInCells - 1)
		{
			ptr[num] = Grid.CellRight(cell);
			ptr2[num] = 2;
			ptr3[num] = 1;
			num++;
		}
		UtilityConnections connections = this.physicalGrid[cell].connections;
		UtilityConnections connections2 = this.visualGrid[cell].connections;
		for (int i = 0; i < num; i++)
		{
			int num2 = ptr[i];
			UtilityConnections utilityConnections = (UtilityConnections)ptr2[i];
			UtilityConnections utilityConnections2 = (UtilityConnections)ptr3[i];
			if ((connections & utilityConnections) != (UtilityConnections)0)
			{
				if (this.physicalNodes.Contains(num2))
				{
					UtilityNetworkGridNode[] array = this.physicalGrid;
					int num3 = num2;
					array[num3].connections = (array[num3].connections | utilityConnections2);
				}
				if (this.visualNodes.Contains(num2))
				{
					UtilityNetworkGridNode[] array2 = this.visualGrid;
					int num4 = num2;
					array2[num4].connections = (array2[num4].connections | utilityConnections2);
				}
			}
			else if ((connections2 & utilityConnections) != (UtilityConnections)0 && (this.physicalNodes.Contains(num2) || this.visualNodes.Contains(num2)))
			{
				UtilityNetworkGridNode[] array3 = this.visualGrid;
				int num5 = num2;
				array3[num5].connections = (array3[num5].connections | utilityConnections2);
			}
		}
	}

	// Token: 0x06008B55 RID: 35669 RVA: 0x0035EED4 File Offset: 0x0035D0D4
	public void RemoveFromVirtualNetworks(object key, object item, bool is_endpoint)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.dirty = true;
		if (item != null)
		{
			if (is_endpoint)
			{
				this.virtualEndpoints[key].Remove(item);
				if (this.virtualEndpoints[key].Count == 0)
				{
					this.virtualEndpoints.Remove(key);
				}
			}
			else
			{
				this.virtualItems[key].Remove(item);
				if (this.virtualItems[key].Count == 0)
				{
					this.virtualItems.Remove(key);
				}
			}
			UtilityNetwork networkForVirtualKey = this.GetNetworkForVirtualKey(key);
			if (networkForVirtualKey != null)
			{
				networkForVirtualKey.RemoveItem(item);
			}
		}
	}

	// Token: 0x06008B56 RID: 35670 RVA: 0x0035EF70 File Offset: 0x0035D170
	public void RemoveFromNetworks(int cell, object item, bool is_endpoint)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.dirty = true;
		if (item != null)
		{
			if (is_endpoint)
			{
				this.endpoints.Remove(cell);
				int networkIdx = this.physicalGrid[cell].networkIdx;
				if (networkIdx != -1)
				{
					this.networks[networkIdx].RemoveItem(item);
					return;
				}
			}
			else
			{
				int networkIdx2 = this.physicalGrid[cell].networkIdx;
				this.physicalGrid[cell].connections = (UtilityConnections)0;
				this.physicalGrid[cell].networkIdx = -1;
				this.items.Remove(cell);
				this.Disconnect(cell);
				object item2;
				if (this.endpoints.TryGetValue(cell, out item2) && networkIdx2 != -1)
				{
					this.networks[networkIdx2].DisconnectItem(item2);
				}
			}
		}
	}

	// Token: 0x06008B57 RID: 35671 RVA: 0x0035F040 File Offset: 0x0035D240
	private unsafe void Disconnect(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		int num = 0;
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		if (vector2I.y < Grid.HeightInCells - 1)
		{
			ptr[num] = Grid.CellAbove(cell);
			ptr2[num] = -9;
			num++;
		}
		if (vector2I.y > 0)
		{
			ptr[num] = Grid.CellBelow(cell);
			ptr2[num] = -5;
			num++;
		}
		if (vector2I.x > 0)
		{
			ptr[num] = Grid.CellLeft(cell);
			ptr2[num] = -3;
			num++;
		}
		if (vector2I.x < Grid.WidthInCells - 1)
		{
			ptr[num] = Grid.CellRight(cell);
			ptr2[num] = -2;
			num++;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = ptr[i];
			int num3 = ptr2[i];
			int connections = (int)(this.physicalGrid[num2].connections & (UtilityConnections)num3);
			this.physicalGrid[num2].connections = (UtilityConnections)connections;
		}
	}

	// Token: 0x06008B58 RID: 35672 RVA: 0x0035F14C File Offset: 0x0035D34C
	private unsafe void RebuildNetworks(int layer, bool is_physical)
	{
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical);
		HashSet<int> nodes = this.GetNodes(is_physical);
		this.visitedCells.Clear();
		this.visitedVirtualKeys.Clear();
		this.queuedVirtualKeys.Clear();
		this.queued.Clear();
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		foreach (int num in nodes)
		{
			UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
			if (!this.visitedCells.Contains(num))
			{
				this.queued.Enqueue(num);
				this.visitedCells.Add(num);
				NetworkType networkType = Activator.CreateInstance<NetworkType>();
				networkType.id = this.networks.Count;
				this.networks.Add(networkType);
				while (this.queued.Count > 0)
				{
					int num2 = this.queued.Dequeue();
					utilityNetworkGridNode = grid[num2];
					object obj = null;
					object obj2 = null;
					if (is_physical)
					{
						if (this.items.TryGetValue(num2, out obj))
						{
							if (obj is IDisconnectable && (obj as IDisconnectable).IsDisconnected())
							{
								continue;
							}
							if (obj != null)
							{
								networkType.AddItem(obj);
							}
						}
						if (this.endpoints.TryGetValue(num2, out obj2) && obj2 != null)
						{
							networkType.AddItem(obj2);
						}
					}
					grid[num2].networkIdx = networkType.id;
					if (obj != null && obj2 != null)
					{
						networkType.ConnectItem(obj2);
					}
					Vector2I vector2I = Grid.CellToXY(num2);
					int num3 = 0;
					if (vector2I.x > 0)
					{
						ptr[num3] = Grid.CellLeft(num2);
						ptr2[num3] = 1;
						num3++;
					}
					if (vector2I.x < Grid.WidthInCells - 1)
					{
						ptr[num3] = Grid.CellRight(num2);
						ptr2[num3] = 2;
						num3++;
					}
					if (vector2I.y > 0)
					{
						ptr[num3] = Grid.CellBelow(num2);
						ptr2[num3] = 8;
						num3++;
					}
					if (vector2I.y < Grid.HeightInCells - 1)
					{
						ptr[num3] = Grid.CellAbove(num2);
						ptr2[num3] = 4;
						num3++;
					}
					for (int i = 0; i < num3; i++)
					{
						int num4 = ptr2[i];
						if ((utilityNetworkGridNode.connections & (UtilityConnections)num4) != (UtilityConnections)0)
						{
							int dest_cell = ptr[i];
							this.QueueCellForVisit(grid, dest_cell, (UtilityConnections)num4);
						}
					}
					int dest_cell2;
					if (this.links.TryGetValue(num2, out dest_cell2))
					{
						this.QueueCellForVisit(grid, dest_cell2, (UtilityConnections)0);
					}
					object obj3;
					if (this.semiVirtualLinks.TryGetValue(num2, out obj3) && !this.visitedVirtualKeys.Contains(obj3))
					{
						this.visitedVirtualKeys.Add(obj3);
						this.virtualKeyToNetworkIdx[obj3] = networkType.id;
						if (this.virtualItems.ContainsKey(obj3))
						{
							foreach (object item in this.virtualItems[obj3])
							{
								networkType.AddItem(item);
								networkType.ConnectItem(item);
							}
						}
						if (this.virtualEndpoints.ContainsKey(obj3))
						{
							foreach (object item2 in this.virtualEndpoints[obj3])
							{
								networkType.AddItem(item2);
								networkType.ConnectItem(item2);
							}
						}
						foreach (KeyValuePair<int, object> keyValuePair in this.semiVirtualLinks)
						{
							if (keyValuePair.Value == obj3)
							{
								this.QueueCellForVisit(grid, keyValuePair.Key, (UtilityConnections)0);
							}
						}
					}
				}
			}
		}
		foreach (KeyValuePair<object, List<object>> keyValuePair2 in this.virtualItems)
		{
			if (!this.visitedVirtualKeys.Contains(keyValuePair2.Key))
			{
				NetworkType networkType2 = Activator.CreateInstance<NetworkType>();
				networkType2.id = this.networks.Count;
				this.visitedVirtualKeys.Add(keyValuePair2.Key);
				this.virtualKeyToNetworkIdx[keyValuePair2.Key] = networkType2.id;
				foreach (object item3 in keyValuePair2.Value)
				{
					networkType2.AddItem(item3);
					networkType2.ConnectItem(item3);
				}
				foreach (object item4 in this.virtualEndpoints[keyValuePair2.Key])
				{
					networkType2.AddItem(item4);
					networkType2.ConnectItem(item4);
				}
				this.networks.Add(networkType2);
			}
		}
		foreach (KeyValuePair<object, List<object>> keyValuePair3 in this.virtualEndpoints)
		{
			if (!this.visitedVirtualKeys.Contains(keyValuePair3.Key))
			{
				NetworkType networkType3 = Activator.CreateInstance<NetworkType>();
				networkType3.id = this.networks.Count;
				this.visitedVirtualKeys.Add(keyValuePair3.Key);
				this.virtualKeyToNetworkIdx[keyValuePair3.Key] = networkType3.id;
				foreach (object item5 in this.virtualEndpoints[keyValuePair3.Key])
				{
					networkType3.AddItem(item5);
					networkType3.ConnectItem(item5);
				}
				this.networks.Add(networkType3);
			}
		}
	}

	// Token: 0x06008B59 RID: 35673 RVA: 0x0035F8A0 File Offset: 0x0035DAA0
	public UtilityNetwork GetNetworkForVirtualKey(object key)
	{
		int index;
		if (this.virtualKeyToNetworkIdx.TryGetValue(key, out index))
		{
			return this.networks[index];
		}
		return null;
	}

	// Token: 0x06008B5A RID: 35674 RVA: 0x0035F8CC File Offset: 0x0035DACC
	public UtilityNetwork GetNetworkByID(int id)
	{
		UtilityNetwork result = null;
		if (0 <= id && id < this.networks.Count)
		{
			result = this.networks[id];
		}
		return result;
	}

	// Token: 0x06008B5B RID: 35675 RVA: 0x0035F8FC File Offset: 0x0035DAFC
	public UtilityNetwork GetNetworkForCell(int cell)
	{
		UtilityNetwork result = null;
		if (Grid.IsValidCell(cell) && 0 <= this.physicalGrid[cell].networkIdx && this.physicalGrid[cell].networkIdx < this.networks.Count)
		{
			result = this.networks[this.physicalGrid[cell].networkIdx];
		}
		return result;
	}

	// Token: 0x06008B5C RID: 35676 RVA: 0x0035F964 File Offset: 0x0035DB64
	public UtilityNetwork GetNetworkForDirection(int cell, Direction direction)
	{
		cell = Grid.GetCellInDirection(cell, direction);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		UtilityNetworkGridNode utilityNetworkGridNode = this.GetGrid(true)[cell];
		UtilityNetwork result = null;
		if (utilityNetworkGridNode.networkIdx != -1 && utilityNetworkGridNode.networkIdx < this.networks.Count)
		{
			result = this.networks[utilityNetworkGridNode.networkIdx];
		}
		return result;
	}

	// Token: 0x06008B5D RID: 35677 RVA: 0x0035F9C4 File Offset: 0x0035DBC4
	private UtilityConnections GetNeighboursAsConnections(int cell, HashSet<int> nodes)
	{
		UtilityConnections utilityConnections = (UtilityConnections)0;
		Vector2I vector2I = Grid.CellToXY(cell);
		if (vector2I.x > 0 && nodes.Contains(Grid.CellLeft(cell)))
		{
			utilityConnections |= UtilityConnections.Left;
		}
		if (vector2I.x < Grid.WidthInCells - 1 && nodes.Contains(Grid.CellRight(cell)))
		{
			utilityConnections |= UtilityConnections.Right;
		}
		if (vector2I.y > 0 && nodes.Contains(Grid.CellBelow(cell)))
		{
			utilityConnections |= UtilityConnections.Down;
		}
		if (vector2I.y < Grid.HeightInCells - 1 && nodes.Contains(Grid.CellAbove(cell)))
		{
			utilityConnections |= UtilityConnections.Up;
		}
		return utilityConnections;
	}

	// Token: 0x06008B5E RID: 35678 RVA: 0x0035FA54 File Offset: 0x0035DC54
	public virtual void SetConnections(UtilityConnections connections, int cell, bool is_physical_building)
	{
		HashSet<int> nodes = this.GetNodes(is_physical_building);
		nodes.Add(cell);
		this.visualGrid[cell].connections = connections;
		if (is_physical_building)
		{
			this.dirty = true;
			UtilityConnections connections2 = is_physical_building ? (connections & this.GetNeighboursAsConnections(cell, nodes)) : connections;
			this.physicalGrid[cell].connections = connections2;
		}
		this.Reconnect(cell);
	}

	// Token: 0x06008B5F RID: 35679 RVA: 0x0035FAB8 File Offset: 0x0035DCB8
	public UtilityConnections GetConnections(int cell, bool is_physical_building)
	{
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
		UtilityConnections utilityConnections = grid[cell].connections;
		if (!is_physical_building)
		{
			grid = this.GetGrid(true);
			utilityConnections |= grid[cell].connections;
		}
		return utilityConnections;
	}

	// Token: 0x06008B60 RID: 35680 RVA: 0x0035FAF8 File Offset: 0x0035DCF8
	public UtilityConnections GetDisplayConnections(int cell)
	{
		UtilityConnections utilityConnections = (UtilityConnections)0;
		UtilityNetworkGridNode[] grid = this.GetGrid(false);
		UtilityConnections utilityConnections2 = utilityConnections | grid[cell].connections;
		grid = this.GetGrid(true);
		return utilityConnections2 | grid[cell].connections;
	}

	// Token: 0x06008B61 RID: 35681 RVA: 0x000FB141 File Offset: 0x000F9341
	public virtual bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason)
	{
		fail_reason = null;
		return true;
	}

	// Token: 0x06008B62 RID: 35682 RVA: 0x0035FB30 File Offset: 0x0035DD30
	public void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building)
	{
		string text;
		if (this.CanAddConnection(new_connection, cell, is_physical_building, out text))
		{
			if (is_physical_building)
			{
				this.dirty = true;
			}
			UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
			UtilityConnections connections = grid[cell].connections;
			grid[cell].connections = (connections | new_connection);
		}
	}

	// Token: 0x06008B63 RID: 35683 RVA: 0x000FB148 File Offset: 0x000F9348
	public void StashVisualGrids()
	{
		Array.Copy(this.visualGrid, this.stashedVisualGrid, this.visualGrid.Length);
	}

	// Token: 0x06008B64 RID: 35684 RVA: 0x000FB163 File Offset: 0x000F9363
	public void UnstashVisualGrids()
	{
		Array.Copy(this.stashedVisualGrid, this.visualGrid, this.visualGrid.Length);
	}

	// Token: 0x06008B65 RID: 35685 RVA: 0x0035FB78 File Offset: 0x0035DD78
	public string GetVisualizerString(int cell)
	{
		UtilityConnections displayConnections = this.GetDisplayConnections(cell);
		return this.GetVisualizerString(displayConnections);
	}

	// Token: 0x06008B66 RID: 35686 RVA: 0x0035FB94 File Offset: 0x0035DD94
	public string GetVisualizerString(UtilityConnections connections)
	{
		string text = "";
		if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
		{
			text += "L";
		}
		if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			text += "R";
		}
		if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
		{
			text += "U";
		}
		if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			text += "D";
		}
		if (text == "")
		{
			text = "None";
		}
		return text;
	}

	// Token: 0x06008B67 RID: 35687 RVA: 0x0035FC00 File Offset: 0x0035DE00
	public object GetEndpoint(int cell)
	{
		object result = null;
		this.endpoints.TryGetValue(cell, out result);
		return result;
	}

	// Token: 0x06008B68 RID: 35688 RVA: 0x000FB17E File Offset: 0x000F937E
	public void AddSemiVirtualLink(int cell1, object virtualKey)
	{
		global::Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
		this.semiVirtualLinks[cell1] = virtualKey;
		this.dirty = true;
	}

	// Token: 0x06008B69 RID: 35689 RVA: 0x000FB1A2 File Offset: 0x000F93A2
	public void RemoveSemiVirtualLink(int cell1, object virtualKey)
	{
		global::Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
		this.semiVirtualLinks.Remove(cell1);
		this.dirty = true;
	}

	// Token: 0x06008B6A RID: 35690 RVA: 0x000FB1C6 File Offset: 0x000F93C6
	public void AddLink(int cell1, int cell2)
	{
		this.links[cell1] = cell2;
		this.links[cell2] = cell1;
		this.dirty = true;
	}

	// Token: 0x06008B6B RID: 35691 RVA: 0x000FB1E9 File Offset: 0x000F93E9
	public void RemoveLink(int cell1, int cell2)
	{
		this.links.Remove(cell1);
		this.links.Remove(cell2);
		this.dirty = true;
	}

	// Token: 0x06008B6C RID: 35692 RVA: 0x000FB20C File Offset: 0x000F940C
	public void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener)
	{
		this.onNetworksRebuilt = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Combine(this.onNetworksRebuilt, listener);
	}

	// Token: 0x06008B6D RID: 35693 RVA: 0x000FB225 File Offset: 0x000F9425
	public void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener)
	{
		this.onNetworksRebuilt = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Remove(this.onNetworksRebuilt, listener);
	}

	// Token: 0x06008B6E RID: 35694 RVA: 0x000FB23E File Offset: 0x000F943E
	public IList<UtilityNetwork> GetNetworks()
	{
		return this.networks;
	}

	// Token: 0x040068C5 RID: 26821
	private Dictionary<int, object> items = new Dictionary<int, object>();

	// Token: 0x040068C6 RID: 26822
	private Dictionary<int, object> endpoints = new Dictionary<int, object>();

	// Token: 0x040068C7 RID: 26823
	private Dictionary<object, List<object>> virtualItems = new Dictionary<object, List<object>>();

	// Token: 0x040068C8 RID: 26824
	private Dictionary<object, List<object>> virtualEndpoints = new Dictionary<object, List<object>>();

	// Token: 0x040068C9 RID: 26825
	private Dictionary<int, int> links = new Dictionary<int, int>();

	// Token: 0x040068CA RID: 26826
	private Dictionary<int, object> semiVirtualLinks = new Dictionary<int, object>();

	// Token: 0x040068CB RID: 26827
	private List<UtilityNetwork> networks;

	// Token: 0x040068CC RID: 26828
	private Dictionary<object, int> virtualKeyToNetworkIdx = new Dictionary<object, int>();

	// Token: 0x040068CD RID: 26829
	private HashSet<int> visitedCells;

	// Token: 0x040068CE RID: 26830
	private HashSet<object> visitedVirtualKeys;

	// Token: 0x040068CF RID: 26831
	private HashSet<object> queuedVirtualKeys;

	// Token: 0x040068D0 RID: 26832
	private Action<IList<UtilityNetwork>, ICollection<int>> onNetworksRebuilt;

	// Token: 0x040068D1 RID: 26833
	private Queue<int> queued = new Queue<int>();

	// Token: 0x040068D2 RID: 26834
	protected UtilityNetworkGridNode[] visualGrid;

	// Token: 0x040068D3 RID: 26835
	private UtilityNetworkGridNode[] stashedVisualGrid;

	// Token: 0x040068D4 RID: 26836
	protected UtilityNetworkGridNode[] physicalGrid;

	// Token: 0x040068D5 RID: 26837
	protected HashSet<int> physicalNodes;

	// Token: 0x040068D6 RID: 26838
	protected HashSet<int> visualNodes;

	// Token: 0x040068D7 RID: 26839
	private bool dirty;

	// Token: 0x040068D8 RID: 26840
	private int tileLayer = -1;
}
