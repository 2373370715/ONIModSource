using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E5C RID: 3676
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPressureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004950 RID: 18768 RVA: 0x000CF8E3 File Offset: 0x000CDAE3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicPressureSensor>(-905833192, LogicPressureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004951 RID: 18769 RVA: 0x0025875C File Offset: 0x0025695C
	private void OnCopySettings(object data)
	{
		LogicPressureSensor component = ((GameObject)data).GetComponent<LogicPressureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x000CF8FC File Offset: 0x000CDAFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x00258798 File Offset: 0x00256998
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			float num2 = Grid.Element[num].IsState(this.desiredState) ? Grid.Mass[num] : 0f;
			this.samples[this.sampleIdx] = num2;
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004954 RID: 18772 RVA: 0x000CF92F File Offset: 0x000CDB2F
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06004955 RID: 18773 RVA: 0x000CF93E File Offset: 0x000CDB3E
	// (set) Token: 0x06004956 RID: 18774 RVA: 0x000CF946 File Offset: 0x000CDB46
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06004957 RID: 18775 RVA: 0x000CF94F File Offset: 0x000CDB4F
	// (set) Token: 0x06004958 RID: 18776 RVA: 0x000CF957 File Offset: 0x000CDB57
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06004959 RID: 18777 RVA: 0x00258860 File Offset: 0x00256A60
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x0600495A RID: 18778 RVA: 0x000CF960 File Offset: 0x000CDB60
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x0600495B RID: 18779 RVA: 0x000CF968 File Offset: 0x000CDB68
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x000CF970 File Offset: 0x000CDB70
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x000CF98E File Offset: 0x000CDB8E
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x0600495E RID: 18782 RVA: 0x000CF79F File Offset: 0x000CD99F
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x0600495F RID: 18783 RVA: 0x000CF7A6 File Offset: 0x000CD9A6
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06004960 RID: 18784 RVA: 0x000CF7B2 File Offset: 0x000CD9B2
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x00258894 File Offset: 0x00256A94
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat;
		if (this.desiredState == Element.State.Gas)
		{
			massFormat = GameUtil.MetricMassFormat.Gram;
		}
		else
		{
			massFormat = GameUtil.MetricMassFormat.Kilogram;
		}
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x000CF9AC File Offset: 0x000CDBAC
	public float ProcessedSliderValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input = Mathf.Round(input * 1000f) / 1000f;
		}
		else
		{
			input = Mathf.Round(input);
		}
		return input;
	}

	// Token: 0x06004963 RID: 18787 RVA: 0x000CF9D6 File Offset: 0x000CDBD6
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06004964 RID: 18788 RVA: 0x000CF9EB File Offset: 0x000CDBEB
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06004965 RID: 18789 RVA: 0x000CF750 File Offset: 0x000CD950
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06004966 RID: 18790 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06004967 RID: 18791 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x06004968 RID: 18792 RVA: 0x000CF9FB File Offset: 0x000CDBFB
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06004969 RID: 18793 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x0600496A RID: 18794 RVA: 0x002588C0 File Offset: 0x00256AC0
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

	// Token: 0x0600496B RID: 18795 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04003316 RID: 13078
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04003317 RID: 13079
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04003318 RID: 13080
	private bool wasOn;

	// Token: 0x04003319 RID: 13081
	public float rangeMin;

	// Token: 0x0400331A RID: 13082
	public float rangeMax = 1f;

	// Token: 0x0400331B RID: 13083
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x0400331C RID: 13084
	private const int WINDOW_SIZE = 8;

	// Token: 0x0400331D RID: 13085
	private float[] samples = new float[8];

	// Token: 0x0400331E RID: 13086
	private int sampleIdx;

	// Token: 0x0400331F RID: 13087
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003320 RID: 13088
	private static readonly EventSystem.IntraObjectHandler<LogicPressureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicPressureSensor>(delegate(LogicPressureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
