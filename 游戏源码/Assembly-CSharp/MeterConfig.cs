using System;
using UnityEngine;

// Token: 0x0200047A RID: 1146
public class MeterConfig : IEntityConfig
{
	// Token: 0x0600140A RID: 5130 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x000AEC74 File Offset: 0x000ACE74
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MeterConfig.ID, MeterConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		gameObject.AddOrGet<KBatchedAnimTracker>();
		return gameObject;
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D93 RID: 3475
	public static readonly string ID = "Meter";
}
