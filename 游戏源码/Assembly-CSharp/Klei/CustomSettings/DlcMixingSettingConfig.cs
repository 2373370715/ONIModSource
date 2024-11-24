using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF6 RID: 15094
	public class DlcMixingSettingConfig : ToggleSettingConfig
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600E818 RID: 59416 RVA: 0x0013B456 File Offset: 0x00139656
		// (set) Token: 0x0600E819 RID: 59417 RVA: 0x0013B45E File Offset: 0x0013965E
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x0600E81A RID: 59418 RVA: 0x004C0284 File Offset: 0x004BE484
		public DlcMixingSettingConfig(string id, string label, string tooltip, long coordinate_range = 5L, bool triggers_custom_game = false, string[] required_content = null, string dlcIdFrom = null, string missing_content_default = "") : base(id, label, tooltip, null, null, null, "Disabled", coordinate_range, false, triggers_custom_game, required_content, missing_content_default)
		{
			this.dlcIdFrom = dlcIdFrom;
			SettingLevel off_level = new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null);
			SettingLevel on_level = new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.TOOLTIP, 1L, null);
			base.StompLevels(off_level, on_level, "Disabled", "Disabled");
		}

		// Token: 0x0400E3E9 RID: 58345
		private const int COORDINATE_RANGE = 5;

		// Token: 0x0400E3EA RID: 58346
		public const string DisabledLevelId = "Disabled";

		// Token: 0x0400E3EB RID: 58347
		public const string EnabledLevelId = "Enabled";
	}
}
