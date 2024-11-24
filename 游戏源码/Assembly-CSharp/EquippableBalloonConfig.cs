using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

public class EquippableBalloonConfig : IEquipmentConfig
{
	public const string ID = "EquippableBalloon";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public EquipmentDef CreateEquipmentDef()
	{
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("EquippableBalloon", EQUIPMENT.TOYS.SLOT, SimHashes.Carbon, EQUIPMENT.TOYS.BALLOON_MASS, EQUIPMENT.VESTS.WARM_VEST_ICON0, null, null, 0, attributeModifiers, null, IsBody: false, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f);
		equipmentDef.OnEquipCallBack = OnEquipBalloon;
		equipmentDef.OnUnequipCallBack = OnUnequipBalloon;
		return equipmentDef;
	}

	private void OnEquipBalloon(Equippable eq)
	{
		if (eq.IsNullOrDestroyed() || eq.assignee.IsNullOrDestroyed())
		{
			return;
		}
		Ownables soleOwner = eq.assignee.GetSoleOwner();
		if (!soleOwner.IsNullOrDestroyed())
		{
			KMonoBehaviour kMonoBehaviour = (KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target;
			Effects component = kMonoBehaviour.GetComponent<Effects>();
			KSelectable component2 = kMonoBehaviour.GetComponent<KSelectable>();
			if (!component.IsNullOrDestroyed())
			{
				component.Add("HasBalloon", should_save: false);
				EquippableBalloon component3 = eq.GetComponent<EquippableBalloon>();
				component2.AddStatusItem(data: (EquippableBalloon.StatesInstance)component3.GetSMI(), status_item: Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon);
				SpawnFxInstanceFor(kMonoBehaviour);
				component3.ApplyBalloonOverrideToBalloonFx();
			}
		}
	}

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
				KMonoBehaviour kMonoBehaviour = (KMonoBehaviour)component.target;
				Effects component2 = kMonoBehaviour.GetComponent<Effects>();
				KSelectable component3 = kMonoBehaviour.GetComponent<KSelectable>();
				if (!component2.IsNullOrDestroyed())
				{
					component2.Remove("HasBalloon");
					component3.RemoveStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon);
					DestroyFxInstanceFor(kMonoBehaviour);
				}
			}
		}
		Util.KDestroyGameObject(eq.gameObject);
	}

	public void DoPostConfigure(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
		Equippable equippable = go.GetComponent<Equippable>();
		if (equippable.IsNullOrDestroyed())
		{
			equippable = go.AddComponent<Equippable>();
		}
		equippable.hideInCodex = true;
		equippable.unequippable = false;
		go.AddOrGet<EquippableBalloon>();
	}

	private void SpawnFxInstanceFor(KMonoBehaviour target)
	{
		new BalloonFX.Instance(target.GetComponent<KMonoBehaviour>()).StartSM();
	}

	private void DestroyFxInstanceFor(KMonoBehaviour target)
	{
		target.GetSMI<BalloonFX.Instance>().StopSM("Unequipped");
	}
}
