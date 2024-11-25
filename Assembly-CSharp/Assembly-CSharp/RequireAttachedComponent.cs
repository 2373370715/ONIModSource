using System;
using KSerialization;
using STRINGS;

[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : ProcessCondition {
    private readonly AttachableBuilding myAttachable;
    private          Type               requiredType;
    private          string             typeNameString;

    public RequireAttachedComponent(AttachableBuilding myAttachable, Type required_type, string type_name_string) {
        this.myAttachable = myAttachable;
        requiredType      = required_type;
        typeNameString    = type_name_string;
    }

    public Type RequiredType {
        get => requiredType;
        set {
            requiredType   = value;
            typeNameString = requiredType.Name;
        }
    }

    public override Status EvaluateCondition() {
        if (myAttachable != null) {
            using (var enumerator = AttachableBuilding.GetAttachedNetwork(myAttachable).GetEnumerator()) {
                while (enumerator.MoveNext())
                    if (enumerator.Current.GetComponent(requiredType))
                        return Status.Ready;
            }

            return Status.Failure;
        }

        return Status.Failure;
    }

    public override string GetStatusMessage(Status status) { return typeNameString; }

    public override string GetStatusTooltip(Status status) {
        if (status == Status.Ready)
            return string.Format(UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, typeNameString.ToLower());

        return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MISSING_TOOLTIP, typeNameString.ToLower());
    }

    public override bool ShowInUI() { return true; }
}