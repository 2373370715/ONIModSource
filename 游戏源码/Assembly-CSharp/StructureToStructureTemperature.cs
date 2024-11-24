using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020019BE RID: 6590
public class StructureToStructureTemperature : KMonoBehaviour
{
	// Token: 0x0600894E RID: 35150 RVA: 0x000F9D88 File Offset: 0x000F7F88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<StructureToStructureTemperature>(-1555603773, StructureToStructureTemperature.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x0600894F RID: 35151 RVA: 0x000F9DA1 File Offset: 0x000F7FA1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DefineConductiveCells();
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
	}

	// Token: 0x06008950 RID: 35152 RVA: 0x000F9DCF File Offset: 0x000F7FCF
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
		this.UnregisterToSIM();
		base.OnCleanUp();
	}

	// Token: 0x06008951 RID: 35153 RVA: 0x00357170 File Offset: 0x00355370
	private void OnStructureTemperatureRegistered(object _sim_handle)
	{
		int sim_handle = (int)_sim_handle;
		this.RegisterToSIM(sim_handle);
	}

	// Token: 0x06008952 RID: 35154 RVA: 0x0035718C File Offset: 0x0035538C
	private void RegisterToSIM(int sim_handle)
	{
		string name = this.building.Def.Name;
		SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle2, Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			this.OnSimRegistered(sim_handle);
		}, null, "StructureToStructureTemperature.SimRegister").index);
	}

	// Token: 0x06008953 RID: 35155 RVA: 0x000F9DFD File Offset: 0x000F7FFD
	private void OnSimRegistered(int sim_handle)
	{
		if (sim_handle != -1)
		{
			this.selfHandle = sim_handle;
			this.hasBeenRegister = true;
			if (this.buildingDestroyed)
			{
				this.UnregisterToSIM();
				return;
			}
			this.Refresh_InContactBuildings();
		}
	}

	// Token: 0x06008954 RID: 35156 RVA: 0x000F9E26 File Offset: 0x000F8026
	private void UnregisterToSIM()
	{
		if (this.hasBeenRegister)
		{
			SimMessages.RemoveBuildingToBuildingHeatExchange(this.selfHandle, -1);
		}
		this.buildingDestroyed = true;
	}

	// Token: 0x06008955 RID: 35157 RVA: 0x003571DC File Offset: 0x003553DC
	private void DefineConductiveCells()
	{
		this.conductiveCells = new List<int>(this.building.PlacementCells);
		this.conductiveCells.Remove(this.building.GetUtilityInputCell());
		this.conductiveCells.Remove(this.building.GetUtilityOutputCell());
	}

	// Token: 0x06008956 RID: 35158 RVA: 0x000F9E43 File Offset: 0x000F8043
	private void Add(StructureToStructureTemperature.InContactBuildingData buildingData)
	{
		if (this.inContactBuildings.Add(buildingData.buildingInContact))
		{
			SimMessages.AddBuildingToBuildingHeatExchange(this.selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
		}
	}

	// Token: 0x06008957 RID: 35159 RVA: 0x000F9E6F File Offset: 0x000F806F
	private void Remove(int building)
	{
		if (this.inContactBuildings.Contains(building))
		{
			this.inContactBuildings.Remove(building);
			SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
		}
	}

	// Token: 0x06008958 RID: 35160 RVA: 0x00357230 File Offset: 0x00355430
	private void OnAnyBuildingChanged(int _cell, object _data)
	{
		if (this.hasBeenRegister)
		{
			StructureToStructureTemperature.BuildingChangedObj buildingChangedObj = (StructureToStructureTemperature.BuildingChangedObj)_data;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < buildingChangedObj.building.PlacementCells.Length; i++)
			{
				int item = buildingChangedObj.building.PlacementCells[i];
				if (this.conductiveCells.Contains(item))
				{
					flag = true;
					num++;
				}
			}
			if (flag)
			{
				int simHandler = buildingChangedObj.simHandler;
				StructureToStructureTemperature.BuildingChangeType changeType = buildingChangedObj.changeType;
				if (changeType == StructureToStructureTemperature.BuildingChangeType.Created)
				{
					StructureToStructureTemperature.InContactBuildingData buildingData = new StructureToStructureTemperature.InContactBuildingData
					{
						buildingInContact = simHandler,
						cellsInContact = num
					};
					this.Add(buildingData);
					return;
				}
				if (changeType != StructureToStructureTemperature.BuildingChangeType.Destroyed)
				{
					return;
				}
				this.Remove(simHandler);
			}
		}
	}

	// Token: 0x06008959 RID: 35161 RVA: 0x003572DC File Offset: 0x003554DC
	private void Refresh_InContactBuildings()
	{
		foreach (StructureToStructureTemperature.InContactBuildingData buildingData in this.GetAll_InContact_Buildings())
		{
			this.Add(buildingData);
		}
	}

	// Token: 0x0600895A RID: 35162 RVA: 0x00357330 File Offset: 0x00355530
	private List<StructureToStructureTemperature.InContactBuildingData> GetAll_InContact_Buildings()
	{
		Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
		List<StructureToStructureTemperature.InContactBuildingData> list = new List<StructureToStructureTemperature.InContactBuildingData>();
		List<GameObject> buildingsInCell = new List<GameObject>();
		using (List<int>.Enumerator enumerator = this.conductiveCells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int cell = enumerator.Current;
				buildingsInCell.Clear();
				Action<int> action = delegate(int layer)
				{
					GameObject gameObject = Grid.Objects[cell, layer];
					if (gameObject != null && !buildingsInCell.Contains(gameObject))
					{
						buildingsInCell.Add(gameObject);
					}
				};
				action(1);
				action(26);
				action(27);
				action(31);
				action(32);
				action(30);
				action(12);
				action(13);
				action(16);
				action(17);
				action(24);
				action(2);
				for (int i = 0; i < buildingsInCell.Count; i++)
				{
					Building building = (buildingsInCell[i] == null) ? null : buildingsInCell[i].GetComponent<Building>();
					if (building != null && building.Def.UseStructureTemperature && building.PlacementCellsContainCell(cell))
					{
						if (!dictionary.ContainsKey(building))
						{
							dictionary.Add(building, 0);
						}
						Dictionary<Building, int> dictionary2 = dictionary;
						Building key = building;
						int num = dictionary2[key];
						dictionary2[key] = num + 1;
					}
				}
			}
		}
		foreach (Building building2 in dictionary.Keys)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(building2);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
				StructureToStructureTemperature.InContactBuildingData item = new StructureToStructureTemperature.InContactBuildingData
				{
					buildingInContact = simHandleCopy,
					cellsInContact = dictionary[building2]
				};
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x04006756 RID: 26454
	[MyCmpGet]
	private Building building;

	// Token: 0x04006757 RID: 26455
	private List<int> conductiveCells;

	// Token: 0x04006758 RID: 26456
	private HashSet<int> inContactBuildings = new HashSet<int>();

	// Token: 0x04006759 RID: 26457
	private bool hasBeenRegister;

	// Token: 0x0400675A RID: 26458
	private bool buildingDestroyed;

	// Token: 0x0400675B RID: 26459
	private int selfHandle;

	// Token: 0x0400675C RID: 26460
	protected static readonly EventSystem.IntraObjectHandler<StructureToStructureTemperature> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<StructureToStructureTemperature>(delegate(StructureToStructureTemperature component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x020019BF RID: 6591
	public enum BuildingChangeType
	{
		// Token: 0x0400675E RID: 26462
		Created,
		// Token: 0x0400675F RID: 26463
		Destroyed,
		// Token: 0x04006760 RID: 26464
		Moved
	}

	// Token: 0x020019C0 RID: 6592
	public struct InContactBuildingData
	{
		// Token: 0x04006761 RID: 26465
		public int buildingInContact;

		// Token: 0x04006762 RID: 26466
		public int cellsInContact;
	}

	// Token: 0x020019C1 RID: 6593
	public struct BuildingChangedObj
	{
		// Token: 0x0600895E RID: 35166 RVA: 0x000F9ED0 File Offset: 0x000F80D0
		public BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType _changeType, Building _building, int sim_handler)
		{
			this.changeType = _changeType;
			this.building = _building;
			this.simHandler = sim_handler;
		}

		// Token: 0x04006763 RID: 26467
		public StructureToStructureTemperature.BuildingChangeType changeType;

		// Token: 0x04006764 RID: 26468
		public int simHandler;

		// Token: 0x04006765 RID: 26469
		public Building building;
	}
}
