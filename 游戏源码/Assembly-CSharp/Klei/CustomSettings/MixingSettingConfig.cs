using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF7 RID: 15095
	public class MixingSettingConfig : ListSettingConfig
	{
		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x0600E81B RID: 59419 RVA: 0x0013B467 File Offset: 0x00139667
		// (set) Token: 0x0600E81C RID: 59420 RVA: 0x0013B46F File Offset: 0x0013966F
		public string worldgenPath { get; private set; }

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600E81D RID: 59421 RVA: 0x0013B478 File Offset: 0x00139678
		public virtual Sprite icon { get; }

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600E81E RID: 59422 RVA: 0x0013B480 File Offset: 0x00139680
		public virtual List<string> forbiddenClusterTags { get; }

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x0600E81F RID: 59423 RVA: 0x0013B488 File Offset: 0x00139688
		// (set) Token: 0x0600E820 RID: 59424 RVA: 0x0013B490 File Offset: 0x00139690
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x0600E821 RID: 59425 RVA: 0x0013B499 File Offset: 0x00139699
		public virtual bool isModded { get; }

		// Token: 0x0600E822 RID: 59426 RVA: 0x004C030C File Offset: 0x004BE50C
		protected MixingSettingConfig(string id, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, string worldgenPath, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = false, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, "", "", levels, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.worldgenPath = worldgenPath;
		}
	}
}
