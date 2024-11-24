using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02003AF0 RID: 15088
	public abstract class SettingConfig
	{
		// Token: 0x0600E7E5 RID: 59365 RVA: 0x004BFED0 File Offset: 0x004BE0D0
		public SettingConfig(string id, string label, string tooltip, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
			this.coordinate_range = coordinate_range;
			this.debug_only = debug_only;
			this.triggers_custom_game = triggers_custom_game;
			this.required_content = required_content;
			this.missing_content_default = missing_content_default;
			this.hide_in_ui = hide_in_ui;
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x0600E7E6 RID: 59366 RVA: 0x0013B221 File Offset: 0x00139421
		// (set) Token: 0x0600E7E7 RID: 59367 RVA: 0x0013B229 File Offset: 0x00139429
		public string id { get; private set; }

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x0600E7E8 RID: 59368 RVA: 0x0013B232 File Offset: 0x00139432
		// (set) Token: 0x0600E7E9 RID: 59369 RVA: 0x0013B23A File Offset: 0x0013943A
		public virtual string label { get; private set; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x0600E7EA RID: 59370 RVA: 0x0013B243 File Offset: 0x00139443
		// (set) Token: 0x0600E7EB RID: 59371 RVA: 0x0013B24B File Offset: 0x0013944B
		public virtual string tooltip { get; private set; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x0600E7EC RID: 59372 RVA: 0x0013B254 File Offset: 0x00139454
		// (set) Token: 0x0600E7ED RID: 59373 RVA: 0x0013B25C File Offset: 0x0013945C
		public long coordinate_range { get; protected set; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x0600E7EE RID: 59374 RVA: 0x0013B265 File Offset: 0x00139465
		// (set) Token: 0x0600E7EF RID: 59375 RVA: 0x0013B26D File Offset: 0x0013946D
		public string[] required_content { get; private set; }

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600E7F0 RID: 59376 RVA: 0x0013B276 File Offset: 0x00139476
		// (set) Token: 0x0600E7F1 RID: 59377 RVA: 0x0013B27E File Offset: 0x0013947E
		public string missing_content_default { get; private set; }

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x0600E7F2 RID: 59378 RVA: 0x0013B287 File Offset: 0x00139487
		// (set) Token: 0x0600E7F3 RID: 59379 RVA: 0x0013B28F File Offset: 0x0013948F
		public bool triggers_custom_game { get; protected set; }

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x0600E7F4 RID: 59380 RVA: 0x0013B298 File Offset: 0x00139498
		// (set) Token: 0x0600E7F5 RID: 59381 RVA: 0x0013B2A0 File Offset: 0x001394A0
		public bool debug_only { get; protected set; }

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x0600E7F6 RID: 59382 RVA: 0x0013B2A9 File Offset: 0x001394A9
		// (set) Token: 0x0600E7F7 RID: 59383 RVA: 0x0013B2B1 File Offset: 0x001394B1
		public bool hide_in_ui { get; protected set; }

		// Token: 0x0600E7F8 RID: 59384
		public abstract SettingLevel GetLevel(string level_id);

		// Token: 0x0600E7F9 RID: 59385
		public abstract List<SettingLevel> GetLevels();

		// Token: 0x0600E7FA RID: 59386 RVA: 0x0013B2BA File Offset: 0x001394BA
		public bool IsDefaultLevel(string level_id)
		{
			return level_id == this.default_level_id;
		}

		// Token: 0x0600E7FB RID: 59387 RVA: 0x0013B2C8 File Offset: 0x001394C8
		public bool ShowInUI()
		{
			return !this.deprecated && !this.hide_in_ui && (!this.debug_only || DebugHandler.enabled) && DlcManager.IsAllContentSubscribed(this.required_content);
		}

		// Token: 0x0600E7FC RID: 59388 RVA: 0x0013B2FB File Offset: 0x001394FB
		public string GetDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.default_level_id;
		}

		// Token: 0x0600E7FD RID: 59389 RVA: 0x0013B324 File Offset: 0x00139524
		public string GetNoSweatDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.nosweat_default_level_id;
		}

		// Token: 0x0400E3DB RID: 58331
		protected string default_level_id;

		// Token: 0x0400E3DC RID: 58332
		protected string nosweat_default_level_id;

		// Token: 0x0400E3E3 RID: 58339
		public bool deprecated;
	}
}
