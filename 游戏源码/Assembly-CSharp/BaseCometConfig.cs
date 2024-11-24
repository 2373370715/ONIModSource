using UnityEngine;

public static class BaseCometConfig
{
	public static GameObject BaseComet(string id, string name, string animName, SimHashes primaryElement, Vector2 massRange, Vector2 temperatureRange, string impactSound = "Meteor_Large_Impact", int flyingSoundID = 1, SimHashes exhaustElement = SimHashes.CarbonDioxide, SpawnFXHashes explosionEffect = SpawnFXHashes.None, float size = 1f)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, name);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = massRange;
		comet.temperatureRange = temperatureRange;
		comet.explosionTemperatureRange = comet.temperatureRange;
		comet.impactSound = impactSound;
		comet.flyingSoundID = flyingSoundID;
		comet.EXHAUST_ELEMENT = exhaustElement;
		comet.explosionEffectHash = explosionEffect;
		PrimaryElement primaryElement2 = gameObject.AddOrGet<PrimaryElement>();
		primaryElement2.SetElement(primaryElement);
		primaryElement2.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kBatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kBatchedAnimController.AnimFiles = new KAnimFile[1] { Assets.GetAnim(animName) };
		kBatchedAnimController.isMovable = true;
		kBatchedAnimController.initialAnim = "fall_loop";
		kBatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kBatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(size, size, 1f);
		gameObject.AddTag(GameTags.Comet);
		return gameObject;
	}
}
