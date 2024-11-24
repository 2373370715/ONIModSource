using System;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
[AddComponentMenu("KMonoBehaviour/scripts/ScannerNetworkVisualizer")]
public class ScannerNetworkVisualizer : KMonoBehaviour
{
	// Token: 0x060034E2 RID: 13538 RVA: 0x000C2724 File Offset: 0x000C0924
	protected override void OnSpawn()
	{
		Components.ScannerVisualizers.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x000C273C File Offset: 0x000C093C
	protected override void OnCleanUp()
	{
		Components.ScannerVisualizers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x040023EC RID: 9196
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040023ED RID: 9197
	public int RangeMin;

	// Token: 0x040023EE RID: 9198
	public int RangeMax;
}
