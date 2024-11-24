using System;
using System.Collections.Generic;
using KSerialization.Converters;
using UnityEngine;

// Token: 0x02001C4C RID: 7244
public class ContentContainer
{
	// Token: 0x060096F8 RID: 38648 RVA: 0x001021CB File Offset: 0x001003CB
	public ContentContainer()
	{
		this.content = new List<ICodexWidget>();
	}

	// Token: 0x060096F9 RID: 38649 RVA: 0x001021DE File Offset: 0x001003DE
	public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
	{
		this.content = content;
		this.contentLayout = contentLayout;
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x060096FA RID: 38650 RVA: 0x001021F4 File Offset: 0x001003F4
	// (set) Token: 0x060096FB RID: 38651 RVA: 0x001021FC File Offset: 0x001003FC
	public List<ICodexWidget> content { get; set; }

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x060096FC RID: 38652 RVA: 0x00102205 File Offset: 0x00100405
	// (set) Token: 0x060096FD RID: 38653 RVA: 0x0010220D File Offset: 0x0010040D
	public string lockID { get; set; }

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x060096FE RID: 38654 RVA: 0x00102216 File Offset: 0x00100416
	// (set) Token: 0x060096FF RID: 38655 RVA: 0x0010221E File Offset: 0x0010041E
	[StringEnumConverter]
	public ContentContainer.ContentLayout contentLayout { get; set; }

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06009700 RID: 38656 RVA: 0x00102227 File Offset: 0x00100427
	// (set) Token: 0x06009701 RID: 38657 RVA: 0x0010222F File Offset: 0x0010042F
	public bool showBeforeGeneratedContent { get; set; }

	// Token: 0x04007548 RID: 30024
	public GameObject go;

	// Token: 0x02001C4D RID: 7245
	public enum ContentLayout
	{
		// Token: 0x0400754E RID: 30030
		Vertical,
		// Token: 0x0400754F RID: 30031
		Horizontal,
		// Token: 0x04007550 RID: 30032
		Grid,
		// Token: 0x04007551 RID: 30033
		GridTwoColumn,
		// Token: 0x04007552 RID: 30034
		GridTwoColumnTall
	}
}
