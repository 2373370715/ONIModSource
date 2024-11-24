using System;
using UnityEngine;

// Token: 0x020004CA RID: 1226
public class BleachStoneConfig : IOreConfig
{
	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060015A7 RID: 5543 RVA: 0x000AFB92 File Offset: 0x000ADD92
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.BleachStone;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060015A8 RID: 5544 RVA: 0x000AFB99 File Offset: 0x000ADD99
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ChlorineGas;
		}
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x00194910 File Offset: 0x00192B10
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.BleachStoneEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.00020000001f, 0.0025000002f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
