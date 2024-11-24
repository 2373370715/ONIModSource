using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class FieldRationConfig : IEntityConfig
{
	// Token: 0x06000C2F RID: 3119 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x001710A8 File Offset: 0x0016F2A8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FieldRation", STRINGS.ITEMS.FOOD.FIELDRATION.NAME, STRINGS.ITEMS.FOOD.FIELDRATION.DESC, 1f, false, Assets.GetAnim("fieldration_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FIELDRATION);
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000930 RID: 2352
	public const string ID = "FieldRation";
}
