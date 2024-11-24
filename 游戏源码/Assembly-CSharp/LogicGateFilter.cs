using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E4E RID: 3662
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateFilter : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x060048B1 RID: 18609 RVA: 0x000CF32D File Offset: 0x000CD52D
	// (set) Token: 0x060048B2 RID: 18610 RVA: 0x00257418 File Offset: 0x00255618
	public float DelayAmount
	{
		get
		{
			return this.delayAmount;
		}
		set
		{
			this.delayAmount = value;
			int delayAmountTicks = this.DelayAmountTicks;
			if (this.delayTicksRemaining > delayAmountTicks)
			{
				this.delayTicksRemaining = delayAmountTicks;
			}
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x060048B3 RID: 18611 RVA: 0x000CF335 File Offset: 0x000CD535
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060048B4 RID: 18612 RVA: 0x000CF348 File Offset: 0x000CD548
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060048B5 RID: 18613 RVA: 0x000CF24D File Offset: 0x000CD44D
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x060048B6 RID: 18614 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x060048B7 RID: 18615 RVA: 0x000CF259 File Offset: 0x000CD459
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x060048B8 RID: 18616 RVA: 0x000CF260 File Offset: 0x000CD460
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x060048B9 RID: 18617 RVA: 0x000CF34F File Offset: 0x000CD54F
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x060048BA RID: 18618 RVA: 0x000CF357 File Offset: 0x000CD557
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x000CF360 File Offset: 0x000CD560
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x000CF367 File Offset: 0x000CD567
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x060048BD RID: 18621 RVA: 0x000CF388 File Offset: 0x000CD588
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateFilter>(-905833192, LogicGateFilter.OnCopySettingsDelegate);
	}

	// Token: 0x060048BE RID: 18622 RVA: 0x00257444 File Offset: 0x00255644
	private void OnCopySettings(object data)
	{
		LogicGateFilter component = ((GameObject)data).GetComponent<LogicGateFilter>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x060048BF RID: 18623 RVA: 0x00257474 File Offset: 0x00255674
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(0f);
	}

	// Token: 0x060048C0 RID: 18624 RVA: 0x002574C0 File Offset: 0x002556C0
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_negative)
		{
			positionPercent = 0f;
		}
		else if (this.delayTicksRemaining > 0)
		{
			positionPercent = (float)(this.DelayAmountTicks - this.delayTicksRemaining) / (float)this.DelayAmountTicks;
		}
		else
		{
			positionPercent = 1f;
		}
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060048C1 RID: 18625 RVA: 0x000CF3A1 File Offset: 0x000CD5A1
	public override void LogicTick()
	{
		if (!this.input_was_previously_negative && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x00257518 File Offset: 0x00255718
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 == 0)
		{
			this.input_was_previously_negative = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(1f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_negative)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_negative = false;
		}
		if (val1 != 0 && this.delayTicksRemaining <= 0)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060048C3 RID: 18627 RVA: 0x0025757C File Offset: 0x0025577C
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(0f);
		if (this.outputValueOne == 1)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 1;
		base.RefreshAnimation();
	}

	// Token: 0x040032C6 RID: 12998
	[Serialize]
	private bool input_was_previously_negative;

	// Token: 0x040032C7 RID: 12999
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x040032C8 RID: 13000
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x040032C9 RID: 13001
	private MeterController meter;

	// Token: 0x040032CA RID: 13002
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040032CB RID: 13003
	private static readonly EventSystem.IntraObjectHandler<LogicGateFilter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateFilter>(delegate(LogicGateFilter component, object data)
	{
		component.OnCopySettings(data);
	});
}
