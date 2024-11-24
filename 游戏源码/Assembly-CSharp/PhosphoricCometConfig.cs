using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class PhosphoricCometConfig : IEntityConfig
{
	// Token: 0x06001368 RID: 4968 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x0018DB64 File Offset: 0x0018BD64
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(PhosphoricCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.PHOSPHORICCOMET.NAME, "meteor_phosphoric_kanim", SimHashes.Phosphorite, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Void, SpawnFXHashes.MeteorImpactPhosphoric, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D2B RID: 3371
	public static string ID = "PhosphoricComet";
}
