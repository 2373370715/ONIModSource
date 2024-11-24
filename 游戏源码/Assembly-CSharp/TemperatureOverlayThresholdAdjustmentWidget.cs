using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001B4E RID: 6990
public class TemperatureOverlayThresholdAdjustmentWidget : KMonoBehaviour
{
	// Token: 0x060092D7 RID: 37591 RVA: 0x000FFE08 File Offset: 0x000FE008
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.scrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x060092D8 RID: 37592 RVA: 0x0038A7AC File Offset: 0x003889AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.scrollbar.size = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange;
		this.scrollbar.value = this.KelvinToScrollPercentage(SaveGame.Instance.relativeTemperatureOverlaySliderValue);
		this.defaultButton.onClick += this.OnDefaultPressed;
	}

	// Token: 0x060092D9 RID: 37593 RVA: 0x000FFE2C File Offset: 0x000FE02C
	private void OnValueChanged(float data)
	{
		this.SetUserConfig(data);
	}

	// Token: 0x060092DA RID: 37594 RVA: 0x000FFE35 File Offset: 0x000FE035
	private float KelvinToScrollPercentage(float kelvin)
	{
		kelvin -= TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature;
		if (kelvin < 1f)
		{
			kelvin = 1f;
		}
		return Mathf.Clamp01(kelvin / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange);
	}

	// Token: 0x060092DB RID: 37595 RVA: 0x0038A808 File Offset: 0x00388A08
	private void SetUserConfig(float scrollPercentage)
	{
		float num = TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature + TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange * scrollPercentage;
		float num2 = num - TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		float num3 = num + TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
		SimDebugView.Instance.user_temperatureThresholds[0] = num2;
		SimDebugView.Instance.user_temperatureThresholds[1] = num3;
		this.scrollBarRangeCenterText.SetText(GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeLowText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num2), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		this.scrollBarRangeHighText.SetText(GameUtil.GetFormattedTemperature((float)Mathf.RoundToInt(num3), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, true));
		SaveGame.Instance.relativeTemperatureOverlaySliderValue = num;
	}

	// Token: 0x060092DC RID: 37596 RVA: 0x000FFE5B File Offset: 0x000FE05B
	private void OnDefaultPressed()
	{
		this.scrollbar.value = this.KelvinToScrollPercentage(294.15f);
	}

	// Token: 0x04006F18 RID: 28440
	public const float DEFAULT_TEMPERATURE = 294.15f;

	// Token: 0x04006F19 RID: 28441
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x04006F1A RID: 28442
	[SerializeField]
	private LocText scrollBarRangeLowText;

	// Token: 0x04006F1B RID: 28443
	[SerializeField]
	private LocText scrollBarRangeCenterText;

	// Token: 0x04006F1C RID: 28444
	[SerializeField]
	private LocText scrollBarRangeHighText;

	// Token: 0x04006F1D RID: 28445
	[SerializeField]
	private KButton defaultButton;

	// Token: 0x04006F1E RID: 28446
	private static float maxTemperatureRange = 700f;

	// Token: 0x04006F1F RID: 28447
	private static float temperatureWindowSize = 200f;

	// Token: 0x04006F20 RID: 28448
	private static float minimumSelectionTemperature = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
}
