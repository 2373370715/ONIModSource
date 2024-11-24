using System;

// Token: 0x02001064 RID: 4196
public class CellVisibility
{
	// Token: 0x060055A0 RID: 21920 RVA: 0x000D7E1F File Offset: 0x000D601F
	public CellVisibility()
	{
		Grid.GetVisibleExtents(out this.MinX, out this.MinY, out this.MaxX, out this.MaxY);
	}

	// Token: 0x060055A1 RID: 21921 RVA: 0x0027F28C File Offset: 0x0027D48C
	public bool IsVisible(int cell)
	{
		int num = Grid.CellColumn(cell);
		if (num < this.MinX || num > this.MaxX)
		{
			return false;
		}
		int num2 = Grid.CellRow(cell);
		return num2 >= this.MinY && num2 <= this.MaxY;
	}

	// Token: 0x04003C1A RID: 15386
	private int MinX;

	// Token: 0x04003C1B RID: 15387
	private int MinY;

	// Token: 0x04003C1C RID: 15388
	private int MaxX;

	// Token: 0x04003C1D RID: 15389
	private int MaxY;
}
