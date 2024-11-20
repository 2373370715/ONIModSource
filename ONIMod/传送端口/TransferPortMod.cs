using System.Collections.Generic;
using HarmonyLib;
using KMod;
using RsLib;
using RsLib.Unity;
using RsTransferPort;
using RsTransferPort.Assets;
using TMPro;
using UnityEngine;



// TransferPortMod 类继承自 UserMod2，主要用于初始化和加载模组资源
public class TransferPortMod : UserMod2 {
    public static BodyAsset BodyAsset;

    /// <summary>
    /// 在 Rs 初始化之前执行的方法，主要用于设置文本样式
    /// </summary>
    private void BeforeRsInit() {
        // 获取基础文本样式
        var baseStyle = RsUITuning.TextStyleSettings.style_bodyText;

        // 创建并设置 rs_channel_name 文本样式
        var style_rs_channel_name = ScriptableObject.CreateInstance<TextStyleSetting>();
        style_rs_channel_name.name               = "style_rs_channel_name";
        // 粗体
        style_rs_channel_name.style              = FontStyles.Bold;
        // 字体
        style_rs_channel_name.sdfFont            = baseStyle.sdfFont;
        // 黑色文本
        style_rs_channel_name.textColor          = Color.black;
        // 自动换行
        style_rs_channel_name.enableWordWrapping = false;
        // 字体大小
        style_rs_channel_name.fontSize           = baseStyle.fontSize;
        RsUITuning.TextStyleSettings.AddTextStyleSetting(style_rs_channel_name);

        // 创建并设置 rs_channel_num 文本样式
        var style_rs_channel_num = ScriptableObject.CreateInstance<TextStyleSetting>();
        style_rs_channel_num.name               = "style_rs_channel_num";
        style_rs_channel_num.style              = baseStyle.style;
        style_rs_channel_num.sdfFont            = baseStyle.sdfFont;
        style_rs_channel_num.textColor          = new Color(0.55f, 0.26f, 0.3f);
        style_rs_channel_num.enableWordWrapping = false;
        style_rs_channel_num.fontSize           = baseStyle.fontSize;
        RsUITuning.TextStyleSettings.AddTextStyleSetting(style_rs_channel_num);

        // 创建并设置 rs_input 文本样式
        var style_rs_input = ScriptableObject.CreateInstance<TextStyleSetting>();
        style_rs_input.name               = "style_rs_input";
        style_rs_input.style              = baseStyle.style;
        style_rs_input.sdfFont            = baseStyle.sdfFont;
        style_rs_input.textColor          = Color.black;
        style_rs_input.enableWordWrapping = false;
        style_rs_input.fontSize           = 16;
        RsUITuning.TextStyleSettings.AddTextStyleSetting(style_rs_input);

        // 创建并设置 rs_placeholder 文本样式
        var style_rs_placeholder = ScriptableObject.CreateInstance<TextStyleSetting>();
        style_rs_placeholder.name               = "style_rs_placeholder";
        style_rs_placeholder.style              = baseStyle.style;
        style_rs_placeholder.sdfFont            = baseStyle.sdfFont;
        style_rs_placeholder.textColor          = Color.gray;
        style_rs_placeholder.enableWordWrapping = false;
        style_rs_placeholder.fontSize           = 16;
        RsUITuning.TextStyleSettings.AddTextStyleSetting(style_rs_placeholder);

        // 创建并设置 rs_detail_label 文本样式
        var style_rs_detail_label = ScriptableObject.CreateInstance<TextStyleSetting>();
        style_rs_detail_label.name               = "style_rs_detail_label";
        style_rs_detail_label.style              = baseStyle.style;
        style_rs_detail_label.sdfFont            = baseStyle.sdfFont;
        style_rs_detail_label.textColor          = new Color(0.35f, 0.14f, 0.57f);
        style_rs_detail_label.enableWordWrapping = false;
        style_rs_detail_label.fontSize           = 14;
        RsUITuning.TextStyleSettings.AddTextStyleSetting(style_rs_detail_label);

        // 创建并设置 rs_warning_label 文本样式
        var style_rs_warning_label = ScriptableObject.CreateInstance<TextStyleSetting>();
        style_rs_warning_label.name               = "style_rs_warning_label";
        style_rs_warning_label.style              = baseStyle.style;
        style_rs_warning_label.sdfFont            = baseStyle.sdfFont;
        style_rs_warning_label.textColor          = new Color(0.88f, 0.38f, 0.27f);
        style_rs_warning_label.enableWordWrapping = false;
        style_rs_warning_label.fontSize           = 14;
        RsUITuning.TextStyleSettings.AddTextStyleSetting(style_rs_warning_label);

        // 创建并设置 rs_port_channel_name 文本样式
        var style_rs_port_channel_name = ScriptableObject.CreateInstance<TextStyleSetting>();
        style_rs_port_channel_name.name               = "style_rs_port_channel_name";
        style_rs_port_channel_name.style              = FontStyles.Bold;
        style_rs_port_channel_name.sdfFont            = baseStyle.sdfFont;
        style_rs_port_channel_name.textColor          = Color.white;
        style_rs_port_channel_name.enableWordWrapping = false;
        style_rs_port_channel_name.fontSize           = 14;
        RsUITuning.TextStyleSettings.AddTextStyleSetting(style_rs_port_channel_name);
    }

