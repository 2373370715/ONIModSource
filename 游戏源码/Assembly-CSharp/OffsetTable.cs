using System;
using System.Collections.Generic;

// Token: 0x0200167A RID: 5754
public static class OffsetTable
{
	// Token: 0x060076E4 RID: 30436 RVA: 0x0030C0D4 File Offset: 0x0030A2D4
	public static CellOffset[][] Mirror(CellOffset[][] table)
	{
		List<CellOffset[]> list = new List<CellOffset[]>();
		foreach (CellOffset[] array in table)
		{
			list.Add(array);
			if (array[0].x != 0)
			{
				CellOffset[] array2 = new CellOffset[array.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = array[j];
					array2[j].x = -array2[j].x;
				}
				list.Add(array2);
			}
		}
		return list.ToArray();
	}
}
