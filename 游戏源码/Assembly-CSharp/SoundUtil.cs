using System;
using UnityEngine;

// Token: 0x020018A5 RID: 6309
public static class SoundUtil
{
	// Token: 0x060082B9 RID: 33465 RVA: 0x0033DE30 File Offset: 0x0033C030
	public static float GetLiquidDepth(int cell)
	{
		float num = 0f;
		num += Grid.Mass[cell] * (Grid.Element[cell].IsLiquid ? 1f : 0f);
		int num2 = Grid.CellBelow(cell);
		if (Grid.IsValidCell(num2))
		{
			num += Grid.Mass[num2] * (Grid.Element[num2].IsLiquid ? 1f : 0f);
		}
		return Mathf.Min(num / 1000f, 1f);
	}

	// Token: 0x060082BA RID: 33466 RVA: 0x000F5F10 File Offset: 0x000F4110
	public static float GetLiquidVolume(float mass)
	{
		return Mathf.Min(mass / 100f, 1f);
	}
}
