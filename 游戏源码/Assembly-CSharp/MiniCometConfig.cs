using STRINGS;
using UnityEngine;

public class MiniCometConfig : IEntityConfig
{
	public static readonly string ID = "MiniComet";

	private const SimHashes element = SimHashes.Regolith;

	private const int ADDED_CELLS = 6;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ID, UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		MiniComet miniComet = gameObject.AddOrGet<MiniComet>();
		_ = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues;
		miniComet.impactSound = "MeteorDamage_Rock";
		miniComet.flyingSoundID = 2;
		miniComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		gameObject.AddOrGet<PrimaryElement>();
		KBatchedAnimController kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kBatchedAnimController.AnimFiles = new KAnimFile[1] { Assets.GetAnim("meteor_sand_kanim") };
		kBatchedAnimController.isMovable = true;
		kBatchedAnimController.initialAnim = "fall_loop";
		kBatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		return gameObject;
	}

	public void OnPrefabInit(GameObject go)
	{
	}

	public void OnSpawn(GameObject go)
	{
	}
}
