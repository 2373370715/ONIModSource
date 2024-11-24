using System;
using System.Collections;
using System.Collections.Generic;

public class ListWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	private List<T> internalList = new List<T>();

	public int Count => internalList.Count;

	public bool IsReadOnly => ((ICollection<T>)internalList).IsReadOnly;

	public T this[int index]
	{
		get
		{
			return internalList[index];
		}
		set
		{
			internalList[index] = value;
		}
	}

	public event Action<T> onAdd;

	public event Action<T> onRemove;

	public void Add(T item)
	{
		internalList.Add(item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	public void Insert(int index, T item)
	{
		internalList.Insert(index, item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	public void RemoveAt(int index)
	{
		T obj = internalList[index];
		internalList.RemoveAt(index);
		if (this.onRemove != null)
		{
			this.onRemove(obj);
		}
	}

	public bool Remove(T item)
	{
		bool num = internalList.Remove(item);
		if (num && this.onRemove != null)
		{
			this.onRemove(item);
		}
		return num;
	}

	public void Clear()
	{
		while (Count > 0)
		{
			RemoveAt(0);
		}
	}

	public int IndexOf(T item)
	{
		return internalList.IndexOf(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		internalList.CopyTo(array, arrayIndex);
	}

	public bool Contains(T item)
	{
		return internalList.Contains(item);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return internalList.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return internalList.GetEnumerator();
	}
}
