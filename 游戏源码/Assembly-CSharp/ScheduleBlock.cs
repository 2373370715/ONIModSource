using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02001841 RID: 6209
[Serializable]
public class ScheduleBlock
{
	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06008059 RID: 32857 RVA: 0x000F47EB File Offset: 0x000F29EB
	public List<ScheduleBlockType> allowed_types
	{
		get
		{
			Debug.Assert(!string.IsNullOrEmpty(this._groupId));
			return Db.Get().ScheduleGroups.Get(this._groupId).allowedTypes;
		}
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x0600805B RID: 32859 RVA: 0x000F4823 File Offset: 0x000F2A23
	// (set) Token: 0x0600805A RID: 32858 RVA: 0x000F481A File Offset: 0x000F2A1A
	public string GroupId
	{
		get
		{
			return this._groupId;
		}
		set
		{
			this._groupId = value;
		}
	}

	// Token: 0x0600805C RID: 32860 RVA: 0x000F482B File Offset: 0x000F2A2B
	public ScheduleBlock(string name, string groupId)
	{
		this.name = name;
		this._groupId = groupId;
	}

	// Token: 0x0600805D RID: 32861 RVA: 0x00334494 File Offset: 0x00332694
	public bool IsAllowed(ScheduleBlockType type)
	{
		if (this.allowed_types != null)
		{
			foreach (ScheduleBlockType scheduleBlockType in this.allowed_types)
			{
				if (type.IdHash == scheduleBlockType.IdHash)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04006150 RID: 24912
	[Serialize]
	public string name;

	// Token: 0x04006151 RID: 24913
	[Serialize]
	private string _groupId;
}
