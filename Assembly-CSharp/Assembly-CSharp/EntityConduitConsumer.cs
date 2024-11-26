using System;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/SpawnableConduitConsumer")]
public class EntityConduitConsumer : KMonoBehaviour, IConduitConsumer {
    public enum WrongElementResult {
        Destroy,
        Dump,
        Store
    }

    [SerializeField]
    public bool alwaysConsume;

    [SerializeField]
    public float capacityKG = float.PositiveInfinity;

    [SerializeField]
    public Tag capacityTag = GameTags.Any;

    [MyCmpReq]
    private EntityCellVisualizer cellVisualizer;

    [SerializeField]
    public ConduitType conduitType;

    [NonSerialized]
    public bool consumedLastTick = true;

    public  float                          consumptionRate = float.PositiveInfinity;
    private FlowUtilityNetwork.NetworkItem endpoint;

    [SerializeField]
    public bool forceAlwaysSatisfied;

    [SerializeField]
    public bool ignoreMinMassCheck;

    [NonSerialized]
    public bool isConsuming = true;

    [SerializeField]
    public bool isOn = true;

    [SerializeField]
    public bool keepZeroMassObject = true;

    public SimHashes lastConsumedElement = SimHashes.Vacuum;

    [MyCmpReq]
    private OccupyArea occupyArea;

    public CellOffset        offset;
    public Operational.State OperatingRequirement;

    [MyCmpReq]
    public Operational operational;

    private HandleVector<int>.Handle partitionerEntry;
    private bool                     satisfied;

    [MyCmpGet]
    public Storage storage;

    private int utilityCell = -1;
    public  WrongElementResult wrongElementResult;
    public  bool IsConnected => Grid.Objects[utilityCell, conduitType == ConduitType.Gas ? 12 : 16] != null;

    public bool CanConsume {
        get {
            var result              = false;
            if (IsConnected) result = GetConduitManager().GetContents(utilityCell).mass > 0f;
            return result;
        }
    }

    public float stored_mass {
        get {
            if (storage == null) return 0f;

            if (!(capacityTag != GameTags.Any)) return storage.MassStored();

            return storage.GetMassAvailable(capacityTag);
        }
    }

    public float space_remaining_kg {
        get {
            var num = capacityKG - stored_mass;
            if (!(storage == null)) return Mathf.Min(storage.RemainingCapacity(), num);

            return num;
        }
    }

    public ConduitType TypeOfConduit => conduitType;
    public bool        IsAlmostEmpty => !ignoreMinMassCheck && MassAvailable < ConsumptionRate * 30f;
    public bool        IsEmpty => !ignoreMinMassCheck && (MassAvailable == 0f || MassAvailable < ConsumptionRate);
    public float       ConsumptionRate => consumptionRate;

    public bool IsSatisfied {
        get => satisfied || !isConsuming;
        set => satisfied = value || forceAlwaysSatisfied;
    }

    public float MassAvailable {
        get {
            var conduitManager = GetConduitManager();
            var inputCell      = GetInputCell(conduitManager.conduitType);
            return conduitManager.GetContents(inputCell).mass;
        }
    }

    public Storage     Storage                          => storage;
    public ConduitType ConduitType                      => conduitType;
    public void        SetConduitData(ConduitType type) { conduitType = type; }

    private ConduitFlow GetConduitManager() {
        var conduitType = this.conduitType;
        if (conduitType == ConduitType.Gas) return Game.Instance.gasConduitFlow;

        if (conduitType != ConduitType.Liquid) return null;

        return Game.Instance.liquidConduitFlow;
    }

    private int GetInputCell(ConduitType inputConduitType) { return occupyArea.GetOffsetCellWithRotation(offset); }

    protected override void OnSpawn() {
        base.OnSpawn();
        var conduitManager = GetConduitManager();
        utilityCell = GetInputCell(conduitManager.conduitType);
        var layer = GameScenePartitioner.Instance.objectLayers[conduitType == ConduitType.Gas ? 12 : 16];
        partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn",
                                                             gameObject,
                                                             utilityCell,
                                                             layer,
                                                             OnConduitConnectionChanged);

        GetConduitManager().AddConduitUpdater(ConduitUpdate);
        endpoint = new FlowUtilityNetwork.NetworkItem(conduitManager.conduitType,
                                                      Endpoint.Sink,
                                                      utilityCell,
                                                      gameObject);

