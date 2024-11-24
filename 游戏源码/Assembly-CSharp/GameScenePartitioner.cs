using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001837 RID: 6199
[AddComponentMenu("KMonoBehaviour/scripts/GameScenePartitioner")]
public class GameScenePartitioner : KMonoBehaviour
{
	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06008008 RID: 32776 RVA: 0x000F443B File Offset: 0x000F263B
	public static GameScenePartitioner Instance
	{
		get
		{
			global::Debug.Assert(GameScenePartitioner.instance != null);
			return GameScenePartitioner.instance;
		}
	}

	// Token: 0x06008009 RID: 32777 RVA: 0x00332D08 File Offset: 0x00330F08
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GameScenePartitioner.instance == null);
		GameScenePartitioner.instance = this;
		this.partitioner = new ScenePartitioner(16, 66, Grid.WidthInCells, Grid.HeightInCells);
		this.solidChangedLayer = this.partitioner.CreateMask("SolidChanged");
		this.liquidChangedLayer = this.partitioner.CreateMask("LiquidChanged");
		this.digDestroyedLayer = this.partitioner.CreateMask("DigDestroyed");
		this.fogOfWarChangedLayer = this.partitioner.CreateMask("FogOfWarChanged");
		this.decorProviderLayer = this.partitioner.CreateMask("DecorProviders");
		this.attackableEntitiesLayer = this.partitioner.CreateMask("FactionedEntities");
		this.fetchChoreLayer = this.partitioner.CreateMask("FetchChores");
		this.pickupablesLayer = this.partitioner.CreateMask("Pickupables");
		this.storedPickupablesLayer = this.partitioner.CreateMask("StoredPickupables");
		this.pickupablesChangedLayer = this.partitioner.CreateMask("PickupablesChanged");
		this.gasConduitsLayer = this.partitioner.CreateMask("GasConduit");
		this.liquidConduitsLayer = this.partitioner.CreateMask("LiquidConduit");
		this.solidConduitsLayer = this.partitioner.CreateMask("SolidConduit");
		this.noisePolluterLayer = this.partitioner.CreateMask("NoisePolluters");
		this.validNavCellChangedLayer = this.partitioner.CreateMask("validNavCellChangedLayer");
		this.dirtyNavCellUpdateLayer = this.partitioner.CreateMask("dirtyNavCellUpdateLayer");
		this.trapsLayer = this.partitioner.CreateMask("trapsLayer");
		this.floorSwitchActivatorLayer = this.partitioner.CreateMask("FloorSwitchActivatorLayer");
		this.floorSwitchActivatorChangedLayer = this.partitioner.CreateMask("FloorSwitchActivatorChangedLayer");
		this.collisionLayer = this.partitioner.CreateMask("Collision");
		this.lure = this.partitioner.CreateMask("Lure");
		this.plants = this.partitioner.CreateMask("Plants");
		this.industrialBuildings = this.partitioner.CreateMask("IndustrialBuildings");
		this.completeBuildings = this.partitioner.CreateMask("CompleteBuildings");
		this.prioritizableObjects = this.partitioner.CreateMask("PrioritizableObjects");
		this.contactConductiveLayer = this.partitioner.CreateMask("ContactConductiveLayer");
		this.objectLayers = new ScenePartitionerLayer[45];
		for (int i = 0; i < 45; i++)
		{
			ObjectLayer objectLayer = (ObjectLayer)i;
			this.objectLayers[i] = this.partitioner.CreateMask(objectLayer.ToString());
		}
	}

	// Token: 0x0600800A RID: 32778 RVA: 0x00332FBC File Offset: 0x003311BC
	protected override void OnForcedCleanUp()
	{
		GameScenePartitioner.instance = null;
		this.partitioner.FreeResources();
		this.partitioner = null;
		this.solidChangedLayer = null;
		this.liquidChangedLayer = null;
		this.digDestroyedLayer = null;
		this.fogOfWarChangedLayer = null;
		this.decorProviderLayer = null;
		this.attackableEntitiesLayer = null;
		this.fetchChoreLayer = null;
		this.pickupablesLayer = null;
		this.storedPickupablesLayer = null;
		this.pickupablesChangedLayer = null;
		this.gasConduitsLayer = null;
		this.liquidConduitsLayer = null;
		this.solidConduitsLayer = null;
		this.noisePolluterLayer = null;
		this.validNavCellChangedLayer = null;
		this.dirtyNavCellUpdateLayer = null;
		this.trapsLayer = null;
		this.floorSwitchActivatorLayer = null;
		this.floorSwitchActivatorChangedLayer = null;
		this.contactConductiveLayer = null;
		this.objectLayers = null;
	}

	// Token: 0x0600800B RID: 32779 RVA: 0x00333074 File Offset: 0x00331274
	protected override void OnSpawn()
	{
		base.OnSpawn();
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		navGrid.OnNavGridUpdateComplete = (Action<IEnumerable<int>>)Delegate.Combine(navGrid.OnNavGridUpdateComplete, new Action<IEnumerable<int>>(this.OnNavGridUpdateComplete));
		NavTable navTable = navGrid.NavTable;
		navTable.OnValidCellChanged = (Action<int, NavType>)Delegate.Combine(navTable.OnValidCellChanged, new Action<int, NavType>(this.OnValidNavCellChanged));
	}

	// Token: 0x0600800C RID: 32780 RVA: 0x003330E0 File Offset: 0x003312E0
	public HandleVector<int>.Handle Add(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		ScenePartitionerEntry scenePartitionerEntry = new ScenePartitionerEntry(name, obj, x, y, width, height, layer, this.partitioner, event_callback);
		this.partitioner.Add(scenePartitionerEntry);
		return this.scenePartitionerEntries.Allocate(scenePartitionerEntry);
	}

	// Token: 0x0600800D RID: 32781 RVA: 0x00333120 File Offset: 0x00331320
	public HandleVector<int>.Handle Add(string name, object obj, Extents extents, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		return this.Add(name, obj, extents.x, extents.y, extents.width, extents.height, layer, event_callback);
	}

	// Token: 0x0600800E RID: 32782 RVA: 0x00333154 File Offset: 0x00331354
	public HandleVector<int>.Handle Add(string name, object obj, int cell, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return this.Add(name, obj, x, y, 1, 1, layer, event_callback);
	}

	// Token: 0x0600800F RID: 32783 RVA: 0x000F4452 File Offset: 0x000F2652
	public void AddGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Combine(layer.OnEvent, action);
	}

	// Token: 0x06008010 RID: 32784 RVA: 0x000F446B File Offset: 0x000F266B
	public void RemoveGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Remove(layer.OnEvent, action);
	}

	// Token: 0x06008011 RID: 32785 RVA: 0x000F4484 File Offset: 0x000F2684
	public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(cells, layer, event_data);
	}

	// Token: 0x06008012 RID: 32786 RVA: 0x000F4494 File Offset: 0x000F2694
	public void TriggerEvent(Extents extents, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(extents.x, extents.y, extents.width, extents.height, layer, event_data);
	}

	// Token: 0x06008013 RID: 32787 RVA: 0x000F44BB File Offset: 0x000F26BB
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(x, y, width, height, layer, event_data);
	}

	// Token: 0x06008014 RID: 32788 RVA: 0x00333180 File Offset: 0x00331380
	public void TriggerEvent(int cell, ScenePartitionerLayer layer, object event_data)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		this.TriggerEvent(x, y, 1, 1, layer, event_data);
	}

	// Token: 0x06008015 RID: 32789 RVA: 0x000F44D1 File Offset: 0x000F26D1
	public void GatherEntries(Extents extents, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.GatherEntries(extents.x, extents.y, extents.width, extents.height, layer, gathered_entries);
	}

	// Token: 0x06008016 RID: 32790 RVA: 0x000F44F3 File Offset: 0x000F26F3
	public void GatherEntries(int x_bottomLeft, int y_bottomLeft, int width, int height, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.partitioner.GatherEntries(x_bottomLeft, y_bottomLeft, width, height, layer, null, gathered_entries);
	}

	// Token: 0x06008017 RID: 32791 RVA: 0x003331A8 File Offset: 0x003313A8
	public void Iterate<IteratorType>(int x, int y, int width, int height, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x, y, width, height, layer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = pooledList[i];
			iterator.Iterate(scenePartitionerEntry.obj);
		}
		pooledList.Recycle();
	}

	// Token: 0x06008018 RID: 32792 RVA: 0x00333200 File Offset: 0x00331400
	public void Iterate<IteratorType>(int cell, int radius, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.Iterate<IteratorType>(num - radius, num2 - radius, radius * 2, radius * 2, layer, ref iterator);
	}

	// Token: 0x06008019 RID: 32793 RVA: 0x000F450A File Offset: 0x000F270A
	public IEnumerable<object> AsyncSafeEnumerate(int x, int y, int width, int height, ScenePartitionerLayer layer)
	{
		return this.partitioner.AsyncSafeEnumerate(x, y, width, height, layer);
	}

	// Token: 0x0600801A RID: 32794 RVA: 0x00333230 File Offset: 0x00331430
	public IEnumerable<object> AsyncSafeEnumerate(int cell, int radius, ScenePartitionerLayer layer)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		return this.AsyncSafeEnumerate(num - radius, num2 - radius, radius * 2, radius * 2, layer);
	}

	// Token: 0x0600801B RID: 32795 RVA: 0x000F451E File Offset: 0x000F271E
	private void OnValidNavCellChanged(int cell, NavType nav_type)
	{
		this.changedCells.Add(cell);
	}

	// Token: 0x0600801C RID: 32796 RVA: 0x00333260 File Offset: 0x00331460
	private void OnNavGridUpdateComplete(IEnumerable<int> dirty_nav_cells)
	{
		GameScenePartitioner.Instance.TriggerEvent(dirty_nav_cells, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, null);
		if (this.changedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.validNavCellChangedLayer, null);
			this.changedCells.Clear();
		}
	}

	// Token: 0x0600801D RID: 32797 RVA: 0x003332B8 File Offset: 0x003314B8
	public void UpdatePosition(HandleVector<int>.Handle handle, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.UpdatePosition(handle, vector2I.x, vector2I.y);
	}

	// Token: 0x0600801E RID: 32798 RVA: 0x000F452C File Offset: 0x000F272C
	public void UpdatePosition(HandleVector<int>.Handle handle, int x, int y)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(x, y);
	}

	// Token: 0x0600801F RID: 32799 RVA: 0x000F454B File Offset: 0x000F274B
	public void UpdatePosition(HandleVector<int>.Handle handle, Extents ext)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(ext);
	}

	// Token: 0x06008020 RID: 32800 RVA: 0x000F4569 File Offset: 0x000F2769
	public void Free(ref HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).Release();
		this.scenePartitionerEntries.Free(handle);
		handle.Clear();
	}

	// Token: 0x06008021 RID: 32801 RVA: 0x000F45A2 File Offset: 0x000F27A2
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.partitioner.Cleanup();
	}

	// Token: 0x06008022 RID: 32802 RVA: 0x000F45B5 File Offset: 0x000F27B5
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		return this.partitioner.DoDebugLayersContainItemsOnCell(cell);
	}

	// Token: 0x06008023 RID: 32803 RVA: 0x000F45C3 File Offset: 0x000F27C3
	public List<ScenePartitionerLayer> GetLayers()
	{
		return this.partitioner.layers;
	}

	// Token: 0x06008024 RID: 32804 RVA: 0x000F45D0 File Offset: 0x000F27D0
	public void SetToggledLayers(HashSet<ScenePartitionerLayer> toggled_layers)
	{
		this.partitioner.toggledLayers = toggled_layers;
	}

	// Token: 0x04006102 RID: 24834
	public ScenePartitionerLayer solidChangedLayer;

	// Token: 0x04006103 RID: 24835
	public ScenePartitionerLayer liquidChangedLayer;

	// Token: 0x04006104 RID: 24836
	public ScenePartitionerLayer digDestroyedLayer;

	// Token: 0x04006105 RID: 24837
	public ScenePartitionerLayer fogOfWarChangedLayer;

	// Token: 0x04006106 RID: 24838
	public ScenePartitionerLayer decorProviderLayer;

	// Token: 0x04006107 RID: 24839
	public ScenePartitionerLayer attackableEntitiesLayer;

	// Token: 0x04006108 RID: 24840
	public ScenePartitionerLayer fetchChoreLayer;

	// Token: 0x04006109 RID: 24841
	public ScenePartitionerLayer pickupablesLayer;

	// Token: 0x0400610A RID: 24842
	public ScenePartitionerLayer storedPickupablesLayer;

	// Token: 0x0400610B RID: 24843
	public ScenePartitionerLayer pickupablesChangedLayer;

	// Token: 0x0400610C RID: 24844
	public ScenePartitionerLayer gasConduitsLayer;

	// Token: 0x0400610D RID: 24845
	public ScenePartitionerLayer liquidConduitsLayer;

	// Token: 0x0400610E RID: 24846
	public ScenePartitionerLayer solidConduitsLayer;

	// Token: 0x0400610F RID: 24847
	public ScenePartitionerLayer wiresLayer;

	// Token: 0x04006110 RID: 24848
	public ScenePartitionerLayer[] objectLayers;

	// Token: 0x04006111 RID: 24849
	public ScenePartitionerLayer noisePolluterLayer;

	// Token: 0x04006112 RID: 24850
	public ScenePartitionerLayer validNavCellChangedLayer;

	// Token: 0x04006113 RID: 24851
	public ScenePartitionerLayer dirtyNavCellUpdateLayer;

	// Token: 0x04006114 RID: 24852
	public ScenePartitionerLayer trapsLayer;

	// Token: 0x04006115 RID: 24853
	public ScenePartitionerLayer floorSwitchActivatorLayer;

	// Token: 0x04006116 RID: 24854
	public ScenePartitionerLayer floorSwitchActivatorChangedLayer;

	// Token: 0x04006117 RID: 24855
	public ScenePartitionerLayer collisionLayer;

	// Token: 0x04006118 RID: 24856
	public ScenePartitionerLayer lure;

	// Token: 0x04006119 RID: 24857
	public ScenePartitionerLayer plants;

	// Token: 0x0400611A RID: 24858
	public ScenePartitionerLayer industrialBuildings;

	// Token: 0x0400611B RID: 24859
	public ScenePartitionerLayer completeBuildings;

	// Token: 0x0400611C RID: 24860
	public ScenePartitionerLayer prioritizableObjects;

	// Token: 0x0400611D RID: 24861
	public ScenePartitionerLayer contactConductiveLayer;

	// Token: 0x0400611E RID: 24862
	private ScenePartitioner partitioner;

	// Token: 0x0400611F RID: 24863
	private static GameScenePartitioner instance;

	// Token: 0x04006120 RID: 24864
	private KCompactedVector<ScenePartitionerEntry> scenePartitionerEntries = new KCompactedVector<ScenePartitionerEntry>(0);

	// Token: 0x04006121 RID: 24865
	private List<int> changedCells = new List<int>();

	// Token: 0x02001838 RID: 6200
	public interface Iterator
	{
		// Token: 0x06008026 RID: 32806
		void Iterate(object obj);

		// Token: 0x06008027 RID: 32807
		void Cleanup();
	}
}
