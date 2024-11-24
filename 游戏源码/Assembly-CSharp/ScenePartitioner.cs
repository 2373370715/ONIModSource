using System;
using System.Collections.Generic;

// Token: 0x02001839 RID: 6201
public class ScenePartitioner : ISim1000ms
{
	// Token: 0x06008028 RID: 32808 RVA: 0x003332E0 File Offset: 0x003314E0
	public ScenePartitioner(int node_size, int layer_count, int scene_width, int scene_height)
	{
		this.nodeSize = node_size;
		int num = scene_width / node_size;
		int num2 = scene_height / node_size;
		this.nodes = new ScenePartitioner.ScenePartitionerNode[layer_count, num2, num];
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					this.nodes[i, j, k].entries = new HashSet<ScenePartitionerEntry>();
				}
			}
		}
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06008029 RID: 32809 RVA: 0x003333A0 File Offset: 0x003315A0
	public void FreeResources()
	{
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[i, j, k].entries)
					{
						if (scenePartitionerEntry != null)
						{
							scenePartitionerEntry.partitioner = null;
							scenePartitionerEntry.obj = null;
						}
					}
					this.nodes[i, j, k].entries.Clear();
				}
			}
		}
		this.nodes = null;
	}

	// Token: 0x0600802A RID: 32810 RVA: 0x00333480 File Offset: 0x00331680
	[Obsolete]
	public ScenePartitionerLayer CreateMask(HashedString name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x0600802B RID: 32811 RVA: 0x00333520 File Offset: 0x00331720
	public ScenePartitionerLayer CreateMask(string name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		HashCache.Get().Add(name);
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x0600802C RID: 32812 RVA: 0x000F45FD File Offset: 0x000F27FD
	private int ClampNodeX(int x)
	{
		return Math.Min(Math.Max(x, 0), this.nodes.GetLength(2) - 1);
	}

	// Token: 0x0600802D RID: 32813 RVA: 0x000F4619 File Offset: 0x000F2819
	private int ClampNodeY(int y)
	{
		return Math.Min(Math.Max(y, 0), this.nodes.GetLength(1) - 1);
	}

	// Token: 0x0600802E RID: 32814 RVA: 0x003335D8 File Offset: 0x003317D8
	private Extents GetNodeExtents(int x, int y, int width, int height)
	{
		Extents extents = default(Extents);
		extents.x = this.ClampNodeX(x / this.nodeSize);
		extents.y = this.ClampNodeY(y / this.nodeSize);
		extents.width = 1 + this.ClampNodeX((x + width) / this.nodeSize) - extents.x;
		extents.height = 1 + this.ClampNodeY((y + height) / this.nodeSize) - extents.y;
		return extents;
	}

	// Token: 0x0600802F RID: 32815 RVA: 0x000F4635 File Offset: 0x000F2835
	private Extents GetNodeExtents(ScenePartitionerEntry entry)
	{
		return this.GetNodeExtents(entry.x, entry.y, entry.width, entry.height);
	}

	// Token: 0x06008030 RID: 32816 RVA: 0x0033365C File Offset: 0x0033185C
	private void Insert(ScenePartitionerEntry entry)
	{
		if (entry.obj == null)
		{
			Debug.LogWarning("Trying to put null go into scene partitioner");
			return;
		}
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
				this.nodes[layer, i, j].entries.Add(entry);
			}
		}
	}

	// Token: 0x06008031 RID: 32817 RVA: 0x00333854 File Offset: 0x00331A54
	private void Widthdraw(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (this.nodes[layer, i, j].entries.Remove(entry) && !this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
	}

	// Token: 0x06008032 RID: 32818 RVA: 0x000F4655 File Offset: 0x000F2855
	public ScenePartitionerEntry Add(ScenePartitionerEntry entry)
	{
		this.Insert(entry);
		return entry;
	}

	// Token: 0x06008033 RID: 32819 RVA: 0x000F465F File Offset: 0x000F285F
	public void UpdatePosition(int x, int y, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = x;
		entry.y = y;
		this.Insert(entry);
	}

	// Token: 0x06008034 RID: 32820 RVA: 0x000F467D File Offset: 0x000F287D
	public void UpdatePosition(Extents e, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = e.x;
		entry.y = e.y;
		entry.width = e.width;
		entry.height = e.height;
		this.Insert(entry);
	}

	// Token: 0x06008035 RID: 32821 RVA: 0x00333A1C File Offset: 0x00331C1C
	public void Remove(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
		entry.obj = null;
	}

	// Token: 0x06008036 RID: 32822 RVA: 0x00333BCC File Offset: 0x00331DCC
	public void Sim1000ms(float dt)
	{
		foreach (ScenePartitioner.DirtyNode dirtyNode in this.dirtyNodes)
		{
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries.RemoveWhere(ScenePartitioner.removeCallback);
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].dirty = false;
		}
		this.dirtyNodes.Clear();
	}

	// Token: 0x06008037 RID: 32823 RVA: 0x00333C74 File Offset: 0x00331E74
	public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.queryId++;
		foreach (int cell in cells)
		{
			int x = 0;
			int y = 0;
			Grid.CellToXY(cell, out x, out y);
			this.GatherEntries(x, y, 1, 1, layer, event_data, pooledList, this.queryId);
		}
		this.RunLayerGlobalEvent(cells, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x06008038 RID: 32824 RVA: 0x00333D00 File Offset: 0x00331F00
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.GatherEntries(x, y, width, height, layer, event_data, pooledList);
		this.RunLayerGlobalEvent(x, y, width, height, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x06008039 RID: 32825 RVA: 0x00333D44 File Offset: 0x00331F44
	private void RunLayerGlobalEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			foreach (int arg in cells)
			{
				layer.OnEvent(arg, event_data);
			}
		}
	}

	// Token: 0x0600803A RID: 32826 RVA: 0x00333D9C File Offset: 0x00331F9C
	private void RunLayerGlobalEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			for (int i = y; i < y + height; i++)
			{
				for (int j = x; j < x + width; j++)
				{
					int num = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(num))
					{
						layer.OnEvent(num, event_data);
					}
				}
			}
		}
	}

	// Token: 0x0600803B RID: 32827 RVA: 0x00333DF0 File Offset: 0x00331FF0
	private void RunEntries(List<ScenePartitionerEntry> gathered_entries, object event_data)
	{
		for (int i = 0; i < gathered_entries.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = gathered_entries[i];
			if (scenePartitionerEntry.obj != null && scenePartitionerEntry.eventCallback != null)
			{
				scenePartitionerEntry.eventCallback(event_data);
			}
		}
	}

	// Token: 0x0600803C RID: 32828 RVA: 0x00333E34 File Offset: 0x00332034
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries)
	{
		int query_id = this.queryId + 1;
		this.queryId = query_id;
		this.GatherEntries(x, y, width, height, layer, event_data, gathered_entries, query_id);
	}

	// Token: 0x0600803D RID: 32829 RVA: 0x00333E64 File Offset: 0x00332064
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries, int query_id)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer2, i, j].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.queryId != this.queryId)
					{
						if (scenePartitionerEntry.obj == null)
						{
							pooledList.Add(scenePartitionerEntry);
						}
						else if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1)
						{
							scenePartitionerEntry.queryId = this.queryId;
							gathered_entries.Add(scenePartitionerEntry);
						}
					}
				}
				this.nodes[layer2, i, j].entries.ExceptWith(pooledList);
				pooledList.Recycle();
			}
		}
	}

	// Token: 0x0600803E RID: 32830 RVA: 0x000F46BD File Offset: 0x000F28BD
	public IEnumerable<object> AsyncSafeEnumerate(int x, int y, int width, int height, ScenePartitionerLayer layer)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int max_y = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num = Math.Max(nodeExtents.y, 0);
		int start_x = Math.Max(nodeExtents.x, 0);
		int max_x = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer_idx = layer.layer;
		int num2;
		for (int node_y = num; node_y < max_y; node_y = num2)
		{
			for (int node_x = start_x; node_x < max_x; node_x = num2)
			{
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer_idx, node_y, node_x].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.obj != null && x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1)
					{
						yield return scenePartitionerEntry.obj;
					}
				}
				HashSet<ScenePartitionerEntry>.Enumerator enumerator = default(HashSet<ScenePartitionerEntry>.Enumerator);
				num2 = node_x + 1;
			}
			num2 = node_y + 1;
		}
		yield break;
		yield break;
	}

	// Token: 0x0600803F RID: 32831 RVA: 0x000C09DD File Offset: 0x000BEBDD
	public void Cleanup()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06008040 RID: 32832 RVA: 0x0033400C File Offset: 0x0033220C
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		int x_bottomLeft = 0;
		int y_bottomLeft = 0;
		Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
		List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
		foreach (ScenePartitionerLayer layer in this.toggledLayers)
		{
			list.Clear();
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, layer, list);
			if (list.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04006122 RID: 24866
	public List<ScenePartitionerLayer> layers = new List<ScenePartitionerLayer>();

	// Token: 0x04006123 RID: 24867
	private int nodeSize;

	// Token: 0x04006124 RID: 24868
	private List<ScenePartitioner.DirtyNode> dirtyNodes = new List<ScenePartitioner.DirtyNode>();

	// Token: 0x04006125 RID: 24869
	private ScenePartitioner.ScenePartitionerNode[,,] nodes;

	// Token: 0x04006126 RID: 24870
	private int queryId;

	// Token: 0x04006127 RID: 24871
	private static readonly Predicate<ScenePartitionerEntry> removeCallback = (ScenePartitionerEntry entry) => entry == null || entry.obj == null;

	// Token: 0x04006128 RID: 24872
	public HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

	// Token: 0x0200183A RID: 6202
	private struct ScenePartitionerNode
	{
		// Token: 0x04006129 RID: 24873
		public HashSet<ScenePartitionerEntry> entries;

		// Token: 0x0400612A RID: 24874
		public bool dirty;
	}

	// Token: 0x0200183B RID: 6203
	private struct DirtyNode
	{
		// Token: 0x0400612B RID: 24875
		public int layer;

		// Token: 0x0400612C RID: 24876
		public int x;

		// Token: 0x0400612D RID: 24877
		public int y;
	}
}
