using System;

namespace Rendering.World
{
	// Token: 0x020020D4 RID: 8404
	public struct Tile
	{
		// Token: 0x0600B2BE RID: 45758 RVA: 0x001140DB File Offset: 0x001122DB
		public Tile(int idx, int tile_x, int tile_y, int mask_count)
		{
			this.Idx = idx;
			this.TileCells = new TileCells(tile_x, tile_y);
			this.MaskCount = mask_count;
		}

		// Token: 0x04008D50 RID: 36176
		public int Idx;

		// Token: 0x04008D51 RID: 36177
		public TileCells TileCells;

		// Token: 0x04008D52 RID: 36178
		public int MaskCount;
	}
}
