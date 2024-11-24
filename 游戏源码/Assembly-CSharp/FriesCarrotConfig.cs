using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class FriesCarrotConfig : IEntityConfig
{
	// Token: 0x06000C43 RID: 3139 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0017123C File Offset: 0x0016F43C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriesCarrot", STRINGS.ITEMS.FOOD.FRIESCARROT.NAME, STRINGS.ITEMS.FOOD.FRIESCARROT.DESC, 1f, false, Assets.GetAnim("rootfries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIES_CARROT);
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000936 RID: 2358
	public const string ID = "FriesCarrot";

	// Token: 0x04000937 RID: 2359
	public static ComplexRecipe recipe;
}
