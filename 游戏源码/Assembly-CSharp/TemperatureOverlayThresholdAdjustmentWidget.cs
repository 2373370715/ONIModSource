using UnityEngine;
using UnityEngine.UI;

public class TemperatureOverlayThresholdAdjustmentWidget : KMonoBehaviour
{
	public const float DEFAULT_TEMPERATURE = 294.15f;

	[SerializeField]
	private Scrollbar scrollbar;

	[SerializeField]
	private LocText scrollBarRangeLowText;

	[SerializeField]
	private LocText scrollBarRangeCenterText;

	[SerializeField]
	private LocText scrollBarRangeHighText;

	[SerializeField]
	private KButton defaultButton;

	private static float maxTemperatureRange = 700f;

	private static float temperatureWindowSize = 200f;

	private static float minimumSelectionTemperature = temperatureWindowSize / 2f;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		scrollbar.onValueChanged.AddListener(OnValueChanged);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		scrollbar.size = temperatureWindowSize / maxTemperatureRange;
		scrollbar.value = KelvinToScrollPercentage(SaveGame.Instance.relativeTemperatureOverlaySliderValue);
		defaultButton.onClick += OnDefaultPressed;
	}

	private void OnValueChanged(float data)
	{
		SetUserConfig(data);
	}

	private float KelvinToScrollPercentage(float kelvin)
	{
		kelvin -= minimumSelectionTemperature;
		if (kelvin < 1f)
		{
			kelvin = 1f;
		}
		return Mathf.Clamp01(kelvin / maxTemperatureRange);
	}

	private void SetUserConfig(float scrollPercentage)
	{
		float num = minimumSelectionTemperature + maxTemperatureRange * scrollPercentage;
		float num2 = num - temperatureWindowSize / 2f;
		float num3 = num + temperatureWindowSize / 2f;
		SimDebugView.Instance.user_temperatureThresholds[0] = num2;
		SimDebugView.Instance.user_temperatureThresholds[1] = num3;
		scrollBarRangeCenterText.SetText(GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, displayUnits: true, roundInDestinationFormat: true));
		scrollBarRangeLowText.SetText(GameUtil.GetFormattedTemperature(Mathf.RoundToInt(num2), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, displayUnits: true, roundInDestinationFormat: true));
		scrollBarRangeHighText.SetText(GameUtil.GetFormattedTemperature(Mathf.RoundToInt(num3), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, displayUnits: true, roundInDestinationFormat: true));
		SaveGame.Instance.relativeTemperatureOverlaySliderValue = num;
	}

	private void OnDefaultPressed()
	{
		scrollbar.value = KelvinToScrollPercentage(294.15f);
	}
}
