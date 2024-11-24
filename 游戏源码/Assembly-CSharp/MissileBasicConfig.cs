using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class MissileBasicConfig : IEntityConfig
{
	// Token: 0x0600143C RID: 5180 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x001905D4 File Offset: 0x0018E7D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MissileBasic", ITEMS.MISSILE_BASIC.NAME, ITEMS.MISSILE_BASIC.DESC, 10f, true, Assets.GetAnim("missile_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Iron, new List<Tag>());
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGetDef<MissileProjectile.Def>();
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 50f;
		return gameObject;
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DA8 RID: 3496
	public const string ID = "MissileBasic";

	// Token: 0x04000DA9 RID: 3497
	public static ComplexRecipe recipe;

	// Token: 0x04000DAA RID: 3498
	public const float MASS_PER_MISSILE = 10f;
}
