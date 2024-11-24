using System;
using System.Collections.Generic;

// Token: 0x020005FE RID: 1534
public class ObjectPool<T>
{
	// Token: 0x06001BE1 RID: 7137 RVA: 0x001AC890 File Offset: 0x001AAA90
	public ObjectPool(Func<T> instantiator, int initial_count = 0)
	{
		this.instantiator = instantiator;
		this.unused = new Stack<T>(initial_count);
		for (int i = 0; i < initial_count; i++)
		{
			this.unused.Push(instantiator());
		}
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x001AC8D4 File Offset: 0x001AAAD4
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

	// Token: 0x06001BE3 RID: 7139 RVA: 0x000B2338 File Offset: 0x000B0538
	public void ReleaseInstance(T instance)
	{
		if (object.Equals(instance, null))
		{
			return;
		}
		this.unused.Push(instance);
	}

	// Token: 0x04001190 RID: 4496
	protected Stack<T> unused;

	// Token: 0x04001191 RID: 4497
	protected Func<T> instantiator;
}
