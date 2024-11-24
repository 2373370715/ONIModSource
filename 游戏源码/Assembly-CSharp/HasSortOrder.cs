using System;
using UnityEngine;

// Token: 0x02001CFE RID: 7422
[AddComponentMenu("KMonoBehaviour/scripts/HasSortOrder")]
public class HasSortOrder : KMonoBehaviour, IHasSortOrder
{
	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x06009AE6 RID: 39654 RVA: 0x00104C26 File Offset: 0x00102E26
	// (set) Token: 0x06009AE7 RID: 39655 RVA: 0x00104C2E File Offset: 0x00102E2E
	public int sortOrder { get; set; }
}
