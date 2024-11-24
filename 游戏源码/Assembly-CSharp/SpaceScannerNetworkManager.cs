using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

[Serializable]
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceScannerNetworkManager : ISim1000ms
{
	[Serialize]
	private Dictionary<int, SpaceScannerWorldData> worldIdToDataMap = new Dictionary<int, SpaceScannerWorldData>();

	private static List<GameplayEventInstance> meteorShowerInstances = new List<GameplayEventInstance>();

	public Dictionary<int, SpaceScannerWorldData> DEBUG_GetWorldIdToDataMap()
	{
		return worldIdToDataMap;
	}

	public bool IsTargetDetectedOnWorld(int worldId, SpaceScannerTarget target)
	{
		if (worldIdToDataMap.TryGetValue(worldId, out var value))
		{
			return value.targetIdsDetected.Contains(target.id);
		}
		return false;
	}

	public MathUtil.MinMax GetDetectTimeRangeForWorld(int worldId)
	{
		return GetDetectTimeRange(GetQualityForWorld(worldId));
	}

	public float GetQualityForWorld(int worldId)
	{
		if (worldIdToDataMap.TryGetValue(worldId, out var value))
		{
			return value.networkQuality01;
		}
		return 0f;
	}

	private SpaceScannerWorldData GetOrCreateWorldData(int worldId)
	{
		if (!worldIdToDataMap.TryGetValue(worldId, out var value))
		{
			value = new SpaceScannerWorldData(worldId);
			worldIdToDataMap[worldId] = value;
		}
		return value;
	}

	public void Sim1000ms(float dt)
	{
		UpdateWorldDataScratchpads(worldIdToDataMap);
		foreach (int worldsId in Components.DetectorNetworks.GetWorldsIds())
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(worldsId);
			if (!world.IsModuleInterior && world.IsDiscovered)
			{
				SpaceScannerWorldData orCreateWorldData = GetOrCreateWorldData(world.id);
				UpdateNetworkQualityFor(orCreateWorldData);
				UpdateDetectionOfTargetsFor(orCreateWorldData);
			}
		}
	}

	private static void UpdateNetworkQualityFor(SpaceScannerWorldData worldData)
	{
		float num = CalcWorldNetworkQuality(worldData.GetWorld());
		foreach (DetectorNetwork.Instance item in Components.DetectorNetworks.CreateOrGetCmps(worldData.GetWorld().id))
		{
			item.Internal_SetNetworkQuality(num);
		}
		worldData.networkQuality01 = num;
	}

	private static void UpdateDetectionOfTargetsFor(SpaceScannerWorldData worldData)
	{
		using HashSetPool<string, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<string>();
		using HashSetPool<string, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet2 = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<string>();
		foreach (string item in worldData.targetIdsDetected)
		{
			pooledHashSet.Add(item);
			pooledHashSet2.Add(item);
		}
		worldData.targetIdsDetected.Clear();
		if (IsDetectingAnyMeteorShower(worldData))
		{
			worldData.targetIdsDetected.Add(SpaceScannerTarget.MeteorShower().id);
		}
		if (IsDetectingAnyBallisticObject(worldData))
		{
			worldData.targetIdsDetected.Add(SpaceScannerTarget.BallisticObject().id);
		}
		foreach (Spacecraft item2 in SpacecraftManager.instance.GetSpacecraft())
		{
			if (IsDetectingRocketBaseGame(worldData, item2.launchConditions))
			{
				worldData.targetIdsDetected.Add(SpaceScannerTarget.RocketBaseGame(item2.launchConditions).id);
			}
		}
		foreach (Clustercraft clustercraft in Components.Clustercrafts)
		{
			if (IsDetectingRocketDlc1(worldData, clustercraft))
			{
				worldData.targetIdsDetected.Add(SpaceScannerTarget.RocketDlc1(clustercraft).id);
			}
		}
		foreach (string item3 in worldData.targetIdsDetected)
		{
			pooledHashSet2.Add(item3);
		}
		foreach (string item4 in pooledHashSet2)
		{
			bool flag = pooledHashSet.Contains(item4);
			if (!worldData.targetIdsDetected.Contains(item4) && flag)
			{
				worldData.targetIdToRandomValue01Map[item4] = UnityEngine.Random.value;
			}
		}
	}

	private static bool IsDetectingAnyMeteorShower(SpaceScannerWorldData worldData)
	{
		meteorShowerInstances.Clear();
		SaveGame.Instance.GetComponent<GameplayEventManager>().GetActiveEventsOfType<MeteorShowerEvent>(worldData.GetWorld().id, ref meteorShowerInstances);
		float detectTime = GetDetectTime(worldData, SpaceScannerTarget.MeteorShower());
		MeteorShowerEvent.StatesInstance candidateTarget = null;
		float num = float.MaxValue;
		foreach (GameplayEventInstance meteorShowerInstance in meteorShowerInstances)
		{
			if (meteorShowerInstance.smi is MeteorShowerEvent.StatesInstance statesInstance)
			{
				float num2 = statesInstance.TimeUntilNextShower();
				if (num2 < num)
				{
					num = num2;
					candidateTarget = statesInstance;
				}
				if (num2 <= detectTime)
				{
					worldData.scratchpad.lastDetectedMeteorShowers.Add(statesInstance);
				}
			}
		}
		return IsDetectedUsingStickyCheck(candidateTarget, num <= detectTime, worldData.scratchpad.lastDetectedMeteorShowers);
	}

	private static bool IsDetectingAnyBallisticObject(SpaceScannerWorldData worldData)
	{
		float num = float.MaxValue;
		foreach (ClusterTraveler ballisticObject in worldData.scratchpad.ballisticObjects)
		{
			num = Mathf.Min(num, ballisticObject.TravelETA());
		}
		return num < GetDetectTime(worldData, SpaceScannerTarget.BallisticObject());
	}

	private static bool IsDetectingRocketBaseGame(SpaceScannerWorldData worldData, LaunchConditionManager rocket)
	{
		Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(rocket);
		return IsDetectedUsingStickyCheck(rocket, IsDetected(worldData, spacecraftFromLaunchConditionManager, rocket), worldData.scratchpad.lastDetectedRocketsBaseGame);
		static bool IsDetected(SpaceScannerWorldData worldData, Spacecraft spacecraft, LaunchConditionManager rocket)
		{
			if (spacecraft.IsNullOrDestroyed())
			{
				return false;
			}
			if (spacecraft.state == Spacecraft.MissionState.Destroyed)
			{
				return false;
			}
			switch (spacecraft.state)
			{
			case Spacecraft.MissionState.Destroyed:
				return false;
			case Spacecraft.MissionState.Launching:
			case Spacecraft.MissionState.WaitingToLand:
			case Spacecraft.MissionState.Landing:
				return true;
			case Spacecraft.MissionState.Underway:
				return spacecraft.GetTimeLeft() <= GetDetectTime(worldData, SpaceScannerTarget.RocketBaseGame(rocket));
			default:
				return false;
			}
		}
	}

	private static bool IsDetectingRocketDlc1(SpaceScannerWorldData worldData, Clustercraft clustercraft)
	{
		if (clustercraft.IsNullOrDestroyed())
		{
			return false;
		}
		ClusterTraveler component = clustercraft.GetComponent<ClusterTraveler>();
		bool flag = false;
		if (clustercraft.Status != 0)
		{
			bool flag2 = component.GetDestinationWorldID() == worldData.GetWorld().id;
			bool flag3 = component.IsTraveling();
			bool flag4 = clustercraft.HasResourcesToMove();
			float num = component.TravelETA();
			flag = (flag2 && flag3 && flag4 && num < GetDetectTime(worldData, SpaceScannerTarget.RocketDlc1(clustercraft))) || (!flag3 && flag2 && clustercraft.Status == Clustercraft.CraftStatus.Landing);
			if (!flag)
			{
				ClusterGridEntity adjacentAsteroid = clustercraft.GetAdjacentAsteroid();
				flag = ((adjacentAsteroid != null) ? ClusterUtil.GetAsteroidWorldIdAtLocation(adjacentAsteroid.Location) : 255) == worldData.GetWorld().id && clustercraft.Status == Clustercraft.CraftStatus.Launching;
			}
		}
		return IsDetectedUsingStickyCheck(clustercraft, flag, worldData.scratchpad.lastDetectedRocketsDLC1);
	}

	private static bool IsDetectedUsingStickyCheck<T>(T candidateTarget, bool isDetected, HashSet<T> existingDetections)
	{
		if (isDetected)
		{
			existingDetections.Add(candidateTarget);
		}
		else if (existingDetections.Contains(candidateTarget))
		{
			isDetected = true;
		}
		return isDetected;
	}

	private static float GetDetectTime(SpaceScannerWorldData worldData, SpaceScannerTarget target)
	{
		if (!worldData.targetIdToRandomValue01Map.TryGetValue(target.id, out var value))
		{
			value = UnityEngine.Random.value;
			worldData.targetIdToRandomValue01Map[target.id] = value;
		}
		return GetDetectTimeRange(worldData.networkQuality01).Lerp(value);
	}

	private static MathUtil.MinMax GetDetectTimeRange(float networkQuality01)
	{
		return new MathUtil.MinMax(Mathf.Lerp(1f, 200f, networkQuality01), 200f);
	}

	private static float CalcWorldNetworkQuality(WorldContainer world)
	{
		int width = world.Width;
		Debug.Assert(width <= 1024, "More world columns than expected");
		bool[] array = new bool[width];
		for (int i = 0; i < width; i++)
		{
			array[i] = false;
		}
		using (HashSetPool<int, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<int>())
		{
			foreach (DetectorNetwork.Instance item in Components.DetectorNetworks.CreateOrGetCmps(world.id))
			{
				if (item.GetComponent<Operational>().IsOperational)
				{
					CometDetectorConfig.SKY_VISIBILITY_INFO.CollectVisibleCellsTo(pooledHashSet, Grid.PosToCell(item.gameObject.transform.position), world);
				}
			}
			foreach (int item2 in pooledHashSet)
			{
				int num = Grid.CellToXY(item2).x - world.WorldOffset.x;
				if (num >= 0 && num < world.Width)
				{
					array[num] = true;
				}
			}
		}
		int num2 = 0;
		for (int j = 0; j < width; j++)
		{
			if (array[j])
			{
				num2++;
			}
		}
		return Mathf.Clamp01(((float)num2 / (float)width).Remap((min: 0f, max: 0.5f), (min: 0f, max: 1f)));
	}

	private static void UpdateWorldDataScratchpads(Dictionary<int, SpaceScannerWorldData> worldIdToDataMap)
	{
		foreach (KeyValuePair<int, SpaceScannerWorldData> item in worldIdToDataMap)
		{
			var (_, worldData) = item;
			if (worldData.scratchpad == null)
			{
				worldData.scratchpad = new SpaceScannerWorldData.Scratchpad();
			}
			worldData.scratchpad.ballisticObjects.Clear();
			worldData.scratchpad.lastDetectedMeteorShowers.RemoveWhere(delegate(MeteorShowerEvent.StatesInstance meteorShower)
			{
				if (meteorShower.IsNullOrDestroyed())
				{
					return true;
				}
				if (meteorShower.IsNullOrStopped())
				{
					return true;
				}
				return (200f < meteorShower.TimeUntilNextShower()) ? true : false;
			});
			worldData.scratchpad.lastDetectedRocketsBaseGame.RemoveWhere(delegate(LaunchConditionManager rocket)
			{
				if (rocket.IsNullOrDestroyed())
				{
					return true;
				}
				Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(rocket);
				if (spacecraftFromLaunchConditionManager.IsNullOrDestroyed())
				{
					return true;
				}
				if (spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Destroyed)
				{
					return true;
				}
				if (spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Underway && 200f < spacecraftFromLaunchConditionManager.GetTimeLeft())
				{
					return true;
				}
				return spacecraftFromLaunchConditionManager.GetTimeLeft() < 1f;
			});
			worldData.scratchpad.lastDetectedRocketsDLC1.RemoveWhere(delegate(Clustercraft clustercraft)
			{
				if (clustercraft.IsNullOrDestroyed())
				{
					return true;
				}
				ClusterTraveler component = clustercraft.GetComponent<ClusterTraveler>();
				if (component.IsNullOrDestroyed())
				{
					return true;
				}
				if (component.IsTraveling())
				{
					if (component.GetDestinationWorldID() != worldData.worldId)
					{
						return true;
					}
					if (200f < component.TravelETA())
					{
						return true;
					}
				}
				return component.TravelETA() < 1f;
			});
		}
		if (Components.DetectorNetworks.GetWorldsIds().Count == 0)
		{
			return;
		}
		foreach (ClusterTraveler clusterTraveler in Components.ClusterTravelers)
		{
			if (clusterTraveler.IsTraveling() && clusterTraveler.GetComponent<Clustercraft>().IsNullOrDestroyed() && worldIdToDataMap.TryGetValue(clusterTraveler.GetDestinationWorldID(), out var value))
			{
				value.scratchpad.ballisticObjects.Add(clusterTraveler);
			}
		}
	}
}
