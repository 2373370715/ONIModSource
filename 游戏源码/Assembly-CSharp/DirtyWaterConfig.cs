using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
public class DirtyWaterConfig : IOreConfig
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060015AC RID: 5548 RVA: 0x000AFBA0 File Offset: 0x000ADDA0
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.DirtyWater;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060015AD RID: 5549 RVA: 0x000AFBA7 File Offset: 0x000ADDA7
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x00194964 File Offset: 0x00192B64
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubbleWater;
		sublimates.info = new Sublimates.Info(4.0000006E-05f, 0.025f, 1.8f, 1f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
