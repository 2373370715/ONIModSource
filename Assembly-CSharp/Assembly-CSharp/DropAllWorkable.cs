using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

/// <summary>
/// 用于实现游戏对象清空功能的类，继承自 Workable。
/// </summary>
[AddComponentMenu("KMonoBehaviour/Workable/DropAllWorkable")]
public class DropAllWorkable : Workable {
    /// <summary>
    /// 用于处理用户菜单刷新事件的委托。
    /// </summary>
    private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnRefreshUserMenuDelegate
        = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data) {
            component.OnRefreshUserMenu(data);
        });

    /// <summary>
    /// 用于处理存储变化事件的委托。
    /// </summary>
    private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnStorageChangeDelegate
        = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data) {
            component.OnStorageChange(data);
        });

    /// <summary>
    /// 当前的清空任务。
    /// </summary>
    private Chore _chore;

    /// <summary>
    /// 优先级管理组件。
    /// </summary>
    [MyCmpAdd]
    private Prioritizable _prioritizable;

    /// <summary>
    /// 清空任务的类型ID。
    /// </summary>
    public string choreTypeID;

    /// <summary>
    /// 清空操作的工作时间。
    /// </summary>
    public float dropWorkTime = 0.1f;

    /// <summary>
    /// 标记是否需要清空。
    /// </summary>
    [Serialize]
    private bool markedForDrop;

    /// <summary>
    /// 清空时需要移除的标签列表。
    /// </summary>
    public List<Tag> removeTags;

    /// <summary>
    /// 是否显示“清空”命令。
    /// </summary>
    private bool showCmd;

    /// <summary>
    /// 与清空任务相关联的状态项ID。
    /// </summary>
    private Guid statusItem;

    /// <summary>
    /// 存储组件数组。
    /// </summary>
    private Storage[] storages;

    /// <summary>
    /// 构造函数，设置偏移表。
    /// </summary>
    protected DropAllWorkable() { 
        SetOffsetTable(OffsetGroups.InvertedStandardTable); 
    }

    /// <summary>
    /// 获取或设置当前的清空任务。
    /// </summary>
    private Chore Chore {
        get => _chore;
        set {
            _chore = value;
            markedForDrop = _chore != null;
        }
    }

    /// <summary>
    /// 初始化预制件时调用的方法。
    /// </summary>
    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        // 订阅用户菜单刷新事件
        Subscribe(493375141, OnRefreshUserMenuDelegate);
        // 订阅存储变化事件
        Subscribe(-1697596308, OnStorageChangeDelegate);
        // 设置工人状态项
        workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
        // 不同步动画
        synchronizeAnims = false;
        // 设置工作时间
        SetWorkTime(dropWorkTime);
        // 添加优先级引用
        Prioritizable.AddRef(gameObject);
    }

    /// <summary>
    /// 获取存储组件数组。
    /// </summary>
    /// <returns>存储组件数组。</returns>
    private Storage[] GetStorages() {
        if (storages == null) storages = GetComponents<Storage>();
        return storages;
    }

    /// <summary>
    /// 实例化时调用的方法。
    /// </summary>
    protected override void OnSpawn() {
        base.OnSpawn();
        // 检查是否需要显示“清空”命令
        showCmd = GetNewShowCmd();
        // 如果标记为需要清空，则执行清空操作
        if (markedForDrop) DropAll();
    }

    /// <summary>
    /// 执行清空操作。
    /// </summary>
    public void DropAll() {
        if (DebugHandler.InstantBuildMode)
            OnCompleteWork(null);
        else if (Chore == null) {
            // 获取清空任务类型
            var chore_type = !string.IsNullOrEmpty(choreTypeID)
                                 ? Db.Get().ChoreTypes.Get(choreTypeID)
                                 : Db.Get().ChoreTypes.EmptyStorage;

            // 创建新的清空任务
            Chore = new WorkChore<DropAllWorkable>(chore_type,
                                                   this,
                                                   null,
                                                   true,
                                                   null,
                                                   null,
                                                   null,
                                                   true,
                                                   null,
                                                   false,
                                                   false);
        } else {
            // 取消当前的清空任务
            Chore.Cancel("Cancelled emptying");
            Chore = null;
            // 移除状态项
            GetComponent<KSelectable>().RemoveStatusItem(workerStatusItem);
            // 隐藏进度条
            ShowProgressBar(false);
        }

        // 刷新状态项
        RefreshStatusItem();
    }

    /// <summary>
    /// 清空任务完成时调用的方法。
    /// </summary>
    /// <param name="worker">执行工作的工人。</param>
    protected override void OnCompleteWork(Worker worker) {
        var array = GetStorages();
        for (var i = 0; i < array.Length; i++) {
            var list = new List<GameObject>(array[i].items);
            for (var j = 0; j < list.Count; j++) {
                var gameObject = array[i].Drop(list[j]);
                if (gameObject != null) {
                    // 移除指定的标签
                    foreach (var tag in removeTags) gameObject.RemoveTag(tag);
                    // 触发事件
                    gameObject.Trigger(580035959, worker);
                }
            }
        }

        // 重置清空任务
        Chore = null;
        // 刷新状态项
        RefreshStatusItem();
        // 触发完成事件
        Trigger(-1957399615);
    }

    /// <summary>
    /// 处理用户菜单刷新事件。
    /// </summary>
    /// <param name="data">事件数据。</param>
    private void OnRefreshUserMenu(object data) {
        if (showCmd) {
            var button = Chore == null
                             ? new KIconButtonMenu.ButtonInfo("action_empty_contents",
                                                              UI.USERMENUACTIONS.EMPTYSTORAGE.NAME,
                                                              DropAll,
                                                              Action.NumActions,
                                                              null,
                                                              null,
                                                              null,
                                                              UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP)
                             : new KIconButtonMenu.ButtonInfo("action_empty_contents",
                                                              UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF,
                                                              DropAll,
                                                              Action.NumActions,
                                                              null,
                                                              null,
                                                              null,
                                                              UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF);

            // 添加按钮到用户菜单
            Game.Instance.userMenu.AddButton(gameObject, button);
        }
    }

    /// <summary>
    /// 检查是否需要显示“清空”命令。
    /// </summary>
    /// <returns>是否需要显示“清空”命令。</returns>
    private bool GetNewShowCmd() {
        var flag = false;
        var array = GetStorages();
        for (var i = 0; i < array.Length; i++) flag = flag || !array[i].IsEmpty();
        return flag;
    }

    /// <summary>
    /// 处理存储变化事件。
    /// </summary>
    /// <param name="data">事件数据。</param>
    private void OnStorageChange(object data) {
        var newShowCmd = GetNewShowCmd();
        if (newShowCmd != showCmd) {
            showCmd = newShowCmd;
            // 刷新用户菜单
            Game.Instance.userMenu.Refresh(gameObject);
        }
    }

    /// <summary>
    /// 刷新与清空任务相关联的状态项。
    /// </summary>
    private void RefreshStatusItem() {
        if (Chore != null && statusItem == Guid.Empty) {
            var component = GetComponent<KSelectable>();
            statusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding);
            return;
        }

        if (Chore == null && statusItem != Guid.Empty) {
            var component2 = GetComponent<KSelectable>();
            statusItem = component2.RemoveStatusItem(statusItem);
        }
    }
}
