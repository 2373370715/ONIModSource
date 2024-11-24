using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class GeneShufflerRechargeConfig : IEntityConfig
{
	// Token: 0x060013B5 RID: 5045 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x0018E568 File Offset: 0x0018C768
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("GeneShufflerRecharge", ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME, ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.DESC, 5f, true, Assets.GetAnim("vacillator_charge_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D48 RID: 3400
	public const string ID = "GeneShufflerRecharge";

	// Token: 0x04000D49 RID: 3401
	public static readonly Tag tag = TagManager.Create("GeneShufflerRecharge");

	// Token: 0x04000D4A RID: 3402
	public const float MASS = 5f;
}
