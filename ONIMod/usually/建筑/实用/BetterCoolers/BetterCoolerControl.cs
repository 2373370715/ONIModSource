using System;
using KSerialization;
using UnityEngine;

#if 液冷
// 优化的冷柜控制类，继承自KMonoBehaviour，实现ISingleSliderControl接口
[SerializationConfig(MemberSerialization.OptIn)]
public class BetterCoolerControl : KMonoBehaviour, ISingleSliderControl {
    // 定义一个事件处理委托，用于处理复制设置事件
    private static readonly EventSystem.IntraObjectHandler<BetterCoolerControl> OnCopySettingsDelegate
        = new EventSystem.IntraObjectHandler<BetterCoolerControl>(delegate(BetterCoolerControl control, object obj) {
                                                                      control.OnCopySettings(obj);
                                                                  });

    // 模组版本号
    private readonly int modVersion = 1;

    // 复制建筑设置组件，[MyCmpAdd]表示unity自动添加
    [MyCmpAdd]
    public CopyBuildingSettings copyBuildingSettings;

    // 旧模组版本号，用于兼容性
    [Serialize]
    public int oldModVersion;

    // 目标温度，默认为293.15K（20°C）
    [Serialize]
    public float TargetTemp { get; set; } = 293.15f;

    // 设置滑块小数位数
    public int SliderDecimalPlaces(int index) { return 2; }

    // 获取滑块最小值
    public float GetSliderMin(int index) { return GameUtil.GetConvertedTemperature(0f); }

    // 获取滑块最大值
    public float GetSliderMax(int index) { return GameUtil.GetConvertedTemperature(5000f); }

    // 获取滑块当前值
    public float GetSliderValue(int index) { return GameUtil.GetConvertedTemperature(TargetTemp); }

    // 设置滑块值
    public void SetSliderValue(float percent, int index) {
        TargetTemp = GameUtil.GetTemperatureConvertedToKelvin(percent);
    }

    // 获取滑块工具提示的键
    public string GetSliderTooltipKey(int index) {
        return "STRINGS.UI.UISIDESCREENS.CONDITIONERCONTROLUISIDESCREEN.TOOLTIP";
    }

    // 获取滑块工具提示
    public string GetSliderTooltip(int index) {
        return string.Format(Strings.Get(GetSliderTooltipKey(0)), TargetTemp, SliderUnits);
    }

    // 滑块标题
    public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.CONDITIONERCONTROLUISIDESCREEN.TITLE";

    // 获取滑块单位
    public string SliderUnits => "  " + GameUtil.GetTemperatureUnitSuffix();

    // 处理复制设置事件
    private void OnCopySettings(object data) {
        var gameObject = data as GameObject;
        var betterCoolerControl = gameObject != null ? gameObject.GetComponent<BetterCoolerControl>() : null;
        var flag = betterCoolerControl == null;
        if (!flag) TargetTemp = betterCoolerControl.TargetTemp;
    }

    // 预制初始化时订阅复制设置事件
    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Subscribe(-905833192, OnCopySettingsDelegate);
    }

    // 实例化时，根据旧模组版本号进行兼容性处理
    protected override void OnSpawn() {
        base.OnSpawn();
        var flag = oldModVersion == 0 && Math.Abs(TargetTemp - 293.15f) > 0.01f;
        if (flag) TargetTemp = GameUtil.GetTemperatureConvertedToKelvin(TargetTemp);
        oldModVersion = modVersion;
    }
}

#endif