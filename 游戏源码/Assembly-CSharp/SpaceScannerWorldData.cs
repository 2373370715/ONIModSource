using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x02001995 RID: 6549
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerWorldData
{
	// Token: 0x0600888E RID: 34958 RVA: 0x000F958A File Offset: 0x000F778A
	[Serialize]
	public SpaceScannerWorldData(int worldId)
	{
		this.worldId = worldId;
	}

	// Token: 0x0600888F RID: 34959 RVA: 0x000F95BA File Offset: 0x000F77BA
	public WorldContainer GetWorld()
	{
		if (this.world == null)
		{
			this.world = ClusterManager.Instance.GetWorld(this.worldId);
		}
		return this.world;
	}

	// Token: 0x040066B6 RID: 26294
	[NonSerialized]
	private WorldContainer world;

	// Token: 0x040066B7 RID: 26295
	[Serialize]
	public int worldId;

	// Token: 0x040066B8 RID: 26296
	[Serialize]
	public float networkQuality01;

	// Token: 0x040066B9 RID: 26297
	[Serialize]
	public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();

	// Token: 0x040066BA RID: 26298
	[Serialize]
	public HashSet<string> targetIdsDetected = new HashSet<string>();

	// Token: 0x040066BB RID: 26299
	[NonSerialized]
	public SpaceScannerWorldData.Scratchpad scratchpad = new SpaceScannerWorldData.Scratchpad();

	// Token: 0x02001996 RID: 6550
	public class Scratchpad
	{
		// Token: 0x040066BC RID: 26300
		public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();

		// Token: 0x040066BD RID: 26301
		public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers = new HashSet<MeteorShowerEvent.StatesInstance>();

		// Token: 0x040066BE RID: 26302
		public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();

		// Token: 0x040066BF RID: 26303
		public HashSet<Clustercraft> lastDetectedRocketsDLC1 = new HashSet<Clustercraft>();
	}
}
