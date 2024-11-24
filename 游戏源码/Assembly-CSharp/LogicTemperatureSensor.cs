using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E68 RID: 3688
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTemperatureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x060049E3 RID: 18915 RVA: 0x00259730 File Offset: 0x00257930
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x060049E4 RID: 18916 RVA: 0x000CFD8C File Offset: 0x000CDF8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTemperatureSensor>(-905833192, LogicTemperatureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060049E5 RID: 18917 RVA: 0x00259758 File Offset: 0x00257958
	private void OnCopySettings(object data)
	{
		LogicTemperatureSensor component = ((GameObject)data).GetComponent<LogicTemperatureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060049E6 RID: 18918 RVA: 0x00259794 File Offset: 0x00257994
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060049E7 RID: 18919 RVA: 0x002597E8 File Offset: 0x002579E8
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.temperatures[this.simUpdateCounter] = Grid.Temperature[i];
				this.simUpdateCounter++;
			}
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageTemp = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageTemp += this.temperatures[j];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp <= this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp >= this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060049E8 RID: 18920 RVA: 0x000CFDA5 File Offset: 0x000CDFA5
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x000CFDAD File Offset: 0x000CDFAD
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x00259900 File Offset: 0x00257B00
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060049EC RID: 18924 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x060049ED RID: 18925 RVA: 0x000CFDBC File Offset: 0x000CDFBC
	// (set) Token: 0x060049EE RID: 18926 RVA: 0x000CFDC4 File Offset: 0x000CDFC4
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x060049EF RID: 18927 RVA: 0x000CFDD4 File Offset: 0x000CDFD4
	// (set) Token: 0x060049F0 RID: 18928 RVA: 0x000CFDDC File Offset: 0x000CDFDC
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x060049F1 RID: 18929 RVA: 0x000CFDEC File Offset: 0x000CDFEC
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x060049F2 RID: 18930 RVA: 0x000CFDF4 File Offset: 0x000CDFF4
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x060049F3 RID: 18931 RVA: 0x000CFDFC File Offset: 0x000CDFFC
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x000CFE04 File Offset: 0x000CE004
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x060049F5 RID: 18933 RVA: 0x000CFE12 File Offset: 0x000CE012
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x060049F6 RID: 18934 RVA: 0x000CA25D File Offset: 0x000C845D
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x060049F7 RID: 18935 RVA: 0x000CFE20 File Offset: 0x000CE020
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x060049F8 RID: 18936 RVA: 0x000CFE27 File Offset: 0x000CE027
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x060049F9 RID: 18937 RVA: 0x000CFE33 File Offset: 0x000CE033
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060049FA RID: 18938 RVA: 0x000CFE3F File Offset: 0x000CE03F
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, true);
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060049FC RID: 18940 RVA: 0x000CA297 File Offset: 0x000C8497
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x060049FD RID: 18941 RVA: 0x0023CEE0 File Offset: 0x0023B0E0
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

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x060049FE RID: 18942 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x060049FF RID: 18943 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06004A00 RID: 18944 RVA: 0x0023CF20 File Offset: 0x0023B120
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

	// Token: 0x0400335D RID: 13149
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x0400335E RID: 13150
	private int simUpdateCounter;

	// Token: 0x0400335F RID: 13151
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04003360 RID: 13152
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04003361 RID: 13153
	[Serialize]
	private bool dirty = true;

	// Token: 0x04003362 RID: 13154
	public float minTemp;

	// Token: 0x04003363 RID: 13155
	public float maxTemp = 373.15f;

	// Token: 0x04003364 RID: 13156
	private const int NumFrameDelay = 8;

	// Token: 0x04003365 RID: 13157
	private float[] temperatures = new float[8];

	// Token: 0x04003366 RID: 13158
	private float averageTemp;

	// Token: 0x04003367 RID: 13159
	private bool wasOn;

	// Token: 0x04003368 RID: 13160
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003369 RID: 13161
	private static readonly EventSystem.IntraObjectHandler<LogicTemperatureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTemperatureSensor>(delegate(LogicTemperatureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
