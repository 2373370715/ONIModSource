using System;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
public class SlimeMoldConfig : IOreConfig
{
	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060015BF RID: 5567 RVA: 0x000AFBC3 File Offset: 0x000ADDC3
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.SlimeMold;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060015C0 RID: 5568 RVA: 0x000AFBA7 File Offset: 0x000ADDA7
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x00194CD0 File Offset: 0x00192ED0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
