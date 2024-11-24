using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ProExoSetLockerConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("PropExoSetLocker", STRINGS.BUILDINGS.PREFABS.PROPEXOSETLOCKER.NAME, STRINGS.BUILDINGS.PREFABS.PROPEXOSETLOCKER.DESC, 100f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("zipper_locker_kanim"), initialAnim: "on", sceneLayer: Grid.SceneLayer.Building, width: 1, height: 2, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_zipper_locker_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.numDataBanks = new int[2] { 1, 4 };
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[2][]
		{
			new string[1] { "Warm_Vest" },
			new string[1] { "Funky_Vest" }
		};
		component.ChooseContents();
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
