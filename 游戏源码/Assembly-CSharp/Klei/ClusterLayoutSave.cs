using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02003AE8 RID: 15080
	public class ClusterLayoutSave
	{
		// Token: 0x0600E7C4 RID: 59332 RVA: 0x0013B0A9 File Offset: 0x001392A9
		public ClusterLayoutSave()
		{
			this.worlds = new List<ClusterLayoutSave.World>();
		}

		// Token: 0x0400E3AC RID: 58284
		public string ID;

		// Token: 0x0400E3AD RID: 58285
		public Vector2I version;

		// Token: 0x0400E3AE RID: 58286
		public List<ClusterLayoutSave.World> worlds;

		// Token: 0x0400E3AF RID: 58287
		public Vector2I size;

		// Token: 0x0400E3B0 RID: 58288
		public int currentWorldIdx;

		// Token: 0x0400E3B1 RID: 58289
		public int numRings;

		// Token: 0x0400E3B2 RID: 58290
		public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

		// Token: 0x0400E3B3 RID: 58291
		public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

		// Token: 0x02003AE9 RID: 15081
		public class World
		{
			// Token: 0x0400E3B4 RID: 58292
			public Data data = new Data();

			// Token: 0x0400E3B5 RID: 58293
			public string name = string.Empty;

			// Token: 0x0400E3B6 RID: 58294
			public bool isDiscovered;

			// Token: 0x0400E3B7 RID: 58295
			public List<string> traits = new List<string>();

			// Token: 0x0400E3B8 RID: 58296
			public List<string> storyTraits = new List<string>();

			// Token: 0x0400E3B9 RID: 58297
			public List<string> seasons = new List<string>();

			// Token: 0x0400E3BA RID: 58298
			public List<string> generatedSubworlds = new List<string>();
		}

		// Token: 0x02003AEA RID: 15082
		public enum POIType
		{
			// Token: 0x0400E3BC RID: 58300
			TemporalTear,
			// Token: 0x0400E3BD RID: 58301
			ResearchDestination
		}
	}
}
