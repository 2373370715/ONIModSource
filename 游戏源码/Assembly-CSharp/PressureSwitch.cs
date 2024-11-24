using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000F25 RID: 3877
[SerializationConfig(MemberSerialization.OptIn)]
public class PressureSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004E22 RID: 20002 RVA: 0x00266F7C File Offset: 0x0026517C
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

	// Token: 0x06004E23 RID: 20003 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06004E24 RID: 20004 RVA: 0x000D2D2C File Offset: 0x000D0F2C
	// (set) Token: 0x06004E25 RID: 20005 RVA: 0x000D2D34 File Offset: 0x000D0F34
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

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06004E26 RID: 20006 RVA: 0x000D2D3D File Offset: 0x000D0F3D
	// (set) Token: 0x06004E27 RID: 20007 RVA: 0x000D2D45 File Offset: 0x000D0F45
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

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06004E28 RID: 20008 RVA: 0x00267044 File Offset: 0x00265244
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

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06004E29 RID: 20009 RVA: 0x000D2D4E File Offset: 0x000D0F4E
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06004E2A RID: 20010 RVA: 0x000D2D56 File Offset: 0x000D0F56
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x000D2D5E File Offset: 0x000D0F5E
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x000D2D7C File Offset: 0x000D0F7C
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06004E2D RID: 20013 RVA: 0x000CF750 File Offset: 0x000CD950
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06004E2E RID: 20014 RVA: 0x000CF79F File Offset: 0x000CD99F
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06004E2F RID: 20015 RVA: 0x000CF7A6 File Offset: 0x000CD9A6
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06004E30 RID: 20016 RVA: 0x000CF7B2 File Offset: 0x000CD9B2
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004E31 RID: 20017 RVA: 0x00267078 File Offset: 0x00265278
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

	// Token: 0x06004E32 RID: 20018 RVA: 0x000D2D9A File Offset: 0x000D0F9A
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

	// Token: 0x06004E33 RID: 20019 RVA: 0x000D2DC4 File Offset: 0x000D0FC4
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06004E34 RID: 20020 RVA: 0x000D2DD9 File Offset: 0x000D0FD9
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06004E35 RID: 20021 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06004E36 RID: 20022 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06004E37 RID: 20023 RVA: 0x000D2DE9 File Offset: 0x000D0FE9
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x0400364B RID: 13899
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x0400364C RID: 13900
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x0400364D RID: 13901
	public float rangeMin;

	// Token: 0x0400364E RID: 13902
	public float rangeMax = 1f;

	// Token: 0x0400364F RID: 13903
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04003650 RID: 13904
	private const int WINDOW_SIZE = 8;

	// Token: 0x04003651 RID: 13905
	private float[] samples = new float[8];

	// Token: 0x04003652 RID: 13906
	private int sampleIdx;
}
