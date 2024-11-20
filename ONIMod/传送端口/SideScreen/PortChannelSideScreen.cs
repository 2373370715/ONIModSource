using System.Linq;
using System.Text;
using RsLib;
using RsLib.Adapter;
using RsLib.Components;
using UnityEngine;
using UnityEngine.UI;
using OUI = STRINGS.UI;

namespace RsTransferPort;

public class PortChannelSideScreen : SideScreenContent {
    [SerializeField]
    private MultiToggleAdapter batchRenameToggle;

    [SerializeField]
    private CandidateNameScreen candidateNameScreenPrefab;

    [SerializeField]
    private KInputTextFieldAdapter channelNameInputField;

    private int detailLevel;

    [SerializeField]
    private MultiToggleAdapter detailLevelToggle;

    [SerializeField]
    private MultiToggleAdapter globalToggle;

    [SerializeField]
    private LocTextAdapter headerLabel;

    [SerializeField]
    private LocTextAdapter infoLocTextPrefab;

    private bool isBatchMode;
    private bool isOpenCandidateName;

    [SerializeField]
    private GameObject listContainer;

    private RsHashUIPool<LocTextAdapter> locTextPool;
    private bool                         needRefresh = true;

    [SerializeField]
    private MultiToggleAdapter openCandidateNameToggle;

    [SerializeField]
    private PriorityBar priorityBar;

    private readonly RsInterval refreshInterval = new(1);

    [SerializeField]
    private RsHierarchyReferences row1Prefab;

    [SerializeField]
    private RsHierarchyReferences row2Prefab;

    // private RsHashUIPool<RsHierarchyReferences> rowPool1;
    private RsHashUIPool<RsHierarchyReferences> rowPool2;
    private TransferPortChannel                 target;

    [SerializeField]
    private LocTextAdapter warningLabel;

    private RsHashUIPool<RsHierarchyReferences> worldInfoPool;

    [SerializeField]
    private RsHierarchyReferences worldInfoPrefab;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        activateOnSpawn = true;

        // rowPool1 = new RsHashUIPool<RsHierarchyReferences>(row1Prefab);
        rowPool2      = new RsHashUIPool<RsHierarchyReferences>(row2Prefab);
        locTextPool   = new RsHashUIPool<LocTextAdapter>(infoLocTextPrefab);
        worldInfoPool = new RsHashUIPool<RsHierarchyReferences>(worldInfoPrefab);

        detailLevelToggle.onClick       = OnDetailLevelToggleClick;
        batchRenameToggle.onClick       = OnBatchRenameClick;
        openCandidateNameToggle.onClick = OnCandidateNameToggleClick;
        globalToggle.onClick            = OnGlobalToggleClick;

        channelNameInputField.onSelect.AddListener(OnChangeNameEditStart);
        channelNameInputField.onEndEdit.AddListener(OnChangeNameEditEnd);
        priorityBar.OnPriorityClick = OnPriorityClick;
        priorityBar.Help.OnToolTip  = OnPriorityBarHelp;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        headerLabel.SetText(MYSTRINGS.UI.SIDESCREEN.RS_PORT_CHANNEL.CHANNEL_NAME);
        PortManager.Instance.OnChannelChange += channel => { Refresh(); };

