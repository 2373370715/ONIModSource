using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200061F RID: 1567
public class UIPrefabLocalPool
{
	// Token: 0x06001C74 RID: 7284 RVA: 0x000B2B10 File Offset: 0x000B0D10
	public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent)
	{
		this.sourcePrefab = sourcePrefab;
		this.parent = parent;
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x001AD194 File Offset: 0x001AB394
	public GameObject Borrow()
	{
		GameObject gameObject;
		if (this.availableInstances.Count == 0)
		{
			gameObject = Util.KInstantiateUI(this.sourcePrefab, this.parent, true);
		}
		else
		{
			gameObject = this.availableInstances.First<KeyValuePair<int, GameObject>>().Value;
			this.availableInstances.Remove(gameObject.GetInstanceID());
		}
		this.checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
		gameObject.SetActive(true);
		gameObject.transform.SetAsLastSibling();
		return gameObject;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000B2B3C File Offset: 0x000B0D3C
	public void Return(GameObject instance)
	{
		this.checkedOutInstances.Remove(instance.GetInstanceID());
		this.availableInstances.Add(instance.GetInstanceID(), instance);
		instance.SetActive(false);
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x001AD210 File Offset: 0x001AB410
	public void ReturnAll()
	{
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.checkedOutInstances)
		{
			int num;
			GameObject gameObject;
			keyValuePair.Deconstruct(out num, out gameObject);
			int key = num;
			GameObject gameObject2 = gameObject;
			this.availableInstances.Add(key, gameObject2);
			gameObject2.SetActive(false);
		}
		this.checkedOutInstances.Clear();
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000B2B69 File Offset: 0x000B0D69
	public IEnumerable<GameObject> GetBorrowedObjects()
	{
		return this.checkedOutInstances.Values;
	}

	// Token: 0x040011BF RID: 4543
	public readonly GameObject sourcePrefab;

	// Token: 0x040011C0 RID: 4544
	public readonly GameObject parent;

	// Token: 0x040011C1 RID: 4545
	private Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();

	// Token: 0x040011C2 RID: 4546
	private Dictionary<int, GameObject> availableInstances = new Dictionary<int, GameObject>();
}
