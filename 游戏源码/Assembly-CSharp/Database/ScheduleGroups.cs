using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02002162 RID: 8546
	public class ScheduleGroups : ResourceSet<ScheduleGroup>
	{
		// Token: 0x0600B5E1 RID: 46561 RVA: 0x00454ECC File Offset: 0x004530CC
		public ScheduleGroup Add(string id, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false)
		{
			ScheduleGroup scheduleGroup = new ScheduleGroup(id, this, defaultSegments, name, description, uiColor, notificationTooltip, allowedTypes, alarm);
			this.allGroups.Add(scheduleGroup);
			return scheduleGroup;
		}

		// Token: 0x0600B5E2 RID: 46562 RVA: 0x00454EFC File Offset: 0x004530FC
		public ScheduleGroups(ResourceSet parent) : base("ScheduleGroups", parent)
		{
			this.allGroups = new List<ScheduleGroup>();
			this.Hygene = this.Add("Hygene", 1, UI.SCHEDULEGROUPS.HYGENE.NAME, UI.SCHEDULEGROUPS.HYGENE.DESCRIPTION, Util.ColorFromHex("5A8DAF"), UI.SCHEDULEGROUPS.HYGENE.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Hygiene,
				Db.Get().ScheduleBlockTypes.Work
			}, false);
			this.Worktime = this.Add("Worktime", 18, UI.SCHEDULEGROUPS.WORKTIME.NAME, UI.SCHEDULEGROUPS.WORKTIME.DESCRIPTION, Util.ColorFromHex("FFA649"), UI.SCHEDULEGROUPS.WORKTIME.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Work
			}, true);
			this.Recreation = this.Add("Recreation", 2, UI.SCHEDULEGROUPS.RECREATION.NAME, UI.SCHEDULEGROUPS.RECREATION.DESCRIPTION, Util.ColorFromHex("70DFAD"), UI.SCHEDULEGROUPS.RECREATION.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Hygiene,
				Db.Get().ScheduleBlockTypes.Eat,
				Db.Get().ScheduleBlockTypes.Recreation,
				Db.Get().ScheduleBlockTypes.Work
			}, false);
			this.Sleep = this.Add("Sleep", 3, UI.SCHEDULEGROUPS.SLEEP.NAME, UI.SCHEDULEGROUPS.SLEEP.DESCRIPTION, Util.ColorFromHex("273469"), UI.SCHEDULEGROUPS.SLEEP.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>
			{
				Db.Get().ScheduleBlockTypes.Sleep
			}, false);
			int num = 0;
			foreach (ScheduleGroup scheduleGroup in this.allGroups)
			{
				num += scheduleGroup.defaultSegments;
			}
			global::Debug.Assert(num == 24, "Default schedule groups must add up to exactly 1 cycle!");
		}

		// Token: 0x0600B5E3 RID: 46563 RVA: 0x00455124 File Offset: 0x00453324
		public ScheduleGroup FindGroupForScheduleTypes(List<ScheduleBlockType> types)
		{
			foreach (ScheduleGroup scheduleGroup in this.allGroups)
			{
				if (Schedule.AreScheduleTypesIdentical(scheduleGroup.allowedTypes, types))
				{
					return scheduleGroup;
				}
			}
			return null;
		}

		// Token: 0x0400941B RID: 37915
		public List<ScheduleGroup> allGroups;

		// Token: 0x0400941C RID: 37916
		public ScheduleGroup Hygene;

		// Token: 0x0400941D RID: 37917
		public ScheduleGroup Worktime;

		// Token: 0x0400941E RID: 37918
		public ScheduleGroup Recreation;

		// Token: 0x0400941F RID: 37919
		public ScheduleGroup Sleep;
	}
}
