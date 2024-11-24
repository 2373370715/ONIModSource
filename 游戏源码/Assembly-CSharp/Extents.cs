using System;
using UnityEngine;

// Token: 0x02001835 RID: 6197
public struct Extents
{
	// Token: 0x06007FFE RID: 32766 RVA: 0x00332970 File Offset: 0x00330B70
	public static Extents OneCell(int cell)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return new Extents(num, num2, 1, 1);
	}

	// Token: 0x06007FFF RID: 32767 RVA: 0x000F43F2 File Offset: 0x000F25F2
	public Extents(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x06008000 RID: 32768 RVA: 0x00332990 File Offset: 0x00330B90
	public Extents(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.x = num - radius;
		this.y = num2 - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x06008001 RID: 32769 RVA: 0x000F4411 File Offset: 0x000F2611
	public Extents(int center_x, int center_y, int radius)
	{
		this.x = center_x - radius;
		this.y = center_y - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x06008002 RID: 32770 RVA: 0x003329D4 File Offset: 0x00330BD4
	public Extents(int cell, CellOffset[] offsets)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, offset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06008003 RID: 32771 RVA: 0x00332A70 File Offset: 0x00330C70
	public Extents(int cell, CellOffset[] offsets, Extents.BoundExtendsToGridFlag _)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			int cell2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(cell2))
			{
				Grid.CellToXY(cell2, out val, out val2);
				num = Math.Min(num, val);
				num2 = Math.Min(num2, val2);
				num3 = Math.Max(num3, val);
				num4 = Math.Max(num4, val2);
			}
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06008004 RID: 32772 RVA: 0x00332B18 File Offset: 0x00330D18
	public Extents(int cell, CellOffset[] offsets, Orientation orientation)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		for (int i = 0; i < offsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offsets[i], orientation);
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06008005 RID: 32773 RVA: 0x00332BB8 File Offset: 0x00330DB8
	public Extents(int cell, CellOffset[][] offset_table)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset[] array in offset_table)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, array[0]), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06008006 RID: 32774 RVA: 0x00332C54 File Offset: 0x00330E54
	public bool Contains(Vector2I pos)
	{
		return this.x <= pos.x && pos.x < this.x + this.width && this.y <= pos.y && pos.y < this.y + this.height;
	}

	// Token: 0x06008007 RID: 32775 RVA: 0x00332CAC File Offset: 0x00330EAC
	public bool Contains(Vector3 pos)
	{
		return (float)this.x <= pos.x && pos.x < (float)(this.x + this.width) && (float)this.y <= pos.y && pos.y < (float)(this.y + this.height);
	}

	// Token: 0x040060FD RID: 24829
	public int x;

	// Token: 0x040060FE RID: 24830
	public int y;

	// Token: 0x040060FF RID: 24831
	public int width;

	// Token: 0x04006100 RID: 24832
	public int height;

	// Token: 0x04006101 RID: 24833
	public static Extents.BoundExtendsToGridFlag BoundsCheckCoords;

	// Token: 0x02001836 RID: 6198
	public struct BoundExtendsToGridFlag
	{
	}
}
