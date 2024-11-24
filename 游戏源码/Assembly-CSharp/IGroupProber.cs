using System;
using System.Collections.Generic;

// Token: 0x020014E8 RID: 5352
public interface IGroupProber
{
	// Token: 0x06006F74 RID: 28532
	void Occupy(object prober, short serial_no, IEnumerable<int> cells);

	// Token: 0x06006F75 RID: 28533
	void SetValidSerialNos(object prober, short previous_serial_no, short serial_no);

	// Token: 0x06006F76 RID: 28534
	bool ReleaseProber(object prober);
}
