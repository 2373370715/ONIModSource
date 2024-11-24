using System;
using UnityEngine;

// Token: 0x02000ACC RID: 2764
[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour
{
	// Token: 0x040022DE RID: 8926
	public Vector2I OriginOffset;

	// Token: 0x040022DF RID: 8927
	public Vector2I RangeMin;

	// Token: 0x040022E0 RID: 8928
	public Vector2I RangeMax;

	// Token: 0x040022E1 RID: 8929
	public bool TestLineOfSight = true;

	// Token: 0x040022E2 RID: 8930
	public bool BlockingTileVisible;

	// Token: 0x040022E3 RID: 8931
	public Func<int, bool> BlockingVisibleCb;

	// Token: 0x040022E4 RID: 8932
	public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);

	// Token: 0x040022E5 RID: 8933
	public bool AllowLineOfSightInvalidCells;
}
