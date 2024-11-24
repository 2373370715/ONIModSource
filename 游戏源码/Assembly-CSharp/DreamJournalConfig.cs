using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class DreamJournalConfig : IEntityConfig
{
	public static Tag ID = new Tag("DreamJournal");

	public const float MASS = 1f;

	public const int FABRICATION_TIME_SECONDS = 300;

	private const string ANIM_FILE = "dream_journal_kanim";

	private const string INITIAL_ANIM = "object";

	public const int MAX_STACK_SIZE = 25;

	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dream_journal_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(ID.Name, ITEMS.DREAMJOURNAL.NAME, ITEMS.DREAMJOURNAL.DESC, 1f, unitMass: true, anim, "object", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, isPickupable: true, 0, SimHashes.Creature, new List<Tag> { GameTags.StoryTraitResource });
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 25f;
		return gameObject;
	}
}
