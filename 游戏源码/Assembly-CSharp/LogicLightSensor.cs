using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E54 RID: 3668
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060048F8 RID: 18680 RVA: 0x00257D70 File Offset: 0x00255F70
	private void OnCopySettings(object data)
	{
		LogicLightSensor component = ((GameObject)data).GetComponent<LogicLightSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x000CF5A0 File Offset: 0x000CD7A0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicLightSensor>(-905833192, LogicLightSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x000CF5B9 File Offset: 0x000CD7B9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x00257DAC File Offset: 0x00255FAC
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 4)
		{
			this.levels[this.simUpdateCounter] = (float)Grid.LightIntensity[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageBrightness = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.averageBrightness += this.levels[i];
		}
		this.averageBrightness /= 4f;
		if (this.activateOnBrighterThan)
		{
			if ((this.averageBrightness > this.thresholdBrightness && !base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageBrightness > this.thresholdBrightness && base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060048FC RID: 18684 RVA: 0x000CF5EC File Offset: 0x000CD7EC
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x060048FD RID: 18685 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060048FE RID: 18686 RVA: 0x00257EA4 File Offset: 0x002560A4
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

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060048FF RID: 18687 RVA: 0x000CF5FB File Offset: 0x000CD7FB
	// (set) Token: 0x06004900 RID: 18688 RVA: 0x000CF603 File Offset: 0x000CD803
	public float Threshold
	{
		get
		{
			return this.thresholdBrightness;
		}
		set
		{
			this.thresholdBrightness = value;
		}
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06004901 RID: 18689 RVA: 0x000CF60C File Offset: 0x000CD80C
	// (set) Token: 0x06004902 RID: 18690 RVA: 0x000CF614 File Offset: 0x000CD814
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnBrighterThan;
		}
		set
		{
			this.activateOnBrighterThan = value;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06004903 RID: 18691 RVA: 0x000CF61D File Offset: 0x000CD81D
	public float CurrentValue
	{
		get
		{
			return this.averageBrightness;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06004904 RID: 18692 RVA: 0x000CF625 File Offset: 0x000CD825
	public float RangeMin
	{
		get
		{
			return this.minBrightness;
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06004905 RID: 18693 RVA: 0x000CF62D File Offset: 0x000CD82D
	public float RangeMax
	{
		get
		{
			return this.maxBrightness;
		}
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x000CF635 File Offset: 0x000CD835
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x000CF63D File Offset: 0x000CD83D
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06004908 RID: 18696 RVA: 0x000CF645 File Offset: 0x000CD845
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.BRIGHTNESSSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06004909 RID: 18697 RVA: 0x000CF64C File Offset: 0x000CD84C
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x0600490A RID: 18698 RVA: 0x000CF653 File Offset: 0x000CD853
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x0600490B RID: 18699 RVA: 0x000CF65F File Offset: 0x000CD85F
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600490C RID: 18700 RVA: 0x000CF66B File Offset: 0x000CD86B
	public string Format(float value, bool units)
	{
		if (units)
		{
			return GameUtil.GetFormattedLux((int)value);
		}
		return string.Format("{0}", (int)value);
	}

	// Token: 0x0600490D RID: 18701 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x0600490E RID: 18702 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x000CF689 File Offset: 0x000CD889
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.LIGHT.LUX;
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06004910 RID: 18704 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06004911 RID: 18705 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06004912 RID: 18706 RVA: 0x000CF690 File Offset: 0x000CD890
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06004913 RID: 18707 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x040032E9 RID: 13033
	private int simUpdateCounter;

	// Token: 0x040032EA RID: 13034
	[Serialize]
	public float thresholdBrightness = 280f;

	// Token: 0x040032EB RID: 13035
	[Serialize]
	public bool activateOnBrighterThan = true;

	// Token: 0x040032EC RID: 13036
	public float minBrightness;

	// Token: 0x040032ED RID: 13037
	public float maxBrightness = 15000f;

	// Token: 0x040032EE RID: 13038
	private const int NumFrameDelay = 4;

	// Token: 0x040032EF RID: 13039
	private float[] levels = new float[4];

	// Token: 0x040032F0 RID: 13040
	private float averageBrightness;

	// Token: 0x040032F1 RID: 13041
	private bool wasOn;

	// Token: 0x040032F2 RID: 13042
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040032F3 RID: 13043
	private static readonly EventSystem.IntraObjectHandler<LogicLightSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicLightSensor>(delegate(LogicLightSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
