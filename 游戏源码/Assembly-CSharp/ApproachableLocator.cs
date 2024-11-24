using System;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class ApproachableLocator : IEntityConfig
{
	// Token: 0x060013E7 RID: 5095 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x000AEBA5 File Offset: 0x000ACDA5
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ApproachableLocator.ID, ApproachableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		return gameObject;
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D73 RID: 3443
	public static readonly string ID = "ApproachableLocator";
}
