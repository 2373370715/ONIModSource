using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B2B RID: 15147
	public class AttributeModifierSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600E923 RID: 59683 RVA: 0x0013BD72 File Offset: 0x00139F72
		public AttributeModifierSickness(AttributeModifier[] attribute_modifiers)
		{
			this.attributeModifiers = attribute_modifiers;
		}

		// Token: 0x0600E924 RID: 59684 RVA: 0x004C553C File Offset: 0x004C373C
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			Attributes attributes = go.GetAttributes();
			for (int i = 0; i < this.attributeModifiers.Length; i++)
			{
				AttributeModifier modifier = this.attributeModifiers[i];
				attributes.Add(modifier);
			}
			return null;
		}

		// Token: 0x0600E925 RID: 59685 RVA: 0x004C5574 File Offset: 0x004C3774
		public override void OnCure(GameObject go, object instance_data)
		{
			Attributes attributes = go.GetAttributes();
			for (int i = 0; i < this.attributeModifiers.Length; i++)
			{
				AttributeModifier modifier = this.attributeModifiers[i];
				attributes.Remove(modifier);
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x0600E926 RID: 59686 RVA: 0x0013BD81 File Offset: 0x00139F81
		public AttributeModifier[] Modifers
		{
			get
			{
				return this.attributeModifiers;
			}
		}

		// Token: 0x0600E927 RID: 59687 RVA: 0x004C55AC File Offset: 0x004C37AC
		public override List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute.Name, attributeModifier.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute.Name, attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
			}
			return list;
		}

		// Token: 0x0400E4BE RID: 58558
		private AttributeModifier[] attributeModifiers;
	}
}
