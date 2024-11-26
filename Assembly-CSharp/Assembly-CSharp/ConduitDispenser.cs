using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/ConduitDispenser")]
public class ConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser {
    private static readonly Operational.Flag outputConduitFlag
        = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

    [SerializeField]
    public bool alwaysDispense;

    [SerializeField]
    public bool blocked;

    [MyCmpGet]
    private Building building;

    [SerializeField]
    public ConduitType conduitType;

    [SerializeField]
    public SimHashes[] elementFilter;

    private int elementOutputOffset;

    [SerializeField]
    public bool empty = true;

    [SerializeField]
    public bool invertElementFilter;

    [SerializeField]
    public bool isOn = true;

    [SerializeField]
    public CellOffset noBuildingOutputCellOffset;

    [MyCmpGet]
    private Operational operational;

    private HandleVector<int>.Handle partitionerEntry;

    [MyCmpReq]
    public Storage storage;

    [SerializeField]
    public bool useSecondaryOutput;

    private int                         utilityCell = -1;
    public  ConduitFlow.ConduitContents ConduitContents => GetConduitManager().GetContents(utilityCell);

    public bool IsConnected {
        get {
            var gameObject = Grid.Objects[utilityCell, conduitType == ConduitType.Gas ? 12 : 16];
            return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
        }
    }

    public Storage     Storage                          => storage;
    public ConduitType ConduitType                      => conduitType;
    public void        SetConduitData(ConduitType type) { conduitType = type; }

    /// <summary>
    /// 获取当前对象关联的管道管理器实例。
    /// </summary>
    /// <returns>
    /// 返回相应的管道管理器实例。如果是气体管道类型，则返回气体管道管理器；
    /// 如果是液体管道类型，则返回液体管道管理器；如果不是液体或气体类型，则返回null。
    /// </returns>
    public ConduitFlow GetConduitManager() {
        // 获取当前对象的管道类型
        var conduitType = this.conduitType;
        
        // 如果管道类型为气体，则返回游戏实例中的气体管道管理器
        if (conduitType == ConduitType.Gas) return Game.Instance.gasConduitFlow;
    
        // 如果管道类型不是液体，则返回null，表示没有对应的管道管理器
        if (conduitType != ConduitType.Liquid) return null;
    
        // 如果管道类型为液体，则返回游戏实例中的液体管道管理器
        return Game.Instance.liquidConduitFlow;
    }

    private void OnConduitConnectionChanged(object data) { Trigger(-2094018600, IsConnected); }

    protected override void OnSpawn() {
        base.OnSpawn();
        GameScheduler.Instance.Schedule("PlumbingTutorial",
                                        2f,
                                        delegate {
                                            Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing);
                                        });

        var conduitManager = GetConduitManager();
        utilityCell = GetOutputCell(conduitManager.conduitType);
        var layer = GameScenePartitioner.Instance.objectLayers[conduitType == ConduitType.Gas ? 12 : 16];
        partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn",
                                                             gameObject,
                                                             utilityCell,
                                                             layer,
                                                             OnConduitConnectionChanged);

        GetConduitManager().AddConduitUpdater(ConduitUpdate, ConduitFlowPriority.Dispense);
        OnConduitConnectionChanged(null);
    }

    protected override void OnCleanUp() {
        GetConduitManager().RemoveConduitUpdater(ConduitUpdate);
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
        base.OnCleanUp();
    }

    public void SetOnState(bool onState) { isOn = onState; }

    private void ConduitUpdate(float dt) {
        if (operational != null) operational.SetFlag(outputConduitFlag, IsConnected);
        blocked = false;
        if (isOn) Dispense(dt);
    }

    private void Dispense(float dt) {
        if ((operational != null && operational.IsOperational) || alwaysDispense) {
            if (building != null && building.Def.CanMove) utilityCell = GetOutputCell(GetConduitManager().conduitType);
            var primaryElement                                        = FindSuitableElement();
            if (primaryElement != null) {
                primaryElement.KeepZeroMassObject = true;
                empty                             = false;
                var num = GetConduitManager()
                    .AddElement(utilityCell,
                                primaryElement.ElementID,
                                primaryElement.Mass,
                                primaryElement.Temperature,
                                primaryElement.DiseaseIdx,
                                primaryElement.DiseaseCount);

                if (num > 0f) {
                    var num2 = (int)(num / primaryElement.Mass * primaryElement.DiseaseCount);
                    primaryElement.ModifyDiseaseCount(-num2, "ConduitDispenser.ConduitUpdate");
                    primaryElement.Mass -= num;
                    storage.Trigger(-1697596308, primaryElement.gameObject);
                    return;
                }

                blocked = true;
                return;
            }

            empty = true;
        }
    }

    private PrimaryElement FindSuitableElement() {
        var items = storage.items;
        var count = items.Count;
        for (var i = 0; i < count; i++) {
            var index     = (i + elementOutputOffset) % count;
            var component = items[index].GetComponent<PrimaryElement>();
            if (component      != null                                                                     &&
                component.Mass > 0f                                                                        &&
                (conduitType == ConduitType.Liquid ? component.Element.IsLiquid : component.Element.IsGas) &&
                (elementFilter        == null                                     ||
                 elementFilter.Length == 0                                        ||
                 (!invertElementFilter && IsFilteredElement(component.ElementID)) ||
                 (invertElementFilter  && !IsFilteredElement(component.ElementID)))) {
                elementOutputOffset = (elementOutputOffset + 1) % count;
                return component;
            }
        }

        return null;
    }

    private bool IsFilteredElement(SimHashes element) {
        for (var num = 0; num != elementFilter.Length; num++)
            if (elementFilter[num] == element)
                return true;

        return false;
    }

    private int GetOutputCell(ConduitType outputConduitType) {
        var component = GetComponent<Building>();
        if (!(component != null)) return Grid.OffsetCell(Grid.PosToCell(this), noBuildingOutputCellOffset);

        if (useSecondaryOutput) {
            var components = GetComponents<ISecondaryOutput>();
            foreach (var secondaryOutput in components)
                if (secondaryOutput.HasSecondaryConduitType(outputConduitType))
                    return Grid.OffsetCell(component.NaturalBuildingCell(),
                                           secondaryOutput.GetSecondaryConduitOffset(outputConduitType));

            return Grid.OffsetCell(component.NaturalBuildingCell(),
                                   components[0].GetSecondaryConduitOffset(outputConduitType));
        }

        return component.GetUtilityOutputCell();
    }
}