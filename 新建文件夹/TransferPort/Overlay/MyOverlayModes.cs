﻿using System.Collections.Generic;
using RsLib;
using RsLib.Adapter;
using RsLib.Collections;
using RsLib.Components;
using UnityEngine;

namespace RsTransferPort;

public class MyOverlayModes {
    public class PortChannel : OverlayModes.Mode {
        public enum WiredPreviewMode {
            None,
            Center,
            Nearby
        }

        public static readonly HashedString ID   = new(nameof(PortChannel));
        public static readonly string       Icon = "rs_overlay_port_channel";
        private static         bool         enableShowOneChannel;
        private static         bool         needRefresh = true;
        private static         bool         m_showOnlyNullChannel;
        private static         bool         m_showOnlyGlobalChannel;
        private static         bool         m_disableLineAnim;

        /// <summary>
        ///     显示优先级信息
        /// </summary>
        private static bool m_showPriorityInfo;

        private static BuildingType     m_buildingType;
        private static WiredPreviewMode m_wiredPreviewMode = WiredPreviewMode.Center;
        private static PortChannelKey   showOneChannelKey;

        private static readonly Dictionary<BuildingType, Color> BuildingTypeColors = new() {
            [BuildingType.None]   = Color.white,
            [BuildingType.Gas]    = new Color(0.44f, 0.96f, 0.16f),
            [BuildingType.Liquid] = new Color(0.16f, 0.65f, 0.97f),
            [BuildingType.Solid]  = new Color(0.95f, 0.55f, 0.18f),
            [BuildingType.Power]  = new Color(0.95f, 0.2f,  0.19f),
            [BuildingType.Logic]  = new Color(0.17f, 0.94f, 0.8f),
            [BuildingType.HEP]    = new Color(0.34f, 0.56f, 0.06f)
        };

        private static readonly Color[] Colors = {
            new(0.49f, 0.28f, 1f),    //530CC8
            new(1f, 0f, 0.02f),       //FF0006
            new(0.06f, 0.74f, 0.05f), //10BC0E
            new(0.31f, 1f, 0.1f),     //FED403
            new(1f, 0f, 0.57f),       //DF007D
            new(0.71f, 1f, 0.07f),    //98DC13
            new(0.03f, 0.99f, 1f),    //05969A
            new(1f, 0.66f, 0f),       //FEA900
            new(0.67f, 0.27f, 1f),    //8702CF
            new(0.8f, 1f, 0.19f),     //FFFF01
            new(0.03f, 0.32f, 1f),    //063FCA
            new(1f, 0.77f, 0.19f)     //FED403
        };

        private static readonly Color[] PriorityColors = {
            new(0.41f, 0.97f, 1f),    //#69F7FF
            new(0.17f, 0.79f, 0.99f), //#2BC9FC
            new(0.12f, 0.51f, 0.98f), //#1F81FB
            new(0.49f, 0.11f, 1f),    //#7C1BFF
            new(0.65f, 0.24f, 0.99f), //#A53CFD

            new(0.79f, 0.38f, 1f), //#C960FF
            new(1f, 0.41f, 0.42f), //#FE686A
            new(1f, 0.24f, 0.19f), //#FE3E30
            new(0.98f, 0.01f, 0f)  //#FB0200
        };

        private          int                                 activeWorldId = -1;
        private readonly GameObject                          arrowParent;
        private readonly int                                 cameraLayerMask;
        private readonly GameObject                          canvasParent;
        private readonly GameObject                          channelNameParent;
        private          int                                 colorIndex;
        private readonly LineList<GameObject>                filterChannels = new();
        private readonly ContrastSet<GameObject>             layoutTargets;
        private readonly RsHashUIPool<LineArrow>             lineArrowPool;
        private readonly UIPool<LineCenterImage>             lineCenterImagePool;
        private readonly GameObject                          lineCenterParent;
        private readonly RsHashUIPool<RsHierarchyReferences> namePool;
        private readonly int                                 objectTargetLayer;
        private readonly GameObject                          priorityParent;
        private readonly RsHashUIPool<PriorityImage>         priorityPool;
        private readonly int                                 selectionMask;

        /// <summary>
        ///     是否使用索引颜色
        /// </summary>
        private bool userIndexColor;

