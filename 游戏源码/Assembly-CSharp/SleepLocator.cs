using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class SleepLocator : IEntityConfig
{
	// Token: 0x060013ED RID: 5101 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x000AEBD5 File Offset: 0x000ACDD5
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SleepLocator.ID, SleepLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Sleepable>().isNormalBed = false;
		return gameObject;
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D74 RID: 3444
	public static readonly string ID = "SleepLocator";
}
