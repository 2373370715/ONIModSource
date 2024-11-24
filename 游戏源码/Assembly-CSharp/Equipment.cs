using System;
using Klei;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020012A9 RID: 4777
[SerializationConfig(MemberSerialization.OptIn)]
public class Equipment : Assignables
{
	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x0600623F RID: 25151 RVA: 0x000E015F File Offset: 0x000DE35F
	// (set) Token: 0x06006240 RID: 25152 RVA: 0x000E0167 File Offset: 0x000DE367
	public bool destroyed { get; private set; }

	// Token: 0x06006241 RID: 25153 RVA: 0x002B5E10 File Offset: 0x002B4010
	public GameObject GetTargetGameObject()
	{
		MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)base.GetAssignableIdentity();
		if (minionAssignablesProxy)
		{
			return minionAssignablesProxy.GetTargetGameObject();
		}
		return null;
	}

	// Token: 0x06006242 RID: 25154 RVA: 0x000E0170 File Offset: 0x000DE370
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Equipment.Add(this);
	}

	// Token: 0x06006243 RID: 25155 RVA: 0x000E0183 File Offset: 0x000DE383
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Equipment>(1502190696, Equipment.SetDestroyedTrueDelegate);
		base.Subscribe<Equipment>(1969584890, Equipment.SetDestroyedTrueDelegate);
	}

	// Token: 0x06006244 RID: 25156 RVA: 0x000E01AD File Offset: 0x000DE3AD
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.refreshHandle.ClearScheduler();
		Components.Equipment.Remove(this);
	}

	// Token: 0x06006245 RID: 25157 RVA: 0x002B5E3C File Offset: 0x002B403C
	public void Equip(Equippable equippable)
	{
		GameObject targetGameObject = this.GetTargetGameObject();
		bool flag = targetGameObject.GetComponent<KBatchedAnimController>() == null;
		if (!flag)
		{
			PrimaryElement component = equippable.GetComponent<PrimaryElement>();
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			invalid.idx = component.DiseaseIdx;
			invalid.count = (int)((float)component.DiseaseCount * 0.33f);
			PrimaryElement component2 = targetGameObject.GetComponent<PrimaryElement>();
			SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
			invalid2.idx = component2.DiseaseIdx;
			invalid2.count = (int)((float)component2.DiseaseCount * 0.33f);
			component2.ModifyDiseaseCount(-invalid2.count, "Equipment.Equip");
			component.ModifyDiseaseCount(-invalid.count, "Equipment.Equip");
			if (invalid2.count > 0)
			{
				component.AddDisease(invalid2.idx, invalid2.count, "Equipment.Equip");
			}
			if (invalid.count > 0)
			{
				component2.AddDisease(invalid.idx, invalid.count, "Equipment.Equip");
			}
		}
		AssignableSlotInstance slot = base.GetSlot(equippable.slot);
		slot.Assign(equippable);
		global::Debug.Assert(targetGameObject, "GetTargetGameObject returned null in Equip");
		targetGameObject.Trigger(-448952673, equippable.GetComponent<KPrefabID>());
		equippable.Trigger(-1617557748, this);
		Attributes attributes = targetGameObject.GetAttributes();
		if (attributes != null)
		{
			foreach (AttributeModifier modifier in equippable.def.AttributeModifiers)
			{
				attributes.Add(modifier);
			}
		}
		SnapOn component3 = targetGameObject.GetComponent<SnapOn>();
		if (component3 != null)
		{
			component3.AttachSnapOnByName(equippable.def.SnapOn);
			if (equippable.def.SnapOn1 != null)
			{
				component3.AttachSnapOnByName(equippable.def.SnapOn1);
			}
		}
		if (equippable.transform.parent)
		{
			Storage component4 = equippable.transform.parent.GetComponent<Storage>();
			if (component4)
			{
				component4.Drop(equippable.gameObject, true);
			}
		}
		equippable.transform.parent = slot.gameObject.transform;
		equippable.transform.SetLocalPosition(Vector3.zero);
		this.SetEquippableStoredModifiers(equippable, true);
		equippable.OnEquip(slot);
		if (this.refreshHandle.TimeRemaining > 0f)
		{
			global::Debug.LogWarning(targetGameObject.GetProperName() + " is already in the process of changing equipment (equip)");
			this.refreshHandle.ClearScheduler();
		}
		CreatureSimTemperatureTransfer transferer = targetGameObject.GetComponent<CreatureSimTemperatureTransfer>();
		if (!flag)
		{
			this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 2f, delegate(object obj)
			{
				if (transferer != null)
				{
					transferer.RefreshRegistration();
				}
			}, null, null);
		}
		Game.Instance.Trigger(-2146166042, null);
	}

	// Token: 0x06006246 RID: 25158 RVA: 0x002B610C File Offset: 0x002B430C
	public void Unequip(Equippable equippable)
	{
		AssignableSlotInstance slot = base.GetSlot(equippable.slot);
		slot.Unassign(true);
		GameObject targetGameObject = this.GetTargetGameObject();
		MinionResume minionResume = (targetGameObject != null) ? targetGameObject.GetComponent<MinionResume>() : null;
		Durability component = equippable.GetComponent<Durability>();
		if (component && minionResume && !slot.IsUnassigning() && minionResume.HasPerk(Db.Get().SkillPerks.ExosuitDurability.Id))
		{
			float num = (GameClock.Instance.GetTimeInCycles() - component.TimeEquipped) * EQUIPMENT.SUITS.SUIT_DURABILITY_SKILL_BONUS;
			component.TimeEquipped += num;
		}
		equippable.Trigger(-170173755, this);
		if (!targetGameObject)
		{
			return;
		}
		targetGameObject.Trigger(-1285462312, equippable.GetComponent<KPrefabID>());
		KBatchedAnimController component2 = targetGameObject.GetComponent<KBatchedAnimController>();
		if (!this.destroyed)
		{
			Attributes attributes = targetGameObject.GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in equippable.def.AttributeModifiers)
				{
					attributes.Remove(modifier);
				}
			}
			if (!equippable.def.IsBody)
			{
				SnapOn component3 = targetGameObject.GetComponent<SnapOn>();
				if (equippable.def.SnapOn != null)
				{
					component3.DetachSnapOnByName(equippable.def.SnapOn);
				}
				if (equippable.def.SnapOn1 != null)
				{
					component3.DetachSnapOnByName(equippable.def.SnapOn1);
				}
			}
			if (equippable.transform.parent)
			{
				Storage component4 = equippable.transform.parent.GetComponent<Storage>();
				if (component4)
				{
					component4.Drop(equippable.gameObject, true);
				}
			}
			this.SetEquippableStoredModifiers(equippable, false);
			equippable.transform.parent = null;
			equippable.transform.SetPosition(targetGameObject.transform.GetPosition() + Vector3.up / 2f);
			KBatchedAnimController component5 = equippable.GetComponent<KBatchedAnimController>();
			if (component5)
			{
				component5.SetSceneLayer(Grid.SceneLayer.Ore);
			}
			if (!(component2 == null))
			{
				if (this.refreshHandle.TimeRemaining > 0f)
				{
					this.refreshHandle.ClearScheduler();
				}
				Equipment instance = this;
				this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 1f, delegate(object obj)
				{
					GameObject gameObject = (instance != null) ? instance.GetTargetGameObject() : null;
					if (gameObject)
					{
						CreatureSimTemperatureTransfer component8 = gameObject.GetComponent<CreatureSimTemperatureTransfer>();
						if (component8 != null)
						{
							component8.RefreshRegistration();
						}
					}
				}, null, null);
			}
			if (!slot.IsUnassigning())
			{
				PrimaryElement component6 = equippable.GetComponent<PrimaryElement>();
				PrimaryElement component7 = targetGameObject.GetComponent<PrimaryElement>();
				if (component6 != null && component7 != null)
				{
					SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
					invalid.idx = component6.DiseaseIdx;
					invalid.count = (int)((float)component6.DiseaseCount * 0.33f);
					SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
					invalid2.idx = component7.DiseaseIdx;
					invalid2.count = (int)((float)component7.DiseaseCount * 0.33f);
					component7.ModifyDiseaseCount(-invalid2.count, "Equipment.Unequip");
					component6.ModifyDiseaseCount(-invalid.count, "Equipment.Unequip");
					if (invalid2.count > 0)
					{
						component6.AddDisease(invalid2.idx, invalid2.count, "Equipment.Unequip");
					}
					if (invalid.count > 0)
					{
						component7.AddDisease(invalid.idx, invalid.count, "Equipment.Unequip");
					}
					if (component != null && component.IsWornOut())
					{
						component.ConvertToWornObject();
					}
				}
			}
		}
		Game.Instance.Trigger(-2146166042, null);
	}

	// Token: 0x06006247 RID: 25159 RVA: 0x000E01CB File Offset: 0x000DE3CB
	public bool IsEquipped(Equippable equippable)
	{
		return equippable.assignee is Equipment && (Equipment)equippable.assignee == this && equippable.isEquipped;
	}

	// Token: 0x06006248 RID: 25160 RVA: 0x002B64AC File Offset: 0x002B46AC
	public bool IsSlotOccupied(AssignableSlot slot)
	{
		EquipmentSlotInstance equipmentSlotInstance = base.GetSlot(slot) as EquipmentSlotInstance;
		return equipmentSlotInstance.IsAssigned() && (equipmentSlotInstance.assignable as Equippable).isEquipped;
	}

	// Token: 0x06006249 RID: 25161 RVA: 0x002B64E0 File Offset: 0x002B46E0
	public void UnequipAll()
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.assignable != null)
			{
				assignableSlotInstance.assignable.Unassign();
			}
		}
	}

	// Token: 0x0600624A RID: 25162 RVA: 0x000E01F5 File Offset: 0x000DE3F5
	private void SetEquippableStoredModifiers(Equippable equippable, bool isStoring)
	{
		GameObject gameObject = equippable.gameObject;
		Storage.MakeItemTemperatureInsulated(gameObject, isStoring, false);
		Storage.MakeItemInvisible(gameObject, isStoring, false);
	}

	// Token: 0x040045EE RID: 17902
	private SchedulerHandle refreshHandle;

	// Token: 0x040045F0 RID: 17904
	private static readonly EventSystem.IntraObjectHandler<Equipment> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equipment>(delegate(Equipment component, object data)
	{
		component.destroyed = true;
	});
}
