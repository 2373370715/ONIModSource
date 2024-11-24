using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x0200077D RID: 1917
[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
	// Token: 0x06002286 RID: 8838 RVA: 0x001C31EC File Offset: 0x001C13EC
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

	// Token: 0x06002287 RID: 8839 RVA: 0x001C3228 File Offset: 0x001C1428
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

	// Token: 0x06002288 RID: 8840 RVA: 0x001C32BC File Offset: 0x001C14BC
	public bool IsPermitted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return !this.forbiddenTagSet.Contains(item) && !this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x001C32F0 File Offset: 0x001C14F0
	public bool IsDietRestricted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x001C3314 File Offset: 0x001C1514
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

	// Token: 0x040016BB RID: 5819
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	[HideInInspector]
	public Tag[] forbiddenTags;

	// Token: 0x040016BC RID: 5820
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x040016BD RID: 5821
	[Serialize]
	public HashSet<Tag> dietaryRestrictionTagSet;

	// Token: 0x040016BE RID: 5822
	public System.Action consumableRulesChanged;
}
