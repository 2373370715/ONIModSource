using System;
using UnityEngine;

// Token: 0x02000CFA RID: 3322
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryInput")]
public class ConduitSecondaryInput : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x060040D7 RID: 16599 RVA: 0x000C9FC0 File Offset: 0x000C81C0
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x000C9FD0 File Offset: 0x000C81D0
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04002C4F RID: 11343
	[SerializeField]
	public ConduitPortInfo portInfo;
}
