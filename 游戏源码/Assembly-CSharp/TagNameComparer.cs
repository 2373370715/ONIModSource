using System;
using System.Collections.Generic;

// Token: 0x02001337 RID: 4919
public class TagNameComparer : IComparer<Tag>
{
	// Token: 0x060064F8 RID: 25848 RVA: 0x000A5E2C File Offset: 0x000A402C
	public TagNameComparer()
	{
	}

	// Token: 0x060064F9 RID: 25849 RVA: 0x000E1E4F File Offset: 0x000E004F
	public TagNameComparer(Tag firstTag)
	{
		this.firstTag = firstTag;
	}

	// Token: 0x060064FA RID: 25850 RVA: 0x002C6B9C File Offset: 0x002C4D9C
	public int Compare(Tag x, Tag y)
	{
		if (x == y)
		{
			return 0;
		}
		if (this.firstTag.IsValid)
		{
			if (x == this.firstTag && y != this.firstTag)
			{
				return 1;
			}
			if (x != this.firstTag && y == this.firstTag)
			{
				return -1;
			}
		}
		return x.ProperNameStripLink().CompareTo(y.ProperNameStripLink());
	}

	// Token: 0x04004A59 RID: 19033
	private Tag firstTag;
}
