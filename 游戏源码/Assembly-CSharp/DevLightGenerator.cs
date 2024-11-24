using STRINGS;

public class DevLightGenerator : Light2D, IMultiSliderControl
{
	protected class LuxController : ISingleSliderControl, ISliderControl
	{
		protected Light2D target;

		public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.BRIGHTNESS_LABEL";

		public string SliderUnits => UI.UNITSUFFIXES.LIGHT.LUX;

		public LuxController(Light2D t)
		{
			target = t;
		}

		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		public float GetSliderMin(int index)
		{
			return 0f;
		}

		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, target.Lux);
		}

		public string GetSliderTooltipKey(int index)
		{
			return "<unused>";
		}

		public float GetSliderValue(int index)
		{
			return target.Lux;
		}

		public void SetSliderValue(float value, int index)
		{
			target.Lux = (int)value;
			target.FullRefresh();
		}

		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}
	}

	protected class RangeController : ISingleSliderControl, ISliderControl
	{
		protected Light2D target;

		public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.RANGE_LABEL";

		public string SliderUnits => UI.UNITSUFFIXES.TILES;

		public RangeController(Light2D t)
		{
			target = t;
		}

		public float GetSliderMax(int index)
		{
			return 20f;
		}

		public float GetSliderMin(int index)
		{
			return 1f;
		}

		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, target.Range);
		}

		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		public float GetSliderValue(int index)
		{
			return target.Range;
		}

		public void SetSliderValue(float value, int index)
		{
			target.Range = (int)value;
			target.FullRefresh();
		}

		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}
	}

	protected class FalloffController : ISingleSliderControl, ISliderControl
	{
		protected Light2D target;

		public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.FALLOFF_LABEL";

		public string SliderUnits => UI.UNITSUFFIXES.PERCENT;

		public FalloffController(Light2D t)
		{
			target = t;
		}

		public float GetSliderMax(int index)
		{
			return 100f;
		}

		public float GetSliderMin(int index)
		{
			return 1f;
		}

		public string GetSliderTooltip(int index)
		{
			return $"{target.FalloffRate * 100f}";
		}

		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		public float GetSliderValue(int index)
		{
			return target.FalloffRate * 100f;
		}

		public void SetSliderValue(float value, int index)
		{
			target.FalloffRate = value / 100f;
			target.FullRefresh();
		}

		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}
	}

	protected ISliderControl[] sliderControls;

	string IMultiSliderControl.SidescreenTitleKey => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.NAME";

	ISliderControl[] IMultiSliderControl.sliderControls => sliderControls;

	public DevLightGenerator()
	{
		sliderControls = new ISliderControl[3]
		{
			new LuxController(this),
			new RangeController(this),
			new FalloffController(this)
		};
	}

	bool IMultiSliderControl.SidescreenEnabled()
	{
		return true;
	}
}
