﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020014D2 RID: 5330
[AddComponentMenu("KMonoBehaviour/game/MedicinalPill")]
public class MedicinalPill : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06006F0D RID: 28429 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006F0E RID: 28430 RVA: 0x002F0E80 File Offset: 0x002EF080
	public List<Descriptor> EffectDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (string.IsNullOrEmpty(this.info.doctorStationId))
		{
			if (this.info.medicineType == MedicineInfo.MedicineType.Booster)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_BOOSTER, Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_BOOSTER_TOOLTIP, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_CURE, Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_CURE_TOOLTIP, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
		}
		else
		{
			string properName = Assets.GetPrefab(this.info.doctorStationId).GetProperName();
			if (this.info.medicineType == MedicineInfo.MedicineType.Booster)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_BOOSTER.Replace("{Station}", properName), Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_BOOSTER_TOOLTIP.Replace("{Station}", properName), Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_CURE.Replace("{Station}", properName), Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_CURE_TOOLTIP.Replace("{Station}", properName), Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
		}
		switch (this.info.medicineType)
		{
		case MedicineInfo.MedicineType.CureAny:
			list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES_ANY, Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES_ANY_TOOLTIP, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			break;
		case MedicineInfo.MedicineType.CureSpecific:
		{
			List<string> list2 = new List<string>();
			foreach (string text in this.info.curedSicknesses)
			{
				list2.Add(Strings.Get("STRINGS.DUPLICANTS.DISEASES." + text.ToUpper() + ".NAME"));
			}
			string arg = string.Join(",", list2.ToArray());
			list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES, arg), string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES_TOOLTIP, arg), Descriptor.DescriptorType.Effect, false));
			break;
		}
		}
		if (!string.IsNullOrEmpty(this.info.effect))
		{
			Effect effect = Db.Get().effects.Get(this.info.effect);
			list.Add(new Descriptor(string.Format(DUPLICANTS.MODIFIERS.MEDICINE_GENERICPILL.EFFECT_DESC, effect.Name), string.Format("{0}\n{1}", effect.description, Effect.CreateTooltip(effect, true, "\n    • ", true)), Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06006F0F RID: 28431 RVA: 0x000E8BFA File Offset: 0x000E6DFA
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.EffectDescriptors(go);
	}

	// Token: 0x040052F5 RID: 21237
	public MedicineInfo info;
}
