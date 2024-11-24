using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200178C RID: 6028
public static class RadiationGridManager
{
	// Token: 0x06007C02 RID: 31746 RVA: 0x000F18D5 File Offset: 0x000EFAD5
	public static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06007C03 RID: 31747 RVA: 0x000F1AFD File Offset: 0x000EFCFD
	public static void Initialise()
	{
		RadiationGridManager.emitters = new List<RadiationGridEmitter>();
	}

	// Token: 0x06007C04 RID: 31748 RVA: 0x000F1B09 File Offset: 0x000EFD09
	public static void Shutdown()
	{
		RadiationGridManager.emitters.Clear();
	}

	// Token: 0x06007C05 RID: 31749 RVA: 0x0031EBF4 File Offset: 0x0031CDF4
	public static void Refresh()
	{
		for (int i = 0; i < RadiationGridManager.emitters.Count; i++)
		{
			if (RadiationGridManager.emitters[i].enabled)
			{
				RadiationGridManager.emitters[i].Emit();
			}
		}
	}

	// Token: 0x04005DBA RID: 23994
	public const float STANDARD_MASS_FALLOFF = 1000000f;

	// Token: 0x04005DBB RID: 23995
	public const int RADIATION_LINGER_RATE = 4;

	// Token: 0x04005DBC RID: 23996
	public static List<RadiationGridEmitter> emitters = new List<RadiationGridEmitter>();

	// Token: 0x04005DBD RID: 23997
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x04005DBE RID: 23998
	public static int[] previewLux;
}
