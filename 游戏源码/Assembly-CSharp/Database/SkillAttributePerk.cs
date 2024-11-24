using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x020021BD RID: 8637
	public class SkillAttributePerk : SkillPerk
	{
		// Token: 0x0600B750 RID: 46928 RVA: 0x0045DEAC File Offset: 0x0045C0AC
		public SkillAttributePerk(string id, string attributeId, float modifierBonus, string modifierDesc) : base(id, "", null, null, delegate(MinionResume identity)
		{
		}, null, false)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.Get(attributeId);
			this.modifier = new AttributeModifier(attributeId, modifierBonus, modifierDesc, false, false, true);
			this.Name = string.Format(UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, this.modifier.GetFormattedString(), attribute.Name);
			base.OnApply = delegate(MinionResume identity)
			{
				if (identity.GetAttributes().Get(this.modifier.AttributeId).Modifiers.FindIndex((AttributeModifier mod) => mod == this.modifier) == -1)
				{
					identity.GetAttributes().Add(this.modifier);
				}
			};
			base.OnRemove = delegate(MinionResume identity)
			{
				identity.GetAttributes().Remove(this.modifier);
			};
		}

		// Token: 0x0400956C RID: 38252
		public AttributeModifier modifier;
	}
}
