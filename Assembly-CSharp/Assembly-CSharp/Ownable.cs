using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Ownable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor {
    private readonly Color ownedTint          = Color.white;
    public           bool  tintWhenUnassigned = true;
    private readonly Color unownedTint        = Color.gray;

    public List<Descriptor> GetDescriptors(GameObject go) {
        var list = new List<Descriptor>();
        var item = default(Descriptor);
        item.SetupDescriptor(UI.BUILDINGEFFECTS.ASSIGNEDDUPLICANT,
                             UI.BUILDINGEFFECTS.TOOLTIPS.ASSIGNEDDUPLICANT,
                             Descriptor.DescriptorType.Requirement);

        list.Add(item);
        return list;
    }

    public override void Assign(IAssignableIdentity new_assignee) {
        if (new_assignee == assignee) return;

        if (this.slot != null && new_assignee is MinionIdentity)
            new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();

        if (this.slot != null && new_assignee is StoredMinionIdentity)
            new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();

        if (new_assignee is MinionAssignablesProxy) {
            var slot = new_assignee.GetSoleOwner().GetComponent<Ownables>().GetSlot(this.slot);
            if (slot != null) {
                var assignable = slot.assignable;
                if (assignable != null) assignable.Unassign();
            }
        }

        base.Assign(new_assignee);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        UpdateTint();
        UpdateStatusString();
        OnAssign += OnNewAssignment;
        if (assignee == null) {
            var component = GetComponent<MinionStorage>();
            if (component) {
                var storedMinionInfo = component.GetStoredMinionInfo();
                if (storedMinionInfo.Count > 0) {
                    var serializedMinion = storedMinionInfo[0].serializedMinion;
                    if (serializedMinion != null && serializedMinion.GetId() != -1) {
                        var component2 = serializedMinion.Get().GetComponent<StoredMinionIdentity>();
                        component2.ValidateProxy();
                        Assign(component2);
                    }
                }
            }
        }
    }

    private void OnNewAssignment(IAssignableIdentity assignables) {
        UpdateTint();
        UpdateStatusString();
    }

    private void UpdateTint() {
        if (tintWhenUnassigned) {
            var component = GetComponent<KAnimControllerBase>();
            if (component != null && component.HasBatchInstanceData) {
                component.TintColour = assignee == null ? unownedTint : ownedTint;
                return;
            }

            var component2 = GetComponent<KBatchedAnimController>();
            if (component2 != null && component2.HasBatchInstanceData)
                component2.TintColour = assignee == null ? unownedTint : ownedTint;
        }
    }

    private void UpdateStatusString() {
        var component = GetComponent<KSelectable>();
        if (component == null) return;

        StatusItem status_item;
        if (assignee != null) {
            if (assignee is MinionIdentity)
                status_item = Db.Get().BuildingStatusItems.AssignedTo;
            else if (assignee is Room)
                status_item = Db.Get().BuildingStatusItems.AssignedTo;
            else
                status_item = Db.Get().BuildingStatusItems.AssignedTo;
        } else
            status_item = Db.Get().BuildingStatusItems.Unassigned;

        component.SetStatusItem(Db.Get().StatusItemCategories.Ownable, status_item, this);
    }
}