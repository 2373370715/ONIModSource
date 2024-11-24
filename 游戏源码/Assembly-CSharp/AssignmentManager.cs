using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C30 RID: 3120
[AddComponentMenu("KMonoBehaviour/scripts/AssignmentManager")]
public class AssignmentManager : KMonoBehaviour
{
	// Token: 0x06003BB8 RID: 15288 RVA: 0x000C684B File Offset: 0x000C4A4B
	public IEnumerator<Assignable> GetEnumerator()
	{
		return this.assignables.GetEnumerator();
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x000C685D File Offset: 0x000C4A5D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe<AssignmentManager>(586301400, AssignmentManager.MinionMigrationDelegate);
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x0022BC34 File Offset: 0x00229E34
	protected void MinionMigration(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject() == minionMigrationEventArgs.minionId.gameObject)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x000C687A File Offset: 0x000C4A7A
	public void Add(Assignable assignable)
	{
		this.assignables.Add(assignable);
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x000C6888 File Offset: 0x000C4A88
	public void Remove(Assignable assignable)
	{
		this.assignables.Remove(assignable);
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x000C6897 File Offset: 0x000C4A97
	public AssignmentGroup TryCreateAssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		if (this.assignment_groups.ContainsKey(id))
		{
			return this.assignment_groups[id];
		}
		return new AssignmentGroup(id, members, name);
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x000C68BC File Offset: 0x000C4ABC
	public void RemoveAssignmentGroup(string id)
	{
		if (!this.assignment_groups.ContainsKey(id))
		{
			global::Debug.LogError("Assignment group with id " + id + " doesn't exists");
			return;
		}
		this.assignment_groups.Remove(id);
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x000C68EF File Offset: 0x000C4AEF
	public void AddToAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].AddMember(member);
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x000C6914 File Offset: 0x000C4B14
	public void RemoveFromAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].RemoveMember(member);
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x0022BCE0 File Offset: 0x00229EE0
	public void RemoveFromAllGroups(IAssignableIdentity member)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee == member)
			{
				assignable.Unassign();
			}
		}
		foreach (KeyValuePair<string, AssignmentGroup> keyValuePair in this.assignment_groups)
		{
			if (keyValuePair.Value.HasMember(member))
			{
				keyValuePair.Value.RemoveMember(member);
			}
		}
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0022BD94 File Offset: 0x00229F94
	public void RemoveFromWorld(IAssignableIdentity minionIdentity, int world_id)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null && assignable.assignee.GetOwners().Count == 1)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee == minionIdentity && assignable.GetMyWorldId() == world_id)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x0022BE38 File Offset: 0x0022A038
	public List<Assignable> GetPreferredAssignables(Assignables owner, AssignableSlot slot)
	{
		List<Assignable> preferredAssignableResults = this.PreferredAssignableResults;
		List<Assignable> preferredAssignableResults2;
		lock (preferredAssignableResults)
		{
			this.PreferredAssignableResults.Clear();
			int num = int.MaxValue;
			foreach (Assignable assignable in this.assignables)
			{
				if (assignable.slot == slot && assignable.assignee != null && assignable.assignee.HasOwner(owner))
				{
					Room room = assignable.assignee as Room;
					if (room != null && room.roomType.priority_building_use)
					{
						this.PreferredAssignableResults.Clear();
						this.PreferredAssignableResults.Add(assignable);
						return this.PreferredAssignableResults;
					}
					int num2 = assignable.assignee.NumOwners();
					if (num2 == num)
					{
						this.PreferredAssignableResults.Add(assignable);
					}
					else if (num2 < num)
					{
						num = num2;
						this.PreferredAssignableResults.Clear();
						this.PreferredAssignableResults.Add(assignable);
					}
				}
			}
			preferredAssignableResults2 = this.PreferredAssignableResults;
		}
		return preferredAssignableResults2;
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x0022BF98 File Offset: 0x0022A198
	public bool IsPreferredAssignable(Assignables owner, Assignable candidate)
	{
		IAssignableIdentity assignee = candidate.assignee;
		if (assignee == null || !assignee.HasOwner(owner))
		{
			return false;
		}
		int num = assignee.NumOwners();
		Room room = assignee as Room;
		if (room != null && room.roomType.priority_building_use)
		{
			return true;
		}
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.slot == candidate.slot && assignable.assignee != assignee)
			{
				Room room2 = assignable.assignee as Room;
				if (room2 != null && room2.roomType.priority_building_use && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
				if (assignable.assignee.NumOwners() < num && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x040028F3 RID: 10483
	private List<Assignable> assignables = new List<Assignable>();

	// Token: 0x040028F4 RID: 10484
	public const string PUBLIC_GROUP_ID = "public";

	// Token: 0x040028F5 RID: 10485
	public Dictionary<string, AssignmentGroup> assignment_groups = new Dictionary<string, AssignmentGroup>
	{
		{
			"public",
			new AssignmentGroup("public", new IAssignableIdentity[0], UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.PUBLIC)
		}
	};

	// Token: 0x040028F6 RID: 10486
	private static readonly EventSystem.IntraObjectHandler<AssignmentManager> MinionMigrationDelegate = new EventSystem.IntraObjectHandler<AssignmentManager>(delegate(AssignmentManager component, object data)
	{
		component.MinionMigration(data);
	});

	// Token: 0x040028F7 RID: 10487
	private List<Assignable> PreferredAssignableResults = new List<Assignable>();
}
