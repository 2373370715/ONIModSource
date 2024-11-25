using UnityEngine;

public abstract class UtilityNetworkLink : KMonoBehaviour {
    private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingBrokenDelegate
        = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data) {
                                                                     component.OnBuildingBroken(data);
                                                                 });

    private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingFullyRepairedDelegate
        = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data) {
                                                                     component.OnBuildingFullyRepaired(data);
                                                                 });

    private bool connected;

    [SerializeField]
    public CellOffset link1;

    [SerializeField]
    public CellOffset link2;

    [MyCmpGet]
    private Rotatable rotatable;

    [SerializeField]
    public bool visualizeOnly;

    protected override void OnSpawn() {
        base.OnSpawn();
        Subscribe(774203113,   OnBuildingBrokenDelegate);
        Subscribe(-1735440190, OnBuildingFullyRepairedDelegate);
        Connect();
    }

    protected override void OnCleanUp() {
        Unsubscribe(774203113,   OnBuildingBrokenDelegate);
        Unsubscribe(-1735440190, OnBuildingFullyRepairedDelegate);
        Disconnect();
        base.OnCleanUp();
    }

    protected void Connect() {
        if (!visualizeOnly && !connected) {
            connected = true;
            int cell;
            int cell2;
            GetCells(out cell, out cell2);
            OnConnect(cell, cell2);
        }
    }

    protected virtual void OnConnect(int cell1, int cell2) { }

    protected void Disconnect() {
        if (!visualizeOnly && connected) {
            connected = false;
            int cell;
            int cell2;
            GetCells(out cell, out cell2);
            OnDisconnect(cell, cell2);
        }
    }

    protected virtual void OnDisconnect(int cell1, int cell2) { }

    public void GetCells(out int linked_cell1, out int linked_cell2) {
        var component = GetComponent<Building>();
        if (component != null) {
            var orientation = component.Orientation;
            var cell        = Grid.PosToCell(transform.GetPosition());
            GetCells(cell, orientation, out linked_cell1, out linked_cell2);
            return;
        }

        linked_cell1 = -1;
        linked_cell2 = -1;
    }

    public void GetCells(int cell, Orientation orientation, out int linked_cell1, out int linked_cell2) {
        var rotatedCellOffset  = Rotatable.GetRotatedCellOffset(link1, orientation);
        var rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(link2, orientation);
        linked_cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
        linked_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
    }

    public bool AreCellsValid(int cell, Orientation orientation) {
        var rotatedCellOffset  = Rotatable.GetRotatedCellOffset(link1, orientation);
        var rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(link2, orientation);
        return Grid.IsCellOffsetValid(cell, rotatedCellOffset) && Grid.IsCellOffsetValid(cell, rotatedCellOffset2);
    }

    private void OnBuildingBroken(object        data) { Disconnect(); }
    private void OnBuildingFullyRepaired(object data) { Connect(); }

    public int GetNetworkCell() {
        int result;
        int num;
        GetCells(out result, out num);
        return result;
    }
}