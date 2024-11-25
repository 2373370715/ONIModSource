﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Database
{
		public class ScheduleGroups : ResourceSet<ScheduleGroup>
	{
				public ScheduleGroup Add(string id, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false)
		{
			ScheduleGroup scheduleGroup = new ScheduleGroup(id, this, defaultSegments, name, description, uiColor, notificationTooltip, allowedTypes, alarm);
			this.allGroups.Add(scheduleGroup);
			return scheduleGroup;
		}

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

				public List<ScheduleGroup> allGroups;

				public ScheduleGroup Hygene;

				public ScheduleGroup Worktime;

				public ScheduleGroup Recreation;

				public ScheduleGroup Sleep;
	}
}
