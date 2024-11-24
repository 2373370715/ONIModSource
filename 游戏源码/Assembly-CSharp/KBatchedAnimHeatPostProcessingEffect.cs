using System;
using UnityEngine;

// Token: 0x02000924 RID: 2340
public class KBatchedAnimHeatPostProcessingEffect : KMonoBehaviour
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06002A42 RID: 10818 RVA: 0x000BB7F0 File Offset: 0x000B99F0
	public float HeatProduction
	{
		get
		{
			return this.heatProduction;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06002A43 RID: 10819 RVA: 0x000BB7F8 File Offset: 0x000B99F8
	public bool IsHeatProductionEnoughToShowEffect
	{
		get
		{
			return this.HeatProduction >= 1f;
		}
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000BB80A File Offset: 0x000B9A0A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.animController.postProcessingEffectsAllowed |= KAnimConverter.PostProcessingEffects.TemperatureOverlay;
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x000BB825 File Offset: 0x000B9A25
	public void SetHeatBeingProducedValue(float heat)
	{
		this.heatProduction = heat;
		this.RefreshEffectVisualState();
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x000BB834 File Offset: 0x000B9A34
	public void RefreshEffectVisualState()
	{
		if (base.enabled && this.IsHeatProductionEnoughToShowEffect)
		{
			this.SetParameterValue(1f);
			return;
		}
		this.SetParameterValue(0f);
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x000BB85D File Offset: 0x000B9A5D
	private void SetParameterValue(float value)
	{
		if (this.animController != null)
		{
			this.animController.postProcessingParameters = value;
		}
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x000BB879 File Offset: 0x000B9A79
	protected override void OnCmpEnable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x000BB879 File Offset: 0x000B9A79
	protected override void OnCmpDisable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x001D9314 File Offset: 0x001D7514
	private void Update()
	{
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad / 1f);
		if (num != this.loopsPlayed)
		{
			this.loopsPlayed = num;
			this.OnNewLoopReached();
		}
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x001D9348 File Offset: 0x001D7548
	private void OnNewLoopReached()
	{
		if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Temperature.ID && this.IsHeatProductionEnoughToShowEffect)
		{
			Vector3 position = base.transform.GetPosition();
			string sound = GlobalAssets.GetSound("Temperature_Heat_Emission", false);
			position.z = 0f;
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, position, 1f, false));
		}
	}

	// Token: 0x04001C0D RID: 7181
	public const float SHOW_EFFECT_HEAT_TRESHOLD = 1f;

	// Token: 0x04001C0E RID: 7182
	private const float DISABLING_VALUE = 0f;

	// Token: 0x04001C0F RID: 7183
	private const float ENABLING_VALUE = 1f;

	// Token: 0x04001C10 RID: 7184
	private float heatProduction;

	// Token: 0x04001C11 RID: 7185
	public const float ANIM_DURATION = 1f;

	// Token: 0x04001C12 RID: 7186
	private int loopsPlayed;

	// Token: 0x04001C13 RID: 7187
	[MyCmpGet]
	private KBatchedAnimController animController;
}
