using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class TableSaltConfig : IEntityConfig
{
	// Token: 0x06000CFF RID: 3327 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x001722D8 File Offset: 0x001704D8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(TableSaltConfig.ID, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC, 1f, false, Assets.GetAnim("seed_saltPlant_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + TableSaltTuning.SORTORDER, SimHashes.Salt, new List<Tag>
		{
			GameTags.Other,
			GameTags.Experimental
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000984 RID: 2436
	public static string ID = "TableSalt";
}
