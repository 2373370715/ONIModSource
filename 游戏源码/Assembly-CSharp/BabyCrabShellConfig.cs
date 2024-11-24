using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000393 RID: 915
public class BabyCrabShellConfig : IEntityConfig
{
	// Token: 0x06000F04 RID: 3844 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0017BB58 File Offset: 0x00179D58
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BabyCrabShell", ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.BABY_CRAB_SHELL.DESC, 5f, true, Assets.GetAnim("crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.Organics
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000ADA RID: 2778
	public const string ID = "BabyCrabShell";

	// Token: 0x04000ADB RID: 2779
	public static readonly Tag TAG = TagManager.Create("BabyCrabShell");

	// Token: 0x04000ADC RID: 2780
	public const float MASS = 5f;
}
