using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class UraniumCometConfig : IEntityConfig
{
	// Token: 0x0600133E RID: 4926 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x0018D39C File Offset: 0x0018B59C
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.UraniumOre).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(UraniumCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.URANIUMORECOMET.NAME, "meteor_uranium_kanim", SimHashes.UraniumOre, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(323.15f, 403.15f), "Meteor_Nuclear_Impact", 3, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactUranium, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.entityDamage = 15;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D1E RID: 3358
	public static readonly string ID = "UraniumComet";

	// Token: 0x04000D1F RID: 3359
	private const SimHashes element = SimHashes.UraniumOre;

	// Token: 0x04000D20 RID: 3360
	private const int ADDED_CELLS = 6;
}
