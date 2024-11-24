using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class MiniCometConfig : IEntityConfig
{
	// Token: 0x06001410 RID: 5136 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x0018FD54 File Offset: 0x0018DF54
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MiniCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.MINICOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		MiniComet miniComet = gameObject.AddOrGet<MiniComet>();
		Sim.PhysicsData defaultValues = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues;
		miniComet.impactSound = "MeteorDamage_Rock";
		miniComet.flyingSoundID = 2;
		miniComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		gameObject.AddOrGet<PrimaryElement>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.HideFromSpawnTool);
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		return gameObject;
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D94 RID: 3476
	public static readonly string ID = "MiniComet";

	// Token: 0x04000D95 RID: 3477
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000D96 RID: 3478
	private const int ADDED_CELLS = 6;
}
