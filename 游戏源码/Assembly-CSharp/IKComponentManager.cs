using System;
using UnityEngine;

// Token: 0x02000CB4 RID: 3252
public interface IKComponentManager
{
	// Token: 0x06003EE7 RID: 16103
	HandleVector<int>.Handle Add(GameObject go);

	// Token: 0x06003EE8 RID: 16104
	void Remove(GameObject go);
}
