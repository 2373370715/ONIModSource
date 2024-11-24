using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Database
{
	// Token: 0x02002165 RID: 8549
	[DebuggerDisplay("{Id}")]
	public class SpaceDestinationType : Resource
	{
		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x0600B5E7 RID: 46567 RVA: 0x001153DF File Offset: 0x001135DF
		// (set) Token: 0x0600B5E8 RID: 46568 RVA: 0x001153E7 File Offset: 0x001135E7
		public int maxiumMass { get; private set; }

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600B5E9 RID: 46569 RVA: 0x001153F0 File Offset: 0x001135F0
		// (set) Token: 0x0600B5EA RID: 46570 RVA: 0x001153F8 File Offset: 0x001135F8
		public int minimumMass { get; private set; }

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x0600B5EB RID: 46571 RVA: 0x00115401 File Offset: 0x00113601
		public float replishmentPerCycle
		{
			get
			{
				return 1000f / (float)this.cyclesToRecover;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x0600B5EC RID: 46572 RVA: 0x00115410 File Offset: 0x00113610
		public float replishmentPerSim1000ms
		{
			get
			{
				return 1000f / ((float)this.cyclesToRecover * 600f);
			}
		}

		// Token: 0x0600B5ED RID: 46573 RVA: 0x00455278 File Offset: 0x00453478
		public SpaceDestinationType(string id, ResourceSet parent, string name, string description, int iconSize, string spriteName, Dictionary<SimHashes, MathUtil.MinMax> elementTable, Dictionary<string, int> recoverableEntities = null, ArtifactDropRate artifactDropRate = null, int max = 64000000, int min = 63994000, int cycles = 6, bool visitable = true) : base(id, parent, name)
		{
			this.typeName = name;
			this.description = description;
			this.iconSize = iconSize;
			this.spriteName = spriteName;
			this.elementTable = elementTable;
			this.recoverableEntities = recoverableEntities;
			this.artifactDropTable = artifactDropRate;
			this.maxiumMass = max;
			this.minimumMass = min;
			this.cyclesToRecover = cycles;
			this.visitable = visitable;
		}

		// Token: 0x04009428 RID: 37928
		public const float MASS_TO_RECOVER = 1000f;

		// Token: 0x04009429 RID: 37929
		public string typeName;

		// Token: 0x0400942A RID: 37930
		public string description;

		// Token: 0x0400942B RID: 37931
		public int iconSize = 128;

		// Token: 0x0400942C RID: 37932
		public string spriteName;

		// Token: 0x0400942D RID: 37933
		public Dictionary<SimHashes, MathUtil.MinMax> elementTable;

		// Token: 0x0400942E RID: 37934
		public Dictionary<string, int> recoverableEntities;

		// Token: 0x0400942F RID: 37935
		public ArtifactDropRate artifactDropTable;

		// Token: 0x04009430 RID: 37936
		public bool visitable;

		// Token: 0x04009433 RID: 37939
		public int cyclesToRecover;
	}
}
