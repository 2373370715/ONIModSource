using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF5 RID: 15093
	public class SeedSettingConfig : SettingConfig
	{
		// Token: 0x0600E815 RID: 59413 RVA: 0x004C0254 File Offset: 0x004BE454
		public SeedSettingConfig(string id, string label, string tooltip, bool debug_only = false, bool triggers_custom_game = true) : base(id, label, tooltip, "", "", -1L, debug_only, triggers_custom_game, null, "", false)
		{
		}

		// Token: 0x0600E816 RID: 59414 RVA: 0x0013B442 File Offset: 0x00139642
		public override SettingLevel GetLevel(string level_id)
		{
			return new SettingLevel(level_id, level_id, level_id, 0L, null);
		}

		// Token: 0x0600E817 RID: 59415 RVA: 0x0013B44F File Offset: 0x0013964F
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>();
		}
	}
}
