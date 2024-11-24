using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class SnowballCometConfig : IEntityConfig
{
	// Token: 0x0600134A RID: 4938 RVA: 0x000AE9F4 File Offset: 0x000ACBF4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY.Append("DLC2_ID");
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x0018D5B4 File Offset: 0x0018B7B4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(SnowballCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SNOWBALLCOMET.NAME, "meteor_snow_kanim", SimHashes.Snow, new Vector2(3f, 20f), new Vector2(253.15f, 263.15f), "Meteor_snowball_Impact", 5, SimHashes.Void, SpawnFXHashes.None, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.splashRadius = 1;
		component.addTiles = 3;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		return gameObject;
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D24 RID: 3364
	public static string ID = "SnowballComet";
}
