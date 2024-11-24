using System;
using System.Collections.Generic;

// Token: 0x020012E9 RID: 4841
public interface IFetchList
{
	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x0600634B RID: 25419
	Storage Destination { get; }

	// Token: 0x0600634C RID: 25420
	float GetMinimumAmount(Tag tag);

	// Token: 0x0600634D RID: 25421
	Dictionary<Tag, float> GetRemaining();

	// Token: 0x0600634E RID: 25422
	Dictionary<Tag, float> GetRemainingMinimum();
}
