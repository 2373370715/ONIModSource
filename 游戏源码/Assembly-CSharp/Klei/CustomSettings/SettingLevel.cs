using System;

namespace Klei.CustomSettings
{
	// Token: 0x02003AEF RID: 15087
	public class SettingLevel
	{
		// Token: 0x0600E7DA RID: 59354 RVA: 0x0013B19F File Offset: 0x0013939F
		public SettingLevel(string id, string label, string tooltip, long coordinate_value = 0L, object userdata = null)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.userdata = userdata;
			this.coordinate_value = coordinate_value;
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x0600E7DB RID: 59355 RVA: 0x0013B1CC File Offset: 0x001393CC
		// (set) Token: 0x0600E7DC RID: 59356 RVA: 0x0013B1D4 File Offset: 0x001393D4
		public string id { get; private set; }

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x0600E7DD RID: 59357 RVA: 0x0013B1DD File Offset: 0x001393DD
		// (set) Token: 0x0600E7DE RID: 59358 RVA: 0x0013B1E5 File Offset: 0x001393E5
		public string tooltip { get; private set; }

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x0600E7DF RID: 59359 RVA: 0x0013B1EE File Offset: 0x001393EE
		// (set) Token: 0x0600E7E0 RID: 59360 RVA: 0x0013B1F6 File Offset: 0x001393F6
		public string label { get; private set; }

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600E7E1 RID: 59361 RVA: 0x0013B1FF File Offset: 0x001393FF
		// (set) Token: 0x0600E7E2 RID: 59362 RVA: 0x0013B207 File Offset: 0x00139407
		public object userdata { get; private set; }

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x0600E7E3 RID: 59363 RVA: 0x0013B210 File Offset: 0x00139410
		// (set) Token: 0x0600E7E4 RID: 59364 RVA: 0x0013B218 File Offset: 0x00139418
		public long coordinate_value { get; private set; }
	}
}
