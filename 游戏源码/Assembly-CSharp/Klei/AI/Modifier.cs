using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02003B7D RID: 15229
	public class Modifier : Resource
	{
		// Token: 0x0600EA79 RID: 60025 RVA: 0x0013CBA4 File Offset: 0x0013ADA4
		public Modifier(string id, string name, string description) : base(id, name)
		{
			this.description = description;
		}

		// Token: 0x0600EA7A RID: 60026 RVA: 0x0013CBC0 File Offset: 0x0013ADC0
		public void Add(AttributeModifier modifier)
		{
			if (modifier.AttributeId != "")
			{
				this.SelfModifiers.Add(modifier);
			}
		}

		// Token: 0x0600EA7B RID: 60027 RVA: 0x004CA358 File Offset: 0x004C8558
		public virtual void AddTo(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Add(modifier);
			}
		}

		// Token: 0x0600EA7C RID: 60028 RVA: 0x004CA3AC File Offset: 0x004C85AC
		public virtual void RemoveFrom(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Remove(modifier);
			}
		}

		// Token: 0x0400E5C6 RID: 58822
		public string description;

		// Token: 0x0400E5C7 RID: 58823
		public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();
	}
}
