using KSerialization;
using UnityEngine;

#if usually || 管道
[SerializationConfig(MemberSerialization.OptIn)]
public class LiquidConduitControl : KMonoBehaviour, ISingleSliderControl {
    private static readonly EventSystem.IntraObjectHandler<LiquidConduitControl> OnCopySettingDelegate
        = new EventSystem.IntraObjectHandler<LiquidConduitControl>(delegate(LiquidConduitControl control, object obj) {
                                                                       control.OnCopySettings(obj);
                                                                   });

    private readonly int modVersion = 1;

    [MyCmpAdd]
    public CopyBuildingSettings CopyBuildingSettings;

    [Serialize]
    public int oldModVersion;

    [Serialize]
    public float capacity { get; set; } = 10f;

    public int    SliderDecimalPlaces(int index)              { throw new System.NotImplementedException(); }
    public float  GetSliderMin(int        index)              { throw new System.NotImplementedException(); }
    public float  GetSliderMax(int        index)              { throw new System.NotImplementedException(); }
    public float  GetSliderValue(int      index)              { throw new System.NotImplementedException(); }
    public void   SetSliderValue(float    percent, int index) { throw new System.NotImplementedException(); }
    public string GetSliderTooltipKey(int index) { throw new System.NotImplementedException(); }
    public string GetSliderTooltip(int    index) { throw new System.NotImplementedException(); }
    public string SliderTitleKey                 { get; }
    public string SliderUnits                    { get; }

    private void OnCopySettings(object data) {
        var gameObject          = data as GameObject;
        var betterCoolerControl = gameObject != null ? gameObject.GetComponent<BetterCoolerControl>() : null;
        var flag                = betterCoolerControl == null;
        // if (!flag) TargetTemp   = betterCoolerControl.TargetTemp;
    }
}

#endif