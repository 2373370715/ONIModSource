using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class DehydratedBerryPieConfig : IEntityConfig
{
	// Token: 0x06000BDC RID: 3036 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x001709D4 File Offset: 0x0016EBD4
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_berry_pie_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedBerryPieConfig.ID.Name, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BERRY_PIE);
		return gameObject;
	}

	// Token: 0x0400090B RID: 2315
	public static Tag ID = new Tag("DehydratedBerryPie");

	// Token: 0x0400090C RID: 2316
	public const float MASS = 1f;

	// Token: 0x0400090D RID: 2317
	public const string ANIM_FILE = "dehydrated_food_berry_pie_kanim";

	// Token: 0x0400090E RID: 2318
	public const string INITIAL_ANIM = "idle";
}
