using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B82 RID: 15234
	[AddComponentMenu("KMonoBehaviour/scripts/PrefabAttributeModifiers")]
	public class PrefabAttributeModifiers : KMonoBehaviour
	{
		// Token: 0x0600EAA0 RID: 60064 RVA: 0x000B2F5A File Offset: 0x000B115A
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
		}

		// Token: 0x0600EAA1 RID: 60065 RVA: 0x0013CDC5 File Offset: 0x0013AFC5
		public void AddAttributeDescriptor(AttributeModifier modifier)
		{
			this.descriptors.Add(modifier);
		}

		// Token: 0x0600EAA2 RID: 60066 RVA: 0x0013CDD3 File Offset: 0x0013AFD3
		public void RemovePrefabAttribute(AttributeModifier modifier)
		{
			this.descriptors.Remove(modifier);
		}

		// Token: 0x0400E5D1 RID: 58833
		public List<AttributeModifier> descriptors = new List<AttributeModifier>();
	}
}
