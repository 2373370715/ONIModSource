using System;
using System.Collections.Generic;
using RsLib;
using RsLib.Adapter;
using RsLib.Components;
using RsTransferPort;
using UnityEngine;



// 定义一个候选名称屏幕类，继承自KScreen
public class CandidateNameScreen : KScreen {
    // 静态变量，用于标记是否已初始化
    private static bool initialized;

    // 候选名称字典，存储不同建筑类型的候选名称数组
    private static readonly Dictionary<BuildingType, string[]> candidateNameMap = new() {
        [BuildingType.Gas] = new[] {
            "GAS_0",
            "GAS_1",
            "GAS_2",
            "GAS_3",
            "GAS_4",
            "GAS_5",
            "GAS_6",
            "GAS_7"
        },
        [BuildingType.Liquid] = new[] {
            "LIQUID_0",
            "LIQUID_1",
            "LIQUID_2",
            "LIQUID_3",
            "LIQUID_4",
            "LIQUID_5",
            "LIQUID_6",
            "LIQUID_7",
            "LIQUID_8",
            "LIQUID_9",
            "LIQUID_10",
            "LIQUID_11",
            "LIQUID_12"
        },
        [BuildingType.Solid] = new[] {
            "SOLID_0",
            "SOLID_1",
            "SOLID_2",
            "SOLID_3",
            "SOLID_4",
            "SOLID_5",
            "SOLID_6",
            "SOLID_7",
            "SOLID_8",
            "SOLID_9"
        },
        [BuildingType.Power] = new[] { "POWER_0", "POWER_1", "POWER_2", "POWER_3", "POWER_4", "POWER_5" },
        [BuildingType.Logic] = new[] { "LOGIC_0", "LOGIC_1", "LOGIC_2", "LOGIC_3", "LOGIC_4" },
        [BuildingType.HEP]   = new[] { "HEP_0", "HEP_1", "HEP_2", "HEP_3", "HEP_4", "HEP_5" }
    };

    // 当前建筑类型
    private BuildingType currentBuildingType;

    // 列表容器游戏对象
    [SerializeField]
    protected GameObject listContainer;

    // 供应状态和温度状态变量
    private int                                 m_supplyState;
    private int                                 m_temperatureState;
    // 管理候选名称行的复用
    private RsHashUIPool<RsHierarchyReferences> rowPool;

    // 行预设
    [SerializeField]
    protected RsHierarchyReferences rowPrefab;

    // 选中候选名称时触发的动作
    public Action<string> selected;

    // 供应和温度切换适配器
    [SerializeField]
    protected MultiToggleAdapter supplyToggle;

    [SerializeField]
    protected MultiToggleAdapter temperatureToggle;

    // 供应状态属性，0代表无，1代表供应，2代表回收
    private int supplyState {
        get => m_supplyState;
        set {
            m_supplyState = value % 3;
            if (!RsUtil.IsNullOrDestroyed(supplyToggle)) {
                supplyToggle.ChangeState(m_supplyState);
                supplyToggle.FindOrAddComponent<ToolTip>().toolTip
                    = Strings.Get("STRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.SUPPLY_STATE_" + m_temperatureState);
            }
        }
    }

    // 温度状态属性，0代表无，1代表低温，2代表高温
    private int temperatureState {
        get => m_temperatureState;
        set {
            m_temperatureState = value % 3;
            if (!RsUtil.IsNullOrDestroyed(supplyToggle)) {
                temperatureToggle.ChangeState(m_temperatureState);
                temperatureToggle.FindOrAddComponent<ToolTip>().toolTip
                    = Strings.Get("STRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.TEMPERATURE_STATE_" + m_temperatureState);
            }
        }
    }

    // 预制初始化方法
    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        rowPool = new RsHashUIPool<RsHierarchyReferences>(rowPrefab);
        if (supplyToggle == null || temperatureToggle == null) {
            Debug.LogWarning("supplyToggle or temperatureToggle is null");
            return;
        }

        supplyState = 0;
        supplyToggle.onClick = delegate {
                                   supplyState = ++supplyState % 3;
                                   supplyToggle.ChangeState(supplyState);
                                   Refresh();
                               };

        temperatureState = 0;
        temperatureToggle.onClick = delegate {
                                        temperatureState = ++temperatureState % 3;
                                        temperatureToggle.ChangeState(temperatureState);
                                        Refresh();
                                    };

        if (!initialized) {
            foreach (var keyValuePair in candidateNameMap) {
                var names = keyValuePair.Value;
                for (var i = 0; i < names.Length; i++)
                    names[i] = Strings.Get("STRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.LABELS." + names[i]);
            }

            initialized = true;
        }
    }

    // 切换候选建筑类型方法
    public void SwitchCandidate(BuildingType buildingType) {
        if (currentBuildingType != buildingType) {
            currentBuildingType = buildingType;
            temperatureState    = 0;
            supplyState         = 0;
            Refresh();
        }
    }

    // 刷新候选名称列表方法
    private void Refresh() {
        if (currentBuildingType == BuildingType.None || !candidateNameMap.ContainsKey(currentBuildingType)) {
            rowPool.ClearAll();
            return;
        }

        var candidateName = candidateNameMap[currentBuildingType];

        rowPool.RecordStart();
        foreach (var sName in candidateName) {
            var name = sName;

            // 根据供应状态生成名称
            if (supplyState == 1)
                name                        = name + MYSTRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.S_NAMES.SUPPLY;
            else if (supplyState == 2) name = name + MYSTRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.S_NAMES.RECYCLE;

            // 根据温度状态生成名称
            if (temperatureState == 1)
                name = MYSTRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.S_NAMES.LOW_TEMPERATURE + name;
            else if (temperatureState == 2)
                name = MYSTRINGS.UI.SIDESCREEN.RS_CANDIDATE_NAME.S_NAMES.HIGH_TEMPERATURE + name;

            var references = rowPool.GetFreeElement(name, listContainer, true);
            references.transform.SetAsLastSibling();

            var textAdapter = references.GetReference<LocTextAdapter>("ChannelName");
            textAdapter.SetTextNoRepeat(name);

            var toggle = references.GetComponent<MultiToggle>();
            toggle.onClick = () => OnRowClick(name);
        }

        rowPool.ClearNoRecordElement();
    }

    // 行点击事件处理方法
    private void OnRowClick(string name) { selected?.Invoke(name); }
}
