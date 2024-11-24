using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000321 RID: 801
public class ShellfishMeatConfig : IEntityConfig
{
	// Token: 0x06000CC0 RID: 3264 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00171E1C File Offset: 0x0017001C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ShellfishMeat", STRINGS.ITEMS.FOOD.SHELLFISHMEAT.NAME, STRINGS.ITEMS.FOOD.SHELLFISHMEAT.DESC, 1f, false, Assets.GetAnim("shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SHELLFISH_MEAT);
		return gameObject;
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000966 RID: 2406
	public const string ID = "ShellfishMeat";
}
