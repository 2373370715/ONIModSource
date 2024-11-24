using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B89 RID: 15241
	public class Trait : Modifier
	{
		// Token: 0x0600EAC6 RID: 60102 RVA: 0x0013CF6F File Offset: 0x0013B16F
		public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait) : base(id, name, description)
		{
			this.Rating = rating;
			this.ShouldSave = should_save;
			this.disabledChoreGroups = disallowed_chore_groups;
			this.PositiveTrait = positive_trait;
			this.ValidStarterTrait = is_valid_starter_trait;
			this.ignoredEffects = new string[0];
		}

		// Token: 0x0600EAC7 RID: 60103 RVA: 0x004CB040 File Offset: 0x004C9240
		public void AddIgnoredEffects(string[] effects)
		{
			List<string> list = new List<string>(this.ignoredEffects);
			list.AddRange(effects);
			this.ignoredEffects = list.ToArray();
		}

		// Token: 0x0600EAC8 RID: 60104 RVA: 0x004CB06C File Offset: 0x004C926C
		public string GetTooltip()
		{
			string text;
			if (this.TooltipCB != null)
			{
				text = this.TooltipCB();
			}
			else
			{
				text = this.description;
				text += this.GetAttributeModifiersString(true);
				text += this.GetDisabledChoresString(true);
				text += this.GetIgnoredEffectsString(true);
				text += this.GetExtendedTooltipStr();
			}
			return text;
		}

		// Token: 0x0600EAC9 RID: 60105 RVA: 0x004CB0D0 File Offset: 0x004C92D0
		public string GetAttributeModifiersString(bool list_entry)
		{
			string text = "";
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				if (list_entry)
				{
					text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
				}
				text += string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString());
			}
			return text;
		}

		// Token: 0x0600EACA RID: 60106 RVA: 0x004CB170 File Offset: 0x004C9370
		public string GetDisabledChoresString(bool list_entry)
		{
			string text = "";
			if (this.disabledChoreGroups != null)
			{
				string format = DUPLICANTS.TRAITS.CANNOT_DO_TASK;
				if (this.isTaskBeingRefused)
				{
					format = DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
				}
				foreach (ChoreGroup choreGroup in this.disabledChoreGroups)
				{
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					text += string.Format(format, choreGroup.Name);
				}
			}
			return text;
		}

		// Token: 0x0600EACB RID: 60107 RVA: 0x004CB1EC File Offset: 0x004C93EC
		public string GetIgnoredEffectsString(bool list_entry)
		{
			string text = "";
			if (this.ignoredEffects != null && this.ignoredEffects.Length != 0)
			{
				for (int i = 0; i < this.ignoredEffects.Length; i++)
				{
					string text2 = this.ignoredEffects[i];
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
					text += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS, arg);
					if (!list_entry && i < this.ignoredEffects.Length - 1)
					{
						text += "\n";
					}
				}
			}
			return text;
		}

		// Token: 0x0600EACC RID: 60108 RVA: 0x004CB29C File Offset: 0x004C949C
		public string GetExtendedTooltipStr()
		{
			string text = "";
			if (this.ExtendedTooltip != null)
			{
				foreach (Func<string> func in this.ExtendedTooltip.GetInvocationList())
				{
					text = text + "\n" + func();
				}
			}
			return text;
		}

		// Token: 0x0600EACD RID: 60109 RVA: 0x004CB2F0 File Offset: 0x004C94F0
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, false);
				}
			}
		}

		// Token: 0x0600EACE RID: 60110 RVA: 0x004CB344 File Offset: 0x004C9544
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, true);
				}
			}
		}

		// Token: 0x0400E5E4 RID: 58852
		public float Rating;

		// Token: 0x0400E5E5 RID: 58853
		public bool ShouldSave;

		// Token: 0x0400E5E6 RID: 58854
		public bool PositiveTrait;

		// Token: 0x0400E5E7 RID: 58855
		public bool ValidStarterTrait;

		// Token: 0x0400E5E8 RID: 58856
		public Action<GameObject> OnAddTrait;

		// Token: 0x0400E5E9 RID: 58857
		public Func<string> TooltipCB;

		// Token: 0x0400E5EA RID: 58858
		public Func<string> ExtendedTooltip;

		// Token: 0x0400E5EB RID: 58859
		public ChoreGroup[] disabledChoreGroups;

		// Token: 0x0400E5EC RID: 58860
		public bool isTaskBeingRefused;

		// Token: 0x0400E5ED RID: 58861
		public string[] ignoredEffects;
	}
}
