using System;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class EyeAnimation : IEntityConfig
{
	// Token: 0x0600139F RID: 5023 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x0018E2A8 File Offset: 0x0018C4A8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(EyeAnimation.ID, EyeAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_blinks_kanim")
		};
		return gameObject;
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D43 RID: 3395
	public static string ID = "EyeAnimation";
}
