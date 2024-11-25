public class AssignableReachabilitySensor : Sensor {
    private readonly Navigator   navigator;
    private readonly SlotEntry[] slots;

    public AssignableReachabilitySensor(Sensors sensors) : base(sensors) {
        var minionAssignablesProxy = gameObject.GetComponent<MinionIdentity>().assignableProxy.Get();
        minionAssignablesProxy.ConfigureAssignableSlots();
        var components = minionAssignablesProxy.GetComponents<Assignables>();
        if (components.Length == 0)
            Debug.LogError(gameObject.GetProperName() +
                           ": No 'Assignables' components found for AssignableReachabilitySensor");

        var num                                     = 0;
        foreach (var assignables in components) num += assignables.Slots.Count;
        slots = new SlotEntry[num];
        var num2 = 0;
        foreach (var assignables2 in components)
            for (var k = 0; k < assignables2.Slots.Count; k++)
                slots[num2++].slot = assignables2.Slots[k];

        navigator = GetComponent<Navigator>();
    }

    public bool IsReachable(AssignableSlot slot) {
        for (var i = 0; i < slots.Length; i++)
            if (slots[i].slot.slot == slot)
                return slots[i].isReachable;

        Debug.LogError("Could not find slot: " + (slot != null ? slot.ToString() : null));
        return false;
    }

    public override void Update() {
        for (var i = 0; i < slots.Length; i++) {
            var slotEntry = slots[i];
            var slot      = slotEntry.slot;
            if (slot.IsAssigned()) {
                var flag                    = slot.assignable.GetNavigationCost(navigator) != -1;
                var component               = slot.assignable.GetComponent<Operational>();
                if (component != null) flag = flag && component.IsOperational;
                if (flag != slotEntry.isReachable) {
                    slotEntry.isReachable = flag;
                    slots[i]              = slotEntry;
                    Trigger(334784980, slotEntry);
                }
            } else if (slotEntry.isReachable) {
                slotEntry.isReachable = false;
                slots[i]              = slotEntry;
                Trigger(334784980, slotEntry);
            }
        }
    }

    private struct SlotEntry {
        public AssignableSlotInstance slot;
        public bool                   isReachable;
    }
}