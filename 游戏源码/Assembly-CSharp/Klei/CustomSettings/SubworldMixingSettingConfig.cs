using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF9 RID: 15097
	public class SubworldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x0600E829 RID: 59433 RVA: 0x004C04D0 File Offset: 0x004BE6D0
		public override string label
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.name, out entry))
				{
					return cachedSubworldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x0600E82A RID: 59434 RVA: 0x004C0508 File Offset: 0x004BE708
		public override string tooltip
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.description, out entry))
				{
					return cachedSubworldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600E82B RID: 59435 RVA: 0x004C0540 File Offset: 0x004BE740
		public override Sprite icon
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedSubworldMixingSetting.icon != null) ? Assets.GetSprite(cachedSubworldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x0600E82C RID: 59436 RVA: 0x0013B4C5 File Offset: 0x001396C5
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600E82D RID: 59437 RVA: 0x0013B4D7 File Offset: 0x001396D7
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x0600E82E RID: 59438 RVA: 0x004C0590 File Offset: 0x004BE790
		public SubworldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP_BASEGAME, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP_BASEGAME, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP_BASEGAME, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x0400E3F6 RID: 58358
		private const int COORDINATE_RANGE = 5;

		// Token: 0x0400E3F7 RID: 58359
		public const string DisabledLevelId = "Disabled";

		// Token: 0x0400E3F8 RID: 58360
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x0400E3F9 RID: 58361
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
