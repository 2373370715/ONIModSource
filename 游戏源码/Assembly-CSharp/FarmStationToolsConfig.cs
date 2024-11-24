using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000399 RID: 921
public class FarmStationToolsConfig : IEntityConfig
{
	// Token: 0x06000F28 RID: 3880 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x0017BEF0 File Offset: 0x0017A0F0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FarmStationTools", ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_planttender_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AF1 RID: 2801
	public const string ID = "FarmStationTools";

	// Token: 0x04000AF2 RID: 2802
	public static readonly Tag tag = TagManager.Create("FarmStationTools");

	// Token: 0x04000AF3 RID: 2803
	public const float MASS = 5f;
}
