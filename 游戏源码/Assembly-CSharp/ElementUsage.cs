using System;

// Token: 0x02001C60 RID: 7264
public class ElementUsage
{
	// Token: 0x06009767 RID: 38759 RVA: 0x00102623 File Offset: 0x00100823
	public ElementUsage(Tag tag, float amount, bool continuous) : this(tag, amount, continuous, null)
	{
	}

	// Token: 0x06009768 RID: 38760 RVA: 0x0010262F File Offset: 0x0010082F
	public ElementUsage(Tag tag, float amount, bool continuous, Func<Tag, float, bool, string> customFormating)
	{
		this.tag = tag;
		this.amount = amount;
		this.continuous = continuous;
		this.customFormating = customFormating;
	}

	// Token: 0x0400757F RID: 30079
	public Tag tag;

	// Token: 0x04007580 RID: 30080
	public float amount;

	// Token: 0x04007581 RID: 30081
	public bool continuous;

	// Token: 0x04007582 RID: 30082
	public Func<Tag, float, bool, string> customFormating;
}
