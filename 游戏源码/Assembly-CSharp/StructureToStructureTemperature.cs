using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureToStructureTemperature : KMonoBehaviour
{
	public enum BuildingChangeType
	{
		Created,
		Destroyed,
		Moved
	}

	public struct InContactBuildingData
	{
		public int buildingInContact;

		public int cellsInContact;
	}

	public struct BuildingChangedObj
	{
		public BuildingChangeType changeType;

		public int simHandler;

		public Building building;

		public BuildingChangedObj(BuildingChangeType _changeType, Building _building, int sim_handler)
		{
			changeType = _changeType;
			building = _building;
			simHandler = sim_handler;
		}
	}

	[MyCmpGet]
	private Building building;

	private List<int> conductiveCells;

	private HashSet<int> inContactBuildings = new HashSet<int>();

	private bool hasBeenRegister;

	private bool buildingDestroyed;

	private int selfHandle;

	protected static readonly EventSystem.IntraObjectHandler<StructureToStructureTemperature> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<StructureToStructureTemperature>(delegate(StructureToStructureTemperature component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-1555603773, OnStructureTemperatureRegisteredDelegate);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		DefineConductiveCells();
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, OnAnyBuildingChanged);
	}

	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, OnAnyBuildingChanged);
		UnregisterToSIM();
		base.OnCleanUp();
	}

	private void OnStructureTemperatureRegistered(object _sim_handle)
	{
		int sim_handle = (int)_sim_handle;
		RegisterToSIM(sim_handle);
	}

	private void RegisterToSIM(int sim_handle)
	{
		_ = building.Def.Name;
		SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle, Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			OnSimRegistered(sim_handle);
		}, null, "StructureToStructureTemperature.SimRegister").index);
	}

	private void OnSimRegistered(int sim_handle)
	{
		if (sim_handle != -1)
		{
			selfHandle = sim_handle;
			hasBeenRegister = true;
			if (buildingDestroyed)
			{
				UnregisterToSIM();
			}
			else
			{
				Refresh_InContactBuildings();
			}
		}
	}

	private void UnregisterToSIM()
	{
		if (hasBeenRegister)
		{
			SimMessages.RemoveBuildingToBuildingHeatExchange(selfHandle);
		}
		buildingDestroyed = true;
	}

	private void DefineConductiveCells()
	{
		conductiveCells = new List<int>(building.PlacementCells);
		conductiveCells.Remove(building.GetUtilityInputCell());
		conductiveCells.Remove(building.GetUtilityOutputCell());
	}

	private void Add(InContactBuildingData buildingData)
	{
		if (inContactBuildings.Add(buildingData.buildingInContact))
		{
			SimMessages.AddBuildingToBuildingHeatExchange(selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
		}
	}

	private void Remove(int building)
	{
		if (inContactBuildings.Contains(building))
		{
			inContactBuildings.Remove(building);
			SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(selfHandle, building);
		}
	}

	private void OnAnyBuildingChanged(int _cell, object _data)
	{
		if (!hasBeenRegister)
		{
			return;
		}
		BuildingChangedObj buildingChangedObj = (BuildingChangedObj)_data;
		bool flag = false;
		int num = 0;
		for (int i = 0; i < buildingChangedObj.building.PlacementCells.Length; i++)
		{
			int item = buildingChangedObj.building.PlacementCells[i];
			if (conductiveCells.Contains(item))
			{
				flag = true;
				num++;
			}
		}
		if (flag)
		{
			int simHandler = buildingChangedObj.simHandler;
			switch (buildingChangedObj.changeType)
			{
			case BuildingChangeType.Created:
			{
				InContactBuildingData inContactBuildingData = default(InContactBuildingData);
				inContactBuildingData.buildingInContact = simHandler;
				inContactBuildingData.cellsInContact = num;
				InContactBuildingData buildingData = inContactBuildingData;
				Add(buildingData);
				break;
			}
			case BuildingChangeType.Destroyed:
				Remove(simHandler);
				break;
			}
		}
	}

	private void Refresh_InContactBuildings()
	{
		foreach (InContactBuildingData all_InContact_Building in GetAll_InContact_Buildings())
		{
			Add(all_InContact_Building);
		}
	}

	private List<InContactBuildingData> GetAll_InContact_Buildings()
	{
		Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
		List<InContactBuildingData> list = new List<InContactBuildingData>();
		List<GameObject> buildingsInCell = new List<GameObject>();
		foreach (int cell in conductiveCells)
		{
			buildingsInCell.Clear();
			Action<int> obj = delegate(int layer)
			{
				GameObject gameObject = Grid.Objects[cell, layer];
				if (gameObject != null && !buildingsInCell.Contains(gameObject))
				{
					buildingsInCell.Add(gameObject);
				}
			};
			obj(1);
			obj(26);
			obj(27);
			obj(31);
			obj(32);
			obj(30);
			obj(12);
			obj(13);
			obj(16);
			obj(17);
			obj(24);
			obj(2);
			for (int i = 0; i < buildingsInCell.Count; i++)
			{
				Building building = ((buildingsInCell[i] == null) ? null : buildingsInCell[i].GetComponent<Building>());
				if (building != null && building.Def.UseStructureTemperature && building.PlacementCellsContainCell(cell))
				{
					if (!dictionary.ContainsKey(building))
					{
						dictionary.Add(building, 0);
					}
					dictionary[building]++;
				}
			}
		}
		foreach (Building key in dictionary.Keys)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(key);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
				InContactBuildingData inContactBuildingData = default(InContactBuildingData);
				inContactBuildingData.buildingInContact = simHandleCopy;
				inContactBuildingData.cellsInContact = dictionary[key];
				InContactBuildingData item = inContactBuildingData;
				list.Add(item);
			}
		}
		return list;
	}
}
