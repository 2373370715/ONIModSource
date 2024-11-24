using System;
using System.Collections.Generic;

// Token: 0x02001C29 RID: 7209
public class CategoryEntry : CodexEntry
{
	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x06009619 RID: 38425 RVA: 0x00101BF5 File Offset: 0x000FFDF5
	// (set) Token: 0x0600961A RID: 38426 RVA: 0x00101BFD File Offset: 0x000FFDFD
	public bool largeFormat { get; set; }

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x0600961B RID: 38427 RVA: 0x00101C06 File Offset: 0x000FFE06
	// (set) Token: 0x0600961C RID: 38428 RVA: 0x00101C0E File Offset: 0x000FFE0E
	public bool sort { get; set; }

	// Token: 0x0600961D RID: 38429 RVA: 0x00101C17 File Offset: 0x000FFE17
	public CategoryEntry(string category, List<ContentContainer> contentContainers, string name, List<CodexEntry> entriesInCategory, bool largeFormat, bool sort) : base(category, contentContainers, name)
	{
		this.entriesInCategory = entriesInCategory;
		this.largeFormat = largeFormat;
		this.sort = sort;
	}

	// Token: 0x04007492 RID: 29842
	public List<CodexEntry> entriesInCategory = new List<CodexEntry>();
}
