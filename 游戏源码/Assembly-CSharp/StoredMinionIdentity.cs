using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B23 RID: 2851
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StoredMinionIdentity")]
public class StoredMinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, IPersonalPriorityManager
{
	// Token: 0x17000255 RID: 597
	// (get) Token: 0x0600360D RID: 13837 RVA: 0x000C32C7 File Offset: 0x000C14C7
	// (set) Token: 0x0600360E RID: 13838 RVA: 0x000C32CF File Offset: 0x000C14CF
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x0600360F RID: 13839 RVA: 0x000C32D8 File Offset: 0x000C14D8
	// (set) Token: 0x06003610 RID: 13840 RVA: 0x000C32E0 File Offset: 0x000C14E0
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06003611 RID: 13841 RVA: 0x000C32E9 File Offset: 0x000C14E9
	// (set) Token: 0x06003612 RID: 13842 RVA: 0x000C32F1 File Offset: 0x000C14F1
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x06003613 RID: 13843 RVA: 0x00212430 File Offset: 0x00210630
	[OnDeserialized]
	[Obsolete]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					num++;
				}
			}
			this.TotalExperienceGained = MinionResume.CalculatePreviousExperienceBar(num);
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
		if (!this.model.IsValid)
		{
			this.model = MinionConfig.MODEL;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
		}
		List<ResourceRef<Accessory>> list = this.accessories.FindAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		if (list.Count > 0)
		{
			List<ClothingItemResource> list2 = new List<ClothingItemResource>();
			foreach (ResourceRef<Accessory> resourceRef in list)
			{
				ClothingItemResource clothingItemResource = Db.Get().Permits.ClothingItems.TryResolveAccessoryResource(resourceRef.Guid);
				if (clothingItemResource != null && !list2.Contains(clothingItemResource))
				{
					list2.Add(clothingItemResource);
					this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing].Add(new ResourceRef<ClothingItemResource>(clothingItemResource));
				}
			}
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		this.OnDeserializeModifiers();
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x002126A0 File Offset: 0x002108A0
	public bool HasPerk(SkillPerk perk)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).perks.Contains(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x000C32FA File Offset: 0x000C14FA
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x000C3318 File Offset: 0x000C1518
	protected override void OnPrefabInit()
	{
		this.assignableProxy = new Ref<MinionAssignablesProxy>();
		this.minionModifiers = base.GetComponent<MinionModifiers>();
		this.savedAttributeValues = new Dictionary<string, float>();
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x00212720 File Offset: 0x00210920
	[OnSerializing]
	private void OnSerialize()
	{
		this.savedAttributeValues.Clear();
		foreach (AttributeInstance attributeInstance in this.minionModifiers.attributes)
		{
			this.savedAttributeValues.Add(attributeInstance.Attribute.Id, attributeInstance.GetTotalValue());
		}
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x00212794 File Offset: 0x00210994
	protected override void OnSpawn()
	{
		string[] attributes = MinionConfig.GetAttributes();
		string[] amounts = MinionConfig.GetAmounts();
		AttributeModifier[] traits = MinionConfig.GetTraits();
		if (this.model == BionicMinionConfig.MODEL)
		{
			attributes = BionicMinionConfig.GetAttributes();
			amounts = BionicMinionConfig.GetAmounts();
			traits = BionicMinionConfig.GetTraits();
		}
		BaseMinionConfig.AddMinionAttributes(this.minionModifiers, attributes);
		BaseMinionConfig.AddMinionAmounts(this.minionModifiers, amounts);
		BaseMinionConfig.AddMinionTraits(BaseMinionConfig.GetMinionNameForModel(this.model), BaseMinionConfig.GetMinionBaseTraitIDForModel(this.model), this.minionModifiers, traits);
		this.ValidateProxy();
		this.CleanupLimboMinions();
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x000C333C File Offset: 0x000C153C
	public void OnHardDelete()
	{
		if (this.assignableProxy.Get() != null)
		{
			Util.KDestroyGameObject(this.assignableProxy.Get().gameObject);
		}
		ScheduleManager.Instance.OnStoredDupeDestroyed(this);
		Components.StoredMinionIdentities.Remove(this);
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x00212820 File Offset: 0x00210A20
	private void OnDeserializeModifiers()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.savedAttributeValues)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(keyValuePair.Key);
			if (attribute == null)
			{
				attribute = Db.Get().BuildingAttributes.TryGet(keyValuePair.Key);
			}
			if (attribute != null)
			{
				if (this.minionModifiers.attributes.Get(attribute.Id) != null)
				{
					this.minionModifiers.attributes.Get(attribute.Id).Modifiers.Clear();
					this.minionModifiers.attributes.Get(attribute.Id).ClearModifiers();
				}
				else
				{
					this.minionModifiers.attributes.Add(attribute);
				}
				this.minionModifiers.attributes.Add(new AttributeModifier(attribute.Id, keyValuePair.Value, () => DUPLICANTS.ATTRIBUTES.STORED_VALUE, false, false));
			}
		}
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x000C337C File Offset: 0x000C157C
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x00212954 File Offset: 0x00210B54
	public string[] GetClothingItemIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customClothingItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customClothingItems[outfitType].Count];
			for (int i = 0; i < this.customClothingItems[outfitType].Count; i++)
			{
				array[i] = this.customClothingItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return null;
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x002129C4 File Offset: 0x00210BC4
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		bool flag = false;
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Stored minion with an invalid kpid! Attempting to recover...",
				this.storedName
			});
			flag = true;
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.storedName
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
		bool flag2 = false;
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
			for (int i = 0; i < storedMinionInfo.Count; i++)
			{
				MinionStorage.Info info = storedMinionInfo[i];
				if (flag && info.serializedMinion != null && info.serializedMinion.GetId() == -1 && info.name == this.storedName)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Found a minion storage with an invalid ref, rebinding.",
						component.InstanceID,
						this.storedName,
						minionStorage.gameObject.name
					});
					info = new MinionStorage.Info(this.storedName, new Ref<KPrefabID>(component));
					storedMinionInfo[i] = info;
					minionStorage.GetComponent<Assignable>().Assign(this);
					flag2 = true;
					break;
				}
				if (info.serializedMinion != null && info.serializedMinion.Get() == component)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		if (!flag2)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Found a stored minion that wasn't in any minion storage. Respawning them at the portal.",
				component.InstanceID,
				this.storedName
			});
			GameObject activeTelepad = GameUtil.GetActiveTelepad();
			if (activeTelepad != null)
			{
				MinionStorage.DeserializeMinion(component.gameObject, activeTelepad.transform.GetPosition());
			}
		}
	}

	// Token: 0x0600361E RID: 13854 RVA: 0x000C3390 File Offset: 0x000C1590
	public string GetProperName()
	{
		return this.storedName;
	}

	// Token: 0x0600361F RID: 13855 RVA: 0x000C3398 File Offset: 0x000C1598
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x000C33AA File Offset: 0x000C15AA
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x000C33BC File Offset: 0x000C15BC
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x06003622 RID: 13858 RVA: 0x000C33CF File Offset: 0x000C15CF
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x00212CA4 File Offset: 0x00210EA4
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x000C08BB File Offset: 0x000BEABB
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x00212D08 File Offset: 0x00210F08
	public string GetStorageReason()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			using (List<MinionStorage.Info>.Enumerator enumerator2 = minionStorage.GetStoredMinionInfo().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.serializedMinion.Get() == component)
					{
						return minionStorage.GetProperName();
					}
				}
			}
		}
		return "";
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x000C33DC File Offset: 0x000C15DC
	public bool IsPermittedToConsume(string consumable)
	{
		return !this.forbiddenTagSet.Contains(consumable);
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x00212DC0 File Offset: 0x00210FC0
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		foreach (string id in this.traitIDs)
		{
			if (Db.Get().traits.Exists(id))
			{
				Trait trait = Db.Get().traits.Get(id);
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == chore_group.IdHash)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x00212E70 File Offset: 0x00211070
	public int GetPersonalPriority(ChoreGroup chore_group)
	{
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(chore_group.IdHash, out priorityInfo))
		{
			return priorityInfo.priority;
		}
		return 0;
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return 0;
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x040024C3 RID: 9411
	[Serialize]
	public string storedName;

	// Token: 0x040024C4 RID: 9412
	[Serialize]
	public Tag model;

	// Token: 0x040024C5 RID: 9413
	[Serialize]
	public string gender;

	// Token: 0x040024C9 RID: 9417
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x040024CA RID: 9418
	[Serialize]
	public int voiceIdx;

	// Token: 0x040024CB RID: 9419
	[Serialize]
	public KCompBuilder.BodyData bodyData;

	// Token: 0x040024CC RID: 9420
	[Serialize]
	public List<Ref<KPrefabID>> assignedItems;

	// Token: 0x040024CD RID: 9421
	[Serialize]
	public List<Ref<KPrefabID>> equippedItems;

	// Token: 0x040024CE RID: 9422
	[Serialize]
	public List<string> traitIDs;

	// Token: 0x040024CF RID: 9423
	[Serialize]
	public List<ResourceRef<Accessory>> accessories;

	// Token: 0x040024D0 RID: 9424
	[Obsolete("Deprecated, use customClothingItems")]
	[Serialize]
	public List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x040024D1 RID: 9425
	[Serialize]
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customClothingItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x040024D2 RID: 9426
	[Serialize]
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x040024D3 RID: 9427
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	public List<Tag> forbiddenTags;

	// Token: 0x040024D4 RID: 9428
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x040024D5 RID: 9429
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x040024D6 RID: 9430
	[Serialize]
	public List<Effects.SaveLoadEffect> saveLoadEffects;

	// Token: 0x040024D7 RID: 9431
	[Serialize]
	public List<Effects.SaveLoadImmunities> saveLoadImmunities;

	// Token: 0x040024D8 RID: 9432
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x040024D9 RID: 9433
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x040024DA RID: 9434
	[Serialize]
	public List<string> grantedSkillIDs = new List<string>();

	// Token: 0x040024DB RID: 9435
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x040024DC RID: 9436
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x040024DD RID: 9437
	[Serialize]
	public float TotalExperienceGained;

	// Token: 0x040024DE RID: 9438
	[Serialize]
	public string currentHat;

	// Token: 0x040024DF RID: 9439
	[Serialize]
	public string targetHat;

	// Token: 0x040024E0 RID: 9440
	[Serialize]
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x040024E1 RID: 9441
	[Serialize]
	public List<AttributeLevels.LevelSaveLoad> attributeLevels;

	// Token: 0x040024E2 RID: 9442
	[Serialize]
	public Dictionary<string, float> savedAttributeValues;

	// Token: 0x040024E3 RID: 9443
	public MinionModifiers minionModifiers;
}
