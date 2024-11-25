using Klei.AI;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour {
    private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate
        = new EventSystem.IntraObjectHandler<CreatureFeeder>(delegate(CreatureFeeder component, object data) {
                                                                 component.OnAteFromStorage(data);
                                                             });

    public string     effectId;
    public CellOffset feederOffset = CellOffset.none;
    public Storage[]  storages;

    protected override void OnSpawn() {
        storages = GetComponents<Storage>();
        Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
        Subscribe(-1452790913, OnAteFromStorageDelegate);
    }

    protected override void OnCleanUp() { Components.CreatureFeeders.Remove(this.GetMyWorldId(), this); }

    private void OnAteFromStorage(object data) {
        if (string.IsNullOrEmpty(effectId)) return;

        (data as GameObject).GetComponent<Effects>().Add(effectId, true);
    }

    public bool StoragesAreEmpty() {
        foreach (var storage in storages)
            if (!(storage == null) && storage.Count > 0)
                return false;

        return true;
    }

    public Vector2I GetTargetFeederCell() { return Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(this), feederOffset)); }
}