using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B03 RID: 15107
	public class AttributeConverter : Resource
	{
		// Token: 0x0600E85D RID: 59485 RVA: 0x0013B6D4 File Offset: 0x001398D4
		public AttributeConverter(string id, string name, string description, float multiplier, float base_value, Attribute attribute, IAttributeFormatter formatter = null) : base(id, name)
		{
			this.description = description;
			this.multiplier = multiplier;
			this.baseValue = base_value;
			this.attribute = attribute;
			this.formatter = formatter;
		}

		// Token: 0x0600E85E RID: 59486 RVA: 0x0013B705 File Offset: 0x00139905
		public AttributeConverterInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600E85F RID: 59487 RVA: 0x004C0CF8 File Offset: 0x004BEEF8
		public AttributeConverterInstance Lookup(GameObject go)
		{
			AttributeConverters component = go.GetComponent<AttributeConverters>();
			if (component != null)
			{
				return component.Get(this);
			}
			return null;
		}

		// Token: 0x0600E860 RID: 59488 RVA: 0x004C0D20 File Offset: 0x004BEF20
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			string text;
			if (this.formatter != null)
			{
				text = this.formatter.GetFormattedValue(value, this.formatter.DeltaTimeSlice);
			}
			else if (this.attribute.formatter != null)
			{
				text = this.attribute.formatter.GetFormattedValue(value, this.attribute.formatter.DeltaTimeSlice);
			}
			else
			{
				text = GameUtil.GetFormattedSimple(value, GameUtil.TimeSlice.None, null);
			}
			if (text != null)
			{
				text = GameUtil.AddPositiveSign(text, value > 0f);
				return string.Format(this.description, text);
			}
			return null;
		}

		// Token: 0x0400E433 RID: 58419
		public string description;

		// Token: 0x0400E434 RID: 58420
		public float multiplier;

		// Token: 0x0400E435 RID: 58421
		public float baseValue;

		// Token: 0x0400E436 RID: 58422
		public Attribute attribute;

		// Token: 0x0400E437 RID: 58423
		public IAttributeFormatter formatter;
	}
}
