using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
		public ObjectPool(Func<T> instantiator, int initial_count = 0)
	{
		this.instantiator = instantiator;
		this.unused = new Stack<T>(initial_count);
		for (int i = 0; i < initial_count; i++)
		{
			this.unused.Push(instantiator());
		}
	}

		public virtual T GetInstance()
	{
		T result = default(T);
		if (this.unused.Count > 0)
		{
			result = this.unused.Pop();
		}
		else
		{
			result = this.instantiator();
		}
		return result;
	}

		public void ReleaseInstance(T instance)
	{
		if (object.Equals(instance, null))
		{
			return;
		}
		this.unused.Push(instance);
	}

		protected Stack<T> unused;

		protected Func<T> instantiator;
}
