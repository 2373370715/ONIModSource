using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Token: 0x02000A69 RID: 2665
public class AssignmentGroup : IAssignableIdentity
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x0600310C RID: 12556 RVA: 0x000BFDB6 File Offset: 0x000BDFB6
	// (set) Token: 0x0600310D RID: 12557 RVA: 0x000BFDBE File Offset: 0x000BDFBE
	public string id { get; private set; }

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x0600310E RID: 12558 RVA: 0x000BFDC7 File Offset: 0x000BDFC7
	// (set) Token: 0x0600310F RID: 12559 RVA: 0x000BFDCF File Offset: 0x000BDFCF
	public string name { get; private set; }

	// Token: 0x06003110 RID: 12560 RVA: 0x001FDEC8 File Offset: 0x001FC0C8
	public AssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		this.id = id;
		this.name = name;
		foreach (IAssignableIdentity item in members)
		{
			this.members.Add(item);
		}
		if (Game.Instance != null)
		{
			Game.Instance.assignmentManager.assignment_groups.Add(id, this);
			Game.Instance.Trigger(-1123234494, this);
		}
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x000BFDD8 File Offset: 0x000BDFD8
	public void AddMember(IAssignableIdentity member)
	{
		if (!this.members.Contains(member))
		{
			this.members.Add(member);
		}
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x000BFE04 File Offset: 0x000BE004
	public void RemoveMember(IAssignableIdentity member)
	{
		this.members.Remove(member);
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x000BFE23 File Offset: 0x000BE023
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x000BFE2B File Offset: 0x000BE02B
	public bool HasMember(IAssignableIdentity member)
	{
		return this.members.Contains(member);
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x000BFE39 File Offset: 0x000BE039
	public ReadOnlyCollection<IAssignableIdentity> GetMembers()
	{
		return this.members.AsReadOnly();
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x001FDF54 File Offset: 0x001FC154
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			this.current_owners.AddRange(assignableIdentity.GetOwners());
		}
		return this.current_owners;
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x001FDFC4 File Offset: 0x001FC1C4
	public Ownables GetSoleOwner()
	{
		if (this.members.Count == 1)
		{
			return this.members[0] as Ownables;
		}
		Debug.LogWarningFormat("GetSoleOwner called on AssignmentGroup with {0} members", new object[]
		{
			this.members.Count
		});
		return null;
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x001FE018 File Offset: 0x001FC218
	public bool HasOwner(Assignables owner)
	{
		using (List<IAssignableIdentity>.Enumerator enumerator = this.members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasOwner(owner))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x001FE074 File Offset: 0x001FC274
	public int NumOwners()
	{
		int num = 0;
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			num += assignableIdentity.NumOwners();
		}
		return num;
	}

	// Token: 0x0400211B RID: 8475
	private List<IAssignableIdentity> members = new List<IAssignableIdentity>();

	// Token: 0x0400211C RID: 8476
	public List<Ownables> current_owners = new List<Ownables>();
}
