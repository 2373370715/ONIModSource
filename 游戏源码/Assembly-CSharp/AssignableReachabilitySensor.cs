using System;

// Token: 0x0200081E RID: 2078
public class AssignableReachabilitySensor : Sensor
{
	// Token: 0x0600253B RID: 9531 RVA: 0x001CB8F0 File Offset: 0x001C9AF0
	public AssignableReachabilitySensor(Sensors sensors) : base(sensors)
	{
		MinionAssignablesProxy minionAssignablesProxy = base.gameObject.GetComponent<MinionIdentity>().assignableProxy.Get();
		minionAssignablesProxy.ConfigureAssignableSlots();
		Assignables[] components = minionAssignablesProxy.GetComponents<Assignables>();
		if (components.Length == 0)
		{
			Debug.LogError(base.gameObject.GetProperName() + ": No 'Assignables' components found for AssignableReachabilitySensor");
		}
		int num = 0;
		foreach (Assignables assignables in components)
		{
			num += assignables.Slots.Count;
		}
		this.slots = new AssignableReachabilitySensor.SlotEntry[num];
		int num2 = 0;
		foreach (Assignables assignables2 in components)
		{
			for (int k = 0; k < assignables2.Slots.Count; k++)
			{
				this.slots[num2++].slot = assignables2.Slots[k];
			}
		}
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x001CB9D8 File Offset: 0x001C9BD8
	public bool IsReachable(AssignableSlot slot)
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			if (this.slots[i].slot.slot == slot)
			{
				return this.slots[i].isReachable;
			}
		}
		Debug.LogError("Could not find slot: " + ((slot != null) ? slot.ToString() : null));
		return false;
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x001CBA40 File Offset: 0x001C9C40
	public override void Update()
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableReachabilitySensor.SlotEntry slotEntry = this.slots[i];
			AssignableSlotInstance slot = slotEntry.slot;
			if (slot.IsAssigned())
			{
				bool flag = slot.assignable.GetNavigationCost(this.navigator) != -1;
				Operational component = slot.assignable.GetComponent<Operational>();
				if (component != null)
				{
					flag = (flag && component.IsOperational);
				}
				if (flag != slotEntry.isReachable)
				{
					slotEntry.isReachable = flag;
					this.slots[i] = slotEntry;
					base.Trigger(334784980, slotEntry);
				}
			}
			else if (slotEntry.isReachable)
			{
				slotEntry.isReachable = false;
				this.slots[i] = slotEntry;
				base.Trigger(334784980, slotEntry);
			}
		}
	}

	// Token: 0x04001923 RID: 6435
	private AssignableReachabilitySensor.SlotEntry[] slots;

	// Token: 0x04001924 RID: 6436
	private Navigator navigator;

	// Token: 0x0200081F RID: 2079
	private struct SlotEntry
	{
		// Token: 0x04001925 RID: 6437
		public AssignableSlotInstance slot;

		// Token: 0x04001926 RID: 6438
		public bool isReachable;
	}
}
