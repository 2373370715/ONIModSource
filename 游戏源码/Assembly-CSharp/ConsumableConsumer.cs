using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	public Tag[] forbiddenTags;

	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	public System.Action consumableRulesChanged;

	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			forbiddenTagSet = new HashSet<Tag>(forbiddenTags);
			forbiddenTags = null;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ConsumerManager.instance != null)
		{
			forbiddenTagSet = new HashSet<Tag>(ConsumerManager.instance.DefaultForbiddenTagsList);
		}
		else
		{
			forbiddenTagSet = new HashSet<Tag>();
		}
	}

	public bool IsPermitted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return !forbiddenTagSet.Contains(item);
	}

	public void SetPermitted(string consumable_id, bool is_allowed)
	{
		Tag item = new Tag(consumable_id);
		if (is_allowed)
		{
			forbiddenTagSet.Remove(item);
		}
		else
		{
			forbiddenTagSet.Add(item);
		}
		consumableRulesChanged.Signal();
	}
}