        if (conduitManager.conduitType == ConduitType.Solid)
            Game.Instance.solidConduitSystem.AddToNetworks(utilityCell, endpoint, true);
        else
            Conduit.GetNetworkManager(conduitManager.conduitType).AddToNetworks(utilityCell, endpoint, true);

        var type = EntityCellVisualizer.Ports.LiquidIn;
        if (conduitManager.conduitType == ConduitType.Solid)
            type                                                     = EntityCellVisualizer.Ports.SolidIn;
        else if (conduitManager.conduitType == ConduitType.Gas) type = EntityCellVisualizer.Ports.GasIn;

        cellVisualizer.AddPort(type, offset);
        OnConduitConnectionChanged(null);
    }

    protected override void OnCleanUp() {
        if (endpoint.ConduitType == ConduitType.Solid)
            Game.Instance.solidConduitSystem.RemoveFromNetworks(endpoint.Cell, endpoint, true);
        else
            Conduit.GetNetworkManager(endpoint.ConduitType).RemoveFromNetworks(endpoint.Cell, endpoint, true);

        GetConduitManager().RemoveConduitUpdater(ConduitUpdate);
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
        base.OnCleanUp();
    }

    private void OnConduitConnectionChanged(object data)    { Trigger(-2094018600, IsConnected); }
    public  void SetOnState(bool                   onState) { isOn = onState; }

    private void ConduitUpdate(float dt) {
        if (isConsuming && isOn) {
            var conduitManager = GetConduitManager();
            Consume(dt, conduitManager);
        }
    }

    private void Consume(float dt, ConduitFlow conduit_mgr) {
        IsSatisfied      = false;
        consumedLastTick = false;
        utilityCell      = GetInputCell(conduit_mgr.conduitType);
        if (!IsConnected) return;

        var contents = conduit_mgr.GetContents(utilityCell);
        if (contents.mass <= 0f) return;

        IsSatisfied = true;
        if (!alwaysConsume && !operational.MeetsRequirements(OperatingRequirement)) return;

        var num = ConsumptionRate * dt;
        num = Mathf.Min(num, space_remaining_kg);
        var element = ElementLoader.FindElementByHash(contents.element);
        if (contents.element != lastConsumedElement)
            DiscoveredResources.Instance.Discover(element.tag, element.materialCategory);

        var num2 = 0f;
        if (num > 0f) {
            var conduitContents = conduit_mgr.RemoveElement(utilityCell, num);
            num2                = conduitContents.mass;
            lastConsumedElement = conduitContents.element;
        }

        var flag = element.HasTag(capacityTag);
        if (num2 > 0f && capacityTag != GameTags.Any && !flag)
            Trigger(-794517298,
                    new BuildingHP.DamageSourceInfo {
                        damage    = 1,
                        source    = BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
                        popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
                    });

        if (flag                                           ||
            wrongElementResult == WrongElementResult.Store ||
            contents.element   == SimHashes.Vacuum         ||
            capacityTag        == GameTags.Any) {
            if (num2 > 0f) {
                consumedLastTick = true;
                var disease_count = (int)(contents.diseaseCount * (num2 / contents.mass));
                var element2      = ElementLoader.FindElementByHash(contents.element);
                var conduitType   = this.conduitType;
                if (conduitType != ConduitType.Gas) {
                    if (conduitType == ConduitType.Liquid) {
                        if (element2.IsLiquid) {
                            storage.AddLiquid(contents.element,
                                              num2,
                                              contents.temperature,
                                              contents.diseaseIdx,
                                              disease_count,
                                              keepZeroMassObject,
                                              false);

                            return;
                        }

                        Debug.LogWarning("Liquid conduit consumer consuming non liquid: " + element2.id);
                    }
                } else {
                    if (element2.IsGas) {
                        storage.AddGasChunk(contents.element,
                                            num2,
                                            contents.temperature,
                                            contents.diseaseIdx,
                                            disease_count,
                                            keepZeroMassObject,
                                            false);

                        return;
                    }

                    Debug.LogWarning("Gas conduit consumer consuming non gas: " + element2.id);
                }
            }
        } else if (num2 > 0f) {
            consumedLastTick = true;
            if (wrongElementResult == WrongElementResult.Dump) {
                var disease_count2 = (int)(contents.diseaseCount * (num2 / contents.mass));
                SimMessages.AddRemoveSubstance(Grid.PosToCell(transform.GetPosition()),
                                               contents.element,
                                               CellEventLogger.Instance.ConduitConsumerWrongElement,
                                               num2,
                                               contents.temperature,
                                               contents.diseaseIdx,
                                               disease_count2);
            }
        }
    }
}