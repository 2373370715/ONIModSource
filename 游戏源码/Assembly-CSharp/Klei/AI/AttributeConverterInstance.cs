using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B04 RID: 15108
	public class AttributeConverterInstance : ModifierInstance<AttributeConverter>
	{
		// Token: 0x0600E861 RID: 59489 RVA: 0x0013B713 File Offset: 0x00139913
		public AttributeConverterInstance(GameObject game_object, AttributeConverter converter, AttributeInstance attribute_instance) : base(game_object, converter)
		{
			this.converter = converter;
			this.attributeInstance = attribute_instance;
		}

		// Token: 0x0600E862 RID: 59490 RVA: 0x0013B72B File Offset: 0x0013992B
		public float Evaluate()
		{
			return this.converter.multiplier * this.attributeInstance.GetTotalValue() + this.converter.baseValue;
		}

		// Token: 0x0600E863 RID: 59491 RVA: 0x0013B750 File Offset: 0x00139950
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			return this.converter.DescriptionFromAttribute(this.Evaluate(), go);
		}

		// Token: 0x0400E438 RID: 58424
		public AttributeConverter converter;

		// Token: 0x0400E439 RID: 58425
		public AttributeInstance attributeInstance;
	}
}
