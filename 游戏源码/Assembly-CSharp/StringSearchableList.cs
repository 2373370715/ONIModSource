using System;
using System.Collections.Generic;

// Token: 0x0200061A RID: 1562
public class StringSearchableList<T>
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06001C63 RID: 7267 RVA: 0x000B2A63 File Offset: 0x000B0C63
	// (set) Token: 0x06001C64 RID: 7268 RVA: 0x000B2A6B File Offset: 0x000B0C6B
	public bool didUseFilter { get; private set; }

	// Token: 0x06001C65 RID: 7269 RVA: 0x000B2A74 File Offset: 0x000B0C74
	public StringSearchableList(List<T> allValues, StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.allValues = allValues;
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x000B2AA0 File Offset: 0x000B0CA0
	public StringSearchableList(StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.allValues = new List<T>();
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x001AD038 File Offset: 0x001AB238
	public void Refilter()
	{
		if (StringSearchableListUtil.ShouldUseFilter(this.filter))
		{
			this.filteredValues.Clear();
			foreach (T t in this.allValues)
			{
				if (!this.shouldFilterOutFn(t, this.filter))
				{
					this.filteredValues.Add(t);
				}
			}
			this.didUseFilter = true;
			return;
		}
		if (this.filteredValues.Count != this.allValues.Count)
		{
			this.filteredValues.Clear();
			this.filteredValues.AddRange(this.allValues);
		}
		this.didUseFilter = false;
	}

	// Token: 0x040011B7 RID: 4535
	public string filter = "";

	// Token: 0x040011B8 RID: 4536
	public List<T> allValues;

	// Token: 0x040011B9 RID: 4537
	public List<T> filteredValues;

	// Token: 0x040011BB RID: 4539
	public readonly StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn;

	// Token: 0x0200061B RID: 1563
	// (Invoke) Token: 0x06001C69 RID: 7273
	public delegate bool ShouldFilterOutFn(T candidateValue, in string filter);
}
