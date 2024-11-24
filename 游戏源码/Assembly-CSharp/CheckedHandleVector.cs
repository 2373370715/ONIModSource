using System;
using System.Collections.Generic;

// Token: 0x0200106A RID: 4202
public class CheckedHandleVector<T> where T : new()
{
	// Token: 0x060055B7 RID: 21943 RVA: 0x0027F830 File Offset: 0x0027DA30
	public CheckedHandleVector(int initial_size)
	{
		this.handleVector = new HandleVector<T>(initial_size);
		this.isFree = new List<bool>(initial_size);
		for (int i = 0; i < initial_size; i++)
		{
			this.isFree.Add(true);
		}
	}

	// Token: 0x060055B8 RID: 21944 RVA: 0x0027F880 File Offset: 0x0027DA80
	public HandleVector<T>.Handle Add(T item, string debug_info)
	{
		HandleVector<T>.Handle result = this.handleVector.Add(item);
		if (result.index >= this.isFree.Count)
		{
			this.isFree.Add(false);
		}
		else
		{
			this.isFree[result.index] = false;
		}
		int i = this.handleVector.Items.Count;
		while (i > this.debugInfo.Count)
		{
			this.debugInfo.Add(null);
		}
		this.debugInfo[result.index] = debug_info;
		return result;
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x0027F910 File Offset: 0x0027DB10
	public T Release(HandleVector<T>.Handle handle)
	{
		if (this.isFree[handle.index])
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Tried to double free checked handle ",
				handle.index,
				"- Debug info:",
				this.debugInfo[handle.index]
			});
		}
		this.isFree[handle.index] = true;
		return this.handleVector.Release(handle);
	}

	// Token: 0x060055BA RID: 21946 RVA: 0x000D7EB5 File Offset: 0x000D60B5
	public T Get(HandleVector<T>.Handle handle)
	{
		return this.handleVector.GetItem(handle);
	}

	// Token: 0x04003C2E RID: 15406
	private HandleVector<T> handleVector;

	// Token: 0x04003C2F RID: 15407
	private List<string> debugInfo = new List<string>();

	// Token: 0x04003C30 RID: 15408
	private List<bool> isFree;
}
