using System;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class ToxicSandConfig : IOreConfig
{
	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060015C4 RID: 5572 RVA: 0x000AFBCA File Offset: 0x000ADDCA
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.ToxicSand;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060015C5 RID: 5573 RVA: 0x000AFBA7 File Offset: 0x000ADDA7
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x00194D24 File Offset: 0x00192F24
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(2.0000001E-05f, 0.05f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
