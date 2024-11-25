﻿using System;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTemperatureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
			public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTemperatureSensor>(-905833192, LogicTemperatureSensor.OnCopySettingsDelegate);
	}

		private void OnCopySettings(object data)
	{
		LogicTemperatureSensor component = ((GameObject)data).GetComponent<LogicTemperatureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

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

		public float GetTemperature()
	{
		return this.averageTemp;
	}

		private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

		private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

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

		protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

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

			public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

			public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

			public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

		public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

		public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

			public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

			public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

			public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

			public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

		public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, true);
	}

		public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

		public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

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

			public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

			public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

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

		private HandleVector<int>.Handle structureTemperature;

		private int simUpdateCounter;

		[Serialize]
	public float thresholdTemperature = 280f;

		[Serialize]
	public bool activateOnWarmerThan;

		[Serialize]
	private bool dirty = true;

		public float minTemp;

		public float maxTemp = 373.15f;

		private const int NumFrameDelay = 8;

		private float[] temperatures = new float[8];

		private float averageTemp;

		private bool wasOn;

		[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

		private static readonly EventSystem.IntraObjectHandler<LogicTemperatureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTemperatureSensor>(delegate(LogicTemperatureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
