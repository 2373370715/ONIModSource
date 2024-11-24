using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02003AE3 RID: 15075
	public class Data
	{
		// Token: 0x0600E7BE RID: 59326 RVA: 0x004BF04C File Offset: 0x004BD24C
		public Data()
		{
			this.worldLayout = new WorldLayout(null, 0);
			this.terrainCells = new List<TerrainCell>();
			this.overworldCells = new List<TerrainCell>();
			this.rivers = new List<ProcGen.River>();
			this.gameSpawnData = new GameSpawnData();
			this.world = new Chunk();
			this.voronoiTree = new Tree(0);
		}

		// Token: 0x0400E38C RID: 58252
		public int globalWorldSeed;

		// Token: 0x0400E38D RID: 58253
		public int globalWorldLayoutSeed;

		// Token: 0x0400E38E RID: 58254
		public int globalTerrainSeed;

		// Token: 0x0400E38F RID: 58255
		public int globalNoiseSeed;

		// Token: 0x0400E390 RID: 58256
		public int chunkEdgeSize = 32;

		// Token: 0x0400E391 RID: 58257
		public WorldLayout worldLayout;

		// Token: 0x0400E392 RID: 58258
		public List<TerrainCell> terrainCells;

		// Token: 0x0400E393 RID: 58259
		public List<TerrainCell> overworldCells;

		// Token: 0x0400E394 RID: 58260
		public List<ProcGen.River> rivers;

		// Token: 0x0400E395 RID: 58261
		public GameSpawnData gameSpawnData;

		// Token: 0x0400E396 RID: 58262
		public Chunk world;

		// Token: 0x0400E397 RID: 58263
		public Tree voronoiTree;

		// Token: 0x0400E398 RID: 58264
		public AxialI clusterLocation;
	}
}