        globalToggle.gameObject.SetActiveNR(DlcManager.IsExpansion1Active());
    }

    private void OnChangeNameEditStart(string text) { isEditing = true; }

    private void OnChangeNameEditEnd(string text) {
        isEditing = false;
        if (target != null) ChangeName(text);

        channelNameInputField.text = target.ChannelName;
    }

    protected void ChangeName(string str) {
        if (isBatchMode) {
            PortManager.Instance.BatchChange(target.ChannelController, str, target.IsGlobal);
            SetBatchRenameState(false);
            return;
        }

        if (!RsUtil.IsNullOrDestroyed(target)) target.CheckSetChannelName(str);
    }

    protected void SetBatchRenameState(bool enable) {
        isBatchMode = enable;
        batchRenameToggle.ChangeState(isBatchMode ? 1 : 0);
        if (isBatchMode) {
            warningLabel.gameObject.SetActiveNR(true);
            warningLabel.SetTextNoRepeat(MYSTRINGS.UI.SIDESCREEN.RS_PORT_CHANNEL.WARIN_BATCH_MODE);
        } else
            warningLabel.gameObject.SetActiveNR(false);
    }

    /// <summary>
    ///     详细等级
    /// </summary>
    private void OnDetailLevelToggleClick() {
        if (target != null) {
            detailLevel = ++detailLevel % 2;
            Refresh();
        }
    }

    private void OnBatchRenameClick() { SetBatchRenameState(!isBatchMode); }

    private void OnCandidateNameToggleClick() {
        isOpenCandidateName = !isOpenCandidateName;
        RefreshCandidateNameState();
    }

    private void OnGlobalToggleClick() {
        if (target != null) {
            if (isBatchMode) {
                PortManager.Instance.BatchChange(target.ChannelController, target.ChannelName, !target.IsGlobal);
                SetBatchRenameState(false);
            } else
                target.CheckSetGlobal(!target.IsGlobal);

            globalToggle.ChangeState(target.IsGlobal ? 1 : 0);
            Refresh();
        }
    }

    private void RefreshCandidateNameState() {
        openCandidateNameToggle.ChangeState(isOpenCandidateName ? 1 : 0);
        if (DetailsScreen.Instance == null) return;

        if (isOpenCandidateName) {
            var sideScreen
                = (CandidateNameScreen)DetailsScreen.Instance.SetSecondarySideScreen(candidateNameScreenPrefab,
                 MYSTRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.TITLE);

            sideScreen.selected = ChangeName;
            sideScreen.SwitchCandidate(target.BuildingType);
        } else
            DetailsScreen.Instance.ClearSecondarySideScreen();
    }

    public override bool IsValidForTarget(GameObject target) {
        return target.GetComponent<TransferPortChannel>() != null;
    }

    public override void SetTarget(GameObject new_target) {
        if (RsUtil.IsNullOrDestroyed(new_target)) return;

        target = new_target.GetComponent<TransferPortChannel>();
        RefreshCandidateNameState();
        SetBatchRenameState(isBatchMode);
        Refresh();
    }

    public override void ClearTarget() {
        target      = null;
        isBatchMode = false;
        if (DetailsScreen.Instance != null) DetailsScreen.Instance.ClearSecondarySideScreen();
    }

    protected override void OnDisable() {
        base.OnDisable();
        isBatchMode = false;
    }

    private void LateUpdate() {
        if (refreshInterval.Update(Time.unscaledDeltaTime)) needRefresh = true;

        if (needRefresh) ImmediateRefresh();
    }

    private void Refresh() { needRefresh = true; }

    private void ImmediateRefresh() {
        needRefresh = false;
        if (target == null) return;

        //更新名称
        if (isEditing == false) channelNameInputField.text = target.ChannelName;

        RefreshPriority();

        batchRenameToggle.ChangeState(isBatchMode ? 1 : 0);
        detailLevelToggle.ChangeState(detailLevel);
        globalToggle.ChangeState(target.IsGlobal ? 1 : 0);

        var controllers = PortManager.Instance.GetChannels(target.BuildingType, target.WorldIdAG, true);

        detailLevelToggle.gameObject.SetActiveNR(true);

        rowPool2.RecordStart();
        locTextPool.RecordStart();
        worldInfoPool.RecordStart();

        foreach (var controller in controllers) {
            var row = rowPool2.GetFreeElement(controller, listContainer, true);
            row.transform.SetAsLastSibling();

            var channelNameLocText = row.GetReference<LocTextAdapter>("ChannelName");
            channelNameLocText.SetTextNoRepeat(controller.DisplayChannelName);

            var toggle = row.GetComponent<MultiToggle>();
            toggle.ChangeState(target.HasChannel(controller) ? 1 : 0);
            toggle.onClick = delegate { target.SetChannel(controller); };
            var globalIcon = row.GetReference("GlobalIcon");
            globalIcon.SetActiveNR(controller.IsGlobal);

            var num1 = row.GetReference<LocTextAdapter>("Num1");
            num1.gameObject.SetActiveNR(true);
            if (controller.BuildingType == BuildingType.Power)
                num1.SetTextNoRepeat("N:" + controller.senders.Count);
            else
                num1.SetTextNoRepeat("S:" + controller.senders.Count);

            var num2 = row.GetReference<LocTextAdapter>("Num2");
            if (controller.BuildingType == BuildingType.Power)
                num2.gameObject.SetActiveNR(false);
            else {
                num2.gameObject.SetActiveNR(true);
                num2.SetTextNoRepeat("R:" + controller.receivers.Count);
            }

            if (detailLevel == 1 && controller.BuildingType == BuildingType.Power && !controller.IsInvalid())
                RefreshPowerInfo(row, controller);

            if (detailLevel == 1 && controller.IsGlobal && DlcManager.IsExpansion1Installed())
                RefreshWorldListInfo(row, controller);
        }

        worldInfoPool.ClearNoRecordElement();
        locTextPool.ClearNoRecordElement();
        rowPool2.ClearNoRecordElement();
    }

    private void RefreshWorldListInfo(RsHierarchyReferences row, SingleChannelController controller) {
        var infoList   = row.GetReference("info");
        var containers = controller.GetIncludeWorldContainer().ToArray();
        for (var i = 0; i < containers.Length; i++) {
            var world   = containers[i];
            var element = worldInfoPool.GetFreeElement(infoList.GetHashCode() | i, infoList, true);
            element.transform.SetAsLastSibling();
            var image = element.GetReference<Image>("WorldIcon");

            if (world.IsModuleInterior) {
                image.sprite = global::Assets.GetSprite((HashedString)"icon_category_rocketry");
                image.color  = Color.white;
                var name = world.GetComponent<Clustercraft>()?.Name ?? "unknown";
                element.GetReference<LocTextAdapter>("Name").SetTextNoRepeat(name);
            } else {
                image.sprite = Def.GetUISprite(world.GetComponent<ClusterGridEntity>()).first;
                image.color  = Def.GetUISprite(world.GetComponent<ClusterGridEntity>()).second;
                var name = world.GetComponent<AsteroidGridEntity>()?.Name ?? "unknown";
                element.GetReference<LocTextAdapter>("Name").SetTextNoRepeat(name);
            }
        }
    }

    private void RefreshPowerInfo(RsHierarchyReferences row, SingleChannelController controller) {
        var infoList = row.GetReference("info");
        infoList.SetActiveNR(true);
        var circuitManager = Game.Instance.circuitManager;
        var circuitID      = circuitManager.GetVirtualCircuitID(controller);
        if (circuitID == ushort.MaxValue) return;

        var label1 = locTextPool.GetFreeElement(infoList.GetHashCode() | 1, infoList, true);
        label1.transform.SetAsLastSibling();
        label1.SetTextNoRepeat(string.Format(OUI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES,
                                             GameUtil.GetFormattedJoules(circuitManager
                                                                             .GetJoulesAvailableOnCircuit(circuitID))));

        // label1.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES_TOOLTIP;

        var generatedByCircuit1 = circuitManager.GetWattsGeneratedByCircuit(circuitID);
        var generatedByCircuit2 = circuitManager.GetPotentialWattsGeneratedByCircuit(circuitID);
        var label2              = locTextPool.GetFreeElement(infoList.GetHashCode() | 2, infoList, true);
        label2.transform.SetAsLastSibling();
        var str = generatedByCircuit1 != (double)generatedByCircuit2
                      ? string.Format("{0} / {1}",
                                      GameUtil.GetFormattedWattage(generatedByCircuit1),
                                      GameUtil.GetFormattedWattage(generatedByCircuit2))
                      : GameUtil.GetFormattedWattage(generatedByCircuit1);

        label2.SetTextNoRepeat(string.Format(OUI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED, str));

        // label2.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED_TOOLTIP;

        var label3 = locTextPool.GetFreeElement(infoList.GetHashCode() | 3, infoList, true);
        label3.transform.SetAsLastSibling();
        label3.SetTextNoRepeat(string.Format(OUI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED,
                                             GameUtil.GetFormattedWattage(circuitManager
                                                                              .GetWattsUsedByCircuit(circuitID))));

        // label3.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED_TOOLTIP;
        var label4 = locTextPool.GetFreeElement(infoList.GetHashCode() | 4, infoList, true);
        label4.transform.SetAsLastSibling();
        label4.SetTextNoRepeat(string.Format(OUI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED,
                                             GameUtil.GetFormattedWattage(circuitManager
                                                                              .GetWattsNeededWhenActive(circuitID))));

        // label4.GetComponent<ToolTip>().toolTip = (string) OUI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED_TOOLTIP;

        var label5 = locTextPool.GetFreeElement(infoList.GetHashCode() | 5, infoList, true);
        label5.transform.SetAsLastSibling();
        label5.SetTextNoRepeat(string.Format(OUI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE,
                                             GameUtil.GetFormattedWattage(circuitManager
                                                                              .GetMaxSafeWattageForCircuit(circuitID))));

        // label5.GetComponent<ToolTip>().toolTip = (string) UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE_TOOLTIP;
    }

    #region 优先度

    private string OnPriorityBarHelp() {
        if (target.BuildingType == BuildingType.Gas    ||
            target.BuildingType == BuildingType.Liquid ||
            target.BuildingType == BuildingType.Solid)
            if (target.ChannelController is TransferConduitChannel conduitChannel) {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(MYSTRINGS.UI.SIDESCREEN.RS_PORT_CHANNEL.PRIORITY_TOOLTIP);
                stringBuilder.AppendLine();
                for (var priority = 9; priority >= 0; priority--) {
                    var senderPriorityInfo   = conduitChannel.senderPriorityList.GetByPriority(priority);
                    var receiverPriorityInfo = conduitChannel.receiverPriorityList.GetByPriority(priority);

                    var senderNum   = senderPriorityInfo   == null ? 0 : senderPriorityInfo.items.Count;
                    var receiverNum = receiverPriorityInfo == null ? 0 : receiverPriorityInfo.items.Count;

                    if (senderNum != 0 || receiverNum != 0) {
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat(MYSTRINGS.UI.SIDESCREEN.RS_PORT_CHANNEL.PRIORITY_LINE_INFO,
                                                   priority,
                                                   senderNum,
                                                   receiverNum);
                    }
                }

                return stringBuilder.ToString();
            }

        return "无";
    }

    private void OnPriorityClick(int priority) {
        if (isBatchMode) {
            PortManager.Instance.BatchChangePriority(target.ChannelController, priority);
            SetBatchRenameState(false);
        } else
            target.CheckSetPriority(priority);
    }

    private void RefreshPriority() {
        if (RsUtil.IsNullOrDestroyed(target)) return;

        if (target.BuildingType == BuildingType.Gas    ||
            target.BuildingType == BuildingType.Liquid ||
            target.BuildingType == BuildingType.Solid) {
            if (target.ChannelController is TransferConduitChannel conduitChannel) {
                priorityBar.gameObject.SetActiveNR(true);

                //重置优先度
                priorityBar.SetAllStateCache(0);

                //设置其它的优先度
                priorityBar.SetStateCacheRange(conduitChannel.senderPriorityList.AllPriority(),   2);
                priorityBar.SetStateCacheRange(conduitChannel.receiverPriorityList.AllPriority(), 2);

                //设置目标优先度
                priorityBar.SetStateCache(target.Priority, 1);

                //应用
                priorityBar.ApplyStateCache();
            }
        } else
            priorityBar.gameObject.SetActiveNR(false);
    }

    #endregion
}