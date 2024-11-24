using System;
using STRINGS;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02002215 RID: 8725
	public class OVERLAY
	{
		// Token: 0x02002216 RID: 8726
		public class TEMPERATURE_LEGEND
		{
			// Token: 0x04009798 RID: 38808
			public static readonly LegendEntry MAXHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04009799 RID: 38809
			public static readonly LegendEntry EXTREMEHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x0400979A RID: 38810
			public static readonly LegendEntry VERYHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0f, 0f), null, null, true);

			// Token: 0x0400979B RID: 38811
			public static readonly LegendEntry HOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 1f, 0f), null, null, true);

			// Token: 0x0400979C RID: 38812
			public static readonly LegendEntry TEMPERATE = new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x0400979D RID: 38813
			public static readonly LegendEntry COLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 1f), null, null, true);

			// Token: 0x0400979E RID: 38814
			public static readonly LegendEntry VERYCOLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 1f), null, null, true);

			// Token: 0x0400979F RID: 38815
			public static readonly LegendEntry EXTREMECOLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);
		}

		// Token: 0x02002217 RID: 8727
		public class HEATFLOW_LEGEND
		{
			// Token: 0x040097A0 RID: 38816
			public static readonly LegendEntry HEATING = new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x040097A1 RID: 38817
			public static readonly LegendEntry NEUTRAL = new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL, UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x040097A2 RID: 38818
			public static readonly LegendEntry COOLING = new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0f, 0f, 0f), null, null, true);
		}

		// Token: 0x02002218 RID: 8728
		public class POWER_LEGEND
		{
			// Token: 0x040097A3 RID: 38819
			public const float WATTAGE_WARNING_THRESHOLD = 0.75f;
		}
	}
}
