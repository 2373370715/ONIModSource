using System.Collections.Generic;

namespace Klei.CustomSettings;

public class ToggleSettingConfig : SettingConfig
{
	public SettingLevel on_level { get; private set; }

	public SettingLevel off_level { get; private set; }

	public ToggleSettingConfig(string id, string label, string tooltip, SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "")
		: base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default)
	{
		this.off_level = off_level;
		this.on_level = on_level;
	}

	public void StompLevels(SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id)
	{
		this.off_level = off_level;
		this.on_level = on_level;
		base.default_level_id = default_level_id;
		base.nosweat_default_level_id = nosweat_default_level_id;
	}

	public override SettingLevel GetLevel(string level_id)
	{
		if (on_level.id == level_id)
		{
			return on_level;
		}
		if (off_level.id == level_id)
		{
			return off_level;
		}
		if (default_level_id == on_level.id)
		{
			Debug.LogWarning("Unable to find level for setting:" + base.id + "(" + level_id + ") Using default level.");
			return on_level;
		}
		if (default_level_id == off_level.id)
		{
			Debug.LogWarning("Unable to find level for setting:" + base.id + "(" + level_id + ") Using default level.");
			return off_level;
		}
		Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
		return null;
	}

	public override List<SettingLevel> GetLevels()
	{
		return new List<SettingLevel> { off_level, on_level };
	}

	public string ToggleSettingLevelID(string current_id)
	{
		if (on_level.id == current_id)
		{
			return off_level.id;
		}
		return on_level.id;
	}

	public bool IsOnLevel(string level_id)
	{
		return level_id == on_level.id;
	}
}
