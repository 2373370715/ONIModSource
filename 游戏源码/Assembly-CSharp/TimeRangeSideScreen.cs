using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FE7 RID: 8167
public class TimeRangeSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x0600AD29 RID: 44329 RVA: 0x00110AC6 File Offset: 0x0010ECC6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.labelHeaderStart.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON;
		this.labelHeaderDuration.text = UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION;
	}

	// Token: 0x0600AD2A RID: 44330 RVA: 0x00110AF8 File Offset: 0x0010ECF8
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicTimeOfDaySensor>() != null;
	}

	// Token: 0x0600AD2B RID: 44331 RVA: 0x004109AC File Offset: 0x0040EBAC
	public override void SetTarget(GameObject target)
	{
		this.imageActiveZone.color = GlobalAssets.Instance.colorSet.logicOnSidescreen;
		this.imageInactiveZone.color = GlobalAssets.Instance.colorSet.logicOffSidescreen;
		base.SetTarget(target);
		this.targetTimedSwitch = target.GetComponent<LogicTimeOfDaySensor>();
		this.duration.onValueChanged.RemoveAllListeners();
		this.startTime.onValueChanged.RemoveAllListeners();
		this.startTime.value = this.targetTimedSwitch.startTime;
		this.duration.value = this.targetTimedSwitch.duration;
		this.ChangeSetting();
		this.startTime.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
		this.duration.onValueChanged.AddListener(delegate(float value)
		{
			this.ChangeSetting();
		});
	}

	// Token: 0x0600AD2C RID: 44332 RVA: 0x00410A94 File Offset: 0x0040EC94
	private void ChangeSetting()
	{
		this.targetTimedSwitch.startTime = this.startTime.value;
		this.targetTimedSwitch.duration = this.duration.value;
		this.imageActiveZone.rectTransform.rotation = Quaternion.identity;
		this.imageActiveZone.rectTransform.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value));
		this.imageActiveZone.fillAmount = this.duration.value;
		this.labelValueStart.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None);
		this.labelValueDuration.text = GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None);
		this.endIndicator.rotation = Quaternion.identity;
		this.endIndicator.Rotate(0f, 0f, this.NormalizedValueToDegrees(this.startTime.value + this.duration.value));
		this.startTime.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.ON_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.startTime * 100f, GameUtil.TimeSlice.None)));
		this.duration.SetTooltipText(string.Format(UI.UISIDESCREENS.TIME_RANGE_SIDE_SCREEN.DURATION_TOOLTIP, GameUtil.GetFormattedPercent(this.targetTimedSwitch.duration * 100f, GameUtil.TimeSlice.None)));
	}

	// Token: 0x0600AD2D RID: 44333 RVA: 0x00110B06 File Offset: 0x0010ED06
	public void Render200ms(float dt)
	{
		this.currentTimeMarker.rotation = Quaternion.identity;
		this.currentTimeMarker.Rotate(0f, 0f, this.NormalizedValueToDegrees(GameClock.Instance.GetCurrentCycleAsPercentage()));
	}

	// Token: 0x0600AD2E RID: 44334 RVA: 0x00110B3D File Offset: 0x0010ED3D
	private float NormalizedValueToDegrees(float value)
	{
		return 360f * value;
	}

	// Token: 0x0600AD2F RID: 44335 RVA: 0x00110B46 File Offset: 0x0010ED46
	private float SecondsToDegrees(float seconds)
	{
		return 360f * (seconds / 600f);
	}

	// Token: 0x0600AD30 RID: 44336 RVA: 0x00110B55 File Offset: 0x0010ED55
	private float DegreesToNormalizedValue(float degrees)
	{
		return degrees / 360f;
	}

	// Token: 0x040087DC RID: 34780
	public Image imageInactiveZone;

	// Token: 0x040087DD RID: 34781
	public Image imageActiveZone;

	// Token: 0x040087DE RID: 34782
	private LogicTimeOfDaySensor targetTimedSwitch;

	// Token: 0x040087DF RID: 34783
	public KSlider startTime;

	// Token: 0x040087E0 RID: 34784
	public KSlider duration;

	// Token: 0x040087E1 RID: 34785
	public RectTransform endIndicator;

	// Token: 0x040087E2 RID: 34786
	public LocText labelHeaderStart;

	// Token: 0x040087E3 RID: 34787
	public LocText labelHeaderDuration;

	// Token: 0x040087E4 RID: 34788
	public LocText labelValueStart;

	// Token: 0x040087E5 RID: 34789
	public LocText labelValueDuration;

	// Token: 0x040087E6 RID: 34790
	public RectTransform currentTimeMarker;
}
