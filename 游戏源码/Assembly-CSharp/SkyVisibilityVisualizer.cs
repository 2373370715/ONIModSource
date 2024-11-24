using System;
using UnityEngine;

// Token: 0x02000B08 RID: 2824
[AddComponentMenu("KMonoBehaviour/scripts/SkyVisibilityVisualizer")]
public class SkyVisibilityVisualizer : KMonoBehaviour
{
	// Token: 0x060034F9 RID: 13561 RVA: 0x000C28AF File Offset: 0x000C0AAF
	private static bool HasSkyVisibility(int cell)
	{
		return Grid.ExposedToSunlight[cell] >= 1;
	}

	// Token: 0x040023F7 RID: 9207
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040023F8 RID: 9208
	public bool TwoWideOrgin;

	// Token: 0x040023F9 RID: 9209
	public int RangeMin;

	// Token: 0x040023FA RID: 9210
	public int RangeMax;

	// Token: 0x040023FB RID: 9211
	public int ScanVerticalStep;

	// Token: 0x040023FC RID: 9212
	public bool SkipOnModuleInteriors;

	// Token: 0x040023FD RID: 9213
	public bool AllOrNothingVisibility;

	// Token: 0x040023FE RID: 9214
	public Func<int, bool> SkyVisibilityCb = new Func<int, bool>(SkyVisibilityVisualizer.HasSkyVisibility);
}
