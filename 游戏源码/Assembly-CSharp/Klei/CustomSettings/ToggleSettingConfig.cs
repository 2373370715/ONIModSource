using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF4 RID: 15092
	public class ToggleSettingConfig : SettingConfig
	{
		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x0600E80B RID: 59403 RVA: 0x0013B3A3 File Offset: 0x001395A3
		// (set) Token: 0x0600E80C RID: 59404 RVA: 0x0013B3AB File Offset: 0x001395AB
		public SettingLevel on_level { get; private set; }

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x0600E80D RID: 59405 RVA: 0x0013B3B4 File Offset: 0x001395B4
		// (set) Token: 0x0600E80E RID: 59406 RVA: 0x0013B3BC File Offset: 0x001395BC
		public SettingLevel off_level { get; private set; }

		// Token: 0x0600E80F RID: 59407 RVA: 0x004C0114 File Offset: 0x004BE314
		public ToggleSettingConfig(string id, string label, string tooltip, SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "") : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, false)
		{
			this.off_level = off_level;
			this.on_level = on_level;
		}

		// Token: 0x0600E810 RID: 59408 RVA: 0x0013B3C5 File Offset: 0x001395C5
		public void StompLevels(SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id)
		{
			this.off_level = off_level;
			this.on_level = on_level;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x0600E811 RID: 59409 RVA: 0x004C014C File Offset: 0x004BE34C
		public override SettingLevel GetLevel(string level_id)
		{
			if (this.on_level.id == level_id)
			{
				return this.on_level;
			}
			if (this.off_level.id == level_id)
			{
				return this.off_level;
			}
			if (this.default_level_id == this.on_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.on_level;
			}
			if (this.default_level_id == this.off_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.off_level;
			}
			Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x0600E812 RID: 59410 RVA: 0x0013B3E4 File Offset: 0x001395E4
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>
			{
				this.off_level,
				this.on_level
			};
		}

		// Token: 0x0600E813 RID: 59411 RVA: 0x0013B403 File Offset: 0x00139603
		public string ToggleSettingLevelID(string current_id)
		{
			if (this.on_level.id == current_id)
			{
				return this.off_level.id;
			}
			return this.on_level.id;
		}

		// Token: 0x0600E814 RID: 59412 RVA: 0x0013B42F File Offset: 0x0013962F
		public bool IsOnLevel(string level_id)
		{
			return level_id == this.on_level.id;
		}
	}
}
