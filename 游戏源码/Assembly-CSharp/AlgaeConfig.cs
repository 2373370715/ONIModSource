using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
public class AlgaeConfig : IOreConfig
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060015A3 RID: 5539 RVA: 0x000AFB6E File Offset: 0x000ADD6E
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Algae;
		}
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x000AFB75 File Offset: 0x000ADD75
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, new List<Tag>
		{
			GameTags.Life
		});
	}
}
