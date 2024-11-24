using System;
using UnityEngine;

// Token: 0x020013D1 RID: 5073
public struct GridArea
{
	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x060067ED RID: 26605 RVA: 0x000E40E3 File Offset: 0x000E22E3
	public Vector2I Min
	{
		get
		{
			return this.min;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x060067EE RID: 26606 RVA: 0x000E40EB File Offset: 0x000E22EB
	public Vector2I Max
	{
		get
		{
			return this.max;
		}
	}

	// Token: 0x060067EF RID: 26607 RVA: 0x002D58A8 File Offset: 0x002D3AA8
	public void SetArea(int cell, int width, int height)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x + width, vector2I.y + height);
		this.SetExtents(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y);
	}

	// Token: 0x060067F0 RID: 26608 RVA: 0x002D58F4 File Offset: 0x002D3AF4
	public void SetExtents(int min_x, int min_y, int max_x, int max_y)
	{
		this.min.x = Math.Max(min_x, 0);
		this.min.y = Math.Max(min_y, 0);
		this.max.x = Math.Min(max_x, Grid.WidthInCells);
		this.max.y = Math.Min(max_y, Grid.HeightInCells);
		this.MinCell = Grid.XYToCell(this.min.x, this.min.y);
		this.MaxCell = Grid.XYToCell(this.max.x, this.max.y);
	}

	// Token: 0x060067F1 RID: 26609 RVA: 0x002D5994 File Offset: 0x002D3B94
	public bool Contains(int cell)
	{
		if (cell >= this.MinCell && cell < this.MaxCell)
		{
			int num = cell % Grid.WidthInCells;
			return num >= this.Min.x && num < this.Max.x;
		}
		return false;
	}

	// Token: 0x060067F2 RID: 26610 RVA: 0x000E40F3 File Offset: 0x000E22F3
	public bool Contains(int x, int y)
	{
		return x >= this.min.x && x < this.max.x && y >= this.min.y && y < this.max.y;
	}

	// Token: 0x060067F3 RID: 26611 RVA: 0x002D59DC File Offset: 0x002D3BDC
	public bool Contains(Vector3 pos)
	{
		return (float)this.min.x <= pos.x && pos.x < (float)this.max.x && (float)this.min.y <= pos.y && pos.y <= (float)this.max.y;
	}

	// Token: 0x060067F4 RID: 26612 RVA: 0x000E412F File Offset: 0x000E232F
	public void RunIfInside(int cell, Action<int> action)
	{
		if (this.Contains(cell))
		{
			action(cell);
		}
	}

	// Token: 0x060067F5 RID: 26613 RVA: 0x002D5A40 File Offset: 0x002D3C40
	public void Run(Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				int obj = Grid.XYToCell(j, i);
				action(obj);
			}
		}
	}

	// Token: 0x060067F6 RID: 26614 RVA: 0x002D5A9C File Offset: 0x002D3C9C
	public void RunOnDifference(GridArea subtract_area, Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				if (!subtract_area.Contains(j, i))
				{
					int obj = Grid.XYToCell(j, i);
					action(obj);
				}
			}
		}
	}

	// Token: 0x060067F7 RID: 26615 RVA: 0x000E4141 File Offset: 0x000E2341
	public int GetCellCount()
	{
		return (this.max.x - this.min.x) * (this.max.y - this.min.y);
	}

	// Token: 0x04004E75 RID: 20085
	private Vector2I min;

	// Token: 0x04004E76 RID: 20086
	private Vector2I max;

	// Token: 0x04004E77 RID: 20087
	private int MinCell;

	// Token: 0x04004E78 RID: 20088
	private int MaxCell;
}
