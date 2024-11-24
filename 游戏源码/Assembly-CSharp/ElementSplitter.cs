using System;
using UnityEngine;

// Token: 0x02000A45 RID: 2629
public struct ElementSplitter
{
	// Token: 0x06003051 RID: 12369 RVA: 0x000BF6A9 File Offset: 0x000BD8A9
	public ElementSplitter(GameObject go)
	{
		this.primaryElement = go.GetComponent<PrimaryElement>();
		this.kPrefabID = go.GetComponent<KPrefabID>();
		this.onTakeCB = null;
		this.canAbsorbCB = null;
	}

	// Token: 0x0400208A RID: 8330
	public PrimaryElement primaryElement;

	// Token: 0x0400208B RID: 8331
	public Func<Pickupable, float, Pickupable> onTakeCB;

	// Token: 0x0400208C RID: 8332
	public Func<Pickupable, bool> canAbsorbCB;

	// Token: 0x0400208D RID: 8333
	public KPrefabID kPrefabID;
}
