using System;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class MorbRoverMaker_Capsule : KMonoBehaviour
{
	// Token: 0x06001563 RID: 5475 RVA: 0x0019351C File Offset: 0x0019171C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.MorbDevelopment_Meter = new MeterController(this.buildingAnimCtr, "meter_morb_target", "meter_morb", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.GermMeter = new MeterController(this.buildingAnimCtr, "meter_germs_target", "meter_germs", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter = new MeterController(this.buildingAnimCtr, "meter_capsule_target", "meter_capsule", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter.meterController.onAnimComplete += this.OnGermAddedAnimationComplete;
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000AF9C0 File Offset: 0x000ADBC0
	private void OnGermAddedAnimationComplete(HashedString animName)
	{
		if (animName == MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play("meter_capsule", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x000AF9F4 File Offset: 0x000ADBF4
	public void PlayPumpGermsAnimation()
	{
		if (this.MorbDevelopment_Capsule_Meter.meterController.currentAnim != MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play(MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x001935B4 File Offset: 0x001917B4
	public void SetMorbDevelopmentProgress(float morbDevelopmentProgress)
	{
		global::Debug.Assert(true, "MORB PHASES COUNT needs to be larger than 0");
		string s = "meter_morb_" + (1 + Mathf.FloorToInt(morbDevelopmentProgress * 4f)).ToString();
		if (this.MorbDevelopment_Meter.meterController.currentAnim != s)
		{
			this.MorbDevelopment_Meter.meterController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x000AFA32 File Offset: 0x000ADC32
	public void SetGermMeterProgress(float progress)
	{
		this.GermMeter.SetPositionPercent(progress);
	}

	// Token: 0x04000E70 RID: 3696
	public const byte MORB_PHASES_COUNT = 5;

	// Token: 0x04000E71 RID: 3697
	public const byte MORB_FIRST_PHASE_INDEX = 1;

	// Token: 0x04000E72 RID: 3698
	private const string GERM_METER_TARGET_NAME = "meter_germs_target";

	// Token: 0x04000E73 RID: 3699
	private const string GERM_METER_ANIMATION_NAME = "meter_germs";

	// Token: 0x04000E74 RID: 3700
	private const string MORB_METER_TARGET_NAME = "meter_morb_target";

	// Token: 0x04000E75 RID: 3701
	private const string MORB_METER_ANIMATION_NAME = "meter_morb";

	// Token: 0x04000E76 RID: 3702
	private const string MORB_CAPSULE_METER_TARGET_NAME = "meter_capsule_target";

	// Token: 0x04000E77 RID: 3703
	private const string MORB_CAPSULE_METER_ANIMATION_NAME = "meter_capsule";

	// Token: 0x04000E78 RID: 3704
	private static HashedString MORB_CAPSULE_METER_PUMP_ANIM_NAME = new HashedString("germ_pump");

	// Token: 0x04000E79 RID: 3705
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x04000E7A RID: 3706
	private MeterController MorbDevelopment_Meter;

	// Token: 0x04000E7B RID: 3707
	private MeterController MorbDevelopment_Capsule_Meter;

	// Token: 0x04000E7C RID: 3708
	private MeterController GermMeter;
}
