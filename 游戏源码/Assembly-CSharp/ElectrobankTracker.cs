using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001277 RID: 4727
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElectrobankTracker")]
public class ElectrobankTracker : WorldResourceAmountTracker<ElectrobankTracker>, ISaveLoadable
{
	// Token: 0x060060ED RID: 24813 RVA: 0x000DF371 File Offset: 0x000DD571
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.itemTag = GameTags.ChargedPortableBattery;
	}

	// Token: 0x060060EE RID: 24814 RVA: 0x002B1334 File Offset: 0x002AF534
	protected override WorldResourceAmountTracker<ElectrobankTracker>.ItemData GetItemData(Pickupable item)
	{
		Electrobank component = item.GetComponent<Electrobank>();
		return new WorldResourceAmountTracker<ElectrobankTracker>.ItemData
		{
			ID = component.ID,
			amountValue = component.Charge * item.PrimaryElement.Units,
			units = item.PrimaryElement.Units
		};
	}
}
