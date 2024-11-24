using System;

// Token: 0x020007F9 RID: 2041
public class CellOffsetQuery : CellArrayQuery
{
	// Token: 0x0600247F RID: 9343 RVA: 0x001C9C28 File Offset: 0x001C7E28
	public CellArrayQuery Reset(int cell, CellOffset[] offsets)
	{
		int[] array = new int[offsets.Length];
		for (int i = 0; i < offsets.Length; i++)
		{
			array[i] = Grid.OffsetCell(cell, offsets[i]);
		}
		base.Reset(array);
		return this;
	}
}
