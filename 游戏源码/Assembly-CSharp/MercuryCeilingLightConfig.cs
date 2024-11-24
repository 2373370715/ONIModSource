using TUNING;
using UnityEngine;

public class MercuryCeilingLightConfig : IBuildingConfig
{
	public const string ID = "MercuryCeilingLight";

	public const float MERCURY_CONSUMED_PER_SECOOND = 0.13000001f;

	public const float CHARGING_DELAY = 60f;

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MercuryCeilingLight", 3, 1, "mercurylight_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnCeiling, noise: NOISE_POLLUTION.NONE, decor: BUILDINGS.DECOR.NONE);
		obj.AddLogicPowerPort = true;
		obj.RequiresPowerInput = true;
		obj.PowerInputOffset = CellOffset.none;
		obj.InputConduitType = ConduitType.Liquid;
		obj.UtilityInputOffset = CellOffset.none;
		obj.EnergyConsumptionWhenActive = 60f;
		obj.SelfHeatKilowattsWhenActive = 1f;
		obj.ViewMode = OverlayModes.Light.ID;
		obj.AudioCategory = "Metal";
		return obj;
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 60000;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = LightShape.Quad;
		lightShapePreview.width = 3;
		lightShapePreview.direction = DiscreteShadowCaster.Direction.South;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource);
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
		light2D.shape = LightShape.Quad;
		light2D.drawOverlay = true;
		light2D.Lux = 60000;
		light2D.LightDirection = DiscreteShadowCaster.Direction.South;
		light2D.Width = 3;
		light2D.FalloffRate = 0.4f;
	}
}
