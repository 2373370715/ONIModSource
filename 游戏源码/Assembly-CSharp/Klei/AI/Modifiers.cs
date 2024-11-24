using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B80 RID: 15232
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Modifiers")]
	public class Modifiers : KMonoBehaviour, ISaveLoadableDetails
	{
		// Token: 0x0600EA8E RID: 60046 RVA: 0x004CA400 File Offset: 0x004C8600
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.amounts = new Amounts(base.gameObject);
			this.sicknesses = new Sicknesses(base.gameObject);
			this.attributes = new Attributes(base.gameObject);
			foreach (string id in this.initialAmounts)
			{
				this.amounts.Add(new AmountInstance(Db.Get().Amounts.Get(id), base.gameObject));
			}
			foreach (string text in this.initialAttributes)
			{
				Attribute attribute = Db.Get().CritterAttributes.TryGet(text);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.TryGet(text);
				}
				if (attribute == null)
				{
					attribute = Db.Get().Attributes.TryGet(text);
				}
				DebugUtil.Assert(attribute != null, "Couldn't find an attribute for id", text);
				this.attributes.Add(attribute);
			}
			Traits component = base.GetComponent<Traits>();
			if (this.initialTraits != null)
			{
				foreach (string id2 in this.initialTraits)
				{
					Trait trait = Db.Get().traits.Get(id2);
					component.Add(trait);
				}
			}
		}

		// Token: 0x0600EA8F RID: 60047 RVA: 0x0013CCDB File Offset: 0x0013AEDB
		public float GetPreModifiedAttributeValue(Attribute attribute)
		{
			return AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
		}

		// Token: 0x0600EA90 RID: 60048 RVA: 0x004CA5AC File Offset: 0x004C87AC
		public string GetPreModifiedAttributeFormattedValue(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return attribute.formatter.GetFormattedValue(totalValue, attribute.formatter.DeltaTimeSlice);
		}

		// Token: 0x0600EA91 RID: 60049 RVA: 0x004CA5E0 File Offset: 0x004C87E0
		public string GetPreModifiedAttributeDescription(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, attribute.Name, attribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None));
		}

		// Token: 0x0600EA92 RID: 60050 RVA: 0x0013CCEA File Offset: 0x0013AEEA
		public string GetPreModifiedAttributeToolTip(Attribute attribute)
		{
			return attribute.formatter.GetTooltip(attribute, this.GetPreModifiers(attribute), null);
		}

		// Token: 0x0600EA93 RID: 60051 RVA: 0x004CA620 File Offset: 0x004C8820
		public List<AttributeModifier> GetPreModifiers(Attribute attribute)
		{
			List<AttributeModifier> list = new List<AttributeModifier>();
			foreach (string id in this.initialTraits)
			{
				foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(id).SelfModifiers)
				{
					if (attributeModifier.AttributeId == attribute.Id)
					{
						list.Add(attributeModifier);
					}
				}
			}
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null && component.MutationIDs != null)
			{
				foreach (string id2 in component.MutationIDs)
				{
					foreach (AttributeModifier attributeModifier2 in Db.Get().PlantMutations.Get(id2).SelfModifiers)
					{
						if (attributeModifier2.AttributeId == attribute.Id)
						{
							list.Add(attributeModifier2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600EA94 RID: 60052 RVA: 0x0013CD00 File Offset: 0x0013AF00
		public void Serialize(BinaryWriter writer)
		{
			this.OnSerialize(writer);
		}

		// Token: 0x0600EA95 RID: 60053 RVA: 0x0013CD09 File Offset: 0x0013AF09
		public void Deserialize(IReader reader)
		{
			this.OnDeserialize(reader);
		}

		// Token: 0x0600EA96 RID: 60054 RVA: 0x0013CD12 File Offset: 0x0013AF12
		public virtual void OnSerialize(BinaryWriter writer)
		{
			this.amounts.Serialize(writer);
			this.sicknesses.Serialize(writer);
		}

		// Token: 0x0600EA97 RID: 60055 RVA: 0x0013CD2C File Offset: 0x0013AF2C
		public virtual void OnDeserialize(IReader reader)
		{
			this.amounts.Deserialize(reader);
			this.sicknesses.Deserialize(reader);
		}

		// Token: 0x0600EA98 RID: 60056 RVA: 0x0013CD46 File Offset: 0x0013AF46
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.amounts != null)
			{
				this.amounts.Cleanup();
			}
		}

		// Token: 0x0400E5CB RID: 58827
		public Amounts amounts;

		// Token: 0x0400E5CC RID: 58828
		public Attributes attributes;

		// Token: 0x0400E5CD RID: 58829
		public Sicknesses sicknesses;

		// Token: 0x0400E5CE RID: 58830
		public List<string> initialTraits = new List<string>();

		// Token: 0x0400E5CF RID: 58831
		public List<string> initialAmounts = new List<string>();

		// Token: 0x0400E5D0 RID: 58832
		public List<string> initialAttributes = new List<string>();
	}
}
