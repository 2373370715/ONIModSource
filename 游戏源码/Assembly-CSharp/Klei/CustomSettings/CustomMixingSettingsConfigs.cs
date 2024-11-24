using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x02003AFA RID: 15098
	public static class CustomMixingSettingsConfigs
	{
		// Token: 0x0400E3FA RID: 58362
		public static SettingConfig DLC2Mixing = new DlcMixingSettingConfig("DLC2_ID", UI.DLC2.NAME, UI.DLC2.MIXING_TOOLTIP, 5L, false, DlcManager.DLC2, "DLC2_ID", "");

		// Token: 0x0400E3FB RID: 58363
		public static SettingConfig DLC3Mixing = new DlcMixingSettingConfig("DLC3_ID", UI.DLC3.NAME, UI.DLC3.MIXING_TOOLTIP, 5L, false, DlcManager.DLC3, "DLC3_ID", "");

		// Token: 0x0400E3FC RID: 58364
		public static SettingConfig CeresAsteroidMixing = new WorldMixingSettingConfig("CeresAsteroidMixing", "dlc2::worldMixing/CeresMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x0400E3FD RID: 58365
		public static SettingConfig IceCavesMixing = new SubworldMixingSettingConfig("IceCavesMixing", "dlc2::subworldMixing/IceCavesMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x0400E3FE RID: 58366
		public static SettingConfig CarrotQuarryMixing = new SubworldMixingSettingConfig("CarrotQuarryMixing", "dlc2::subworldMixing/CarrotQuarryMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x0400E3FF RID: 58367
		public static SettingConfig SugarWoodsMixing = new SubworldMixingSettingConfig("SugarWoodsMixing", "dlc2::subworldMixing/SugarWoodsMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);
	}
}
