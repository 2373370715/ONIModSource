using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017F9 RID: 6137
public class RoomProber : ISim1000ms
{
	// Token: 0x06007E92 RID: 32402 RVA: 0x0032B7FC File Offset: 0x003299FC
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

	// Token: 0x06007E93 RID: 32403 RVA: 0x000F36D8 File Offset: 0x000F18D8
	public void Refresh()
	{
		this.ProcessSolidChanges();
		this.RefreshRooms();
	}

	// Token: 0x06007E94 RID: 32404 RVA: 0x000F36E6 File Offset: 0x000F18E6
	private void SolidChangedEvent(int cell)
	{
		this.SolidChangedEvent(cell, true);
	}

	// Token: 0x06007E95 RID: 32405 RVA: 0x000F36F0 File Offset: 0x000F18F0
	private void OnBuildingsChanged(int cell, object building)
	{
		if (this.GetCavityForCell(cell) != null)
		{
			this.solidChanges.Add(cell);
			this.dirty = true;
		}
	}

	// Token: 0x06007E96 RID: 32406 RVA: 0x000F370F File Offset: 0x000F190F
	public void SolidChangedEvent(int cell, bool ignoreDoors)
	{
		if (ignoreDoors && Grid.HasDoor[cell])
		{
			return;
		}
		this.solidChanges.Add(cell);
		this.dirty = true;
	}

	// Token: 0x06007E97 RID: 32407 RVA: 0x0032B948 File Offset: 0x00329B48
	private CavityInfo CreateNewCavity()
	{
		CavityInfo cavityInfo = new CavityInfo();
		cavityInfo.handle = this.cavityInfos.Allocate(cavityInfo);
		return cavityInfo;
	}

	// Token: 0x06007E98 RID: 32408 RVA: 0x0032B970 File Offset: 0x00329B70
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

	// Token: 0x06007E99 RID: 32409 RVA: 0x0032BC44 File Offset: 0x00329E44
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

	// Token: 0x06007E9A RID: 32410 RVA: 0x000F3736 File Offset: 0x000F1936
	public void Sim1000ms(float dt)
	{
		if (this.dirty)
		{
			this.ProcessSolidChanges();
			this.RefreshRooms();
		}
	}

	// Token: 0x06007E9B RID: 32411 RVA: 0x0032BE10 File Offset: 0x0032A010
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

	// Token: 0x06007E9C RID: 32412 RVA: 0x000F374C File Offset: 0x000F194C
	private void ClearRoom(Room room)
	{
		this.UnassignBuildingsToRoom(room);
		room.CleanUp();
		this.rooms.Remove(room);
	}

	// Token: 0x06007E9D RID: 32413 RVA: 0x0032BE68 File Offset: 0x0032A068
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

	// Token: 0x06007E9E RID: 32414 RVA: 0x0032C00C File Offset: 0x0032A20C
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

	// Token: 0x06007E9F RID: 32415 RVA: 0x0032C0C8 File Offset: 0x0032A2C8
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

