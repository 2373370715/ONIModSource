using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F4C RID: 3916
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Refinery")]
public class Refinery : KMonoBehaviour
{
	// Token: 0x06004F3A RID: 20282 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x02000F4D RID: 3917
	[Serializable]
	public struct OrderSaveData
	{
		// Token: 0x06004F3C RID: 20284 RVA: 0x000D3A20 File Offset: 0x000D1C20
		public OrderSaveData(string id, bool infinite)
		{
			this.id = id;
			this.infinite = infinite;
		}

		// Token: 0x0400374D RID: 14157
		public string id;

		// Token: 0x0400374E RID: 14158
		public bool infinite;
	}
}
