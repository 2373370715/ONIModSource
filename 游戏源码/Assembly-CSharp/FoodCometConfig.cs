using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class FoodCometConfig : IEntityConfig
{
	// Token: 0x06001330 RID: 4912 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x0018CFB4 File Offset: 0x0018B1B4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(FoodCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.FOODCOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(0.2f, 0.5f);
		comet.temperatureRange = new Vector2(298.15f, 303.15f);
		comet.entityDamage = 0;
		comet.totalTileDamage = 0f;
		comet.splashRadius = 0;
		comet.impactSound = "Meteor_dust_heavy_Impact";
		comet.flyingSoundID = 0;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.canHitDuplicants = true;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Creature, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		comet.EXHAUST_ELEMENT = SimHashes.Void;
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0018D120 File Offset: 0x0018B320
	public void OnPrefabInit(GameObject go)
	{
		Comet component = go.GetComponent<Comet>();
		component.OnImpact = (System.Action)Delegate.Combine(component.OnImpact, new System.Action(delegate()
		{
			int i = 10;
			while (i > 0)
			{
				i--;
				Vector3 vector = go.transform.position + new Vector3((float)UnityEngine.Random.Range(-2, 3), (float)UnityEngine.Random.Range(-2, 3), 0f);
				if (!Grid.Solid[Grid.PosToCell(vector)])
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("FoodSplat"), vector);
					gameObject.SetActive(true);
					gameObject.transform.Rotate(0f, 0f, (float)UnityEngine.Random.Range(-90, 90));
					i = 0;
				}
			}
		}));
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D1B RID: 3355
	public static string ID = "FoodComet";
}
