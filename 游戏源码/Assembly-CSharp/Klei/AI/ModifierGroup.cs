using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02003B7E RID: 15230
	public class ModifierGroup<T> : Resource
	{
		// Token: 0x0600EA7D RID: 60029 RVA: 0x0013CBE0 File Offset: 0x0013ADE0
		public IEnumerator<T> GetEnumerator()
		{
			return this.modifiers.GetEnumerator();
		}

		// Token: 0x17000C2B RID: 3115
		public T this[int idx]
		{
			get
			{
				return this.modifiers[idx];
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x0600EA7F RID: 60031 RVA: 0x0013CC00 File Offset: 0x0013AE00
		public int Count
		{
			get
			{
				return this.modifiers.Count;
			}
		}

		// Token: 0x0600EA80 RID: 60032 RVA: 0x0013CC0D File Offset: 0x0013AE0D
		public ModifierGroup(string id, string name) : base(id, name)
		{
		}

		// Token: 0x0600EA81 RID: 60033 RVA: 0x0013CC22 File Offset: 0x0013AE22
		public void Add(T modifier)
		{
			this.modifiers.Add(modifier);
		}

		// Token: 0x0400E5C8 RID: 58824
		public List<T> modifiers = new List<T>();
	}
}
