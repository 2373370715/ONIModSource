using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/AccessControl")]
public class AccessControl : KMonoBehaviour, IGameObjectEffectDescriptor {
    public enum Permission {
        Both,
        GoLeft,
        GoRight,
        Neither
    }

    private static StatusItem accessControlActive;

    private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate
        = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data) {
                                                                component.OnControlStateChanged(data);
                                                            });

    private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate
        = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data) {
                                                                component.OnCopySettings(data);
                                                            });

    [Serialize]
    private Permission _defaultPermission;

    [Serialize]
    public bool controlEnabled;

    [MyCmpAdd]
    private CopyBuildingSettings copyBuildingSettings;

    private bool isTeleporter;

    [MyCmpGet]
    private Operational operational;

    public Door.ControlState overrideAccess;

    [Serialize]
    public bool registered = true;

    private int[] registeredBuildingCells;

    [Serialize]
    private readonly List<KeyValuePair<Ref<KPrefabID>, Permission>> savedPermissions
        = new List<KeyValuePair<Ref<KPrefabID>, Permission>>();

    [MyCmpReq]
    private KSelectable selectable;

    public Permission DefaultPermission {
        get => _defaultPermission;
        set {
            _defaultPermission = value;
            SetStatusItem();
            SetGridRestrictions(null, _defaultPermission);
        }
    }

    public bool Online => true;

    public List<Descriptor> GetDescriptors(GameObject go) {
        var list = new List<Descriptor>();
        var item = default(Descriptor);
        item.SetupDescriptor(UI.BUILDINGEFFECTS.ACCESS_CONTROL, UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL);
        list.Add(item);
        return list;
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        if (accessControlActive == null)
            accessControlActive = new StatusItem("accessControlActive",
                                                 BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME,
                                                 BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP,
                                                 "",
                                                 StatusItem.IconType.Info,
                                                 NotificationType.Neutral,
                                                 false,
                                                 OverlayModes.None.ID);

        Subscribe(279163026,  OnControlStateChangedDelegate);
        Subscribe(-905833192, OnCopySettingsDelegate);
    }

    private void CheckForBadData() {
        var list = new List<KeyValuePair<Ref<KPrefabID>, Permission>>();
        foreach (var item in savedPermissions)
            if (item.Key.Get() == null)
                list.Add(item);

        foreach (var item2 in list) savedPermissions.Remove(item2);
    }

    protected override void OnSpawn() {
        isTeleporter = GetComponent<NavTeleporter>() != null;
        base.OnSpawn();
        if (savedPermissions.Count > 0) CheckForBadData();
        if (registered) {
            RegisterInGrid(true);
            RestorePermissions();
        }

        var pooledList = ListPool<Tuple<MinionAssignablesProxy, Permission>, AccessControl>.Allocate();
        for (var i = savedPermissions.Count - 1; i >= 0; i--) {
            var kprefabID = savedPermissions[i].Key.Get();
            if (kprefabID != null) {
                var component = kprefabID.GetComponent<MinionIdentity>();
                if (component != null) {
                    pooledList.Add(new Tuple<MinionAssignablesProxy, Permission>(component.assignableProxy.Get(),
                                    savedPermissions[i].Value));

                    savedPermissions.RemoveAt(i);
                    ClearGridRestrictions(kprefabID);
                }
            }
        }

        foreach (var tuple in pooledList) SetPermission(tuple.first, tuple.second);
        pooledList.Recycle();
        SetStatusItem();
    }

    protected override void OnCleanUp() {
        RegisterInGrid(false);
        base.OnCleanUp();
    }

    private void OnControlStateChanged(object data) { overrideAccess = (Door.ControlState)data; }

    private void OnCopySettings(object data) {
        var component = ((GameObject)data).GetComponent<AccessControl>();
        if (component != null) {
            savedPermissions.Clear();
            foreach (var keyValuePair in component.savedPermissions)
                if (keyValuePair.Key.Get() != null)
                    SetPermission(keyValuePair.Key.Get().GetComponent<MinionAssignablesProxy>(), keyValuePair.Value);

            _defaultPermission = component._defaultPermission;
            SetGridRestrictions(null, DefaultPermission);
        }
    }

    public void SetRegistered(bool newRegistered) {
        if (newRegistered && !registered) {
            RegisterInGrid(true);
            RestorePermissions();
            return;
        }

        if (!newRegistered && registered) RegisterInGrid(false);
    }

    public void SetPermission(MinionAssignablesProxy key, Permission permission) {
        var component = key.GetComponent<KPrefabID>();
        if (component == null) return;

        var flag = false;
        for (var i = 0; i < savedPermissions.Count; i++)
            if (savedPermissions[i].Key.GetId() == component.InstanceID) {
                flag = true;
                var keyValuePair = savedPermissions[i];
                savedPermissions[i] = new KeyValuePair<Ref<KPrefabID>, Permission>(keyValuePair.Key, permission);
                break;
            }

        if (!flag)
            savedPermissions.Add(new KeyValuePair<Ref<KPrefabID>, Permission>(new Ref<KPrefabID>(component),
                                                                              permission));

        SetStatusItem();
        SetGridRestrictions(component, permission);
    }

    private void RestorePermissions() {
        SetGridRestrictions(null, DefaultPermission);
        foreach (var keyValuePair in savedPermissions) {
            var x = keyValuePair.Key.Get();
            if (x == null)
                DebugUtil.Assert(x == null,
                                 "Tried to set a duplicant-specific access restriction with a null key! This will result in an invisible default permission!");

            SetGridRestrictions(keyValuePair.Key.Get(), keyValuePair.Value);
        }
    }

    private void RegisterInGrid(bool register) {
        var component  = GetComponent<Building>();
        var component2 = GetComponent<OccupyArea>();
        if (component2 == null && component == null) return;

        if (register) {
            var                          component3 = GetComponent<Rotatable>();
            Grid.Restriction.Orientation orientation;
            if (!isTeleporter)
                orientation = component3 == null || component3.GetOrientation() == Orientation.Neutral
                                  ? Grid.Restriction.Orientation.Vertical
                                  : Grid.Restriction.Orientation.Horizontal;
            else
                orientation = Grid.Restriction.Orientation.SingleCell;

            if (component != null) {
                registeredBuildingCells = component.PlacementCells;
                var array = registeredBuildingCells;
                for (var i = 0; i < array.Length; i++) Grid.RegisterRestriction(array[i], orientation);
            } else
                foreach (var offset in component2.OccupiedCellsOffsets)
                    Grid.RegisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), orientation);

            if (isTeleporter) Grid.RegisterRestriction(GetComponent<NavTeleporter>().GetCell(), orientation);
        } else {
            if (component != null) {
                if (component.GetMyWorldId() != 255 && registeredBuildingCells != null) {
                    var array = registeredBuildingCells;
                    for (var i = 0; i < array.Length; i++) Grid.UnregisterRestriction(array[i]);
                    registeredBuildingCells = null;
                }
            } else
                foreach (var offset2 in component2.OccupiedCellsOffsets)
                    Grid.UnregisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset2));

            if (isTeleporter) {
                var cell = GetComponent<NavTeleporter>().GetCell();
                if (cell != Grid.InvalidCell) Grid.UnregisterRestriction(cell);
            }
        }

        registered = register;
    }

    private void SetGridRestrictions(KPrefabID kpid, Permission permission) {
        if (!registered || !isSpawned) return;

        var component  = GetComponent<Building>();
        var component2 = GetComponent<OccupyArea>();
        if (component2 == null && component == null) return;

        var                         minionInstanceID = kpid != null ? kpid.InstanceID : -1;
        Grid.Restriction.Directions directions       = 0;
        switch (permission) {
            case Permission.Both:
                directions = 0;
                break;
            case Permission.GoLeft:
                directions = Grid.Restriction.Directions.Right;
                break;
            case Permission.GoRight:
                directions = Grid.Restriction.Directions.Left;
                break;
            case Permission.Neither:
                directions = Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right;
                break;
        }

        if (isTeleporter) {
            if (directions != 0)
                directions = Grid.Restriction.Directions.Teleport;
            else
                directions = 0;
        }

        if (component != null) {
            var array = registeredBuildingCells;
            for (var i = 0; i < array.Length; i++) Grid.SetRestriction(array[i], minionInstanceID, directions);
        } else
            foreach (var offset in component2.OccupiedCellsOffsets)
                Grid.SetRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID, directions);

        if (isTeleporter) Grid.SetRestriction(GetComponent<NavTeleporter>().GetCell(), minionInstanceID, directions);
    }

    private void ClearGridRestrictions(KPrefabID kpid) {
        var component  = GetComponent<Building>();
        var component2 = GetComponent<OccupyArea>();
        if (component2 == null && component == null) return;

        var minionInstanceID = kpid != null ? kpid.InstanceID : -1;
        if (component != null) {
            var array = registeredBuildingCells;
            for (var i = 0; i < array.Length; i++) Grid.ClearRestriction(array[i], minionInstanceID);
            return;
        }

        foreach (var offset in component2.OccupiedCellsOffsets)
            Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID);
    }

    public Permission GetPermission(Navigator minion) {
        var controlState = overrideAccess;
        if (controlState == Door.ControlState.Opened) return Permission.Both;

        if (controlState == Door.ControlState.Locked) return Permission.Neither;

        return GetSetPermission(GetKeyForNavigator(minion));
    }

    private MinionAssignablesProxy GetKeyForNavigator(Navigator minion) {
        return minion.GetComponent<MinionIdentity>().assignableProxy.Get();
    }

    public Permission GetSetPermission(MinionAssignablesProxy key) {
        return GetSetPermission(key.GetComponent<KPrefabID>());
    }

    private Permission GetSetPermission(KPrefabID kpid) {
        var result = DefaultPermission;
        if (kpid != null)
            for (var i = 0; i < savedPermissions.Count; i++)
                if (savedPermissions[i].Key.GetId() == kpid.InstanceID) {
                    result = savedPermissions[i].Value;
                    break;
                }

        return result;
    }

    public void ClearPermission(MinionAssignablesProxy key) {
        var component = key.GetComponent<KPrefabID>();
        if (component != null)
            for (var i = 0; i < savedPermissions.Count; i++)
                if (savedPermissions[i].Key.GetId() == component.InstanceID) {
                    savedPermissions.RemoveAt(i);
                    break;
                }

        SetStatusItem();
        ClearGridRestrictions(component);
    }

    public bool IsDefaultPermission(MinionAssignablesProxy key) {
        var flag      = false;
        var component = key.GetComponent<KPrefabID>();
        if (component != null)
            for (var i = 0; i < savedPermissions.Count; i++)
                if (savedPermissions[i].Key.GetId() == component.InstanceID) {
                    flag = true;
                    break;
                }

        return !flag;
    }

    private void SetStatusItem() {
        if (_defaultPermission != Permission.Both || savedPermissions.Count > 0) {
            selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, accessControlActive);
            return;
        }

        selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, null);
    }
}