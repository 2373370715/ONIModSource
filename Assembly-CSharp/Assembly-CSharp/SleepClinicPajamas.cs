﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class SleepClinicPajamas : IEquipmentConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public EquipmentDef CreateEquipmentDef()
	{
		ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.FANCY_CLOTHING;
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("SleepClinicPajamas", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, "pajamas_kanim", TUNING.EQUIPMENT.VESTS.SNAPON0, "body_pajamas_kanim", 4, attributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.SLEEPCLINICPAJAMAS.DESC + "\n\n" + STRINGS.EQUIPMENT.PREFABS.SLEEPCLINICPAJAMAS.EFFECT;
		Descriptor item = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)), Descriptor.DescriptorType.Effect, false);
		Descriptor item2 = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod), Descriptor.DescriptorType.Effect, false);
		equipmentDef.additionalDescriptors.Add(item);
		equipmentDef.additionalDescriptors.Add(item2);
		Effect.AddModifierDescriptions(null, equipmentDef.additionalDescriptors, "SleepClinic", false);
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo);
		};
		equipmentDef.OnUnequipCallBack = delegate(Equippable eq)
		{
			ClothingWearer.ClothingInfo.OnUnequipVest(eq);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, STRINGS.EQUIPMENT.PREFABS.SLEEPCLINICPAJAMAS.DESTROY_TOAST, eq.transform, 1.5f, false);
			eq.gameObject.DeleteObject();
		};
		return equipmentDef;
	}

	public void DoPostConfigure(GameObject go)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Clothes, false);
		component.AddTag(GameTags.PedestalDisplayable, false);
		go.AddOrGet<ClinicDreamable>().workTime = 300f;
		go.AddOrGet<Equippable>().SetQuality(global::QualityLevel.Poor);
		go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingFront;
	}

	public const string ID = "SleepClinicPajamas";

	public const string EFFECT_ID = "SleepClinic";
}
