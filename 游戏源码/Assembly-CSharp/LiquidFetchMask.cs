using System;

// Token: 0x02001497 RID: 5271
public class LiquidFetchMask
{
	// Token: 0x06006D3C RID: 27964 RVA: 0x002EB058 File Offset: 0x002E9258
	public LiquidFetchMask(CellOffset[][] offset_table)
	{
		for (int i = 0; i < offset_table.Length; i++)
		{
			for (int j = 0; j < offset_table[i].Length; j++)
			{
				this.maxOffset.x = Math.Max(this.maxOffset.x, Math.Abs(offset_table[i][j].x));
				this.maxOffset.y = Math.Max(this.maxOffset.y, Math.Abs(offset_table[i][j].y));
			}
		}
		this.isLiquidAvailable = new bool[Grid.CellCount];
		for (int k = 0; k < Grid.CellCount; k++)
		{
			this.RefreshCell(k);
		}
	}

	// Token: 0x06006D3D RID: 27965 RVA: 0x002EB10C File Offset: 0x002E930C
	private void RefreshCell(int cell)
	{
		CellOffset offset = Grid.GetOffset(cell);
		int num = Math.Max(0, offset.y - this.maxOffset.y);
		while (num < Grid.HeightInCells && num < offset.y + this.maxOffset.y)
		{
			int num2 = Math.Max(0, offset.x - this.maxOffset.x);
			while (num2 < Grid.WidthInCells && num2 < offset.x + this.maxOffset.x)
			{
				if (Grid.Element[Grid.XYToCell(num2, num)].IsLiquid)
				{
					this.isLiquidAvailable[cell] = true;
					return;
				}
				num2++;
			}
			num++;
		}
		this.isLiquidAvailable[cell] = false;
	}

	// Token: 0x06006D3E RID: 27966 RVA: 0x000E7A67 File Offset: 0x000E5C67
	public void MarkDirty(int cell)
	{
		this.RefreshCell(cell);
	}

	// Token: 0x06006D3F RID: 27967 RVA: 0x000E7A70 File Offset: 0x000E5C70
	public bool IsLiquidAvailable(int cell)
	{
		return this.isLiquidAvailable[cell];
	}

	// Token: 0x06006D40 RID: 27968 RVA: 0x000E7A7A File Offset: 0x000E5C7A
	public void Destroy()
	{
		this.isLiquidAvailable = null;
	}

	// Token: 0x040051F7 RID: 20983
	private bool[] isLiquidAvailable;

	// Token: 0x040051F8 RID: 20984
	private CellOffset maxOffset;
}
