using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200024E RID: 590
[EntityConfigOrder(2)]
public class LightBugPurpleBabyConfig : IEntityConfig
{
	// Token: 0x0600085D RID: 2141 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x000A9FCD File Offset: 0x000A81CD
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurpleBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPurple", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400063F RID: 1599
	public const string ID = "LightBugPurpleBaby";
}
