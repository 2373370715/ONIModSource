using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropGravitasFirstAidKitConfig : IEntityConfig
{
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public GameObject CreatePrefab()
	{
		GameObject obj = EntityTemplates.CreatePlacedEntity("PropGravitasFirstAidKit", STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.NAME, STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.DESC, 50f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("gravitas_first_aid_kit_kanim"), initialAnim: "off", sceneLayer: Grid.SceneLayer.Building, width: 1, height: 1, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Gravitas });
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite);
		component.Temperature = 294.15f;
		Workable workable = obj.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = obj.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		obj.AddOrGet<Demolishable>();
		return obj;
	}

	public static string[][] GetLockerBaseContents()
	{
		string text = (DlcManager.FeatureRadiationEnabled() ? "BasicRadPill" : "IntermediateCure");
		return new string[2][]
		{
			new string[3] { "BasicCure", "BasicCure", "BasicCure" },
			new string[2] { text, text }
		};
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = GetLockerBaseContents();
		component.ChooseContents();
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
