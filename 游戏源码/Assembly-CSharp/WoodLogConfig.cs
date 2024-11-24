using System;
using UnityEngine;

// Token: 0x0200039E RID: 926
public class WoodLogConfig : IOreConfig
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000F46 RID: 3910 RVA: 0x000ACAD0 File Offset: 0x000AACD0
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.WoodLog;
		}
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0017C1C4 File Offset: 0x0017A3C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.prefabInitFn += this.OnInit;
		component.prefabSpawnFn += this.OnSpawn;
		component.RemoveTag(GameTags.HideFromSpawnTool);
		return gameObject;
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x000ACAD7 File Offset: 0x000AACD7
	public void OnInit(GameObject inst)
	{
		PrimaryElement component = inst.GetComponent<PrimaryElement>();
		component.SetElement(this.ElementID, true);
		Element element = component.Element;
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x000ACAF2 File Offset: 0x000AACF2
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<PrimaryElement>().SetElement(this.ElementID, true);
	}

	// Token: 0x04000B00 RID: 2816
	public const string ID = "WoodLog";

	// Token: 0x04000B01 RID: 2817
	public const float C02MassEmissionWhenBurned = 0.142f;

	// Token: 0x04000B02 RID: 2818
	public const float HeatWhenBurned = 7500f;

	// Token: 0x04000B03 RID: 2819
	public const float EnergyWhenBurned = 250f;

	// Token: 0x04000B04 RID: 2820
	public static readonly Tag TAG = TagManager.Create("WoodLog");
}
