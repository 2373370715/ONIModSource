using System;
using UnityEngine;

// Token: 0x020004CE RID: 1230
public class NuclearWasteConfig : IOreConfig
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060015B6 RID: 5558 RVA: 0x000AFBAE File Offset: 0x000ADDAE
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.NuclearWaste;
		}
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x00194C20 File Offset: 0x00192E20
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.decayStorage = true;
		sublimates.spawnFXHash = SpawnFXHashes.NuclearWasteDrip;
		sublimates.info = new Sublimates.Info(0.066f, 6.6f, 1000f, 0f, this.ElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
