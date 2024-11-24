using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000C2A RID: 3114
public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06003B80 RID: 15232 RVA: 0x000C662D File Offset: 0x000C482D
	public AssignableSlot slot
	{
		get
		{
			if (this._slot == null)
			{
				this._slot = Db.Get().AssignableSlots.Get(this.slotID);
			}
			return this._slot;
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06003B81 RID: 15233 RVA: 0x000C6658 File Offset: 0x000C4858
	public bool CanBeAssigned
	{
		get
		{
			return this.canBeAssigned;
		}
	}

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06003B82 RID: 15234 RVA: 0x0022B1D0 File Offset: 0x002293D0
	// (remove) Token: 0x06003B83 RID: 15235 RVA: 0x0022B208 File Offset: 0x00229408
	public event Action<IAssignableIdentity> OnAssign;

	// Token: 0x06003B84 RID: 15236 RVA: 0x000A5E40 File Offset: 0x000A4040
	[OnDeserialized]
	internal void OnDeserialized()
	{
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0022B240 File Offset: 0x00229440
	private void RestoreAssignee()
	{
		IAssignableIdentity savedAssignee = this.GetSavedAssignee();
		if (savedAssignee != null)
		{
			AssignableSlotInstance savedSlotInstance = this.GetSavedSlotInstance(savedAssignee);
			this.Assign(savedAssignee, savedSlotInstance);
		}
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0022B268 File Offset: 0x00229468
	private AssignableSlotInstance GetSavedSlotInstance(IAssignableIdentity savedAsignee)
	{
		if ((savedAsignee != null && savedAsignee is MinionIdentity) || savedAsignee is StoredMinionIdentity || savedAsignee is MinionAssignablesProxy)
		{
			Ownables soleOwner = savedAsignee.GetSoleOwner();
			if (soleOwner != null)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				if (slots != null)
				{
					AssignableSlotInstance assignableSlotInstance = slots.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance != null)
					{
						return assignableSlotInstance;
					}
				}
			}
			Equipment component = soleOwner.GetComponent<Equipment>();
			if (component != null)
			{
				AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
				if (slots2 != null)
				{
					AssignableSlotInstance assignableSlotInstance2 = slots2.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance2 != null)
					{
						return assignableSlotInstance2;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x0022B308 File Offset: 0x00229508
	private IAssignableIdentity GetSavedAssignee()
	{
		if (this.assignee_identityRef.Get() != null)
		{
			return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
		}
		if (!string.IsNullOrEmpty(this.assignee_groupID))
		{
			return Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID];
		}
		return null;
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x0022B364 File Offset: 0x00229564
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreAssignee();
		Components.AssignableItems.Add(this);
		Game.Instance.assignmentManager.Add(this);
		if (this.assignee == null && this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			GameObject targetGameObject = proxy.GetTargetGameObject();
			return targetGameObject.GetComponent<KMonoBehaviour>().GetMyWorldId() == this.GetMyWorldId() || targetGameObject.IsMyParentWorld(base.gameObject);
		});
		this.autoassignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			Operational component = base.GetComponent<Operational>();
			return !(component != null) || component.IsOperational;
		});
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x000C6660 File Offset: 0x000C4860
	protected override void OnCleanUp()
	{
		this.Unassign();
		Components.AssignableItems.Remove(this);
		Game.Instance.assignmentManager.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x0022B3F8 File Offset: 0x002295F8
	public bool CanAutoAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		if (!this.CanAssignTo(minionAssignablesProxy))
		{
			return false;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.autoassignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x0022B470 File Offset: 0x00229670
	public bool CanAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.assignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x000C6689 File Offset: 0x000C4889
	public bool IsAssigned()
	{
		return this.assignee != null;
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x0022B4DC File Offset: 0x002296DC
	public bool IsAssignedTo(IAssignableIdentity identity)
	{
		global::Debug.Assert(identity != null, "IsAssignedTo identity is null");
		Ownables soleOwner = identity.GetSoleOwner();
		global::Debug.Assert(soleOwner != null, "IsAssignedTo identity sole owner is null");
		if (this.assignee != null)
		{
			foreach (Ownables ownables in this.assignee.GetOwners())
			{
				global::Debug.Assert(ownables, "Assignable owners list contained null");
				if (ownables.gameObject == soleOwner.gameObject)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x000C6694 File Offset: 0x000C4894
	public virtual void Assign(IAssignableIdentity new_assignee)
	{
		this.Assign(new_assignee, null);
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x0022B584 File Offset: 0x00229784
	public virtual void Assign(IAssignableIdentity new_assignee, AssignableSlotInstance specificSlotInstance)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee is KMonoBehaviour)
		{
			if (!this.CanAssignTo(new_assignee))
			{
				return;
			}
			this.assignee_identityRef.Set((KMonoBehaviour)new_assignee);
			this.assignee_groupID = "";
		}
		else if (new_assignee is AssignmentGroup)
		{
			this.assignee_identityRef.Set(null);
			this.assignee_groupID = ((AssignmentGroup)new_assignee).id;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Assigned, false);
		this.assignee = new_assignee;
		this.assignee_slotInstanceID = null;
		if (this.slot != null && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			if (specificSlotInstance == null)
			{
				Ownables soleOwner = new_assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					AssignableSlotInstance slot = soleOwner.GetSlot(this.slot);
					if (slot != null)
					{
						this.assignee_slotInstanceID = slot.ID;
						slot.Assign(this);
					}
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance slot2 = component.GetSlot(this.slot);
					if (slot2 != null)
					{
						this.assignee_slotInstanceID = slot2.ID;
						slot2.Assign(this);
					}
				}
			}
			else
			{
				this.assignee_slotInstanceID = specificSlotInstance.ID;
				specificSlotInstance.Assign(this);
			}
		}
		if (this.OnAssign != null)
		{
			this.OnAssign(new_assignee);
		}
		base.Trigger(684616645, new_assignee);
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0022B6D0 File Offset: 0x002298D0
	public virtual void Unassign()
	{
		if (this.assignee == null)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
		if (this.slot != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				AssignableSlotInstance assignableSlotInstance = (slots == null) ? null : slots.FindFirst((AssignableSlotInstance s) => s.assignable == this);
				if (assignableSlotInstance != null)
				{
					assignableSlotInstance.Unassign(true);
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
					assignableSlotInstance = ((slots2 == null) ? null : slots2.FindFirst((AssignableSlotInstance s) => s.assignable == this));
					if (assignableSlotInstance != null)
					{
						assignableSlotInstance.Unassign(true);
					}
				}
			}
		}
		this.assignee = null;
		if (this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignee_slotInstanceID = null;
		this.assignee_identityRef.Set(null);
		this.assignee_groupID = "";
		if (this.OnAssign != null)
		{
			this.OnAssign(null);
		}
		base.Trigger(684616645, null);
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x000C669E File Offset: 0x000C489E
	public void SetCanBeAssigned(bool state)
	{
		this.canBeAssigned = state;
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x000C66A7 File Offset: 0x000C48A7
	public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.assignmentPreconditions.Add(precondition);
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x000C66B5 File Offset: 0x000C48B5
	public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.autoassignmentPreconditions.Add(precondition);
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x0022B7F4 File Offset: 0x002299F4
	public int GetNavigationCost(Navigator navigator)
	{
		int num = -1;
		int cell = Grid.PosToCell(this);
		IApproachable component = base.GetComponent<IApproachable>();
		CellOffset[] array = (component != null) ? component.GetOffsets() : new CellOffset[1];
		DebugUtil.DevAssert(navigator != null, "Navigator is mysteriously null", null);
		if (navigator == null)
		{
			return -1;
		}
		foreach (CellOffset offset in array)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			int navigationCost = navigator.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x040028DD RID: 10461
	public string slotID;

	// Token: 0x040028DE RID: 10462
	private AssignableSlot _slot;

	// Token: 0x040028DF RID: 10463
	public IAssignableIdentity assignee;

	// Token: 0x040028E0 RID: 10464
	[Serialize]
	protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();

	// Token: 0x040028E1 RID: 10465
	[Serialize]
	protected string assignee_slotInstanceID;

	// Token: 0x040028E2 RID: 10466
	[Serialize]
	private string assignee_groupID = "";

	// Token: 0x040028E3 RID: 10467
	public AssignableSlot[] subSlots;

	// Token: 0x040028E4 RID: 10468
	public bool canBePublic;

	// Token: 0x040028E5 RID: 10469
	[Serialize]
	private bool canBeAssigned = true;

	// Token: 0x040028E6 RID: 10470
	private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

	// Token: 0x040028E7 RID: 10471
	private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
}
