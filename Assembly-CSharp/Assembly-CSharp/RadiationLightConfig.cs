﻿using System;
using TUNING;
using UnityEngine;

public class RadiationLightConfig : IBuildingConfig
{
		public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

		public override BuildingDef CreateBuildingDef()
	{
		string id = "RadiationLight";
		int width = 1;
		int height = 1;
		string anim = "radiation_lamp_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnWall;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = CellOffset.none;
		return buildingDef;
	}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 50f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = this.FUEL_ELEMENT;
		manualDeliveryKG.capacity = 50f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitAngle = 90f;
		radiationEmitter.emitDirection = 0f;
		radiationEmitter.emissionOffset = Vector3.right;
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 16;
		radiationEmitter.emitRadiusY = 4;
		radiationEmitter.emitRads = 240f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(this.FUEL_ELEMENT, 0.016666668f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.008333334f, this.WASTE_ELEMENT, 0f, false, true, 0f, 0.5f, 0.5f, byte.MaxValue, 0, true)
		};
		ElementDropper elementDropper = go.AddOrGet<ElementDropper>();
		elementDropper.emitTag = this.WASTE_ELEMENT.CreateTag();
		elementDropper.emitMass = 5f;
		RadiationLight radiationLight = go.AddComponent<RadiationLight>();
		radiationLight.elementToConsume = this.FUEL_ELEMENT;
		radiationLight.consumptionRate = 0.016666668f;
	}

		public override void DoPostConfigureComplete(GameObject go)
	{
	}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

		public const string ID = "RadiationLight";

		private Tag FUEL_ELEMENT = SimHashes.UraniumOre.CreateTag();

		private SimHashes WASTE_ELEMENT = SimHashes.DepletedUranium;

		private const float FUEL_PER_CYCLE = 10f;

		private const float CYCLES_PER_REFILL = 5f;

		private const float FUEL_TO_WASTE_RATIO = 0.5f;

		private const float FUEL_STORAGE_AMOUNT = 50f;

		private const float FUEL_CONSUMPTION_RATE = 0.016666668f;

		private const short RAD_LIGHT_SIZE_X = 16;

		private const short RAD_LIGHT_SIZE_Y = 4;
}
