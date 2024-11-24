using System;
using STRINGS;
using UnityEngine;

// Token: 0x020004ED RID: 1261
public class MopPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06001636 RID: 5686 RVA: 0x00196FB0 File Offset: 0x001951B0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MopPlacerConfig.ID, MISC.PLACERS.MOPPLACER.NAME, Assets.instance.mopPlacerAssets.material);
		gameObject.AddTag(GameTags.NotConversationTopic);
		Moppable moppable = gameObject.AddOrGet<Moppable>();
		moppable.synchronizeAnims = false;
		moppable.amountMoppedPerTick = 20f;
		gameObject.AddOrGet<Cancellable>();
		return gameObject;
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000EFC RID: 3836
	public static string ID = "MopPlacer";

	// Token: 0x020004EE RID: 1262
	[Serializable]
	public class MopPlacerAssets
	{
		// Token: 0x04000EFD RID: 3837
		public Material material;
	}
}
