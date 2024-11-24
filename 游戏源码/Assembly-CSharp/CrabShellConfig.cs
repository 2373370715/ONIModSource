using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000395 RID: 917
public class CrabShellConfig : IEntityConfig
{
	// Token: 0x06000F10 RID: 3856 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0017BCA4 File Offset: 0x00179EA4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("CrabShell", ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 10f, true, Assets.GetAnim("crabshells_large_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AE1 RID: 2785
	public const string ID = "CrabShell";

	// Token: 0x04000AE2 RID: 2786
	public static readonly Tag TAG = TagManager.Create("CrabShell");

	// Token: 0x04000AE3 RID: 2787
	public const float MASS = 10f;
}
