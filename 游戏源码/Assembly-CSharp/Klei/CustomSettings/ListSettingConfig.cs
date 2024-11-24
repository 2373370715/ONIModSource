using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF1 RID: 15089
	public class ListSettingConfig : SettingConfig
	{
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600E7FE RID: 59390 RVA: 0x0013B34D File Offset: 0x0013954D
		// (set) Token: 0x0600E7FF RID: 59391 RVA: 0x0013B355 File Offset: 0x00139555
		public List<SettingLevel> levels { get; private set; }

		// Token: 0x0600E800 RID: 59392 RVA: 0x004BFF38 File Offset: 0x004BE138
		public ListSettingConfig(string id, string label, string tooltip, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.levels = levels;
		}

		// Token: 0x0600E801 RID: 59393 RVA: 0x0013B35E File Offset: 0x0013955E
		public void StompLevels(List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id)
		{
			this.levels = levels;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x0600E802 RID: 59394 RVA: 0x004BFF68 File Offset: 0x004BE168
		public override SettingLevel GetLevel(string level_id)
		{
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == level_id)
				{
					return this.levels[i];
				}
			}
			for (int j = 0; j < this.levels.Count; j++)
			{
				if (this.levels[j].id == this.default_level_id)
				{
					return this.levels[j];
				}
			}
			global::Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x0600E803 RID: 59395 RVA: 0x0013B375 File Offset: 0x00139575
		public override List<SettingLevel> GetLevels()
		{
			return this.levels;
		}

		// Token: 0x0600E804 RID: 59396 RVA: 0x004C0010 File Offset: 0x004BE210
		public string CycleSettingLevelID(string current_id, int direction)
		{
			string result = "";
			if (current_id == "")
			{
				current_id = this.levels[0].id;
			}
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == current_id)
				{
					int index = Mathf.Clamp(i + direction, 0, this.levels.Count - 1);
					result = this.levels[index].id;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600E805 RID: 59397 RVA: 0x004C00A0 File Offset: 0x004BE2A0
		public bool IsFirstLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == 0;
		}

		// Token: 0x0600E806 RID: 59398 RVA: 0x004C00D4 File Offset: 0x004BE2D4
		public bool IsLastLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == this.levels.Count - 1;
		}
	}
}
