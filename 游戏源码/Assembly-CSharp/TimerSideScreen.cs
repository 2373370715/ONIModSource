using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FE8 RID: 8168
public class TimerSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x0600AD34 RID: 44340 RVA: 0x00110B66 File Offset: 0x0010ED66
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderOnDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.ON;
		this.labelHeaderOffDuration.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.OFF;
	}

	// Token: 0x0600AD35 RID: 44341 RVA: 0x00410C0C File Offset: 0x0040EE0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.modeButton.onClick += delegate()
		{
			this.ToggleMode();
		};
		this.resetButton.onClick += this.ResetTimer;
		this.onDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.onDurationNumberInput.currentValue, this.onDurationSlider);
		};
		this.offDurationNumberInput.onEndEdit += delegate()
		{
			this.UpdateDurationValueFromTextInput(this.offDurationNumberInput.currentValue, this.offDurationSlider);
		};
		this.onDurationSlider.wholeNumbers = false;
		this.offDurationSlider.wholeNumbers = false;
	}

	// Token: 0x0600AD36 RID: 44342 RVA: 0x00110B98 File Offset: 0x0010ED98
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimerSensor>() != null;
	}

	// Token: 0x0600AD37 RID: 44343 RVA: 0x00410C94 File Offset: 0x0040EE94
	public override void SetTarget(GameObject target)
	{
		this.greenActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.redActiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimerSensor>();
		this.onDurationSlider.onValueChanged.RemoveAllListeners();
		this.offDurationSlider.onValueChanged.RemoveAllListeners();
		this.cyclesMode = this.targetTimedSwitch.displayCyclesMode;
		this.UpdateVisualsForNewTarget();
		this.ReconfigureRingVisuals();
		this.onDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.offDurationSlider.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x0600AD38 RID: 44344 RVA: 0x00410D68 File Offset: 0x0040EF68
	private void UpdateVisualsForNewTarget()
	{
		float onDuration = this.targetTimedSwitch.onDuration;
		float offDuration = this.targetTimedSwitch.offDuration;
		bool displayCyclesMode = this.targetTimedSwitch.displayCyclesMode;
		if (displayCyclesMode)
		{
			this.onDurationSlider.minValue = this.minCycles;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxCycles;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 2;
			this.offDurationSlider.minValue = this.minCycles;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxCycles;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 2;
			this.onDurationSlider.value = onDuration / 600f;
			this.offDurationSlider.value = offDuration / 600f;
			this.onDurationNumberInput.SetAmount(onDuration / 600f);
			this.offDurationNumberInput.SetAmount(offDuration / 600f);
		}
		else
		{
			this.onDurationSlider.minValue = this.minSeconds;
			this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
			this.onDurationSlider.maxValue = this.maxSeconds;
			this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
			this.onDurationNumberInput.decimalPlaces = 1;
			this.offDurationSlider.minValue = this.minSeconds;
			this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
			this.offDurationSlider.maxValue = this.maxSeconds;
			this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
			this.offDurationNumberInput.decimalPlaces = 1;
			this.onDurationSlider.value = onDuration;
			this.offDurationSlider.value = offDuration;
			this.onDurationNumberInput.SetAmount(onDuration);
			this.offDurationNumberInput.SetAmount(offDuration);
		}
		this.modeButton.GetComponentInChildren<LocText>().text = (displayCyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x0600AD39 RID: 44345 RVA: 0x00410FA8 File Offset: 0x0040F1A8
	private void ToggleMode()
	{
		this.cyclesMode = !this.cyclesMode;
		this.targetTimedSwitch.displayCyclesMode = this.cyclesMode;
		float num = this.onDurationSlider.value;
		float num2 = this.offDurationSlider.value;
		if (this.cyclesMode)
		{
			num = this.onDurationSlider.value / 600f;
			num2 = this.offDurationSlider.value / 600f;
		}
		else
		{
			num = this.onDurationSlider.value * 600f;
			num2 = this.offDurationSlider.value * 600f;
		}
		this.onDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.onDurationNumberInput.minValue = this.onDurationSlider.minValue;
		this.onDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.onDurationNumberInput.maxValue = this.onDurationSlider.maxValue;
		this.onDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.offDurationSlider.minValue = (this.cyclesMode ? this.minCycles : this.minSeconds);
		this.offDurationNumberInput.minValue = this.offDurationSlider.minValue;
		this.offDurationSlider.maxValue = (this.cyclesMode ? this.maxCycles : this.maxSeconds);
		this.offDurationNumberInput.maxValue = this.offDurationSlider.maxValue;
		this.offDurationNumberInput.decimalPlaces = (this.cyclesMode ? 2 : 1);
		this.onDurationSlider.value = num;
		this.offDurationSlider.value = num2;
		this.onDurationNumberInput.SetAmount(num);
		this.offDurationNumberInput.SetAmount(num2);
		this.modeButton.GetComponentInChildren<LocText>().text = (this.cyclesMode ? UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_CYCLES : UI.UISIDESCREENS.TIMER_SIDE_SCREEN.MODE_LABEL_SECONDS);
	}

	// Token: 0x0600AD3A RID: 44346 RVA: 0x004111A4 File Offset: 0x0040F3A4
	private void ChangeSetting()
	{
		this.targetTimedSwitch.onDuration = (this.cyclesMode ? (this.onDurationSlider.value * 600f) : this.onDurationSlider.value);
		this.targetTimedSwitch.offDuration = (this.cyclesMode ? (this.offDurationSlider.value * 600f) : this.offDurationSlider.value);
		this.ReconfigureRingVisuals();
		this.onDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.onDuration / 600f).ToString("F2") : this.targetTimedSwitch.onDuration.ToString());
		this.offDurationNumberInput.SetDisplayValue(this.cyclesMode ? (this.targetTimedSwitch.offDuration / 600f).ToString("F2") : this.targetTimedSwitch.offDuration.ToString());
		this.onDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.GREEN_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.onDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.onDuration, "F0")));
		this.offDurationSlider.SetTooltipText(string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.RED_DURATION_TOOLTIP, this.cyclesMode ? GameUtil.GetFormattedCycles(this.targetTimedSwitch.offDuration, "F2", false) : GameUtil.GetFormattedTime(this.targetTimedSwitch.offDuration, "F0")));
		if (this.phaseLength == 0f)
		{
			this.timeLeft.text = UI.UISIDESCREENS.TIMER_SIDE_SCREEN.DISABLED;
			if (this.targetTimedSwitch.IsSwitchedOn)
			{
				this.greenActiveZone.fillAmount = 1f;
				this.redActiveZone.fillAmount = 0f;
			}
			else
			{
				this.greenActiveZone.fillAmount = 0f;
				this.redActiveZone.fillAmount = 1f;
			}
			this.targetTimedSwitch.timeElapsedInCurrentState = 0f;
			this.currentTimeMarker.rotation = Quaternion.identity;
			this.currentTimeMarker.Rotate(0f, 0f, 0f);
		}
	}

	// Token: 0x0600AD3B RID: 44347 RVA: 0x004113EC File Offset: 0x0040F5EC
	private void ReconfigureRingVisuals()
	{
		this.phaseLength = this.targetTimedSwitch.onDuration + this.targetTimedSwitch.offDuration;
		this.greenActiveZone.fillAmount = this.targetTimedSwitch.onDuration / this.phaseLength;
		this.redActiveZone.fillAmount = this.targetTimedSwitch.offDuration / this.phaseLength;
	}

	// Token: 0x0600AD3C RID: 44348 RVA: 0x00411450 File Offset: 0x0040F650
	public void RenderEveryTick(float dt)
	{
		if (this.phaseLength == 0f)
		{
			return;
		}
		float timeElapsedInCurrentState = this.targetTimedSwitch.timeElapsedInCurrentState;
		if (this.cyclesMode)
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedCycles(timeElapsedInCurrentState, "F2", false), GameUtil.GetFormattedCycles(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F2", false));
		}
		else
		{
			this.timeLeft.text = string.Format(UI.UISIDESCREENS.TIMER_SIDE_SCREEN.CURRENT_TIME, GameUtil.GetFormattedTime(timeElapsedInCurrentState, "F1"), GameUtil.GetFormattedTime(this.targetTimedSwitch.IsSwitchedOn ? this.targetTimedSwitch.onDuration : this.targetTimedSwitch.offDuration, "F1"));
		}
		this.currentTimeMarker.rotation = Quaternion.identity;
		if (this.targetTimedSwitch.IsSwitchedOn)
		{
			this.currentTimeMarker.Rotate(0f, 0f, this.targetTimedSwitch.timeElapsedInCurrentState / this.phaseLength * -360f);
			return;
		}
		this.currentTimeMarker.Rotate(0f, 0f, (this.targetTimedSwitch.onDuration + this.targetTimedSwitch.timeElapsedInCurrentState) / this.phaseLength * -360f);
	}

	// Token: 0x0600AD3D RID: 44349 RVA: 0x004115B0 File Offset: 0x0040F7B0
	private void UpdateDurationValueFromTextInput(float newValue, KSlider slider)
	{
		if (newValue < slider.minValue)
		{
			newValue = slider.minValue;
		}
		if (newValue > slider.maxValue)
		{
			newValue = slider.maxValue;
		}
		slider.value = newValue;
		NonLinearSlider nonLinearSlider = slider as NonLinearSlider;
		if (nonLinearSlider != null)
		{
			slider.value = nonLinearSlider.GetPercentageFromValue(newValue);
			return;
		}
		slider.value = newValue;
	}

	// Token: 0x0600AD3E RID: 44350 RVA: 0x00110BA6 File Offset: 0x0010EDA6
	private void ResetTimer()
	{
		this.targetTimedSwitch.ResetTimer();
	}

	// Token: 0x040087E7 RID: 34791
	public Image greenActiveZone;

	// Token: 0x040087E8 RID: 34792
	public Image redActiveZone;

	// Token: 0x040087E9 RID: 34793
	private LogicTimerSensor targetTimedSwitch;

	// Token: 0x040087EA RID: 34794
	public KToggle modeButton;

	// Token: 0x040087EB RID: 34795
	public KButton resetButton;

	// Token: 0x040087EC RID: 34796
	public KSlider onDurationSlider;

	// Token: 0x040087ED RID: 34797
	[SerializeField]
	private KNumberInputField onDurationNumberInput;

	// Token: 0x040087EE RID: 34798
	public KSlider offDurationSlider;

	// Token: 0x040087EF RID: 34799
	[SerializeField]
	private KNumberInputField offDurationNumberInput;

	// Token: 0x040087F0 RID: 34800
	public RectTransform endIndicator;

	// Token: 0x040087F1 RID: 34801
	public RectTransform currentTimeMarker;

	// Token: 0x040087F2 RID: 34802
	public LocText labelHeaderOnDuration;

	// Token: 0x040087F3 RID: 34803
	public LocText labelHeaderOffDuration;

	// Token: 0x040087F4 RID: 34804
	public LocText labelValueOnDuration;

	// Token: 0x040087F5 RID: 34805
	public LocText labelValueOffDuration;

	// Token: 0x040087F6 RID: 34806
	public LocText timeLeft;

	// Token: 0x040087F7 RID: 34807
	public float phaseLength;

	// Token: 0x040087F8 RID: 34808
	private bool cyclesMode;

	// Token: 0x040087F9 RID: 34809
	[SerializeField]
	private float minSeconds;

	// Token: 0x040087FA RID: 34810
	[SerializeField]
	private float maxSeconds = 600f;

	// Token: 0x040087FB RID: 34811
	[SerializeField]
	private float minCycles;

	// Token: 0x040087FC RID: 34812
	[SerializeField]
	private float maxCycles = 10f;

	// Token: 0x040087FD RID: 34813
	private const int CYCLEMODE_DECIMALS = 2;

	// Token: 0x040087FE RID: 34814
	private const int SECONDSMODE_DECIMALS = 1;
}
