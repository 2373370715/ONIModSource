using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI;

public class Trait : Modifier
{
	public float Rating;

	public bool ShouldSave;

	public bool PositiveTrait;

	public bool ValidStarterTrait;

	public Action<GameObject> OnAddTrait;

	public Func<string> TooltipCB;

	public Func<string> ExtendedTooltip;

	public ChoreGroup[] disabledChoreGroups;

	public bool isTaskBeingRefused;

	public string[] ignoredEffects;

	public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait)
		: base(id, name, description)
	{
		Rating = rating;
		ShouldSave = should_save;
		disabledChoreGroups = disallowed_chore_groups;
		PositiveTrait = positive_trait;
		ValidStarterTrait = is_valid_starter_trait;
		ignoredEffects = new string[0];
	}

	public void AddIgnoredEffects(string[] effects)
	{
		List<string> list = new List<string>(ignoredEffects);
		list.AddRange(effects);
		ignoredEffects = list.ToArray();
	}

	public string GetTooltip()
	{
		if (TooltipCB != null)
		{
			return TooltipCB();
		}
		string text = description;
		text += GetAttributeModifiersString(list_entry: true);
		text += GetDisabledChoresString(list_entry: true);
		text += GetIgnoredEffectsString(list_entry: true);
		return text + GetExtendedTooltipStr();
	}

	public string GetAttributeModifiersString(bool list_entry)
	{
		string text = "";
		foreach (AttributeModifier selfModifier in SelfModifiers)
		{
			Attribute attribute = Db.Get().Attributes.Get(selfModifier.AttributeId);
			if (list_entry)
			{
				text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
			}
			text += string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, selfModifier.GetFormattedString());
		}
		return text;
	}

	public string GetDisabledChoresString(bool list_entry)
	{
		string text = "";
		if (disabledChoreGroups != null)
		{
			string format = DUPLICANTS.TRAITS.CANNOT_DO_TASK;
			if (isTaskBeingRefused)
			{
				format = DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
			}
			ChoreGroup[] array = disabledChoreGroups;
			foreach (ChoreGroup choreGroup in array)
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

	public string GetIgnoredEffectsString(bool list_entry)
	{
		string text = "";
		if (ignoredEffects != null && ignoredEffects.Length != 0)
		{
			for (int i = 0; i < ignoredEffects.Length; i++)
			{
				string text2 = ignoredEffects[i];
				if (list_entry)
				{
					text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
				}
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
				text += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS, arg);
				if (!list_entry && i < ignoredEffects.Length - 1)
				{
					text += "\n";
				}
			}
		}
		return text;
	}

	public string GetExtendedTooltipStr()
	{
		string text = "";
		if (ExtendedTooltip != null)
		{
			Delegate[] invocationList = ExtendedTooltip.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Func<string> func = (Func<string>)invocationList[i];
				text = text + "\n" + func();
			}
		}
		return text;
	}

	public override void AddTo(Attributes attributes)
	{
		base.AddTo(attributes);
		ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
		if (component != null && disabledChoreGroups != null)
		{
			ChoreGroup[] array = disabledChoreGroups;
			foreach (ChoreGroup chore_group in array)
			{
				component.SetPermittedByTraits(chore_group, is_enabled: false);
			}
		}
	}

	public override void RemoveFrom(Attributes attributes)
	{
		base.RemoveFrom(attributes);
		ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
		if (component != null && disabledChoreGroups != null)
		{
			ChoreGroup[] array = disabledChoreGroups;
			foreach (ChoreGroup chore_group in array)
			{
				component.SetPermittedByTraits(chore_group, is_enabled: true);
			}
		}
	}
}
