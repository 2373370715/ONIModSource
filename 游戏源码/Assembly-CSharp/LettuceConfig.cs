using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class LettuceConfig : IEntityConfig
{
	// Token: 0x06000C57 RID: 3159 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x001713F0 File Offset: 0x0016F5F0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", STRINGS.ITEMS.FOOD.LETTUCE.NAME, STRINGS.ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim("sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.LETTUCE);
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400093E RID: 2366
	public const string ID = "Lettuce";
}
