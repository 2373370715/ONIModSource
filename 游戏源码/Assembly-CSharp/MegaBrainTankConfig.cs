using STRINGS;
using TUNING;
using UnityEngine;

public class MegaBrainTankConfig : IBuildingConfig
{
	public const string ID = "MegaBrainTank";

	public const string INITIAL_LORE_UNLOCK_ID = "story_trait_mega_brain_tank_initial";

	public const string COMPLETED_LORE_UNLOCK_ID = "story_trait_mega_brain_tank_competed";

	public const string ACTIVE_EFFECT_ID = "MegaBrainTankBonus";

	public static object[,] STAT_BONUSES = new object[6, 3]
	{
		{
			Db.Get().Amounts.Stress.deltaAttribute.Id,
			-25f,
			Units.PerDay
		},
		{
			Db.Get().Attributes.Athletics.Id,
			5f,
			Units.Flat
		},
		{
			Db.Get().Attributes.Strength.Id,
			5f,
			Units.Flat
		},
		{
			Db.Get().Attributes.Learning.Id,
			5f,
			Units.Flat
		},
		{
			Db.Get().Attributes.SpaceNavigation.Id,
			5f,
			Units.Flat
		},
		{
			Db.Get().Attributes.Machinery.Id,
			5f,
			Units.Flat
		}
	};

	private const float KG_OXYGEN_CONSUMED_PER_SECOND = 0.5f;

	public const float MIN_OXYGEN_TO_WAKE_UP = 1f;

	private const float KG_OXYGEN_STORAGE_CAPACITY = 5f;

	public const short JOURNALS_TO_ACTIVATE = 25;

	public const float DIGESTION_RATE = 60f;

	public const float MAX_DIGESTION_TIME = 1500f;

	public const float REFILL_THESHOLD_ADJUSTMENT = 1f;

	public const short MAX_PHYSICAL_JOURNALS = 5;

	public const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	private const string ANIM_FILE = "gravitas_megabrain_kanim";

	public const string METER_ANIM = "meter";

	public const string METER_TARGET = "meter_oxygen_target";

	public static string[] METER_SYMBOLS = new string[3] { "meter_oxygen_target", "meter_oxygen_frame", "meter_oxygen_fill" };

	public const short TOTAL_BRAINS = 5;

	public const string BRAIN_HUM_EVENT = "MegaBrainTank_brain_wave_LP";

	public const float METER_INCREMENT_SPEED = 0.04f;

	public static HashedString ACTIVATE_ALL = new HashedString("brains_up");

	public static HashedString DEACTIVATE_ALL = new HashedString("brains_down");

	public static HashedString[] ACTIVATION_ANIMS = new HashedString[10]
	{
		new HashedString("brain1_pre"),
		new HashedString("brain2_pre"),
		new HashedString("brain3_pre"),
		new HashedString("brain4_pre"),
		new HashedString("brain5_pre"),
		new HashedString("brain1_loop"),
		new HashedString("brain2_loop"),
		new HashedString("brain3_loop"),
		new HashedString("brain4_loop"),
		new HashedString("idle")
	};

	public const short MAX_STORAGE_WORK_TIME = 2;

	private const string KACHUNK_ANIM = "kachunk";

	public static HashedString KACHUNK = new HashedString("kachunk");

	public static HashedString JOURNAL_SHELF = new HashedString("meter_journals_target");

	public static HashedString[] JOURNAL_SYMBOLS = new HashedString[5]
	{
		new HashedString("journal1"),
		new HashedString("journal2"),
		new HashedString("journal3"),
		new HashedString("journal4"),
		new HashedString("journal5")
	};