        public PortChannel(Canvas ___powerLabelParent) {
            var arrowPrefab = TransferPortMod.BodyAsset.lineArrow;

            namePool = new RsHashUIPool<RsHierarchyReferences>(TransferPortMod.BodyAsset.portChannelName);
            lineArrowPool = new RsHashUIPool<LineArrow>(arrowPrefab);
            priorityPool = new RsHashUIPool<PriorityImage>(RsResources.Load<PriorityImage>("prefabs/priority_image"));
            lineCenterImagePool = new UIPool<LineCenterImage>(TransferPortMod.BodyAsset.lineCenterImage);

            canvasParent      = ___powerLabelParent.gameObject;
            arrowParent       = CreateLayer("arrowParent",       canvasParent);
            channelNameParent = CreateLayer("channelNameParent", canvasParent);
            priorityParent    = CreateLayer("priorityParent",    canvasParent);
            lineCenterParent  = CreateLayer("lineCenterParent",  canvasParent);

            objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
            cameraLayerMask   = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
            selectionMask     = cameraLayerMask;
            layoutTargets = new ContrastSet<GameObject>(root => {
                                                            var position  = root.transform.GetPosition();
                                                            var num       = position.z;
                                                            var component = root.GetComponent<KPrefabID>();
                                                            if (component != null) {
                                                                if (component.HasTag(GameTags.OverlayInFrontOfConduits))
                                                                    num = Grid.GetLayerZ(ViewMode() ==
                                                                                  OverlayModes.LiquidConduits.ID
                                                                                  ? Grid.SceneLayer.LiquidConduits
                                                                                  : Grid.SceneLayer.GasConduits) -
                                                                          0.2f;
                                                                else if
                                                                    (component.HasTag(GameTags.OverlayBehindConduits))
                                                                    num = Grid.GetLayerZ(ViewMode() ==
                                                                                  OverlayModes.LiquidConduits.ID
                                                                                  ? Grid.SceneLayer.LiquidConduits
                                                                                  : Grid.SceneLayer.GasConduits) +
                                                                          0.2f;
                                                            }

                                                            position.z = num;
                                                            root.transform.SetPosition(position);
                                                            var animController
                                                                = root.GetComponent<KBatchedAnimController>();

                                                            if (animController != null)
                                                                animController.SetLayer(objectTargetLayer);
                                                        },
                                                        root => {
                                                            if (root == null) return;

                                                            var defaultDepth
                                                                = GetDefaultDepth(root.GetComponent<KMonoBehaviour>());

                                                            var position = root.transform.GetPosition();
                                                            position.z = defaultDepth;
                                                            root.transform.SetPosition(position);

                                                            var animController
                                                                = root.GetComponent<KBatchedAnimController>();

                                                            if (animController) ResetDisplayValues(animController);
                                                        });
        }

        public static bool showOnlyNullChannel {
            get => m_showOnlyNullChannel;
            set {
                needRefresh           = true;
                m_showOnlyNullChannel = value;
            }
        }

        public static bool showOnlyGlobalChannel {
            get => m_showOnlyGlobalChannel;
            set {
                needRefresh             = true;
                m_showOnlyGlobalChannel = value;
            }
        }

        public static BuildingType buildingType {
            get => m_buildingType;
            set {
                needRefresh    = true;
                m_buildingType = value;
            }
        }

        public static WiredPreviewMode wiredPreviewMode {
            get => m_wiredPreviewMode;
            set {
                needRefresh        = true;
                m_wiredPreviewMode = value;
            }
        }

        public static bool showPriorityInfo {
            get => m_showPriorityInfo;
            set {
                needRefresh        = true;
                m_showPriorityInfo = value;
            }
        }

        public static bool disableLineAnim {
            get => m_disableLineAnim;
            set {
                needRefresh       = true;
                m_disableLineAnim = value;
            }
        }

        public override HashedString ViewMode()     { return ID; }
        public override string       GetSoundName() { return "Disease"; }

        public override void Enable() {
            //关闭功能
            Camera.main.cullingMask |= cameraLayerMask;
            SelectTool.Instance.SetLayerMask(selectionMask);
            GridCompositor.Instance.ToggleMinor(false);

            //添加事件监听
            PortManager.Instance.OnChannelChange += OnChannelChange;
            needRefresh                          =  true;

            base.Enable();
        }

        public override void Disable() {
            namePool.ClearAll();
            lineArrowPool.ClearAll();
            priorityPool.ClearAll();
            filterChannels.Clear();
            layoutTargets.Clear();
            lineCenterImagePool.ClearAll();

            ClearActiveChannel();

            //恢复摄像机
            Camera.main.cullingMask &= ~ cameraLayerMask;
            SelectTool.Instance.ClearLayerMask();
            GridCompositor.Instance.ToggleMinor(false);
            PortManager.Instance.OnChannelChange -= OnChannelChange;
            base.Disable();
        }

