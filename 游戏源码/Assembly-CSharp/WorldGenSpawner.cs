using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using UnityEngine;

// Token: 0x02001A53 RID: 6739
[AddComponentMenu("KMonoBehaviour/scripts/WorldGenSpawner")]
public class WorldGenSpawner : KMonoBehaviour
{
	// Token: 0x06008CEC RID: 36076 RVA: 0x000FC1E1 File Offset: 0x000FA3E1
	public bool SpawnsRemain()
	{
		return this.spawnables.Count > 0;
	}

	// Token: 0x06008CED RID: 36077 RVA: 0x00365774 File Offset: 0x00363974
	public void SpawnEverything()
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			this.spawnables[i].TrySpawn();
		}
	}

	// Token: 0x06008CEE RID: 36078 RVA: 0x003657A8 File Offset: 0x003639A8
	public void SpawnTag(string id)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (this.spawnables[i].spawnInfo.id == id)
			{
				this.spawnables[i].TrySpawn();
			}
		}
	}

	// Token: 0x06008CEF RID: 36079 RVA: 0x003657FC File Offset: 0x003639FC
	public void ClearSpawnersInArea(Vector2 root_position, CellOffset[] area)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (Grid.IsCellOffsetOf(Grid.PosToCell(root_position), this.spawnables[i].cell, area))
			{
				this.spawnables[i].FreeResources();
			}
		}
	}

	// Token: 0x06008CF0 RID: 36080 RVA: 0x000FC1F1 File Offset: 0x000FA3F1
	public IReadOnlyList<WorldGenSpawner.Spawnable> GetSpawnables()
	{
		return this.spawnables;
	}

	// Token: 0x06008CF1 RID: 36081 RVA: 0x00365850 File Offset: 0x00363A50
	protected override void OnSpawn()
	{
		if (!this.hasPlacedTemplates)
		{
			global::Debug.Assert(SaveLoader.Instance.Cluster != null, "Trying to place templates for an already-loaded save, no worldgen data available");
			this.DoReveal(SaveLoader.Instance.Cluster);
			this.PlaceTemplates(SaveLoader.Instance.Cluster);
			this.hasPlacedTemplates = true;
		}
		if (this.spawnInfos == null)
		{
			return;
		}
		for (int i = 0; i < this.spawnInfos.Length; i++)
		{
			this.AddSpawnable(this.spawnInfos[i]);
		}
	}

	// Token: 0x06008CF2 RID: 36082 RVA: 0x003658D0 File Offset: 0x00363AD0
	[OnSerializing]
	private void OnSerializing()
	{
		List<Prefab> list = new List<Prefab>();
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			WorldGenSpawner.Spawnable spawnable = this.spawnables[i];
			if (!spawnable.isSpawned)
			{
				list.Add(spawnable.spawnInfo);
			}
		}
		this.spawnInfos = list.ToArray();
	}

	// Token: 0x06008CF3 RID: 36083 RVA: 0x000FC1F9 File Offset: 0x000FA3F9
	private void AddSpawnable(Prefab prefab)
	{
		this.spawnables.Add(new WorldGenSpawner.Spawnable(prefab));
	}

	// Token: 0x06008CF4 RID: 36084 RVA: 0x00365928 File Offset: 0x00363B28
	public void AddLegacySpawner(Tag tag, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.AddSpawnable(new Prefab(tag.Name, Prefab.Type.Other, vector2I.x, vector2I.y, SimHashes.Carbon, -1f, 1f, null, 0, Orientation.Neutral, null, null, 0, null));
	}

	// Token: 0x06008CF5 RID: 36085 RVA: 0x00365974 File Offset: 0x00363B74
	public List<Tag> GetUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => !match.isSpawned && (int)Grid.WorldIdx[match.cell] == worldID && Assets.GetPrefab(match.spawnInfo.id) != null && Assets.GetPrefab(match.spawnInfo.id).GetComponent<T>() != null));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06008CF6 RID: 36086 RVA: 0x00365A10 File Offset: 0x00363C10
	public List<WorldGenSpawner.Spawnable> GeInfoOfUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => !match.isSpawned && (int)Grid.WorldIdx[match.cell] == worldID && Assets.GetPrefab(match.spawnInfo.id) != null && Assets.GetPrefab(match.spawnInfo.id).GetComponent<T>() != null));
		}
		foreach (WorldGenSpawner.Spawnable item in list2.FindAll(match2))
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06008CF7 RID: 36087 RVA: 0x00365AA0 File Offset: 0x00363CA0
	public List<Tag> GetSpawnersWithTag(Tag tag, int worldID, bool includeSpawned = false)
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => (includeSpawned || !match.isSpawned) && (int)Grid.WorldIdx[match.cell] == worldID && match.spawnInfo.id == tag));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06008CF8 RID: 36088 RVA: 0x00365B4C File Offset: 0x00363D4C
	public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(Tag tag, int worldID, bool includeSpawned = false)
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => (includeSpawned || !match.isSpawned) && (int)Grid.WorldIdx[match.cell] == worldID && match.spawnInfo.id == tag));
		}
		foreach (WorldGenSpawner.Spawnable item in list2.FindAll(match2))
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06008CF9 RID: 36089 RVA: 0x00365BE8 File Offset: 0x00363DE8
	public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(bool includeSpawned = false, params Tag[] tags)
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => includeSpawned || !match.isSpawned));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			foreach (Tag b in tags)
			{
				if (spawnable.spawnInfo.id == b)
				{
					list.Add(spawnable);
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06008CFA RID: 36090 RVA: 0x00365CB4 File Offset: 0x00363EB4
	private void PlaceTemplates(Cluster clusterLayout)
	{
		this.spawnables = new List<WorldGenSpawner.Spawnable>();
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			foreach (Prefab prefab in worldGen.SpawnData.buildings)
			{
				prefab.location_x += worldGen.data.world.offset.x;
				prefab.location_y += worldGen.data.world.offset.y;
				prefab.type = Prefab.Type.Building;
				this.AddSpawnable(prefab);
			}
			foreach (Prefab prefab2 in worldGen.SpawnData.elementalOres)
			{
				prefab2.location_x += worldGen.data.world.offset.x;
				prefab2.location_y += worldGen.data.world.offset.y;
				prefab2.type = Prefab.Type.Ore;
				this.AddSpawnable(prefab2);
			}
			foreach (Prefab prefab3 in worldGen.SpawnData.otherEntities)
			{
				prefab3.location_x += worldGen.data.world.offset.x;
				prefab3.location_y += worldGen.data.world.offset.y;
				prefab3.type = Prefab.Type.Other;
				this.AddSpawnable(prefab3);
			}
			foreach (Prefab prefab4 in worldGen.SpawnData.pickupables)
			{
				prefab4.location_x += worldGen.data.world.offset.x;
				prefab4.location_y += worldGen.data.world.offset.y;
				prefab4.type = Prefab.Type.Pickupable;
				this.AddSpawnable(prefab4);
			}
			foreach (Tag tag in worldGen.SpawnData.discoveredResources)
			{
				DiscoveredResources.Instance.Discover(tag);
			}
			worldGen.SpawnData.buildings.Clear();
			worldGen.SpawnData.elementalOres.Clear();
			worldGen.SpawnData.otherEntities.Clear();
			worldGen.SpawnData.pickupables.Clear();
			worldGen.SpawnData.discoveredResources.Clear();
		}
	}

	// Token: 0x06008CFB RID: 36091 RVA: 0x00366050 File Offset: 0x00364250
	private void DoReveal(Cluster clusterLayout)
	{
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			Game.Instance.Reset(worldGen.SpawnData, worldGen.WorldOffset);
		}
		for (int i = 0; i < Grid.CellCount; i++)
		{
			Grid.Revealed[i] = false;
			Grid.Spawnable[i] = 0;
		}
		float innerRadius = 16.5f;
		int radius = 18;
		Vector2I vector2I = clusterLayout.currentWorld.SpawnData.baseStartPos;
		vector2I += clusterLayout.currentWorld.WorldOffset;
		GridVisibility.Reveal(vector2I.x, vector2I.y, radius, innerRadius);
	}

	// Token: 0x040069F3 RID: 27123
	[Serialize]
	private Prefab[] spawnInfos;

	// Token: 0x040069F4 RID: 27124
	[Serialize]
	private bool hasPlacedTemplates;

	// Token: 0x040069F5 RID: 27125
	private List<WorldGenSpawner.Spawnable> spawnables = new List<WorldGenSpawner.Spawnable>();

	// Token: 0x02001A54 RID: 6740
	public class Spawnable
	{
		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06008CFD RID: 36093 RVA: 0x000FC21F File Offset: 0x000FA41F
		// (set) Token: 0x06008CFE RID: 36094 RVA: 0x000FC227 File Offset: 0x000FA427
		public Prefab spawnInfo { get; private set; }

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06008CFF RID: 36095 RVA: 0x000FC230 File Offset: 0x000FA430
		// (set) Token: 0x06008D00 RID: 36096 RVA: 0x000FC238 File Offset: 0x000FA438
		public bool isSpawned { get; private set; }

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06008D01 RID: 36097 RVA: 0x000FC241 File Offset: 0x000FA441
		// (set) Token: 0x06008D02 RID: 36098 RVA: 0x000FC249 File Offset: 0x000FA449
		public int cell { get; private set; }

		// Token: 0x06008D03 RID: 36099 RVA: 0x00366120 File Offset: 0x00364320
		public Spawnable(Prefab spawn_info)
		{
			this.spawnInfo = spawn_info;
			int num = Grid.XYToCell(this.spawnInfo.location_x, this.spawnInfo.location_y);
			GameObject prefab = Assets.GetPrefab(spawn_info.id);
			if (prefab != null)
			{
				WorldSpawnableMonitor.Def def = prefab.GetDef<WorldSpawnableMonitor.Def>();
				if (def != null && def.adjustSpawnLocationCb != null)
				{
					num = def.adjustSpawnLocationCb(num);
				}
			}
			this.cell = num;
			global::Debug.Assert(Grid.IsValidCell(this.cell));
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
				return;
			}
			this.fogOfWarPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnReveal", this, this.cell, GameScenePartitioner.Instance.fogOfWarChangedLayer, new Action<object>(this.OnReveal));
		}

		// Token: 0x06008D04 RID: 36100 RVA: 0x000FC252 File Offset: 0x000FA452
		private void OnReveal(object data)
		{
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
			}
		}

		// Token: 0x06008D05 RID: 36101 RVA: 0x000FC269 File Offset: 0x000FA469
		private void OnSolidChanged(object data)
		{
			if (!Grid.Solid[this.cell])
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				this.Spawn();
			}
		}

		// Token: 0x06008D06 RID: 36102 RVA: 0x003661F0 File Offset: 0x003643F0
		public void FreeResources()
		{
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				if (Game.Instance != null)
				{
					Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				}
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			this.isSpawned = true;
		}

		// Token: 0x06008D07 RID: 36103 RVA: 0x00366254 File Offset: 0x00364454
		public void TrySpawn()
		{
			if (this.isSpawned)
			{
				return;
			}
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				return;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[this.cell]);
			bool flag = world != null && world.IsDiscovered;
			GameObject prefab = Assets.GetPrefab(this.GetPrefabTag());
			if (!(prefab != null))
			{
				if (flag)
				{
					GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
					this.Spawn();
				}
				return;
			}
			if (!(flag | prefab.HasTag(GameTags.WarpTech)))
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			bool flag2 = false;
			if (prefab.GetComponent<Pickupable>() != null && !prefab.HasTag(GameTags.Creatures.Digger))
			{
				flag2 = true;
			}
			else if (prefab.GetDef<BurrowMonitor.Def>() != null)
			{
				flag2 = true;
			}
			if (flag2 && Grid.Solid[this.cell])
			{
				this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnSolidChanged", this, this.cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
				Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(this.cell);
				return;
			}
			this.Spawn();
		}

		// Token: 0x06008D08 RID: 36104 RVA: 0x00366388 File Offset: 0x00364588
		private Tag GetPrefabTag()
		{
			Mob mob = SettingsCache.mobs.GetMob(this.spawnInfo.id);
			if (mob != null && mob.prefabName != null)
			{
				return new Tag(mob.prefabName);
			}
			return new Tag(this.spawnInfo.id);
		}

		// Token: 0x06008D09 RID: 36105 RVA: 0x003663D4 File Offset: 0x003645D4
		private void Spawn()
		{
			this.isSpawned = true;
			GameObject gameObject = WorldGenSpawner.Spawnable.GetSpawnableCallback(this.spawnInfo.type)(this.spawnInfo, 0);
			if (gameObject != null && gameObject)
			{
				gameObject.SetActive(true);
				gameObject.Trigger(1119167081, this.spawnInfo);
			}
			this.FreeResources();
		}

		// Token: 0x06008D0A RID: 36106 RVA: 0x00366434 File Offset: 0x00364634
		public static WorldGenSpawner.Spawnable.PlaceEntityFn GetSpawnableCallback(Prefab.Type type)
		{
			switch (type)
			{
			case Prefab.Type.Building:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceBuilding);
			case Prefab.Type.Ore:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceElementalOres);
			case Prefab.Type.Pickupable:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlacePickupables);
			case Prefab.Type.Other:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			default:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			}
		}

		// Token: 0x040069F9 RID: 27129
		private HandleVector<int>.Handle fogOfWarPartitionerEntry;

		// Token: 0x040069FA RID: 27130
		private HandleVector<int>.Handle solidChangedPartitionerEntry;

		// Token: 0x02001A55 RID: 6741
		// (Invoke) Token: 0x06008D0C RID: 36108
		public delegate GameObject PlaceEntityFn(Prefab prefab, int root_cell);
	}
}
