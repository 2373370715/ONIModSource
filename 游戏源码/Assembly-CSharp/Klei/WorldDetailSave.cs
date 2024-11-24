using System;
using System.Collections.Generic;
using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGenGame;

namespace Klei
{
	// Token: 0x02003AE5 RID: 15077
	public class WorldDetailSave
	{
		// Token: 0x0600E7C0 RID: 59328 RVA: 0x0013B057 File Offset: 0x00139257
		public WorldDetailSave()
		{
			this.overworldCells = new List<WorldDetailSave.OverworldCell>();
		}

		// Token: 0x0400E39E RID: 58270
		public List<WorldDetailSave.OverworldCell> overworldCells;

		// Token: 0x0400E39F RID: 58271
		public int globalWorldSeed;

		// Token: 0x0400E3A0 RID: 58272
		public int globalWorldLayoutSeed;

		// Token: 0x0400E3A1 RID: 58273
		public int globalTerrainSeed;

		// Token: 0x0400E3A2 RID: 58274
		public int globalNoiseSeed;

		// Token: 0x02003AE6 RID: 15078
		[SerializationConfig(MemberSerialization.OptOut)]
		public class OverworldCell
		{
			// Token: 0x0600E7C1 RID: 59329 RVA: 0x000A5E2C File Offset: 0x000A402C
			public OverworldCell()
			{
			}

			// Token: 0x0600E7C2 RID: 59330 RVA: 0x0013B06A File Offset: 0x0013926A
			public OverworldCell(SubWorld.ZoneType zoneType, TerrainCell tc)
			{
				this.poly = tc.poly;
				this.tags = tc.node.tags;
				this.zoneType = zoneType;
			}

			// Token: 0x0400E3A3 RID: 58275
			public Polygon poly;

			// Token: 0x0400E3A4 RID: 58276
			public TagSet tags;

			// Token: 0x0400E3A5 RID: 58277
			public SubWorld.ZoneType zoneType;
		}
	}
}