    /// <summary>
    /// 模组加载时执行的方法，主要用于初始化资源和设置
    /// </summary>
    /// <param name="harmony">Harmony 实例，用于应用 Harmony 补丁</param>
    public override void OnLoad(Harmony harmony) {
        base.OnLoad(harmony);

        // 设置字体
        BeforeRsInit();
        // 初始化所有资源任务
        MyResource.InitAllTask();

        // 加载 AssetBundle
        var assetBundle = RsAssetBundle.LoadAssetBundle(mod.ContentPath, "transferport", null, true);
        RsResources.AddAssetBundle(assetBundle);

        // 加载 BodyAsset
        BodyAsset = assetBundle.LoadAsset<BodyAsset>("BodyAsset");

        // 初始化资源图标
        RsAssets.Initialize(mod, harmony)
                .AddSprite(MyOverlayModes.PortChannel.Icon, BodyAsset.portOverlayButton)
                .AddStatusItemIcon("unconnected_channel_icon", BodyAsset.unconnectedChannelIcon)
                .AddStatusItemIcon("global_connectivity_icon", BodyAsset.globalConnectivityIcon)
                .AddStatusItemIcon("planetary_isolation_icon", BodyAsset.planetaryIsolationIcon);

        // 初始化按钮菜单图标
        RsButtonMenu.Initialize(mod, harmony).AddIcon("show_overlay_self_icon", BodyAsset.showOverlayButton);

        // 初始化本地化
        RsLocalization.Initialize(mod, harmony)
                      .RegisterLoad(typeof(MYSTRINGS))
                      .RegisterAddStrings(typeof(STRINGS.BUILDINGS))
                      .RegisterAddStrings(typeof(STRINGS.UI))
                      .RegisterAddStrings(typeof(STRINGS.BUILDING));

        // 初始化侧面屏幕设置
        RsSideScreen.Initialize(mod, harmony)
                    .CopyAndCreate<ClusterLocationFilterSideScreen, WorldDiscoveredSideScreen>()
                    .CopyAndCreate<HighEnergyParticleDirectionSideScreen, MyHighEnergyParticleDirectionSideScreen>()
                    .Add(() => BodyAsset.portChannelSideScreen, true);

        // 初始化覆盖层设置
        RsOverlay.Initialize(mod, harmony)
                 .AddOverlayMode(screen => new MyOverlayModes.PortChannel(screen.powerLabelParent))
                 .AddOverlayMenuToggleInfo(() =>
                                               new RsOverlay.RsOverlayToggleInfo(MYSTRINGS.UI.OVERLAYS.PORTCHANNELMODE
                                                    .BUTTON,
                                                MyOverlayModes.PortChannel.Icon,
                                                MyOverlayModes.PortChannel.ID,
                                                "",
                                                Action.NumActions,
                                                MYSTRINGS.UI.TOOLTIPS
                                                         .PORTCHANNELMODE_OVERLAY_STRING,
                                                MYSTRINGS.UI.OVERLAYS.PORTCHANNELMODE.BUTTON))
                 .AddOverlayLegendInfo(() => new OverlayLegend.OverlayInfo {
                     mode = MyOverlayModes.PortChannel.ID,
                     name = "STRINGS.UI.OVERLAYS.PORTCHANNELMODE.NAME",
                     infoUnits = new List<OverlayLegend.OverlayInfoUnit> { new(null, "", Color.gray, Color.black) },
                     isProgrammaticallyPopulated = true,
                     diagrams = new List<GameObject> { PortChannelDiagram.Prefab }
                 })
                 .AddHoverTextCardOverlayFilterMap(MyOverlayModes.PortChannel.ID,
                                                   () => {
                                                       // int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
                                                       return false;
                                                   });

        // 初始化建筑设置
        RsBuilding.Initialize(mod, harmony)
                  .ToAdvanced()
                  .PlanAndTech(RsTypes.PlanType.Plumbing, "rs_transfer_port", "LiquidPiping")
                  .AddBuilding(LiquidTransferConduitSenderConfig.ID)
                  .AddBuilding(LiquidTransferConduitReceiverConfig.ID)
                  .PlanAndTech(RsTypes.PlanType.HVAC, "rs_transfer_port", "GasPiping")
                  .AddBuilding(GasTransferConduitSenderConfig.ID)
                  .AddBuilding(GasTransferConduitReceiverConfig.ID)
                  .PlanAndTech(RsTypes.PlanType.Conveyance, "rs_transfer_port", "SolidTransport")
                  .AddBuilding(SolidTransferConduitSenderConfig.ID)
                  .AddBuilding(SolidTransferConduitReceiverConfig.ID)
                  .PlanAndTech(RsTypes.PlanType.Automation, "rs_transfer_port", "LogicControl")
                  .AddBuilding(WirelessLogicSenderConfig.ID)
                  .AddBuilding(WirelessLogicReceiverConfig.ID)
                  .PlanAndTech(RsTypes.PlanType.Power, "rs_transfer_port", "PrettyGoodConductors")
                  .AddBuilding(WirelessPowerPortConfig.ID)
                  .PlanAndTech(RsTypes.PlanType.HEP, "rs_transfer_port", "AdvancedNuclearResearch", true)
                  .AddBuilding(RadiantParticlesTransferSenderConfig.ID)
                  .AddBuilding(RadiantParticlesTransferReceiverConfig.ID)
                  .OnlyPlan(RsTypes.PlanType.Base, "rs_transfer_port", true)
                  .AddBuilding(TransferPortCenterConfig.ID);
    }
}
