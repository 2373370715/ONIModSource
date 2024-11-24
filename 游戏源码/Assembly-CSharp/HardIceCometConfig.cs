using STRINGS;
using UnityEngine;

public class HardIceCometConfig : IEntityConfig
{
	public static readonly string ID = "HardIceComet";

	private const SimHashes element = SimHashes.CrushedIce;

	private const int ADDED_CELLS = 6;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY.Append("DLC2_ID");
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ID, UI.SPACEDESTINATIONS.COMETS.HARDICECOMET.NAME);
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
		primaryElement.SetElement(SimHashes.CrushedIce);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kBatchedAnimController.AnimFiles = new KAnimFile[1] { Assets.GetAnim("meteor_ice_kanim") };
		kBatchedAnimController.isMovable = true;
		kBatchedAnimController.initialAnim = "fall_loop";
		kBatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}

	public void OnPrefabInit(GameObject go)
	{
	}

	public void OnSpawn(GameObject go)
	{
	}
}
