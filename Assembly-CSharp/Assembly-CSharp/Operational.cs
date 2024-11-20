using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Operational")]
public class Operational : KMonoBehaviour {
    public enum State {
        Operational,
        Functional,
        Active,
        None
    }

    private static readonly EventSystem.IntraObjectHandler<Operational> OnNewBuildingDelegate
        = new EventSystem.IntraObjectHandler<Operational>(delegate(Operational component, object data) {
                                                              component.OnNewBuilding(data);
                                                          });

    [Serialize]
    public float activeStartTime;

    [Serialize]
    private float activeTime;

    public Dictionary<Flag, bool> Flags = new Dictionary<Flag, bool>();

    [Serialize]
    public float inactiveStartTime;

    [Serialize]
    private float inactiveTime;

    private readonly int MAX_DATA_POINTS = 5;

    [Serialize]
    private readonly List<float> uptimeData = new List<float>();

    public bool IsFunctional  { get; private set; }
    public bool IsOperational { get; private set; }
    public bool IsActive      { get; private set; }

    [OnSerializing]
    private void OnSerializing() {
        AddTimeData(IsActive);
        activeStartTime   = GameClock.Instance.GetTime();
        inactiveStartTime = GameClock.Instance.GetTime();
    }

    protected override void OnPrefabInit() {
        UpdateFunctional();
        UpdateOperational();
        Subscribe(-1661515756, OnNewBuildingDelegate);
        GameClock.Instance.Subscribe(631075836, OnNewDay);
    }

    public void OnNewBuilding(object data) {
        var component = GetComponent<BuildingComplete>();
        if (component.creationTime > 0f) {
            inactiveStartTime = component.creationTime;
            activeStartTime   = component.creationTime;
        }
    }

    public bool IsOperationalType(Flag.Type type) {
        if (type == Flag.Type.Functional) return IsFunctional;

        return IsOperational;
    }

    public void SetFlag(Flag flag, bool value) {
        var flag2 = false;
        if (Flags.TryGetValue(flag, out flag2)) {
            if (flag2 != value) {
                Flags[flag] = value;
                Trigger(187661686, flag);
            }
        } else {
            Flags[flag] = value;
            Trigger(187661686, flag);
        }

        if (flag.FlagType == Flag.Type.Functional && value != IsFunctional) UpdateFunctional();
        if (value != IsOperational) UpdateOperational();
    }

    public bool GetFlag(Flag flag) {
        var result = false;
        Flags.TryGetValue(flag, out result);
        return result;
    }

    private void UpdateFunctional() {
        var isFunctional = true;
        foreach (var keyValuePair in Flags)
            if (keyValuePair.Key.FlagType == Flag.Type.Functional && !keyValuePair.Value) {
                isFunctional = false;
                break;
            }

        IsFunctional = isFunctional;
        Trigger(-1852328367, IsFunctional);
    }

    private void UpdateOperational() {
        var enumerator = Flags.GetEnumerator();
        var flag       = true;
        while (enumerator.MoveNext()) {
            var keyValuePair = enumerator.Current;
            if (!keyValuePair.Value) {
                flag = false;
                break;
            }
        }

        if (flag != IsOperational) {
            IsOperational = flag;
            if (!IsOperational) SetActive(false);
            if (IsOperational)
                GetComponent<KPrefabID>().AddTag(GameTags.Operational);
            else
                GetComponent<KPrefabID>().RemoveTag(GameTags.Operational);

            Trigger(-592767678, IsOperational);
            Game.Instance.Trigger(-809948329, gameObject);
        }
    }

    public void SetActive(bool value, bool force_ignore = false) {
        if (IsActive != value) {
            AddTimeData(value);
            Trigger(824508782, this);
            Game.Instance.Trigger(-809948329, gameObject);
        }
    }

    private void AddTimeData(bool value) {
        var num  = IsActive ? activeStartTime : inactiveStartTime;
        var time = GameClock.Instance.GetTime();
        var num2 = time - num;
        if (IsActive)
            activeTime += num2;
        else
            inactiveTime += num2;

        IsActive = value;
        if (IsActive) {
            activeStartTime = time;
            return;
        }

        inactiveStartTime = time;
    }

    public void OnNewDay(object data) {
        AddTimeData(IsActive);
        uptimeData.Add(activeTime / 600f);
        while (uptimeData.Count > MAX_DATA_POINTS) uptimeData.RemoveAt(0);
        activeTime   = 0f;
        inactiveTime = 0f;
    }

    public float GetCurrentCycleUptime() {
        if (IsActive) {
            var num  = IsActive ? activeStartTime : inactiveStartTime;
            var num2 = GameClock.Instance.GetTime() - num;
            return (activeTime + num2) / GameClock.Instance.GetTimeSinceStartOfCycle();
        }

        return activeTime / GameClock.Instance.GetTimeSinceStartOfCycle();
    }

    public float GetLastCycleUptime() {
        if (uptimeData.Count > 0) return uptimeData[uptimeData.Count - 1];

        return 0f;
    }

    public float GetUptimeOverCycles(int num_cycles) {
        if (uptimeData.Count > 0) {
            var num                                 = Mathf.Min(uptimeData.Count, num_cycles);
            var num2                                = 0f;
            for (var i = num - 1; i >= 0; i--) num2 += uptimeData[i];
            return num2 / num;
        }

        return 0f;
    }

    public bool MeetsRequirements(State stateRequirement) {
        switch (stateRequirement) {
            case State.Operational:
                return IsOperational;
            case State.Functional:
                return IsFunctional;
            case State.Active:
                return IsActive;
        }

        return true;
    }

    public static GameHashes GetEventForState(State state) {
        if (state == State.Operational) return GameHashes.OperationalChanged;

        if (state == State.Functional) return GameHashes.FunctionalChanged;

        return GameHashes.ActiveChanged;
    }

    public class Flag {
        public enum Type {
            Requirement,
            Functional
        }

        public Type   FlagType;
        public string Name;

        public Flag(string name, Type type) {
            Name     = name;
            FlagType = type;
        }

        public static Type GetFlagType(State operationalState) {
            switch (operationalState) {
                case State.Operational:
                case State.Active:
                    return Type.Requirement;
                case State.Functional:
                    return Type.Functional;
            }

            throw new InvalidOperationException("Can not convert NONE state to an Operational Flag Type");
        }
    }
}