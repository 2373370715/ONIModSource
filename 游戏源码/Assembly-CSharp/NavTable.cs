using System;

// Token: 0x020007DE RID: 2014
public class NavTable
{
	// Token: 0x06002409 RID: 9225 RVA: 0x001C886C File Offset: 0x001C6A6C
	public NavTable(int cell_count)
	{
		this.ValidCells = new short[cell_count];
		this.NavTypeMasks = new short[11];
		for (short num = 0; num < 11; num += 1)
		{
			this.NavTypeMasks[(int)num] = (short)(1 << (int)num);
		}
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000B7663 File Offset: 0x000B5863
	public bool IsValid(int cell, NavType nav_type = NavType.Floor)
	{
		return Grid.IsValidCell(cell) && (this.NavTypeMasks[(int)nav_type] & this.ValidCells[cell]) != 0;
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x001C88B8 File Offset: 0x001C6AB8
	public void SetValid(int cell, NavType nav_type, bool is_valid)
	{
		short num = this.NavTypeMasks[(int)nav_type];
		short num2 = this.ValidCells[cell];
		if ((num2 & num) != 0 != is_valid)
		{
			if (is_valid)
			{
				this.ValidCells[cell] = (num | num2);
			}
			else
			{
				this.ValidCells[cell] = (~num & num2);
			}
			if (this.OnValidCellChanged != null)
			{
				this.OnValidCellChanged(cell, nav_type);
			}
		}
	}

	// Token: 0x04001823 RID: 6179
	public Action<int, NavType> OnValidCellChanged;

	// Token: 0x04001824 RID: 6180
	private short[] NavTypeMasks;

	// Token: 0x04001825 RID: 6181
	private short[] ValidCells;
}
