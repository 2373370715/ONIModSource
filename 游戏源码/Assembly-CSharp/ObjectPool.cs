using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
	protected Stack<T> unused;

	protected Func<T> instantiator;

	public ObjectPool(Func<T> instantiator, int initial_count = 0)
	{
		this.instantiator = instantiator;
		unused = new Stack<T>(initial_count);
		for (int i = 0; i < initial_count; i++)
		{
			unused.Push(instantiator());
		}
	}

	public virtual T GetInstance()
	{
		T val = default(T);
		if (unused.Count > 0)
		{
			return unused.Pop();
		}
		return instantiator();
	}

	public void ReleaseInstance(T instance)
	{
		if (!object.Equals(instance, null))
		{
			unused.Push(instance);
		}
	}
}
