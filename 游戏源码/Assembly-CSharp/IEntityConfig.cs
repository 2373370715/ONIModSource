using System;
using UnityEngine;

// Token: 0x0200129C RID: 4764
public interface IEntityConfig
{
	// Token: 0x0600620B RID: 25099
	GameObject CreatePrefab();

	// Token: 0x0600620C RID: 25100
	void OnPrefabInit(GameObject inst);

	// Token: 0x0600620D RID: 25101
	void OnSpawn(GameObject inst);

	// Token: 0x0600620E RID: 25102
	string[] GetDlcIds();
}
