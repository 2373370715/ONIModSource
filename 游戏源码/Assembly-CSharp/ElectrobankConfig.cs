using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class ElectrobankConfig : IEntityConfig
{
	// Token: 0x06001390 RID: 5008 RVA: 0x0018E14C File Offset: 0x0018C34C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Electrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.DESC, 20f, true, Assets.GetAnim("electrobank_large_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Aluminum, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddComponent<Electrobank>().rechargeable = true;
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x000A5F37 File Offset: 0x000A4137
	public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D3C RID: 3388
	public const string ID = "Electrobank";

	// Token: 0x04000D3D RID: 3389
	public const float MASS = 20f;

	// Token: 0x04000D3E RID: 3390
	public const float POWER_CAPACITY = 120000f;

	// Token: 0x04000D3F RID: 3391
	public static ComplexRecipe recipe;
}
