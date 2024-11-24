using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F8 RID: 760
public class CarrotConfig : IEntityConfig
{
	// Token: 0x06000BED RID: 3053 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00170B18 File Offset: 0x0016ED18
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(CarrotConfig.ID, STRINGS.ITEMS.FOOD.CARROT.NAME, STRINGS.ITEMS.FOOD.CARROT.DESC, 1f, false, Assets.GetAnim("purplerootVegetable_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CARROT);
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x000AB944 File Offset: 0x000A9B44
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, CarrotConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00170B7C File Offset: 0x0016ED7C
	private static void OnEatComplete(Edible edible)
	{
		if (edible != null)
		{
			int num = 0;
			float unitsConsumed = edible.unitsConsumed;
			int num2 = Mathf.FloorToInt(unitsConsumed);
			float num3 = unitsConsumed % 1f;
			if (UnityEngine.Random.value < num3)
			{
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				if (UnityEngine.Random.value < CarrotConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("CarrotPlantSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000915 RID: 2325
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x04000916 RID: 2326
	public static string ID = "Carrot";

	// Token: 0x04000917 RID: 2327
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		CarrotConfig.OnEatComplete(component);
	});
}
