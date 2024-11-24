using System;
using UnityEngine;

// Token: 0x0200098B RID: 2443
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Approachable")]
public class Approachable : KMonoBehaviour, IApproachable
{
	// Token: 0x06002C4E RID: 11342 RVA: 0x000BCAC1 File Offset: 0x000BACC1
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