        public void OnChannelChange(TransferPortChannel channel) { needRefresh = true; }

        public static void ActiveChannel(PortChannelKey channelKey) {
            showOneChannelKey    = channelKey;
            enableShowOneChannel = true;
            needRefresh          = true;
        }

        public static bool IsActiveChannelStatus() { return enableShowOneChannel; }

        public static void ClearActiveChannel() {
            enableShowOneChannel = false;
            showOneChannelKey    = default(PortChannelKey);
        }

        public override void Update() {
            base.Update();

            if (activeWorldId != ClusterManager.Instance.activeWorld.id) {
                needRefresh   = true;
                activeWorldId = ClusterManager.Instance.activeWorld.id;
            }

            if (needRefresh) {
                needRefresh = false;
                FilterChannel();
                UpdateColorMode();
                UpdateSort();
                UpdateArrow();
                UpdateLabel();
                UpdatePriority();
            }
        }

        public void UpdateColorMode() {
            userIndexColor = !IsActiveChannelStatus() && buildingType != BuildingType.None;
            colorIndex     = 0;
        }

        public void UpdateSort() {
            layoutTargets.StartRecord();
            foreach (var channel in filterChannels) {
                foreach (var go in channel) layoutTargets.Add(go);
            }

            layoutTargets.EndAndContrast();
        }

        public void UpdatePriority() {
            priorityPool.RecordStart();

            if (showPriorityInfo)
                foreach (var channel in filterChannels) {
                    foreach (var gameObject in channel) {
                        var portChannel = gameObject.GetComponent<TransferPortChannel>();
                        var priority    = portChannel.Priority;
                        if (MyUtils.IsUsePriority(portChannel.BuildingType)) {
                            var position = gameObject.transform.position + new Vector3(0, 0.5f, 0);
                            var pi       = priorityPool.GetFreeElement(gameObject, priorityParent, true);
                            pi.priority           = priority;
                            pi.transform.position = position;
                        }
                    }
                }

            priorityPool.ClearNoRecordElement();
        }

        private void FilterChannel() {
            filterChannels.Clear();

            FilterOneTypeChannel(activeWorldId, BuildingType.Gas);
            FilterOneTypeChannel(activeWorldId, BuildingType.Liquid);
            FilterOneTypeChannel(activeWorldId, BuildingType.Solid);
            FilterOneTypeChannel(activeWorldId, BuildingType.Power);
            FilterOneTypeChannel(activeWorldId, BuildingType.Logic);
            FilterOneTypeChannel(activeWorldId, BuildingType.HEP);
        }

        private void FilterOneTypeChannel(int activeWorldId, BuildingType buildingType) {
            if (NeedShowBuildingType(buildingType)) {
                var channels = PortManager.Instance.GetChannels(buildingType);
                foreach (var channel in channels)
                    if (NeedShowChannel(channel)) {
                        filterChannels.NextLine();
                        foreach (var obj in channel.all)
                            if (obj.GetMyWorldId() == activeWorldId)
                                filterChannels.Add(obj.gameObject);

                        filterChannels.PreviousLineIfEmpty();
                    }
            }
        }

        private bool NeedShowBuildingType(BuildingType _buildingType) {
            if (enableShowOneChannel) return showOneChannelKey.buildingType == _buildingType;

            if (buildingType == BuildingType.None) return true;

            return buildingType == _buildingType;
        }

        private bool NeedShowChannel(SingleChannelController controller) {
            if (enableShowOneChannel)
                return showOneChannelKey.worldId == controller.WorldIdAG &&
                       showOneChannelKey.name    == controller.ChannelName;

            if (showOnlyGlobalChannel)
                if (!controller.IsGlobal)
                    return false;

            if (showOnlyNullChannel)
                if (!controller.IsInvalid())
                    return false;

            return true;
        }

        private void UpdateLabel() {
            namePool.RecordStart();
            foreach (var channel in filterChannels) UpdateLabelFromChannel(channel);
            namePool.ClearNoRecordElement();
        }

        // private void UpdateLabel()
        // {
        //     namePool.RecordStart();
        //     foreach (var channel in filterChannels) UpdateLabelFromChannel(channel);
        //     namePool.ClearNoRecordElement();
        // }

