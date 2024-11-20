using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using MonoMod.Utils;
using UnityEngine;

namespace RsLib;

public delegate OverlayModes.Mode CreateModesHandle(RsOverlay.OverlayScreenWrap overlayScreen);

public delegate RsOverlay.RsOverlayToggleInfo CreateOverlayToggleInfoHandle();

public delegate OverlayLegend.OverlayInfo CreateOverlayLegendInfoHandle();

public class RsOverlay : RsModule<RsOverlay> {
    private static readonly MethodInfo RegisterModesMethodInfo
        = typeof(OverlayScreen).GetMethod("RegisterMode", BindingFlags.Instance | BindingFlags.NonPublic);

    private static readonly Type OverlayToggleInfoType
        = typeof(OverlayMenu).GetNestedType("OverlayToggleInfo", BindingFlags.NonPublic);

    private static readonly FieldInfo OverlayBitfieldMapFieldInfo
        = typeof(StatusItem).GetField("overlayBitfieldMap", BindingFlags.Static | BindingFlags.NonPublic);

    private readonly List<CreateModesHandle>              createModesHandles             = new();
    private readonly List<CreateOverlayLegendInfoHandle>  createOverlayLegendInfoHandles = new();
    private readonly List<CreateOverlayToggleInfoHandle>  createOverlayToggleInfoHandles = new();
    private readonly Dictionary<HashedString, Func<bool>> overlayFilterMap               = new();

    private static void OverlayScreen_RegisterModes_Postfix(OverlayScreen __instance) {
        var screenWrap = new OverlayScreenWrap();
        RsField.Copy(__instance, screenWrap);
        foreach (var func in Instance.createModesHandles) {
            var mode = func(screenWrap);

            //添加bit，防止warning

            var itemOverlaysMap
                = (Dictionary<HashedString, StatusItem.StatusItemOverlays>)OverlayBitfieldMapFieldInfo.GetValue(null);

            if (!itemOverlaysMap.ContainsKey(mode.ViewMode()))
                itemOverlaysMap.Add(mode.ViewMode(), StatusItem.StatusItemOverlays.None);

            RegisterModesMethodInfo.Invoke(__instance, new object[] { mode });
        }
    }

    private static void OverlayMenu_InitializeToggles_Postfix(List<KIconToggleMenu.ToggleInfo> ___overlayToggleInfos) {
        foreach (var infoHandle in Instance.createOverlayToggleInfoHandles)
            ___overlayToggleInfos.Add(infoHandle().ToOverlayToggleInfo());
    }

    private static void OverlayLegend_OnSpawn_Prefix(List<OverlayLegend.OverlayInfo> ___overlayInfoList) {
        foreach (var handle in Instance.createOverlayLegendInfoHandles) ___overlayInfoList.Add(handle());
    }

    private static void SelectToolHoverTextCard_OnSpawn_Postfix(
        Dictionary<HashedString, Func<bool>> ___overlayFilterMap) {
        if (Instance.overlayFilterMap != null && Instance.overlayFilterMap.Count > 0)
            ___overlayFilterMap.AddRange(Instance.overlayFilterMap);
    }

    public RsOverlay AddOverlayMode(CreateModesHandle handle) {
        createModesHandles.Add(handle);
        return this;
    }

    /// <summary>
    ///     注意：需要手动添加icon
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    public RsOverlay AddOverlayMenuToggleInfo(CreateOverlayToggleInfoHandle handle) {
        createOverlayToggleInfoHandles.Add(handle);
        return this;
    }

    public RsOverlay AddOverlayLegendInfo(CreateOverlayLegendInfoHandle handle) {
        createOverlayLegendInfoHandles.Add(handle);
        return this;
    }

    public RsOverlay AddHoverTextCardOverlayFilterMap(HashedString viewMode, Func<bool> handle) {
        overlayFilterMap.Add(viewMode, handle);
        return this;
    }

    protected override void Initialized() {
        Harmony.Patch(typeof(OverlayScreen),
                      "RegisterModes",
                      postfix: new HarmonyMethod(typeof(RsOverlay), nameof(OverlayScreen_RegisterModes_Postfix)));

        Harmony.Patch(typeof(OverlayMenu),
                      "InitializeToggles",
                      postfix: new HarmonyMethod(typeof(RsOverlay), nameof(OverlayMenu_InitializeToggles_Postfix)));

        Harmony.Patch(typeof(OverlayLegend),
                      "OnSpawn",
                      new HarmonyMethod(typeof(RsOverlay), nameof(OverlayLegend_OnSpawn_Prefix)));

        Harmony.Patch(typeof(SelectToolHoverTextCard),
                      "OnSpawn",
                      postfix: new HarmonyMethod(typeof(RsOverlay), nameof(SelectToolHoverTextCard_OnSpawn_Postfix)));
    }

    public class RsOverlayToggleInfo {
        private readonly Action       hotKey = Action.NumActions;
        private readonly string       icon_name;
        private readonly string       required_tech_item = "";
        private readonly HashedString sim_view;
        private readonly string       text;
        private readonly string       tooltip        = "";
        private readonly string       tooltip_header = "";

        public RsOverlayToggleInfo(string       text,
                                   string       icon_name,
                                   HashedString sim_view,
                                   string       required_tech_item = "",
                                   Action       hotKey             = Action.NumActions,
                                   string       tooltip            = "",
                                   string       tooltip_header     = "") {
            this.text               = text;
            this.icon_name          = icon_name;
            this.sim_view           = sim_view;
            this.required_tech_item = required_tech_item;
            this.hotKey             = hotKey;
            this.tooltip            = tooltip;
            this.tooltip_header     = tooltip_header;
        }

        public KIconToggleMenu.ToggleInfo ToOverlayToggleInfo() {
            return (KIconToggleMenu.ToggleInfo)Activator.CreateInstance(OverlayToggleInfoType,
                                                                        text,
                                                                        icon_name,
                                                                        sim_view,
                                                                        required_tech_item,
                                                                        hotKey,
                                                                        tooltip,
                                                                        tooltip_header);
        }
    }

    public class OverlayScreenWrap {
        public Vector3          batteryUIOffset;
        public Vector3          batteryUISmallTransformerOffset;
        public Vector3          batteryUITransformerOffset;
        public BatteryUI        batUIPrefab;
        public Color            buildingDisabledColour;
        public Color32          circuitOverloadingColour;
        public Color32          circuitSafeColour;
        public Color32          circuitStrainingColour;
        public Color32          circuitUnpoweredColour;
        public Color            consumerColour;
        public GameObject       diseaseOverlayPrefab;
        public Color            generatorColour;
        public GameObject       harvestableNotificationPrefab;
        public LogicModeUI      logicModeUIPrefab;
        public Vector3          powerLabelOffset;
        public Canvas           powerLabelParent;
        public LocText          powerLabelPrefab;
        public GameObject       suitOverlayPrefab;
        public TextStyleSetting TooltipDescription;
        public TextStyleSetting TooltipHeader;
    }

    // public class OverlayModeInfo
    // {
    //     
    // }
}