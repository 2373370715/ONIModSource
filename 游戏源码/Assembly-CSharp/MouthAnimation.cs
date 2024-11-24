using System;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class MouthAnimation : IEntityConfig
{
	// Token: 0x0600144C RID: 5196 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x00190964 File Offset: 0x0018EB64
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MouthAnimation.ID, MouthAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_mouth_flap_kanim")
		};
		return gameObject;
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DAE RID: 3502
	public static string ID = "MouthAnimation";
}
