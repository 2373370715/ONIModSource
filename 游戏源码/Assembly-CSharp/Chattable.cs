using System;
using UnityEngine;

// Token: 0x020009AE RID: 2478
[AddComponentMenu("KMonoBehaviour/scripts/Chattable")]
public class Chattable : KMonoBehaviour, IApproachable
{
	// Token: 0x06002D62 RID: 11618 RVA: 0x000BD671 File Offset: 0x000BB871
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Chat;
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
