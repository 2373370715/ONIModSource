using System;
using UnityEngine;

// Token: 0x02000AF4 RID: 2804
public interface IRottable
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06003494 RID: 13460
	GameObject gameObject { get; }

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06003495 RID: 13461
	float RotTemperature { get; }

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06003496 RID: 13462
	float PreserveTemperature { get; }
}
