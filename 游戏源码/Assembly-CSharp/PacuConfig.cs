using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200025E RID: 606
[EntityConfigOrder(1)]
public class PacuConfig : IEntityConfig
{
	// Token: 0x060008BB RID: 2235 RVA: 0x00163AF8 File Offset: 0x00161CF8
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		return EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, null, 273.15f, 333.15f, 253.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00163B38 File Offset: 0x00161D38
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(PacuConfig.CreatePacu("Pacu", CREATURES.SPECIES.PACU.NAME, CREATURES.SPECIES.PACU.DESC, "pacu_kanim", false), "PacuEgg", CREATURES.SPECIES.PACU.EGG_NAME, CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_BASE, this.GetDlcIds(), 500, false, true, false, 0.75f, false);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x000AA2B7 File Offset: 0x000A84B7
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<LoopingSounds>();
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000688 RID: 1672
	public const string ID = "Pacu";

	// Token: 0x04000689 RID: 1673
	public const string BASE_TRAIT_ID = "PacuBaseTrait";

	// Token: 0x0400068A RID: 1674
	public const string EGG_ID = "PacuEgg";

	// Token: 0x0400068B RID: 1675
	public const int EGG_SORT_ORDER = 500;
}
