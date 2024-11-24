using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class EquippableBalloonConfig : IEquipmentConfig
{
	// Token: 0x06000316 RID: 790 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0014C2F0 File Offset: 0x0014A4F0
	public EquipmentDef CreateEquipmentDef()
	{
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("EquippableBalloon", EQUIPMENT.TOYS.SLOT, SimHashes.Carbon, EQUIPMENT.TOYS.BALLOON_MASS, EQUIPMENT.VESTS.WARM_VEST_ICON0, null, null, 0, attributeModifiers, null, false, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		equipmentDef.OnEquipCallBack = new Action<Equippable>(this.OnEquipBalloon);
		equipmentDef.OnUnequipCallBack = new Action<Equippable>(this.OnUnequipBalloon);
		return equipmentDef;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0014C358 File Offset: 0x0014A558
	private void OnEquipBalloon(Equippable eq)
	{
		if (!eq.IsNullOrDestroyed() && !eq.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target;
			Effects component = kmonoBehaviour.GetComponent<Effects>();
			KSelectable component2 = kmonoBehaviour.GetComponent<KSelectable>();
			if (!component.IsNullOrDestroyed())
			{
				component.Add("HasBalloon", false);
				EquippableBalloon component3 = eq.GetComponent<EquippableBalloon>();
				EquippableBalloon.StatesInstance data = (EquippableBalloon.StatesInstance)component3.GetSMI();
				component2.AddStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon, data);
				this.SpawnFxInstanceFor(kmonoBehaviour);
				component3.ApplyBalloonOverrideToBalloonFx();
			}
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0014C400 File Offset: 0x0014A600
	private void OnUnequipBalloon(Equippable eq)
	{
		if (!eq.IsNullOrDestroyed() && !eq.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			MinionAssignablesProxy component = soleOwner.GetComponent<MinionAssignablesProxy>();
			if (!component.target.IsNullOrDestroyed())
			{
				KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)component.target;
				Effects component2 = kmonoBehaviour.GetComponent<Effects>();
				KSelectable component3 = kmonoBehaviour.GetComponent<KSelectable>();
				if (!component2.IsNullOrDestroyed())
				{
					component2.Remove("HasBalloon");
					component3.RemoveStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon, false);
					this.DestroyFxInstanceFor(kmonoBehaviour);
				}
			}
		}
		Util.KDestroyGameObject(eq.gameObject);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0014C4A8 File Offset: 0x0014A6A8
	public void DoPostConfigure(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
		Equippable equippable = go.GetComponent<Equippable>();
		if (equippable.IsNullOrDestroyed())
		{
			equippable = go.AddComponent<Equippable>();
		}
		equippable.hideInCodex = true;
		equippable.unequippable = false;
		go.AddOrGet<EquippableBalloon>();
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000A6F74 File Offset: 0x000A5174
	private void SpawnFxInstanceFor(KMonoBehaviour target)
	{
		new BalloonFX.Instance(target.GetComponent<KMonoBehaviour>()).StartSM();
	}

	// Token: 0x0600031C RID: 796 RVA: 0x000A6F86 File Offset: 0x000A5186
	private void DestroyFxInstanceFor(KMonoBehaviour target)
	{
		target.GetSMI<BalloonFX.Instance>().StopSM("Unequipped");
	}

	// Token: 0x040001E5 RID: 485
	public const string ID = "EquippableBalloon";
}
