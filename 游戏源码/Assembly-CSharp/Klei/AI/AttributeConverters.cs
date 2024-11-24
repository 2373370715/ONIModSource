using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B05 RID: 15109
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeConverters")]
	public class AttributeConverters : KMonoBehaviour
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x0600E864 RID: 59492 RVA: 0x0013B764 File Offset: 0x00139964
		public int Count
		{
			get
			{
				return this.converters.Count;
			}
		}

		// Token: 0x0600E865 RID: 59493 RVA: 0x004C0DAC File Offset: 0x004BEFAC
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				foreach (AttributeConverter converter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance item = new AttributeConverterInstance(base.gameObject, converter, attributeInstance);
					this.converters.Add(item);
				}
			}
		}

		// Token: 0x0600E866 RID: 59494 RVA: 0x004C0E50 File Offset: 0x004BF050
		public AttributeConverterInstance Get(AttributeConverter converter)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter == converter)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x0600E867 RID: 59495 RVA: 0x004C0EAC File Offset: 0x004BF0AC
		public AttributeConverterInstance GetConverter(string id)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter.Id == id)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x0400E43A RID: 58426
		public List<AttributeConverterInstance> converters = new List<AttributeConverterInstance>();
	}
}