	// Token: 0x06007EA0 RID: 32416 RVA: 0x000F3768 File Offset: 0x000F1968
	private void UnassignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		this.UnassignKPrefabIDs(room, room.buildings);
		this.UnassignKPrefabIDs(room, room.plants);
	}

	// Token: 0x06007EA1 RID: 32417 RVA: 0x0032C144 File Offset: 0x0032A344
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

	// Token: 0x06007EA2 RID: 32418 RVA: 0x0032C228 File Offset: 0x0032A428
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

	// Token: 0x06007EA3 RID: 32419 RVA: 0x0032C264 File Offset: 0x0032A464
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

	// Token: 0x06007EA4 RID: 32420 RVA: 0x0032C28C File Offset: 0x0032A48C
	private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
	{
		CavityInfo result = null;
		if (id.IsValid())
		{
			result = this.cavityInfos.GetData(id);
		}
		return result;
	}

	// Token: 0x06007EA5 RID: 32421 RVA: 0x0032C2B4 File Offset: 0x0032A4B4
	public CavityInfo GetCavityForCell(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		HandleVector<int>.Handle id = this.CellCavityID[cell];
		return this.GetCavityInfo(id);
	}

	// Token: 0x04005FEA RID: 24554
	public List<Room> rooms = new List<Room>();

	// Token: 0x04005FEB RID: 24555
	private KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024);

	// Token: 0x04005FEC RID: 24556
	private HandleVector<int>.Handle[] CellCavityID;

	// Token: 0x04005FED RID: 24557
	private bool dirty = true;

	// Token: 0x04005FEE RID: 24558
	private HashSet<int> solidChanges = new HashSet<int>();

	// Token: 0x04005FEF RID: 24559
	private HashSet<int> visitedCells = new HashSet<int>();

	// Token: 0x04005FF0 RID: 24560
	private HashSet<int> floodFillSet = new HashSet<int>();

	// Token: 0x04005FF1 RID: 24561
	private HashSet<HandleVector<int>.Handle> releasedIDs = new HashSet<HandleVector<int>.Handle>();

	// Token: 0x04005FF2 RID: 24562
	private RoomProber.CavityFloodFiller floodFiller;

	// Token: 0x04005FF3 RID: 24563
	private List<KPrefabID> releasedCritters = new List<KPrefabID>();

	// Token: 0x020017FA RID: 6138
	public class Tuning : TuningData<RoomProber.Tuning>
	{
		// Token: 0x04005FF4 RID: 24564
		public int maxRoomSize;
	}

	// Token: 0x020017FB RID: 6139
	private class CavityFloodFiller
	{
		// Token: 0x06007EA7 RID: 32423 RVA: 0x000F3795 File Offset: 0x000F1995
		public CavityFloodFiller(HandleVector<int>.Handle[] grid)
		{
			this.grid = grid;
		}

		// Token: 0x06007EA8 RID: 32424 RVA: 0x000F37A4 File Offset: 0x000F19A4
		public void Reset(HandleVector<int>.Handle search_id)
		{
			this.cavityID = search_id;
			this.numCells = 0;
			this.minX = int.MaxValue;
			this.minY = int.MaxValue;
			this.maxX = 0;
			this.maxY = 0;
		}

		// Token: 0x06007EA9 RID: 32425 RVA: 0x000F37D8 File Offset: 0x000F19D8
		private static bool IsWall(int cell)
		{
			return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || Grid.HasDoor[cell];
		}

		// Token: 0x06007EAA RID: 32426 RVA: 0x0032C2E0 File Offset: 0x0032A4E0
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

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06007EAB RID: 32427 RVA: 0x000F37F5 File Offset: 0x000F19F5
		public int NumCells
		{
			get
			{
				return this.numCells;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06007EAC RID: 32428 RVA: 0x000F37FD File Offset: 0x000F19FD
		public int MinX
		{
			get
			{
				return this.minX;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06007EAD RID: 32429 RVA: 0x000F3805 File Offset: 0x000F1A05
		public int MinY
		{
			get
			{
				return this.minY;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06007EAE RID: 32430 RVA: 0x000F380D File Offset: 0x000F1A0D
		public int MaxX
		{
			get
			{
				return this.maxX;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06007EAF RID: 32431 RVA: 0x000F3815 File Offset: 0x000F1A15
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
		}

		// Token: 0x04005FF5 RID: 24565
		private HandleVector<int>.Handle[] grid;

		// Token: 0x04005FF6 RID: 24566
		private HandleVector<int>.Handle cavityID;

		// Token: 0x04005FF7 RID: 24567
		private int numCells;

		// Token: 0x04005FF8 RID: 24568
		private int minX;

		// Token: 0x04005FF9 RID: 24569
		private int minY;

		// Token: 0x04005FFA RID: 24570
		private int maxX;

		// Token: 0x04005FFB RID: 24571
		private int maxY;
	}
}
