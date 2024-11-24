using System;

// Token: 0x02001404 RID: 5124
public interface IFuelTank
{
	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x0600693F RID: 26943
	IStorage Storage { get; }

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06006940 RID: 26944
	bool ConsumeFuelOnLand { get; }

	// Token: 0x06006941 RID: 26945
	void DEBUG_FillTank();
}
