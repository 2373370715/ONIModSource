using System;
using TUNING;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public class MercuryCeilingLightConfig : IBuildingConfig
{
	// Token: 0x060011B7 RID: 4535 RVA: 0x000A6337 File Offset: 0x000A4537
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x00185514 File Offset: 0x00183714
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MercuryCeilingLight";
		int width = 3;
		int height = 1;
		string anim = "mercurylight_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.AddLogicPowerPort = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = CellOffset.none;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = CellOffset.none;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x000ADE30 File Offset: 0x000AC030
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 60000;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = global::LightShape.Quad;
		lightShapePreview.width = 3;
		lightShapePreview.direction = DiscreteShadowCaster.Direction.South;
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x001855B4 File Offset: 0x001837B4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Mercury).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 0.26000002f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		MercuryLight.Def def = go.AddOrGetDef<MercuryLight.Def>();
		def.FUEL_MASS_PER_SECOND = 0.13000001f;
		def.MAX_LUX = 60000f;
		def.TURN_ON_DELAY = 60f;
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x0018563C File Offset: 0x0018383C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.autoRespondToOperational = false;
		light2D.overlayColour = LIGHT2D.MERCURYCEILINGLIGHT_LUX_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.MERCURYCEILINGLIGHT_COLOR;
		light2D.Range = 8f;
		light2D.Angle = 2.6f;
		light2D.Direction = LIGHT2D.MERCURYCEILINGLIGHT_DIRECTIONVECTOR;
		light2D.Offset = LIGHT2D.MERCURYCEILINGLIGHT_OFFSET;
		light2D.shape = global::LightShape.Quad;
		light2D.drawOverlay = true;
		light2D.Lux = 60000;
		light2D.LightDirection = DiscreteShadowCaster.Direction.South;
		light2D.Width = 3;
		light2D.FalloffRate = 0.4f;
	}

	// Token: 0x04000C1C RID: 3100
	public const string ID = "MercuryCeilingLight";

	// Token: 0x04000C1D RID: 3101
	public const float MERCURY_CONSUMED_PER_SECOOND = 0.13000001f;

	// Token: 0x04000C1E RID: 3102
	public const float CHARGING_DELAY = 60f;
}
