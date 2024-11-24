using System;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class TemporalTearConfig : IEntityConfig
{
	// Token: 0x06001483 RID: 5251 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000AEF84 File Offset: 0x000AD184
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TemporalTear", "TemporalTear", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<TemporalTear>();
		return gameObject;
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DB9 RID: 3513
	public const string ID = "TemporalTear";
}
