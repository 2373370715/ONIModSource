using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class HardIceCometConfig : IEntityConfig
{
	// Token: 0x06001356 RID: 4950 RVA: 0x000AE9F4 File Offset: 0x000ACBF4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY.Append("DLC2_ID");
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x0018D7CC File Offset: 0x0018B9CC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(HardIceCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.HARDICECOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		float mass = ElementLoader.FindElementByHash(SimHashes.CrushedIce).defaultValues.mass;
		comet.massRange = new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f);
		comet.temperatureRange = new Vector2(173.15f, 248.15f);
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.addTiles = 6;
		comet.addTilesMinHeight = 2;
		comet.addTilesMaxHeight = 8;
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_ice_Impact";
		comet.flyingSoundID = 6;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactIce;
		comet.EXHAUST_ELEMENT = SimHashes.Oxygen;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.CrushedIce, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_ice_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D26 RID: 3366
	public static readonly string ID = "HardIceComet";

	// Token: 0x04000D27 RID: 3367
	private const SimHashes element = SimHashes.CrushedIce;

	// Token: 0x04000D28 RID: 3368
	private const int ADDED_CELLS = 6;
}
