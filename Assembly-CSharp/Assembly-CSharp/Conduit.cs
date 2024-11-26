using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/Conduit")]
public class Conduit
    : KMonoBehaviour,
      IFirstFrameCallback,
      IHaveUtilityNetworkMgr,
      IBridgedNetworkItem,
      IDisconnectable,
      FlowUtilityNetwork.IItem {
    protected static readonly EventSystem.IntraObjectHandler<Conduit> OnHighlightedDelegate
        = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data) {
                                                          component.OnHighlighted(data);
                                                      });

    protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitFrozenDelegate
        = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data) {
                                                          component.OnConduitFrozen(data);
                                                      });

    protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitBoilingDelegate
        = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data) {
                                                          component.OnConduitBoiling(data);
                                                      });

    protected static readonly EventSystem.IntraObjectHandler<Conduit> OnStructureTemperatureRegisteredDelegate
        = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data) {
                                                          component.OnStructureTemperatureRegistered(data);
                                                      });

    protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingBrokenDelegate
        = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data) {
                                                          component.OnBuildingBroken(data);
                                                      });

    protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingFullyRepairedDelegate
        = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data) {
                                                          component.OnBuildingFullyRepaired(data);
                                                      });

    [SerializeField]
    private bool disconnected = true;

    private System.Action firstFrameCallback;

    [MyCmpReq]
    private KAnimGraphTileVisualizer graphTileDependency;

    public ConduitType type;

    public virtual void AddNetworks(ICollection<UtilityNetwork> networks) {
        var networkForCell = GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
        if (networkForCell != null) networks.Add(networkForCell);
    }

    public virtual bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks) {
        var networkForCell = GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
        return networks.Contains(networkForCell);
    }

    public virtual int  GetNetworkCell() { return Grid.PosToCell(this); }
    public         bool IsDisconnected() { return disconnected; }

    public bool Connect() {
        var component = GetComponent<BuildingHP>();
        if (component == null || component.HitPoints > 0) {
            disconnected = false;
            GetNetworkManager().ForceRebuildNetworks();
        }

        return !disconnected;
    }

    public void Disconnect() {
        disconnected = true;
        GetNetworkManager().ForceRebuildNetworks();
    }

    public void SetFirstFrameCallback(System.Action ffCb) {
        firstFrameCallback = ffCb;
        StartCoroutine(RunCallback());
    }

    public IUtilityNetworkMgr GetNetworkManager() {
        if (type != ConduitType.Gas) return Game.Instance.liquidConduitSystem;

        return Game.Instance.gasConduitSystem;
    }

    public FlowUtilityNetwork Network {
        set { }
    }

    public int         Cell         => Grid.PosToCell(this);
    public Endpoint    EndpointType => Endpoint.Conduit;
    public ConduitType ConduitType  => type;
    public GameObject  GameObject   => gameObject;

    private IEnumerator RunCallback() {
        yield return null;

        if (firstFrameCallback != null) {
            firstFrameCallback();
            firstFrameCallback = null;
        }

        yield return null;
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Subscribe(-1201923725, OnHighlightedDelegate);
        Subscribe(-700727624,  OnConduitFrozenDelegate);
        Subscribe(-1152799878, OnConduitBoilingDelegate);
        Subscribe(-1555603773, OnStructureTemperatureRegisteredDelegate);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        Subscribe(774203113,   OnBuildingBrokenDelegate);
        Subscribe(-1735440190, OnBuildingFullyRepairedDelegate);
    }

    protected virtual void OnStructureTemperatureRegistered(object data) {
        var cell = Grid.PosToCell(this);
        GetNetworkManager().AddToNetworks(cell, this, false);
        Connect();
        GetComponent<KSelectable>()
            .SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Pipe, this);

        var def = GetComponent<Building>().Def;
        if (def != null && def.ThermalConductivity != 1f)
            GetFlowVisualizer()
                .AddThermalConductivity(Grid.PosToCell(transform.GetPosition()), def.ThermalConductivity);
    }

    protected override void OnCleanUp() {
        Unsubscribe(774203113,   OnBuildingBrokenDelegate);
        Unsubscribe(-1735440190, OnBuildingFullyRepairedDelegate);
        var def = GetComponent<Building>().Def;
        if (def != null && def.ThermalConductivity != 1f)
            GetFlowVisualizer()
                .RemoveThermalConductivity(Grid.PosToCell(transform.GetPosition()), def.ThermalConductivity);

        var cell = Grid.PosToCell(transform.GetPosition());
        GetNetworkManager().RemoveFromNetworks(cell, this, false);
        var component = GetComponent<BuildingComplete>();
        if (component.Def.ReplacementLayer                          == ObjectLayer.NumLayers ||
            Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null) {
            GetNetworkManager().RemoveFromNetworks(cell, this, false);
            GetFlowManager().EmptyConduit(Grid.PosToCell(transform.GetPosition()));
        }

        base.OnCleanUp();
    }

    protected ConduitFlowVisualizer GetFlowVisualizer() {
        if (type != ConduitType.Gas) return Game.Instance.liquidFlowVisualizer;

        return Game.Instance.gasFlowVisualizer;
    }

    public ConduitFlow GetFlowManager() {
        if (type != ConduitType.Gas) return Game.Instance.liquidConduitFlow;

        return Game.Instance.gasConduitFlow;
    }

    public static ConduitFlow GetFlowManager(ConduitType type) {
        if (type != ConduitType.Gas) return Game.Instance.liquidConduitFlow;

        return Game.Instance.gasConduitFlow;
    }

    public static IUtilityNetworkMgr GetNetworkManager(ConduitType type) {
        if (type != ConduitType.Gas) return Game.Instance.liquidConduitSystem;

        return Game.Instance.gasConduitSystem;
    }

    private void OnHighlighted(object data) {
        var highlightedCell = (bool)data ? Grid.PosToCell(transform.GetPosition()) : -1;
        GetFlowVisualizer().SetHighlightedCell(highlightedCell);
    }

    private void OnConduitFrozen(object data) {
        Trigger(-794517298,
                new BuildingHP.DamageSourceInfo {
                    damage    = 1,
                    source    = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_FROZE,
                    popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_FROZE,
                    takeDamageEffect
                        = ConduitType == ConduitType.Gas
                              ? SpawnFXHashes.BuildingLeakLiquid
                              : SpawnFXHashes.BuildingFreeze,
                    fullDamageEffectName = ConduitType == ConduitType.Gas ? "water_damage_kanim" : "ice_damage_kanim"
                });

        GetFlowManager().EmptyConduit(Grid.PosToCell(transform.GetPosition()));
    }

    private void OnConduitBoiling(object data) {
        Trigger(-794517298,
                new BuildingHP.DamageSourceInfo {
                    damage               = 1,
                    source               = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_BOILED,
                    popString            = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_BOILED,
                    takeDamageEffect     = SpawnFXHashes.BuildingLeakGas,
                    fullDamageEffectName = "gas_damage_kanim"
                });

        GetFlowManager().EmptyConduit(Grid.PosToCell(transform.GetPosition()));
    }

    private void OnBuildingBroken(object        data) { Disconnect(); }
    private void OnBuildingFullyRepaired(object data) { Connect(); }
}