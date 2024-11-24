using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public class PowerControlStationConfig : IBuildingConfig
{
	// Token: 0x06001651 RID: 5713 RVA: 0x00197528 File Offset: 0x00195728
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PowerControlStation";
		int width = 2;
		int height = 4;
		string anim = "electricianworkdesk_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x000AFD76 File Offset: 0x000ADF76
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x001975A8 File Offset: 0x001957A8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 50f;
		storage.showInUI = true;
		storage.storageFilters = new List<Tag>
		{
			PowerControlStationConfig.MATERIAL_FOR_TINKER
		};
		TinkerStation tinkerstation = go.AddOrGet<TinkerStation>();
		tinkerstation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_electricianworkdesk_kanim")
		};
		tinkerstation.inputMaterial = PowerControlStationConfig.MATERIAL_FOR_TINKER;
		tinkerstation.massPerTinker = 5f;
		tinkerstation.outputPrefab = PowerControlStationConfig.TINKER_TOOLS;
		tinkerstation.outputTemperature = 308.15f;
		tinkerstation.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerstation.choreType = Db.Get().ChoreTypes.PowerFabricate.IdHash;
		tinkerstation.useFilteredStorage = true;
		tinkerstation.fetchChoreType = Db.Get().ChoreTypes.PowerFetch.IdHash;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			TinkerStation component = game_object.GetComponent<TinkerStation>();
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			tinkerstation.SetWorkTime(160f);
		};
	}

	// Token: 0x04000F0F RID: 3855
	public const string ID = "PowerControlStation";

	// Token: 0x04000F10 RID: 3856
	public static Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

	// Token: 0x04000F11 RID: 3857
	public static Tag TINKER_TOOLS = PowerStationToolsConfig.tag;

	// Token: 0x04000F12 RID: 3858
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000F13 RID: 3859
	public static string ROLE_PERK = "CanPowerTinker";

	// Token: 0x04000F14 RID: 3860
	public const float OUTPUT_TEMPERATURE = 308.15f;
}
