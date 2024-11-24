using STRINGS;

namespace Klei.CustomSettings;

public class DlcMixingSettingConfig : ToggleSettingConfig
{
	private const int COORDINATE_RANGE = 5;

	public const string DisabledLevelId = "Disabled";

	public const string EnabledLevelId = "Enabled";

	public virtual string dlcIdFrom { get; protected set; }

	public DlcMixingSettingConfig(string id, string label, string tooltip, long coordinate_range = 5L, bool triggers_custom_game = false, string[] required_content = null, string dlcIdFrom = null, string missing_content_default = "")
		: base(id, label, tooltip, null, null, null, "Disabled", coordinate_range, debug_only: false, triggers_custom_game, required_content, missing_content_default)
	{
		this.dlcIdFrom = dlcIdFrom;
		SettingLevel settingLevel = new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.TOOLTIP, 0L);
		SettingLevel settingLevel2 = new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.TOOLTIP, 1L);
		StompLevels(settingLevel, settingLevel2, "Disabled", "Disabled");
	}
}
