using System.Collections.Generic;
using RsLib;
using RsLib.Adapter;
using UnityEngine;
using UnityEngine.UI;

namespace RsTransferPort;

public class PortChannelDiagram : MonoBehaviour {
    [SerializeField]
    private MultiToggle disableLineAnimToggle;

    // [SerializeField] private MultiToggle centralConnectionToggle;
    // [SerializeField] private MultiToggle nearbyConnectionToggle;
    // [SerializeField] private MultiToggle noneConnectionToggle;
    [SerializeField]
    private RsMultiToggleGroupCom lineToggleGroup;

    [SerializeField]
    private GameObject moreDetailParent;

    [SerializeField]
    private MultiToggle showOnlyGlobalToggle;

    [SerializeField]
    private MultiToggle showOnlyNameToggle;

    [SerializeField]
    private MultiToggle showPriorityInfoToggle;

    [SerializeField]
    private RsMultiToggleGroupCom typeToggleGroup;

    public static GameObject Prefab => RsResources.Load<GameObject>("ui/port_overlay_diagram");

    public static void InitPrefab() {
        RsResources.AddLoadPrefabTask("ui/port_overlay_diagram",
                                      parent => {
                                          var root = RsUIBuilder.UIGameObject(nameof(PortChannelDiagram),
                                                                              parent,
                                                                              false);

                                          root.name = nameof(PortChannelDiagram);
                                          var layoutGroup = root.AddComponent<VerticalLayoutGroup>();

                                          layoutGroup.childControlHeight = true;
                                          layoutGroup.childControlWidth  = true;

                                          var rectTransform = root.rectTransform();
                                          rectTransform.anchorMin = new Vector2(0, 0.5f);
                                          rectTransform.anchorMax = new Vector2(1, 0.5f);
                                          rectTransform.offsetMax = new Vector2();
                                          rectTransform.offsetMin = new Vector2();

                                          var diagram = root.AddComponent<PortChannelDiagram>();

                                          var lineToggleGroup = root.AddComponent<RsMultiToggleGroupCom>();
                                          lineToggleGroup.toggles = new[] {
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_Hide_LINE,
                                               0,
                                               root),
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_CENTER_LINE,
                                               0,
                                               root),
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_NEAR_LINE,
                                               0,
                                               root)
                                          };

                                          RsUIBuilder.BlockLine(root, 16);

                                          //禁止连线动画
                                          var disableLineAnim = RsUIBuilder
                                                                .ToggleEntry(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                                                 .RS_DISABLE_LINE_ANIM,
                                                                             ToolParameterMenu.ToggleState.Off,
                                                                             root)
                                                                .GetComponentInChildren<MultiToggle>();

                                          var showPriorityInfo = RsUIBuilder
                                                                 .ToggleEntry(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                                                  .RS_SHOW_PRIORITY,
                                                                              ToolParameterMenu.ToggleState.Off,
                                                                              root)
                                                                 .GetComponentInChildren<MultiToggle>();

                                          var moreDetail            = RsUIBuilder.UIGameObject("MoreDetail", root);
                                          var moreDetailLayoutGroup = moreDetail.AddComponent<VerticalLayoutGroup>();

                                          moreDetailLayoutGroup.childControlHeight = true;
                                          moreDetailLayoutGroup.childControlWidth  = true;
                                          moreDetailLayoutGroup.padding            = new RectOffset(0, 0, 16, 0);

                                          var showOnlyNameToggle = RsUIBuilder
                                                                   .ToggleEntry(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                                                    .RS_ONLY_NULL_CHANNEL,
                                                                                ToolParameterMenu.ToggleState.Off,
                                                                                moreDetail)
                                                                   .GetComponentInChildren<MultiToggle>();

                                          // RsUIBuilder.BlockLine(moreDetail, 16);

                                          if (DlcManager.IsExpansion1Active()) {
                                              var showOnlyGlobalToggle = RsUIBuilder
                                                                         .ToggleEntry(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                                              .RS_ONLY_GLOBAL_CHANNEL,
                                                                          ToolParameterMenu.ToggleState.Off,
                                                                          moreDetail)
                                                                         .GetComponentInChildren<MultiToggle>();

                                              diagram.showOnlyGlobalToggle = showOnlyGlobalToggle;
                                          }

                                          RsUIBuilder.BlockLine(moreDetail, 16);

