using FoodRehydrator;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class FoodRehydratorConfig : IBuildingConfig
{
	public const string ID = "FoodRehydrator";

	public static Tag REHYDRATION_TAG = GameTags.Water;

	public const float REHYDRATION_COST = 1f;

	public const float REHYDRATOR_PACKAGES_CAPACITY = 5f;

	public const float REHYDRATION_WORK_TIME = 5f;

	public static Effect RehydrationEffect = ConstructRehydrationEffect();

	public const string REHYDRATION_DEBUFF_ID = "RehydratedFoodConsumed";

	public const string REHDYRATION_DEBUFF_NAME = "RehydratedFoodConsumed";

	public const float REHYDRATION_DEBUFF_DURATION = 600f;

	public const float REHYDRATION_DEBUFF_EFFECT = -1f;

	private static Effect ConstructRehydrationEffect()
	{
		Effect effect = new Effect("RehydratedFoodConsumed", "RehydratedFoodConsumed", ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED, 600f, show_in_ui: false, trigger_floating_text: false, is_bad: true);
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, -1f, ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED));
		return effect;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("FoodRehydrator", 1, 2, "Rehydrator_kanim", 10, 120f, new float[2]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		}, new string[2] { "RefinedMetal", "Plastic" }, 800f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0);
		obj.Overheatable = true;
		obj.Floodable = false;
		obj.AudioCategory = "Plastic";
		obj.AudioSize = "small";
		obj.RequiresPowerInput = true;
		obj.InputConduitType = ConduitType.Liquid;
		obj.UtilityInputOffset = new CellOffset(0, 0);
		obj.EnergyConsumptionWhenActive = 60f;
		obj.SelfHeatKilowattsWhenActive = 0.5f;
		obj.ViewMode = OverlayModes.Power.ID;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = 5f;
		storage.showInUI = true;
		storage.showDescriptor = false;
		storage.allowItemRemoval = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.capacity = 5f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.StorageFetch.Id;
		manualDeliveryKG.requestedItemTag = GameTags.Dehydrated;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showCapacityStatusItem = true;
		storage2.allowItemRemoval = false;
		storage2.showCapacityStatusItem = true;
		storage2.capacityKg = 20f;
		ConduitConsumer conduitConsumer = go.AddComponent<ConduitConsumer>();
		conduitConsumer.capacityTag = REHYDRATION_TAG;
		conduitConsumer.capacityKG = storage2.capacityKg;
		conduitConsumer.storage = storage2;
		conduitConsumer.alwaysConsume = true;
		Prioritizable.AddRef(go);
		go.AddOrGet<AccessabilityManager>();
		go.AddOrGet<DehydratedManager>();
		go.AddOrGet<ResourceRequirementMonitor>();
		go.AddOrGetDef<FoodRehydratorSM.Def>();
		go.AddOrGet<UserNameable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
