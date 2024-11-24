using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class WaterCoolerConfig : IBuildingConfig
{
	public const string ID = "WaterCooler";

	public static Tuple<Tag, string>[] BEVERAGE_CHOICE_OPTIONS = new Tuple<Tag, string>[2]
	{
		new Tuple<Tag, string>(SimHashes.Water.CreateTag(), ""),
		new Tuple<Tag, string>(SimHashes.Milk.CreateTag(), "DuplicantGotMilk")
	};

	public const string MilkEffectID = "DuplicantGotMilk";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("WaterCooler", 2, 2, "watercooler_kanim", 30, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.RAW_MINERALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.BONUS.TIER1);
		obj.Floodable = false;
		obj.AudioCategory = "Metal";
		obj.Overheatable = false;
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 9f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		HeatImmunityProvider.Def def = go.AddOrGetDef<HeatImmunityProvider.Def>();
		def.range = new CellOffset[2][]
		{
			new CellOffset[1]
			{
				new CellOffset(1, 0)
			},
			new CellOffset[1]
			{
				new CellOffset(0, 0)
			}
		};
		def.overrideFileName = "anim_interacts_watercooler_kanim";
		def.overrideAnims = new string[3] { "working_pre", "working_loop", "working_pst" };
		def.specialRequirements = RefreshFromHeatCondition;
		def.onEffectApplied = (Action<GameObject, HeatImmunityProvider.Instance>)Delegate.Combine(def.onEffectApplied, new Action<GameObject, HeatImmunityProvider.Instance>(OnHeatImmunityEffectApplied));
		go.AddOrGet<WaterCooler>();
		WaterCooler.OnDuplicantDrank = (Action<GameObject, GameObject>)Delegate.Combine(WaterCooler.OnDuplicantDrank, new Action<GameObject, GameObject>(ApplyImmunityEffectWhenDrankRecreationally));
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	private void ApplyImmunityEffectWhenDrankRecreationally(GameObject duplicant, GameObject waterCoolerInstance)
	{
		waterCoolerInstance.GetSMI<HeatImmunityProvider.Instance>()?.ApplyImmunityEffect(duplicant, triggerEvents: false);
	}

	private void OnHeatImmunityEffectApplied(GameObject duplicant, HeatImmunityProvider.Instance smi)
	{
		smi.GetSMI<WaterCooler.StatesInstance>().Drink(duplicant, triggerOnDrinkCallback: false);
	}

	private bool RefreshFromHeatCondition(GameObject go_instance)
	{
		WaterCooler.StatesInstance sMI = go_instance.GetSMI<WaterCooler.StatesInstance>();
		return sMI?.IsInsideState(sMI.sm.dispensing) ?? false;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
