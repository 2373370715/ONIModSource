using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropClothesHanger : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity("PropClothesHanger", STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.NAME, STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.DESC, 50f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("unlock_clothing_kanim"), initialAnim: "on", sceneLayer: Grid.SceneLayer.Building, width: 1, height: 2, element: SimHashes.Creature, additionalTags: new List<Tag>
		{
			GameTags.Gravitas,
			GameTags.RoomProberBuilding
		});
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Cinnabar);
		component.Temperature = 294.15f;
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.dropOnDeconstruct = true;
		gameObject.AddOrGet<Deconstructable>().audioSize = "small";
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[1][] { new string[1] { "Warm_Vest" } };
		component.ChooseContents();
	}

	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Deconstructable>().SetWorkTime(5f);
	}
}
