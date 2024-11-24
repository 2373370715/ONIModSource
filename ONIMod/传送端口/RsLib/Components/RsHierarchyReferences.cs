﻿using UnityEngine;

namespace RsLib.Components;

public class RsHierarchyReferences : MonoBehaviour {
    [SerializeField]
    public GameObject[] references;

    private void Awake() {
        if (references == null) {
            Debug.LogWarning("RsHierarchyReferences.references is null");
            references = new GameObject[0];
        }
    }

    public bool HasReference(string name) {
        foreach (var reference in references)
            if (reference.name == name)
                return true;

        return false;
    }

    public T GetReference<T>(string name) where T : Component {
        foreach (var reference in references)
            if (reference != null && reference.name == name) {
                var component = reference.GetComponent<T>();
                if (component != null) return component;
            }

        Debug.LogError(string.Format("Could not find UI reference '{0}' or convert to specified type)", name));
        return default(T);
    }

    public GameObject GetReference(string name) {
        foreach (var reference in references)
            if (reference != null && reference.name == name)
                return reference;

        Debug.LogWarning("Couldn't find reference to object named {0} Make sure the name matches the field in the inspector.");
        return null;
    }
}