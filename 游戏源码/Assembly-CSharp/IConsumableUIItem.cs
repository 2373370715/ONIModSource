using System;

// Token: 0x02001DC0 RID: 7616
public interface IConsumableUIItem
{
	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x06009F01 RID: 40705
	string ConsumableId { get; }

	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x06009F02 RID: 40706
	string ConsumableName { get; }

	// Token: 0x17000A5C RID: 2652
	// (get) Token: 0x06009F03 RID: 40707
	int MajorOrder { get; }

	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x06009F04 RID: 40708
	int MinorOrder { get; }

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x06009F05 RID: 40709
	bool Display { get; }
}
