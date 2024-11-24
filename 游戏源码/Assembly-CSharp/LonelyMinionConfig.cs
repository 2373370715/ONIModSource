using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class LonelyMinionConfig : IEntityConfig
{
	// Token: 0x060013FD RID: 5117 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x0018F934 File Offset: 0x0018DB34
	public GameObject CreatePrefab()
	{
		string name = DUPLICANTS.MODEL.STANDARD.NAME;
		GameObject gameObject = EntityTemplates.CreateEntity(LonelyMinionConfig.ID, name, true);
		gameObject.AddComponent<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		gameObject.AddComponent<Storage>().doDiseaseTransfer = false;
		gameObject.AddComponent<StateMachineController>();
		LonelyMinion.Def def = gameObject.AddOrGetDef<LonelyMinion.Def>();
		def.Personality = Db.Get().Personalities.Get("JORGE");
		def.Personality.Disabled = true;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.defaultAnim = "idle_default";
		kbatchedAnimController.initialAnim = "idle_default";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_interacts_lonely_dupe_kanim")
		};
		this.ConfigurePackageOverride(gameObject);
		SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		symbolOverrideController.applySymbolOverridesEveryFrame = true;
		symbolOverrideController.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(string.Format("cheek_00{0}", def.Personality.headShape)), 1);
		BaseMinionConfig.ConfigureSymbols(gameObject, true);
		return gameObject;
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x0018FA78 File Offset: 0x0018DC78
	private void ConfigurePackageOverride(GameObject go)
	{
		GameObject gameObject = new GameObject("PackageSnapPoint");
		gameObject.transform.SetParent(go.transform);
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.transform.position = Vector3.forward * -0.1f;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		kbatchedAnimController.initialAnim = "object";
		component.SetSymbolVisiblity(LonelyMinionConfig.PARCEL_SNAPTO, false);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.controller = component;
		kbatchedAnimTracker.symbol = LonelyMinionConfig.PARCEL_SNAPTO;
	}

	// Token: 0x04000D77 RID: 3447
	public static string ID = "LonelyMinion";

	// Token: 0x04000D78 RID: 3448
	public const int VOICE_IDX = -2;

	// Token: 0x04000D79 RID: 3449
	public const int STARTING_SKILL_POINTS = 3;

	// Token: 0x04000D7A RID: 3450
	public const int BASE_ATTRIBUTE_LEVEL = 7;

	// Token: 0x04000D7B RID: 3451
	public const int AGE_MIN = 2190;

	// Token: 0x04000D7C RID: 3452
	public const int AGE_MAX = 3102;

	// Token: 0x04000D7D RID: 3453
	public const float MIN_IDLE_DELAY = 20f;

	// Token: 0x04000D7E RID: 3454
	public const float MAX_IDLE_DELAY = 40f;

	// Token: 0x04000D7F RID: 3455
	public const string IDLE_PREFIX = "idle_blinds";

	// Token: 0x04000D80 RID: 3456
	public static readonly HashedString GreetingCriteraId = "Neighbor";

	// Token: 0x04000D81 RID: 3457
	public static readonly HashedString FoodCriteriaId = "FoodQuality";

	// Token: 0x04000D82 RID: 3458
	public static readonly HashedString DecorCriteriaId = "Decor";

	// Token: 0x04000D83 RID: 3459
	public static readonly HashedString PowerCriteriaId = "SuppliedPower";

	// Token: 0x04000D84 RID: 3460
	public static readonly HashedString CHECK_MAIL = "mail_pre";

	// Token: 0x04000D85 RID: 3461
	public static readonly HashedString CHECK_MAIL_SUCCESS = "mail_success_pst";

	// Token: 0x04000D86 RID: 3462
	public static readonly HashedString CHECK_MAIL_FAILURE = "mail_failure_pst";

	// Token: 0x04000D87 RID: 3463
	public static readonly HashedString CHECK_MAIL_DUPLICATE = "mail_duplicate_pst";

	// Token: 0x04000D88 RID: 3464
	public static readonly HashedString FOOD_SUCCESS = "food_like_loop";

	// Token: 0x04000D89 RID: 3465
	public static readonly HashedString FOOD_FAILURE = "food_dislike_loop";

	// Token: 0x04000D8A RID: 3466
	public static readonly HashedString FOOD_DUPLICATE = "food_duplicate_loop";

	// Token: 0x04000D8B RID: 3467
	public static readonly HashedString FOOD_IDLE = "idle_food_quest";

	// Token: 0x04000D8C RID: 3468
	public static readonly HashedString DECOR_IDLE = "idle_decor_quest";

	// Token: 0x04000D8D RID: 3469
	public static readonly HashedString POWER_IDLE = "idle_power_quest";

	// Token: 0x04000D8E RID: 3470
	public static readonly HashedString BLINDS_IDLE_0 = "idle_blinds_0";

	// Token: 0x04000D8F RID: 3471
	public static readonly HashedString PARCEL_SNAPTO = "parcel_snapTo";

	// Token: 0x04000D90 RID: 3472
	public const string PERSONALITY_ID = "JORGE";

	// Token: 0x04000D91 RID: 3473
	public const string BODY_ANIM_FILE = "body_lonelyminion_kanim";
}
