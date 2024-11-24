using System;
using UnityEngine;

// Token: 0x0200200C RID: 8204
[AddComponentMenu("KMonoBehaviour/scripts/SpawnScreen")]
public class SpawnScreen : KMonoBehaviour
{
	// Token: 0x0600AE72 RID: 44658 RVA: 0x00111814 File Offset: 0x0010FA14
	protected override void OnPrefabInit()
	{
		Util.KInstantiateUI(this.Screen, base.gameObject, false);
	}

	// Token: 0x04008932 RID: 35122
	public GameObject Screen;
}
