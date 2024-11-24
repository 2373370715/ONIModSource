using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D06 RID: 3334
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitTemperatureSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06004125 RID: 16677 RVA: 0x0023CE18 File Offset: 0x0023B018
	private void GetContentsTemperature(out float temperature, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			temperature = contents.temperature;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			temperature = pickupable.PrimaryElement.Temperature;
			hasMass = true;
			return;
		}
		temperature = 0f;
		hasMass = false;
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06004126 RID: 16678 RVA: 0x0023CEB8 File Offset: 0x0023B0B8
	public override float CurrentValue
	{
		get
		{
			float num;
			bool flag;
			this.GetContentsTemperature(out num, out flag);
			if (flag)
			{
				this.lastValue = num;
			}
			return this.lastValue;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06004127 RID: 16679 RVA: 0x000CA231 File Offset: 0x000C8431
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06004128 RID: 16680 RVA: 0x000CA239 File Offset: 0x000C8439
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06004129 RID: 16681 RVA: 0x000CA241 File Offset: 0x000C8441
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x0600412A RID: 16682 RVA: 0x000CA24F File Offset: 0x000C844F
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x0600412B RID: 16683 RVA: 0x000CA25D File Offset: 0x000C845D
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x0600412C RID: 16684 RVA: 0x000CA264 File Offset: 0x000C8464
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x0600412D RID: 16685 RVA: 0x000CA26B File Offset: 0x000C846B
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x0600412E RID: 16686 RVA: 0x000CA277 File Offset: 0x000C8477
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600412F RID: 16687 RVA: 0x000CA283 File Offset: 0x000C8483
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x06004130 RID: 16688 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06004131 RID: 16689 RVA: 0x000CA297 File Offset: 0x000C8497
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06004132 RID: 16690 RVA: 0x0023CEE0 File Offset: 0x0023B0E0
	public LocString ThresholdValueUnits()
	{
		LocString result = null;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			break;
		}
		return result;
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06004133 RID: 16691 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06004134 RID: 16692 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06004135 RID: 16693 RVA: 0x0023CF20 File Offset: 0x0023B120
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(25f, 260f),
				new NonLinearSlider.Range(50f, 400f),
				new NonLinearSlider.Range(12f, 1500f),
				new NonLinearSlider.Range(13f, 10000f)
			};
		}
	}

	// Token: 0x04002C6E RID: 11374
	public float rangeMin;

	// Token: 0x04002C6F RID: 11375
	public float rangeMax = 373.15f;

	// Token: 0x04002C70 RID: 11376
	[Serialize]
	private float lastValue;
}
