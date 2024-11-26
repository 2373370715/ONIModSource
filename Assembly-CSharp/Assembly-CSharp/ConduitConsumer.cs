using System;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/ConduitConsumer")]
public class ConduitConsumer : KMonoBehaviour, IConduitConsumer {
    public enum WrongElementResult {
        Destroy,
        Dump,
        Store
    }

    [SerializeField]
    public bool alwaysConsume;

    [MyCmpReq]
    protected Building building;

    [SerializeField]
    public float capacityKG = float.PositiveInfinity;

    [SerializeField]
    public Tag capacityTag = GameTags.Any;

    [SerializeField]
    public ConduitType conduitType;

    [NonSerialized]
    public bool consumedLastTick = true;

    public float consumptionRate = float.PositiveInfinity;

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

    [MyCmpGet]
    private BuildingComplete m_buildingComplete;

    public Operational.State OperatingRequirement;

    [MyCmpReq]
    public Operational operational;

    private HandleVector<int>.Handle partitionerEntry;
    private bool                     satisfied;

    [MyCmpGet]
    public Storage storage;

    public ISecondaryInput targetSecondaryInput;

    [SerializeField]
    public bool useSecondaryInput;

    private int                utilityCell = -1;
    public  WrongElementResult wrongElementResult;

    public bool IsConnected =>
        Grid.Objects[utilityCell, conduitType == ConduitType.Gas ? 12 : 16] != null && m_buildingComplete != null;

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

    /// <summary>
    /// 获取当前管理器对应的管道流量管理器实例。
    /// </summary>
    /// <returns>返回对应的管道流量管理器实例，如果没有对应的管理器则返回null。</returns>
    private ConduitFlow GetConduitManager() {
        // 获取当前管理器的管道类型
        var conduitType = this.conduitType;
    
        // 如果管道类型为气体，则返回气体管道流量管理器实例
        if (conduitType == ConduitType.Gas) return Game.Instance.gasConduitFlow;
    
        // 如果管道类型不是液体，则不支持该类型，返回null
        if (conduitType != ConduitType.Liquid) return null;
    
        // 如果管道类型为液体，则返回液体管道流量管理器实例
        return Game.Instance.liquidConduitFlow;
    }

    protected virtual int GetInputCell(ConduitType inputConduitType) {
        if (useSecondaryInput) {
            var components = GetComponents<ISecondaryInput>();
            foreach (var secondaryInput in components)
                if (secondaryInput.HasSecondaryConduitType(inputConduitType))
                    return Grid.OffsetCell(building.NaturalBuildingCell(),
                                           secondaryInput.GetSecondaryConduitOffset(inputConduitType));

            Debug.LogWarning("No secondaryInput of type was found");
            return Grid.OffsetCell(building.NaturalBuildingCell(),
                                   components[0].GetSecondaryConduitOffset(inputConduitType));
        }

        return building.GetUtilityInputCell();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        GameScheduler.Instance.Schedule("PlumbingTutorial",
                                        2f,
                                        delegate {
                                            Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing);
                                        });

        var conduitManager = GetConduitManager();
        utilityCell = GetInputCell(conduitManager.conduitType);
        var layer = GameScenePartitioner.Instance.objectLayers[conduitType == ConduitType.Gas ? 12 : 16];
        partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn",
                                                             gameObject,
                                                             utilityCell,
                                                             layer,
                                                             OnConduitConnectionChanged);

        GetConduitManager().AddConduitUpdater(ConduitUpdate);
        OnConduitConnectionChanged(null);
    }

    protected override void OnCleanUp() {
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
        if (building.Def.CanMove) utilityCell = GetInputCell(conduit_mgr.conduitType);
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