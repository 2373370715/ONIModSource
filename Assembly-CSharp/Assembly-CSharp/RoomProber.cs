﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomProber : ISim1000ms
{
	public RoomProber()
	{
		this.CellCavityID = new HandleVector<int>.Handle[Grid.CellCount];
		this.floodFiller = new RoomProber.CavityFloodFiller(this.CellCavityID);
		for (int i = 0; i < this.CellCavityID.Length; i++)
		{
			this.solidChanges.Add(i);
		}
		this.ProcessSolidChanges();
		this.RefreshRooms();
		Game instance = Game.Instance;
		instance.OnSpawnComplete = (System.Action)Delegate.Combine(instance.OnSpawnComplete, new System.Action(this.Refresh));
		World instance2 = World.Instance;
		instance2.OnSolidChanged = (Action<int>)Delegate.Combine(instance2.OnSolidChanged, new Action<int>(this.SolidChangedEvent));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingsChanged));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[2], new Action<int, object>(this.OnBuildingsChanged));
	}

	public void Refresh()
	{
		this.ProcessSolidChanges();
		this.RefreshRooms();
	}

	private void SolidChangedEvent(int cell)
	{
		this.SolidChangedEvent(cell, true);
	}

	private void OnBuildingsChanged(int cell, object building)
	{
		if (this.GetCavityForCell(cell) != null)
		{
			this.solidChanges.Add(cell);
			this.dirty = true;
		}
	}

	public void SolidChangedEvent(int cell, bool ignoreDoors)
	{
		if (ignoreDoors && Grid.HasDoor[cell])
		{
			return;
		}
		this.solidChanges.Add(cell);
		this.dirty = true;
	}

	private CavityInfo CreateNewCavity()
	{
		CavityInfo cavityInfo = new CavityInfo();
		cavityInfo.handle = this.cavityInfos.Allocate(cavityInfo);
		return cavityInfo;
	}

	private unsafe void ProcessSolidChanges()
	{
		int* ptr = stackalloc int[(UIntPtr)20];
		*ptr = 0;
		ptr[1] = -Grid.WidthInCells;
		ptr[2] = -1;
		ptr[3] = 1;
		ptr[4] = Grid.WidthInCells;
		foreach (int num in this.solidChanges)
		{
			for (int i = 0; i < 5; i++)
			{
				int num2 = num + ptr[i];
				if (Grid.IsValidCell(num2))
				{
					this.floodFillSet.Add(num2);
					HandleVector<int>.Handle item = this.CellCavityID[num2];
					if (item.IsValid())
					{
						this.CellCavityID[num2] = HandleVector<int>.InvalidHandle;
						this.releasedIDs.Add(item);
					}
				}
			}
		}
		CavityInfo cavityInfo = this.CreateNewCavity();
		foreach (int num3 in this.floodFillSet)
		{
			if (!this.visitedCells.Contains(num3))
			{
				HandleVector<int>.Handle handle = this.CellCavityID[num3];
				if (!handle.IsValid())
				{
					CavityInfo cavityInfo2 = cavityInfo;
					this.floodFiller.Reset(cavityInfo2.handle);
					GameUtil.FloodFillConditional(num3, new Func<int, bool>(this.floodFiller.ShouldContinue), this.visitedCells, null);
					if (this.floodFiller.NumCells > 0)
					{
						cavityInfo2.numCells = this.floodFiller.NumCells;
						cavityInfo2.minX = this.floodFiller.MinX;
						cavityInfo2.minY = this.floodFiller.MinY;
						cavityInfo2.maxX = this.floodFiller.MaxX;
						cavityInfo2.maxY = this.floodFiller.MaxY;
						cavityInfo = this.CreateNewCavity();
					}
				}
			}
		}
		if (cavityInfo.numCells == 0)
		{
			this.releasedIDs.Add(cavityInfo.handle);
		}
		foreach (HandleVector<int>.Handle handle2 in this.releasedIDs)
		{
			CavityInfo data = this.cavityInfos.GetData(handle2);
			this.releasedCritters.AddRange(data.creatures);
			if (data.room != null)
			{
				this.ClearRoom(data.room);
			}
			this.cavityInfos.Free(handle2);
		}
		this.RebuildDirtyCavities(this.visitedCells);
		this.releasedIDs.Clear();
		this.visitedCells.Clear();
		this.solidChanges.Clear();
		this.floodFillSet.Clear();
	}

	private void RebuildDirtyCavities(ICollection<int> visited_cells)
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (int num in visited_cells)
		{
			HandleVector<int>.Handle handle = this.CellCavityID[num];
			if (handle.IsValid())
			{
				CavityInfo data = this.cavityInfos.GetData(handle);
				if (0 < data.numCells && data.numCells <= maxRoomSize)
				{
					GameObject gameObject = Grid.Objects[num, 1];
					if (gameObject != null)
					{
						KPrefabID component = gameObject.GetComponent<KPrefabID>();
						bool flag = false;
						foreach (KPrefabID kprefabID in data.buildings)
						{
							if (component.InstanceID == kprefabID.InstanceID)
							{
								flag = true;
								break;
							}
						}
						foreach (KPrefabID kprefabID2 in data.plants)
						{
							if (component.InstanceID == kprefabID2.InstanceID)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (component.HasTag(GameTags.RoomProberBuilding))
							{
								data.AddBuilding(component);
							}
							else if (component.HasTag(GameTags.Plant) && !component.HasTag(GameTags.PlantBranch))
							{
								data.AddPlants(component);
							}
						}
					}
				}
			}
		}
		visited_cells.Clear();
	}

	public void Sim1000ms(float dt)
	{
		if (this.dirty)
		{
			this.ProcessSolidChanges();
			this.RefreshRooms();
		}
	}

	private void CreateRoom(CavityInfo cavity)
	{
		global::Debug.Assert(cavity.room == null);
		Room room = new Room();
		room.cavity = cavity;
		cavity.room = room;
		this.rooms.Add(room);
		room.roomType = Db.Get().RoomTypes.GetRoomType(room);
		this.AssignBuildingsToRoom(room);
	}

	private void ClearRoom(Room room)
	{
		this.UnassignBuildingsToRoom(room);
		room.CleanUp();
		this.rooms.Remove(room);
	}

	private void RefreshRooms()
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (CavityInfo cavityInfo in this.cavityInfos.GetDataList())
		{
			if (cavityInfo.dirty)
			{
				global::Debug.Assert(cavityInfo.room == null, "I expected info.room to always be null by this point");
				if (cavityInfo.numCells > 0)
				{
					if (cavityInfo.numCells <= maxRoomSize)
					{
						this.CreateRoom(cavityInfo);
					}
					foreach (KPrefabID kprefabID in cavityInfo.buildings)
					{
						kprefabID.Trigger(144050788, cavityInfo.room);
					}
					foreach (KPrefabID kprefabID2 in cavityInfo.plants)
					{
						kprefabID2.Trigger(144050788, cavityInfo.room);
					}
				}
				cavityInfo.dirty = false;
			}
		}
		foreach (KPrefabID kprefabID3 in this.releasedCritters)
		{
			if (kprefabID3 != null)
			{
				OvercrowdingMonitor.Instance smi = kprefabID3.GetSMI<OvercrowdingMonitor.Instance>();
				if (smi != null)
				{
					smi.RoomRefreshUpdateCavity();
				}
			}
		}
		this.releasedCritters.Clear();
		this.dirty = false;
	}

	private void AssignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		RoomType roomType = room.roomType;
		if (roomType == Db.Get().RoomTypes.Neutral)
		{
			return;
		}
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (!(kprefabID == null) && !kprefabID.HasTag(GameTags.NotRoomAssignable))
			{
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && (roomType.primary_constraint == null || !roomType.primary_constraint.building_criteria(kprefabID.GetComponent<KPrefabID>())))
				{
					component.Assign(room);
				}
			}
		}
	}

	private void UnassignKPrefabIDs(Room room, List<KPrefabID> list)
	{
		foreach (KPrefabID kprefabID in list)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, null);
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && component.assignee == room)
				{
					component.Unassign();
				}
			}
		}
	}

	private void UnassignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		this.UnassignKPrefabIDs(room, room.buildings);
		this.UnassignKPrefabIDs(room, room.plants);
	}

	public void UpdateRoom(CavityInfo cavity)
	{
		if (cavity == null)
		{
			return;
		}
		if (cavity.room != null)
		{
			this.ClearRoom(cavity.room);
			cavity.room = null;
		}
		this.CreateRoom(cavity);
		foreach (KPrefabID kprefabID in cavity.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(144050788, cavity.room);
			}
		}
		foreach (KPrefabID kprefabID2 in cavity.plants)
		{
			if (kprefabID2 != null)
			{
				kprefabID2.Trigger(144050788, cavity.room);
			}
		}
	}

	public Room GetRoomOfGameObject(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		int cell = Grid.PosToCell(go);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		CavityInfo cavityForCell = this.GetCavityForCell(cell);
		if (cavityForCell == null)
		{
			return null;
		}
		return cavityForCell.room;
	}

	public bool IsInRoomType(GameObject go, RoomType checkType)
	{
		Room roomOfGameObject = this.GetRoomOfGameObject(go);
		if (roomOfGameObject != null)
		{
			RoomType roomType = roomOfGameObject.roomType;
			return checkType == roomType;
		}
		return false;
	}

	private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
	{
		CavityInfo result = null;
		if (id.IsValid())
		{
			result = this.cavityInfos.GetData(id);
		}
		return result;
	}

	public CavityInfo GetCavityForCell(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		HandleVector<int>.Handle id = this.CellCavityID[cell];
		return this.GetCavityInfo(id);
	}

	public List<Room> rooms = new List<Room>();

	private KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024);

	private HandleVector<int>.Handle[] CellCavityID;

	private bool dirty = true;

	private HashSet<int> solidChanges = new HashSet<int>();

	private HashSet<int> visitedCells = new HashSet<int>();

	private HashSet<int> floodFillSet = new HashSet<int>();

	private HashSet<HandleVector<int>.Handle> releasedIDs = new HashSet<HandleVector<int>.Handle>();

	private RoomProber.CavityFloodFiller floodFiller;

	private List<KPrefabID> releasedCritters = new List<KPrefabID>();

	public class Tuning : TuningData<RoomProber.Tuning>
	{
		public int maxRoomSize;
	}

	private class CavityFloodFiller
	{
		public CavityFloodFiller(HandleVector<int>.Handle[] grid)
		{
			this.grid = grid;
		}

		public void Reset(HandleVector<int>.Handle search_id)
		{
			this.cavityID = search_id;
			this.numCells = 0;
			this.minX = int.MaxValue;
			this.minY = int.MaxValue;
			this.maxX = 0;
			this.maxY = 0;
		}

		private static bool IsWall(int cell)
		{
			return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || Grid.HasDoor[cell];
		}

		public bool ShouldContinue(int flood_cell)
		{
			if (RoomProber.CavityFloodFiller.IsWall(flood_cell))
			{
				this.grid[flood_cell] = HandleVector<int>.InvalidHandle;
				return false;
			}
			this.grid[flood_cell] = this.cavityID;
			int val;
			int val2;
			Grid.CellToXY(flood_cell, out val, out val2);
			this.minX = Math.Min(val, this.minX);
			this.minY = Math.Min(val2, this.minY);
			this.maxX = Math.Max(val, this.maxX);
			this.maxY = Math.Max(val2, this.maxY);
			this.numCells++;
			return true;
		}

				public int NumCells
		{
			get
			{
				return this.numCells;
			}
		}

				public int MinX
		{
			get
			{
				return this.minX;
			}
		}

				public int MinY
		{
			get
			{
				return this.minY;
			}
		}

				public int MaxX
		{
			get
			{
				return this.maxX;
			}
		}

				public int MaxY
		{
			get
			{
				return this.maxY;
			}
		}

		private HandleVector<int>.Handle[] grid;

		private HandleVector<int>.Handle cavityID;

		private int numCells;

		private int minX;

		private int minY;

		private int maxX;

		private int maxY;
	}
}
