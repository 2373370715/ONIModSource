using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000398 RID: 920
public class EggShellConfig : IEntityConfig
{
	// Token: 0x06000F22 RID: 3874 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0017BE70 File Offset: 0x0017A070
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EggShell", ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.DESC, 1f, false, Assets.GetAnim("eggshells_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AEE RID: 2798
	public const string ID = "EggShell";

	// Token: 0x04000AEF RID: 2799
	public static readonly Tag TAG = TagManager.Create("EggShell");

	// Token: 0x04000AF0 RID: 2800
	public const float EGG_TO_SHELL_RATIO = 0.5f;
}
