using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A9D RID: 6813
public class UIPool<T> where T : MonoBehaviour
{
	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06008E76 RID: 36470 RVA: 0x000FD20C File Offset: 0x000FB40C
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06008E77 RID: 36471 RVA: 0x000FD219 File Offset: 0x000FB419
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06008E78 RID: 36472 RVA: 0x000FD226 File Offset: 0x000FB426
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06008E79 RID: 36473 RVA: 0x000FD235 File Offset: 0x000FB435
	public UIPool(T prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<T>();
		this.activeElements = new List<T>();
	}

	// Token: 0x06008E7A RID: 36474 RVA: 0x00370758 File Offset: 0x0036E958
	public T GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI<T>(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			T t = this.freeElements[0];
			this.activeElements.Add(t);
			if (t.transform.parent != instantiateParent)
			{
				t.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		T t2 = this.activeElements[this.activeElements.Count - 1];
		if (t2.gameObject.activeInHierarchy != forceActive)
		{
			t2.gameObject.SetActive(forceActive);
		}
		return t2;
	}

	// Token: 0x06008E7B RID: 36475 RVA: 0x00370828 File Offset: 0x0036EA28
	public void ClearElement(T element)
	{
		if (!this.activeElements.Contains(element))
		{
			global::Debug.LogError(this.freeElements.Contains(element) ? "The element provided is already inactive" : "The element provided does not belong to this pool");
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.gameObject.transform.SetParent(this.disabledElementParent);
		}
		element.gameObject.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06008E7C RID: 36476 RVA: 0x003708B8 File Offset: 0x0036EAB8
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].gameObject.transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].gameObject.SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x06008E7D RID: 36477 RVA: 0x000FD270 File Offset: 0x000FB470
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06008E7E RID: 36478 RVA: 0x000FD27E File Offset: 0x000FB47E
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06008E7F RID: 36479 RVA: 0x000FD2B5 File Offset: 0x000FB4B5
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06008E80 RID: 36480 RVA: 0x000FD2EC File Offset: 0x000FB4EC
	public void ForEachActiveElement(Action<T> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06008E81 RID: 36481 RVA: 0x000FD2FA File Offset: 0x000FB4FA
	public void ForEachFreeElement(Action<T> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04006B65 RID: 27493
	private T prefab;

	// Token: 0x04006B66 RID: 27494
	private List<T> freeElements = new List<T>();

	// Token: 0x04006B67 RID: 27495
	private List<T> activeElements = new List<T>();

	// Token: 0x04006B68 RID: 27496
	public Transform disabledElementParent;
}
