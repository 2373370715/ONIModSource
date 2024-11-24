using System;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class TallowConfig : IOreConfig
{
	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06001479 RID: 5241 RVA: 0x000AEF45 File Offset: 0x000AD145
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Tallow;
		}
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x000AEF4C File Offset: 0x000AD14C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
	}

	// Token: 0x04000DB6 RID: 3510
	public const string ID = "Tallow";

	// Token: 0x04000DB7 RID: 3511
	public static readonly Tag TAG = TagManager.Create("Tallow");
}
