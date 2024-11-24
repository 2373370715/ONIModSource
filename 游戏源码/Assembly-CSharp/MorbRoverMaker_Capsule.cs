using UnityEngine;

public class MorbRoverMaker_Capsule : KMonoBehaviour
{
	public const byte MORB_PHASES_COUNT = 5;

	public const byte MORB_FIRST_PHASE_INDEX = 1;

	private const string GERM_METER_TARGET_NAME = "meter_germs_target";

	private const string GERM_METER_ANIMATION_NAME = "meter_germs";

	private const string MORB_METER_TARGET_NAME = "meter_morb_target";

	private const string MORB_METER_ANIMATION_NAME = "meter_morb";

	private const string MORB_CAPSULE_METER_TARGET_NAME = "meter_capsule_target";

	private const string MORB_CAPSULE_METER_ANIMATION_NAME = "meter_capsule";

	private static HashedString MORB_CAPSULE_METER_PUMP_ANIM_NAME = new HashedString("germ_pump");

	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	private MeterController MorbDevelopment_Meter;

	private MeterController MorbDevelopment_Capsule_Meter;

	private MeterController GermMeter;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MorbDevelopment_Meter = new MeterController(buildingAnimCtr, "meter_morb_target", "meter_morb", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack);
		GermMeter = new MeterController(buildingAnimCtr, "meter_germs_target", "meter_germs", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack);
		MorbDevelopment_Capsule_Meter = new MeterController(buildingAnimCtr, "meter_capsule_target", "meter_capsule", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack);
		MorbDevelopment_Capsule_Meter.meterController.onAnimComplete += OnGermAddedAnimationComplete;
	}

	private void OnGermAddedAnimationComplete(HashedString animName)
	{
		if (animName == MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			MorbDevelopment_Capsule_Meter.meterController.Play("meter_capsule");
		}
	}

	public void PlayPumpGermsAnimation()
	{
		if (MorbDevelopment_Capsule_Meter.meterController.currentAnim != MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			MorbDevelopment_Capsule_Meter.meterController.Play(MORB_CAPSULE_METER_PUMP_ANIM_NAME);
		}
	}

	public void SetMorbDevelopmentProgress(float morbDevelopmentProgress)
	{
		Debug.Assert(condition: true, "MORB PHASES COUNT needs to be larger than 0");
		string text = "meter_morb_" + (1 + Mathf.FloorToInt(morbDevelopmentProgress * 4f));
		if (MorbDevelopment_Meter.meterController.currentAnim != text)
		{
			MorbDevelopment_Meter.meterController.Play(text, KAnim.PlayMode.Loop);
		}
	}

	public void SetGermMeterProgress(float progress)
	{
		GermMeter.SetPositionPercent(progress);
	}
}
