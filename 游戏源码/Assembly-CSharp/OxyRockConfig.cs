using System;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class OxyRockConfig : IOreConfig
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060015BA RID: 5562 RVA: 0x000AFBB5 File Offset: 0x000ADDB5
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.OxyRock;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060015BB RID: 5563 RVA: 0x000AFBBC File Offset: 0x000ADDBC
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.Oxygen;
		}
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x00194C7C File Offset: 0x00192E7C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.010000001f, 0.0050000004f, 1.8f, 0.7f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
