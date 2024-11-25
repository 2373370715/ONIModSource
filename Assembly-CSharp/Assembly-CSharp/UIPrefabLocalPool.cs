using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPrefabLocalPool {
    public readonly  GameObject                  parent;
    public readonly  GameObject                  sourcePrefab;
    private readonly Dictionary<int, GameObject> availableInstances  = new Dictionary<int, GameObject>();
    private readonly Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();

    public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent) {
        this.sourcePrefab = sourcePrefab;
        this.parent       = parent;
    }

    public GameObject Borrow() {
        GameObject gameObject;
        if (availableInstances.Count == 0)
            gameObject = Util.KInstantiateUI(sourcePrefab, parent, true);
        else {
            gameObject = availableInstances.First().Value;
            availableInstances.Remove(gameObject.GetInstanceID());
        }

        checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();
        return gameObject;
    }

    public void Return(GameObject instance) {
        checkedOutInstances.Remove(instance.GetInstanceID());
        availableInstances.Add(instance.GetInstanceID(), instance);
        instance.SetActive(false);
    }

    public void ReturnAll() {
        foreach (var keyValuePair in checkedOutInstances) {
            int        num;
            GameObject gameObject;
            keyValuePair.Deconstruct(out num, out gameObject);
            var key         = num;
            var gameObject2 = gameObject;
            availableInstances.Add(key, gameObject2);
            gameObject2.SetActive(false);
        }

        checkedOutInstances.Clear();
    }

    public IEnumerable<GameObject> GetBorrowedObjects() { return checkedOutInstances.Values; }
}