                                          var typeToggleGroup = root.AddComponent<RsMultiToggleGroupCom>();
                                          var multiToggles    = new List<MultiToggle>();
                                          multiToggles.AddRange(new[] {
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_ALL_PORT,
                                               0,
                                               moreDetail),
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_GAS_PORT,
                                               0,
                                               moreDetail),
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_LIQUID_PORT,
                                               0,
                                               moreDetail),
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_SOLID_PORT,
                                               0,
                                               moreDetail),
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_POWER_PORT,
                                               0,
                                               moreDetail),
                                              RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS.FILTERLAYERS
                                                   .RS_LOGIC_PORT,
                                               0,
                                               moreDetail)
                                          });

                                          if (DlcManager.IsExpansion1Active())
                                              multiToggles.Add(RsUIBuilder.ToggleEntryToMultiToggle(MYSTRINGS.UI.TOOLS
                                                                    .FILTERLAYERS.RS_HEP_PORT,
                                                                0,
                                                                moreDetail));

                                          typeToggleGroup.toggles = multiToggles.ToArray();

                                          diagram.lineToggleGroup        = lineToggleGroup;
                                          diagram.showOnlyNameToggle     = showOnlyNameToggle;
                                          diagram.showPriorityInfoToggle = showPriorityInfo;
                                          diagram.typeToggleGroup        = typeToggleGroup;
                                          diagram.moreDetailParent       = moreDetail;
                                          diagram.disableLineAnimToggle  = disableLineAnim;
                                          root.SetActive(true);

                                          return root;
                                      });
    }

    public void Start() {
        lineToggleGroup.onSelected += ToggleWiredPreviewMode;
        typeToggleGroup.onSelected += ToggleBuildingType;

        if (showOnlyNameToggle != null) showOnlyNameToggle.onClick += ToggleShowOnlyName;

        if (showOnlyGlobalToggle != null) showOnlyGlobalToggle.onClick += ToggleGlobalChannel;

        showPriorityInfoToggle.onClick += ToggleShowPriorityInfo;
        disableLineAnimToggle.onClick  += ToggleDisableLineAnim;
    }

    public void OnEnable() { UpdateState(); }

    private void Update() {
        if (OverlayScreen.Instance.mode == MyOverlayModes.PortChannel.ID)
            if (moreDetailParent.activeSelf == MyOverlayModes.PortChannel.IsActiveChannelStatus())
                UpdateState();
    }

    private void UpdateState() {
        if (OverlayScreen.Instance.mode == MyOverlayModes.PortChannel.ID) {
            lineToggleGroup.Select((int)MyOverlayModes.PortChannel.wiredPreviewMode);

            var activeChannelStatus = MyOverlayModes.PortChannel.IsActiveChannelStatus();
            moreDetailParent.SetActiveNR(!activeChannelStatus);

            showPriorityInfoToggle.ChangeState(MyOverlayModes.PortChannel.showPriorityInfo ? 1 : 0);
            disableLineAnimToggle.ChangeState(MyOverlayModes.PortChannel.disableLineAnim ? 1 : 0);
            if (!activeChannelStatus) {
                typeToggleGroup.Select((int)MyOverlayModes.PortChannel.buildingType);
                showOnlyNameToggle.ChangeState(MyOverlayModes.PortChannel.showOnlyNullChannel ? 1 : 0);
            }
        }
    }

    /// <summary>
    ///     切换连线预览方式
    /// </summary>
    private void ToggleWiredPreviewMode(int i) {
        MyOverlayModes.PortChannel.wiredPreviewMode = (MyOverlayModes.PortChannel.WiredPreviewMode)i;
    }

    private void ToggleBuildingType(int i) { MyOverlayModes.PortChannel.buildingType = (BuildingType)i; }

    private void ToggleShowOnlyName() {
        if (MyOverlayModes.PortChannel.showOnlyNullChannel) {
            MyOverlayModes.PortChannel.showOnlyNullChannel = false;
            showOnlyNameToggle.ChangeState(0);
        } else {
            MyOverlayModes.PortChannel.showOnlyNullChannel = true;
            showOnlyNameToggle.ChangeState(1);
        }
    }

    private void ToggleGlobalChannel() {
        if (MyOverlayModes.PortChannel.showOnlyGlobalChannel) {
            MyOverlayModes.PortChannel.showOnlyGlobalChannel = false;
            showOnlyGlobalToggle.ChangeState(0);
        } else {
            MyOverlayModes.PortChannel.showOnlyGlobalChannel = true;
            showOnlyGlobalToggle.ChangeState(1);
        }
    }

    private void ToggleShowPriorityInfo() {
        if (MyOverlayModes.PortChannel.showPriorityInfo) {
            MyOverlayModes.PortChannel.showPriorityInfo = false;
            showPriorityInfoToggle.ChangeState(0);
        } else {
            MyOverlayModes.PortChannel.showPriorityInfo = true;
            showPriorityInfoToggle.ChangeState(1);
        }
    }

    private void ToggleDisableLineAnim() {
        if (MyOverlayModes.PortChannel.disableLineAnim) {
            MyOverlayModes.PortChannel.disableLineAnim = false;
            disableLineAnimToggle.ChangeState(0);
        } else {
            MyOverlayModes.PortChannel.disableLineAnim = true;
            disableLineAnimToggle.ChangeState(1);
        }
    }
}