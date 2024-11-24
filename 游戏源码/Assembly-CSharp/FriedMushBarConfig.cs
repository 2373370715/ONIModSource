using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class FriedMushBarConfig : IEntityConfig
{
	// Token: 0x06000C39 RID: 3129 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x00171174 File Offset: 0x0016F374
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushBar", STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIEDMUSHBAR);
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000932 RID: 2354
	public const string ID = "FriedMushBar";

	// Token: 0x04000933 RID: 2355
	public static ComplexRecipe recipe;
}
