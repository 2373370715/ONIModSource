using System;
using System.Collections.Generic;

namespace Klei.AI
{
		public class Modifier : Resource
	{
				public Modifier(string id, string name, string description) : base(id, name)
		{
			this.description = description;
		}

				public void Add(AttributeModifier modifier)
		{
			if (modifier.AttributeId != "")
			{
				this.SelfModifiers.Add(modifier);
			}
		}

				public virtual void AddTo(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Add(modifier);
			}
		}

				public virtual void RemoveFrom(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Remove(modifier);
			}
		}

				public string description;

				public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();
	}
}
