﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
		[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ConsumerManager.instance != null)
		{
			this.forbiddenTagSet = new HashSet<Tag>(ConsumerManager.instance.DefaultForbiddenTagsList);
			if (this.HasTag(GameTags.Minions.Models.Standard))
			{
				this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.StandardDuplicantDietaryRestrictions);
				return;
			}
			if (this.HasTag(GameTags.Minions.Models.Bionic))
			{
				this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.BionicDuplicantDietaryRestrictions);
				return;
			}
		}
		else
		{
			this.forbiddenTagSet = new HashSet<Tag>();
			this.dietaryRestrictionTagSet = new HashSet<Tag>();
		}
	}

		public bool IsPermitted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return !this.forbiddenTagSet.Contains(item) && !this.dietaryRestrictionTagSet.Contains(item);
	}

		public bool IsDietRestricted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return this.dietaryRestrictionTagSet.Contains(item);
	}

		public void SetPermitted(string consumable_id, bool is_allowed)
	{
		Tag item = new Tag(consumable_id);
		is_allowed = (is_allowed && !this.dietaryRestrictionTagSet.Contains(consumable_id));
		if (is_allowed)
		{
			this.forbiddenTagSet.Remove(item);
		}
		else
		{
			this.forbiddenTagSet.Add(item);
		}
		this.consumableRulesChanged.Signal();
	}

		[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	[HideInInspector]
	public Tag[] forbiddenTags;

		[Serialize]
	public HashSet<Tag> forbiddenTagSet;

		[Serialize]
	public HashSet<Tag> dietaryRestrictionTagSet;

		public System.Action consumableRulesChanged;
}