        private void UpdateArrow() {
            lineCenterImagePool.ClearAll();
            lineArrowPool.RecordStart();
            foreach (IList<GameObject> channelObjects in filterChannels) {
                if (channelObjects.Count == 0) continue;

                var component = channelObjects[0].GetComponent<TransferPortChannel>();

                if (string.IsNullOrEmpty(component.ChannelName)) continue;

                Color color;

                //选取颜色
                if (userIndexColor) {
                    colorIndex = colorIndex % Colors.Length;
                    color      = Colors[colorIndex];
                    colorIndex++;
                } else
                    color = BuildingTypeColors[component.BuildingType];

                if (wiredPreviewMode == WiredPreviewMode.Center)
                    UpdateCenterPreview(channelObjects, component.BuildingType, component.ChannelName, color);
                else if (wiredPreviewMode == WiredPreviewMode.Nearby)
                    UpdateNearbyPreview(channelObjects, component.BuildingType, component.ChannelName, color);
            }

            lineArrowPool.ClearNoRecordElement();
        }

        private void UpdateLabelFromChannel(ICollection<GameObject> items) {
            foreach (var item in items) {
                var transferPortChannel = item.GetComponent<TransferPortChannel>();
                if (transferPortChannel == null) continue;

                RsHierarchyReferences label;

                if (namePool.GetFreeElement(item, out label, channelNameParent, true))
                    label.transform.SetPositionXY(item.transform.position);

                // label.GetReference("Icon").SetActiveNR(transferPortChannel.IsGlobal);
                label.GetReference("IconRoot").SetActiveNR(transferPortChannel.IsGlobal);
                label.GetReference<LocTextAdapter>("Name").SetTextNoRepeat(transferPortChannel.DisplayChannelName);

                // label.transform.SetAsLastSibling();
            }
        }

        private void UpdateNearbyPreview(IList<GameObject> items,
                                         BuildingType      _buildingType,
                                         string            channelName,
                                         Color             color) {
            if (items.Count == 0) return;

            RsUtil.NearestSort(items);

            for (var i = 0; i < items.Count - 1; i++) {
                var lineArrow = lineArrowPool.GetFreeElement(items[i], arrowParent, true);
                lineArrow.transform.SetAsLastSibling();
                lineArrow.SetTwoPoint((Vector2)items[i].transform.position     + new Vector2(0, 0.5f),
                                      (Vector2)items[i + 1].transform.position + new Vector2(0, 0.5f));

                lineArrow.EnableAnim = !disableLineAnim;
                lineArrow.SetColor(color);
            }
        }

        private void UpdateCenterPreview(IList<GameObject> items,
                                         BuildingType      _buildingType,
                                         string            channelName,
                                         Color             color) {
            if (items.Count < 2) return;

            var center = items.Center();
            center.y += 0.5f;

            foreach (var item in items) {
                var transferPortChannel = item.GetComponent<TransferPortChannel>();
                var endPos              = item.transform.position;
                if (transferPortChannel == null) continue;

                var lineArrow = lineArrowPool.GetFreeElement(item, arrowParent, true);
                lineArrow.transform.SetAsLastSibling();
                endPos.y += 0.5f;
                if (transferPortChannel.InOutType == InOutType.Receiver)
                    lineArrow.SetTwoPoint(center, endPos);
                else
                    lineArrow.SetTwoPoint(endPos, center);

                lineArrow.EnableAnim = !disableLineAnim;

                if (showPriorityInfo && MyUtils.IsUsePriority(_buildingType)) {
                    var priority = Mathf.Clamp(transferPortChannel.Priority, 1, 9);
                    lineArrow.SetColor(PriorityColors[priority - 1]);
                } else
                    lineArrow.SetColor(color);
            }

            //绘制中心点
            var centerImage = lineCenterImagePool.GetFreeElement(lineCenterParent, true);
            centerImage.transform.SetAsLastSibling();
            centerImage.SetImage(_buildingType);
            centerImage.SetColor(color);
            centerImage.transform.position = center;
        }

        private GameObject CreateLayer(string name, GameObject parent) {
            var gameObject  = RsUIBuilder.UIGameObject(name, parent);
            var canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.interactable   = false;
            canvasGroup.blocksRaycasts = false;
            return gameObject;
        }

        public static class FILTERLAYERS {
            public const string Gas    = "RS_GAS_PORT";
            public const string Liquid = "RS_LIQUID_PORT";
            public const string Solid  = "RS_SOLID_PORT";
            public const string Power  = "RS_POWER_PORT";
            public const string Logic  = "RS_LOGIC_PORT";
            public const string HEP    = "RS_HEP_PORT";
        }
    }
}