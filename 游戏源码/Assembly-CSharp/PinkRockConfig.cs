using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PinkRockConfig : IEntityConfig
{
	public string ID = "PinkRock";

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(ID, STRINGS.CREATURES.SPECIES.PINKROCK.NAME, STRINGS.CREATURES.SPECIES.PINKROCK.DESC, 25f, decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0, noise: NOISE_POLLUTION.NOISY.TIER0, anim: Assets.GetAnim("pinkrock_kanim"), initialAnim: "idle", sceneLayer: Grid.SceneLayer.Building, width: 1, height: 1, element: SimHashes.Creature, additionalTags: new List<Tag> { GameTags.Experimental });
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium);
		component.Temperature = 235.15f;
		gameObject.AddOrGet<Carvable>().dropItemPrefabId = "PinkRockCarved";
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1] { ObjectLayer.Building };
		Light2D light2D = gameObject.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
		light2D.Color = LIGHT2D.PINKROCK_COLOR;
		light2D.Range = 2f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
		light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
		light2D.shape = LightShape.Circle;
		light2D.drawOverlay = true;
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}
}
