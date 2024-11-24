using System;
using UnityEngine;

// Token: 0x0200098A RID: 2442
public interface IApproachable
{
	// Token: 0x06002C4B RID: 11339
	CellOffset[] GetOffsets();

	// Token: 0x06002C4C RID: 11340
	int GetCell();

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06002C4D RID: 11341
	Transform transform { get; }
}
