using System;
using UnityEngine;

// Token: 0x020012A4 RID: 4772
[AddComponentMenu("KMonoBehaviour/scripts/EntityPrefabs")]
public class EntityPrefabs : KMonoBehaviour
{
	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06006224 RID: 25124 RVA: 0x000E0072 File Offset: 0x000DE272
	// (set) Token: 0x06006225 RID: 25125 RVA: 0x000E0079 File Offset: 0x000DE279
	public static EntityPrefabs Instance { get; private set; }

	// Token: 0x06006226 RID: 25126 RVA: 0x000E0081 File Offset: 0x000DE281
	public static void DestroyInstance()
	{
		EntityPrefabs.Instance = null;
	}

	// Token: 0x06006227 RID: 25127 RVA: 0x000E0089 File Offset: 0x000DE289
	protected override void OnPrefabInit()
	{
		EntityPrefabs.Instance = this;
	}

	// Token: 0x040045D9 RID: 17881
	public GameObject SelectMarker;

	// Token: 0x040045DA RID: 17882
	public GameObject ForegroundLayer;
}
