using System;
using TUNING;
using UnityEngine;

// Token: 0x0200041F RID: 1055
public class MilkFeederConfig : IBuildingConfig
{
	// Token: 0x060011E5 RID: 4581 RVA: 0x00186844 File Offset: 0x00184A44
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MilkFeeder";
		int width = 3;
		int height = 3;
		string anim = "critter_milk_feeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x001868C4 File Offset: 0x00184AC4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LogicOperationalController>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 80f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Milk);
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x000ADF46 File Offset: 0x000AC146
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MilkFeeder.Def>();
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000C3E RID: 3134
	public const string ID = "MilkFeeder";

	// Token: 0x04000C3F RID: 3135
	public const string HAD_CONSUMED_MILK_RECENTLY_EFFECT_ID = "HadMilk";

	// Token: 0x04000C40 RID: 3136
	public const float EFFECT_DURATION_IN_SECONDS = 600f;

	// Token: 0x04000C41 RID: 3137
	public static readonly CellOffset DRINK_FROM_OFFSET = new CellOffset(1, 0);

	// Token: 0x04000C42 RID: 3138
	public static readonly Tag MILK_TAG = SimHashes.Milk.CreateTag();

	// Token: 0x04000C43 RID: 3139
	public const float UNITS_OF_MILK_CONSUMED_PER_FEEDING = 5f;
}
