using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class DisposableElectrobankConfig : IMultiEntityConfig
{
	// Token: 0x06001384 RID: 4996 RVA: 0x0018DD80 File Offset: 0x0018BF80
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_RawMetal", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.DESC, 20f, SimHashes.Cuprite, "electrobank_popcan_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_BasicSingleHarvestPlant", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_MUCKROOT.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_MUCKROOT.DESC, 20f, SimHashes.Creature, "electrobank_muckroot_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_LightBugEgg", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_LIGHTBUGEGG.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_LIGHTBUGEGG.DESC, 20f, SimHashes.Creature, "electrobank_shinebug_egg_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_Sucrose", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SUCROSE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SUCROSE.DESC, 20f, SimHashes.Sucrose, "electrobank_sucrose_kanim", DlcManager.DLC3, null, "object"));
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_UraniumOre", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.DESC, 20f, SimHashes.UraniumOre, "electrobank_uranium_kanim", DlcManager.DLC3.Append(DlcManager.EXPANSION1), null, "object"));
		list.RemoveAll((GameObject t) => t == null);
		return list;
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x0018DECC File Offset: 0x0018C0CC
	private GameObject CreateDisposableElectrobank(string id, LocString name, LocString description, float mass, SimHashes element, string animName, string[] requiredDlcIDs = null, string[] forbiddenDlcIds = null, string initialAnim = "object")
	{
		if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIDs, forbiddenDlcIds))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, description, mass, true, Assets.GetAnim(animName), initialAnim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddComponent<Electrobank>();
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D32 RID: 3378
	public const string ID = "DisposableElectrobank_";

	// Token: 0x04000D33 RID: 3379
	public const float MASS = 20f;

	// Token: 0x04000D34 RID: 3380
	public static Dictionary<Tag, ComplexRecipe> recipes = new Dictionary<Tag, ComplexRecipe>();

	// Token: 0x04000D35 RID: 3381
	public const string ID_METAL_ORE = "DisposableElectrobank_RawMetal";

	// Token: 0x04000D36 RID: 3382
	public const string ID_MUCKROOT = "DisposableElectrobank_BasicSingleHarvestPlant";

	// Token: 0x04000D37 RID: 3383
	public const string ID_LIGHTBUG_EGG = "DisposableElectrobank_LightBugEgg";

	// Token: 0x04000D38 RID: 3384
	public const string ID_SUCROSE = "DisposableElectrobank_Sucrose";

	// Token: 0x04000D39 RID: 3385
	public const string ID_URANIUM_ORE = "DisposableElectrobank_UraniumOre";
}
