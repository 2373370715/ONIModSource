using System;
using KSerialization;

// Token: 0x02000B91 RID: 2961
[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
	// Token: 0x17000294 RID: 660
	// (get) Token: 0x060038A8 RID: 14504 RVA: 0x000C4AC0 File Offset: 0x000C2CC0
	// (set) Token: 0x060038A9 RID: 14505 RVA: 0x000C4AD7 File Offset: 0x000C2CD7
	public ScheduleGroup scheduleGroup
	{
		get
		{
			return Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
		}
		set
		{
			this.scheduleGroupID = value.Id;
		}
	}

	// Token: 0x060038AA RID: 14506 RVA: 0x000C4AE5 File Offset: 0x000C2CE5
	public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
	{
		this.scheduleGroup = scheduleGroup;
		this.segments = scheduleGroup.defaultSegments;
	}

	// Token: 0x040026A4 RID: 9892
	[Serialize]
	private string scheduleGroupID;

	// Token: 0x040026A5 RID: 9893
	[Serialize]
	public int segments;
}
