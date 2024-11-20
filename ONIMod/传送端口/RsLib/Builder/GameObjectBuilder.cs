using UnityEngine;

namespace RsLib.Builder;

public class GameObjectBuilder : BaseBuilder<GameObject> {
    public override void Build() {
        AppendPrefix();
        Append("|- ");
        Append(target.name);
        Append(" [ ");

        AppendKeyValue("active", target.activeSelf);

        AppendKeyValue("instanceID", target.GetInstanceID());

        AppendKeyValue("layer", LayerMask.LayerToName(target.layer));
        Append("|").Append(target.layer);

        AppendKeyValue("tag", target.tag);

        AppendKeyValue("static", target.isStatic);
        CheckAddendSpace();
        Append("]");

        var components = target.GetComponents<Component>();

        foreach (var component in components) otherBuilderInfos.Add(new OtherBuilderInfo(prefix + "      ", component));

        for (var i = 0; i < target.transform.childCount; i++)
            otherBuilderInfos.Add(new OtherBuilderInfo(prefix + "    ", target.transform.GetChild(i).gameObject));
    }
}