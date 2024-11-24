using System;
using UnityEngine;

// Token: 0x02001062 RID: 4194
public class CellSelectionInstantiator : MonoBehaviour
{
	// Token: 0x0600558C RID: 21900 RVA: 0x0027E8F0 File Offset: 0x0027CAF0
	private void Awake()
	{
		GameObject gameObject = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		CellSelectionObject component = gameObject.GetComponent<CellSelectionObject>();
		CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
		component.alternateSelectionObject = component2;
		component2.alternateSelectionObject = component;
	}

	// Token: 0x04003C01 RID: 15361
	public GameObject CellSelectionPrefab;
}
