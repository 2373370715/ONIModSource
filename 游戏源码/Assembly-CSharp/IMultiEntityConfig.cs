using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200129D RID: 4765
public interface IMultiEntityConfig
{
	// Token: 0x0600620F RID: 25103
	List<GameObject> CreatePrefabs();

	// Token: 0x06006210 RID: 25104
	void OnPrefabInit(GameObject inst);

	// Token: 0x06006211 RID: 25105
	void OnSpawn(GameObject inst);
}
