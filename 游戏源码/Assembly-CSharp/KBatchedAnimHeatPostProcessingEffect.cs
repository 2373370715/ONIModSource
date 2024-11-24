using UnityEngine;

public class KBatchedAnimHeatPostProcessingEffect : KMonoBehaviour
{
	public const float SHOW_EFFECT_HEAT_TRESHOLD = 1f;

	private const float DISABLING_VALUE = 0f;

	private const float ENABLING_VALUE = 1f;

	private float heatProduction;

	public const float ANIM_DURATION = 1f;

	private int loopsPlayed;

	[MyCmpGet]
	private KBatchedAnimController animController;

	public float HeatProduction => heatProduction;

	public bool IsHeatProductionEnoughToShowEffect => HeatProduction >= 1f;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		animController.postProcessingEffectsAllowed |= KAnimConverter.PostProcessingEffects.TemperatureOverlay;
	}

	public void SetHeatBeingProducedValue(float heat)
	{
		heatProduction = heat;
		RefreshEffectVisualState();
	}

	public void RefreshEffectVisualState()
	{
		if (base.enabled && IsHeatProductionEnoughToShowEffect)
		{
			SetParameterValue(1f);
		}
		else
		{
			SetParameterValue(0f);
		}
	}

	private void SetParameterValue(float value)
	{
		if (animController != null)
		{
			animController.postProcessingParameters = value;
		}
	}

	protected override void OnCmpEnable()
	{
		RefreshEffectVisualState();
	}

	protected override void OnCmpDisable()
	{
		RefreshEffectVisualState();
	}

	private void Update()
	{
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad / 1f);
		if (num != loopsPlayed)
		{
			loopsPlayed = num;
			OnNewLoopReached();
		}
	}

	private void OnNewLoopReached()
	{
		if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Temperature.ID && IsHeatProductionEnoughToShowEffect)
		{
			Vector3 position = base.transform.GetPosition();
			string sound = GlobalAssets.GetSound("Temperature_Heat_Emission");
			position.z = 0f;
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, position));
		}
	}
}
