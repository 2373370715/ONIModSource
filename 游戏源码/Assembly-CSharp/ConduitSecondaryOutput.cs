using System;
using UnityEngine;

// Token: 0x02000CFB RID: 3323
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryOutput")]
public class ConduitSecondaryOutput : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x060040DA RID: 16602 RVA: 0x000C9FF1 File Offset: 0x000C81F1
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x000CA001 File Offset: 0x000C8201
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.portInfo.conduitType)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04002C50 RID: 11344
	[SerializeField]
	public ConduitPortInfo portInfo;
}
