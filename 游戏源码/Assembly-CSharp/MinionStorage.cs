using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020014E4 RID: 5348
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionStorage")]
public class MinionStorage : KMonoBehaviour
{
	// Token: 0x06006F51 RID: 28497 RVA: 0x000E8E05 File Offset: 0x000E7005
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.MinionStorages.Add(this);
	}

	// Token: 0x06006F52 RID: 28498 RVA: 0x000E8E18 File Offset: 0x000E7018
	protected override void OnCleanUp()
	{
		Components.MinionStorages.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06006F53 RID: 28499 RVA: 0x002F2D88 File Offset: 0x002F0F88
	private KPrefabID CreateSerializedMinion(GameObject src_minion)
	{
		GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab(StoredMinionConfig.ID), Vector3.zero);
		gameObject.SetActive(true);
		MinionIdentity component = src_minion.GetComponent<MinionIdentity>();
		StoredMinionIdentity component2 = gameObject.GetComponent<StoredMinionIdentity>();
		this.CopyMinion(component, component2);
		MinionStorage.RedirectInstanceTracker(src_minion, gameObject);
		component.assignableProxy.Get().SetTarget(component2, gameObject);
		Util.KDestroyGameObject(src_minion);
		return gameObject.GetComponent<KPrefabID>();
	}

	// Token: 0x06006F54 RID: 28500 RVA: 0x002F2DFC File Offset: 0x002F0FFC
	private void CopyMinion(MinionIdentity src_id, StoredMinionIdentity dest_id)
	{
		dest_id.storedName = src_id.name;
		dest_id.nameStringKey = src_id.nameStringKey;
		dest_id.personalityResourceId = src_id.personalityResourceId;
		dest_id.model = src_id.model;
		dest_id.gender = src_id.gender;
		dest_id.genderStringKey = src_id.genderStringKey;
		dest_id.arrivalTime = src_id.arrivalTime;
		dest_id.voiceIdx = src_id.voiceIdx;
		dest_id.bodyData = src_id.GetComponent<Accessorizer>().bodyData;
		Traits component = src_id.GetComponent<Traits>();
		dest_id.traitIDs = new List<string>(component.GetTraitIds());
		dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
		dest_id.assignableProxy.Get().SetTarget(dest_id, dest_id.gameObject);
		Accessorizer component2 = src_id.GetComponent<Accessorizer>();
		dest_id.accessories = component2.GetAccessories();
		WearableAccessorizer component3 = src_id.GetComponent<WearableAccessorizer>();
		dest_id.customClothingItems = component3.GetCustomClothingItems();
		dest_id.wearables = component3.Wearables;
		ConsumableConsumer component4 = src_id.GetComponent<ConsumableConsumer>();
		if (component4.forbiddenTagSet != null)
		{
			dest_id.forbiddenTagSet = new HashSet<Tag>(component4.forbiddenTagSet);
		}
		MinionResume component5 = src_id.GetComponent<MinionResume>();
		dest_id.MasteryBySkillID = component5.MasteryBySkillID;
		dest_id.grantedSkillIDs = component5.GrantedSkillIDs;
		dest_id.AptitudeBySkillGroup = component5.AptitudeBySkillGroup;
		dest_id.TotalExperienceGained = component5.TotalExperienceGained;
		dest_id.currentHat = component5.CurrentHat;
		dest_id.targetHat = component5.TargetHat;
		ChoreConsumer component6 = src_id.GetComponent<ChoreConsumer>();
		dest_id.choreGroupPriorities = component6.GetChoreGroupPriorities();
		AttributeLevels component7 = src_id.GetComponent<AttributeLevels>();
		component7.OnSerializing();
		dest_id.attributeLevels = new List<AttributeLevels.LevelSaveLoad>(component7.SaveLoadLevels);
		Effects component8 = src_id.GetComponent<Effects>();
		dest_id.saveLoadEffects = component8.GetAllEffectsForSerialization();
		dest_id.saveLoadImmunities = component8.GetAllImmunitiesForSerialization();
		MinionStorage.StoreModifiers(src_id, dest_id);
		Schedulable component9 = src_id.GetComponent<Schedulable>();
		Schedule schedule = component9.GetSchedule();
		if (schedule != null)
		{
			schedule.Unassign(component9);
			Schedulable component10 = dest_id.GetComponent<Schedulable>();
			schedule.Assign(component10);
		}
	}

	// Token: 0x06006F55 RID: 28501 RVA: 0x002F2FF8 File Offset: 0x002F11F8
	private static void StoreModifiers(MinionIdentity src_id, StoredMinionIdentity dest_id)
	{
		foreach (AttributeInstance attributeInstance in src_id.GetComponent<MinionModifiers>().attributes)
		{
			if (dest_id.minionModifiers.attributes.Get(attributeInstance.Attribute.Id) == null)
			{
				dest_id.minionModifiers.attributes.Add(attributeInstance.Attribute);
			}
			for (int i = 0; i < attributeInstance.Modifiers.Count; i++)
			{
				dest_id.minionModifiers.attributes.Get(attributeInstance.Id).Add(attributeInstance.Modifiers[i]);
			}
		}
	}

	// Token: 0x06006F56 RID: 28502 RVA: 0x002F30B4 File Offset: 0x002F12B4
	private static void CopyMinion(StoredMinionIdentity src_id, MinionIdentity dest_id)
	{
		dest_id.SetName(src_id.storedName);
		dest_id.nameStringKey = src_id.nameStringKey;
		dest_id.model = src_id.model;
		dest_id.personalityResourceId = src_id.personalityResourceId;
		dest_id.gender = src_id.gender;
		dest_id.genderStringKey = src_id.genderStringKey;
		dest_id.arrivalTime = src_id.arrivalTime;
		dest_id.voiceIdx = src_id.voiceIdx;
		dest_id.GetComponent<Accessorizer>().bodyData = src_id.bodyData;
		if (src_id.traitIDs != null)
		{
			dest_id.GetComponent<Traits>().SetTraitIds(src_id.traitIDs);
		}
		if (src_id.accessories != null)
		{
			dest_id.GetComponent<Accessorizer>().SetAccessories(src_id.accessories);
		}
		dest_id.GetComponent<WearableAccessorizer>().RestoreWearables(src_id.wearables, src_id.customClothingItems);
		ConsumableConsumer component = dest_id.GetComponent<ConsumableConsumer>();
		if (src_id.forbiddenTagSet != null)
		{
			component.forbiddenTagSet = new HashSet<Tag>(src_id.forbiddenTagSet);
		}
		if (src_id.MasteryBySkillID != null)
		{
			MinionResume component2 = dest_id.GetComponent<MinionResume>();
			component2.RestoreResume(src_id.MasteryBySkillID, src_id.AptitudeBySkillGroup, src_id.grantedSkillIDs, src_id.TotalExperienceGained);
			component2.SetHats(src_id.currentHat, src_id.targetHat);
		}
		if (src_id.choreGroupPriorities != null)
		{
			dest_id.GetComponent<ChoreConsumer>().SetChoreGroupPriorities(src_id.choreGroupPriorities);
		}
		AttributeLevels component3 = dest_id.GetComponent<AttributeLevels>();
		if (src_id.attributeLevels != null)
		{
			component3.SaveLoadLevels = src_id.attributeLevels.ToArray();
			component3.OnDeserialized();
		}
		Effects component4 = dest_id.GetComponent<Effects>();
		if (src_id.saveLoadImmunities != null)
		{
			foreach (Effects.SaveLoadImmunities saveLoadImmunities in src_id.saveLoadImmunities)
			{
				if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
				{
					Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
					component4.AddImmunity(effect, saveLoadImmunities.giverID, saveLoadImmunities.saved);
				}
			}
		}
		if (src_id.saveLoadEffects != null)
		{
			foreach (Effects.SaveLoadEffect saveLoadEffect in src_id.saveLoadEffects)
			{
				if (Db.Get().effects.Exists(saveLoadEffect.id))
				{
					Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
					EffectInstance effectInstance = component4.Add(newEffect, saveLoadEffect.saved);
					if (effectInstance != null)
					{
						effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
					}
				}
			}
		}
		dest_id.GetComponent<Accessorizer>().ApplyAccessories();
		dest_id.assignableProxy = new Ref<MinionAssignablesProxy>();
		dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
		dest_id.assignableProxy.Get().SetTarget(dest_id, dest_id.gameObject);
		Equipment equipment = dest_id.GetEquipment();
		foreach (AssignableSlotInstance assignableSlotInstance in equipment.Slots)
		{
			Equippable equippable = assignableSlotInstance.assignable as Equippable;
			if (equippable != null)
			{
				equipment.Equip(equippable);
			}
		}
		Schedulable component5 = src_id.GetComponent<Schedulable>();
		Schedule schedule = component5.GetSchedule();
		if (schedule != null)
		{
			schedule.Unassign(component5);
			Schedulable component6 = dest_id.GetComponent<Schedulable>();
			schedule.Assign(component6);
		}
	}

	// Token: 0x06006F57 RID: 28503 RVA: 0x002F341C File Offset: 0x002F161C
	public static void RedirectInstanceTracker(GameObject src_minion, GameObject dest_minion)
	{
		KPrefabID component = src_minion.GetComponent<KPrefabID>();
		dest_minion.GetComponent<KPrefabID>().InstanceID = component.InstanceID;
		component.InstanceID = -1;
	}

	// Token: 0x06006F58 RID: 28504 RVA: 0x002F3448 File Offset: 0x002F1648
	public void SerializeMinion(GameObject minion)
	{
		this.CleanupBadReferences();
		KPrefabID kprefabID = this.CreateSerializedMinion(minion);
		MinionStorage.Info item = new MinionStorage.Info(kprefabID.GetComponent<StoredMinionIdentity>().storedName, new Ref<KPrefabID>(kprefabID));
		this.serializedMinions.Add(item);
	}

	// Token: 0x06006F59 RID: 28505 RVA: 0x002F3488 File Offset: 0x002F1688
	private void CleanupBadReferences()
	{
		for (int i = this.serializedMinions.Count - 1; i >= 0; i--)
		{
			if (this.serializedMinions[i].serializedMinion == null || this.serializedMinions[i].serializedMinion.Get() == null)
			{
				this.serializedMinions.RemoveAt(i);
			}
		}
	}

	// Token: 0x06006F5A RID: 28506 RVA: 0x002F34EC File Offset: 0x002F16EC
	private int GetMinionIndex(Guid id)
	{
		int result = -1;
		for (int i = 0; i < this.serializedMinions.Count; i++)
		{
			if (this.serializedMinions[i].id == id)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006F5B RID: 28507 RVA: 0x002F3530 File Offset: 0x002F1730
	public GameObject DeserializeMinion(Guid id, Vector3 pos)
	{
		int minionIndex = this.GetMinionIndex(id);
		if (minionIndex < 0 || minionIndex >= this.serializedMinions.Count)
		{
			return null;
		}
		KPrefabID kprefabID = this.serializedMinions[minionIndex].serializedMinion.Get();
		this.serializedMinions.RemoveAt(minionIndex);
		if (kprefabID == null)
		{
			return null;
		}
		return MinionStorage.DeserializeMinion(kprefabID.gameObject, pos);
	}

	// Token: 0x06006F5C RID: 28508 RVA: 0x002F3594 File Offset: 0x002F1794
	public static GameObject DeserializeMinion(GameObject sourceMinion, Vector3 pos)
	{
		StoredMinionIdentity component = sourceMinion.GetComponent<StoredMinionIdentity>();
		GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab(BaseMinionConfig.GetMinionIDForModel(component.model)), pos);
		MinionIdentity component2 = gameObject.GetComponent<MinionIdentity>();
		MinionStorage.RedirectInstanceTracker(sourceMinion, gameObject);
		gameObject.SetActive(true);
		MinionStorage.CopyMinion(component, component2);
		component.assignableProxy.Get().SetTarget(component2, gameObject);
		Util.KDestroyGameObject(sourceMinion);
		return gameObject;
	}

	// Token: 0x06006F5D RID: 28509 RVA: 0x002F3604 File Offset: 0x002F1804
	public void DeleteStoredMinion(Guid id)
	{
		int minionIndex = this.GetMinionIndex(id);
		if (minionIndex < 0)
		{
			return;
		}
		if (this.serializedMinions[minionIndex].serializedMinion != null)
		{
			this.serializedMinions[minionIndex].serializedMinion.Get().GetComponent<StoredMinionIdentity>().OnHardDelete();
			Util.KDestroyGameObject(this.serializedMinions[minionIndex].serializedMinion.Get().gameObject);
		}
		this.serializedMinions.RemoveAt(minionIndex);
	}

	// Token: 0x06006F5E RID: 28510 RVA: 0x000E8E2B File Offset: 0x000E702B
	public List<MinionStorage.Info> GetStoredMinionInfo()
	{
		return this.serializedMinions;
	}

	// Token: 0x06006F5F RID: 28511 RVA: 0x000E8E33 File Offset: 0x000E7033
	public void SetStoredMinionInfo(List<MinionStorage.Info> info)
	{
		this.serializedMinions = info;
	}

	// Token: 0x04005347 RID: 21319
	[Serialize]
	private List<MinionStorage.Info> serializedMinions = new List<MinionStorage.Info>();

	// Token: 0x020014E5 RID: 5349
	public struct Info
	{
		// Token: 0x06006F61 RID: 28513 RVA: 0x000E8E4F File Offset: 0x000E704F
		public Info(string name, Ref<KPrefabID> ref_obj)
		{
			this.id = Guid.NewGuid();
			this.name = name;
			this.serializedMinion = ref_obj;
		}

		// Token: 0x06006F62 RID: 28514 RVA: 0x002F3680 File Offset: 0x002F1880
		public static MinionStorage.Info CreateEmpty()
		{
			return new MinionStorage.Info
			{
				id = Guid.Empty,
				name = null,
				serializedMinion = null
			};
		}

		// Token: 0x04005348 RID: 21320
		public Guid id;

		// Token: 0x04005349 RID: 21321
		public string name;

		// Token: 0x0400534A RID: 21322
		public Ref<KPrefabID> serializedMinion;
	}
}
