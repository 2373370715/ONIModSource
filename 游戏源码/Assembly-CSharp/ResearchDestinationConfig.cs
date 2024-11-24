using System;
using UnityEngine;

// Token: 0x02000489 RID: 1161
public class ResearchDestinationConfig : IEntityConfig
{
	// Token: 0x06001468 RID: 5224 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x000AEEF4 File Offset: 0x000AD0F4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ResearchDestination", "ResearchDestination", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ResearchDestination>();
		return gameObject;
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DB3 RID: 3507
	public const string ID = "ResearchDestination";
}
