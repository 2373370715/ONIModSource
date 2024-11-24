using System;
using UnityEngine;

// Token: 0x02000488 RID: 1160
public class RepairableStorageProxy : IEntityConfig
{
	// Token: 0x06001462 RID: 5218 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x000AEEC4 File Offset: 0x000AD0C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID, true);
		gameObject.AddOrGet<Storage>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DB2 RID: 3506
	public static string ID = "RepairableStorageProxy";
}
