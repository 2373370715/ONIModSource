using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A9B RID: 6811
public class UIGameObjectPool
{
	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x06008E66 RID: 36454 RVA: 0x000FD0FC File Offset: 0x000FB2FC
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06008E67 RID: 36455 RVA: 0x000FD109 File Offset: 0x000FB309
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06008E68 RID: 36456 RVA: 0x000FD116 File Offset: 0x000FB316
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06008E69 RID: 36457 RVA: 0x000FD125 File Offset: 0x000FB325
	public UIGameObjectPool(GameObject prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<GameObject>();
		this.activeElements = new List<GameObject>();
	}

	// Token: 0x06008E6A RID: 36458 RVA: 0x0037056C File Offset: 0x0036E76C
	public GameObject GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			GameObject gameObject = this.freeElements[0];
			this.activeElements.Add(gameObject);
			if (gameObject.transform.parent != instantiateParent)
			{
				gameObject.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		GameObject gameObject2 = this.activeElements[this.activeElements.Count - 1];
		if (gameObject2.gameObject.activeInHierarchy != forceActive)
		{
			gameObject2.gameObject.SetActive(forceActive);
		}
		return gameObject2;
	}

	// Token: 0x06008E6B RID: 36459 RVA: 0x00370624 File Offset: 0x0036E824
	public void ClearElement(GameObject element)
	{
		if (!this.activeElements.Contains(element))
		{
			object obj = this.freeElements.Contains(element) ? (element.name + ": The element provided is already inactive") : (element.name + ": The element provided does not belong to this pool");
			element.SetActive(false);
			if (this.disabledElementParent != null)
			{
				element.transform.SetParent(this.disabledElementParent);
			}
			global::Debug.LogError(obj);
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.transform.SetParent(this.disabledElementParent);
		}
		element.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06008E6C RID: 36460 RVA: 0x003706DC File Offset: 0x0036E8DC
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x06008E6D RID: 36461 RVA: 0x000FD160 File Offset: 0x000FB360
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06008E6E RID: 36462 RVA: 0x000FD16E File Offset: 0x000FB36E
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06008E6F RID: 36463 RVA: 0x000FD1A5 File Offset: 0x000FB3A5
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06008E70 RID: 36464 RVA: 0x000FD1DC File Offset: 0x000FB3DC
	public void ForEachActiveElement(Action<GameObject> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06008E71 RID: 36465 RVA: 0x000FD1EA File Offset: 0x000FB3EA
	public void ForEachFreeElement(Action<GameObject> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04006B5E RID: 27486
	private GameObject prefab;

	// Token: 0x04006B5F RID: 27487
	private List<GameObject> freeElements = new List<GameObject>();

	// Token: 0x04006B60 RID: 27488
	private List<GameObject> activeElements = new List<GameObject>();

	// Token: 0x04006B61 RID: 27489
	public Transform disabledElementParent;
}
