using System;
using KSerialization;
using UnityEngine;

#if 液冷
[SerializationConfig(MemberSerialization.OptIn)]
public class BetterCoolerControl : KMonoBehaviour, ISingleSliderControl {
    private static readonly EventSystem.IntraObjectHandler<BetterCoolerControl> OnCopySettingsDelegate
        = new EventSystem.IntraObjectHandler<BetterCoolerControl>(delegate(BetterCoolerControl control, object obj) {
                                                                      control.OnCopySettings(obj);
                                                                  });

    private readonly int modVersion = 1;

    [MyCmpAdd]
    public CopyBuildingSettings copyBuildingSettings;

    [Serialize]
    public int oldModVersion;

    [Serialize]
    public float TargetTemp { get; set; } = 293.15f;

    public int   SliderDecimalPlaces(int index) { return 2; }
    public float GetSliderMin(int        index) { return GameUtil.GetConvertedTemperature(0f); }
    public float GetSliderMax(int        index) { return GameUtil.GetConvertedTemperature(5000f); }
    public float GetSliderValue(int      index) { return GameUtil.GetConvertedTemperature(TargetTemp); }

    public void SetSliderValue(float percent, int index) {
        TargetTemp = GameUtil.GetTemperatureConvertedToKelvin(percent);
    }

    public string GetSliderTooltipKey(int index) {
        return "STRINGS.UI.UISIDESCREENS.CONDITIONERCONTROLUISIDESCREEN.TOOLTIP";
    }

    public string GetSliderTooltip(int index) {
        return string.Format(Strings.Get(GetSliderTooltipKey(0)), TargetTemp, SliderUnits);
    }

    public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.CONDITIONERCONTROLUISIDESCREEN.TITLE";
    public string SliderUnits    => "  " + GameUtil.GetTemperatureUnitSuffix();

    private void OnCopySettings(object data) {
        var gameObject          = data as GameObject;
        var betterCoolerControl = gameObject != null ? gameObject.GetComponent<BetterCoolerControl>() : null;
        var flag                = betterCoolerControl == null;
        if (!flag) TargetTemp   = betterCoolerControl.TargetTemp;
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Subscribe(-905833192, OnCopySettingsDelegate);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        var flag             = oldModVersion == 0 && Math.Abs(TargetTemp - 293.15f) > 0.01f;
        if (flag) TargetTemp = GameUtil.GetTemperatureConvertedToKelvin(TargetTemp);
        oldModVersion = modVersion;
    }
}
#endif