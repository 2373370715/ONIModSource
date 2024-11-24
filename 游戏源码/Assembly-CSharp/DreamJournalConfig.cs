using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000397 RID: 919
public class DreamJournalConfig : IEntityConfig
{
	// Token: 0x06000F1C RID: 3868 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0017BDF0 File Offset: 0x00179FF0
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dream_journal_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DreamJournalConfig.ID.Name, ITEMS.DREAMJOURNAL.NAME, ITEMS.DREAMJOURNAL.DESC, 1f, true, anim, "object", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.StoryTraitResource
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 25f;
		return gameObject;
	}

	// Token: 0x04000AE8 RID: 2792
	public static Tag ID = new Tag("DreamJournal");

	// Token: 0x04000AE9 RID: 2793
	public const float MASS = 1f;

	// Token: 0x04000AEA RID: 2794
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x04000AEB RID: 2795
	private const string ANIM_FILE = "dream_journal_kanim";

	// Token: 0x04000AEC RID: 2796
	private const string INITIAL_ANIM = "object";

	// Token: 0x04000AED RID: 2797
	public const int MAX_STACK_SIZE = 25;
}
