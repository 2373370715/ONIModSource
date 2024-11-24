﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class AlgaeHabitatConfig : IBuildingConfig
{
	// Token: 0x0600007E RID: 126 RVA: 0x0013FEA8 File Offset: 0x0013E0A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AlgaeHabitat";
		int width = 1;
		int height = 2;
		string anim = "algaefarm_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] farmable = MATERIALS.FARMABLE;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_bubbles", NOISE_POLLUTION.NOISY.TIER0);
		SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_in", NOISE_POLLUTION.NOISY.TIER0);
		SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_out", NOISE_POLLUTION.NOISY.TIER0);
		return buildingDef;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x0013FF70 File Offset: 0x0013E170
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		List<Tag> storageFilters = new List<Tag>
		{
			SimHashes.DirtyWater.CreateTag()
		};
		Tag tag = SimHashes.Algae.CreateTag();
		Tag tag2 = SimHashes.Water.CreateTag();
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 360f;
		storage2.showInUI = true;
		storage2.SetDefaultStoredItemModifiers(AlgaeHabitatConfig.PollutedWaterStorageModifiers);
		storage2.allowItemRemoval = false;
		storage2.storageFilters = storageFilters;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = tag;
		manualDeliveryKG.capacity = 90f;
		manualDeliveryKG.refillMass = 18f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG2.SetStorage(storage);
		manualDeliveryKG2.RequestedItemTag = tag2;
		manualDeliveryKG2.capacity = 360f;
		manualDeliveryKG2.refillMass = 72f;
		manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_algae_terarrium_kanim")
		};
		AlgaeHabitatEmpty algaeHabitatEmpty = go.AddOrGet<AlgaeHabitatEmpty>();
		algaeHabitatEmpty.workTime = 5f;
		algaeHabitatEmpty.overrideAnims = overrideAnims;
		algaeHabitatEmpty.workLayer = Grid.SceneLayer.BuildingFront;
		AlgaeHabitat algaeHabitat = go.AddOrGet<AlgaeHabitat>();
		algaeHabitat.lightBonusMultiplier = 1.1f;
		algaeHabitat.pressureSampleOffset = new CellOffset(0, 1);
		ElementConverter elementConverter = go.AddComponent<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(tag, 0.030000001f, true),
			new ElementConverter.ConsumedElement(tag2, 0.3f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.040000003f, SimHashes.Oxygen, 303.15f, false, false, 0f, 1f, 1f, byte.MaxValue, 0, true)
		};
		go.AddComponent<ElementConverter>().outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.29033336f, SimHashes.DirtyWater, 303.15f, false, true, 0f, 1f, 1f, byte.MaxValue, 0, true)
		};
		ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
		elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
		elementConsumer.consumptionRate = 0.0003333333f;
		elementConsumer.consumptionRadius = 3;
		elementConsumer.showInStatusPanel = true;
		elementConsumer.sampleCellOffset = new Vector3(0f, 1f, 0f);
		elementConsumer.isRequired = false;
		PassiveElementConsumer passiveElementConsumer = go.AddComponent<PassiveElementConsumer>();
		passiveElementConsumer.elementToConsume = SimHashes.Water;
		passiveElementConsumer.consumptionRate = 1.2f;
		passiveElementConsumer.consumptionRadius = 1;
		passiveElementConsumer.showDescriptor = false;
		passiveElementConsumer.storeOnConsume = true;
		passiveElementConsumer.capacityKG = 360f;
		passiveElementConsumer.showInStatusPanel = false;
		go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		go.AddOrGet<AnimTileable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400005F RID: 95
	public const string ID = "AlgaeHabitat";

	// Token: 0x04000060 RID: 96
	private const float ALGAE_RATE = 0.030000001f;

	// Token: 0x04000061 RID: 97
	private const float WATER_RATE = 0.3f;

	// Token: 0x04000062 RID: 98
	private const float OXYGEN_RATE = 0.040000003f;

	// Token: 0x04000063 RID: 99
	private const float CO2_RATE = 0.0003333333f;

	// Token: 0x04000064 RID: 100
	private const float ALGAE_CAPACITY = 90f;

	// Token: 0x04000065 RID: 101
	private const float WATER_CAPACITY = 360f;

	// Token: 0x04000066 RID: 102
	private static readonly List<Storage.StoredItemModifier> PollutedWaterStorageModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};
}
