using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class FarmStationConfig : IBuildingConfig
{
	// Token: 0x06000366 RID: 870 RVA: 0x0014E26C File Offset: 0x0014C46C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FarmStation";
		int width = 2;
		int height = 3;
		string anim = "planttender_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x000A70A5 File Offset: 0x000A52A5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FarmStationType, false);
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0014E2EC File Offset: 0x0014C4EC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = FarmStationConfig.MATERIAL_FOR_TINKER;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.capacity = 50f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
		TinkerStation tinkerStation = go.AddOrGet<TinkerStation>();
		tinkerStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_planttender_kanim")
		};
		tinkerStation.inputMaterial = FarmStationConfig.MATERIAL_FOR_TINKER;
		tinkerStation.massPerTinker = 5f;
		tinkerStation.outputPrefab = FarmStationConfig.TINKER_TOOLS;
		tinkerStation.outputTemperature = 308.15f;
		tinkerStation.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		tinkerStation.choreType = Db.Get().ChoreTypes.FarmingFabricate.IdHash;
		tinkerStation.fetchChoreType = Db.Get().ChoreTypes.FarmFetch.IdHash;
		tinkerStation.EffectTitle = UI.BUILDINGEFFECTS.IMPROVED_PLANTS;
		tinkerStation.EffectTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_PLANTS;
		tinkerStation.EffectItemString = UI.BUILDINGEFFECTS.IMPROVED_PLANTS_ITEM;
		tinkerStation.EffectItemTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_PLANTS_ITEM;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			TinkerStation component = game_object.GetComponent<TinkerStation>();
			component.AttributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
			component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			component.SetWorkTime(15f);
		};
	}

	// Token: 0x0400020D RID: 525
	public const string ID = "FarmStation";

	// Token: 0x0400020E RID: 526
	public static Tag MATERIAL_FOR_TINKER = GameTags.Fertilizer;

	// Token: 0x0400020F RID: 527
	public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;

	// Token: 0x04000210 RID: 528
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000211 RID: 529
	public const float OUTPUT_TEMPERATURE = 308.15f;
}
