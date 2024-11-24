using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class PioneerModuleConfig : IBuildingConfig
{
	// Token: 0x06001622 RID: 5666 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x00196A58 File Offset: 0x00194C58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PioneerModule";
		int width = 3;
		int height = 3;
		string anim = "rocket_pioneer_cargo_module_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "deployed";
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x00196AFC File Offset: 0x00194CFC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		BuildingInternalConstructor.Def def = go.AddOrGetDef<BuildingInternalConstructor.Def>();
		def.constructionMass = 400f;
		def.outputIDs = new List<string>
		{
			"PioneerLander"
		};
		def.spawnIntoStorage = true;
		def.storage = storage;
		def.constructionSymbol = "under_construction";
		go.AddOrGet<BuildingInternalConstructorWorkable>().SetWorkTime(30f);
		JettisonableCargoModule.Def def2 = go.AddOrGetDef<JettisonableCargoModule.Def>();
		def2.landerPrefabID = "PioneerLander".ToTag();
		def2.landerContainer = storage;
		def2.clusterMapFXPrefabID = "DeployingPioneerLanderFX";
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
		};
		go.AddOrGet<NavTeleporter>();
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x001946D0 File Offset: 0x001928D0
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE, 0f, 0f);
		FakeFloorAdder fakeFloorAdder = go.AddOrGet<FakeFloorAdder>();
		fakeFloorAdder.floorOffsets = new CellOffset[]
		{
			new CellOffset(-1, -1),
			new CellOffset(0, -1),
			new CellOffset(1, -1)
		};
		fakeFloorAdder.initiallyActive = false;
	}

	// Token: 0x04000EF7 RID: 3831
	public const string ID = "PioneerModule";
}
