using System;
using STRINGS;
using UnityEngine;

// Token: 0x020004EF RID: 1263
public class MovePickupablePlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x0600163C RID: 5692 RVA: 0x0019700C File Offset: 0x0019520C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MovePickupablePlacerConfig.ID, MISC.PLACERS.MOVEPICKUPABLEPLACER.NAME, Assets.instance.movePickupToPlacerAssets.material);
		gameObject.AddOrGet<CancellableMove>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.showUnreachableStatus = true;
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000EFE RID: 3838
	public static string ID = "MovePickupablePlacer";

	// Token: 0x020004F0 RID: 1264
	[Serializable]
	public class MovePickupablePlacerAssets
	{
		// Token: 0x04000EFF RID: 3839
		public Material material;
	}
}
