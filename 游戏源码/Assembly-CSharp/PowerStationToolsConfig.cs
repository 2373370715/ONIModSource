using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class PowerStationToolsConfig : IEntityConfig
{
	// Token: 0x06000F3A RID: 3898 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0017C0AC File Offset: 0x0017A2AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PowerStationTools", ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialProduct,
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AFA RID: 2810
	public const string ID = "PowerStationTools";

	// Token: 0x04000AFB RID: 2811
	public static readonly Tag tag = TagManager.Create("PowerStationTools");

	// Token: 0x04000AFC RID: 2812
	public const float MASS = 5f;
}
