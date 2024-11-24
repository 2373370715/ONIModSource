using System;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class TargetLocator : IEntityConfig
{
	// Token: 0x060013E1 RID: 5089 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x000AEB7C File Offset: 0x000ACD7C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(TargetLocator.ID, TargetLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D72 RID: 3442
	public static readonly string ID = "TargetLocator";
}
