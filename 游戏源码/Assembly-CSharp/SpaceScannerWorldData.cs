using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

[Serializable]
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceScannerWorldData
{
	public class Scratchpad
	{
		public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();

		public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers = new HashSet<MeteorShowerEvent.StatesInstance>();

		public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();

		public HashSet<Clustercraft> lastDetectedRocketsDLC1 = new HashSet<Clustercraft>();
	}

	[NonSerialized]
	private WorldContainer world;

	[Serialize]
	public int worldId;

	[Serialize]
	public float networkQuality01;

	[Serialize]
	public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();

	[Serialize]
	public HashSet<string> targetIdsDetected = new HashSet<string>();

	[NonSerialized]
	public Scratchpad scratchpad = new Scratchpad();

	[Serialize]
	public SpaceScannerWorldData(int worldId)
	{
		this.worldId = worldId;
	}

	public WorldContainer GetWorld()
	{
		if (world == null)
		{
			world = ClusterManager.Instance.GetWorld(worldId);
		}
		return world;
	}
}
