using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020012B4 RID: 4788
[SerializationConfig(MemberSerialization.OptIn)]
public class Equippable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor, IQuality
{
	// Token: 0x06006266 RID: 25190 RVA: 0x000E0398 File Offset: 0x000DE598
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x06006267 RID: 25191 RVA: 0x000E03A0 File Offset: 0x000DE5A0
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06006268 RID: 25192 RVA: 0x000E03A9 File Offset: 0x000DE5A9
	// (set) Token: 0x06006269 RID: 25193 RVA: 0x000E03B6 File Offset: 0x000DE5B6
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x0600626A RID: 25194 RVA: 0x002B6794 File Offset: 0x002B4994
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x0600626B RID: 25195 RVA: 0x002B67E4 File Offset: 0x002B49E4
	protected override void OnSpawn()
	{
		Components.AssignableItems.Add(this);
		if (this.isEquipped)
		{
			if (this.assignee != null && this.assignee is MinionIdentity)
			{
				this.assignee = (this.assignee as MinionIdentity).assignableProxy.Get();
				this.assignee_identityRef.Set(this.assignee as KMonoBehaviour);
			}
			if (this.assignee == null && this.assignee_identityRef.Get() != null)
			{
				this.assignee = this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
			}
			if (this.assignee != null)
			{
				this.assignee.GetSoleOwner().GetComponent<Equipment>().Equip(this);
			}
			else
			{
				global::Debug.LogWarning("Equippable trying to be equipped to missing prefab");
				this.isEquipped = false;
			}
		}
		base.Subscribe<Equippable>(1969584890, Equippable.SetDestroyedTrueDelegate);
	}

	// Token: 0x0600626C RID: 25196 RVA: 0x002B68C0 File Offset: 0x002B4AC0
	public KAnimFile GetBuildOverride()
	{
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component == null || component.BuildOverride == null)
		{
			return this.def.BuildOverride;
		}
		return Assets.GetAnim(component.BuildOverride);
	}

	// Token: 0x0600626D RID: 25197 RVA: 0x002B6904 File Offset: 0x002B4B04
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Equipment>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x0600626E RID: 25198 RVA: 0x002B69A0 File Offset: 0x002B4BA0
	public override void Unassign()
	{
		if (this.isEquipped)
		{
			((this.assignee is MinionIdentity) ? ((MinionIdentity)this.assignee).assignableProxy.Get().GetComponent<Equipment>() : ((KMonoBehaviour)this.assignee).GetComponent<Equipment>()).Unequip(this);
			this.OnUnequip();
		}
		base.Unassign();
	}

	// Token: 0x0600626F RID: 25199 RVA: 0x002B6A00 File Offset: 0x002B4C00
	public void OnEquip(AssignableSlotInstance slot)
	{
		this.isEquipped = true;
		if (SelectTool.Instance.selected == this.selectable)
		{
			SelectTool.Instance.Select(null, false);
		}
		base.GetComponent<KBatchedAnimController>().enabled = false;
		base.GetComponent<KSelectable>().IsSelectable = false;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		GameObject targetGameObject = slot.gameObject.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		Effects component = targetGameObject.GetComponent<Effects>();
		if (component != null)
		{
			foreach (Effect effect in this.def.EffectImmunites)
			{
				component.AddImmunity(effect, name, true);
			}
		}
		if (this.def.OnEquipCallBack != null)
		{
			this.def.OnEquipCallBack(this);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Equipped, false);
		targetGameObject.Trigger(-210173199, this);
	}

	// Token: 0x06006270 RID: 25200 RVA: 0x002B6B0C File Offset: 0x002B4D0C
	public void OnUnequip()
	{
		this.isEquipped = false;
		if (this.destroyed)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Equipped);
		base.GetComponent<KBatchedAnimController>().enabled = true;
		base.GetComponent<KSelectable>().IsSelectable = true;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		if (this.assignee != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					Effects component = targetGameObject.GetComponent<Effects>();
					if (component != null)
					{
						foreach (Effect effect in this.def.EffectImmunites)
						{
							component.RemoveImmunity(effect, name);
						}
					}
				}
			}
		}
		if (this.def.OnUnequipCallBack != null)
		{
			this.def.OnUnequipCallBack(this);
		}
		if (this.assignee != null)
		{
			Ownables soleOwner2 = this.assignee.GetSoleOwner();
			if (soleOwner2)
			{
				GameObject targetGameObject2 = soleOwner2.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject2)
				{
					targetGameObject2.Trigger(-1841406856, this);
				}
			}
		}
	}

	// Token: 0x06006271 RID: 25201 RVA: 0x002B6C54 File Offset: 0x002B4E54
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.def != null)
		{
			List<Descriptor> equipmentEffects = GameUtil.GetEquipmentEffects(this.def);
			if (this.def.additionalDescriptors != null)
			{
				foreach (Descriptor item in this.def.additionalDescriptors)
				{
					equipmentEffects.Add(item);
				}
			}
			return equipmentEffects;
		}
		return new List<Descriptor>();
	}

	// Token: 0x04004611 RID: 17937
	private global::QualityLevel quality;

	// Token: 0x04004612 RID: 17938
	[MyCmpAdd]
	private EquippableWorkable equippableWorkable;

	// Token: 0x04004613 RID: 17939
	[MyCmpAdd]
	private EquippableFacade facade;

	// Token: 0x04004614 RID: 17940
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04004615 RID: 17941
	public DefHandle defHandle;

	// Token: 0x04004616 RID: 17942
	[Serialize]
	public bool isEquipped;

	// Token: 0x04004617 RID: 17943
	private bool destroyed;

	// Token: 0x04004618 RID: 17944
	[Serialize]
	public bool unequippable = true;

	// Token: 0x04004619 RID: 17945
	[Serialize]
	public bool hideInCodex;

	// Token: 0x0400461A RID: 17946
	private static readonly EventSystem.IntraObjectHandler<Equippable> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equippable>(delegate(Equippable component, object data)
	{
		component.destroyed = true;
	});
}
