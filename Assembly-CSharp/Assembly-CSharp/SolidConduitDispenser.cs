using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser {
    private static readonly Operational.Flag outputConduitFlag
        = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

    [SerializeField]
    public bool alwaysDispense;

    [SerializeField]
    public SimHashes[] elementFilter;

    [SerializeField]
    public bool invertElementFilter;

    [MyCmpReq]
    private Operational operational;

    private HandleVector<int>.Handle partitionerEntry;
    private int                      round_robin_index;

    [SerializeField]
    public bool solidOnly;

    [MyCmpReq]
    public Storage storage;

    [SerializeField]
    public bool useSecondaryOutput;

    private int                              utilityCell = -1;
    public  SolidConduitFlow.ConduitContents ConduitContents => GetConduitFlow().GetContents(utilityCell);
    public  bool                             IsDispensing    { get; private set; }

    public bool IsConnected {
        get {
            var gameObject = Grid.Objects[utilityCell, 20];
            return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
        }
    }

    public Storage          Storage          => storage;
    public ConduitType      ConduitType      => ConduitType.Solid;
    public SolidConduitFlow GetConduitFlow() { return Game.Instance.solidConduitFlow; }

    protected override void OnSpawn() {
        base.OnSpawn();
        utilityCell = GetOutputCell();
        var layer = GameScenePartitioner.Instance.objectLayers[20];
        partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn",
                                                             gameObject,
                                                             utilityCell,
                                                             layer,
                                                             OnConduitConnectionChanged);

        GetConduitFlow().AddConduitUpdater(ConduitUpdate, ConduitFlowPriority.Dispense);
        OnConduitConnectionChanged(null);
    }

    protected override void OnCleanUp() {
        GetConduitFlow().RemoveConduitUpdater(ConduitUpdate);
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
        base.OnCleanUp();
    }

    private void OnConduitConnectionChanged(object data) {
        IsDispensing = IsDispensing && IsConnected;
        Trigger(-2094018600, IsConnected);
    }

    private void ConduitUpdate(float dt) {
        var flag = false;
        operational.SetFlag(outputConduitFlag, IsConnected);
        if (operational.IsOperational || alwaysDispense) {
            var conduitFlow = GetConduitFlow();
            if (conduitFlow.HasConduit(utilityCell) && conduitFlow.IsConduitEmpty(utilityCell)) {
                var pickupable = FindSuitableItem();
                if (pickupable) {
                    if (pickupable.PrimaryElement.Mass > 20f) pickupable = pickupable.Take(20f);
                    conduitFlow.AddPickupable(utilityCell, pickupable);
                    flag = true;
                }
            }
        }

        storage.storageNetworkID = GetConnectedNetworkID();
        IsDispensing             = flag;
    }

    private bool isSolid(GameObject o) {
        var component = o.GetComponent<PrimaryElement>();
        return component == null || component.Element.IsLiquid || component.Element.IsGas;
    }

    private Pickupable FindSuitableItem() {
        var list = storage.items;
        if (solidOnly) {
            var list2 = new List<GameObject>(list);
            list2.RemoveAll(isSolid);
            list = list2;
        }

        if (list.Count < 1) return null;

        round_robin_index %= list.Count;
        var gameObject = list[round_robin_index];
        round_robin_index++;
        if (!gameObject) return null;

        return gameObject.GetComponent<Pickupable>();
    }

    private int GetConnectedNetworkID() {
        var gameObject     = Grid.Objects[utilityCell, 20];
        var solidConduit   = gameObject   != null ? gameObject.GetComponent<SolidConduit>() : null;
        var utilityNetwork = solidConduit != null ? solidConduit.GetNetwork() : null;
        if (utilityNetwork == null) return -1;

        return utilityNetwork.id;
    }

    private int GetOutputCell() {
        var component = GetComponent<Building>();
        if (useSecondaryOutput) {
            foreach (var secondaryOutput in GetComponents<ISecondaryOutput>())
                if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
                    return Grid.OffsetCell(component.NaturalBuildingCell(),
                                           secondaryOutput.GetSecondaryConduitOffset(ConduitType.Solid));

            return Grid.OffsetCell(component.NaturalBuildingCell(), CellOffset.none);
        }

        return component.GetUtilityOutputCell();
    }
}