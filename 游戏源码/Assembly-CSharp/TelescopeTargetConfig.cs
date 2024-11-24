using System;
using UnityEngine;

// Token: 0x0200048D RID: 1165
public class TelescopeTargetConfig : IEntityConfig
{
	// Token: 0x0600147E RID: 5246 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x000AEF6B File Offset: 0x000AD16B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TelescopeTarget", "TelescopeTarget", true);
		gameObject.AddOrGet<TelescopeTarget>();
		return gameObject;
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DB8 RID: 3512
	public const string ID = "TelescopeTarget";
}
