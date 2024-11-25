using System;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/Switch")]
public class Switch : KMonoBehaviour, ISaveLoadable, IToggleHandler {
    private static readonly EventSystem.IntraObjectHandler<Switch> OnRefreshUserMenuDelegate
        = new EventSystem.IntraObjectHandler<Switch>(delegate(Switch component, object data) {
                                                         component.OnRefreshUserMenu(data);
                                                     });

    [SerializeField]
    public bool defaultState = true;

    [SerializeField]
    public bool manuallyControlled = true;

    [MyCmpAdd]
    private Toggleable openSwitch;

    private int openToggleIndex;

    [Serialize]
    protected bool switchedOn = true;

    public bool               IsSwitchedOn   => switchedOn;
    public void               HandleToggle() { Toggle(); }
    public bool               IsHandlerOn()  { return switchedOn; }
    public event Action<bool> OnToggle;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        switchedOn = defaultState;
    }

    protected override void OnSpawn() {
        openToggleIndex = openSwitch.SetTarget(this);
        if (OnToggle != null) OnToggle(switchedOn);
        if (manuallyControlled) Subscribe(493375141, OnRefreshUserMenuDelegate);
        UpdateSwitchStatus();
    }

    private void OnMinionToggle() {
        if (!DebugHandler.InstantBuildMode) {
            openSwitch.Toggle(openToggleIndex);
            return;
        }

        Toggle();
    }

    protected virtual void Toggle() { SetState(!switchedOn); }

    protected virtual void SetState(bool on) {
        if (switchedOn != on) {
            switchedOn = on;
            UpdateSwitchStatus();
            if (OnToggle != null) OnToggle(switchedOn);
            if (manuallyControlled) Game.Instance.userMenu.Refresh(gameObject);
        }
    }

    protected virtual void OnRefreshUserMenu(object data) {
        var loc_string = switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF : BUILDINGS.PREFABS.SWITCH.TURN_ON;
        var loc_string2 = switchedOn
                              ? BUILDINGS.PREFABS.SWITCH.TURN_OFF_TOOLTIP
                              : BUILDINGS.PREFABS.SWITCH.TURN_ON_TOOLTIP;

        Game.Instance.userMenu.AddButton(gameObject,
                                         new KIconButtonMenu.ButtonInfo("action_power",
                                                                        loc_string,
                                                                        OnMinionToggle,
                                                                        Action.ToggleEnabled,
                                                                        null,
                                                                        null,
                                                                        null,
                                                                        loc_string2));
    }

    protected virtual void UpdateSwitchStatus() {
        var status_item = switchedOn
                              ? Db.Get().BuildingStatusItems.SwitchStatusActive
                              : Db.Get().BuildingStatusItems.SwitchStatusInactive;

        GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
    }
}