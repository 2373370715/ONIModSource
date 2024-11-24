using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000C34 RID: 3124
[AddComponentMenu("KMonoBehaviour/scripts/AtmoSuit")]
public class AtmoSuit : KMonoBehaviour
{
	// Token: 0x06003BE8 RID: 15336 RVA: 0x000C6AFD File Offset: 0x000C4CFD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AtmoSuit>(-1697596308, AtmoSuit.OnStorageChangedDelegate);
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0022C53C File Offset: 0x0022A73C
	private void RefreshStatusEffects(object data)
	{
		if (this == null)
		{
			return;
		}
		Equippable component = base.GetComponent<Equippable>();
		bool flag = base.GetComponent<Storage>().Has(GameTags.AnyWater);
		if (component.assignee != null && flag)
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					AssignableSlotInstance slot = ((KMonoBehaviour)component.assignee).GetComponent<Equipment>().GetSlot(component.slot);
					Effects component2 = targetGameObject.GetComponent<Effects>();
					if (component2 != null && !component2.HasEffect("SoiledSuit") && !slot.IsUnassigning())
					{
						component2.Add("SoiledSuit", true);
					}
				}
			}
		}
	}

	// Token: 0x04002900 RID: 10496
	private static readonly EventSystem.IntraObjectHandler<AtmoSuit> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<AtmoSuit>(delegate(AtmoSuit component, object data)
	{
		component.RefreshStatusEffects(data);
	});
}
