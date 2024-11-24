using System;
using Database;

// Token: 0x0200096C RID: 2412
public interface IBlueprintInfo : IBlueprintDlcInfo
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06002B76 RID: 11126
	// (set) Token: 0x06002B77 RID: 11127
	string id { get; set; }

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06002B78 RID: 11128
	// (set) Token: 0x06002B79 RID: 11129
	string name { get; set; }

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06002B7A RID: 11130
	// (set) Token: 0x06002B7B RID: 11131
	string desc { get; set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06002B7C RID: 11132
	// (set) Token: 0x06002B7D RID: 11133
	PermitRarity rarity { get; set; }

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06002B7E RID: 11134
	// (set) Token: 0x06002B7F RID: 11135
	string animFile { get; set; }
}
