using System;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public interface IOreConfig
{
	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060015B1 RID: 5553
	SimHashes ElementID { get; }

	// Token: 0x060015B2 RID: 5554
	GameObject CreatePrefab();
}
