using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class MessageTarget : ISaveLoadable {
    [Serialize]
    private readonly string name;

    [Serialize]
    private readonly Vector3 position;

    [Serialize]
    private readonly Ref<KPrefabID> prefabId = new Ref<KPrefabID>();

    public MessageTarget(KPrefabID prefab_id) {
        prefabId.Set(prefab_id);
        position = prefab_id.transform.GetPosition();
        name     = "Unknown";
        var component               = prefab_id.GetComponent<KSelectable>();
        if (component != null) name = component.GetName();
        prefab_id.Subscribe(-1940207677, OnAbsorbedBy);
    }

    public Vector3 GetPosition() {
        if (prefabId.Get() != null) return prefabId.Get().transform.GetPosition();

        return position;
    }

    public KSelectable GetSelectable() {
        if (prefabId.Get() != null) return prefabId.Get().transform.GetComponent<KSelectable>();

        return null;
    }

    public string GetName() { return name; }

    private void OnAbsorbedBy(object data) {
        if (prefabId.Get() != null) prefabId.Get().Unsubscribe(-1940207677, OnAbsorbedBy);
        var component = ((GameObject)data).GetComponent<KPrefabID>();
        component.Subscribe(-1940207677, OnAbsorbedBy);
        prefabId.Set(component);
    }

    public void OnCleanUp() {
        if (prefabId.Get() != null) {
            prefabId.Get().Unsubscribe(-1940207677, OnAbsorbedBy);
            prefabId.Set(null);
        }
    }
}