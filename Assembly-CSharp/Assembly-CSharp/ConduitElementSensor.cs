using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitElementSensor : ConduitSensor {
    [MyCmpGet]
    private Filterable filterable;

    protected override void OnSpawn() {
        base.OnSpawn();
        filterable.onFilterChanged += OnFilterChanged;
        OnFilterChanged(filterable.SelectedTag);
    }

    private void OnFilterChanged(Tag tag) {
        if (!tag.IsValid) return;

        var on = tag == GameTags.Void;
        GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on);
    }

    protected override void ConduitUpdate(float dt) {
        Tag  a;
        bool flag;
        GetContentsElement(out a, out flag);
        if (!IsSwitchedOn) {
            if (a == filterable.SelectedTag && flag) Toggle();
        } else if (a != filterable.SelectedTag || !flag) Toggle();
    }

    private void GetContentsElement(out Tag element, out bool hasMass) {
        var cell = Grid.PosToCell(this);
        if (conduitType == ConduitType.Liquid || conduitType == ConduitType.Gas) {
            var contents = Conduit.GetFlowManager(conduitType).GetContents(cell);
            element = contents.element.CreateTag();
            hasMass = contents.mass > 0f;
            return;
        }

        var flowManager = SolidConduit.GetFlowManager();
        var contents2   = flowManager.GetContents(cell);
        var pickupable  = flowManager.GetPickupable(contents2.pickupableHandle);
        var kprefabID   = pickupable != null ? pickupable.GetComponent<KPrefabID>() : null;
        if (kprefabID != null && pickupable.PrimaryElement.Mass > 0f) {
            element = kprefabID.PrefabTag;
            hasMass = true;
            return;
        }

        element = GameTags.Void;
        hasMass = false;
    }
}