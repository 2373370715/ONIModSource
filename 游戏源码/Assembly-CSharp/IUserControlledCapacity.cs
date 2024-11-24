using System;

// Token: 0x02001F3B RID: 7995
public interface IUserControlledCapacity
{
	// Token: 0x17000AC6 RID: 2758
	// (get) Token: 0x0600A8C2 RID: 43202
	// (set) Token: 0x0600A8C3 RID: 43203
	float UserMaxCapacity { get; set; }

	// Token: 0x17000AC7 RID: 2759
	// (get) Token: 0x0600A8C4 RID: 43204
	float AmountStored { get; }

	// Token: 0x17000AC8 RID: 2760
	// (get) Token: 0x0600A8C5 RID: 43205
	float MinCapacity { get; }

	// Token: 0x17000AC9 RID: 2761
	// (get) Token: 0x0600A8C6 RID: 43206
	float MaxCapacity { get; }

	// Token: 0x17000ACA RID: 2762
	// (get) Token: 0x0600A8C7 RID: 43207
	bool WholeValues { get; }

	// Token: 0x17000ACB RID: 2763
	// (get) Token: 0x0600A8C8 RID: 43208
	LocString CapacityUnits { get; }
}
