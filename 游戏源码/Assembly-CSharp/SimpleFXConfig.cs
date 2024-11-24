using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class SimpleFXConfig : IEntityConfig
{
	// Token: 0x0600146D RID: 5229 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x000AEF14 File Offset: 0x000AD114
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SimpleFXConfig.ID, SimpleFXConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		return gameObject;
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DB4 RID: 3508
	public static readonly string ID = "SimpleFX";
}
