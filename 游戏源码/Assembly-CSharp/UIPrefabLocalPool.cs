using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPrefabLocalPool
{
	public readonly GameObject sourcePrefab;

	public readonly GameObject parent;

	private Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();

	private Dictionary<int, GameObject> availableInstances = new Dictionary<int, GameObject>();

	public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent)
	{
		this.sourcePrefab = sourcePrefab;
		this.parent = parent;
	}

	public GameObject Borrow()
	{
		GameObject gameObject;
		if (availableInstances.Count == 0)
		{
			gameObject = Util.KInstantiateUI(sourcePrefab, parent, force_active: true);
		}
		else
		{
			gameObject = availableInstances.First().Value;
			availableInstances.Remove(gameObject.GetInstanceID());
		}
		checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
		gameObject.SetActive(value: true);
		gameObject.transform.SetAsLastSibling();
		return gameObject;
	}

	public void Return(GameObject instance)
	{
		checkedOutInstances.Remove(instance.GetInstanceID());
		availableInstances.Add(instance.GetInstanceID(), instance);
		instance.SetActive(value: false);
	}

	public void ReturnAll()
	{
		foreach (var (key, gameObject2) in checkedOutInstances)
		{
			availableInstances.Add(key, gameObject2);
			gameObject2.SetActive(value: false);
		}
		checkedOutInstances.Clear();
	}

	public IEnumerable<GameObject> GetBorrowedObjects()
	{
		return checkedOutInstances.Values;
	}
}
