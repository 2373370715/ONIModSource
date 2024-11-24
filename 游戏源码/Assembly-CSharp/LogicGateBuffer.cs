using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E4C RID: 3660
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateBuffer : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06004899 RID: 18585 RVA: 0x000CF22B File Offset: 0x000CD42B
	// (set) Token: 0x0600489A RID: 18586 RVA: 0x00257250 File Offset: 0x00255450
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

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x0600489B RID: 18587 RVA: 0x000CF233 File Offset: 0x000CD433
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x0600489C RID: 18588 RVA: 0x000CF246 File Offset: 0x000CD446
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x0600489D RID: 18589 RVA: 0x000CF24D File Offset: 0x000CD44D
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x0600489E RID: 18590 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x000CF259 File Offset: 0x000CD459
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x060048A0 RID: 18592 RVA: 0x000CF260 File Offset: 0x000CD460
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x060048A1 RID: 18593 RVA: 0x000CF267 File Offset: 0x000CD467
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x060048A2 RID: 18594 RVA: 0x000CF26F File Offset: 0x000CD46F
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x060048A3 RID: 18595 RVA: 0x000CF278 File Offset: 0x000CD478
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x060048A4 RID: 18596 RVA: 0x000CF27F File Offset: 0x000CD47F
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x060048A5 RID: 18597 RVA: 0x000CF2A0 File Offset: 0x000CD4A0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateBuffer>(-905833192, LogicGateBuffer.OnCopySettingsDelegate);
	}

	// Token: 0x060048A6 RID: 18598 RVA: 0x0025727C File Offset: 0x0025547C
	private void OnCopySettings(object data)
	{
		LogicGateBuffer component = ((GameObject)data).GetComponent<LogicGateBuffer>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x060048A7 RID: 18599 RVA: 0x002572AC File Offset: 0x002554AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(1f);
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x002572F8 File Offset: 0x002554F8
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_positive)
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

	// Token: 0x060048A9 RID: 18601 RVA: 0x000CF2B9 File Offset: 0x000CD4B9
	public override void LogicTick()
	{
		if (!this.input_was_previously_positive && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x060048AA RID: 18602 RVA: 0x00257350 File Offset: 0x00255550
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 != 0)
		{
			this.input_was_previously_positive = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(0f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_positive)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_positive = false;
		}
		if (val1 == 0 && this.delayTicksRemaining <= 0)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x002573B4 File Offset: 0x002555B4
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(1f);
		if (this.outputValueOne == 0)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 0;
		base.RefreshAnimation();
	}

	// Token: 0x040032BF RID: 12991
	[Serialize]
	private bool input_was_previously_positive;

	// Token: 0x040032C0 RID: 12992
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x040032C1 RID: 12993
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x040032C2 RID: 12994
	private MeterController meter;

	// Token: 0x040032C3 RID: 12995
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040032C4 RID: 12996
	private static readonly EventSystem.IntraObjectHandler<LogicGateBuffer> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateBuffer>(delegate(LogicGateBuffer component, object data)
	{
		component.OnCopySettings(data);
	});
}
