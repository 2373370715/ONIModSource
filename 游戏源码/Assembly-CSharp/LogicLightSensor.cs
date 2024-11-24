using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	private int simUpdateCounter;

	[Serialize]
	public float thresholdBrightness = 280f;

	[Serialize]
	public bool activateOnBrighterThan = true;

	public float minBrightness;

	public float maxBrightness = 15000f;

	private const int NumFrameDelay = 4;

	private float[] levels = new float[4];

	private float averageBrightness;

	private bool wasOn;

	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	private static readonly EventSystem.IntraObjectHandler<LogicLightSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicLightSensor>(delegate(LogicLightSensor component, object data)
	{
		component.OnCopySettings(data);
	});

	public float Threshold
	{
		get
		{
			return thresholdBrightness;
		}
		set
		{
			thresholdBrightness = value;
		}
	}

	public bool ActivateAboveThreshold
	{
		get
		{
			return activateOnBrighterThan;
		}
		set
		{
			activateOnBrighterThan = value;
		}
	}

	public float CurrentValue => averageBrightness;

	public float RangeMin => minBrightness;

	public float RangeMax => maxBrightness;

	public LocString Title => UI.UISIDESCREENS.BRIGHTNESSSWITCHSIDESCREEN.TITLE;

	public LocString ThresholdValueName => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS;

	public string AboveToolTip => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_ABOVE;

	public string BelowToolTip => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_BELOW;

	public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

	public int IncrementScale => 1;

	public NonLinearSlider.Range[] GetRanges => NonLinearSlider.GetDefaultRange(RangeMax);

	private void OnCopySettings(object data)
	{
		LogicLightSensor component = ((GameObject)data).GetComponent<LogicLightSensor>();
		if (component != null)
		{
			Threshold = component.Threshold;
			ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-905833192, OnCopySettingsDelegate);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += OnSwitchToggled;
		UpdateVisualState(force: true);
		UpdateLogicCircuit();
		wasOn = switchedOn;
	}

	public void Sim200ms(float dt)
	{
		if (simUpdateCounter < 4)
		{
			levels[simUpdateCounter] = Grid.LightIntensity[Grid.PosToCell(this)];
			simUpdateCounter++;
			return;
		}
		simUpdateCounter = 0;
		averageBrightness = 0f;
		for (int i = 0; i < 4; i++)
		{
			averageBrightness += levels[i];
		}
		averageBrightness /= 4f;
		if (activateOnBrighterThan)
		{
			if ((averageBrightness > thresholdBrightness && !base.IsSwitchedOn) || (averageBrightness < thresholdBrightness && base.IsSwitchedOn))
			{
				Toggle();
			}
		}
		else if ((averageBrightness > thresholdBrightness && base.IsSwitchedOn) || (averageBrightness < thresholdBrightness && !base.IsSwitchedOn))
		{
			Toggle();
		}
	}

	private void OnSwitchToggled(bool toggled_on)
	{
		UpdateVisualState();
		UpdateLogicCircuit();
	}

	private void UpdateLogicCircuit()
	{
		GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, switchedOn ? 1 : 0);
	}

	private void UpdateVisualState(bool force = false)
	{
		if (wasOn != switchedOn || force)
		{
			wasOn = switchedOn;
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			component.Play(switchedOn ? "on_pre" : "on_pst");
			component.Queue(switchedOn ? "on" : "off");
		}
	}

	public float GetRangeMinInputField()
	{
		return RangeMin;
	}

	public float GetRangeMaxInputField()
	{
		return RangeMax;
	}

	public string Format(float value, bool units)
	{
		if (units)
		{
			return GameUtil.GetFormattedLux((int)value);
		}
		return $"{(int)value}";
	}

	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	public float ProcessedInputValue(float input)
	{
		return input;
	}

	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.LIGHT.LUX;
	}

	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = (switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive);
		GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
	}
}
