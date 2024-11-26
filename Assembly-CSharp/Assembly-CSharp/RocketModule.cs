using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/RocketModule")]
public class RocketModule : KMonoBehaviour {
    public static readonly Operational.Flag landedFlag
        = new Operational.Flag("landed", Operational.Flag.Type.Requirement);

    private static readonly EventSystem.IntraObjectHandler<RocketModule> DEBUG_OnDestroyDelegate
        = new EventSystem.IntraObjectHandler<RocketModule>(delegate(RocketModule component, object data) {
                                                               component.DEBUG_OnDestroy(data);
                                                           });

    private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketOnGroundTagDelegate
        = GameUtil.CreateHasTagHandler(GameTags.RocketOnGround,
                                       delegate(RocketModule component, object data) {
                                           component.OnRocketOnGroundTag(data);
                                       });

    private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketNotOnGroundTagDelegate
        = GameUtil.CreateHasTagHandler(GameTags.RocketNotOnGround,
                                       delegate(RocketModule component, object data) {
                                           component.OnRocketNotOnGroundTag(data);
                                       });

    [SerializeField]
    private KAnimFile bgAnimFile;

    public LaunchConditionManager conditionManager;

    public Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleConditions
        = new Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>();

    public           bool   operationalLandedRequired    = true;
    protected        string parentRocketName             = UI.STARMAP.DEFAULT_NAME;
    private readonly string rocket_module_bg_affix       = "BG";
    private readonly string rocket_module_bg_anim        = "on";
    private readonly string rocket_module_bg_base_string = "{0}{1}";

    public ProcessCondition AddModuleCondition(ProcessCondition.ProcessConditionType conditionType,
                                               ProcessCondition                      condition) {
        if (!moduleConditions.ContainsKey(conditionType))
            moduleConditions.Add(conditionType, new List<ProcessCondition>());

        if (!moduleConditions[conditionType].Contains(condition)) moduleConditions[conditionType].Add(condition);
        return condition;
    }

