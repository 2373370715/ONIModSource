using System;
using UnityEngine;

// Token: 0x02000FFA RID: 4090
public interface IUsable
{
	// Token: 0x06005350 RID: 21328
	bool IsUsable();

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06005351 RID: 21329
	Transform transform { get; }
}
