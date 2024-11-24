using System;
using UnityEngine;

// Token: 0x02000B04 RID: 2820
[AddComponentMenu("KMonoBehaviour/scripts/SimpleMassStatusItem")]
public class SimpleMassStatusItem : KMonoBehaviour
{
	// Token: 0x060034EB RID: 13547 RVA: 0x000C27A7 File Offset: 0x000C09A7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
	}

	// Token: 0x040023F0 RID: 9200
	public string symbolPrefix = "";
}
