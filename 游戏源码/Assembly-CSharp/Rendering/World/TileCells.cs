using System;

namespace Rendering.World
{
	// Token: 0x020020D3 RID: 8403
	public struct TileCells
	{
		// Token: 0x0600B2BD RID: 45757 RVA: 0x00438364 File Offset: 0x00436564
		public TileCells(int tile_x, int tile_y)
		{
			int val = Grid.WidthInCells - 1;
			int val2 = Grid.HeightInCells - 1;
			this.Cell0 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell1 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell2 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(tile_y, val2));
			this.Cell3 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(tile_y, val2));
		}

		// Token: 0x04008D4C RID: 36172
		public int Cell0;

		// Token: 0x04008D4D RID: 36173
		public int Cell1;

		// Token: 0x04008D4E RID: 36174
		public int Cell2;

		// Token: 0x04008D4F RID: 36175
		public int Cell3;
	}
}
