using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

[Serialize, SerializationConfig(MemberSerialization.OptIn), Serializable]
public class SpaceScannerWorldData {
    [Serialize]
    public float networkQuality01;

    [NonSerialized]
    public Scratchpad scratchpad = new Scratchpad();

    [Serialize]
    public HashSet<string> targetIdsDetected = new HashSet<string>();

    [Serialize]
    public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();

    [NonSerialized]
    private WorldContainer world;

    [Serialize]
    public int worldId;

    [Serialize]
    public SpaceScannerWorldData(int worldId) { this.worldId = worldId; }

    public WorldContainer GetWorld() {
        if (world == null) world = ClusterManager.Instance.GetWorld(worldId);
        return world;
    }

    public class Scratchpad {
        public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();

        public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers
            = new HashSet<MeteorShowerEvent.StatesInstance>();

        public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();
        public HashSet<Clustercraft>           lastDetectedRocketsDLC1     = new HashSet<Clustercraft>();
    }
}