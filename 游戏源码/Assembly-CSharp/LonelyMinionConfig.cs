using STRINGS;
using UnityEngine;

public class LonelyMinionConfig : IEntityConfig
{
	public static string ID = "LonelyMinion";

	public const int VOICE_IDX = -2;

	public const int STARTING_SKILL_POINTS = 3;

	public const int BASE_ATTRIBUTE_LEVEL = 7;

	public const int AGE_MIN = 2190;

	public const int AGE_MAX = 3102;

	public const float MIN_IDLE_DELAY = 20f;

	public const float MAX_IDLE_DELAY = 40f;

	public const string IDLE_PREFIX = "idle_blinds";

	public static readonly HashedString GreetingCriteraId = "Neighbor";

	public static readonly HashedString FoodCriteriaId = "FoodQuality";

	public static readonly HashedString DecorCriteriaId = "Decor";

	public static readonly HashedString PowerCriteriaId = "SuppliedPower";

	public static readonly HashedString CHECK_MAIL = "mail_pre";

	public static readonly HashedString CHECK_MAIL_SUCCESS = "mail_success_pst";

	public static readonly HashedString CHECK_MAIL_FAILURE = "mail_failure_pst";

	public static readonly HashedString CHECK_MAIL_DUPLICATE = "mail_duplicate_pst";

	public static readonly HashedString FOOD_SUCCESS = "food_like_loop";

	public static readonly HashedString FOOD_FAILURE = "food_dislike_loop";

	public static readonly HashedString FOOD_DUPLICATE = "food_duplicate_loop";

	public static readonly HashedString FOOD_IDLE = "idle_food_quest";

	public static readonly HashedString DECOR_IDLE = "idle_decor_quest";

	public static readonly HashedString POWER_IDLE = "idle_power_quest";

	public static readonly HashedString BLINDS_IDLE_0 = "idle_blinds_0";

	public static readonly HashedString PARCEL_SNAPTO = "parcel_snapTo";

	public const string PERSONALITY_ID = "JORGE";

	public const string BODY_ANIM_FILE = "body_lonelyminion_kanim";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		string name = DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME;
		GameObject gameObject = EntityTemplates.CreateEntity(ID, name);
		gameObject.AddComponent<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		gameObject.AddComponent<Storage>().doDiseaseTransfer = false;
		gameObject.AddComponent<StateMachineController>();
		LonelyMinion.Def def = gameObject.AddOrGetDef<LonelyMinion.Def>();
		def.Personality = Db.Get().Personalities.Get("JORGE");
		def.Personality.Disabled = true;
		KBatchedAnimController kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kBatchedAnimController.defaultAnim = "idle_default";
		kBatchedAnimController.initialAnim = "idle_default";
		kBatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kBatchedAnimController.AnimFiles = new KAnimFile[3]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_interacts_lonely_dupe_kanim")
		};
		ConfigurePackageOverride(gameObject);
		SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		symbolOverrideController.applySymbolOverridesEveryFrame = true;
		symbolOverrideController.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol($"cheek_00{def.Personality.headShape}"), 1);
		MinionConfig.ConfigureSymbols(gameObject);
		return gameObject;
	}

	public void OnPrefabInit(GameObject go)
	{
	}

	public void OnSpawn(GameObject go)
	{
	}

	private void ConfigurePackageOverride(GameObject go)
	{
		GameObject gameObject = new GameObject("PackageSnapPoint");
		gameObject.transform.SetParent(go.transform);
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kBatchedAnimController.transform.position = Vector3.forward * -0.1f;
		kBatchedAnimController.AnimFiles = new KAnimFile[1] { Assets.GetAnim("mushbar_kanim") };
		kBatchedAnimController.initialAnim = "object";
		component.SetSymbolVisiblity(PARCEL_SNAPTO, is_visible: false);
		KBatchedAnimTracker kBatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kBatchedAnimTracker.controller = component;
		kBatchedAnimTracker.symbol = PARCEL_SNAPTO;
	}
}
