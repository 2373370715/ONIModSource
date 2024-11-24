using System;
using STRINGS;

// Token: 0x02000D32 RID: 3378
public class DevLightGenerator : Light2D, IMultiSliderControl
{
	// Token: 0x06004200 RID: 16896 RVA: 0x000CAA73 File Offset: 0x000C8C73
	public DevLightGenerator()
	{
		this.sliderControls = new ISliderControl[]
		{
			new DevLightGenerator.LuxController(this),
			new DevLightGenerator.RangeController(this),
			new DevLightGenerator.FalloffController(this)
		};
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06004201 RID: 16897 RVA: 0x000CAAA2 File Offset: 0x000C8CA2
	string IMultiSliderControl.SidescreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.NAME";
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06004202 RID: 16898 RVA: 0x000CAAA9 File Offset: 0x000C8CA9
	ISliderControl[] IMultiSliderControl.sliderControls
	{
		get
		{
			return this.sliderControls;
		}
	}

	// Token: 0x06004203 RID: 16899 RVA: 0x000A65EC File Offset: 0x000A47EC
	bool IMultiSliderControl.SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x04002D07 RID: 11527
	protected ISliderControl[] sliderControls;

	// Token: 0x02000D33 RID: 3379
	protected class LuxController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06004204 RID: 16900 RVA: 0x000CAAB1 File Offset: 0x000C8CB1
		public LuxController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x000CAAC0 File Offset: 0x000C8CC0
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.BRIGHTNESS_LABEL";
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x000CAAC7 File Offset: 0x000C8CC7
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.LIGHT.LUX;
			}
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x000CA1CC File Offset: 0x000C83CC
		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x000BCEBF File Offset: 0x000BB0BF
		public float GetSliderMin(int index)
		{
			return 0f;
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x000CAAD3 File Offset: 0x000C8CD3
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.target.Lux);
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x000CAAF4 File Offset: 0x000C8CF4
		public string GetSliderTooltipKey(int index)
		{
			return "<unused>";
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x000CAAFB File Offset: 0x000C8CFB
		public float GetSliderValue(int index)
		{
			return (float)this.target.Lux;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x000CAB09 File Offset: 0x000C8D09
		public void SetSliderValue(float value, int index)
		{
			this.target.Lux = (int)value;
			this.target.FullRefresh();
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04002D08 RID: 11528
		protected Light2D target;
	}

	// Token: 0x02000D34 RID: 3380
	protected class RangeController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x0600420E RID: 16910 RVA: 0x000CAB23 File Offset: 0x000C8D23
		public RangeController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x000CAB32 File Offset: 0x000C8D32
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.RANGE_LABEL";
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x000CAB39 File Offset: 0x000C8D39
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.TILES;
			}
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x000CAB45 File Offset: 0x000C8D45
		public float GetSliderMax(int index)
		{
			return 20f;
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x000B4E11 File Offset: 0x000B3011
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x000CAB4C File Offset: 0x000C8D4C
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.target.Range);
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x000CA99D File Offset: 0x000C8B9D
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x000CAB6D File Offset: 0x000C8D6D
		public float GetSliderValue(int index)
		{
			return this.target.Range;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x000CAB7B File Offset: 0x000C8D7B
		public void SetSliderValue(float value, int index)
		{
			this.target.Range = (float)((int)value);
			this.target.FullRefresh();
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04002D09 RID: 11529
		protected Light2D target;
	}

	// Token: 0x02000D35 RID: 3381
	protected class FalloffController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06004218 RID: 16920 RVA: 0x000CAB96 File Offset: 0x000C8D96
		public FalloffController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x000CABA5 File Offset: 0x000C8DA5
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.FALLOFF_LABEL";
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x000CABAC File Offset: 0x000C8DAC
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.PERCENT;
			}
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x000C8A64 File Offset: 0x000C6C64
		public float GetSliderMax(int index)
		{
			return 100f;
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x000B4E11 File Offset: 0x000B3011
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x000CABB8 File Offset: 0x000C8DB8
		public string GetSliderTooltip(int index)
		{
			return string.Format("{0}", this.target.FalloffRate * 100f);
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x000CA99D File Offset: 0x000C8B9D
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x000CABDA File Offset: 0x000C8DDA
		public float GetSliderValue(int index)
		{
			return this.target.FalloffRate * 100f;
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x000CABEE File Offset: 0x000C8DEE
		public void SetSliderValue(float value, int index)
		{
			this.target.FalloffRate = value / 100f;
			this.target.FullRefresh();
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04002D0A RID: 11530
		protected Light2D target;
	}
}
