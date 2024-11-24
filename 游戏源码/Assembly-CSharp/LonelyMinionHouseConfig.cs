using STRINGS;
using TUNING;
using UnityEngine;

public class LonelyMinionHouseConfig : IBuildingConfig
{
	public const string ID = "LonelyMinionHouse";

	public const string LORE_UNLOCK_PREFIX = "story_trait_lonelyminion_";

	public const int FriendshipQuestCount = 3;

	public const string METER_TARGET = "meter_storage_target";

	public const string METER_ANIM = "meter";

	public static readonly string[] METER_SYMBOLS = new string[2] { "meter_storage", "meter_level" };

	public const string BLINDS_TARGET = "blinds_target";

	public const string BLINDS_PREFIX = "meter_blinds";

	public static readonly string[] BLINDS_SYMBOLS = new string[4] { "blinds_target", "blind", "blind_string", "blinds" };

	private const string LIGHTS_TARGET = "lights_target";

	private static readonly string[] LIGHTS_SYMBOLS = new string[5] { "lights_target", "festive_lights", "lights_wire", "light_bulb", "snapTo_light_locator" };

	public static readonly HashedString ANSWER = "answer";

	public static readonly HashedString LIGHTS_OFF = "meter_lights_off";

	public static readonly HashedString LIGHTS_ON = "meter_lights_on_loop";

	public static readonly HashedString STORAGE = "storage_off";

	public static readonly HashedString STORAGE_WORK_PST = "working_pst";

	public static readonly HashedString[] STORAGE_WORKING = new HashedString[2] { "working_pre", "working_loop" };

	public static readonly EffectorValues HOUSE_DECOR = new EffectorValues
	{
		amount = -25,
		radius = 6
	};

	public static readonly EffectorValues STORAGE_DECOR = DECOR.PENALTY.TIER1;

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("LonelyMinionHouse", 4, 6, "lonely_dupe_home_kanim", 1000, 480f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, new string[1] { SimHashes.Steel.ToString() }, 9999f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: HOUSE_DECOR);
		obj.DefaultAnimState = "on";
		obj.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		obj.EnergyConsumptionWhenActive = 60f;
		obj.AddLogicPowerPort = false;
		obj.RequiresPowerInput = true;
		obj.PowerInputOffset = new CellOffset(2, 1);
		obj.ShowInBuildMenu = false;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "large";
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<NonEssentialEnergyConsumer>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Prioritizable.AddRef(go);
		go.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.high, 5));
		Storage storage = go.AddOrGet<Storage>();
		KnockKnock knockKnock = go.AddOrGet<KnockKnock>();
		LonelyMinionHouse.Def def = go.AddOrGetDef<LonelyMinionHouse.Def>();
		storage.allowItemRemoval = false;
		storage.capacityKg = 250000f;
		storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
		storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		knockKnock.triggerWorkReactions = false;
		knockKnock.synchronizeAnims = false;
		knockKnock.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_doorknock_kanim") };
		knockKnock.workAnims = new HashedString[2] { "knocking_pre", "knocking_loop" };
		knockKnock.workingPstComplete = new HashedString[1] { "knocking_pst" };
		knockKnock.workingPstFailed = null;
		knockKnock.SetButtonTextOverride(new ButtonMenuTextOverride
		{
			Text = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TEXT,
			CancelText = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.CANCELTEXT,
			ToolTip = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TOOLTIP,
			CancelToolTip = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.CANCEL_TOOLTIP
		});
		def.Story = Db.Get().Stories.LonelyMinion;
		def.CompletionData = new StoryCompleteData
		{
			KeepSakeSpawnOffset = default(CellOffset),
			CameraTargetOffset = new CellOffset(0, 3)
		};
		def.InitalLoreId = "story_trait_lonelyminion_initial";
		def.EventIntroInfo = new StoryManager.PopupInfo
		{
			Title = CODEX.STORY_TRAITS.LONELYMINION.BEGIN_POPUP.NAME,
			Description = CODEX.STORY_TRAITS.LONELYMINION.BEGIN_POPUP.DESCRIPTION,
			CloseButtonText = CODEX.STORY_TRAITS.CLOSE_BUTTON,
			TextureName = "minionhouseactivate_kanim",
			DisplayImmediate = true,
			PopupType = EventInfoDataHelper.PopupType.BEGIN
		};
		def.CompleteLoreId = "story_trait_lonelyminion_complete";
		def.EventCompleteInfo = new StoryManager.PopupInfo
		{
			Title = CODEX.STORY_TRAITS.LONELYMINION.END_POPUP.NAME,
			Description = CODEX.STORY_TRAITS.LONELYMINION.END_POPUP.DESCRIPTION,
			CloseButtonText = CODEX.STORY_TRAITS.LONELYMINION.END_POPUP.BUTTON,
			TextureName = "minionhousecomplete_kanim",
			PopupType = EventInfoDataHelper.PopupType.COMPLETE
		};
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		Object.Destroy(go.GetComponent<BuildingEnabledButton>());
		go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
		ConfigureLights(go);
	}

	private void ConfigureLights(GameObject go)
	{
		GameObject gameObject = new GameObject("FestiveLights");
		gameObject.SetActive(value: false);
		gameObject.transform.SetParent(go.transform);
		gameObject.AddOrGet<Light2D>();
		KBatchedAnimController kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		kBatchedAnimController.AnimFiles = component.AnimFiles;
		kBatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kBatchedAnimController.initialAnim = "meter_lights_off";
		kBatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kBatchedAnimController.isMovable = true;
		kBatchedAnimController.FlipX = component.FlipX;
		kBatchedAnimController.FlipY = component.FlipY;
		KBatchedAnimTracker kBatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kBatchedAnimTracker.SetAnimControllers(kBatchedAnimController, component);
		kBatchedAnimTracker.symbol = "lights_target";
		kBatchedAnimTracker.offset = Vector3.zero;
		for (int i = 0; i < LIGHTS_SYMBOLS.Length; i++)
		{
			component.SetSymbolVisiblity(LIGHTS_SYMBOLS[i], is_visible: false);
		}
	}
}