	public static StatusItem MaximumAptitude = new StatusItem("MaximumAptitude", DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Messages, allow_multiples: false, OverlayModes.None.ID);

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MegaBrainTank", 7, 7, "gravitas_megabrain_kanim", 100, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, MATERIALS.RAW_METALS, 2400f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER5, decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER4);
		obj.Floodable = true;
		obj.Entombable = false;
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		obj.ShowInBuildMenu = false;
		obj.InputConduitType = ConduitType.Gas;
		obj.UtilityInputOffset = new CellOffset(0, 0);
		obj.ExhaustKilowattsWhenActive = 0f;
		obj.SelfHeatKilowattsWhenActive = 0f;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.Color = LIGHT2D.HEADQUARTERS_COLOR;
		light2D.Range = 7f;
		light2D.Lux = 7200;
		light2D.overlayColour = LIGHT2D.HEADQUARTERS_OVERLAYCOLOR;
		light2D.shape = LightShape.Circle;
		light2D.drawOverlay = true;
		light2D.Offset = new Vector2(0f, 2f);
		go.GetComponent<BuildingHP>().invincible = true;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Demolishable>();
		go.AddOrGet<MegaBrainTank>();
		go.AddOrGet<Notifier>();
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		component.AddTag(GameTags.Gravitas);
		ConfigureJournalShelf(component);
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.SetWorkTime(5f);
		activatable.Required = false;
		activatable.synchronizeAnims = false;
		activatable.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_use_remote_kanim") };
		PrimaryElement component2 = go.GetComponent<PrimaryElement>();
		component2.SetElement(SimHashes.Steel);
		component2.Temperature = 294.15f;
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.storageWorkTime = 2f;
		storage.capacityKg = 30f;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.requestedItemTag = DreamJournalConfig.ID;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.refillMass = 25f;
		manualDeliveryKG.capacity = 25f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		manualDeliveryKG.ShowStatusItem = false;
		manualDeliveryKG.RoundFetchAmountToInt = true;
		manualDeliveryKG.FillToCapacity = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.capacityKG = 5f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Oxygen);
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.OperatingRequirement = Operational.State.Functional;
		RequireInputs requireInputs = go.AddOrGet<RequireInputs>();
		requireInputs.requireConduitHasMass = false;
		requireInputs.visualizeRequirements = RequireInputs.Requirements.NoWire;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
		{
			new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Oxygen).tag, 0.5f),
			new ElementConverter.ConsumedElement(DreamJournalConfig.ID, 1f / 60f)
		};
		elementConverter.OperationalRequirement = Operational.State.Operational;
		elementConverter.ShowInUI = false;
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	private void ConfigureJournalShelf(KPrefabID parentId)
	{
		KBatchedAnimController component = parentId.GetComponent<KBatchedAnimController>();
		GameObject gameObject = new GameObject("Journal Shelf");
		gameObject.transform.SetParent(parentId.transform);
		gameObject.transform.localPosition = Vector3.forward * -0.1f;
		gameObject.AddComponent<KPrefabID>().PrefabTag = parentId.PrefabTag;
		KBatchedAnimController kBatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kBatchedAnimController.AnimFiles = component.AnimFiles;
		kBatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kBatchedAnimController.initialAnim = "kachunk";
		kBatchedAnimController.initialMode = KAnim.PlayMode.Paused;
		kBatchedAnimController.isMovable = true;
		kBatchedAnimController.FlipX = component.FlipX;
		kBatchedAnimController.FlipY = component.FlipY;
		KBatchedAnimTracker kBatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kBatchedAnimTracker.SetAnimControllers(kBatchedAnimController, component);
		kBatchedAnimTracker.symbol = JOURNAL_SHELF;
		kBatchedAnimTracker.offset = Vector3.zero;
		component.SetSymbolVisiblity(JOURNAL_SHELF, is_visible: false);
		for (int i = 0; i < JOURNAL_SYMBOLS.Length; i++)
		{
			component.SetSymbolVisiblity(JOURNAL_SYMBOLS[i], is_visible: false);
			kBatchedAnimController.SetSymbolVisiblity(JOURNAL_SYMBOLS[i], is_visible: false);
		}
	}
}
