using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020005FD RID: 1533
public class ListWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06001BCE RID: 7118 RVA: 0x000B2233 File Offset: 0x000B0433
	public int Count
	{
		get
		{
			return this.internalList.Count;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000B2240 File Offset: 0x000B0440
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.internalList).IsReadOnly;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06001BD0 RID: 7120 RVA: 0x001AC774 File Offset: 0x001AA974
	// (remove) Token: 0x06001BD1 RID: 7121 RVA: 0x001AC7AC File Offset: 0x001AA9AC
	public event Action<T> onAdd;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06001BD2 RID: 7122 RVA: 0x001AC7E4 File Offset: 0x001AA9E4
	// (remove) Token: 0x06001BD3 RID: 7123 RVA: 0x001AC81C File Offset: 0x001AAA1C
	public event Action<T> onRemove;

	// Token: 0x1700009A RID: 154
	public T this[int index]
	{
		get
		{
			return this.internalList[index];
		}
		set
		{
			this.internalList[index] = value;
		}
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x000B226A File Offset: 0x000B046A
	public void Add(T item)
	{
		this.internalList.Add(item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x000B228C File Offset: 0x000B048C
	public void Insert(int index, T item)
	{
		this.internalList.Insert(index, item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x001AC854 File Offset: 0x001AAA54
	public void RemoveAt(int index)
	{
		T obj = this.internalList[index];
		this.internalList.RemoveAt(index);
		if (this.onRemove != null)
		{
			this.onRemove(obj);
		}
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x000B22AF File Offset: 0x000B04AF
	public bool Remove(T item)
	{
		bool flag = this.internalList.Remove(item);
		if (flag && this.onRemove != null)
		{
			this.onRemove(item);
		}
		return flag;
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x000B22D4 File Offset: 0x000B04D4
	public void Clear()
	{
		while (this.Count > 0)
		{
			this.RemoveAt(0);
		}
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x000B22E8 File Offset: 0x000B04E8
	public int IndexOf(T item)
	{
		return this.internalList.IndexOf(item);
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x000B22F6 File Offset: 0x000B04F6
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.internalList.CopyTo(array, arrayIndex);
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x000B2305 File Offset: 0x000B0505
	public bool Contains(T item)
	{
		return this.internalList.Contains(item);
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x000B2313 File Offset: 0x000B0513
	public IEnumerator<T> GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x000B2313 File Offset: 0x000B0513
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x0400118D RID: 4493
	private List<T> internalList = new List<T>();
}
