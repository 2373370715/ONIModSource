using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/HierarchyReferences")]
public class HierarchyReferences : KMonoBehaviour {
    public ElementReference[] references;

    public bool HasReference(string name) {
        var array = references;
        for (var i = 0; i < array.Length; i++)
            if (array[i].Name == name)
                return true;

        return false;
    }

    public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component {
        foreach (var elementReference in references)
            if (elementReference.Name == name) {
                if (elementReference.behaviour is SpecifiedType) return (SpecifiedType)elementReference.behaviour;

                Debug.LogError(string.Format("Behavior is not specified type", Array.Empty<object>()));
            }

        Debug.LogError(string.Format("Could not find UI reference '{0}' or convert to specified type)", name));
        return default(SpecifiedType);
    }

    public Component GetReference(string name) {
        foreach (var elementReference in references)
            if (elementReference.Name == name)
                return elementReference.behaviour;

        Debug.LogWarning("Couldn't find reference to object named " +
                         name                                       +
                         " Make sure the name matches the field in the inspector.");

        return null;
    }
}