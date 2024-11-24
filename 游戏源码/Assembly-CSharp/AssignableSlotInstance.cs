using System;
using UnityEngine;

// Token: 0x02000C2C RID: 3116
public abstract class AssignableSlotInstance
{
	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06003B9E RID: 15262 RVA: 0x000C6737 File Offset: 0x000C4937
	// (set) Token: 0x06003B9F RID: 15263 RVA: 0x000C673F File Offset: 0x000C493F
	public Assignables assignables { get; private set; }

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06003BA0 RID: 15264 RVA: 0x000C6748 File Offset: 0x000C4948
	public GameObject gameObject
	{
		get
		{
			return this.assignables.gameObject;
		}
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x000C6755 File Offset: 0x000C4955
	public AssignableSlotInstance(Assignables assignables, AssignableSlot slot) : this(slot.Id, assignables, slot)
	{
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x000C6765 File Offset: 0x000C4965
	public AssignableSlotInstance(string id, Assignables assignables, AssignableSlot slot)
	{
		this.ID = id;
		this.slot = slot;
		this.assignables = assignables;
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x000C6782 File Offset: 0x000C4982
	public void Assign(Assignable assignable)
	{
		if (this.assignable == assignable)
		{
			return;
		}
		this.Unassign(false);
		this.assignable = assignable;
		this.assignables.Trigger(-1585839766, this);
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0022B910 File Offset: 0x00229B10
	public virtual void Unassign(bool trigger_event = true)
	{
		if (this.unassigning)
		{
			return;
		}
		if (this.IsAssigned())
		{
			this.unassigning = true;
			this.assignable.Unassign();
			if (trigger_event)
			{
				this.assignables.Trigger(-1585839766, this);
			}
			this.assignable = null;
			this.unassigning = false;
		}
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x000C67B2 File Offset: 0x000C49B2
	public bool IsAssigned()
	{
		return this.assignable != null;
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x000C67C0 File Offset: 0x000C49C0
	public bool IsUnassigning()
	{
		return this.unassigning;
	}

	// Token: 0x040028EA RID: 10474
	public string ID;

	// Token: 0x040028EB RID: 10475
	public AssignableSlot slot;

	// Token: 0x040028EC RID: 10476
	public Assignable assignable;

	// Token: 0x040028EE RID: 10478
	private bool unassigning;
}
