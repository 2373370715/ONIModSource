using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF8 RID: 15096
	public class WorldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600E823 RID: 59427 RVA: 0x004C0344 File Offset: 0x004BE544
		public override string label
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.name, out entry))
				{
					return cachedWorldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600E824 RID: 59428 RVA: 0x004C037C File Offset: 0x004BE57C
		public override string tooltip
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.description, out entry))
				{
					return cachedWorldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600E825 RID: 59429 RVA: 0x004C03B4 File Offset: 0x004BE5B4
		public override Sprite icon
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedWorldMixingSetting.icon != null) ? ColonyDestinationAsteroidBeltData.GetUISprite(cachedWorldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite(cachedWorldMixingSetting.icon);
				}
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x0600E826 RID: 59430 RVA: 0x0013B4A1 File Offset: 0x001396A1
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x0600E827 RID: 59431 RVA: 0x0013B4B3 File Offset: 0x001396B3
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x0600E828 RID: 59432 RVA: 0x004C0418 File Offset: 0x004BE618
		public WorldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x0400E3F2 RID: 58354
		private const int COORDINATE_RANGE = 5;

		// Token: 0x0400E3F3 RID: 58355
		public const string DisabledLevelId = "Disabled";

		// Token: 0x0400E3F4 RID: 58356
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x0400E3F5 RID: 58357
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