    public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType) {
        var list = new List<ProcessCondition>();
        if (conditionType == ProcessCondition.ProcessConditionType.All)
            using (var enumerator = moduleConditions.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var keyValuePair = enumerator.Current;
                    list.AddRange(keyValuePair.Value);
                }

                return list;
            }

        if (moduleConditions.ContainsKey(conditionType)) list = moduleConditions[conditionType];
        return list;
    }

    public void SetBGKAnim(KAnimFile anim_file) { bgAnimFile = anim_file; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        GameUtil.SubscribeToTags(this, OnRocketOnGroundTagDelegate,    false);
        GameUtil.SubscribeToTags(this, OnRocketNotOnGroundTagDelegate, false);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        if (!DlcManager.FeatureClusterSpaceEnabled()) {
            conditionManager = FindLaunchConditionManager();
            var spacecraftFromLaunchConditionManager
                = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(conditionManager);

            if (spacecraftFromLaunchConditionManager != null)
                SetParentRocketName(spacecraftFromLaunchConditionManager.GetRocketName());

            RegisterWithConditionManager();
        }

        var component = GetComponent<KSelectable>();
        if (component != null) component.AddStatusItem(Db.Get().BuildingStatusItems.RocketName, this);
        Subscribe(1502190696, DEBUG_OnDestroyDelegate);
        FixSorting();
        var component2 = GetComponent<AttachableBuilding>();
        component2.onAttachmentNetworkChanged
            = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged,
                                               new Action<object>(OnAttachmentNetworkChanged));

        if (bgAnimFile != null) AddBGGantry();
    }

    public void FixSorting() {
        var num       = 0;
        var component = GetComponent<AttachableBuilding>();
        while (component != null) {
            var attachedTo = component.GetAttachedTo();
            if (!(attachedTo != null)) break;

            component = attachedTo.GetComponent<AttachableBuilding>();
            num++;
        }

        var localPosition = transform.GetLocalPosition();
        localPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Building) - num * 0.01f;
        transform.SetLocalPosition(localPosition);
        var component2 = GetComponent<KBatchedAnimController>();
        if (component2.enabled) {
            component2.enabled = false;
            component2.enabled = true;
        }
    }

    private void OnAttachmentNetworkChanged(object ab) { FixSorting(); }

    private void AddBGGantry() {
        var component  = GetComponent<KAnimControllerBase>();
        var gameObject = new GameObject();
        gameObject.name = string.Format(rocket_module_bg_base_string, name, rocket_module_bg_affix);
        gameObject.SetActive(false);
        var position = component.transform.GetPosition();
        position.z = Grid.GetLayerZ(Grid.SceneLayer.InteriorWall);
        gameObject.transform.SetPosition(position);
        gameObject.transform.parent = transform;
        var kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
        kbatchedAnimController.AnimFiles   = new[] { bgAnimFile };
        kbatchedAnimController.initialAnim = rocket_module_bg_anim;
        kbatchedAnimController.fgLayer     = Grid.SceneLayer.NoLayer;
        kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
        kbatchedAnimController.FlipX       = component.FlipX;
        kbatchedAnimController.FlipY       = component.FlipY;
        gameObject.SetActive(true);
    }

    private void DEBUG_OnDestroy(object data) {
        if (conditionManager != null && !App.IsExiting && !isLoadingScene) {
            var spacecraftFromLaunchConditionManager
                = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(conditionManager);

            conditionManager.DEBUG_TraceModuleDestruction(name,
                                                          spacecraftFromLaunchConditionManager == null
                                                              ? "null spacecraft"
                                                              : spacecraftFromLaunchConditionManager.state.ToString(),
                                                          new StackTrace(true).ToString());
        }
    }

    private void OnRocketOnGroundTag(object data) {
        RegisterComponents();
        var component = GetComponent<Operational>();
        if (operationalLandedRequired && component != null) component.SetFlag(landedFlag, true);
    }

    private void OnRocketNotOnGroundTag(object data) {
        DeregisterComponents();
        var component = GetComponent<Operational>();
        if (operationalLandedRequired && component != null) component.SetFlag(landedFlag, false);
    }

    public void DeregisterComponents() {
        var component = GetComponent<KSelectable>();
        component.IsSelectable = false;
        var component2 = GetComponent<BuildingComplete>();
        if (component2                   != null) component2.UpdatePosition();
        if (SelectTool.Instance.selected == component) SelectTool.Instance.Select(null);
        var component3 = GetComponent<Deconstructable>();
        if (component3 != null) component3.SetAllowDeconstruction(false);
        var handle = GameComps.StructureTemperatures.GetHandle(gameObject);
        if (handle.IsValid()) GameComps.StructureTemperatures.Disable(handle);
        var component4 = GetComponent<FakeFloorAdder>();
        if (component4 != null) component4.SetFloor(false);
        var component5 = GetComponent<AccessControl>();
        if (component5 != null) component5.SetRegistered(false);
        foreach (var manualDeliveryKG in GetComponents<ManualDeliveryKG>()) {
            DebugUtil.DevAssert(!manualDeliveryKG.IsPaused,
                                "RocketModule ManualDeliver chore was already paused, when this rocket lands it will re-enable it.");

            manualDeliveryKG.Pause(true, "Rocket heading to space");
        }

        var components2 = GetComponents<BuildingConduitEndpoints>();
        for (var i = 0; i < components2.Length; i++) components2[i].RemoveEndPoint();
        var component6 = GetComponent<ReorderableBuilding>();
        if (component6 != null) component6.ShowReorderArm(false);
        var component7 = GetComponent<Workable>();
        if (component7 != null) component7.RefreshReachability();
        var component8 = GetComponent<Structure>();
        if (component8 != null) component8.UpdatePosition();
        var component9 = GetComponent<WireUtilitySemiVirtualNetworkLink>();
        if (component9 != null) component9.SetLinkConnected(false);
        var component10 = GetComponent<PartialLightBlocking>();
        if (component10 != null) component10.ClearLightBlocking();
    }

    public void RegisterComponents() {
        GetComponent<KSelectable>().IsSelectable = true;
        var component = GetComponent<BuildingComplete>();
        if (component != null) component.UpdatePosition();
        var component2 = GetComponent<Deconstructable>();
        if (component2 != null) component2.SetAllowDeconstruction(true);
        var handle = GameComps.StructureTemperatures.GetHandle(gameObject);
        if (handle.IsValid()) GameComps.StructureTemperatures.Enable(handle);
        var components = GetComponents<Storage>();
        for (var i = 0; i < components.Length; i++) components[i].UpdateStoredItemCachedCells();
        var component3 = GetComponent<FakeFloorAdder>();
        if (component3 != null) component3.SetFloor(true);
        var component4 = GetComponent<AccessControl>();
        if (component4 != null) component4.SetRegistered(true);
        var components2 = GetComponents<ManualDeliveryKG>();
        for (var i = 0; i < components2.Length; i++) components2[i].Pause(false, "Landing on world");
        var components3 = GetComponents<BuildingConduitEndpoints>();
        for (var i = 0; i < components3.Length; i++) components3[i].AddEndpoint();
        var component5 = GetComponent<ReorderableBuilding>();
        if (component5 != null) component5.ShowReorderArm(true);
        var component6 = GetComponent<Workable>();
        if (component6 != null) component6.RefreshReachability();
        var component7 = GetComponent<Structure>();
        if (component7 != null) component7.UpdatePosition();
        var component8 = GetComponent<WireUtilitySemiVirtualNetworkLink>();
        if (component8 != null) component8.SetLinkConnected(true);
        var component9 = GetComponent<PartialLightBlocking>();
        if (component9 != null) component9.SetLightBlocking();
    }

    private void ToggleComponent(Type cmpType, bool enabled) {
        var monoBehaviour                                = (MonoBehaviour)GetComponent(cmpType);
        if (monoBehaviour != null) monoBehaviour.enabled = enabled;
    }

    public void RegisterWithConditionManager() {
        Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
        if (conditionManager != null) conditionManager.RegisterRocketModule(this);
    }

    protected override void OnCleanUp() {
        if (conditionManager != null) conditionManager.UnregisterRocketModule(this);
        base.OnCleanUp();
    }

    public virtual LaunchConditionManager FindLaunchConditionManager() {
        if (!DlcManager.FeatureClusterSpaceEnabled())
            foreach (var gameObject in AttachableBuilding.GetAttachedNetwork(GetComponent<AttachableBuilding>())) {
                var component = gameObject.GetComponent<LaunchConditionManager>();
                if (component != null) return component;
            }

        return null;
    }

    public void SetParentRocketName(string newName) {
        parentRocketName = newName;
        NameDisplayScreen.Instance.UpdateName(gameObject);
    }

    public virtual string GetParentRocketName() { return parentRocketName; }

    /// <summary>
    /// 将对象从其当前位置移动到一个新位置，并执行必要的清理和初始化操作。
    /// </summary>
    public void MoveToSpace() {
        // 获取并检查当前对象的优先级组件，如果存在且属于某个世界，则从该世界的优先级列表中移除。
        var component = GetComponent<Prioritizable>();
        if (component != null && component.GetMyWorld() != null)
            component.GetMyWorld().RemoveTopPriorityPrioritizable(component);
    
        // 将当前对象在网格中的位置转换为单元格坐标。
        var cell = Grid.PosToCell(transform.GetPosition());
        
        // 获取当前对象的建筑组件，并调用其定义的UnmarkArea方法清理之前的区域标记。
        var component2 = GetComponent<Building>();
        component2.Def.UnmarkArea(cell, component2.Orientation, component2.Def.ObjectLayer, gameObject);
        
        // 设置当前对象的新位置为一个特定的向量值。
        var position = new Vector3(-1f, -1f, 0f);
        gameObject.transform.SetPosition(position);
        
        // 获取并调用当前对象的逻辑端口组件的OnMove方法，如果该组件存在。
        var component3 = GetComponent<LogicPorts>();
        if (component3 != null) component3.OnMove();
        
        // 切换当前对象的选择项状态，具体为关闭“Entombed”状态。
        GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, false, this);
    }

    public void MoveToPad(int newCell) {
        gameObject.transform.SetPosition(Grid.CellToPos(newCell, CellAlignment.Bottom, Grid.SceneLayer.Building));
        var cell      = Grid.PosToCell(transform.GetPosition());
        var component = GetComponent<Building>();
        component.RefreshCells();
        component.Def.MarkArea(cell, component.Orientation, component.Def.ObjectLayer, gameObject);
        var component2 = GetComponent<LogicPorts>();
        if (component2 != null) component2.OnMove();
        var component3 = GetComponent<Prioritizable>();
        if (component3 != null && component3.IsTopPriority())
            component3.GetMyWorld().AddTopPriorityPrioritizable(component3);
    }
}