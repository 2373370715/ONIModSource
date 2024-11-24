using System;
using System.Collections.Generic;

// Token: 0x020019DA RID: 6618
public class TagCollection : IReadonlyTags
{
	// Token: 0x060089D8 RID: 35288 RVA: 0x000FA4D4 File Offset: 0x000F86D4
	public TagCollection()
	{
	}

	// Token: 0x060089D9 RID: 35289 RVA: 0x003587C0 File Offset: 0x003569C0
	public TagCollection(int[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(initialTags[i]);
		}
	}

	// Token: 0x060089DA RID: 35290 RVA: 0x003587FC File Offset: 0x003569FC
	public TagCollection(string[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(Hash.SDBMLower(initialTags[i]));
		}
	}

	// Token: 0x060089DB RID: 35291 RVA: 0x000FA4E7 File Offset: 0x000F86E7
	public TagCollection(TagCollection initialTags)
	{
		if (initialTags != null && initialTags.tags != null)
		{
			this.tags.UnionWith(initialTags.tags);
		}
	}

	// Token: 0x060089DC RID: 35292 RVA: 0x0035883C File Offset: 0x00356A3C
	public TagCollection Append(TagCollection others)
	{
		foreach (int item in others.tags)
		{
			this.tags.Add(item);
		}
		return this;
	}

	// Token: 0x060089DD RID: 35293 RVA: 0x000FA516 File Offset: 0x000F8716
	public void AddTag(string tag)
	{
		this.tags.Add(Hash.SDBMLower(tag));
	}

	// Token: 0x060089DE RID: 35294 RVA: 0x000FA52A File Offset: 0x000F872A
	public void AddTag(int tag)
	{
		this.tags.Add(tag);
	}

	// Token: 0x060089DF RID: 35295 RVA: 0x000FA539 File Offset: 0x000F8739
	public void RemoveTag(string tag)
	{
		this.tags.Remove(Hash.SDBMLower(tag));
	}

	// Token: 0x060089E0 RID: 35296 RVA: 0x000FA54D File Offset: 0x000F874D
	public void RemoveTag(int tag)
	{
		this.tags.Remove(tag);
	}

	// Token: 0x060089E1 RID: 35297 RVA: 0x000FA55C File Offset: 0x000F875C
	public bool HasTag(string tag)
	{
		return this.tags.Contains(Hash.SDBMLower(tag));
	}

	// Token: 0x060089E2 RID: 35298 RVA: 0x000FA56F File Offset: 0x000F876F
	public bool HasTag(int tag)
	{
		return this.tags.Contains(tag);
	}

	// Token: 0x060089E3 RID: 35299 RVA: 0x00358898 File Offset: 0x00356A98
	public bool HasTags(int[] searchTags)
	{
		for (int i = 0; i < searchTags.Length; i++)
		{
			if (!this.tags.Contains(searchTags[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040067B9 RID: 26553
	private HashSet<int> tags = new HashSet<int>();
}
