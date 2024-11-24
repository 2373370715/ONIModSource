using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000463 RID: 1123
public class EmptyElectrobankConfig : IEntityConfig
{
	// Token: 0x06001395 RID: 5013 RVA: 0x0018E200 File Offset: 0x0018C400
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EmptyElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.DESC, 20f, true, Assets.GetAnim("electrobank_large_depleted_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Aluminum, new List<Tag>
		{
			GameTags.EmptyPortableBattery,
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x000A5F37 File Offset: 0x000A4137
	public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D40 RID: 3392
	public const string ID = "EmptyElectrobank";

	// Token: 0x04000D41 RID: 3393
	public const float MASS = 20f;
}
