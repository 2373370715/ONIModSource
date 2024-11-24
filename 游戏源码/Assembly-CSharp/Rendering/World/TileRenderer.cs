using System;
using System.Collections.Generic;

namespace Rendering.World
{
	// Token: 0x020020D5 RID: 8405
	public abstract class TileRenderer : KMonoBehaviour
	{
		// Token: 0x0600B2BF RID: 45759 RVA: 0x00438408 File Offset: 0x00436608
		protected override void OnSpawn()
		{
			this.Masks = this.GetMasks();
			this.TileGridWidth = Grid.WidthInCells + 1;
			this.TileGridHeight = Grid.HeightInCells + 1;
			this.BrushGrid = new int[this.TileGridWidth * this.TileGridHeight * 4];
			for (int i = 0; i < this.BrushGrid.Length; i++)
			{
				this.BrushGrid[i] = -1;
			}
			this.TileGrid = new Tile[this.TileGridWidth * this.TileGridHeight];
			for (int j = 0; j < this.TileGrid.Length; j++)
			{
				int tile_x = j % this.TileGridWidth;
				int tile_y = j / this.TileGridWidth;
				this.TileGrid[j] = new Tile(j, tile_x, tile_y, this.Masks.Length);
			}
			this.LoadBrushes();
			this.VisibleAreaUpdater = new VisibleAreaUpdater(new Action<int>(this.UpdateOutsideView), new Action<int>(this.UpdateInsideView), "TileRenderer");
		}

		// Token: 0x0600B2C0 RID: 45760 RVA: 0x004384F8 File Offset: 0x004366F8
		protected virtual Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 2, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, true, false),
				new Mask(this.Atlas, 2, false, false, false, false),
				new Mask(this.Atlas, 1, true, false, false, false),
				new Mask(this.Atlas, 3, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, false, false),
				new Mask(this.Atlas, 3, true, false, false, false),
				new Mask(this.Atlas, 1, true, false, true, false),
				new Mask(this.Atlas, 4, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, false, false),
				new Mask(this.Atlas, 4, false, true, false, false),
				new Mask(this.Atlas, 0, false, false, false, true)
			};
		}

		// Token: 0x0600B2C1 RID: 45761 RVA: 0x00438684 File Offset: 0x00436884
		private void UpdateInsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
				this.DirtyTiles.Add(item);
			}
		}

		// Token: 0x0600B2C2 RID: 45762 RVA: 0x004386C8 File Offset: 0x004368C8
		private void UpdateOutsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
			}
		}

		// Token: 0x0600B2C3 RID: 45763 RVA: 0x004386FC File Offset: 0x004368FC
		private int[] GetCellTiles(int cell)
		{
			int num = 0;
			int num2 = 0;
			Grid.CellToXY(cell, out num, out num2);
			this.CellTiles[0] = num2 * this.TileGridWidth + num;
			this.CellTiles[1] = num2 * this.TileGridWidth + (num + 1);
			this.CellTiles[2] = (num2 + 1) * this.TileGridWidth + num;
			this.CellTiles[3] = (num2 + 1) * this.TileGridWidth + (num + 1);
			return this.CellTiles;
		}

		// Token: 0x0600B2C4 RID: 45764
		public abstract void LoadBrushes();

		// Token: 0x0600B2C5 RID: 45765 RVA: 0x001140F9 File Offset: 0x001122F9
		public void MarkDirty(int cell)
		{
			this.VisibleAreaUpdater.UpdateCell(cell);
		}

		// Token: 0x0600B2C6 RID: 45766 RVA: 0x00438770 File Offset: 0x00436970
		private void LateUpdate()
		{
			foreach (int num in this.ClearTiles)
			{
				this.Clear(ref this.TileGrid[num], this.Brushes, this.BrushGrid);
			}
			this.ClearTiles.Clear();
			foreach (int num2 in this.DirtyTiles)
			{
				this.MarkDirty(ref this.TileGrid[num2], this.Brushes, this.BrushGrid);
			}
			this.DirtyTiles.Clear();
			this.VisibleAreaUpdater.Update();
			foreach (Brush brush in this.DirtyBrushes)
			{
				brush.Refresh();
			}
			this.DirtyBrushes.Clear();
			foreach (Brush brush2 in this.ActiveBrushes)
			{
				brush2.Render();
			}
		}

		// Token: 0x0600B2C7 RID: 45767
		public abstract void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid);

		// Token: 0x0600B2C8 RID: 45768 RVA: 0x004388E0 File Offset: 0x00436AE0
		public void Clear(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = tile.Idx * 4 + i;
				if (brush_grid[num] != -1)
				{
					brush_array[brush_grid[num]].Remove(tile.Idx);
				}
			}
		}

		// Token: 0x04008D53 RID: 36179
		private Tile[] TileGrid;

		// Token: 0x04008D54 RID: 36180
		private int[] BrushGrid;

		// Token: 0x04008D55 RID: 36181
		protected int TileGridWidth;

		// Token: 0x04008D56 RID: 36182
		protected int TileGridHeight;

		// Token: 0x04008D57 RID: 36183
		private int[] CellTiles = new int[4];

		// Token: 0x04008D58 RID: 36184
		protected Brush[] Brushes;

		// Token: 0x04008D59 RID: 36185
		protected Mask[] Masks;

		// Token: 0x04008D5A RID: 36186
		protected List<Brush> DirtyBrushes = new List<Brush>();

		// Token: 0x04008D5B RID: 36187
		protected List<Brush> ActiveBrushes = new List<Brush>();

		// Token: 0x04008D5C RID: 36188
		private VisibleAreaUpdater VisibleAreaUpdater;

		// Token: 0x04008D5D RID: 36189
		private HashSet<int> ClearTiles = new HashSet<int>();

		// Token: 0x04008D5E RID: 36190
		private HashSet<int> DirtyTiles = new HashSet<int>();

		// Token: 0x04008D5F RID: 36191
		public TextureAtlas Atlas;
	}
}
