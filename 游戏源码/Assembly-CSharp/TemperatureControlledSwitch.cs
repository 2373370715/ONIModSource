using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000FF2 RID: 4082
[SerializationConfig(MemberSerialization.OptIn)]
public class TemperatureControlledSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06005303 RID: 21251 RVA: 0x00276D5C File Offset: 0x00274F5C
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x06005304 RID: 21252 RVA: 0x000D6160 File Offset: 0x000D4360
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x06005305 RID: 21253 RVA: 0x00276D84 File Offset: 0x00274F84
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8)
		{
			this.temperatures[this.simUpdateCounter] = Grid.Temperature[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageTemp = 0f;
		for (int i = 0; i < 8; i++)
		{
			this.averageTemp += this.temperatures[i];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp > this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06005306 RID: 21254 RVA: 0x000D617E File Offset: 0x000D437E
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06005307 RID: 21255 RVA: 0x000D6186 File Offset: 0x000D4386
	// (set) Token: 0x06005308 RID: 21256 RVA: 0x000D618E File Offset: 0x000D438E
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06005309 RID: 21257 RVA: 0x000D6197 File Offset: 0x000D4397
	// (set) Token: 0x0600530A RID: 21258 RVA: 0x000D619F File Offset: 0x000D439F
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x0600530B RID: 21259 RVA: 0x000D61A8 File Offset: 0x000D43A8
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x0600530C RID: 21260 RVA: 0x000D61B0 File Offset: 0x000D43B0
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x0600530D RID: 21261 RVA: 0x000D61B8 File Offset: 0x000D43B8
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x0600530E RID: 21262 RVA: 0x000D61C0 File Offset: 0x000D43C0
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x0600530F RID: 21263 RVA: 0x000D61CE File Offset: 0x000D43CE
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06005310 RID: 21264 RVA: 0x000CA25D File Offset: 0x000C845D
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06005311 RID: 21265 RVA: 0x000CFE20 File Offset: 0x000CE020
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06005312 RID: 21266 RVA: 0x000CFE27 File Offset: 0x000CE027
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06005313 RID: 21267 RVA: 0x000CFE33 File Offset: 0x000CE033
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06005314 RID: 21268 RVA: 0x000CA283 File Offset: 0x000C8483
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x06005315 RID: 21269 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06005316 RID: 21270 RVA: 0x000CA297 File Offset: 0x000C8497
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06005317 RID: 21271 RVA: 0x0023CEE0 File Offset: 0x0023B0E0
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

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06005318 RID: 21272 RVA: 0x000A65EC File Offset: 0x000A47EC
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.InputField;
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06005319 RID: 21273 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x0600531A RID: 21274 RVA: 0x000D61DC File Offset: 0x000D43DC
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x040039FD RID: 14845
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x040039FE RID: 14846
	private int simUpdateCounter;

	// Token: 0x040039FF RID: 14847
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04003A00 RID: 14848
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04003A01 RID: 14849
	public float minTemp;

	// Token: 0x04003A02 RID: 14850
	public float maxTemp = 373.15f;

	// Token: 0x04003A03 RID: 14851
	private const int NumFrameDelay = 8;

	// Token: 0x04003A04 RID: 14852
	private float[] temperatures = new float[8];

	// Token: 0x04003A05 RID: 14853
	private float averageTemp;
}
