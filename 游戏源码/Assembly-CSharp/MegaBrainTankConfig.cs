using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class MegaBrainTankConfig : IBuildingConfig
{
	// Token: 0x060011B1 RID: 4529 RVA: 0x00184DBC File Offset: 0x00182FBC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MegaBrainTank";
		int width = 7;
		int height = 7;
		string anim = "gravitas_megabrain_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER4, tier2, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ShowInBuildMenu = false;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		return buildingDef;
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x00184E54 File Offset: 0x00183054
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.Color = LIGHT2D.HEADQUARTERS_COLOR;
		light2D.Range = 7f;
		light2D.Lux = 7200;
		light2D.overlayColour = LIGHT2D.HEADQUARTERS_OVERLAYCOLOR;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		light2D.Offset = new Vector2(0f, 2f);
		go.GetComponent<BuildingHP>().invincible = true;
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x00184ECC File Offset: 0x001830CC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Demolishable>();
		go.AddOrGet<MegaBrainTank>();
		go.AddOrGet<Notifier>();
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		component.AddTag(GameTags.Gravitas, false);
		this.ConfigureJournalShelf(component);
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.SetWorkTime(5f);
		activatable.Required = false;
		activatable.synchronizeAnims = false;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		PrimaryElement component2 = go.GetComponent<PrimaryElement>();
		component2.SetElement(SimHashes.Steel, true);
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
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Oxygen).tag, 0.5f, true),
			new ElementConverter.ConsumedElement(DreamJournalConfig.ID, 0.016666668f, true)
		};
		elementConverter.OperationalRequirement = Operational.State.Operational;
		elementConverter.ShowInUI = false;
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x001850C4 File Offset: 0x001832C4
	private void ConfigureJournalShelf(KPrefabID parentId)
	{
		KBatchedAnimController component = parentId.GetComponent<KBatchedAnimController>();
		GameObject gameObject = new GameObject("Journal Shelf");
		gameObject.transform.SetParent(parentId.transform);
		gameObject.transform.localPosition = Vector3.forward * -0.1f;
		gameObject.AddComponent<KPrefabID>().PrefabTag = parentId.PrefabTag;
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = component.AnimFiles;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kbatchedAnimController.initialAnim = "kachunk";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.FlipX = component.FlipX;
		kbatchedAnimController.FlipY = component.FlipY;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.SetAnimControllers(kbatchedAnimController, component);
		kbatchedAnimTracker.symbol = MegaBrainTankConfig.JOURNAL_SHELF;
		kbatchedAnimTracker.offset = Vector3.zero;
		component.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SHELF, false);
		for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
		{
			component.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], false);
			kbatchedAnimController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], false);
		}
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x001851E0 File Offset: 0x001833E0
	// Note: this type is marked as 'beforefieldinit'.
	static MegaBrainTankConfig()
	{
		object[,] array = new object[6, 3];
		array[0, 0] = Db.Get().Amounts.Stress.deltaAttribute.Id;
		array[0, 1] = -25f;
		array[0, 2] = Units.PerDay;
		array[1, 0] = Db.Get().Attributes.Athletics.Id;
		array[1, 1] = 5f;
		array[1, 2] = Units.Flat;
		array[2, 0] = Db.Get().Attributes.Strength.Id;
		array[2, 1] = 5f;
		array[2, 2] = Units.Flat;
		array[3, 0] = Db.Get().Attributes.Learning.Id;
		array[3, 1] = 5f;
		array[3, 2] = Units.Flat;
		array[4, 0] = Db.Get().Attributes.SpaceNavigation.Id;
		array[4, 1] = 5f;
		array[4, 2] = Units.Flat;
		array[5, 0] = Db.Get().Attributes.Machinery.Id;
		array[5, 1] = 5f;
		array[5, 2] = Units.Flat;
		MegaBrainTankConfig.STAT_BONUSES = array;
		MegaBrainTankConfig.METER_SYMBOLS = new string[]
		{
			"meter_oxygen_target",
			"meter_oxygen_frame",
			"meter_oxygen_fill"
		};
		MegaBrainTankConfig.ACTIVATE_ALL = new HashedString("brains_up");
		MegaBrainTankConfig.DEACTIVATE_ALL = new HashedString("brains_down");
		MegaBrainTankConfig.ACTIVATION_ANIMS = new HashedString[]
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
		MegaBrainTankConfig.KACHUNK = new HashedString("kachunk");
		MegaBrainTankConfig.JOURNAL_SHELF = new HashedString("meter_journals_target");
		MegaBrainTankConfig.JOURNAL_SYMBOLS = new HashedString[]
		{
			new HashedString("journal1"),
			new HashedString("journal2"),
			new HashedString("journal3"),
			new HashedString("journal4"),
			new HashedString("journal5")
		};
		MegaBrainTankConfig.MaximumAptitude = new StatusItem("MaximumAptitude", DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Messages, false, OverlayModes.None.ID, 129022, true, null);
	}

	// Token: 0x04000BFE RID: 3070
	public const string ID = "MegaBrainTank";

	// Token: 0x04000BFF RID: 3071
	public const string INITIAL_LORE_UNLOCK_ID = "story_trait_mega_brain_tank_initial";

	// Token: 0x04000C00 RID: 3072
	public const string COMPLETED_LORE_UNLOCK_ID = "story_trait_mega_brain_tank_competed";

	// Token: 0x04000C01 RID: 3073
	public const string ACTIVE_EFFECT_ID = "MegaBrainTankBonus";

	// Token: 0x04000C02 RID: 3074
	public static object[,] STAT_BONUSES;

	// Token: 0x04000C03 RID: 3075
	private const float KG_OXYGEN_CONSUMED_PER_SECOND = 0.5f;

	// Token: 0x04000C04 RID: 3076
	public const float MIN_OXYGEN_TO_WAKE_UP = 1f;

	// Token: 0x04000C05 RID: 3077
	private const float KG_OXYGEN_STORAGE_CAPACITY = 5f;

	// Token: 0x04000C06 RID: 3078
	public const short JOURNALS_TO_ACTIVATE = 25;

	// Token: 0x04000C07 RID: 3079
	public const float DIGESTION_RATE = 60f;

	// Token: 0x04000C08 RID: 3080
	public const float MAX_DIGESTION_TIME = 1500f;

	// Token: 0x04000C09 RID: 3081
	public const float REFILL_THESHOLD_ADJUSTMENT = 1f;

	// Token: 0x04000C0A RID: 3082
	public const short MAX_PHYSICAL_JOURNALS = 5;

	// Token: 0x04000C0B RID: 3083
	public const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000C0C RID: 3084
	private const string ANIM_FILE = "gravitas_megabrain_kanim";

	// Token: 0x04000C0D RID: 3085
	public const string METER_ANIM = "meter";

	// Token: 0x04000C0E RID: 3086
	public const string METER_TARGET = "meter_oxygen_target";

	// Token: 0x04000C0F RID: 3087
	public static string[] METER_SYMBOLS;

	// Token: 0x04000C10 RID: 3088
	public const short TOTAL_BRAINS = 5;

	// Token: 0x04000C11 RID: 3089
	public const string BRAIN_HUM_EVENT = "MegaBrainTank_brain_wave_LP";

	// Token: 0x04000C12 RID: 3090
	public const float METER_INCREMENT_SPEED = 0.04f;

	// Token: 0x04000C13 RID: 3091
	public static HashedString ACTIVATE_ALL;

	// Token: 0x04000C14 RID: 3092
	public static HashedString DEACTIVATE_ALL;

	// Token: 0x04000C15 RID: 3093
	public static HashedString[] ACTIVATION_ANIMS;

	// Token: 0x04000C16 RID: 3094
	public const short MAX_STORAGE_WORK_TIME = 2;

	// Token: 0x04000C17 RID: 3095
	private const string KACHUNK_ANIM = "kachunk";

	// Token: 0x04000C18 RID: 3096
	public static HashedString KACHUNK;

	// Token: 0x04000C19 RID: 3097
	public static HashedString JOURNAL_SHELF;

	// Token: 0x04000C1A RID: 3098
	public static HashedString[] JOURNAL_SYMBOLS;

	// Token: 0x04000C1B RID: 3099
	public static StatusItem MaximumAptitude;
}
