using System;
using UnityEngine;

// Token: 0x020014E1 RID: 5345
[AddComponentMenu("KMonoBehaviour/scripts/Meter")]
public class Meter : KMonoBehaviour
{
	// Token: 0x020014E2 RID: 5346
	public enum Offset
	{
		// Token: 0x04005326 RID: 21286
		Infront,
		// Token: 0x04005327 RID: 21287
		Behind,
		// Token: 0x04005328 RID: 21288
		UserSpecified,
		// Token: 0x04005329 RID: 21289
		NoChange
	}
}
